
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UtilityService.Models;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UtilityService.Utils
{
    public class SubtitleBlock
    {
        public string Timestamp { get; set; } = "";

        public string Text { get; set; } = "";

        public int CharCount { get; set; } = 0;
    }

    public static class Translator
    {
        private static readonly string apiURL = "https://api.cognitive.microsofttranslator.com/";

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<AzureLanguagesGetModel?> GetTranslationLanguages()
        {
            var requestURL = apiURL + "languages?api-version=3.0&scope=translation";

            try
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestURL),
                    Headers =
                    {
                        { "Accept-Language", "en" }
                    }
                };

                var response = await httpClient.SendAsync(httpRequestMessage);
                var stream = await response.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<AzureLanguagesGetModel>(stream);

                return asObj;
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return default;
            }
        }

        public static async Task<string?> Translate(string subtitlePath, TranslationPostModel model, string webRootPath, string apiKey)
        {
            var savePath = ResolvePath(model.Release, model.LanguageFrom, model.LanguageTo, webRootPath);

            try
            {
                await using var stream = new FileStream(savePath, FileMode.Append);
                await using var translatedFile = new StreamWriter(stream, Encoding.UTF8);

                var untranslatedBlocks = await ParseSubtitleFile(subtitlePath);

                if (untranslatedBlocks == null)
                    return null;

                var translateQueue = untranslatedBlocks.Chunk(500).ToList();
                var lastBlockIndex = 1;

                for (var i = 0; i < translateQueue.Count; i++)
                {
                    var blockArray = translateQueue[i];
                    var textToTranslate = blockArray.Select(x => x.Text).ToArray();
                    var translatedText = await GetTranslation(textToTranslate, model.LanguageFrom, model.LanguageTo, apiKey);

                    if (translatedText == null)
                    {
                        LoggerManager.GetInstance().LogError($"Failed to parse subtitle {model.Release}");
                        return null;
                    }

                    for (var j = 0; j < blockArray.Length; j++)
                    {
                        blockArray[i].Text = translatedText[j].Translations[0].Text;

                        await translatedFile.WriteLineAsync($"{lastBlockIndex}");
                        await translatedFile.WriteLineAsync(blockArray[i].Timestamp);
                        await translatedFile.WriteLineAsync(blockArray[i].Text);
                        await translatedFile.WriteLineAsync("");
                        lastBlockIndex++;
                    }
                }

                if (model.FileID != null)
                {
                    File.Delete(subtitlePath);
                }

                return Path.Combine("translations", model.LanguageTo, model.Release) + ".srt";
            }
            catch(Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return default;
            }
        }

        private static string ResolvePath(string newSubtitleName, string languageFrom, string languageTo, string webRootPath)
        {
            var languageFolder = Path.Combine(webRootPath, "translations", languageTo);

            if (!Directory.Exists(languageFolder))
            {
                Directory.CreateDirectory(languageFolder);
            }

            return Path.Combine(languageFolder, newSubtitleName + ".srt");
        }

        private static async Task<List<SubtitleBlock>?> ParseSubtitleFile(string subtitlePath)
        {
            try
            {
                using var file = new StreamReader(subtitlePath, Encoding.UTF8);
                var blocks = new List<SubtitleBlock>();
                var counter = -1;

                while (!file.EndOfStream)
                {
                    var line = await file.ReadLineAsync();
                    var timestamp = false;

                    if (line == "")
                        continue;

                    if (line == null)
                    {
                        LoggerManager.GetInstance().LogError("Line was null when reading subtitle file");
                        return null;
                    }

                    if (!Regex.IsMatch(line, "\\D"))
                        continue;

                    // Encountered timestamp
                    if (Regex.IsMatch(line, "^\\d{2}:\\d{2}:\\d{2},\\d{3} --> \\d{2}:\\d{2}:\\d{2},\\d{3}"))
                    {
                        timestamp = true;
                        blocks.Add(new SubtitleBlock() { Timestamp = line });
                        counter++;
                    }

                    if (!timestamp)
                    {
                        if (blocks[counter].Text == "")
                        {
                            blocks[counter].Text = line;
                            blocks[counter].CharCount = line.Length;
                        }
                        else
                        {
                            blocks[counter].Text += $" {line}";
                            blocks[counter].CharCount += line.Length + 1; // Accounting for the whitespace
                        }
                    }
                }

                return blocks;
            }
            catch(Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return default;
            }
        }

        private static async Task<List<AzureTranslationGetModel>?> GetTranslation(string[] texts, string languageFrom, string languageTo, string apiKey)
        {
            var requestURL = apiURL + $"translate?api-version=3.0&from={languageFrom}&to={languageTo}";
            var body = new List<object>();

            foreach (var text in texts)
            {
                body.Add(new { Text = text });
            }

            var requestBody = JsonConvert.SerializeObject(body);

            try
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestURL),
                    Headers =
                    {
                        { "Ocp-Apim-Subscription-Key", apiKey },
                        { "Ocp-Apim-Subscription-Region", "northeurope" }
                    },
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(httpRequestMessage);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var asObj = await JsonSerializer.DeserializeAsync<AzureTranslationGetModel[]>(stream);

                    if (asObj != null)
                    {
                        return asObj.ToList();
                    }
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
            catch (JsonException e)
            {
                return null;
            }

            return null;
        }
    }
}

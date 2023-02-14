using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UtilityService.Models;

namespace UtilityService.Utils
{
    public static class OpenSubtitlesFetcher
    {
        private static readonly string apiURL = "https://api.opensubtitles.com/api/v1";

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<OpenSubtitlesModel?> FetchSubtitles(string imdbID, string language, int? seasonNumber, int? episodeNumber, string apiKey)
        {
            imdbID = imdbID.TrimStart('t');
            imdbID = imdbID.TrimStart('0');

            var requestURL = apiURL + $"/subtitles?imdb_id={imdbID}&languages={language}";

            if (seasonNumber != null && episodeNumber != null)
            {
                requestURL += $"&episode_number={episodeNumber}&season_number={seasonNumber}";
            }

            try
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestURL),
                    Headers =
                    {
                        { "Api-Key", apiKey },
                    },
                };

                var result = await httpClient.SendAsync(httpRequestMessage);

                if (result.StatusCode == HttpStatusCode.NoContent || result.StatusCode == HttpStatusCode.NotFound)
                {
                    LoggerManager.GetInstance().LogError($"Failed to fetch {imdbID} from OpenSubtitles with status code {result.StatusCode}");
                    return default;
                }

                var body = await result.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<OpenSubtitlesModel>(body);

                return asObj;

            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
            }

            return default;
        }

        public static async Task<OpenSubtitlesDownloadModel?> GetDownloadLink(string fileID, string apiKey)
        {
            var requestURL = apiURL + $"/download";

            try
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestURL),
                    Headers =
                    {
                        { "Api-Key", apiKey },
                        { "Accept", "*/*"}
                    },
                    Content = JsonContent.Create(new { file_id = int.Parse(fileID) })
                };

                var result = await httpClient.SendAsync(httpRequestMessage);

                if (result.StatusCode == HttpStatusCode.NoContent || result.StatusCode == HttpStatusCode.NotFound)
                {
                    LoggerManager.GetInstance().LogError($"Failed to get download link for file {fileID} from OpenSubtitles with status code {result.StatusCode}");
                    return default;
                }

                var body = await result.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<OpenSubtitlesDownloadModel>(body);

                if (asObj != null)
                    if (asObj.Link != null)
                        return asObj;
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
            }

            return null;
        }
    }
}

using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using UtilityService.Models;
using UtilityService.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public TranslationController(CaptioneerDBContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        // GET: api/Translation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Language>>> GetSupportedLanguages()
        {
            var azureLanguages = await Translator.GetTranslationLanguages();

            if (azureLanguages == null)
            {
                LoggerManager.GetInstance().LogError("Could not fetch languages that can be translated from Azure");
                return NotFound("Could not fetch languages that can be translated from Azure");
            }

            var languages = new List<Language>();

            foreach (var language in azureLanguages.TranslationLanguages)
            {
                var dbLanguage = new Language();

                // Microsoft decided to not use standard language codes for Chinese, so specific handling here must be done
                // Also, Serbian is defaulted to cyrillic in our database, but Azure regards both cyrillic and latin as valid so it must be explicitly handled here as well
                if (language.Value.Name == "Chinese Simplified")
                    dbLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == "zh-cn");
                else if (language.Value.Name == "Chinese Traditional")
                    dbLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == "zh-tw");
                else if (language.Value.Name == "Serbian (Cyrillic)")
                    dbLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == "sr");
                else
                    dbLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == language.Key);

                if (dbLanguage != null && !languages.Contains(dbLanguage))
                {
                    languages.Add(dbLanguage);
                }
            }

            return Ok(languages);
        }

        // POST: api/Translation
        [HttpPost]
        public async Task<ActionResult> GetTranslation(TranslationPostModel model)
        {
            var translation = await _context.Translations.FirstOrDefaultAsync(t => t.Language.LanguageCode == model.LanguageTo && t.Release == model.Release);

            if (translation != null)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, translation.SubtitlePath);
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                return new FileContentResult(fileBytes, new MediaTypeHeaderValue("application/octet"));
            }

            var newTranslation = new Translation();
            var microsoftTranslatorKey = _configuration["ApiKeys:MicrosoftTranslator"];

            if (model.FileID != null)
            {
                var openSubtitlesKey = _configuration["ApiKeys:OpenSubtitlesKey"];
                var downloadModel = await OpenSubtitlesFetcher.GetDownloadLink(model.FileID, openSubtitlesKey);

                if (downloadModel != null)
                {
                    var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "translations", "temp", downloadModel.FileName!);

                    if (!await FileDownloader.Download(downloadModel.Link!, savePath))
                    {
                        LoggerManager.GetInstance().LogError("Could not download subtitle from OpenSubtitles");
                        return NotFound("Could not download subtitle from OpenSubtitles");
                    }

                    var translatedFile = await Translator.Translate(savePath, model, _webHostEnvironment.WebRootPath, microsoftTranslatorKey);

                    if (translatedFile == null)
                    {
                        LoggerManager.GetInstance().LogError("Could not translate subtitle");
                        return NotFound("Could not translate subtitle");
                    }

                    newTranslation.SubtitlePath = translatedFile;
                }
            }
            else
            {
                // To do for TV show subtitles as well when upload has been implemented
                var subtitleTvShow = await _context.SubtitleTVShows.FirstOrDefaultAsync(sTv => sTv.Release == model.Release);
                var subtitleMovie = await _context.SubtitleMovies.FirstOrDefaultAsync(sM => sM.Release == model.Release);

                var subtitlePath = Path.Combine(_webHostEnvironment.WebRootPath, (subtitleMovie?.SubtitlePath ?? subtitleTvShow?.SubtitlePath) ?? string.Empty);
                var translatedFile = await Translator.Translate(subtitlePath, model, _webHostEnvironment.WebRootPath, microsoftTranslatorKey);

                if (translatedFile == null)
                {
                    LoggerManager.GetInstance().LogError("Could not translate subtitle");
                    return NotFound("Could not translate subtitle!");
                }

                newTranslation.SubtitlePath = translatedFile;
            }

            var language = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == model.LanguageTo);

            if (language == null)
            {
                LoggerManager.GetInstance().LogError("Could not find the translation language");
                return NotFound("Could not find the translation language");
            }

            newTranslation.Language = language;
            newTranslation.Release = model.Release;

            await _context.Translations.AddAsync(newTranslation);
            await _context.SaveChangesAsync();

            var newTranslationBytes = await System.IO.File.ReadAllBytesAsync(Path.Combine(_webHostEnvironment.WebRootPath, newTranslation.SubtitlePath));
            LoggerManager.GetInstance().LogInfo($"Served {newTranslation.SubtitlePath} after translation");

            return new FileContentResult(newTranslationBytes, new MediaTypeHeaderValue("application/octet"));
        }
    }
}

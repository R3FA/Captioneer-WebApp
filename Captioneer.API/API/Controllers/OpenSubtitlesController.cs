using Microsoft.AspNetCore.Mvc;
using UtilityService.Models;
using UtilityService.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenSubtitlesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OpenSubtitlesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/OpenSubtitles/tt0944947/en
        // GET: api/OpenSubtitles/tt0944947/en?seasonNumber=1&episodeNumber=1
        [HttpGet("{imdbID}/{language}")]
        public async Task<ActionResult<IEnumerable<OpenSubtitlesViewModel>?>> Get(string imdbID, string language, int? seasonNumber = null, int? episodeNumber = null)
        {
            var apiKey = _configuration["ApiKeys:OpenSubtitlesKey"];
            var model = await OpenSubtitlesFetcher.FetchSubtitles(imdbID, language, seasonNumber, episodeNumber, apiKey);

            if (model == null)
            {
                var errorMsg = seasonNumber == null ? $"Could not find subtitles for {imdbID} on OpenSubtitles" 
                    : $"Could not find subtitles for {imdbID}, season {seasonNumber} and episode {episodeNumber} on OpenSubtitles";

                LoggerManager.GetInstance().LogError("Status 404: " + errorMsg);
                return NotFound(errorMsg);
            }

            var viewModels = new List<OpenSubtitlesViewModel>();

            foreach (var data in model.Data)
            {
                viewModels.Add(new OpenSubtitlesViewModel()
                {
                    FileId = data.Attributes.Files.First().FileId,
                    FileName = data.Attributes.Files.First().FileName,
                    Fps = data.Attributes.Fps,
                    Language = data.Attributes.Language,
                    Release = data.Attributes.Release,
                    UploadDate = data.Attributes.UploadDate,
                    Uploader = data.Attributes.Uploader.Name
                });
            }

            return Ok(viewModels);
        }

        // POST: api/OpenSubtitles/OpenSubtitlesFileID
        [HttpPost("{fileID}")]
        public async Task<ActionResult<OpenSubtitlesDownloadModel?>> Download(string fileID)
        {
            var apiKey = _configuration["ApiKeys:OpenSubtitlesKey"];
            var link = await OpenSubtitlesFetcher.GetDownloadLink(fileID, apiKey);

            if (link == null)
            {
                LoggerManager.GetInstance().LogError($"Could not fetch download link for OpenSubtitles file {fileID}");
                return NotFound($"Could not fetch download link for OpenSubtitles file {fileID}");
            }

            return Ok(link);
        }
    }
}

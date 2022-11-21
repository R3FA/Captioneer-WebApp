using Captioneer.API.Data.OpenSubtitles;
using Captioneer.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenSubtitlesController : ControllerBase
    {
        // GET: api/OpenSubtitles/tt0944947/en
        // GET: api/OpenSubtitles/tt0944947/en?seasonNumber=1&episodeNumber=1
        [HttpGet("{imdbID}/{language}")]
        public async Task<ActionResult<IEnumerable<OpenSubtitlesViewModel>?>> Get(string imdbID, string language, int? seasonNumber = null, int? episodeNumber = null)
        {
            var model = await OpenSubtitlesFetcher.FetchSubtitles(imdbID, language, seasonNumber, episodeNumber);

            if (model == null)
            {
                return NotFound();
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

        [HttpPost("{fileID}")]
        public async Task<ActionResult<OpenSubtitlesDownloadModel?>> Download(string fileID)
        {
            var link = await OpenSubtitlesFetcher.GetDownloadLink(fileID);

            if (link == null)
                return NotFound();

            return Ok(link);
        }
    }
}

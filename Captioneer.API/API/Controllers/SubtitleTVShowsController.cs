using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using Microsoft.Extensions.Hosting;
using UtilityService.Utils;
using API.DTO;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleTVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IContentTypeProvider _contentTypeProvider;

        public SubtitleTVShowsController(CaptioneerDBContext context, IWebHostEnvironment hostEnvironment, IContentTypeProvider contentTypeProvider)
        {
            _context = context;
            _hostEnvironment = hostEnvironment; 
            _contentTypeProvider = contentTypeProvider;
        }

        // GET: api/SubtitleTVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubtitleTVShow>>> GetSubtitleTVShows()
        {
            return await _context.SubtitleTVShows.Include(s=>s.Episode).Include(s=>s.Episode.Season).Include(s=>s.Episode.Season.TVShow).Include(s=>s.Language).ToListAsync();
        }

        // GET: api/SubtitleTVShows/tt0944947/1/1
        [HttpGet("{seasonNumber}/{episodeNumber}")]
        public async Task<ActionResult<SubtitleTVShow?>> GetSubtitleTVShow(int movieId, string languageCode, int seasonNumber, int episodeNumber)
        {
            var subTVList = await _context.SubtitleTVShows.Include(s=>s.Episode.Season.TVShow).Include(s => s.Language).Where(s => (s.Episode.Season.TVShow.ID == movieId) && (s.Language.LanguageCode == languageCode)).Where(s=>(s.Episode.EpisodeNumber==episodeNumber)&&(s.Episode.Season.SeasonNumber==seasonNumber)).ToListAsync();
            var subtitleUserList = _context.SubtitleUsers.Include(s => s.User).Include(s => s.SubtitleMovie).Include(s => s.SubtitleTVShow);
            List<SubtitleUser> subtitleUsers = new List<SubtitleUser>();
            foreach (var subMovie in subTVList)
            {
                foreach (var subUser in subtitleUserList)
                {
                    if (subUser.SubtitleTVShow != null)
                    {
                        if (subUser.SubtitleTVShow.ID == subMovie.ID)
                        {
                            subtitleUsers.Add(subUser);
                        }
                    }
                }
            }
            List<SubtitleViewModel> filteredList = new List<SubtitleViewModel>();
            foreach (var subTitle in subTVList)
            {
                filteredList.Add(new SubtitleViewModel
                {
                    uploader = "aaaaaaa",
                    fps = 0,
                    release = "",
                    ratingValue = subTitle.RatingValue,
                    ratingCount = subTitle.RatingCount,
                    downloadCount = subTitle.DownloadCount,
                    subMovieID = subTitle.ID
                }
                );
            };
            for (int i = 0; i < filteredList.Count(); i++)
            {
                for (int j = 0; j < subtitleUsers.Count(); j++)
                {
                    if (subtitleUsers[j].SubtitleTVShow != null)
                    {
                        if (subtitleUsers[j].SubtitleTVShow.ID == filteredList[i].subMovieID)
                        {
                            filteredList[i].uploader = subtitleUsers[j].User.Username;
                        }
                    }
                }
            };
            return Ok(filteredList);
        }

        [HttpGet]
        [Route("api/download")]
        public async Task<IActionResult> Download(int subMovieID, string userEmail)
        {
            var subtitleMovie = _context.SubtitleTVShows.FirstOrDefault(s => s.ID == subMovieID);
            var fileName = subtitleMovie.SubtitlePath;
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find user with email {userEmail}");
                return NotFound($"Could not find user with email {userEmail}");
            }

            user.SubtitleDownload++;
            subtitleMovie.DownloadCount++;
            await _context.SaveChangesAsync();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var contentType = _contentTypeProvider.TryGetContentType(fileName, out string mimeType) ? mimeType : "application/octet-stream";

                    return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Unable to download file because it is being used by another process.");
            }
        }

        // POST api/SubtitleMovie
        [HttpPost]
        public async Task<ActionResult> Post(int tvShowID, int seasonNumber, int episodeNumber, string languageCode, int? frameRate, string? release, string userEmail, IFormFile file)
        {
            var movie = await _context.TVShows.FirstOrDefaultAsync(m => m.ID == tvShowID);

            if (movie == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find movie with ID {tvShowID}");
                return BadRequest($"Could not find movie with ID {tvShowID}");
            }

            var language = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == languageCode);
            string path;


            var uploads = Path.Combine(_hostEnvironment.WebRootPath, "subtitleTVShowUploads");

            if (file.Length > 0)
            {
                int dotIndex = file.FileName.IndexOf('.');
                string name = file.FileName;

                if (dotIndex >= 0)
                    name = file.FileName.Substring(0, dotIndex);

                var fileName = $"{name}{DateTime.Now}";
                fileName = fileName.Replace("/", "");
                fileName = fileName.Replace(":", "");
                fileName = fileName.Replace(" ", "");
                fileName += ".srt";

                path = Path.Combine(uploads, fileName);
                await Upload(file, path);

                int currentId = await _context.SubtitleMovies.CountAsync();

                int frameRateSet;
                if (frameRate == null)
                    frameRateSet = 0;
                else
                    frameRateSet = (int)frameRate;

                string releaseSet;
                if (release == null)
                    releaseSet = "";
                else
                    releaseSet = release;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    LoggerManager.GetInstance().LogError($"Could not find user with email {userEmail}");
                    return NotFound($"Could not find user with email {userEmail}");
                }

                user.SubtitleUpload++;

                Season season = new Season();
                season.TVShow = movie;
                season.SeasonNumber = seasonNumber;
                Episode episode = new Episode();
                episode.EpisodeNumber = episodeNumber;
                episode.Name = "";
                episode.Season = season;
                SubtitleTVShow subtitleTVShow = new SubtitleTVShow();
                subtitleTVShow.Episode = episode;
                subtitleTVShow.Language = language;
                subtitleTVShow.DownloadCount = 0;
                subtitleTVShow.SubtitlePath = Path.Combine("subtitleTVShowUploads", fileName);
                subtitleTVShow.RatingValue = 0;
                subtitleTVShow.RatingCount = 0;
                subtitleTVShow.FrameRate = frameRateSet;
                subtitleTVShow.Release = release;

                await _context.SubtitleTVShows.AddAsync(subtitleTVShow);
                await _context.SaveChangesAsync();

                int currentUserId = await _context.SubtitleTVShows.CountAsync();
                SubtitleUser subtitleUser = new SubtitleUser();
                subtitleUser.User= user;
                subtitleUser.SubtitleTVShow= subtitleTVShow;
                subtitleUser.RatingValue = 0;
                subtitleUser.RatingCount = 0;
                await _context.SubtitleUsers.AddAsync(subtitleUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int subMovieID, string userEmail, int userRatingValue)
        {
            var subtitleTVShow = _context.SubtitleTVShows.FirstOrDefault(s => s.ID == subMovieID);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find user with email {userEmail}");
                return NotFound($"Could not find user with email {userEmail}");
            }
            if (subtitleTVShow.RatingCount == 0)
            {
                subtitleTVShow.RatingValue = userRatingValue;
                subtitleTVShow.RatingCount++;
            }
            else
            {
                subtitleTVShow.RatingValue = (subtitleTVShow.RatingValue * subtitleTVShow.RatingCount + userRatingValue) / (subtitleTVShow.RatingCount + 1);
                subtitleTVShow.RatingValue = Math.Round(subtitleTVShow.RatingValue, 2);
                subtitleTVShow.RatingCount++;
            }
            await _context.SaveChangesAsync();
            return Ok(subtitleTVShow.RatingValue);
        }

        // DELETE api/<SubtitleTVShowController>/5
        [HttpDelete("{subtitleId}")]
        public async Task<IActionResult> Delete(int subtitleId, string userUploader, int episodeNumber, int seasonNumber)
        {
            var dbTVShowSubtitle = await this._context.SubtitleTVShows.FirstOrDefaultAsync(x => x.ID == subtitleId);
            if (dbTVShowSubtitle != null)
            {
                var dbTVShowUploader = await this._context.SubtitleUsers.Include(x => x.User).Include(x => x.SubtitleTVShow).FirstOrDefaultAsync(x => x.User.Username == userUploader && x.SubtitleTVShow.ID == subtitleId);
                var dbTVShowSubtitleComments = await this._context.Comments.Where(x => x.SubtitleTVShow.ID == subtitleId && x.SubtitleTVShow.Episode.EpisodeNumber == episodeNumber && x.SubtitleTVShow.Episode.Season.SeasonNumber == seasonNumber).ToListAsync();
                if (dbTVShowSubtitleComments.Count != 0)
                    this._context.Comments.RemoveRange(dbTVShowSubtitleComments);
                this._context.SubtitleUsers.Remove(dbTVShowUploader);
                this._context.SubtitleTVShows.Remove(dbTVShowSubtitle);
                await this._context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }

        private static async Task Upload(IFormFile file, string path)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
            }
        }
    }
}

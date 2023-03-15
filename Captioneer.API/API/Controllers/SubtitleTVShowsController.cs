using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using Microsoft.Extensions.Hosting;
using UtilityService.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleTVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SubtitleTVShowsController(CaptioneerDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment; 
        }

        // GET: api/SubtitleTVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubtitleTVShow>>> GetSubtitleTVShows()
        {
            return await _context.SubtitleTVShows.Include(s=>s.Episode).Include(s=>s.Episode.Season).Include(s=>s.Episode.Season.TVShow).Include(s=>s.Language).ToListAsync();
        }

        // GET: api/SubtitleTVShows/tt0944947/1/1
        [HttpGet("{imdbID}/{seasonNumber}/{episodeNumber}")]
        public async Task<ActionResult<SubtitleTVShow?>> GetSubtitleTVShow(string imdbID, int seasonNumber, int episodeNumber)
        {
            return default(SubtitleTVShow);
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
                var fileName = $"{file.FileName}{DateTime.Now}";
                fileName = fileName.Replace("/", "");
                fileName = fileName.Replace(":", "");
                fileName = fileName.Replace(" ", "");

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

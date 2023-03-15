using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleMovieController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SubtitleMovieController(CaptioneerDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/SubtitleMovie
        [HttpGet]
        public async Task<ActionResult<List<SubtitleMovie>>> Get()
        {
            var subMovieList = await _context.SubtitleMovies.Include(s => s.Movie).Include(s => s.Language).ToListAsync();

            return Ok(subMovieList);
        }

        // POST api/SubtitleMovie
        [HttpPost]
        public async Task<ActionResult> Post(int movieId, string languageCode, int? frameRate, string? release, string userEmail, IFormFile file)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == movieId);

            if (movie == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find movie with ID {movieId}");
                return BadRequest($"Could not find movie with ID {movieId}");
            }

            var language = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageCode == languageCode);

            string path;

            var uploads = Path.Combine(_hostEnvironment.WebRootPath, "subtitleMovieUploads");

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

                SubtitleMovie subtitleMovie = new SubtitleMovie();
                subtitleMovie.Movie = movie;
                subtitleMovie.Language = language;
                subtitleMovie.DownloadCount = 0;
                subtitleMovie.SubtitlePath = Path.Combine("subtitleMovieUploads", fileName);
                subtitleMovie.RatingValue = 0;
                subtitleMovie.RatingCount = 0;
                subtitleMovie.FrameRate = frameRateSet;
                subtitleMovie.Release = releaseSet;
                await _context.SubtitleMovies.AddAsync(subtitleMovie);
                await _context.SaveChangesAsync();

                int currentUserId = await _context.SubtitleMovies.CountAsync();
                SubtitleUser subtitleUser = new SubtitleUser();
                subtitleUser.User = user;
                subtitleUser.SubtitleMovie = subtitleMovie;
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
            catch(Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
            }
        }

        // PUT api/<SubtitleMovieController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SubtitleMovieController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

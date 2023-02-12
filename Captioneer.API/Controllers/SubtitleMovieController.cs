using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Captioneer.API.Controllers
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

        // GET: api/<SubtitleMovieController>
        [HttpGet]
        public List<SubtitleMovie> Get()
        {
            //SubtitleMovie[] result = null;
            //result= await _context.SubtitleMovies.ToArrayAsync();
            List<SubtitleMovie> subMovieList= new List<SubtitleMovie>();
            return _context.SubtitleMovies.Include(s=>s.Movie)
                                   .Include(s=>s.Language).ToList();
        }

        // GET api/<SubtitleMovieController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SubtitleMovieController>
        [HttpPost]
        public async Task<ActionResult> Post(int movieId,string languageCode,int? frameRate,string? release,string userEmail,IFormFile file)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == movieId);
            if (movie == null)
                return BadRequest();
            var language=await _context.Languages.FirstOrDefaultAsync(l=>l.LanguageCode== languageCode);

            string path;

            var uploads = Path.Combine(_hostEnvironment.WebRootPath, "subtitleMovieUploads");

            if (file.Length > 0)
            {
                var fileName = $"{file.FileName}{DateTime.Now}";
                fileName = fileName.Replace("/", "");
                fileName = fileName.Replace(":", "");
                fileName = fileName.Replace(" ", "");
                
                path = Path.Combine(uploads,fileName);
                await Upload(file, path);

                int currentId = await _context.SubtitleMovies.CountAsync();

                int frameRateSet;
                if (frameRate==null)
                    frameRateSet= 0;
                else
                    frameRateSet = (int)frameRate;

                string releaseSet;
                if (release == null)
                    releaseSet = "";
                else
                    releaseSet = release;

                var user= await _context.Users.FirstOrDefaultAsync(u=>u.Email==userEmail);
                if (user == null)
                    return BadRequest();

                SubtitleMovie subtitleMovie = new SubtitleMovie
                {
                    ID= ++currentId,
                    Movie = movie,
                    Language = language,
                    DownloadCount = 0,
                    SubtitlePath = Path.Combine("subtitleMovieUploads", fileName),
                    RatingValue = 0,
                    RatingCount = 0,
                    FrameRate=frameRateSet, 
                    Release=releaseSet,
                };
                await _context.SubtitleMovies.AddAsync(subtitleMovie);
                await _context.SaveChangesAsync();

                int currentUserId = await _context.SubtitleMovies.CountAsync();
                SubtitleUser subtitleUser = new SubtitleUser
                {
                    ID = ++currentUserId,
                    User = user,
                    SubtitleMovie = subtitleMovie,
                    RatingValue = 0,
                    RatingCount = 0,
                };
                await _context.SubtitleUsers.AddAsync(subtitleUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
            }

        private static async Task Upload(IFormFile file, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
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

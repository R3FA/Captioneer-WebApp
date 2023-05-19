using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using Microsoft.AspNetCore.Http.Features;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleMovieController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IContentTypeProvider _contentTypeProvider;

        public SubtitleMovieController(CaptioneerDBContext context, IWebHostEnvironment hostEnvironment, IContentTypeProvider contentTypeProvider)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _contentTypeProvider = contentTypeProvider;
        }

        // GET: api/SubtitleMovie
        [HttpGet]
        public async Task<ActionResult> Get(int movieId, string languageCode)
        {
            var subMovieList = await _context.SubtitleMovies.Include(s => s.Movie).Include(s => s.Language).Where(s=>(s.Movie.ID==movieId)&&(s.Language.LanguageCode==languageCode)).ToListAsync();
            var subtitleUserList = _context.SubtitleUsers.Include(s => s.User).Include(s=>s.SubtitleMovie).Include(s=>s.SubtitleTVShow);
            List<SubtitleUser> subtitleUsers = new List<SubtitleUser>();
            foreach (var subMovie in subMovieList)
            {
                foreach (var subUser in subtitleUserList)
                {
                    if (subUser.SubtitleMovie!=null)
                    {
                        if (subUser.SubtitleMovie.ID==subMovie.ID)
                        {
                            subtitleUsers.Add(subUser);
                        }
                    }
                }
            }
            List<SubtitleViewModel> filteredList = new List<SubtitleViewModel>();
            foreach (var subTitle in subMovieList)
            {
                filteredList.Add(new SubtitleViewModel
                {
                    uploader = "aaaaaaaa",
                    fps = subTitle.FrameRate,
                    release = subTitle.Release,
                    ratingValue = subTitle.RatingValue,
                    ratingCount = subTitle.RatingCount,
                    downloadCount = subTitle.DownloadCount,
                    subMovieID = subTitle.ID
                }
                );
            };
            for (int i = 0; i < filteredList.Count(); i++)
            {
                for (int j= 0; j < subtitleUsers.Count(); j++)
                {
                    if (subtitleUsers[j].SubtitleMovie!=null)
                    {
                        if (subtitleUsers[j].SubtitleMovie.ID == filteredList[i].subMovieID)
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
        public async Task<IActionResult> Download(int subMovieID,string userEmail)
        {
            var subtitleMovie = _context.SubtitleMovies.FirstOrDefault(s => s.ID == subMovieID);
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


        // PUT api/<SubtitleMovieController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int subMovieID, string userEmail,int userRatingValue)
        {
            var subtitleMovie = _context.SubtitleMovies.FirstOrDefault(s => s.ID == subMovieID);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find user with email {userEmail}");
                return NotFound($"Could not find user with email {userEmail}");
            }
            if (subtitleMovie.RatingCount==0)
            {
                subtitleMovie.RatingValue = userRatingValue;
                subtitleMovie.RatingCount++;
            }
            else
            {
                subtitleMovie.RatingValue =(subtitleMovie.RatingValue*subtitleMovie.RatingCount+userRatingValue)/(subtitleMovie.RatingCount+1);
                subtitleMovie.RatingValue = Math.Round(subtitleMovie.RatingValue, 2);
                subtitleMovie.RatingCount++;
            }
            await _context.SaveChangesAsync();
            return Ok(subtitleMovie.RatingValue);
        }

        // DELETE api/<SubtitleMovieController>/5
        [HttpDelete("{subtitleId}")]
        public async Task<IActionResult> Delete(int subtitleId)
        {
            var dbMovieSubtitle = await this._context.SubtitleMovies.FirstOrDefaultAsync(x => x.ID == subtitleId);
            if (dbMovieSubtitle != null)
            {
                var dbMovieSubtitleComments = await this._context.Comments.Where(x => x.SubtitleMovie.ID == subtitleId).ToListAsync();
                this._context.Comments.RemoveRange(dbMovieSubtitleComments);
                this._context.SubtitleMovies.Remove(dbMovieSubtitle);
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
            catch(Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
            }
        }
    }
}

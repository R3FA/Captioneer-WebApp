using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtilityService.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMoviesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public UserMoviesController(CaptioneerDBContext context)
        {
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<MovieViewModel>>> GetUserMovie(string username)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            var dbUserMovies = await _context.UsersMovies.Where(uM => uM.UserID == dbUser.ID).ToListAsync();
            var favoriteMovies = new List<MovieViewModel>();

            if (dbUserMovies.Count == 0)
                return Ok(favoriteMovies);

            foreach (var dbUserMovie in dbUserMovies)
            {
                var dbMovie = await _context.Movies.FindAsync(dbUserMovie.MovieID);

                if (dbMovie == null)
                {
                    LoggerManager.GetInstance().LogError($"Could not find one or more favorite movies for user {username}");
                    return NotFound($"Could not find one or more favorite movies for user {username}");
                }

                favoriteMovies.Add(new MovieViewModel()
                {
                    Title = dbMovie.Title,
                    IMDBId = dbMovie.IMDBId,
                    Synopsis = dbMovie.Synopsis,
                    Year = dbMovie.Year,
                    Runtime = dbMovie.Runtime,
                    IMDBRatingValue = dbMovie.IMDBRatingValue,
                    IMDBRatingCount = dbMovie.IMDBRatingCount,
                    RottenTomatoesValue = dbMovie.RottenTomatoesValue,
                    MetacriticValue = dbMovie.MetacriticValue,
                    CoverArt = dbMovie.CoverArt
                });
            }

            return Ok(favoriteMovies);
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> PostUserMovie(string username, MovieViewModel movie)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == movie.Title && movie.Year == m.Year);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            if (dbMovie == null)
            {
                LoggerManager.GetInstance().LogError($"Movie {movie.Title} was not found");
                return NotFound($"Movie {movie.Title} was not found");
            }

            if (_context.UsersMovies.Any(uM => uM.UserID == dbUser.ID && uM.MovieID == dbMovie.ID))
            {
                LoggerManager.GetInstance().LogError($"Movie {movie.Title} has already been favorited by user {username}");
                return BadRequest($"Movie {movie.Title} has already been favorited by user {username}");
            }

            await _context.UsersMovies.AddAsync(new UserMovies()
            {
                UserID = dbUser.ID,
                MovieID = dbMovie.ID
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUserMovie(string username, MovieViewModel movie)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == movie.Title && movie.Year == m.Year);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            if (dbMovie == null)
            {
                LoggerManager.GetInstance().LogError($"Movie {movie.Title} was not found");
                return NotFound($"Movie {movie.Title} was not found");
            }

            var dbUserMovie = await _context.UsersMovies.FindAsync(dbUser.ID, dbMovie.ID);

            if (dbUserMovie == null)
            {
                LoggerManager.GetInstance().LogError($"Movie {movie.Title} is not favorited by {username} and cannot be deleted");
                return BadRequest($"Movie {movie.Title} is not favorited by {username} and cannot be deleted");
            }

            _context.UsersMovies.Remove(dbUserMovie);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

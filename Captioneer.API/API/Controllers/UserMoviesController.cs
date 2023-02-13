using Captioneer.API.Data;
using Captioneer.API.DTO;
using Captioneer.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Controllers
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
                return NotFound("User with the provided username was not found!");

            var dbUserMovies = await _context.UsersMovies.Where(uM => uM.UserID == dbUser.ID).ToListAsync();
            
            if (dbUserMovies.Count == 0)
                return Ok(new List<MovieViewModel>());

            var favoriteMovies = new List<MovieViewModel>();

            foreach (var dbUserMovie in dbUserMovies)
            {
                var dbMovie = await _context.Movies.FindAsync(dbUserMovie.MovieID);

                if (dbMovie == null )
                    return NotFound("Could not find one or more favorite movies!");

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
                return NotFound("User with the provided username was not found!");

            if (dbMovie == null)
                return NotFound("The provided movie was not found!");

            if (_context.UsersMovies.Any(uM => uM.UserID == dbUser.ID && uM.MovieID == dbMovie.ID))
                return BadRequest("The provided movie has already been favorited by the user!");

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
                return NotFound("User with the provided username was not found!");

            if (dbMovie == null)
                return NotFound("The provided movie was not found!");

            var dbUserMovie = await _context.UsersMovies.FindAsync(dbUser.ID, dbMovie.ID);

            if (dbUserMovie == null)
                return BadRequest("The provided movie is not favorited by the user!");
            
            _context.UsersMovies.Remove(dbUserMovie);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

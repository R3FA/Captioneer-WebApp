using Captioneer.API.Data;
using Captioneer.API.DTO;
using Captioneer.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTVShowsController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public UserTVShowsController(CaptioneerDBContext context)
        {
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<TVShowViewModel>>> GetUserTVShow(string username)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (dbUser == null)
                return NotFound("User with the provided username was not found!");

            var dbUserTVShows = await _context.UsersTVShows.Where(uM => uM.UserID == dbUser.ID).ToListAsync();
            
            if (dbUserTVShows.Count == 0)
                return Ok(new List<TVShowViewModel>());

            var favoriteTVShows = new List<TVShowViewModel>();

            foreach (var dbUserTVShow in dbUserTVShows)
            {
                var dbTVShow = await _context.TVShows.FindAsync(dbUserTVShow.TVShowID);

                if (dbTVShow == null )
                    return NotFound("Could not find one or more favorite TV shows!");

                favoriteTVShows.Add(new TVShowViewModel()
                {
                    Title = dbTVShow.Title,
                    IMDBId = dbTVShow.IMDBId,
                    Synopsis = dbTVShow.Synopsis,
                    Year = dbTVShow.Year,
                    SeasonCount = dbTVShow.SeasonCount,
                    EpisodeCount = dbTVShow.SeasonCount,
                    IMDBRatingValue = dbTVShow.IMDBRatingValue,
                    IMDBRatingCount = dbTVShow.IMDBRatingCount,
                    RottenTomatoesValue = dbTVShow.RottenTomatoesValue,
                    MetacriticValue = dbTVShow.MetacriticValue,
                    CoverArt = dbTVShow.CoverArt
                });
            }

            return Ok(favoriteTVShows);
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> PostUserTVShow(string username, TVShowViewModel TVShow)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var dbTVShow = await _context.TVShows.FirstOrDefaultAsync(m => m.Title == TVShow.Title && TVShow.Year == m.Year);

            if (dbUser == null)
                return NotFound("User with the provided username was not found!");

            if (dbTVShow == null)
                return NotFound("The provided TVShow was not found!");

            if (_context.UsersTVShows.Any(uM => uM.UserID == dbUser.ID && uM.TVShowID == dbTVShow.ID))
                return BadRequest("The provided TVShow has already been favorited by the user!");

            await _context.UsersTVShows.AddAsync(new UserTVShows()
            {
                UserID = dbUser.ID,
                TVShowID = dbTVShow.ID
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUserTVShow(string username, TVShowViewModel TVShow)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var dbTVShow = await _context.TVShows.FirstOrDefaultAsync(m => m.Title == TVShow.Title && TVShow.Year == m.Year);

            if (dbUser == null)
                return NotFound("User with the provided username was not found!");

            if (dbTVShow == null)
                return NotFound("The provided TV show was not found!");

            var dbUserTVShow = await _context.UsersTVShows.FindAsync(dbUser.ID, dbTVShow.ID);

            if (dbUserTVShow == null)
                return BadRequest("The provided TV show is not favorited by the user!");
            
            _context.UsersTVShows.Remove(dbUserTVShow);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

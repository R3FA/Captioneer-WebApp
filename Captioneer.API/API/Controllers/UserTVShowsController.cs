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
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            var dbUserTVShows = await _context.UsersTVShows.Where(uM => uM.UserID == dbUser.ID).ToListAsync();

            if (dbUserTVShows.Count == 0)
                return Ok(new List<TVShowViewModel>());

            var favoriteTVShows = new List<TVShowViewModel>();

            foreach (var dbUserTVShow in dbUserTVShows)
            {
                var dbTVShow = await _context.TVShows.FindAsync(dbUserTVShow.TVShowID);

                if (dbTVShow == null)
                {
                    LoggerManager.GetInstance().LogError($"Could not find one or more favorite TV shows for user {username}");
                    return NotFound($"Could not find one or more favorite TV shows for user {username}");
                }

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
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            if (dbTVShow == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find TV show {TVShow.Title}");
                return NotFound($"Could not find TV show {TVShow.Title}");
            }

            if (_context.UsersTVShows.Any(uM => uM.UserID == dbUser.ID && uM.TVShowID == dbTVShow.ID))
            {
                LoggerManager.GetInstance().LogError($"TV show {TVShow.Title} has already been favorited by user {username}");
                return BadRequest($"TV show {TVShow.Title} has already been favorited by user {username}");
            }

            await _context.UsersTVShows.AddAsync(new UserTVShows()
            {
                UserID = dbUser.ID,
                TVShowID = dbTVShow.ID
            });
            await _context.SaveChangesAsync();

            LoggerManager.GetInstance().LogInfo($"Added new favorite show to user {username}");
            return Ok();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUserTVShow(string username, TVShowViewModel TVShow)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var dbTVShow = await _context.TVShows.FirstOrDefaultAsync(m => m.Title == TVShow.Title && TVShow.Year == m.Year);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            if (dbTVShow == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find TV show {TVShow.Title}");
                return NotFound($"Could not find TV show {TVShow.Title}");
            }

            var dbUserTVShow = await _context.UsersTVShows.FindAsync(dbUser.ID, dbTVShow.ID);

            if (dbUserTVShow == null)
            {
                LoggerManager.GetInstance().LogError($"TV show {TVShow.Title} is not favorited by user {username} so it cannot be deleted");
                return BadRequest($"TV show {TVShow.Title} is not favorited by user {username} so it cannot be deleted");
            }

            _context.UsersTVShows.Remove(dbUserTVShow);
            await _context.SaveChangesAsync();

            LoggerManager.GetInstance().LogInfo($"Removed favorite show for user {username}");
            return Ok();
        }
    }
}

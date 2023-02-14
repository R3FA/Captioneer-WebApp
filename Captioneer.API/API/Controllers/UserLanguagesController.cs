using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTO;
using API.Data;
using API.Entities;
using UtilityService.Utils;
using System.Web.Http.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLanguagesController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public UserLanguagesController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/UserLanguages/adivonslav
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<UserLanguageViewModel>>> GetUserLanguage(string username)
        {
            var userLanguages = await _context.UsersLanguages.Where(ul => ul.User.Username == username).ToListAsync();
            var viewModels = new List<UserLanguageViewModel>();

            if (userLanguages.Count == 0)
            {
                return Ok(viewModels);
            }

            foreach (var userLanguage in userLanguages)
            {
                var dbUser = await _context.Users.FindAsync(userLanguage.UserID);
                var dbLanguage = await _context.Languages.FindAsync(userLanguage.LanguageID);

                viewModels.Add(new UserLanguageViewModel()
                {
                    Username = userLanguage.User.Username,
                    LanguageName = userLanguage.Language.Name,
                    EnglishLanguageName = userLanguage.Language.EnglishName,
                    Flag = userLanguage.Language.Flag,
                });
            }

            return Ok(viewModels);
        }

        // POST: api/UserLanguages/adivonslav
        [HttpPost("{username}")]
        public async Task<ActionResult<UserLanguageViewModel>> PostUserLanguage(string username, string englishLanguageName)
        {
            if (englishLanguageName == "")
                return BadRequest("Must provide language query");

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var dbLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.EnglishName == englishLanguageName);

            if (dbUser == null || dbLanguage == null)
            {
                LoggerManager.GetInstance().LogError("The user or language does not exist in the database");
                return NotFound("The user or language does not exist in the database");
            }

            if (_context.UsersLanguages.Any(ul => ul.UserID == dbUser.ID && ul.LanguageID == dbLanguage.ID))
            {
                LoggerManager.GetInstance().LogError($"UserLanguage for {username} and {englishLanguageName} already exists");
                return BadRequest($"UserLanguage for {username} and {englishLanguageName} already exists");
            }

            var newUserLanguage = new UserLanguage()
            {
                User = dbUser,
                Language = dbLanguage
            };

            try
            {
                await _context.AddAsync(newUserLanguage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return StatusCode(500);
            }

            return Ok();
        }

        // DELETE: api/UserLanguages
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUserLanguage(string username, string englishLanguageName)
        {
            if (englishLanguageName == "")
                return BadRequest("Must provide language query");

            var dbUserLanguage = await _context.UsersLanguages.
                FirstOrDefaultAsync(ul => ul.User.Username == username && ul.Language.EnglishName == englishLanguageName);

            if (dbUserLanguage == null)
            {
                LoggerManager.GetInstance().LogError($"The UserLanguage for {username} and {englishLanguageName} does not exist");
                return NotFound($"The UserLanguage for {username} and {englishLanguageName} does not exist");
            }

            try
            {
                _context.UsersLanguages.Remove(dbUserLanguage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}

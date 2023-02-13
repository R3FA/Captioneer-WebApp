using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.DTO;

namespace Captioneer.API.Controllers
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

            if (userLanguages.Count == 0)
            {
                return NotFound("No UserLanguage exists for the given parameters");
            }

            var viewModels = new List<UserLanguageViewModel>();

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
                return NotFound("The user or language does not exist in the database");

            if (_context.UsersLanguages.Any(ul => ul.UserID == dbUser.ID && ul.LanguageID == dbLanguage.ID))
                return BadRequest("The UserLanguage already exists in the database");

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
                return BadRequest(e.Message);
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
                return NotFound("The UserLanguage does not exist");

            try
            { 
                _context.UsersLanguages.Remove(dbUserLanguage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}

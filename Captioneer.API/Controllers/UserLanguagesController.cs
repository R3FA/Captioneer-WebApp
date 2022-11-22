using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.ViewModels;

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
            var userLanguage = await _context.UsersLanguages.Where(ul => ul.User.Username == username).ToListAsync();

            if (userLanguage.Count == 0)
            {
                return NotFound("No UserLanguage exists for the given parameters");
            }

            var viewModels = new List<UserLanguageViewModel>();

            userLanguage.ForEach(ul =>
            {
                viewModels.Add(new UserLanguageViewModel()
                {
                    Username = ul.User.Username,
                    LanguageName = ul.Language.Name,
                    EnglishLanguageName = ul.Language.EnglishName,
                    Flag = ul.Language.Flag,
                });
            });

            return Ok(viewModels);
        }

        // POST: api/UserLanguages
        [HttpPost]
        public async Task<ActionResult<UserLanguageViewModel>> PostUserLanguage(UserLanguageViewModel userLanguageVM)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLanguageVM.Username);
            var dbLanguage = await _context.Languages.FirstOrDefaultAsync(l => l.Name == userLanguageVM.LanguageName);

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

            return Ok(userLanguageVM);
        }

        // DELETE: api/UserLanguages
        [HttpDelete]
        public async Task<IActionResult> DeleteUserLanguage(UserLanguageViewModel userLanguageVM)
        {
            var dbUserLanguage = await _context.UsersLanguages.
                FirstOrDefaultAsync(ul => ul.User.Username == userLanguageVM.Username && ul.Language.Name == userLanguageVM.LanguageName);

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

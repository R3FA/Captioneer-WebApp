using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Captioneer.API.Data;
using Captioneer.API.Entities;
using Captioneer.API.Utils;
using Captioneer.API.ViewModels;

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public UsersController(CaptioneerDBContext context)
        {
            _context = context;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/adivonslav
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, UserUpdateViewModel userUpdateVM)
        {
            if (userUpdateVM.Password == string.Empty)
            {
                return BadRequest("Must provide a password to make any changes");
            }

            var dbUser = _context.Users.FirstOrDefault(u => u.Username == username);

            if (dbUser == null)
            {
                return NotFound("The User does not exist in the database");
            }


            if (!BCryptHasher.Verify(userUpdateVM.Password, dbUser.Password))
            {
                return BadRequest("The provided password is incorrect");
            }

            if (userUpdateVM.NewEmail != null)
                dbUser.Email = userUpdateVM.NewEmail;
            if (userUpdateVM.NewPassword != null)
                dbUser.Password = BCryptHasher.Hash(userUpdateVM.NewPassword);
            if (userUpdateVM.NewUsername != null)
                dbUser.Username = userUpdateVM.NewUsername;

            try
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> PostUser(UserViewModel user)
        {
            var hashedPassword = BCryptHasher.Hash(user.Password);

            var newUser = new User()
            {
                Email = user.Email,
                Password = hashedPassword,
                Username = user.Username,
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

           return Ok(newUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}

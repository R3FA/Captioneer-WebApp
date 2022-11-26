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
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly CaptioneerDBContext _context;

        public UsersController(CaptioneerDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostEnvironment = environment;
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
        public async Task<IActionResult> PutUser(string username, UserUpdateModel userUpdate)
        {
            // Password can only be null if the user wishes to update their profile image
            if (userUpdate.Password == null && userUpdate.NewProfileImage == null)
            {
                return BadRequest("Must provide a password to make any changes");
            }

            var dbUser = _context.Users.FirstOrDefault(u => u.Username == username);

            if (dbUser == null)
            {
                return NotFound("The User does not exist in the database");
            }

            // Verify that the passed password is correct if the user's username, password or email is being updated
            if (userUpdate.Password != null)
            {
                if (!BCryptHasher.Verify(userUpdate.Password!, dbUser.Password))
                {
                    return BadRequest("The provided password is incorrect");
                }
            }

            if (userUpdate.NewEmail != null)
                dbUser.Email = userUpdate.NewEmail;
            if (userUpdate.NewPassword != null)
                dbUser.Password = BCryptHasher.Hash(userUpdate.NewPassword);
            if (userUpdate.NewUsername != null)
                dbUser.Username = userUpdate.NewUsername;
            if (userUpdate.NewProfileImage != null)
            {
                var writeName = dbUser.Username;
                var filePath = await ImageSerializer.Serialize(userUpdate.NewProfileImage, _hostEnvironment.WebRootPath,  writeName);

                if (filePath == null)
                {
                    return BadRequest("Failed to save image, check the format and size again");
                }

                dbUser.ProfileImage = filePath;
            }

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
        public async Task<ActionResult<UserViewModel>> PostUser(UserPostModel user)
        {
            if (!UserExistsEmail(user)&&!UserExistsName(user))
            {
                var hashedPassword = BCryptHasher.Hash(user.Password);

                var newUser = new User()
                {
                    Email = user.Email,
                    Password = hashedPassword,
                    Username = user.Username,
                };

                try
                {
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    return BadRequest(e.Message);
                }

                return Ok();
            }
            else
                return UserExistsEmail(user)?BadRequest("Email is in use"):BadRequest("Username exists");
        }

        // DELETE: api/Users
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(UserPostModel user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null)
                return NotFound("Could not find a User with the provided username");

            if (!BCryptHasher.Verify(user.Password, dbUser.Password))
                return BadRequest("The provided password is incorrect!");

            try
            {
                _context.Users.Remove(dbUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        private bool UserExistsEmail(UserPostModel user)
        {
            return _context.Users.Any(e => e.Email == user.Email);
        } 
        private bool UserExistsName(UserPostModel user)
        {
            return _context.Users.Any(e => e.Username == user.Username);
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}

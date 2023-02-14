using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UtilityService.Utils;
using API.DTO;
using API.Data;
using API.Entities;

namespace API.Controllers
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

        //GET: api/Users/example@mail.com
        [HttpGet("{mail}")]
        public async Task<ActionResult<UserViewModel>> GetUserByEmail(string? mail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == mail);

            if (user == null)
            {
                LoggerManager.GetInstance().LogError($"User with {mail} was not found!");
                return NotFound($"User with {mail} was not found!");
            }

            UserViewModel userReturn = new UserViewModel()
            {
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Username = user.Username,
                Designation = user.Designation,
                funFact = user.funFact,
                RegistrationDate = user.RegistrationDate,
                SubtitleDownload = user.SubtitleDownload,
                SubtitleUpload = user.SubtitleUpload
            };
            return userReturn;
        }

        // GET: api/Users/adivonslav/profileimage
        [HttpGet("{username}/profileimage")]
        public async Task<ActionResult<string>> GetProfileImage(string username)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            if (dbUser.ProfileImage == null)
            {
                LoggerManager.GetInstance().LogError($"User {username} does not have a profile image!");
                return NotFound($"User {username} does not have a profile image!");
            }

            var encodedImage = await ImageSerializer.Deserialize(_hostEnvironment.WebRootPath, dbUser.ProfileImage);

            if (encodedImage == null)
            {
                LoggerManager.GetInstance().LogError($"Could not encode image to Base64 for user {username}");
                return StatusCode(500, $"Could not encode image to Base64 for user {username}");
            }

            return Ok(encodedImage);
        }

        // PUT: api/Users/adivonslav
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, UserUpdateModel userUpdate)
        {
            // Password can only be null if the user wishes to update their profile image
            if (userUpdate.Password == null && userUpdate.NewProfileImage == null)
            {
                LoggerManager.GetInstance().LogError($"User {username} must provide a password to make any changes");
                return BadRequest($"User {username} must provide a password to make any changes");
            }

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"User with {username} was not found!");
                return NotFound($"User with {username} was not found!");
            }

            // Verify that the passed password is correct if the user's username, password or email is being updated
            if (userUpdate.Password != null)
            {
                if (!BCryptHasher.Verify(userUpdate.Password!, dbUser.Password))
                {
                    LoggerManager.GetInstance().LogInfo($"User {username} provided the wrong password");
                    return BadRequest($"User {username} provided the wrong password");
                }
            }

            if (userUpdate.NewEmail != null)
                dbUser.Email = userUpdate.NewEmail;
            if (userUpdate.NewPassword != null)
            {
                var hashedPassword = BCryptHasher.Hash(userUpdate.NewPassword);

                if (hashedPassword == null)
                {
                    LoggerManager.GetInstance().LogError($"Failed to hash password for user {username}");
                    return StatusCode(500, $"Failed to hash password for user {username}");
                }

                dbUser.Password = hashedPassword;
            }
            if (userUpdate.NewUsername != null)
                dbUser.Username = userUpdate.NewUsername;
            if (userUpdate.NewProfileImage != null)
            {
                var writeName = dbUser.Username;
                var filePath = await ImageSerializer.Serialize(userUpdate.NewProfileImage, _hostEnvironment.WebRootPath, writeName);

                if (filePath == null)
                {
                    LoggerManager.GetInstance().LogError($"Failed to save image for user {username}, check the format and size again");
                    return BadRequest($"Failed to save image for user {username}, check the format and size again");
                }

                dbUser.ProfileImage = filePath;
            }
            if (userUpdate.Designation != null)
                dbUser.Designation = userUpdate.Designation;
            if (userUpdate.funFact != null)
                dbUser.funFact = userUpdate.funFact;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return BadRequest(e.Message);
            }

            LoggerManager.GetInstance().LogInfo($"Set new profile image for user {username}");
            return Ok();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> PostUser(UserPostModel user)
        {
            if (!UserExistsEmail(user) && !UserExistsName(user))
            {
                var hashedPassword = BCryptHasher.Hash(user.Password);

                if (hashedPassword == null)
                {
                    LoggerManager.GetInstance().LogError($"Failed to hash password for user {user.Username}");
                    return StatusCode(500, $"Failed to hash password for user {user.Username}");
                }

                var newUser = new User()
                {
                    Email = user.Email,
                    Password = hashedPassword,
                    Username = user.Username,
                    RegistrationDate = DateTime.Now,
                };

                try
                {
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    LoggerManager.GetInstance().LogError(e.Message);
                    return BadRequest(e.Message);
                }

                LoggerManager.GetInstance().LogInfo($"Created new user with username {user.Username} and email {user.Email}");
                return Ok();
            }
            else
            {
                LoggerManager.GetInstance().LogInfo($"User {user.Username} could not be created because it already exists");
                return UserExistsEmail(user) ? BadRequest("Email is in use") : BadRequest("Username exists");
            }
        }

        [HttpPost("login")]
        public async Task<IResult> loginUser([FromBody] UserLoginModel sentCredentials)
        {
            if (sentCredentials == null)
            {
                LoggerManager.GetInstance().LogError("No credentials passed for login");
                return (IResult)BadRequest("No credentials passed for login");
            }

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == sentCredentials.Email);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogInfo($"Could not find user with email {sentCredentials.Email}");
                return (IResult)BadRequest($"Could not find user with email {sentCredentials.Email}");
            }

            if (!BCryptHasher.Verify(sentCredentials.Password, dbUser.Password))
            {
                LoggerManager.GetInstance().LogInfo($"Password was wrong for user ${sentCredentials.Email}");
                return (IResult)BadRequest($"Password was wrong for user ${sentCredentials.Email}");
            }

            var stringToken = GenerateJwt(sentCredentials);

            return Results.Ok(stringToken);
        }

        // DELETE: api/Users
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(UserPostModel user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find user with username {user.Username}");
                return NotFound($"Could not find user with username {user.Username}");
            }

            if (!BCryptHasher.Verify(user.Password, dbUser.Password))
            {
                LoggerManager.GetInstance().LogInfo($"Password was wrong for user ${user.Username}");
                return BadRequest($"Password was wrong for user ${user.Username}");
            }

            try
            {
                _context.Users.Remove(dbUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return StatusCode(500, e.Message);
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
        private string GenerateJwt(UserLoginModel sentCredentials)
        {
            var builder = WebApplication.CreateBuilder();
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
                (builder.Configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, sentCredentials.Email),
                new Claim(JwtRegisteredClaimNames.Email, sentCredentials.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMonths(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}

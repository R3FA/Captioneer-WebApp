﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UtilityService.Utils;
using API.DTO;
using API.Data;
using API.Entities;
using Azure;
using Captioneer.API.Migrations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailService _emailService;

        private readonly CaptioneerDBContext _context;

        private string twoStepVerificationCode = string.Empty;
        private string twoStepMailSubject = string.Empty;
        private string twoStepMailBody = string.Empty;


        public UsersController(CaptioneerDBContext context, IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _context = context;
            _hostEnvironment = environment;
            _emailService = emailService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllUsers(int page = 1)
        {
            var pageResults = 3f;
            var pageCount = Math.Ceiling(this._context.Users.Count() / pageResults);
            var selectedUsers = await this._context.Users.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            List<UserViewModel> returnedUsers = new List<UserViewModel>();
            foreach (var u in selectedUsers)
            {
                var tempUsers = new UserViewModel()
                {
                    ProfileImage = u.ProfileImage,
                    Designation = u.Designation,
                    Email = u.Email,
                    funFact = u.funFact,
                    Id = u.ID,
                    RegistrationDate = u.RegistrationDate,
                    SubtitleDownload = u.SubtitleDownload,
                    SubtitleUpload = u.SubtitleUpload,
                    Username = u.Username,
                    isVerificationActive = u.isVerificationActive,
                    VerificationCode = u.VerificationCode,
                    VerificationExpireDate = u.VerificationExpireDate
                };
                returnedUsers.Add(tempUsers);
            }
            var response = new UsersResponse()
            {
                Users = returnedUsers,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            if(page > (int)pageCount) { return NotFound("There are no more pages!"); }
                return Ok(response);
        }

        //GET: api/Users/example@mail.com || example
        [HttpGet("")]
        public async Task<ActionResult<UserViewModel>> GetUserByEmail(string? mail,string? username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Email == mail) || (u.Username == username));

            if (user == null)
            {
                LoggerManager.GetInstance().LogError($"User with {mail} was not found!");
                return NotFound($"User with {mail} or {username} was not found!");
            }
            var admin = await _context.Admins.FirstOrDefaultAsync(u => (u.User == user));
            bool isAdmin = false;
            if (admin != null)
                isAdmin = true;

            UserViewModel userReturn = new UserViewModel()
            {
                Id = user.ID,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Username = user.Username,
                Designation = user.Designation,
                funFact = user.funFact,
                RegistrationDate = user.RegistrationDate,
                SubtitleDownload = user.SubtitleDownload,
                SubtitleUpload = user.SubtitleUpload,
                isBanned = user.isBanned,
                isAdmin = isAdmin,
                isVerificationActive = user.isVerificationActive,
                VerificationCode = user.VerificationCode,
                VerificationExpireDate = user.VerificationExpireDate

            };
            return userReturn;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUserByID(int id)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(x=>x.ID == id);
            if(user == null) { return NotFound("User with this ID isn't available!"); }
            var admin = await _context.Admins.FirstOrDefaultAsync(u => (u.User == user));
            bool isAdmin = false;
            if (admin != null)
                isAdmin = true;
            UserViewModel userReturn = new UserViewModel()
            {
                Id = user.ID,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Username = user.Username,
                Designation = user.Designation,
                funFact = user.funFact,
                RegistrationDate = user.RegistrationDate,
                SubtitleDownload = user.SubtitleDownload,
                SubtitleUpload = user.SubtitleUpload,
                isBanned = user.isBanned,
                isAdmin = isAdmin,
                isVerificationActive = user.isVerificationActive,
                VerificationCode = user.VerificationCode,
                VerificationExpireDate = user.VerificationExpireDate
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

            //var encodedImage = await ImageSerializer.Deserialize(_hostEnvironment.WebRootPath, dbUser.ProfileImage);

            /*
            if (encodedImage == null)
            {
                LoggerManager.GetInstance().LogError($"Could not encode image to Base64 for user {username}");
                return StatusCode(500, $"Could not encode image to Base64 for user {username}");
            }
            */

            return Ok(dbUser.ProfileImage);
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

        [HttpPut("[action]/{sentUserID}")]
        public async Task<IActionResult>BanUser(UserViewModel adminUser, int sentUserID)
        {
            var selectedUser = await this._context.Users.FirstOrDefaultAsync(x => x.ID == sentUserID);
            if(selectedUser  == null || adminUser.Id == sentUserID) { return BadRequest("You either sent an ID who isn't in our database or you tried to ban yourself!"); }
            selectedUser.isBanned = !selectedUser.isBanned;
            if (selectedUser.isBanned)
            {
                selectedUser.funFact = "This user is banned!";
                selectedUser.Designation = "Banned account!";
            }
            else
            {
                selectedUser.funFact = "";
                selectedUser.Designation = "Administrator";
            }
            await this._context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("[action]/{sentUserID}")]
        public async Task<IActionResult> MakeAdmin(UserViewModel adminUser, int sentUserID)
        {
            var selectedUser = await this._context.Users.FirstOrDefaultAsync(x => x.ID == sentUserID);
            if (selectedUser == null) { return BadRequest("You either sent an ID who isn't in our database or you tried to make yourself an admin!"); }
            Admin newAdmin = new Admin()
            {
                User = selectedUser,
                RemovedCommentsNumber = 0,
                BannedUsersNumber = 0,
                RemovedMovieSubtitlesNumber = 0,
                RemovedTVShowSubtitlesNumber = 0,
            };
            try
            {
                await _context.Admins.AddAsync(newAdmin);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return BadRequest(e.Message);
            }
            return Ok();
        }
        [HttpPut("[action]/{sentUserID}")]
        public async Task<IActionResult> RemoveAdmin(UserViewModel adminUser, int sentUserID)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(x => x.ID == sentUserID);
            if (user == null) { return BadRequest("You either sent an ID who isn't in our database or you tried to make yourself an admin!"); }
            var dbUser = await _context.Admins.FirstOrDefaultAsync(u => u.User == user);

            if (dbUser == null)
            {
                LoggerManager.GetInstance().LogError($"Could not find user with username {user.Username}");
                return NotFound($"Could not find user with username {user.Username}");
            }
            try
            {
                _context.Admins.Remove(dbUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return StatusCode(500, e.Message);
            }
            return Ok();
        }

        [HttpPut("EnableVerification/{userID}")]
        public async Task<ActionResult> EnableVerification(int userID)
        {
            var dbUser = await _context.Users.FindAsync(userID);
            if (dbUser == null)
            {
                return NotFound("User isn't found!");
            }
            if (dbUser.isVerificationActive)
            {
                dbUser.isVerificationActive = false;
            }
            else
            {
                dbUser.isVerificationActive = true;
            }
            await this._context.SaveChangesAsync();
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

                var newUser = new Entities.User()
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

        [HttpPost("SendEmailVerification/{userID}")]
        public async Task<ActionResult> VerificationEmail(int userID)
        {
            var dbUser = await _context.Users.FindAsync(userID);
            
            if(dbUser == null)
            {
                return NotFound("User isn't found!");
            }

            if (dbUser.isVerificationActive)
            {

                if (dbUser.VerificationExpireDate < DateTime.Now)
                {
                    dbUser.VerificationCode = null;
                    dbUser.VerificationExpireDate = null;
                    await this._context.SaveChangesAsync();
                }

                if (dbUser.VerificationCode == null && dbUser.VerificationExpireDate == null)
                {
                    this.twoStepVerificationCode = CreateVerificationKey();
                    this.twoStepMailSubject = "Code Verification";
                    this.twoStepMailBody = $"<p>Hello,<br/><br/>Enter this verification code, that is sent to you, into Captioneer web page: <strong>{this.twoStepVerificationCode}</strong><br/><br/>Sincerely,<br/>The Captioneer Team</p>";

                    dbUser.VerificationCode = this.twoStepVerificationCode;
                    dbUser.VerificationExpireDate = DateTime.Now.AddMinutes(10);
                    await this._context.SaveChangesAsync();

                    var sentObject = new EmailViewModel()
                    {
                        userID = dbUser.ID,
                        RecipientEmail = dbUser.Email,
                        Subject = this.twoStepMailSubject,
                        Body = this.twoStepMailBody
                    };
                    _emailService.TwoStepVerificationMail(sentObject);
                }
            }
            return Ok();
        }

        [HttpPost("VerifyLogin/{userID}/{verificationCode}")]
        public async Task<ActionResult> VerifyLogin(int userID, string verificationCode)
        {
            var dbUser = await _context.Users.FindAsync(userID);

            if (dbUser == null)
            {
                return NotFound("User isn't found!");
            }

            if(dbUser.VerificationExpireDate < DateTime.Now)
            {
                dbUser.VerificationCode = null;
                dbUser.VerificationExpireDate = null;
                await _context.SaveChangesAsync();
                return BadRequest("You didn't enter verification code in time!");
            }

            if (verificationCode != dbUser.VerificationCode)
            {
                return BadRequest("Wrong verification code!");
            }

            else
            {
                dbUser.VerificationCode = null;
                dbUser.VerificationExpireDate = null;
                await _context.SaveChangesAsync();
                return Ok();
            }
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

        private string CreateVerificationKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numbers = "0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < 2; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            for (int i = 2; i < 6; i++)
            {
                stringChars[i] = numbers[random.Next(numbers.Length)];
            }
            for (int i = 6; i < 8; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}

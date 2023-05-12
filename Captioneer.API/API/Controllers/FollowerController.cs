using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly CaptioneerDBContext _context;

        public FollowerController(CaptioneerDBContext context)
        {
            _context = context;
        }

        [HttpGet("GetFollowerOfUser/{loggedUserId}")]
        public async Task<ActionResult<UserViewModel>> GetFollowerOfUser(int loggedUserId)
        {
            var selectedFollowerUser = await this._context.Followers.Where(x => x.UserId == loggedUserId).ToListAsync();
            return Ok(selectedFollowerUser);
        }
        [HttpGet("GetFollowerCount/{userID}")]
        public async Task<ActionResult<FollowerViewModel>> GetFollowerCount(int userID)
        {
            var selectedUser = await this._context.Users.FindAsync(userID);
            if (selectedUser == null) { return NotFound("This user isn't found in our database!"); }
            var followerCount = await this._context.Followers.CountAsync(x => x.UserId == userID);
            var followingCount = await this._context.Followers.CountAsync(x => x.UserFollowingId == userID);
            var getFollowerCount = new FollowerViewModel()
            {
                getfollowerCount = followerCount,
                getfollowingCount = followingCount
            };
            return Ok(getFollowerCount);
        }

        [HttpPost("AddAUserFollower")]
        public async Task<ActionResult> AddAUserFollower(UserViewModel loggedUser, string followerUsername)
        {
            var selectedFollowerUser = await this._context.Users.FirstOrDefaultAsync(x => x.Username == followerUsername);
            var dbLoggedUser = await this._context.Users.FindAsync(loggedUser.Id);
            if ((dbLoggedUser == null) || (selectedFollowerUser == null))
                return BadRequest("Error! You either aren't logged in or you're searching for an user who isn't in our database!");
            if (await this._context.Followers.AnyAsync(x => x.UserFollowingId == selectedFollowerUser.ID))
            {
                return BadRequest("You already follow this user.");
            }
            var addedFollower = new Follower()
            {
                UserId = dbLoggedUser.ID,
                UserFollowingId = selectedFollowerUser.ID,
                FollowingCreatedAt = DateTime.Now,
            };
            await this._context.Followers.AddAsync(addedFollower);
            await this._context.SaveChangesAsync();
            return Ok("Follower added!");
        }

        [HttpDelete("DeleteFollower")]
        public async Task<ActionResult> DeleteFollower(UserViewModel loggedUser, string followerUsername)
        {
            var dbLoggedUser = await this._context.Users.FindAsync(loggedUser.Id);
            if(dbLoggedUser == null) { return BadRequest("You aren't logged in!"); }
            var selectedFollowerUser = await this._context.Followers.FirstOrDefaultAsync(x => x.UserFollowing.Username == followerUsername && x.UserId == dbLoggedUser.ID);
            if(selectedFollowerUser == null) { return BadRequest("You aren't following this user!"); }
            this._context.Followers.Remove(selectedFollowerUser);
            await this._context.SaveChangesAsync();
            return Ok("User unfollowed successfully!");
        }
    }
}
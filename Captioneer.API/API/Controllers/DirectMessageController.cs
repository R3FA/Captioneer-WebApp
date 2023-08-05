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
    public class DirectMessageController : ControllerBase
    {
        private readonly CaptioneerDBContext _dbContext;

        public DirectMessageController(CaptioneerDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // api/DirectMessage/GetMessages/{senderID}/{receiverID}
        [HttpGet("GetMessages/{senderID}/{receiverID}")]
        public async Task<ActionResult<IEnumerable<DirectMessage>>> GetMessages(int senderID, int receiverID)
        {
            var getMessagesData = await this._dbContext.DirectMessages
                .Where(x => x.User.ID == senderID && x.RecipientUser.ID == receiverID)
                .Include(x => x.User)
                .Include(x => x.RecipientUser)
                .ToListAsync();

            if (getMessagesData == null)
            {
                return NotFound("There are no conversation beetwen these two users!");
            }

            return Ok(getMessagesData);
        }

        // api/DirectMessage/SendMessage
        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage(DirectMessageViewModel objectData)
        {
            var userSender = await this._dbContext.Users.FindAsync(objectData.UserID);
            var userReceiver = await this._dbContext.Users.FindAsync(objectData.RecipientUserID);
            string messageContent = objectData.MessageContent;

            if (userSender == null || userReceiver == null || string.IsNullOrEmpty(messageContent))
            {
                return BadRequest("Wrong User or Receiver ID or you are sending an empty string!");
            }

            var sentObject = new DirectMessage()
            {
                User = userSender,
                RecipientUser = userReceiver,
                Content = messageContent,
                Time = DateTime.Now
            };

            await this._dbContext.DirectMessages.AddAsync(sentObject);
            await this._dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
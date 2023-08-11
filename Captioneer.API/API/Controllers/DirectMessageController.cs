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

        // api/DirectMessage/GetAllConversations/{userID}
        [HttpGet("GetAllConversations/{userID}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllConversations(int userID)
        {
            var userConversations = await this._dbContext.DirectMessages
                .Where(x => x.User.ID == userID)
                .Select(x => x.RecipientUser)
                .Distinct()
                .ToListAsync();

            var conversationCount = userConversations.Count;
                
            if(conversationCount != 0)
            {
                return Ok(userConversations);
            } else
            {
                userConversations = null;
                userConversations = await this._dbContext.DirectMessages
                .Where(x => x.RecipientUser.ID == userID)
                .Select(x => x.User)
                .Distinct()
                .ToListAsync();
                return Ok(userConversations);
            }
        }

        // api/DirectMessage/GetMessages/{senderID}/{receiverID}
        [HttpGet("GetMessagesForUser/{senderID}/{receiverID}")]
        public async Task<ActionResult<IEnumerable<DirectMessageViewModel>>> GetMessagesForUser(int senderID, int receiverID)
        {
            var getMessagesData = await this._dbContext.DirectMessages
                .Where(x => x.User.ID == senderID && x.RecipientUser.ID == receiverID)
                .Include(x => x.User)
                .Include(x => x.RecipientUser)
                .ToListAsync();
            List<DirectMessageViewModel> sentObject = new List<DirectMessageViewModel>();

            if (getMessagesData == null)
            {
                return NotFound("There are no conversation beetwen these two users!");
            }

            foreach(var messageData in getMessagesData) {
                var objectViewModel = new DirectMessageViewModel
                {
                    MessageContent = messageData.Content,
                    RecipientUserID = messageData.RecipientUser.ID,
                    UserID = messageData.User.ID,
                    TimeSent = messageData.Time
                };
                sentObject.Add(objectViewModel);
            }
            return Ok(sentObject);
        }

        // api/DirectMessage/SendMessage
        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage(DirectMessageViewModel objectData)
        {
            var userSender = await this._dbContext.Users.FindAsync(objectData.UserID);
            var userReceiver = await this._dbContext.Users.FindAsync(objectData.RecipientUserID);

            if (userSender == null || userReceiver == null || string.IsNullOrEmpty(objectData.MessageContent))
            {
                return BadRequest("Wrong User or Receiver ID or you are sending an empty string!");
            }

            var sentObject = new DirectMessage()
            {
                User = userSender,
                RecipientUser = userReceiver,
                Content = objectData.MessageContent,
                Time = DateTime.Now
            };

            await this._dbContext.DirectMessages.AddAsync(sentObject);
            await this._dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
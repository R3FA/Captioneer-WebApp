using API.Entities;

namespace API.DTO
{
    public class DirectMessageViewModel
    {
        public int UserID { get; set; }
        public int RecipientUserID { get; set; }
        public string? MessageContent { get; set; }
        public DateTime TimeSent { get; set; }
    }
}
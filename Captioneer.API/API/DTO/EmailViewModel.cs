namespace API.DTO
{
    public class EmailViewModel
    {
        public int userID { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
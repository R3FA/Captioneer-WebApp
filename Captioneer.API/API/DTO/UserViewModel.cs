namespace API.DTO
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? ProfileImage { get; set; }
        public string? Designation { get; set; } = string.Empty;
        public int? SubtitleUpload { get; set; } = 0;
        public int? SubtitleDownload { get; set; } = 0;
        public string? funFact { get; set; } = string.Empty;
        public DateTime? RegistrationDate { get; set; }
        public bool isBanned { get; set; }
    }
}
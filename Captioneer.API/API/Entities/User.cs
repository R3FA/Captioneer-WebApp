using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? ProfileImage { get; set; } = string.Empty;
        public string? Designation { get; set; } = "Not specified";
        public int SubtitleUpload { get; set; } = 0;
        public int SubtitleDownload { get; set; } = 0;
        public string? funFact { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public bool isBanned { get; set; } = false;
    }
}

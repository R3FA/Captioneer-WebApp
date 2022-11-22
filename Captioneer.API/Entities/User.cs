using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
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
    }
}

using Captioneer.API.Entities;

namespace Captioneer.API.ViewModels
{
    public class UserUpdateModel
    {
        public string? Password { get; set; }

        public string? NewUsername { get; set; }

        public string? NewPassword { get; set; }

        public string? NewEmail { get; set; }

        public string? NewProfileImage { get; set; }
        public string? funFact { get; set; } = string.Empty;
        public string? Designation { get; set; } = string.Empty;
    }
}
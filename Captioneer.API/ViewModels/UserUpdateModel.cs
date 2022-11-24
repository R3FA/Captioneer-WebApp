namespace Captioneer.API.ViewModels
{
    public class UserUpdateModel
    {
        public string Password { get; set; }

        public string? NewUsername { get; set; }

        public string? NewPassword { get; set; }

        public string? NewEmail { get; set; }
    }
}

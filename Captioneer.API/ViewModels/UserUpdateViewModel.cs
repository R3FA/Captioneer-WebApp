namespace Captioneer.API.ViewModels
{
    public class UserUpdateViewModel
    {
        public string Password { get; set; }

        public string? NewUsername { get; set; }

        public string? NewPassword { get; set; }

        public string? NewEmail { get; set; }
    }
}

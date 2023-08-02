using API.Entities;

namespace API.DTO
{
    public class UsersResponse
    {
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
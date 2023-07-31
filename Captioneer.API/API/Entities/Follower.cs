using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("followers")]
    public class Follower
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserFollowingId { get; set; }
        public DateTime FollowingCreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("UserFollowingId")]
        public virtual User UserFollowing { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class Admin
    {
        [Key]
        public int ID { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> RemovedComments { get; set; }

        public virtual ICollection<User> BannedUsers { get; set; }

        public virtual ICollection<SubtitleMovie> RemovedMovieSubtitles { get; set; }

        public virtual ICollection<SubtitleTVShow> RemovedTVShowSubtitles { get; set; }
    }
}

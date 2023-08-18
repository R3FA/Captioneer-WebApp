using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("admins")]
    public class Admin
    {
        [Key]
        public int ID { get; set; }

        public virtual User User { get; set; }

        public virtual int RemovedCommentsNumber { get; set; }

        public virtual int BannedUsersNumber { get; set; }

        public virtual int RemovedMovieSubtitlesNumber { get; set; }

        public virtual int RemovedTVShowSubtitlesNumber { get; set; }
    }
}

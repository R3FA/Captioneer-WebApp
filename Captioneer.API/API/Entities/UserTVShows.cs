using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class UserTVShows
    {
        public int UserID { get; set; }

        public int TVShowID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("TVShowID")]
        public virtual TVShow TVShow { get; set; }
    }
}

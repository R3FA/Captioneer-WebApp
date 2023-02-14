using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class CreatorTVShow
    {
        public int CreatorID { get; set; }

        public int TVShowID { get; set; }

        [StringLength(30)]
        public string Position { get; set; } = string.Empty;

        [ForeignKey("CreatorID")]
        public virtual Creator Creator { get; set; }

        [ForeignKey("TVShowID")]
        public virtual TVShow TVShow { get; set; }
    }
}

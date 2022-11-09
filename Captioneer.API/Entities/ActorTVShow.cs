using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Captioneer.API.Entities
{
    public class ActorTVShow
    {
        public int ActorID { get; set; }
        
        public int TVShowID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EpisodeCount { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set;} = string.Empty;

        [ForeignKey("ActorID")]
        public virtual Actor Actor { get; set; }

        [ForeignKey("MovieID")]
        public virtual TVShow TVShow { get; set; }
    }
}

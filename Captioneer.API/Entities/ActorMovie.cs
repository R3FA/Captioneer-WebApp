using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Captioneer.API.Entities
{
    public class ActorMovie
    {
        public int ActorID { get; set; }

        public int MovieID { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;

        [ForeignKey("ActorID")]
        public virtual Actor Actor { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}

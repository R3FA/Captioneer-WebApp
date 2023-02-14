using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class ActorTVShow
    {
        public int ActorID { get; set; }

        public int TVShowID { get; set; }

        [ForeignKey("ActorID")]
        public virtual Actor Actor { get; set; }

        [ForeignKey("TVShowID")]
        public virtual TVShow TVShow { get; set; }
    }
}

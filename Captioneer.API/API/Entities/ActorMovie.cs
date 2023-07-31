using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("actormovies")]
    public class ActorMovie
    {
        public int ActorID { get; set; }

        public int MovieID { get; set; }

        [ForeignKey("ActorID")]
        public virtual Actor Actor { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}

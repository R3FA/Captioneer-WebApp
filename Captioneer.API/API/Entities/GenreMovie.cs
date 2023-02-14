using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class GenreMovie
    {
        public int GenreID { get; set; }

        public int MovieID { get; set; }

        [ForeignKey("GenreID")]
        public virtual Genre Genre { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}

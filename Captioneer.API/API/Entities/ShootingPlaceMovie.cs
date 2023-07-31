using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("shootingplacesmovie")]
    public class ShootingPlaceMovie
    {
        public int ShootingPlaceID { get; set; }

        public int MovieID { get; set; }

        [ForeignKey("ShootingPlaceID")]
        public virtual ShootingPlace ShootingPlace { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}

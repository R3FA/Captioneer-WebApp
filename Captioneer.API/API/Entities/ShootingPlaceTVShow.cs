using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("shootingplacestvshows")]
    public class ShootingPlaceTVShow
    {
        public int ShootingPlaceID { get; set; }

        public int TVShowID { get; set; }

        [ForeignKey("ShootingPlaceID")]
        public virtual ShootingPlace ShootingPlace { get; set; }

        [ForeignKey("TVShowID")]
        public virtual TVShow TVShow { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("shootingplaces")]
    public class ShootingPlace
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Country { get; set; } = string.Empty;

        [StringLength(20)]
        public string? City { get; set; } = string.Empty;

        // Using decimal degrees
        public string? MapsCoordinates { get; set; } = string.Empty;
    }
}

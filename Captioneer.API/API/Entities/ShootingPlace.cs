using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class ShootingPlace
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Country { get; set; } = string.Empty;

        [StringLength(20)]
        public string? City { get; set; } = string.Empty;

        // Using decimal degrees
        public string? MapsCoordinates { get; set; } = string.Empty;
    }
}

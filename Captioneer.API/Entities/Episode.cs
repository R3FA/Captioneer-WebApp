using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class Episode
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public virtual Season Season { get; set; }

        // In minutes
        [Required]
        [Range(1, int.MaxValue)]
        public int Runtime { get; set; }

        [Required]
        [Range(0.0, 10.0)]
        public double RatingValue { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int RatingCount { get; set; }
    }
}

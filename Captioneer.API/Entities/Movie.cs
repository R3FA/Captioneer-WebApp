using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [Required]
        [StringLength(600)]
        public string Synopsis { get; set; } = string.Empty;

        [Required]
        [Range(1900, int.MaxValue)]
        public int Year { get; set; }

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

        public string? CoverArt { get; set; }
    }
}

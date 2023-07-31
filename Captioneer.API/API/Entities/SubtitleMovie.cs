using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("subtitlemovies")]
    public class SubtitleMovie
    {
        [Key]
        public int ID { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual Language Language { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int DownloadCount { get; set; }

        [Required]
        public string SubtitlePath { get; set; } = string.Empty;

        [Required]
        [Range(0.0, 10.0)]
        public double RatingValue { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int RatingCount { get; set; }

        public string? Release { get; set; }

        public int? FrameRate { get; set; }
    }
}

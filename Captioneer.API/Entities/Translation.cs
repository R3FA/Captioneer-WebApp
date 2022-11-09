using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class Translation
    {
        [Key]
        public int ID { get; set; }

        public virtual Language Language { get; set; }

        [Required]
        public string SubtitlePath { get; set; } = string.Empty;
    }
}

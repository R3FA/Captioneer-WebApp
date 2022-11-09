using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class Season
    {
        [Key]
        public int ID { get; set; }

        public virtual TVShow TVShow { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EpisodeCount { get; set; }

        public string? CoverArt { get; set; }
    }
}

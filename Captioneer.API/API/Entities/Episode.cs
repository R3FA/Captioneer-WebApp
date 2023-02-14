using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Episode
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int EpisodeNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public virtual Season Season { get; set; }
    }
}

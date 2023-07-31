using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("episodes")]
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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("translations")]
    public class Translation
    {
        [Key]
        public int ID { get; set; }
        public virtual Language Language { get; set; }
        public string Release { get; set; }

        [Required]
        public string SubtitlePath { get; set; } = string.Empty;
    }
}

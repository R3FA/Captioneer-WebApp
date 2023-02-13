using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
    public class DirectMessage
    {
        [Key]
        public int ID { get; set; }

        public virtual User User { get; set; }

        public virtual User RecipientUser { get; set; }

        [Required]
        [StringLength(100)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime Time { get; set; } = DateTime.Now;
    }
}

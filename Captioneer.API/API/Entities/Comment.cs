using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }

        public virtual User User { get; set; }

        public virtual SubtitleMovie? SubtitleMovie { get; set; }

        public virtual SubtitleTVShow? SubtitleTVShow { get; set; }

        [Required]
        [StringLength(100)]
        public string Content { get; set; } = string.Empty;
    }
}

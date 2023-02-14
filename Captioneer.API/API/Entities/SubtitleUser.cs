using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class SubtitleUser
    {
        [Key]
        public int ID { get; set; }

        public virtual User User { get; set; }

        public virtual SubtitleMovie? SubtitleMovie { get; set; }

        public virtual SubtitleTVShow? SubtitleTVShow { get; set; }

        public virtual Translation? Translation { get; set; }

        [Range(0.0, 10.0)]
        public double RatingValue { get; set; }

        [Range(0, int.MaxValue)]
        public int RatingCount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("seasons")]
    public class Season
    {
        [Key]
        public int ID { get; set; }

        public virtual TVShow TVShow { get; set; }

        public int SeasonNumber { get; set; }

        [Range(1, int.MaxValue)]
        public int? EpisodeCount { get; set; }

        public string? CoverArt { get; set; }
    }
}

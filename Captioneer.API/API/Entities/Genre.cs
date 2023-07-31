using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("genres")]
    public class Genre
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
    }
}

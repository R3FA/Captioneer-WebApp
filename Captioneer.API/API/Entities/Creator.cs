using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("creators")]
    public class Creator
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        public string Surname { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            var creator = obj as Creator;

            if (creator != null)
                return FirstName == creator.FirstName && Surname == creator.Surname;

            return false;
        }
    }
}

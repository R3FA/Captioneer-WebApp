using System.ComponentModel.DataAnnotations;

namespace Captioneer.API.Entities
{
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
                return this.FirstName == creator.FirstName && this.Surname == creator.Surname;

            return false;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Language
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string EnglishName { get; set; } = string.Empty;

        [Required]
        public string LanguageCode { get; set; } = string.Empty;

        public string? Flag { get; set; } = string.Empty;
    }
}

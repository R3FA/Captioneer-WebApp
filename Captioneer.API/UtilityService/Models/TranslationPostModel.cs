namespace UtilityService.Models
{
    public class TranslationPostModel
    {
        public string Release { get; set; }

        public string LanguageFrom { get; set; }

        public string LanguageTo { get; set; }

        public string? FileID { get; set; }
    }
}

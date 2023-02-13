using System.Text.Json.Serialization;

namespace Captioneer.API.DTO
{
    public class AzureLanguagesGetModel
    {
        [JsonPropertyName("translation")]
        public Dictionary<string, TranslationLanguage> TranslationLanguages { get; set; }
    }

    public class TranslationLanguage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nativeName")]
        public string NativeName { get; set; }

        [JsonPropertyName("dir")]
        public string Direction { get; set; }
    }
}

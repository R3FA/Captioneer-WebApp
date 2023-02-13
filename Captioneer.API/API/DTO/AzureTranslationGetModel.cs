using System.Text.Json.Serialization;

namespace Captioneer.API.DTO
{
    public class AzureTranslationGetModel
    {
        [JsonPropertyName("translations")]
        public AzureTranslation[] Translations { get; set; }
    }

    public class AzureTranslation
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }
    }
}

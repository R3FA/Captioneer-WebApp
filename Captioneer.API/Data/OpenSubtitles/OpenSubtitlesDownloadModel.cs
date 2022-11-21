using System.Text.Json.Serialization;

namespace Captioneer.API.Data.OpenSubtitles
{
    public class OpenSubtitlesDownloadModel
    {
        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("requests")]
        public int Requests { get; set; }

        [JsonPropertyName("remaining")]
        public int Remaining { get; set; }
    }
}

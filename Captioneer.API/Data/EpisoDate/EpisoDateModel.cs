using System.Text.Json.Serialization;

namespace Captioneer.API.Data.EpisoDate
{
    public class EpisoDateModel
    {
        [JsonPropertyName("tvShow")]
        public EpisoDateShow Show { get; set; }
    }

    public class EpisoDateShow
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("start_date")]
        public string? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public string? EndDate { get; set;}

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }

        [JsonPropertyName("image_path")]
        public string? ImagePath { get; set; }

        [JsonPropertyName("episodes")]
        public List<EpisoDateEpisodeModel> Episodes { get; set; }
    }

    public class EpisoDateEpisodeModel
    {
        [JsonPropertyName("season")]
        public int? Season { get; set; }

        [JsonPropertyName("episode")]
        public int? EpisodeNumber { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("air_date")]
        public string? AirDate { get; set; }
    }
}

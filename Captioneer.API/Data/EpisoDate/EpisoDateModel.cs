using Newtonsoft.Json;

namespace Captioneer.API.Data.EpisoDate
{
    public class EpisoDateModel
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("start_date")]
        public string? StartDate { get; set; }

        [JsonProperty("end_date")]
        public string? EndDate { get; set;}

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("runtime")]
        public int? Runtime { get; set; }

        [JsonProperty("image_path")]
        public string? ImagePath { get; set; }

        [JsonProperty("episodes")]
        public EpisoDateEpisodeModel[]? Episodes { get; set; }
    }

    public class EpisoDateEpisodeModel
    {
        [JsonProperty("season")]
        public int? Season { get; set; }

        [JsonProperty("episode")]
        public int? EpisodeNumber { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("air_date")]
        public string? AirDate { get; set; }
    }
}

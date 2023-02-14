using Newtonsoft.Json;

namespace UtilityService.Models
{
    /// <summary>
    /// Short form model of the OMDbModel class
    /// </summary>
    public class OMDbModelShort
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Year")]
        public string Year { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Poster")]
        public Uri Poster { get; set; }
    }
}

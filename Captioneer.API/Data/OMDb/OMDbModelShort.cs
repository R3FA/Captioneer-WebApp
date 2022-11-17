using Newtonsoft.Json;

namespace Captioneer.API.Data.OMDb
{
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

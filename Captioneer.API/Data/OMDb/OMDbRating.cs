using Newtonsoft.Json;

namespace Captioneer.API.Data.OMDb
{
    public class OMDbRating
    {
        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}

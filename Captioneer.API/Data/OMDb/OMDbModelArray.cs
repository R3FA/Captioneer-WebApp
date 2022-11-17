using Newtonsoft.Json;

namespace Captioneer.API.Data.OMDb
{
    public class OMDbModelArray
    {
        [JsonProperty("Search")]
        public OMDbModelShort[] list { get; set; }
    }
}

using Newtonsoft.Json;

namespace UtilityService.Models
{
    /// <summary>
    /// Model class for the JSON result from the OMDb API that sends a list of models
    /// </summary>
    public class OMDbModelArray
    {
        [JsonProperty("Search")]
        public OMDbModelShort[] list { get; set; }
    }
}

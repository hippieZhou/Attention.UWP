using Newtonsoft.Json;

namespace Attention.UWP.Models
{
    /// <summary>
    /// https://pixabay.com/api/docs/
    /// </summary>
    [JsonObject("API_KEY")]
    public class API_KEY
    {
        [JsonProperty("Debug")]
        public string Debug { get; set; }
        [JsonProperty("Release")]
        public string Release { get; set; }
    }
}

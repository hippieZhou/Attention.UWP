using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using System;
using Newtonsoft.Json.Linq;

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

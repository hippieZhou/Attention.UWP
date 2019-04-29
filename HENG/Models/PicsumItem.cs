using Newtonsoft.Json;

namespace HENG.Models
{
    public class PicsumItem: DataItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "download_url")]
        public string Download_url { get; set; }

        public string Thumb => $"https://picsum.photos/id/{Id}/367/267";
    }
}

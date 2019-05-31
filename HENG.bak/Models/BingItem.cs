using HENG.Helpers;
using Newtonsoft.Json;
using System;

namespace HENG.Models
{
    public class BingSource
    {
        [JsonProperty(PropertyName = "items")]
        public BingItem[] Bings { get; set; }
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }
        [JsonProperty(PropertyName = "pages")]
        public int Pages { get; set; }
        [JsonProperty(PropertyName = "per_page")]
        public int Per_page { get; set; }
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }

    public class BingItem : DataItem
    {
        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }
        [JsonProperty(PropertyName = "pub_date"), JsonConverter(typeof(BingDateTimeConverter))]
        public DateTime Datetime { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "hsh")]
        public string Hsh { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}

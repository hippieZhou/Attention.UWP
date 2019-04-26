using HENG.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENG.Models
{
    public class BingSource
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "data")]
        public BingItem[] Bings { get; set; }
    }

    public class BingItem
    {
        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }
        [JsonProperty(PropertyName = "datetime"), JsonConverter(typeof(BingDateTimeConverter))]
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

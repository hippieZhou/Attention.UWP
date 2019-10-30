using Newtonsoft.Json;
using SQLite;

namespace Attention.UWP.Models.Core
{
    public class Download : Entity
    {
        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public string Json { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}

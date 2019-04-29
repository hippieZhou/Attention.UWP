using Newtonsoft.Json;
using System;

namespace HENG.Models
{
    public class PaperItem: DataItem
    {
        [JsonProperty(PropertyName = "_id")]
        public string _id { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public DateTime Created_at { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime Updated_at { get; set; }
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }
        [JsonProperty(PropertyName = "download")]
        public int Download { get; set; }
        [JsonProperty(PropertyName = "__v")]
        public int __v { get; set; }
        [JsonProperty(PropertyName = "style")]
        public string Style { get; set; }
        [JsonProperty(PropertyName = "urls")]
        public Urls Urls { get; set; }
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
        [JsonProperty(PropertyName = "flow_type")]
        public string Flow_type { get; set; }
    }

    public class Urls
    {
        [JsonProperty(PropertyName = "raw")]
        public string Raw { get; set; }
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
        [JsonProperty(PropertyName = "regular")]
        public string Regular { get; set; }
        [JsonProperty(PropertyName = "small")]
        public string Small { get; set; }
        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class Links
    {
        [JsonProperty(PropertyName = "self")]
        public string Self { get; set; }
        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }
        [JsonProperty(PropertyName = "download")]
        public string Download { get; set; }
        [JsonProperty(PropertyName = "download_location")]
        public string Download_location { get; set; }
    }

    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "first_name")]
        public string First_name { get; set; }
        [JsonProperty(PropertyName = "last_name")]
        public string Last_name { get; set; }
        [JsonProperty(PropertyName = "twitter_username")]
        public string Twitter_username { get; set; }
        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "profile_image")]
        public Profile_Image Profile_image { get; set; }
        [JsonProperty(PropertyName = "links")]
        public Links1 Links { get; set; }
    }

    public class Profile_Image
    {
        [JsonProperty(PropertyName = "small")]
        public string Small { get; set; }
        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }
        [JsonProperty(PropertyName = "large")]
        public string Large { get; set; }
    }

    public class Links1
    {
        [JsonProperty(PropertyName = "self")]
        public string Self { get; set; }
        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }
        [JsonProperty(PropertyName = "photos")]
        public string Photos { get; set; }
    }
}

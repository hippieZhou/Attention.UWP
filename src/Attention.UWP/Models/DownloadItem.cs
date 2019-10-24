using SQLite;

namespace Attention.UWP.Models
{
    public class DownloadItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Json { get; set; }
        public double Progress { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}

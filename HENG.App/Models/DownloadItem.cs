using PixabaySharp.Models;
using SQLite.Net.Attributes;

namespace HENG.App.Models
{
    [Table("downloads")]
    public class DownloadItem : ImageItem
    {
        [PrimaryKey]
        public new int Id { get; set; }

        [Ignore]
        public int Progress { get; set; }

        public DownloadItem()
        {

        }

        public DownloadItem(ImageItem item)
        {
            this.Id = item.Id;
            this.Likes = item.Likes;
            this.Favorites = item.Favorites;
            this.Tags = item.Tags;
            this.Views = item.Views;
            this.Comments = item.Comments;
            this.Downloads = item.Downloads;
            this.PageURL = item.PageURL;
            this.UserId = item.UserId;
            this.User = item.User;
            this.Type = item.Type;
            this.UserImageURL = item.UserImageURL;

            this.IdHash = item.IdHash;
            this.WebformatURL = item.WebformatURL;
            this.WebformatWidth = item.WebformatWidth;
            this.WebformatHeight = item.WebformatHeight;
            this.PreviewURL = item.PreviewURL;
            this.PreviewWidth = item.PreviewWidth;
            this.PreviewHeight = item.PreviewHeight;
            this.ImageURL = item.ImageURL;
            this.ImageWidth = item.ImageWidth;
            this.ImageHeight = item.ImageHeight;
            this.LargeImageURL = item.LargeImageURL;
            this.FullHDImageURL = item.FullHDImageURL;
        }
    }
}

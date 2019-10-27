using System.Threading.Tasks;
using Attention.UWP.Models.Core;
using Attention.UWP.Services;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using PixabaySharp.Models;

namespace Attention.UWP.Models
{
    public enum DownloadItemResult
    {
        Started,
        Error,
        AllreadyDownloaded,
    }

    public class DownloadItem : ObservableObject
    {
        public Download Entity { get; private set; }
        public DownloadItem(ImageItem item)
        {
            Entity = new Download()
            {
                FileName = $"{item.IdHash}.png",
                Json = JsonConvert.SerializeObject(item),
                ImageUrl = item.FullHDImageURL ?? item.LargeImageURL,
                Progress = 0.0
            };
        }

        public async Task<DownloadItemResult> StartAsync()
        {
            var item = await DAL.GetByIdAsync(Entity.Id);
            return DownloadItemResult.Started;
        }
    }
}

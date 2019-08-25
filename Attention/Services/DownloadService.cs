using Attention.Models;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Attention.Services
{
    public class DownloadService
    {
        private readonly AppSettings _settings;

        public DownloadService(AppSettings settings)
        {
            _settings = settings;
        }
        public async Task DownloadAsync(DownloadItem model)
        {
            if (string.IsNullOrWhiteSpace(_settings.DownloadPath))
            {
                _settings.DownloadPath = await GetDefaultDownloadPathAsync();
            }
        }

        private async Task<string> GetDefaultDownloadPathAsync()
        {
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
            return folder.Path;
        }
    }
}

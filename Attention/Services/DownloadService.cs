using Attention.Models;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Attention.Services
{
    public class DownloadService
    {
        private readonly string _downloadPath;

        public DownloadService(string downloadPath)
        {
            if (string.IsNullOrWhiteSpace(downloadPath))
            {
                throw new ArgumentNullException();
            }
            _downloadPath = downloadPath;
        }

        public async Task Download(DownloadItem model)
        {
            await Task.CompletedTask;
        }
    }
}

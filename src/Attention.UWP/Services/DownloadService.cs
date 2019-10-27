using Attention.UWP.Models;
using System.Threading.Tasks;

namespace Attention.UWP.Services
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/uwp/api/windows.networking.backgroundtransfer.backgrounddownloader
    /// https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/BackgroundTransfer
    /// https://github.com/jQuery2DotNet/UWP-Samples
    /// </summary>
    public class DownloadService
    {
        public async Task Download(DownloadItem download)
        {
            await Task.CompletedTask;
        }
    }
}

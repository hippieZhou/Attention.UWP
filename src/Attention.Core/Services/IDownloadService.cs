using System;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    [Flags]
    public enum DownloadResult : byte
    {
        Started = 1 << 0,
        Error = 1 << 1,
        AllreadyDownloaded = 1 << 2,
    }

    public interface IDownloadService
    {
        Task<DownloadResult> Download(string folderName, string fileName, string downloadUri, CancellationToken cancellationToken);
    }
}

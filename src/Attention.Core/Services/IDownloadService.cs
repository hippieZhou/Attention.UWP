using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public interface IDownloadService
    {
        Task Download(string folderName, string fileName, string downloadUri);
    }
}

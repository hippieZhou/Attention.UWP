using Attention.UWP.Models.Repositories;
using System.IO;
using Windows.Storage;

namespace Attention.UWP.Services
{
    public class DAL
    {
        public DownloadRepository DownloadRepo { get; private set; }
        public DAL(string dbPath = default)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite");
            }
            DownloadRepo = new DownloadRepository(dbPath);
        }
    }
}

using Attention.UWP.Models.Core;

namespace Attention.UWP.Models.Repositories
{
    public class DownloadRepository : Repository<Download>
    {
        public DownloadRepository(string dbPath) : base(dbPath)
        {
        }
    }
}

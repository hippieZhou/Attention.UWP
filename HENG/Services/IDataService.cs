using HENG.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HENG.Services
{
    /// <summary>
    /// http://paper.meiyuan.in/
    /// </summary>
    public interface IDataService
    {
        Task<IEnumerable<PaperItem>> GetNewestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<PaperItem>> GetHottestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<PaperItem>> GetGirlsAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<PaperItem>> GetSkyAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken));
    }
}

using Attention.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public interface IWebClient
    {
        Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default);
    }
}

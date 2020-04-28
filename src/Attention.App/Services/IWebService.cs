using Attention.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public interface IWebService
    {
        string APIKEY { get; }
        Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default);
    }
}

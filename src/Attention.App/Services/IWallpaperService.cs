using Attention.App.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.App.Services
{
    public interface IWallpaperService
    {
        string APIKEY { get; }
        Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    }

    public class WallpaperService : IWallpaperService
    {
        public WallpaperService(string apiKey) => APIKEY = apiKey;

        public string APIKEY { get; }

        public virtual Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

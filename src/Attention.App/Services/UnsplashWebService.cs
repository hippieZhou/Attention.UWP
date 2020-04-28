using Attention.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.Core.Services
{
    public class UnsplashWebService : IWebService
    {
        public string APIKEY => throw new NotImplementedException();

        public Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

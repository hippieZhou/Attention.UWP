using Attention.App.Framework;
using Attention.App.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attention.App.Services
{
    public interface IWallpaperService
    {
        string APIKEY { get; }
        Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default);
    }

    public class WallpaperService : IWallpaperService
    {
        public WallpaperService(string apiKey) => APIKEY = apiKey;

        public string APIKEY { get; }

        public virtual Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected IEnumerable<WallpaperEntity> MapToEntities<T>(IEnumerable<T> source) where T : class
        {
            if (source == default)
            {
                return Array.Empty<WallpaperEntity>();
            }

            var mapper = EnginContext.Current.Resolve<IMapper>();
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(IMapper));
            }
            return mapper.Map<IEnumerable<WallpaperEntity>>(source);
        }
    }
}

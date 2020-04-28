using Attention.App.Models;
using Attention.Framework;
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
        Task<IEnumerable<WallpaperDto>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default);
    }

    public class WallpaperService : IWallpaperService
    {
        public WallpaperService(string apiKey) => APIKEY = apiKey;

        public string APIKEY { get; }

        public virtual Task<IEnumerable<WallpaperDto>> GetPagedItemsAsync(int page, int perPage, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected IEnumerable<WallpaperDto> MapToEntities<T>(IEnumerable<T> source) where T : class
        {
            if (source == default)
            {
                return Array.Empty<WallpaperDto>();
            }

            var mapper = EnginContext.Current.Resolve<IMapper>();
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(IMapper));
            }
            return mapper.Map<IEnumerable<WallpaperDto>>(source);
        }
    }
}

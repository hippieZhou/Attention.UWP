using Attention.App.Framework;
using Attention.App.Models;
using Attention.App.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;

namespace Attention.App.Businesss
{
    public class WallpaperItemSource : IIncrementalSource<WallpaperEntity>
    {
        private readonly IWallpaperService _client;
        private readonly List<WallpaperEntity> _entities;
        public WallpaperItemSource()
        {
            _client = EnginContext.Current.Resolve<IWallpaperService>(nameof(UnsplashService)) ?? throw new ArgumentNullException(nameof(UnsplashService));
            _entities = new List<WallpaperEntity>();
            var colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).Select(x => new WallpaperEntity
            {
                Background = new AcrylicBrush
                {
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    TintColor = x,
                    FallbackColor = x,
                    TintOpacity = 1.0,
                    TintLuminosityOpacity = 1.0,
                },
                CreateAt = DateTime.Now
            });
            _entities.AddRange(colors);
        }

        public async Task<IEnumerable<WallpaperEntity>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = (from p in _entities
                          select p).Skip(pageIndex * pageSize).Take(pageSize);
            await Task.Delay(1000);
            return result;
        }
    }
}

using Attention.App.Models;
using Attention.Core.Framework;
using Attention.Core.Services;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.UI.Xaml.Media;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;

namespace Attention.App.Businesss
{
    public sealed class WallpaperItemSource : IIncrementalSource<WallpaperDto>
    {
        private readonly IWebClient _client;
        private readonly ILoggerFacade _logger;
        private readonly List<WallpaperDto> _entities;
        public WallpaperItemSource()
        {
            _client = EnginContext.Current.Resolve<IWebClient>(nameof(UnsplashWebClient)) ?? throw new ArgumentNullException(nameof(UnsplashWebClient));
            _logger = EnginContext.Current.Resolve<ILoggerFacade>() ?? throw new ArgumentNullException(nameof(ILoggerFacade));

            ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
            ImageCache.Instance.MaxMemoryCacheCount = 200;

            _entities = new List<WallpaperDto>();

            var colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).Select(x => new WallpaperDto
            {
                Background = new AcrylicBrush
                {
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    TintColor = x,
                    FallbackColor = x,
                    TintOpacity = 1.0,
                    TintLuminosityOpacity = 1.0,
                }
            });
            _entities.AddRange(colors);
        }

        public async Task<IEnumerable<WallpaperDto>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            Stopwatch sp = Stopwatch.StartNew();

            var photos = await _client.GetPagedItemsAsync(pageIndex, pageSize, cancellationToken);

            //var photos = (from p in _entities
            //              select p).Skip(pageIndex * pageSize).Take(pageSize);
            await Task.Delay(1000);
            sp.Stop();

            _logger.Log(string.Format(
                "{0} 数据请求共计耗时：{1} 毫秒，内存消耗：{2}",
                nameof(WallpaperItemSource),
                sp.ElapsedMilliseconds,
                Microsoft.Toolkit.Converters.ToFileSizeString((long)MemoryManager.AppMemoryUsage)),
                Category.Debug, Priority.None);

            return default;
        }
    }
}

using Attention.App.Framework;
using Attention.App.Models;
using Attention.App.Services;
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
    public class WallpaperItemSource : IIncrementalSource<WallpaperEntity>
    {
        private readonly IWallpaperService _client;
        private readonly ILoggerFacade _logger;
        private readonly List<WallpaperEntity> _entities;
        public WallpaperItemSource()
        {
            ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
            ImageCache.Instance.MaxMemoryCacheCount = 200;

            _client = EnginContext.Current.Resolve<IWallpaperService>(nameof(PixabayService)) ?? throw new ArgumentNullException(nameof(PixabayService));
            _logger = EnginContext.Current.Resolve<ILoggerFacade>() ?? throw new ArgumentNullException(nameof(ILoggerFacade));
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
            //var photos = await _client.GetPagedItemsAsync(pageIndex, pageSize, cancellationToken);
            Stopwatch sp = Stopwatch.StartNew();
            var result = (from p in _entities
                          select p).Skip(pageIndex * pageSize).Take(pageSize);
            await Task.Delay(1000);
            sp.Stop();

            _logger.Log(string.Format(
                "数据请求共计耗时：{0} 毫秒，内存消耗：{1}",
                sp.ElapsedMilliseconds,
                Microsoft.Toolkit.Converters.ToFileSizeString((long)MemoryManager.AppMemoryUsage)),
                Category.Debug, Priority.None);
            return result;
        }
    }
}

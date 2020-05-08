using Attention.Core.Bus;
using Attention.Core.Commands;
using Attention.Core.Dtos;
using Attention.Core.Framework;
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
        private readonly IMediatorHandler _bus;
        private readonly ILoggerFacade _logger;
        private readonly List<WallpaperDto> _entities;
        public WallpaperItemSource()
        {
            _bus = EnginContext.Current.Resolve<IMediatorHandler>() ?? throw new ArgumentNullException(nameof(IMediatorHandler));
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
                },
                Created = DateTime.Now,
                ImageAuthor = DateTime.Now.Year.ToString(),
                ImageAuthorUrl = "https://www.baidu.com",
                ImageUri = "ms-appx:///Assets/Images/bantersnaps-wPMvPMD9KBI-unsplash.jpg",
                Thumbnail = "ms-appx:///Assets/Images/bantersnaps-wPMvPMD9KBI-unsplash.jpg"
            });
            _entities.AddRange(colors);
        }

        public async Task<IEnumerable<WallpaperDto>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            Stopwatch sp = Stopwatch.StartNew();
            var response = await _bus.Send(new PixabayCommand { Page = pageIndex, PerPage = pageSize });
            //var photos = (from p in _entities
            //              select p).Skip(pageIndex * pageSize).Take(pageSize);
            sp.Stop();

            _logger.Log(string.Format(
                "{0} 数据请求共计耗时：{1} 毫秒，内存消耗：{2}",
                nameof(WallpaperItemSource),
                sp.ElapsedMilliseconds,
                Microsoft.Toolkit.Converters.ToFileSizeString((long)MemoryManager.AppMemoryUsage)),
                Category.Debug, Priority.None);

            return response.Data;
        }
    }
}

using Attention.Core.Bus;
using Attention.Core.Commands;
using Attention.Core.Dtos;
using Attention.Core.Framework;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace Attention.App.Businesss
{
    public sealed class WallpaperItemSource : IIncrementalSource<WallpaperDto>
    {
        private readonly IMediatorHandler _bus;
        private readonly ILoggerFacade _logger;
        public WallpaperItemSource()
        {
            _bus = EnginContext.Current.Resolve<IMediatorHandler>() ?? throw new ArgumentNullException(nameof(IMediatorHandler));
            _logger = EnginContext.Current.Resolve<ILoggerFacade>() ?? throw new ArgumentNullException(nameof(ILoggerFacade));

            ImageCache.Instance.CacheDuration = TimeSpan.FromHours(24);
            ImageCache.Instance.MaxMemoryCacheCount = 200;
        }

        public async Task<IEnumerable<WallpaperDto>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            Stopwatch sp = Stopwatch.StartNew();
            var response = await _bus.Send(new PixabayCommand { Page = pageIndex, PerPage = pageSize });
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

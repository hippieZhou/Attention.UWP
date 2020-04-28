using Attention.App.Models;
using Attention.Framework;
using Microsoft.Toolkit.Collections;
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
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Businesss
{
    public class ExploreItemSource : IIncrementalSource<ExploreDto>
    {
        private readonly ILoggerFacade _logger;
        private readonly List<ExploreDto> _entities;
        public ExploreItemSource()
        {
            _logger = EnginContext.Current.Resolve<ILoggerFacade>() ?? throw new ArgumentNullException(nameof(ILoggerFacade));
            _entities = new List<ExploreDto>();

            var random = new Random(DateTime.Now.Second);
            var colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).Select(x => new ExploreDto
            {
                Background = new AcrylicBrush
                {
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    TintColor = x,
                    FallbackColor = x,
                    TintOpacity = 1.0,
                    TintLuminosityOpacity = 1.0,
                },
                Thumbnail = new BitmapImage(new Uri($"ms-appx:///Assets/Explore/Avatar0{random.Next(0, 5)}.png")),
                Title = DateTime.Now.ToString()
            });
            _entities.AddRange(colors);
        }

        public async Task<IEnumerable<ExploreDto>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
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

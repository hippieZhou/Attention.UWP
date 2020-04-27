using Attention.App.Models;
using Microsoft.UI.Xaml.Media;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.ViewModels.UcViewModels
{
    public class WallpaperExploreViewModel : UcBaseViewModel
    {
        private ObservableCollection<ExploreEntity> _entities;
        public ObservableCollection<ExploreEntity> Entities
        {
            get { return _entities; }
            set { SetProperty(ref _entities, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(() =>
                    {
                        Stopwatch sp = Stopwatch.StartNew();

                        var random = new Random(DateTime.Now.Second);
                        var colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).Select(x => new ExploreEntity
                        {
                            Background = new AcrylicBrush
                            {
                                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                                TintColor = x,
                                FallbackColor = x,
                                TintOpacity = 1.0,
                                TintLuminosityOpacity = 1.0,
                            },
                            Thumbnail = new BitmapImage
                            {
                                UriSource = new Uri($"ms-appx:///Assets/Explore/Avatar0{random.Next(0, 5)}.png")
                            },
                            Title = DateTime.Now.ToString()
                        });

                        Entities = new ObservableCollection<ExploreEntity>(colors);

                        sp.Stop();

                        Trace.WriteLine(string.Format(
                            "{0} 数据请求共计耗时：{1} 毫秒，内存消耗：{2}",
                            nameof(WallpaperExploreViewModel),
                            sp.ElapsedMilliseconds,
                            Microsoft.Toolkit.Converters.ToFileSizeString((long)MemoryManager.AppMemoryUsage)));
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _switchCommand;
        public ICommand SwitchCommand
        {
            get
            {
                if (_switchCommand == null)
                {
                    _switchCommand = new DelegateCommand<object>(visibility =>
                    {
                        if (visibility is Visibility vis)
                        {
                            Visibility = vis;
                        }
                    });
                }
                return _switchCommand;
            }
        }
    }
}

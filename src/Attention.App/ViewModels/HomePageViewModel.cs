using Attention.App.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Attention.App.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {

        }

        private ObservableCollection<WallpaperEntity> _wallpapers;
        public ObservableCollection<WallpaperEntity> Wallpapers
        {
            get { return _wallpapers ?? (_wallpapers = new ObservableCollection<WallpaperEntity>()); }
            set { SetProperty(ref _wallpapers, value); }
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
                        Wallpapers.Clear();
                        var colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null));
                        foreach (var color in colors)
                        {
                            
                            Wallpapers.Add(new WallpaperEntity
                            {
                                Background = new AcrylicBrush 
                                { 
                                    BackgroundSource = AcrylicBackgroundSource.Backdrop, 
                                    TintColor=color, 
                                    FallbackColor = color,
                                    TintOpacity=0.8,
                                    TintLuminosityOpacity = 0.8,
                                },
                                CreateAt = DateTime.Now
                            });
                        }
                    });
                }
                return _loadCommand;
            }
        }
    }
}

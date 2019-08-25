using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Models;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using System;
using Attention.Models;
using Attention.Services;

namespace Attention.ViewModels
{
    public class PhotoItemViewModel : ViewModelBase
    {
        private FrameworkElement _destinationElement;

        private DownloadItem _model;
        public DownloadItem Model
        {
            get { return _model; }
            set { Set(ref _model, value); }
        }

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        protected ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<FrameworkElement>(destinationElement =>
                    {
                        _destinationElement = destinationElement;
                        Visibility = Visibility.Collapsed;
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _hideCommand;
        public ICommand HideCommand
        {
            get
            {
                if (_hideCommand == null)
                {
                    _hideCommand = new RelayCommand<TappedRoutedEventArgs>(async args =>
                    {
                        if (args.OriginalSource is FrameworkElement root && root.Name == "rootGrid")
                        {
                            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _destinationElement);
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                            animation.IsScaleAnimationEnabled = true;
                            var done = await ViewModelLocator.Current.Shell.PhotoGridViewModel.TryStart(Model.Item, animation);
                            if (done)
                            {
                                Visibility = Visibility.Collapsed;
                            }
                        }
                    });
                }
                return _hideCommand;
            }
        }

        private ICommand _infoCommand;
        public ICommand InfoCommand
        {
            get
            {
                if (_infoCommand == null)
                {
                    _infoCommand = new RelayCommand(async () => 
                    {
                        await Launcher.LaunchUriAsync(new Uri(Model.Item.PageURL));
                    });
                }
                return _infoCommand; }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(async () =>
                    {
                        var service = ViewModelLocator.Current.GetRequiredService<DownloadService>();
                        await service.DownloadAsync(Model);
                    });
                }
                return _downloadCommand; }
        }

        public bool TryStart(object item, ConnectedAnimation animation)
        {
            if (item is ImageItem val)
            {
                Model = new DownloadItem(val);
                Visibility = Visibility.Visible;
                return animation.TryStart(_destinationElement);
            }
            else
            {
                return false;
            }
        }
    }
}

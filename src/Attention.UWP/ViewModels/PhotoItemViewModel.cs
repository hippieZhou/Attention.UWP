using Attention.UWP.Helpers;
using Attention.UWP.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Models;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.UWP.ViewModels
{
    public class PhotoItemViewModel : ViewModelBase
    {
        private UIElement _destinationElement;

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get => _visibility;
            set => Set(ref _visibility, value);
        }

        private ImageItem _item;
        public ImageItem Item
        {
            get => _item;
            set => Set(ref _item, value);
        }

        public void Initialize(Grid destinationElement) => _destinationElement = destinationElement;

        protected ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {

                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand<TappedRoutedEventArgs>(async args =>
                    {
                        if (args.OriginalSource is FrameworkElement root && root.Name == "overlayPopup")
                        {
                            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _destinationElement);
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                            animation.IsScaleAnimationEnabled = true;
                            var done = await ViewModelLocator.Current.Main.PhotoGridViewModel.TryStart(Item, animation);
                            if (done)
                            {
                                Visibility = Visibility.Collapsed;
                            }
                        }
                    });
                }
                return _backCommand;
            }
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
                        StorageFolder folder = await App.Settings.GetSavingFolderAsync();
                        DownloadItemResult state = await new DownloadItem(Item, folder).DownloadAsync();
                        Debug.WriteLine(state);
                    });
                }
                return _downloadCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new RelayCommand(() =>
                    {
                        ShareHelper.ShareData(Item);
                    });
                }
                return _shareCommand;
            }
        }

        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new RelayCommand(async () =>
                    {
                        await Launcher.LaunchUriAsync(new Uri(Item.PageURL));
                    });
                }
                return _browseCommand;
            }
        }

        public bool TryStart(object selected, ConnectedAnimation animation)
        {
            if (selected is ImageItem item)
            {
                Item = item;
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

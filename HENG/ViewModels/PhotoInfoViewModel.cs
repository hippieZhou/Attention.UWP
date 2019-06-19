using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using HENG.Services;
using PixabaySharp.Models;
using System;
using System.Windows.Input;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace HENG.ViewModels
{
    public class PhotoInfoViewModel : ViewModelBase
    {
        protected Grid _smokeGrid;

        private ImageItem _storedItem;
        public ImageItem StoredItem
        {
            get { return _storedItem; }
            set { Set(ref _storedItem, value); }
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private ICommand _showTipsCommand;
        public ICommand ShowTipsCommand
        {
            get
            {
                if (_showTipsCommand == null)
                {
                    _showTipsCommand = new RelayCommand(() =>
                    {
                        IsPaneOpen = !IsPaneOpen;
                    }, () => StoredItem != null);
                }
                return _showTipsCommand;
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
                        if (!string.IsNullOrWhiteSpace(StoredItem?.PageURL))
                        {
                            await Launcher.LaunchUriAsync(new Uri(StoredItem.PageURL));
                        }
                    }, () => StoredItem != null);
                }
                return _browseCommand;
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
                        var count = ViewModelLocator.Current.Db.InsertItem(StoredItem);
                        if (count > 0)
                        {
                            var download = new DownloadItem(StoredItem);
                            await DownloadService.DownloadAsync(download);
                        }
                    }, () => StoredItem != null);
                }
                return _downloadCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(async () =>
                    {
                        ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _smokeGrid.FindName("destinationElement") as UIElement);
                        animation.Completed += (sender, e) =>
                        {
                            _smokeGrid.Visibility = Visibility.Collapsed;
                        };
                        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                        {
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                        }
                        await ViewModelLocator.Current.Photo.TryBackwardAsync(StoredItem, animation);
                    }, () => StoredItem != null);
                }
                return _backCommand;
            }
        }

        public void Initialize(Grid smokeGrid)
        {
            _smokeGrid = smokeGrid;
        }
        public void TryForwardStart(ImageItem storedItem, ConnectedAnimation animation)
        {
            StoredItem = storedItem;
            _smokeGrid.Visibility = Visibility.Visible;
            animation.TryStart(_smokeGrid.FindName("destinationElement") as UIElement);
        }
    }
}

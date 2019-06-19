using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Models;
using System.Windows.Input;
using Windows.Foundation.Metadata;
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

        private bool _teachingTipIsOpen;
        public bool TeachingTipIsOpen
        {
            get { return _teachingTipIsOpen; }
            set { Set(ref _teachingTipIsOpen, value); }
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
                        TeachingTipIsOpen = !TeachingTipIsOpen;
                    });
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
                    _browseCommand = new RelayCommand(() =>
                    {

                    });
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
                    _downloadCommand = new RelayCommand<ImageItem>(item =>
                    {

                    });
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
                    });
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

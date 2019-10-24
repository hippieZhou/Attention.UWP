using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.UWP.Services;
using PixabaySharp.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace HENG.UWP.ViewModels
{
    public class PhotoItemViewModel : ViewModelBase
    {
        private readonly DownloadService _downloadService;
        private Grid _destinationElement;

        private ImageItem _photo;
        public ImageItem Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        private Visibility _backVisibility = Visibility.Visible;
        public Visibility BackVisibility
        {
            get { return _backVisibility; }
            set { Set(ref _backVisibility, value); }
        }

        private Visibility _infoVisibility = Visibility.Collapsed;
        public Visibility InfoVisibility
        {
            get { return _infoVisibility; }
            set { Set(ref _infoVisibility, value); }
        }

        private int _displayIndex = 0;
        public int DisplayIndex
        {
            get { return _displayIndex; }
            set
            {
                if (_displayIndex == value)
                    return;

                Set(ref _displayIndex, value);
            }
        }

        public PhotoItemViewModel(DownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        protected ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<Grid>(grid =>
                    {
                        _destinationElement = grid;
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
                    _backCommand = new RelayCommand(async () =>
                    {
                        ShowInformation(false);
                        if (DisplayIndex != 0)
                        {
                            DisplayIndex = 0;
                        }

                        ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _destinationElement);
                        animation.Completed += (sender, e) => 
                        {
                            Visibility = Visibility.Collapsed;
                        };
                  
                        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                        {
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                        }

                        await ViewModelLocator.Current.Home.PhotoGirdViewModel.TryStartConnectedAnimationAsync(Photo, animation);
                    });
                }
                return _backCommand;
            }
        }

        private ICommand _showInfoCommand;
        public ICommand ShowInfoCommand
        {
            get
            {
                if (_showInfoCommand == null)
                {
                    _showInfoCommand = new RelayCommand(() =>
                    {
                        ShowInformation(true);
                    });
                }
                return _showInfoCommand;
            }
        }

        private ICommand _hideInfoCommand;
        public ICommand HideInfoCommand
        {
            get
            {
                if (_hideInfoCommand == null)
                {
                    _hideInfoCommand = new RelayCommand(() =>
                    {
                        ShowInformation(false);
                    });
                }
                return _hideInfoCommand;
            }
        }

        private void ShowInformation(bool show)
        {
            if (show)
            {
                InfoVisibility = Visibility.Visible;
                BackVisibility = Visibility.Collapsed;
            }
            else
            {
                InfoVisibility = Visibility.Collapsed;
                BackVisibility = Visibility.Visible;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand<int>(async count =>
                    {
                        DisplayIndex = count > 0 ? count - 1 : count;

                        _downloadService.SaveToPicturesLibrary(Photo);
                        await Task.Delay(2000);
                    });
                }
                return _downloadCommand;
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(() =>
                    {
                        DisplayIndex = 0;
                    });
                }
                return _cancelCommand;
            }
        }

        private ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                if (_pauseCommand == null)
                {
                    _pauseCommand = new RelayCommand(() =>
                    {
                        DisplayIndex = 0;
                    });
                }
                return _pauseCommand;
            }
        }

        private ICommand _filpCommand;
        public ICommand FilpCommand
        {
            get
            {
                if (_filpCommand == null)
                {
                    _filpCommand = new RelayCommand(() =>
                    {
                        DisplayIndex = 0;
                    });
                }
                return _filpCommand;
            }
        }

        public bool TryStart(ImageItem photo, ConnectedAnimation animation)
        {
            Photo = photo;
            Visibility = Visibility.Visible;

            return animation.TryStart(_destinationElement);
        }
    }
}

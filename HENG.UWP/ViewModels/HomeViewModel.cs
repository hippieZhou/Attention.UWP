using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.UWP.Services;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace HENG.UWP.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public PhotoGridViewModel PhotoGirdViewModel { get; private set; } = new PhotoGridViewModel();
        public PhotoItemViewModel PhotoItemViewModel { get; private set; }
        public PhotoSearchViewModel PhotoSearchViewModel { get; private set; } = new PhotoSearchViewModel();

        public HomeViewModel(DownloadService downloadService)
        {
            PhotoItemViewModel = new PhotoItemViewModel(downloadService);
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(() =>
                    {
                        PhotoSearchViewModel.Visibility = Visibility.Visible;
                    });
                }
                return _searchCommand;
            }
        }

    }
}

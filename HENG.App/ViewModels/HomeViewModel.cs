using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.App.Models;
using HENG.App.Services;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace HENG.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly DbContext _dbContext;
        private readonly DownloadService _downloadService;

        public PhotoGridViewModel PhotoGridViewModel { get; private set; }

        public HomeViewModel(DbContext dbContext, DownloadService downloadService)
        {
            _dbContext = dbContext;
            _downloadService = downloadService;
        }

        private ICommand _loadedCommand;
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

        public void Initialize(GridView masterView, Grid detailView)
        {
            PhotoGridViewModel = new PhotoGridViewModel(_dbContext, _downloadService);
            PhotoGridViewModel.Initialize(masterView, detailView);
        }
    }
}

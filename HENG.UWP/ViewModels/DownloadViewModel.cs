using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.UWP.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HENG.UWP.ViewModels
{
    public class DownloadViewModel : ViewModelBase
    {
        private DownloadService _downloadService;
        public DownloadViewModel(DownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        await Task.CompletedTask;
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

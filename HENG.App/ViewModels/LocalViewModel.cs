using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.App.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HENG.App.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {
        private readonly DbContext _dbContext;

        public LocalViewModel(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ObservableCollection<DownloadItem> _downloads;
        public ObservableCollection<DownloadItem> Downloads
        {
            get { return _downloads ?? (_downloads = new ObservableCollection<DownloadItem>()); }
            set { Set(ref _downloads, value); }
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
                        var items = _dbContext.GetAllDownloads();
                        foreach (DownloadItem item in items)
                        {
                            Downloads.Add(item);
                        }
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

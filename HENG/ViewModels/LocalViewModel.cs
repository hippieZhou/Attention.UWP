using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HENG.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {
        private ObservableCollection<DownloadItem> _photos;
        public ObservableCollection<DownloadItem> Photos
        {
            get { return _photos ?? (_photos = new ObservableCollection<DownloadItem>()); }
            set { _photos = value; }
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
    }
}

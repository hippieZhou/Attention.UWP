using Attention.UWP.Models;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attention.UWP.ViewModels
{
    public class LocalViewModel : BaseViewModel
    {
        private ObservableCollection<DownloadItem> _items;

        public ObservableCollection<DownloadItem> Items
        {
            get { return _items ?? (_items = new ObservableCollection<DownloadItem>()); }
            set { Set(ref _items, value); }
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

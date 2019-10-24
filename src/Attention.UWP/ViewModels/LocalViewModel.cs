using Attention.UWP.Models;
using Attention.UWP.Services;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
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
                        var items = await DAL.GetAllDownloadsAsync();
                        Items.Clear();
                        foreach (var item in items)
                        {
                            Items.Add(item);
                        }
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

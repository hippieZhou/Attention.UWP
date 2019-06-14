using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using PixabaySharp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        await Task.Run(async () => 
                        {
                            IEnumerable<ImageItem> items = ViewModelLocator.Current.Db.GetAllItems<ImageItem>();
                            await DispatcherHelper.ExecuteOnUIThreadAsync(() => 
                            {
                                foreach (var item in items)
                                {
                                    Photos.Add(new DownloadItem(item));
                                }
                            });
                        });
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

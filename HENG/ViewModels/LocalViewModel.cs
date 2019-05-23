using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {

        private ObservableCollection<DownloadItem> _photos;
        public ObservableCollection<DownloadItem> Photos
        {
            get { return _photos ?? (_photos = new ObservableCollection<DownloadItem>()); }
            set { Set(ref _photos, value); }
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
                       Photos.Clear();
                       await InitializeAsync();

                       await DispatcherHelper.RunAsync(() => 
                       {
                           var items = Singleton<DataService>.Instance.Downloads;
                           foreach (DownloadItem item in items)
                           {
                               Photos.Add(item);
                           }
                       });

                       Singleton<DataService>.Instance.DownloadsEvent += async (sender, items) => 
                       {
                           await DispatcherHelper.RunAsync(() =>
                            {
                                foreach (DownloadItem item in items)
                                {
                                    Photos.Add(item);
                                }
                            });
                       };
                   });
                }
                return _loadedCommand;
            }
        }

        private async Task InitializeAsync()
        {
            StorageFolder downloadFolder = await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string> { ".jpg", ".png" });
            IReadOnlyList<StorageFile> sfs = await downloadFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            await DispatcherHelper.RunAsync(async () =>
             {
                 foreach (var sf in sfs)
                 {
                     BitmapImage photo = await ImageHelper.StorageFileToBitmapImage(sf);
                     Photos.Add(new DownloadItem()
                     {
                         ResultFile = sf,
                         Photo = photo
                     });
                 }
             });
        }
    }
}

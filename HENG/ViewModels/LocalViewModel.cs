using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HENG.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {

        private ObservableCollection<LocalItem> _photos;
        public ObservableCollection<LocalItem> Photos
        {
            get { return _photos ?? (_photos = new ObservableCollection<LocalItem>()); }
            set { Set(ref _photos, value); }
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

    //public class LocalItemSource : IIncrementalSource<LocalItem>
    //{
    //    private async Task<IEnumerable<StorageFile>> InitializedAsync()
    //    {
    //        var items = new List<StorageFile>();
    //        var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string> { ".jpg", ".png" });
    //        var folders = await KnownFolders.PicturesLibrary.GetFoldersAsync();
    //        foreach (var folder in folders)
    //        {
    //            var files = await folder.CreateFileQueryWithOptions(queryOptions)?.GetFilesAsync();
    //            items.AddRange(files);
    //        }

    //        return items;
    //    }

    //    public async Task<IEnumerable<LocalItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        var _local = await InitializedAsync();

    //        var photos = new List<LocalItem>();
    //        var files = (from p in _local select p).Skip(pageIndex * pageSize).Take(pageSize);
    //        foreach (var file in files)
    //        {
    //            var bmp = await ImageHelper.StorageFileToBitmapImage(file);
    //            photos.Add(new LocalItem { ImageCache = bmp, Key = Path.GetDirectoryName(file.Path) });
    //        }
    //        return photos;
    //    }
    //}
}

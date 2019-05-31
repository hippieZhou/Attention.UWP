using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Collections;
using PixabaySharp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HENG.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private PhotoViewModel _photo;
        public PhotoViewModel Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
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
                        if (Photo == null)
                            Photo = new PhotoViewModel();
                    });
                }
                return _loadedCommand;
            }
        }
    }

    public class PhotoViewModel : PixViewModel<PhotoItemSource, ImageItem> { }

    public class PhotoItemSource : IIncrementalSource<ImageItem>
    {
        public async Task<IEnumerable<ImageItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await ViewModelLocator.Current.PxService.QueryImagesAsync("", ++pageIndex, pageSize);
            return result?.Images;
        }
    }
}

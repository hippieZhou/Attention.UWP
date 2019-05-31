using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI.Controls;
using PixabaySharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

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

    public class PhotoViewModel : PixViewModel<PhotoItemSource, ImageItem>
    {
        public PhotoViewModel()
        {
            Messenger.Default.Register<GenericMessage<ImageItem>>(this, "backwardsAnimation", async item =>
            {
                _listView.ScrollIntoView(item.Content, ScrollIntoViewAlignment.Default);
                _listView.UpdateLayout();
                if (item.Target is ConnectedAnimation animation)
                {
                    await _listView.TryStartConnectedAnimationAsync(animation, item.Content, "connectedElement");
                }
            });
        }

        protected override void NavToByItem(ImageItem item)
        {
            ConnectedAnimation animation = null;
            if (item != null)
            {
                animation = this._listView.PrepareConnectedAnimation("forwardAnimation", item, "connectedElement");
            }
            Messenger.Default.Send(new GenericMessage<ImageItem>(this, animation, item), "forwardAnimation");
        }
    }

    public class PhotoItemSource : IIncrementalSource<ImageItem>
    {
        public async Task<IEnumerable<ImageItem>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await ViewModelLocator.Current.PxService.QueryImagesAsync("", ++pageIndex, pageSize);
            return result?.Images;
        }
    }
}

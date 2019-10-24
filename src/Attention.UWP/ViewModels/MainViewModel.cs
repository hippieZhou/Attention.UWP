using Attention.UWP.Services;
using GalaSoft.MvvmLight;

namespace Attention.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public PhotoGridHeaderViewModel PhotoGridHeaderViewModel { get; }
        public PhotoGridViewModel PhotoGridViewModel { get; }
        public PhotoItemViewModel PhotoItemViewModel { get; }

        public MainViewModel(PixabayService service)
        {
            PhotoGridHeaderViewModel = new PhotoGridHeaderViewModel();
            PhotoGridViewModel = new PhotoGridViewModel(service);
            PhotoItemViewModel = new PhotoItemViewModel();
        }
    }
}

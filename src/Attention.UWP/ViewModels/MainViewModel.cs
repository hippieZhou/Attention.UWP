using Attention.UWP.Services;
using GalaSoft.MvvmLight;

namespace Attention.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public PhotoGridHeaderViewModel PhotoGridHeaderViewModel { get; }
        public PhotoGridViewModel PhotoGridViewModel { get; }
        public PhotoItemViewModel PhotoItemViewModel { get; }

        public MainViewModel(PixabayService pixabay)
        {
            PhotoGridHeaderViewModel = new PhotoGridHeaderViewModel();
            PhotoGridViewModel = new PhotoGridViewModel(pixabay);
            PhotoItemViewModel = new PhotoItemViewModel();
        }
    }
}

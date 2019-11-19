using GalaSoft.MvvmLight;

namespace Attention.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public PhotoGridHeaderViewModel PhotoGridHeaderViewModel { get; } = new PhotoGridHeaderViewModel();
        public PhotoGridViewModel PhotoGridViewModel { get; } = new PhotoGridViewModel();
        public PhotoItemViewModel PhotoItemViewModel { get; } = new PhotoItemViewModel();
    }
}

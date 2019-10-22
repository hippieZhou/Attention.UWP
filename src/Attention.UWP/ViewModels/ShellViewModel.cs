using Attention.UWP.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public PhotoFilterViewModel PhotoFilterViewModel { get; } 
        public ShellViewModel(PixabayService service)
        {
            PhotoFilterViewModel = new PhotoFilterViewModel(service);
        }
        /// <summary>
        /// https://www.cnblogs.com/shaomeng/p/8678641.html
        /// https://github.com/r2d2rigo/WinCompositionTiltEffect
        /// https://docs.microsoft.com/en-us/windows/communitytoolkit/animations/scale
        /// </summary>
        public FrameworkElement UiElement { get; private set; }

        private bool _isPaneOpen = false;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<FrameworkElement>(uiElement =>
                    {
                        UiElement = uiElement;
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

using Attention.UWP.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroLog;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly ILogger _logger;

        public PhotoFilterViewModel PhotoFilterViewModel { get; } = new PhotoFilterViewModel();
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

        public ShellViewModel(ILogManager logManager)
        {
            _logger = logManager.GetLogger<ShellViewModel>();
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
                        _logger.Info(App.Settings.AppSummary);
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

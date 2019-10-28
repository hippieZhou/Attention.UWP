using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroLog;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        public FrameworkElement UiElement { get; private set; }

        public PhotoFilterViewModel PhotoFilterViewModel { get; } = new PhotoFilterViewModel();

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

        public void Initialize(FrameworkElement uiElement) => UiElement = uiElement;

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<RoutedEventArgs>(args =>
                    {
                        _logger.Info(App.Settings.AppSummary);
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

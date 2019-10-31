using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public FrameworkElement UiElement { get; private set; }

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
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

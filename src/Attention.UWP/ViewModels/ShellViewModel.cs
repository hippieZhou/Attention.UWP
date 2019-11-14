using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using Attention.UWP.Helpers;
using Attention.UWP.Views;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public FrameworkElement MainElement { get; private set; }
        public ShellViewModel()
        {
            Messenger.Default.Register<string>(this, nameof(AppNotification), async str =>
            {
                await Singleton<AppNotification>.Instance.ShowAsync(str, TimeSpan.FromSeconds(2.0));
            });
        }

        public void Initialize(MainView mainElement)
        {
            MainElement = mainElement;
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<RoutedEventArgs>(args =>
                    {
                        MainElement.Visibility = Visibility.Visible;
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

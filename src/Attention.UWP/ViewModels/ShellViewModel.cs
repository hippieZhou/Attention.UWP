using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using Attention.UWP.Helpers;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            Messenger.Default.Register<string>(this, nameof(AppNotification), async str =>
            {
                await Singleton<AppNotification>.Instance.ShowAsync(str, TimeSpan.FromSeconds(2.0));
            });
        }

        private bool _springVector3AnimationEnabled = false;
        public bool SpringVector3AnimationEnabled
        {
            get => _springVector3AnimationEnabled;
            set => Set(ref _springVector3AnimationEnabled, value);
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
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

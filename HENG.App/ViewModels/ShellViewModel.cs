using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.App.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private UserControl _locaView;

        private bool _isPaneOpen;
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
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        _locaView.Visibility = Visibility.Collapsed;
                        await _locaView.Offset(0, -(float)Window.Current.Bounds.Height, 0).StartAsync();
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _navToDownloadCommand;
        public ICommand NavToDownloadCommand
        {
            get
            {
                if (_navToDownloadCommand == null)
                {
                    _navToDownloadCommand = new RelayCommand(async () =>
                    {
                        LoadedCommand.Execute(null);

                        _locaView.Visibility = Visibility.Visible;
                        await _locaView.Offset(duration: 1000).StartAsync();
                    });
                }
                return _navToDownloadCommand;
            }
        }

        private ICommand _navToBackCommand;
        public ICommand NavToBackCommand
        {
            get
            {
                if (_navToBackCommand == null)
                {
                    _navToBackCommand = new RelayCommand(async () =>
                    {
                        var anim = _locaView.Offset(0, -(float)Window.Current.Bounds.Height, 1000);
                        anim.Completed += (sender, e) => 
                        {
                            _locaView.Visibility = Visibility.Collapsed;
                        };
                        await anim.StartAsync();
                    });
                }
                return _navToBackCommand;
            }
        }

        private ICommand _navToMoreCommand;
        public ICommand NavToMoreCommand
        {
            get
            {
                if (_navToMoreCommand == null)
                {
                    _navToMoreCommand = new RelayCommand(() =>
                    {
                        IsPaneOpen = !IsPaneOpen;
                    });
                }
                return _navToMoreCommand;
            }
        }

        public void Initialize(UserControl localView)
        {
            _locaView = localView;
        }
    }
}

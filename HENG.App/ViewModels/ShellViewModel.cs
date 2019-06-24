using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.App.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private Button _backButton;
        private Button _menuButton;

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private ViewModelBase _selected;
        public ViewModelBase Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        Selected = ViewModelLocator.Current.Home;
                        _backButton.Visibility = Visibility.Collapsed;
                        _menuButton.Visibility = Visibility.Visible;
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
                        Selected = ViewModelLocator.Current.Local;
                        _backButton.Visibility = Visibility.Visible;
                        _menuButton.Visibility = Visibility.Collapsed;

                        await _backButton.Rotate(value: -180.0f,
                            centerX: (float)(_backButton.ActualWidth / 2),
                            centerY: (float)(_backButton.ActualHeight / 2),
                            duration: 1000, delay: 0).Fade(value: 1.0f, duration: 800, delay: 0).StartAsync();
                    });
                }
                return _navToDownloadCommand;
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

        private ICommand _navToBackCommand;
        public ICommand NavToBackCommand
        {
            get
            {
                if (_navToBackCommand == null)
                {
                    _navToBackCommand = new RelayCommand(async () =>
                    {
                        Selected = ViewModelLocator.Current.Home;

                        var anim = _backButton.Rotate(value: 0.0f,
                            centerX: (float)(_backButton.ActualWidth / 2),
                            centerY: (float)(_backButton.ActualHeight / 2),
                            duration: 1000, delay: 0).Fade(value: 0.2f, duration: 800, delay: 0);
                        anim.Completed += (sender, e) => 
                        {
                            _backButton.Visibility = Visibility.Collapsed;
                            _menuButton.Visibility = Visibility.Visible;
                        };
                        await anim.StartAsync();
                    });
                }
                return _navToBackCommand;
            }
        }

        public void Initialize(Button backButton, Button menuButton)
        {
            _backButton = backButton;
            _menuButton = menuButton;
        }
    }
}

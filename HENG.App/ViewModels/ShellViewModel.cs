using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace HENG.App.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly NavigationService _navService;
        private Frame currentFrame;

        public ShellViewModel(NavigationService navService)
        {
            _navService = navService;
        }

        public void Initialize(Frame contentFrame)
        {
            currentFrame = contentFrame;
            _navService.CurrentFrame = currentFrame;
        }

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
                    _loadedCommand = new RelayCommand(() =>
                    {
                        _navService.NavigateTo(typeof(HomeViewModel).FullName);
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _openPaneCommand;
        public ICommand OpenPaneCommand
        {
            get
            {
                if (_openPaneCommand == null)
                {
                    _openPaneCommand = new RelayCommand(() =>
                    {
                        IsPaneOpen = !IsPaneOpen;
                    });
                }
                return _openPaneCommand;
            }
        }

    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace HENG.App.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
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
                    _navToDownloadCommand = new RelayCommand(() =>
                    {
                        Selected = ViewModelLocator.Current.Local;
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
                    _navToBackCommand = new RelayCommand(() =>
                    {
                        Selected = ViewModelLocator.Current.Home;
                    });
                }
                return _navToBackCommand;
            }
        }
    }
}

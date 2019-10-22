using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class PhotoGridHeaderViewModel : ViewModelBase
    {
        private ICommand _paneOpenCommand;
        public ICommand PaneOpenCommand
        {
            get
            {
                if (_paneOpenCommand == null)
                {
                    _paneOpenCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Shell.IsPaneOpen = 
                        !ViewModelLocator.Current.Shell.IsPaneOpen;
                    });
                }
                return _paneOpenCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Shell.IsPaneOpen = false;
                        ViewModelLocator.Current.Download.Visibility = Visibility.Visible;
                    });
                }
                return _downloadCommand;
            }
        }

        private ICommand _moreCommand;
        public ICommand MoreCommand
        {
            get
            {
                if (_moreCommand == null)
                {
                    _moreCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Shell.IsPaneOpen = false;
                        ViewModelLocator.Current.More.Visibility = Visibility.Visible;
                    });
                }
                return _moreCommand;
            }
        }
    }
}

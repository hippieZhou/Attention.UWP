using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace HENG.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ScrollHeaderMode _headerMode = ThemeSelectorService.ScrollHeaderMode;
        public ScrollHeaderMode HeaderMode
        {
            get { return _headerMode; }
            set { Set(ref _headerMode, value); }
        }

        private PhotoViewModel _photo;
        public PhotoViewModel Photo
        {
            get { return _photo ?? (_photo = new PhotoViewModel()); }
            set { Set(ref _photo, value); }
        }

        private PhotoInfoViewModel _photoInfo;
        public PhotoInfoViewModel PhotoInfo
        {
            get { return _photoInfo ?? (_photoInfo = new PhotoInfoViewModel()); }
            set { Set(ref _photoInfo, value); }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new RelayCommand<string>(controlKey =>
                    {
                        ViewModelLocator.Current.Shell.NavToCommand.Execute(controlKey);
                    });
                }
                return _itemInvokedCommand;
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Home.Photo.RefreshCommand.Execute(null);
                    });
                }
                return _refreshCommand;
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand<AutoSuggestBoxQuerySubmittedEventArgs>(args =>
                    {
                        ViewModelLocator.Current.Px.QueryText = args.QueryText;
                        RefreshCommand.Execute(null);
                    });
                }
                return _searchCommand;
            }
        }
    }
}

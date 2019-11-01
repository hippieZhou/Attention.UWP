using Attention.UWP.Extensions;
using Attention.UWP.Helpers;
using Attention.UWP.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class PhotoGridHeaderViewModel : ViewModelBase
    {
        public PhotoGridHeaderViewModel()
        {
            SwitchHeaderMode(App.Settings.HeaderMode);
        }

        private ScrollHeaderMode _headerMode;
        public ScrollHeaderMode HeaderModel
        {
            get { return _headerMode; }
            set { Set(ref _headerMode, value); }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(async () =>
                    {
                        await Singleton<SearchView>.Instance.ShowAsync();
                    });
                }
                return _searchCommand;
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
                        BackToView(ViewModelLocator.Current.Local);
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
                        BackToView(ViewModelLocator.Current.More);
                    });
                }
                return _moreCommand;
            }
        }

        public void SwitchHeaderMode(int mode) => HeaderModel = (ScrollHeaderMode)Enum.ToObject(typeof(ScrollHeaderMode), mode);

        private void BackToView(BaseViewModel uiElement)
        {
            ViewModelLocator.Current.Shell.MainElement.PlayScaleSpringAnimation(true);
            uiElement.Visibility = Visibility.Visible;
        }
    }
}

using Attention.UWP.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Numerics;
using System.Windows.Input;
using Windows.UI.Composition;
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
            ViewModelLocator.Current.Shell.UiElement.PlayScaleSpringAnimation(true);
            uiElement.Visibility = Visibility.Visible;
        }
    }
}

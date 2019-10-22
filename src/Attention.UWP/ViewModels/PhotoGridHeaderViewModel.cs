using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Numerics;
using System.Windows.Input;
using Windows.UI.Composition;
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
                        BackToView(ViewModelLocator.Current.Download);
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

        private void BackToView(BaseViewModel view)
        {
            ViewModelLocator.Current.Shell.IsPaneOpen = false;
            
            SpringVector3NaturalMotionAnimation springAnimation = Window.Current.Compositor.CreateSpringVector3Animation();
            springAnimation.Target = "Scale";
            springAnimation.FinalValue = new Vector3(0.8f);
            FrameworkElement root = ViewModelLocator.Current.Shell.UiElement;
            ViewModelLocator.Current.Shell.UiElement.CenterPoint = new Vector3((float)(root.ActualSize.X / 2.0), (float)(root.ActualSize.Y / 2.0), 1.0f);
            root.StartAnimation(springAnimation);

            view.Visibility = Visibility.Visible;
        }
    }
}

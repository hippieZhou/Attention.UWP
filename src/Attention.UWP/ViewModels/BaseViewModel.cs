using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Numerics;
using System.Windows.Input;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility

        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        protected ICommand _backCommand;
        public virtual ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(() =>
                    {
                        Visibility = Visibility.Collapsed;

                        SpringVector3NaturalMotionAnimation springAnimation = Window.Current.Compositor.CreateSpringVector3Animation();
                        springAnimation.Target = "Scale";
                        springAnimation.FinalValue = new Vector3(1.0f);
                        FrameworkElement root = ViewModelLocator.Current.Shell.UiElement;
                        ViewModelLocator.Current.Shell.UiElement.CenterPoint = new Vector3((float)(root.ActualSize.X / 2.0), (float)(root.ActualSize.Y / 2.0), 1.0f);
                        root.StartAnimation(springAnimation);
                    });
                }
                return _backCommand;
            }
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Models;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.UWP.ViewModels
{
    public class PhotoItemViewModel : ViewModelBase
    {
        private UIElement _destinationElement;

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        private ImageItem _item;
        public ImageItem Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        protected ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<UIElement>(destinationElement =>
                    {
                        _destinationElement = destinationElement;
                        Visibility = Visibility.Collapsed;
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand<TappedRoutedEventArgs>(async args =>
                    {
                        if (args.OriginalSource is FrameworkElement root && root.Name == "OverlayPopup")
                        {
                            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _destinationElement);
                            animation.Configuration = new DirectConnectedAnimationConfiguration();
                            animation.IsScaleAnimationEnabled = true;
                            var done = await ViewModelLocator.Current.Main.PhotoGridViewModel.TryStart(Item, animation);
                            if (done)
                            {
                                Visibility = Visibility.Collapsed;
                            }
                        }
                    });
                }
                return _backCommand;
            }
        }


        public bool TryStart(object selected, ConnectedAnimation animation)
        {
            if (selected is ImageItem item)
            {
                Item = item;
                Visibility = Visibility.Visible;
                return animation.TryStart(_destinationElement);
            }
            else
            {
                return false;
            }
        }
    }
}

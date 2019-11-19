using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.UWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public PhotoGridHeaderViewModel PhotoGridHeaderViewModel { get; } = new PhotoGridHeaderViewModel();
        public PhotoGridViewModel PhotoGridViewModel { get; } = new PhotoGridViewModel();
        public PhotoItemViewModel PhotoItemViewModel { get; } = new PhotoItemViewModel();

        private const string forwardAnimation = "forwardAnimation";
        private const string backwardsAnimation = "backwardsAnimation";
        private string connectedElementName;

        public bool Forward(GridViewItem container)
        {
            ConnectedAnimationService.GetForCurrentView().DefaultDuration = TimeSpan.FromSeconds(1.0);

            connectedElementName = ((FrameworkElement)container.ContentTemplateRoot).Name;

            ConnectedAnimation animation = PhotoGridViewModel.ViewContainer.PrepareConnectedAnimation(
                forwardAnimation, PhotoGridViewModel.Selected, connectedElementName);
            animation.IsScaleAnimationEnabled = true;
            animation.Configuration = new BasicConnectedAnimationConfiguration();
            animation.Completed += (sender, e) =>
            {
                container.Opacity = 0.0d;
            };
            
            PhotoItemViewModel.Visibility = Visibility.Visible;
            PhotoItemViewModel.Item = PhotoGridViewModel.Selected;

            return animation.TryStart(PhotoItemViewModel.DestinationElement, new List<UIElement> { container });
        }

        public async Task<bool> Backwards()
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate(
                backwardsAnimation, PhotoItemViewModel.DestinationElement);
            animation.IsScaleAnimationEnabled = true;
            animation.Configuration = new DirectConnectedAnimationConfiguration();

            if (PhotoGridViewModel.ViewContainer.ContainerFromItem(PhotoItemViewModel.Item) is GridViewItem container)
            {
                PhotoItemViewModel.Visibility = Visibility.Collapsed;

                PhotoGridViewModel.Selected = PhotoItemViewModel.Item;
                PhotoGridViewModel.ViewContainer.ScrollIntoView(PhotoGridViewModel.Selected, ScrollIntoViewAlignment.Default);
                PhotoGridViewModel.ViewContainer.UpdateLayout();
                container.Opacity = 1.0d;

                return await PhotoGridViewModel.ViewContainer.TryStartConnectedAnimationAsync(animation, PhotoGridViewModel.Selected, connectedElementName);
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attention.UWP.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.StartScreen;
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

        public MainViewModel()
        {
            Messenger.Default.Register<bool>(this, nameof(App.Settings.LiveTitle), async enabled =>
            {
                AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
                bool isPinned = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
                if (isPinned && enabled)
                {
                    IEnumerable<string> images = from p in PhotoGridViewModel.Items.Take(5) select p.PreviewURL;
                    LiveTileHelper.UpdateLiveTile(images);
                }
                else
                {
                    LiveTileHelper.CleanUpTile();
                }
            });
        }

        #region ConnectedAnimation
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
        #endregion
    }
}

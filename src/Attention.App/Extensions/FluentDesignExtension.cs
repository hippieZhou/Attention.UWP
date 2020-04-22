using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App.Extensions
{
    public static class FluentDesignExtension
    {
        public static Visual ElementVisual(this UIElement element) => ElementCompositionPreview.GetElementVisual(element);

        private const float SCALE_ANIMATION_FACTOR = 1.05f;

        public static Vector3KeyFrameAnimation CreateScaleAnimation(this UIElement root, bool show)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var rootVisual = root.ElementVisual();
            var scaleAnimation = rootVisual.Compositor.CreateVector3KeyFrameAnimation();
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.InsertKeyFrame(1.0f, new Vector3(show ? SCALE_ANIMATION_FACTOR : 1.0f));
            return scaleAnimation;
        }

        public static FrameworkElement Play(this FrameworkElement root, CompositionAnimation animation)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var rootVisual = root.ElementVisual();
            if (rootVisual.CenterPoint.X == 0 && rootVisual.CenterPoint.Y == 0)
            {
                rootVisual.CenterPoint = new Vector3((float)(root.ActualWidth / 2.0), (float)(root.ActualHeight / 2.0), 0f);
            }
            rootVisual.StartAnimation("Scale", animation);
            return root;
        }

        public static TChild FindVisualChild<TChild>(DependencyObject obj) where TChild : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChild found)
                    return found;
                else
                {
                    TChild childOfChild = FindVisualChild<TChild>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static ConnectedAnimation CreateForwardAnimation(this GridViewItem container, GridView root, object entity, Action onCompleted = null)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            ConnectedAnimationService.GetForCurrentView().DefaultDuration = TimeSpan.FromSeconds(1.0);
            var connectedElement = container.ContentTemplateRoot as FrameworkElement;
            ConnectedAnimation animation = root.PrepareConnectedAnimation("forwardAnimation", entity, connectedElement.Name);
            animation.IsScaleAnimationEnabled = true;
            animation.Configuration = new BasicConnectedAnimationConfiguration();
            animation.Completed += (sender, e) => { onCompleted?.Invoke(); };
            return animation;
        }

        public static ConnectedAnimation CreateBackwardsAnimation(this UIElement destinationElement, Action onCompleted = null)
        {
            if (destinationElement == null)
            {
                throw new ArgumentNullException(nameof(destinationElement));
            }

            ConnectedAnimationService.GetForCurrentView().DefaultDuration = TimeSpan.FromSeconds(1.0);
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", destinationElement);
            animation.Configuration = new DirectConnectedAnimationConfiguration();
            animation.IsScaleAnimationEnabled = true;
            animation.Completed += (sender, e) => { onCompleted?.Invoke(); };
            return animation;
        }
    }
}
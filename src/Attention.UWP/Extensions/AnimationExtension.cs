using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Attention.UWP.Extensions
{
    public static class AnimationExtension
    {
        public static Visual Visual(this UIElement element) => ElementCompositionPreview.GetElementVisual(element);

        private const float SCALE_ANIMATION_FACTOR = 1.05f;

        public static ScalarKeyFrameAnimation CreateScaleAnimation(bool show)
        {
            var scaleAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1f, show ? SCALE_ANIMATION_FACTOR : 1f);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }

        public static void PlayScaleAnimation(this FrameworkElement parent, FrameworkElement child, ScalarKeyFrameAnimation scaleAnimation)
        {
            var imgVisual = child.Visual();
            if (imgVisual.CenterPoint.X == 0 && imgVisual.CenterPoint.Y == 0)
            {
                imgVisual.CenterPoint = new Vector3((float)(parent.ActualWidth / 2.0), (float)(parent.ActualHeight / 2.0), 0f);
            }
            imgVisual.StartAnimation("Scale.X", scaleAnimation);
            imgVisual.StartAnimation("Scale.Y", scaleAnimation);
        }

        public static void PlayScaleSpringAnimation(this FrameworkElement element, bool back = false)
        {
            SpringVector3NaturalMotionAnimation springAnimation = Window.Current.Compositor.CreateSpringVector3Animation();
            springAnimation.Target = "Scale";
            springAnimation.FinalValue = back ? new Vector3(0.8f) : new Vector3(1.0f);
            element.CenterPoint = new Vector3((float)(element.ActualSize.X / 2.0), (float)(element.ActualSize.Y / 2.0), 1.0f);
            element.StartAnimation(springAnimation);
        }
    }
}

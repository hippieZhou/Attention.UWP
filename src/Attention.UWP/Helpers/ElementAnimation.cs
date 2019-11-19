using Attention.UWP.Extensions;
using Windows.UI.Xaml;

namespace Attention.UWP.Helpers
{
    public class ElementAnimation
    {
        public static bool GetSpringVector3AnimationEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(SpringVector3AnimationEnabledProperty);
        }

        public static void SetSpringVector3AnimationEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(SpringVector3AnimationEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for SpringVector3AnimationEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpringVector3AnimationEnabledProperty =
            DependencyProperty.RegisterAttached("SpringVector3AnimationEnabled", typeof(bool), typeof(ElementAnimation), new PropertyMetadata(false, (d, e) => 
            {
                if (d is FrameworkElement handler && e.NewValue is bool enabled)
                {
                    handler.PlayScaleSpringAnimation(enabled);
                }
            }));
    }
}

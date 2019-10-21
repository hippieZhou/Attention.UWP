using Attention.UWP.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Attention.UWP.Behaviors
{
    public class GridScalarAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var element = TargetObject ?? sender as FrameworkElement;
            if (element != null)
            {
                element.PlayScaleAnimation(element, AnimationExtension.CreateScaleAnimation(Enable));
            }
            return null;
        }

        public bool Enable
        {
            get { return (bool)GetValue(EnableProperty); }
            set { SetValue(EnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Enable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.Register("Enable", typeof(bool), typeof(GridScalarAction), new PropertyMetadata(false));

        public FrameworkElement TargetObject
        {
            get { return (FrameworkElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(GridScalarAction), new PropertyMetadata(null));
    }
}

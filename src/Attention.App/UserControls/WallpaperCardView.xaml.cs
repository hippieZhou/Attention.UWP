using Attention.App.ViewModels.UcViewModels;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App.UserControls
{
    public sealed partial class WallpaperCardView : UserControl
    {
        public WallpaperCardView()
        {
            this.InitializeComponent();
            SharedShadow.Receivers.Add(BackgroundGrid);
            destinationElement.Translation += new Vector3(0, 0, 32);
        }

        public WallpaperCardViewModel ConcreteDataContext
        {
            get { return (WallpaperCardViewModel)GetValue(ConcreteDataContextProperty); }
            set { SetValue(ConcreteDataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConcreteDataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConcreteDataContextProperty =
            DependencyProperty.Register("ConcreteDataContext", typeof(WallpaperCardViewModel), typeof(WallpaperCardView), new PropertyMetadata(null, (d, e) =>
             {
                 if (d is WallpaperCardView handler && e.NewValue is WallpaperCardViewModel vm)
                 {
                     vm.LoadCommand.Execute(handler.destinationElement);
                 }
             }));

        private void DestinationElement_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
        }

        private void DestinationElement_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            destinationElement_Transform.TranslateX += e.Delta.Translation.X;
            destinationElement_Transform.TranslateY += e.Delta.Translation.Y;
        }

        private void DestinationElement_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            DoubleAnimation CreateTranslateAnimation(EasingFunctionBase easingFunction, double from, double to = 0, double duration = 600)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    EasingFunction = easingFunction,
                    From = from,
                    To = to,
                    Duration = new Duration(TimeSpan.FromMilliseconds(duration))
                };
                return animation;
            }

            var ef = new CubicEase() { EasingMode = EasingMode.EaseInOut };
            DoubleAnimation translateXAnimation = CreateTranslateAnimation(ef, destinationElement_Transform.TranslateX);
            DoubleAnimation translateYAnimation = CreateTranslateAnimation(ef, destinationElement_Transform.TranslateY);

            Storyboard.SetTarget(translateXAnimation, destinationElement_Transform);
            Storyboard.SetTarget(translateYAnimation, destinationElement_Transform);
            Storyboard.SetTargetProperty(translateXAnimation, "CompositeTransform.TranslateX");
            Storyboard.SetTargetProperty(translateYAnimation, "CompositeTransform.TranslateY");

            Storyboard sb = new Storyboard();
            sb.Children.Add(translateXAnimation);
            sb.Children.Add(translateYAnimation);
            sb.Begin();
        }
    }
}

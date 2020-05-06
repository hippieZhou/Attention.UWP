using Attention.App.ViewModels.UcViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using System;
using System.Numerics;
using Windows.Devices.Input;
using Windows.UI.Composition;
using Windows.UI.Composition.Interactions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App.UserControls
{
    public sealed partial class WallpaperCardView : UserControl
    {
        private readonly Compositor _compositor;
        private readonly Visual _root;

        private VisualInteractionSource _interactionSource;

        public WallpaperCardView()
        {
            this.InitializeComponent();
            _compositor = Window.Current.Compositor;
            _root = ElementCompositionPreview.GetElementVisual(Root);

            Loaded += (s, e) =>
            {
                _interactionSource = VisualInteractionSource.Create(_root);
                _interactionSource.PositionYSourceMode = InteractionSourceMode.EnabledWithInertia;
                _interactionSource.PositionXSourceMode = InteractionSourceMode.EnabledWithInertia;
                _interactionSource.IsPositionXRailsEnabled = false;
                _interactionSource.IsPositionYRailsEnabled = false;

                ConfigSpringBasedTracker(destinationElement, 240.0d, 0.4f);
            };
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


        #region PC Mouse Animation
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
        #endregion

        #region Surface Touch Animation
        private void ConfigSpringBasedTracker(UIElement element, double periodInMs,
            float dampingRatio, float finalValue = 0.0f, double delayInMs = 0.0f)
        {
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);

            var tracker = InteractionTracker.Create(_compositor);
            tracker.InteractionSources.Add(_interactionSource);

            UpdateTrackingZone(tracker, Root);
            Root.SizeChanged += (sender, e) =>
            {
                if (e.PreviousSize.Equals(e.NewSize)) return;

                UpdateTrackingZone(tracker, Root);
            };

            var modifier = InteractionTrackerInertiaNaturalMotion.Create(_compositor);
            var springAnimation = _compositor.CreateSpringScalarAnimation();
            springAnimation.Period = TimeSpan.FromMilliseconds(periodInMs);
            springAnimation.DampingRatio = dampingRatio;
            springAnimation.FinalValue = finalValue;
            // This will cause a black screen for a few seconds. Bug??
            springAnimation.DelayTime = TimeSpan.FromMilliseconds(delayInMs);

            modifier.Condition = _compositor.CreateExpressionAnimation("true");
            modifier.NaturalMotion = springAnimation;

            tracker.ConfigurePositionXInertiaModifiers(new InteractionTrackerInertiaModifier[] { modifier });
            tracker.ConfigurePositionYInertiaModifiers(new InteractionTrackerInertiaModifier[] { modifier });

            var imageVisual = ElementCompositionPreview.GetElementVisual(element);
            imageVisual.StartAnimation("Translation", -tracker.GetReference().Position);
        }

        private void UpdateTrackingZone(InteractionTracker tracker, UIElement rootZone)
        {
            tracker.MaxPosition = new Vector3(rootZone.RenderSize.ToVector2() / 2, 0.0f);
            tracker.MinPosition = -tracker.MaxPosition;
        }

        private void DestinationElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                try
                {
                    _interactionSource.TryRedirectForManipulation(e.GetCurrentPoint(Root));
                }
                catch (Exception)
                {
                    // Catch unauthorized input.
                }
            }
        }
        #endregion
    }
}

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
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Attention.App.UserControls
{
    public sealed partial class DetailCardView : UserControl
    {
        private readonly Compositor _compositor;
        private VisualInteractionSource _interactionSource;

        private readonly Visual _root;

        public DetailCardView()
        {
            this.InitializeComponent();
            _compositor = Window.Current.Compositor;
            _root = ElementCompositionPreview.GetElementVisual(OverlayPopup);
            Loaded += (s, e) =>
            {
                _interactionSource = VisualInteractionSource.Create(_root);
                _interactionSource.PositionYSourceMode = InteractionSourceMode.EnabledWithInertia;
                _interactionSource.PositionXSourceMode = InteractionSourceMode.EnabledWithInertia;
                _interactionSource.IsPositionXRailsEnabled = false;
                _interactionSource.IsPositionYRailsEnabled = false;

                ConfigSpringBasedTracker(HeroImage, 240.0d, 0.4f);
            };
        }

        private void ConfigSpringBasedTracker(UIElement element, double periodInMs,
                   float dampingRatio, float finalValue = 0.0f, double delayInMs = 0.0f)
        {
            ElementCompositionPreview.SetIsTranslationEnabled(element, true);

            var tracker = InteractionTracker.Create(_compositor);
            tracker.InteractionSources.Add(_interactionSource);

            UpdateTrackingZone(tracker, OverlayPopup);
            OverlayPopup.SizeChanged += (sender, e) =>
            {
                if (e.PreviousSize.Equals(e.NewSize)) return;

                UpdateTrackingZone(tracker, OverlayPopup);
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

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                try
                {
                    _interactionSource.TryRedirectForManipulation(e.GetCurrentPoint(OverlayPopup));
                }
                catch (Exception)
                {
                    // Catch unauthorized input.
                }
            }
        }

        public DetailCardViewModel ConcreteDataContext
        {
            get { return (DetailCardViewModel)GetValue(ConcreteDataContextProperty); }
            set { SetValue(ConcreteDataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConcreteDataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConcreteDataContextProperty =
            DependencyProperty.Register("ConcreteDataContext", typeof(DetailCardViewModel), typeof(DetailCardView), new PropertyMetadata(null, (d, e) =>
             {
                 if (d is DetailCardView handler && e.NewValue is DetailCardViewModel vm)
                 {
                     vm.Initialize(handler.HeroImage, handler.Header, handler.Pane2Panel);
                 }
             }));

        private void OnContentViewModeChanged(WinUI.TwoPaneView sender, object args)
        {
            switch (sender.Mode)
            {
                // Update layout when two Panes are stacked horizontally.
                case WinUI.TwoPaneViewMode.Wide:
                    PlanTripTop.Visibility = Visibility.Collapsed;
                    break;

                // Update layout when two Panes are stacked vertically.
                case WinUI.TwoPaneViewMode.Tall:
                    PlanTripTop.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}

using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Attention.UWP
{
    public sealed partial class ShellPage : Page
    {
        private readonly CompositionLinearGradientBrush _gradientBrush1;
        private readonly CompositionLinearGradientBrush _gradientBrush2;
        private readonly SpriteVisual _backgroundVisual;
        private static readonly Color GradientStop1StartColor = Color.FromArgb(255, 43, 210, 255);
        private static readonly Color GradientStop2StartColor = Color.FromArgb(255, 43, 255, 136);
        private static readonly Color GradientStop3StartColor = Colors.Red;
        private static readonly Color GradientStop4StartColor = Colors.Black;

        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;

        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            Messenger.Default.Register<string>(this, nameof(InAppNotification), str =>
            {
                InAppNotification.Show(str, 2000);
            });

            var compositor = Window.Current.Compositor;

            _gradientBrush1 = compositor.CreateLinearGradientBrush();
            _gradientBrush1.StartPoint = new Vector2(1.0f);
            _gradientBrush1.EndPoint = Vector2.Zero;

            var gradientStop1 = compositor.CreateColorGradientStop();
            gradientStop1.Offset = 0.5f;
            gradientStop1.Color = GradientStop1StartColor;
            var gradientStop2 = compositor.CreateColorGradientStop();
            gradientStop2.Offset = 0.5f;
            gradientStop2.Color = GradientStop2StartColor;
            _gradientBrush1.ColorStops.Add(gradientStop1);
            _gradientBrush1.ColorStops.Add(gradientStop2);

            _gradientBrush2 = compositor.CreateLinearGradientBrush();
            _gradientBrush2.StartPoint = new Vector2(1.0f, 0);
            _gradientBrush2.EndPoint = new Vector2(0, 1.0f);

            var gradientStop3 = compositor.CreateColorGradientStop();
            gradientStop3.Offset = 0.25f;
            gradientStop3.Color = GradientStop4StartColor;
            var gradientStop4 = compositor.CreateColorGradientStop();
            gradientStop4.Offset = 1.0f;
            gradientStop4.Color = GradientStop4StartColor;
            _gradientBrush2.ColorStops.Add(gradientStop3);
            _gradientBrush2.ColorStops.Add(gradientStop4);

            var graphicsEffect = new BlendEffect()
            {
                Mode = BlendEffectMode.Screen,
                Foreground = new CompositionEffectSourceParameter("Main"),
                Background = new CompositionEffectSourceParameter("Tint"),
            };

            var effectFactory = compositor.CreateEffectFactory(graphicsEffect);
            var brush = effectFactory.CreateBrush();
            brush.SetSourceParameter("Main", _gradientBrush1);
            brush.SetSourceParameter("Tint", _gradientBrush2);

            _backgroundVisual = compositor.CreateSpriteVisual();
            _backgroundVisual.Brush = brush;
            ElementCompositionPreview.SetElementChildVisual(Gradient, _backgroundVisual);

            var gradientStop1OffsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            gradientStop1OffsetAnimation.Duration = TimeSpan.FromSeconds(1);
            gradientStop1OffsetAnimation.DelayTime = TimeSpan.FromSeconds(2);
            gradientStop1OffsetAnimation.InsertKeyFrame(1.0f, 0.25f);
            gradientStop1.StartAnimation(nameof(gradientStop1.Offset), gradientStop1OffsetAnimation);

            var gradientStop2OffsetAnimation = compositor.CreateScalarKeyFrameAnimation();
            gradientStop2OffsetAnimation.Duration = TimeSpan.FromSeconds(1);
            gradientStop2OffsetAnimation.DelayTime = TimeSpan.FromSeconds(2);
            gradientStop2OffsetAnimation.InsertKeyFrame(1.0f, 1.0f);
            gradientStop2.StartAnimation(nameof(gradientStop1.Offset), gradientStop2OffsetAnimation);

            var gradientStop3Animation = compositor.CreateColorKeyFrameAnimation();
            gradientStop3Animation.Duration = TimeSpan.FromSeconds(2);
            gradientStop3Animation.DelayTime = TimeSpan.FromSeconds(2);
            gradientStop3Animation.Direction = AnimationDirection.Alternate;
            gradientStop3Animation.InsertKeyFrame(1.0f, GradientStop3StartColor);
            gradientStop3.StartAnimation(nameof(gradientStop1.Color), gradientStop3Animation);

            Loaded += async (s, e) =>
            {
                await Task.Yield();
                MainGrid.Visibility = Visibility.Visible;
                Gradient.Visibility = Visibility.Collapsed;
            };

            Gradient.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                _backgroundVisual.Size = e.NewSize.ToVector2();
                _gradientBrush1.CenterPoint = _backgroundVisual.Size / 2;
                _gradientBrush2.CenterPoint = _backgroundVisual.Size / 2;
            };
        }
    }
}

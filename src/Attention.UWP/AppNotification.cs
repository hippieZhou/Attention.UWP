using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.UWP
{
    public sealed class AppNotification : Control
    {
        public AppNotification()
        {
            DefaultStyleKey = typeof(AppNotification);
            Width = Window.Current.Bounds.Width;
            Height = Window.Current.Bounds.Height;
            Transitions = new TransitionCollection
            {
                new EntranceThemeTransition()
            };
            Window.Current.SizeChanged += (sender, e) =>
            {
                Width = Window.Current.Bounds.Width;
                Height = Window.Current.Bounds.Height;
            };
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AppNotification), new PropertyMetadata(string.Empty));

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AppNotification), new PropertyMetadata(TimeSpan.FromSeconds(2.0)));

        public async Task ShowAsync(string text, TimeSpan? timeSpan = default)
        {
            Text = text;
            if (timeSpan.HasValue)
            {
                Duration = timeSpan.Value;
            }

            var popup = new Popup
            {
                IsOpen = true,
                Child = this
            };
            await Task.Delay(Duration);
            popup.Child = null;
            popup.IsOpen = false;
        }
    }
}

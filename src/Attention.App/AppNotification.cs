using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App
{
    public sealed class AppNotification : Control
    {
        public AppNotification()
        {
            this.DefaultStyleKey = typeof(AppNotification);
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

        public string Notification
        {
            get { return (string)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Notification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(string), typeof(AppNotification), new PropertyMetadata(default));


        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AppNotification), new PropertyMetadata(TimeSpan.FromSeconds(2.0)));


        public async Task ShowAsync(string notification, TimeSpan? timeSpan = default)
        {
            Notification = notification;
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

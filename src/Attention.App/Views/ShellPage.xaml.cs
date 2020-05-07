using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Attention.App.Views
{
    public sealed partial class ShellPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public ShellPageViewModel ConcreteDataContext => DataContext as ShellPageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ShellPage()
        {
            this.InitializeComponent();
            ConcreteDataContext.Initialize(shellNav, shellFrame, inAppNotification);
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };

            CustomizeTitleBar();
            void CustomizeTitleBar()
            {
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                // Draw into the title bar.
                coreTitleBar.ExtendViewIntoTitleBar = true;
                DraggableAppTitleBarArea.Height = coreTitleBar.Height;

                // Set a draggable region.
                Window.Current.SetTitleBar(DraggableAppTitleBarArea);

                coreTitleBar.LayoutMetricsChanged += (s, args) =>
                {
                    DraggableAppTitleBarArea.Height = s.Height;
                };

                coreTitleBar.IsVisibleChanged += (s, args) =>
                {
                    DraggableAppTitleBarArea.Visibility = s.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                };

                // Remove the solid-colored backgrounds behind the caption controls and system back button.
                var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
                viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                viewTitleBar.ButtonForegroundColor = Colors.DarkGray;
            }

            Loaded += (sender, e) => 
            {
                var titleBarHeight = CoreApplication.GetCurrentView().TitleBar.Height;
                shellNav.Padding = new Thickness(0, titleBarHeight, 0, 0);
            };
        }
    }
}

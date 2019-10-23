using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace Attention.UWP
{
    public sealed partial class ExtendedTitleBar : UserControl
    {
        private readonly UISettings _uiSettings;
        private readonly AccessibilitySettings _accessibilitySettings;
        private readonly CoreApplicationViewTitleBar _coreTitleBar;

        public ObservableCollection<Button> Buttons { get; } = new ObservableCollection<Button>();

        public ExtendedTitleBar()
        {
            this.InitializeComponent();

            _uiSettings = new UISettings();
            _accessibilitySettings = new AccessibilitySettings();
            _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            _coreTitleBar.ExtendViewIntoTitleBar = true;

            Loaded += OnLoaded;
            Buttons.CollectionChanged += (sender, e) => 
            {
                ItemsPanel.Children.Clear();
                foreach (var button in Buttons)
                {
                    ItemsPanel.Children.Add(button);
                }
            };
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ExtendedTitleBar), new PropertyMetadata(string.Empty));

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(BackgroundElement);

            // Register events
            _coreTitleBar.IsVisibleChanged += (_s, _e) =>
            {
                SetTitleBarVisibility();
            };

            _coreTitleBar.LayoutMetricsChanged += (_s, _e) => 
            {
                LayoutRoot.Height = _coreTitleBar.Height;
                SetTitleBarPadding();
            };

            _uiSettings.ColorValuesChanged += async (_s, _e) => 
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SetTitleBarControlColors(); });
            };

            _accessibilitySettings.HighContrastChanged += async (_s, _e) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SetTitleBarControlColors();
                    SetTitleBarVisibility();
                });
            };
            Window.Current.Activated += (_s, _e) =>
            {
                VisualStateManager.GoToState(
                    this, _e.WindowActivationState == CoreWindowActivationState.Deactivated ?
                    WindowNotFocused.Name : WindowFocused.Name, false);
            };

            // Set properties
            LayoutRoot.Height = _coreTitleBar.Height;

            SetTitleBarControlColors();
            SetTitleBarVisibility();
            SetTitleBarPadding();
        }

        private void SetTitleBarVisibility()
        {
            LayoutRoot.Visibility = _coreTitleBar.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetTitleBarPadding()
        {
            double leftAddition;
            double rightAddition;
            if (FlowDirection == FlowDirection.LeftToRight)
            {
                leftAddition = _coreTitleBar.SystemOverlayLeftInset;
                rightAddition = _coreTitleBar.SystemOverlayRightInset;
            }
            else
            {
                leftAddition = _coreTitleBar.SystemOverlayRightInset;
                rightAddition = _coreTitleBar.SystemOverlayLeftInset;
            }

            LayoutRoot.Padding = new Thickness(leftAddition, 0, rightAddition, 0);
        }

        private void SetTitleBarControlColors()
        {
            var applicationView = ApplicationView.GetForCurrentView();
            if (applicationView == null)
                return;

            var applicationTitleBar = applicationView.TitleBar;
            if (applicationTitleBar == null)
                return;

            if (_accessibilitySettings.HighContrast)
            {
                // Reset to use default colors.
                applicationTitleBar.ButtonBackgroundColor = null;
                applicationTitleBar.ButtonForegroundColor = null;
                applicationTitleBar.ButtonInactiveBackgroundColor = null;
                applicationTitleBar.ButtonInactiveForegroundColor = null;
                applicationTitleBar.ButtonHoverBackgroundColor = null;
                applicationTitleBar.ButtonHoverForegroundColor = null;
                applicationTitleBar.ButtonPressedBackgroundColor = null;
                applicationTitleBar.ButtonPressedForegroundColor = null;
            }
            else
            {
                Color bgColor = Colors.Transparent;
                //Color fgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlPageTextBaseHighBrush"]).Color;
                //Color inactivefgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundChromeDisabledLowBrush"]).Color;
                //Color hoverbgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundListLowBrush"]).Color;
                //Color hoverfgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;
                //Color pressedbgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundListMediumBrush"]).Color;
                //Color pressedfgColor = ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;

                Color fgColor = ((SolidColorBrush)Resources["ButtonForegroundColor"]).Color;
                Color inactivefgColor = ((SolidColorBrush)Resources["ButtonInactiveForegroundBrush"]).Color;
                Color hoverbgColor = ((SolidColorBrush)Resources["ButtonHoverBackgroundBrush"]).Color;
                Color hoverfgColor = ((SolidColorBrush)Resources["ButtonHoverForegroundBrush"]).Color;
                Color pressedbgColor = ((SolidColorBrush)Resources["ButtonPressedBackgroundBrush"]).Color;
                Color pressedfgColor = ((SolidColorBrush)Resources["ButtonPressedForegroundBrush"]).Color;
                applicationTitleBar.ButtonBackgroundColor = bgColor;
                applicationTitleBar.ButtonForegroundColor = fgColor;
                applicationTitleBar.ButtonInactiveBackgroundColor = bgColor;
                applicationTitleBar.ButtonInactiveForegroundColor = inactivefgColor;
                applicationTitleBar.ButtonHoverBackgroundColor = hoverbgColor;
                applicationTitleBar.ButtonHoverForegroundColor = hoverfgColor;
                applicationTitleBar.ButtonPressedBackgroundColor = pressedbgColor;
                applicationTitleBar.ButtonPressedForegroundColor = pressedfgColor;
            }
        }
    }
}

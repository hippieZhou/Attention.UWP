using Attention.UWP.Helpers;
using Attention.UWP.Models;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP
{
    sealed partial class App : Application
    {
        public static AppSettings Settings => Current.Resources["AppSettings"] as AppSettings;
        private Frame rootFrame;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;

            new BackgroundProxy().Register();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox")
            {
                ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
                ApplicationViewScaling.TrySetDisableLayoutScaling(true);
            }

            await InitializeAsync();
            InitWindow(skipWindowCreation: e.PrelaunchActivated);
            //await StartupAsync();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await InitializeAsync();
            InitWindow(skipWindowCreation: false);

            if (args.Kind == ActivationKind.ToastNotification)
            {
                Window.Current.Activate();
                //await StartupAsync();
            }
        }

        private async Task InitializeAsync() => await Settings.InitializeAsync();

        private void InitWindow(bool skipWindowCreation)
        {
            rootFrame = Window.Current.Content as Frame;
            bool initApp = rootFrame == null && !skipWindowCreation;
            if (initApp)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += (sender, e) => 
                {
                    throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
                };
                Window.Current.Content = rootFrame;

                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(ShellPage));
                }

                SetupTitlebar();
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void SetupTitlebar()
        {
            ElementTheme TrueTheme()
            {
                var frameworkElement = Window.Current.Content as FrameworkElement;
                return frameworkElement.ActualTheme;
            }

            void SetTitleBarColors()
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    if (TrueTheme() == ElementTheme.Dark)
                    {
                        titleBar.ButtonForegroundColor = Colors.White;
                        titleBar.ForegroundColor = Colors.White;
                    }
                    else
                    {
                        titleBar.ButtonForegroundColor = Colors.Black;
                        titleBar.ForegroundColor = Colors.Black;
                    }

                    titleBar.BackgroundColor = Colors.Black;

                    titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    titleBar.ButtonInactiveForegroundColor = Colors.LightGray;
                }
            }

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                SetTitleBarColors();
                CoreApplicationViewTitleBar coreTitleBar = TitleBarHelper.Instance.TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;

                Messenger.Default.Register<ElementTheme>(this, nameof(ElementTheme), theme =>
                {
                    SetTitleBarColors();
                });

                new UISettings().ColorValuesChanged += async (_s, _e) =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(SetTitleBarColors);
                };
            }
        }
    }
}

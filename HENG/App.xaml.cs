using Microsoft.HockeyApp;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using HENG.Services;
using System.Threading.Tasks;

namespace HENG
{
    sealed partial class App
    {
        public App()
        {
            HockeyClient.Current.Configure("f9f04c24aefd4b3fa38f825676a79aa6");
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await InitializeAsync();
            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                ExtendedSplash extendedSplash = new ExtendedSplash(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            Window.Current.Activate();
            DispatcherHelper.Initialize();
            await DispatcherHelper.UIDispatcher.RunIdleAsync(async s =>
            {
                await BackgroundTaskService.AttachToDownloadsAsync();
            });


            ExtendAcrylicIntoTitleBar();

            void ExtendAcrylicIntoTitleBar()
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }

        private async Task InitializeAsync()
        {
            await ThemeSelectorService.InitializeAsync();
            await BackgroundTaskService.RegisterBackgroundTaskAsync();
        }

        public static async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        protected async override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            var instance = args?.TaskInstance;
            await BackgroundTaskService.CheckCompletionResult(instance);
            base.OnBackgroundActivated(args);
        }
    }
}
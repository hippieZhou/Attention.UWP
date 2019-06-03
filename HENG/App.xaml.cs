using GalaSoft.MvvmLight.Threading;
using HENG.Services;
using System;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HENG
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            if (e.PrelaunchActivated)
                return;

            ExtendAcrylicIntoTitleBar();
            void ExtendAcrylicIntoTitleBar()
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
            }

            CreateFrameWithArgurments(e.Arguments);
            DispatcherHelper.Initialize();
            await DownloadService.AttachToDownloadsAsync(new CancellationTokenSource());
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            var taskInstance = args.TaskInstance;

            taskInstance.Canceled += (sender, reason) => 
            {
                var deferral = taskInstance.GetDeferral();
                deferral.Complete();
            };
            base.OnBackgroundActivated(args);
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            string arg = null;
            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;
                arg = toastActivationArgs.Argument;
            }
            CreateFrameWithArgurments(arg);
        }

        private Frame CreateFrameWithArgurments(string args)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(Shell), args);
            Window.Current.Activate();
            return rootFrame;
        }
    }
}

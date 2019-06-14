using HENG.Services;
using HENG.Tasks;
using HENG.ViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XamlBrewer.Uwp.Controls;

namespace HENG
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += (sender, e) =>
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                //TODO: Save application state and stop any background activity
                deferral.Complete();
            };
            this.UnhandledException += (sender, e) => { e.Handled = true; };
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
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

            RegisterBackgroundTask();

            await InitializeAsync();
            await InitWindowAsync(e.Arguments, e.SplashScreen);
            await DownloadService.AttachToDownloadsAsync();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);


            var taskInstance = args.TaskInstance;
            var deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += (sender, reason) =>
            {
                var tasks = BackgroundTaskRegistration.AllTasks.Values.Where(t => t.Name == DownloadService.NAME);
                tasks.AsParallel().ForAll(p => p.Unregister(true));
            };

            deferral.Complete();
        }

        protected override async void OnActivated(IActivatedEventArgs e)
        {
            string arg = null;
            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;
                arg = toastActivationArgs.Argument;
            }
            await InitWindowAsync(arg, e.SplashScreen);
        }

        private async Task<Frame> InitWindowAsync(string args, SplashScreen splashScreen)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += (sender, e) => { throw new Exception("Failed to load Page " + e.SourcePageType.FullName); };
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(Shell), args);
            (rootFrame.Content as Page).OpenFromSplashScreen(splashScreen.ImageLocation);
            Window.Current.Activate();

            await StartupAsync();

            return rootFrame;
        }

        private async Task InitializeAsync()
        {
            await ThemeSelectorService.InitializeAsync();
        }

        private async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
        }

        private void RegisterBackgroundTask()
        {
            BackgroundTaskRegistration registered = BackgroundTaskHelper.Register(typeof(HENGBackgroundTask),
                                new TimeTrigger(15, true), false, true,
                                new SystemCondition(SystemConditionType.InternetAvailable));
            if (registered != null)
            {
                Trace.WriteLine($"Task {typeof(HENGBackgroundTask)} registered successfully.");
            }
        }
    }
}

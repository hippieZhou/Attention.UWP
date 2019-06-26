using HENG.App.Models;
using HENG.App.Services;
using HENG.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.App
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += (sender, e) =>
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                deferral.Complete();
            };
            this.UnhandledException += (sender, e) => { e.Handled = true; };
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (e.PrelaunchActivated)
                return;

            await InitializeAsync();
            await InitWindowAsync(e.Arguments, e.SplashScreen);
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
    }

    sealed partial class App : Application
    {
        private async Task InitializeAsync()
        {
            if (SystemInformation.IsFirstRun)
            {
                if (Resources["AppSettings"] is AppSettings settings)
                {
                    settings.ThemeMode = (int)ElementTheme.Default;
                    settings.Language = Language.CurrentInputMethodLanguageTag == "zh-Hans-CN" ? 0 : 1;
                }
            }
            RegisterBackgroundTask();
            //await DownloadService.AttachToDownloadsAsync();
            //await ViewModelLocator.Current.Db.Initialize();

            await Task.Yield();
        }

        private async Task<Frame> InitWindowAsync(string args, SplashScreen splashScreen = null)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += (sender, e) => { throw new Exception("Failed to load Page " + e.SourcePageType.FullName); };
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(ShellPage), args);
            Window.Current.Activate();
            ExtendAcrylicIntoTitleBar();

            await StartupAsync();

            return rootFrame;
        }

        private async Task StartupAsync()
        {
            if (Resources["AppSettings"] is AppSettings settings)
            {
                settings.UpdateTheme();
                settings.UpdateLanguage();
                await settings.UpdateDownloadPathAsync();
            }
            await Task.Yield();
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

        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}

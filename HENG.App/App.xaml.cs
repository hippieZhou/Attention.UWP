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
            InitWindow(skipWindowCreation: e.PrelaunchActivated);
            await StartupAsync();
        }

        protected override async void OnActivated(IActivatedEventArgs e)
        {
            await InitializeAsync();
            InitWindow(skipWindowCreation: false);
            await StartupAsync();
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
    }

    sealed partial class App : Application
    {
        private async Task InitializeAsync()
        {
            RegisterBackgroundTask();
            await Task.Yield();
        }

        private void InitWindow(bool skipWindowCreation)
        {
            var rootFrame = Window.Current.Content as Frame;
            bool initApp = rootFrame == null && !skipWindowCreation;
            if (initApp)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += (sender, e) => { throw new Exception("Failed to load Page " + e.SourcePageType.FullName); };
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(ShellPage));
            ExtendAcrylicIntoTitleBar();

            Window.Current.Activate();
        }

        private async Task StartupAsync()
        {

            if (Resources["AppSettings"] is AppSettings settings)
            {
                await settings.InitConfiguration();
            }
            await Window.Current.Dispatcher.RunIdleAsync(async _ => await DownloadService.AttachToDownloads());
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

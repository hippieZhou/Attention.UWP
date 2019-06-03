using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Services;
using HENG.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

            await RegisterBackgroundTaskAsync();
            DispatcherHelper.Initialize();
            await DownloadService.AttachToDownloadsAsync();
        }

        private Frame CreateFrameWithArgurments(string args)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += (sender, e) => { throw new Exception("Failed to load Page " + e.SourcePageType.FullName); };
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(Shell), args);
            Window.Current.Activate();
            return rootFrame;
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
            var deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += (sender, reason) => 
            {
                var tasks = BackgroundTaskRegistration.AllTasks.Values.Where(t => t.Name == DownloadService.NAME);
                tasks.AsParallel().ForAll(p => p.Unregister(true));
            };

            deferral.Complete();
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

        private async Task RegisterBackgroundTaskAsync()
        {
            BackgroundTaskRegistration task = await BackgroundTaskHelper.RegisterBackgroundTask(typeof(HENGBackgroundTask),
                "HENGBackgroundTask", new TimeTrigger(15, false), null);
            task.Progress += (sender, args) => { Trace.WriteLine($"Background {sender.Name} TaskOnProgress."); };
            task.Completed += (sender, args) => { Debug.WriteLine($"Background {sender.Name} TaskOnCompleted."); };
        }
    }
}

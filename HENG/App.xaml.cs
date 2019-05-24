using Microsoft.HockeyApp;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using HENG.Services;
using System.Threading.Tasks;
using HENG.Models;
using Microsoft.Extensions.Configuration;
using Windows.Storage;
using HENG.Helpers;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.ApplicationModel.Background;
using System.Threading;
using System.Linq;

namespace HENG
{
    sealed partial class App
    {
        public static AppSettings Settings { get; private set; }

        private CancellationTokenSource _cancellationTokenSource;

        public App()
        {
            HockeyClient.Current.Configure("f9f04c24aefd4b3fa38f825676a79aa6");
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox")
            {
                ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
                bool result = ApplicationViewScaling.TrySetDisableLayoutScaling(true);
            }

            await InitializeAsync();
            bool loadState = e.PreviousExecutionState == ApplicationExecutionState.Terminated;
            await InitWindowAsync(e.SplashScreen, loadState);

        }

        private async Task InitWindowAsync(SplashScreen splashScreen,bool loadState)
        {
            ExtendedSplash extendedSplash = new ExtendedSplash(splashScreen, loadState);
            Window.Current.Content = extendedSplash;

            ApplicationView.PreferredLaunchViewSize = new Size(1280, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            ExtendAcrylicIntoTitleBar();

            void ExtendAcrylicIntoTitleBar()
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
            }

            DispatcherHelper.Initialize();

            _cancellationTokenSource = new CancellationTokenSource();
            await Window.Current.Dispatcher.RunIdleAsync(async (s) => await BackgroundDownloadHelper.AttachToDownloadsAsync(_cancellationTokenSource));
            Window.Current.Activate();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            if (args.Kind == ActivationKind.ToastNotification)
            {
                await InitializeAsync();

                await InitWindowAsync(args.SplashScreen, false);
            }
        }

        private async Task InitializeAsync()
        {
            async Task LoadConfigurationAsync()
            {
                var builder = new ConfigurationBuilder().AddJsonFile("AppSettings.json", true, true);
                var conf = builder.Build();
                Settings = conf.Get<AppSettings>();

                if (string.IsNullOrWhiteSpace(Settings.DownloadPath))
                {
                    StorageFolder sf = await KnownFolders.PicturesLibrary.CreateFolderAsync("HENG", CreationCollisionOption.OpenIfExists);
                    Settings.DownloadPath = sf.Path;
                }
            }
            await LoadConfigurationAsync();

            await ThemeSelectorService.InitializeAsync();

            await Task.CompletedTask;
        }

        public static async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            IBackgroundTaskInstance taskInstance = args.TaskInstance;
            var taskDef = taskInstance.GetDeferral();

            taskInstance.Canceled += (sender, reason) => 
            {
                _cancellationTokenSource?.Cancel();
                taskDef.Complete();
                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                {
                    if (cur.Value.Name == BackgroundDownloadHelper.NAME)
                    {
                        cur.Value.Unregister(true);
                    }
                }
            };
        }
    }
}
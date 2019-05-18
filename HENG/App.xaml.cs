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

namespace HENG
{
    sealed partial class App
    {
        public static AppSettings Settings { get; private set; }

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

            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = e.PreviousExecutionState == ApplicationExecutionState.Terminated;
                ExtendedSplash extendedSplash = new ExtendedSplash(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            ApplicationView.PreferredLaunchViewSize = new Size(1280, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Window.Current.Activate();

            ExtendAcrylicIntoTitleBar();

            void ExtendAcrylicIntoTitleBar()
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }

            DispatcherHelper.Initialize();

            await Window.Current.Dispatcher.RunIdleAsync(async (s) => await BackgroundDownloadHelper.AttachToDownloadsAsync());
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
                taskDef.Complete();
            };
        }
    }
}
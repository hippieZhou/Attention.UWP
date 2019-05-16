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
using HENG.Models;
using Microsoft.Extensions.Configuration;
using Windows.Storage;

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
            await InitializeAsync();

            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                ExtendedSplash extendedSplash = new ExtendedSplash(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

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

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
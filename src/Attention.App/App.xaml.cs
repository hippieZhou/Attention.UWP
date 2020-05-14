using Attention.App.Events;
using Attention.App.Extensions;
using Attention.App.Models;
using Attention.App.Services;
using Attention.App.ViewModels.UcViewModels;
using Attention.App.Views;
using Attention.Core;
using Attention.Core.Context;
using Attention.Core.Framework;
using Attention.Core.Services;
using Microsoft.Practices.Unity;
using Microsoft.Toolkit.Uwp.Helpers;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Attention.App
{
    public sealed partial class App : PrismUnityApplication
    {
        public static AppSettings Settings => Current.Resources["AppSettings"] as AppSettings;

        public App()
        {
            CoreApplication.EnablePrelaunch(true);

            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.File(path: Path.Combine(ApplicationData.Current.LocalFolder.Path, "logs", "log.txt"),
                encoding: Encoding.UTF8,
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Warning)
                .CreateLogger();

            Logger = new SerilogLoggerFacade(Log.Logger, Logger);
            Log.Information("系统已启动。");

            InitializeComponent();
            ExtendedSplashScreenFactory = (splashscreen) => new ExtendedSplashScreen(splashscreen);
            UnhandledException += (sender, e) =>
            {
                EventAggregator.GetEvent<RaisedExceptionEvent>().Publish(e.Exception);
                e.Handled = true;
            };
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await LoadAppResources();
            }

            if (!args.PrelaunchActivated)
            {
                NavigationService.NavigateToPage<ShellPage>(args.Arguments);
            }

            Window.Current.Activate();
        }

        private async Task LoadAppResources()
        {
            #region Database Initialize
            if (!(await Settings.LocalFolder.TryGetItemAsync(AppSettings.DBFile) is IStorageFile dbFile))
            {
                var dbContext = Container.Resolve<IApplicationDbContext>();
                ApplicationDbInitializer.Migrate(dbContext);
            }
            #endregion

            #region Application Configuration
            ApplicationLanguages.PrimaryLanguageOverride = Settings.Language;
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = Settings.Theme;
            }

            var uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += async (sender, e) =>
            {
                Color bg = sender.GetColorValue(UIColorType.Accent);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                    var isDarkTheme = Settings.Theme == ElementTheme.Default ? Application.Current.RequestedTheme == ApplicationTheme.Dark : Settings.Theme == ElementTheme.Dark;
                    titleBar.ButtonForegroundColor = isDarkTheme ? (Color?)Colors.White : (Color?)Colors.Black;
                }, Windows.UI.Core.CoreDispatcherPriority.Normal);
            };

            Settings.EnableCompact(Settings.IsCompact);
            Settings.EnableLiveTitle(Settings.IsLiveTitle);
            Settings.EnableSound(Settings.SoundPlayerState);
            #endregion

            await Task.Yield();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance(Logger);
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance(EventAggregator);
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            Container.RegisterApplicationCore(Path.Combine(Settings.LocalFolder.Path, AppSettings.DBFile));

            Container.RegisterType<PickedPaneViewModel, PickedSearchViewModel>(nameof(PickedSearchViewModel));
            Container.RegisterType<PickedPaneViewModel, PickedDownloadViewModel>(nameof(PickedDownloadViewModel));
            Container.RegisterType<PickedPaneViewModel, PickedSettingsViewModel>(nameof(PickedSettingsViewModel));

            Container.RegisterType<IDownloadService, DownloadService>();

            EnginContext.Initialize(new GeneralEngine(Container));
            return base.OnInitializeAsync(args);
        }
    }
}

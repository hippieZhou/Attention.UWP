using Prism.Unity.Windows;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using Prism.Windows.AppModel;
using Windows.ApplicationModel.Resources;
using Attention.App.Extensions;
using Serilog;
using Attention.App.Views;
using Attention.App.Services;
using AutoMapper;
using Attention.App.Models;
using System.Text;
using Serilog.Events;
using Windows.Storage;
using System.IO;
using Windows.Globalization;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.Unity;
using Attention.Core.Framework;
using Attention.Core.Context;
using Attention.Core.Uow;

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
                Logger.Log(e.Exception.ToString(), Prism.Logging.Category.Exception, Prism.Logging.Priority.High);
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

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            void ExtendTitlebar()
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                ApplicationView appView = ApplicationView.GetForCurrentView();
                appView.TitleBar.BackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonForegroundColor = Colors.DarkGray;
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                appView.TitleBar.InactiveBackgroundColor = Colors.Transparent;
            }

            ExtendTitlebar();

            //ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = false;

            base.OnWindowCreated(args);
        }

        private async Task LoadAppResources()
        {
            #region Database Initialize
            if (!(await Settings.LocalFolder.TryGetItemAsync(AppSettings.DBFile) is IStorageFile dbFile))
            {
                using (var dbContext = Container.Resolve<ApplicationDbContext>())
                {
                    dbContext.DbFilePath = Path.Combine(Settings.LocalFolder.Path, AppSettings.DBFile);
                    dbContext.Database.Migrate();
                }
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
                    //todo
                }, Windows.UI.Core.CoreDispatcherPriority.Normal);
            };
            #endregion

            EnginContext.Initialize(new GeneralEngine(Container));
            await Task.Yield();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance(Logger);
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance(EventAggregator);
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            Container.RegisterType<ApplicationDbContext>();
            Container.RegisterType<IDateTime, MachineDateTime>();
            Container.RegisterType(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            Container.RegisterType<IUnitOfWork, UnitOfWork>();

            Container.RegisterInstance<IMapper>(new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PixabayMappingProfile>();
                cfg.AddProfile<UnsplashMappingProfile>();
            })));
            Container.RegisterInstance<IWallpaperService>(nameof(PixabayService), new PixabayService("12645414-59a5251905dfea7b916dd796f"));
            Container.RegisterInstance<IWallpaperService>(nameof(UnsplashService), new UnsplashService("xtU9WrbC5zUgMhkHAoNnq1La-vaVZYa8pxMtf-XiLgU"));
            Container.RegisterInstance(new AppNotification(), new ContainerControlledLifetimeManager());
            return base.OnInitializeAsync(args);
        }
    }
}

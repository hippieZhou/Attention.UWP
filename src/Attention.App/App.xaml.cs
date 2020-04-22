using Prism.Unity.Windows;
using Microsoft.Practices.Unity;
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
using System.IO;
using Windows.Storage;
using System.Text;
using Serilog.Events;
using Attention.App.Views;
using Attention.App.Services;
using Attention.App.Framework;

namespace Attention.App
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
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
            NavigationService.NavigateToPage<ShellPage>(args.Arguments);
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
            base.OnWindowCreated(args);
        }

        private async Task LoadAppResources()
        {
            EnginContext.Initialize(new GeneralEngine(this.Container));
            //return Task.Delay(2000);
            await Task.Yield();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance(Logger);
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance(EventAggregator);
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterInstance<IWallpaperService>(nameof(PixabayService), new PixabayService("12645414-59a5251905dfea7b916dd796f"));
            Container.RegisterInstance<IWallpaperService>(nameof(UnsplashService), new UnsplashService("12645414-59a5251905dfea7b916dd796f"));
            return base.OnInitializeAsync(args);
        }
    }
}

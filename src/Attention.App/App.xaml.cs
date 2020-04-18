using Prism.Unity.Windows;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace Attention.App
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
            ExtendedSplashScreenFactory = (splashscreen) => new ExtendedSplashScreen(splashscreen);
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await LoadAppResources();
            }
            NavigationService.Navigate(PageTokens.Shell.ToString(), args.Arguments);
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
            //return Task.Delay(2000);
            await Task.Yield();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance(EventAggregator);
            //Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            return base.OnInitializeAsync(args);
        }
    }
}

using Attention.Commons;
using Attention.ViewModels;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Attention
{
    sealed partial class App : Application
    {
        public static string API_KEY => Current.Resources["API-KEY"] as string;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //new BackgroundProxy().Register();
        }

        protected override void OnLaunchedAsync(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    bool loadState = e.PreviousExecutionState == ApplicationExecutionState.Terminated;
                    rootFrame.Content = new ExtendedSplashScreen(e.SplashScreen, loadState); ;
                }
                Window.Current.Activate();
            }
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

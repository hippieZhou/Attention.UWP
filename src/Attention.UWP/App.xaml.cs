using Attention.UWP.Models;
using Attention.UWP.ViewModels;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Attention.UWP
{
    sealed partial class App : Application
    {
        public static AppSettings Settings => Current.Resources["AppSettings"] as AppSettings;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
            InitializeContent(e.Arguments);

        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            InitializeContent(args);
            if (args.Kind == ActivationKind.ToastNotification)
            {
                ViewModelLocator.Current.Main.PhotoGridHeaderViewModel.DownloadCommand.Execute(null);
            }
        }

        private void InitializeContent(object args)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(ShellPage), args);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

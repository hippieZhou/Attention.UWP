using Attention.UWP.Helpers;
using Attention.UWP.Models;
using Attention.UWP.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Attention.UWP
{
    sealed partial class App : Application
    {
        public static AppSettings Settings => Current.Resources["AppSettings"] as AppSettings;
        public static string API_KEY;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            new BackgroundProxy().Register();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
            await InitializeContentAsync(e.Arguments);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            await InitializeContentAsync(args);

            if (args.Kind == ActivationKind.ToastNotification)
            {
                ViewModelLocator.Current.Main.PhotoGridHeaderViewModel.DownloadCommand.Execute(null);
            }
        }

        private async Task InitializeContentAsync(object args)
        {
            await LoadSecretAsync(true);

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

            SetupTitlebar();
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async Task LoadSecretAsync(bool release = false)
        {
            StorageFile secret = await StorageFile.GetFileFromPathAsync(Settings.SecretFile);
            string json = await FileIO.ReadTextAsync(secret);
            API_KEY key = JsonConvert.DeserializeObject<JObject>(json)[nameof(API_KEY)].ToObject<API_KEY>();
            API_KEY = release ? key?.Release : key?.Debug;
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

        private void SetupTitlebar()
        {
            ElementTheme TrueTheme()
            {
                var frameworkElement = Window.Current.Content as FrameworkElement;
                return frameworkElement.ActualTheme;
            }

            void SetTitleBarColors()
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    if (TrueTheme() == ElementTheme.Dark)
                    {
                        titleBar.ButtonForegroundColor = Colors.White;
                        titleBar.ForegroundColor = Colors.White;
                    }
                    else
                    {
                        titleBar.ButtonForegroundColor = Colors.Black;
                        titleBar.ForegroundColor = Colors.Black;
                    }

                    titleBar.BackgroundColor = Colors.Black;

                    titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    titleBar.ButtonInactiveForegroundColor = Colors.LightGray;
                }
            }

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                SetTitleBarColors();
                CoreApplicationViewTitleBar coreTitleBar = TitleBarHelper.Instance.TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;

                Messenger.Default.Register<ElementTheme>(this, nameof(ElementTheme), theme =>
                {
                    SetTitleBarColors();
                });

                new UISettings().ColorValuesChanged += async (_s, _e) =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(SetTitleBarColors);
                };
            }
        }
    }
}

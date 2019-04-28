using GalaSoft.MvvmLight.Threading;
using HENG.Views;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG
{
    public sealed partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect; 
        internal bool dismissed = false; 
        internal Frame rootFrame;

        private SplashScreen splash; 
        private double ScaleFactor; 

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += ExtendedSplash_OnResize;
            ScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            splash = splashscreen;

            if (splash != null)
            {
                splash.Dismissed += DismissedEventHandler;
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            rootFrame = new Frame();
            RestoreStateAsync(loadState);
        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        private async void DismissedEventHandler(SplashScreen sender, object args)
        {
            dismissed = true;
            await Task.Delay(1000);

            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                rootFrame.Navigate(typeof(ShellPage));
                Window.Current.Content = rootFrame;
                await App.StartupAsync();
            });
        }

        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                extendedSplashImage.Height = splashImageRect.Height / ScaleFactor;
                extendedSplashImage.Width = splashImageRect.Width / ScaleFactor;
            }
            else
            {
                extendedSplashImage.Height = splashImageRect.Height;
                extendedSplashImage.Width = splashImageRect.Width;
            }
        }

        private void RestoreStateAsync(bool loadState)
        {
            if (loadState)
            {
                //await SuspensionManager.RestoreAsync();
            }
        }
    }
}

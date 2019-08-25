using Attention.ViewModels;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention
{
    public partial class ExtendedSplashScreen
    {
        private readonly SplashScreen _splash;
        private readonly bool _loadState;
        private readonly double ScaleFactor;

        private Rect splashImageRect;
        private bool dismissed = false;

        public ExtendedSplashScreenViewModel ViewModel => ViewModelLocator.Current.GetRequiredService<ExtendedSplashScreenViewModel>();

        public ExtendedSplashScreen(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();
            ScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            _splash = splashscreen;
            if (_splash != null)
            {
                _splash.Dismissed += OnSplashDismissed;
                splashImageRect = _splash.ImageLocation;
                PositionImage();
            }
            _loadState = loadState;

            Window.Current.SizeChanged += OnWindowSizeChanged;
            Loaded += OnLoaded;
        }

        private void OnSplashDismissed(SplashScreen sender, object args)
        {
            dismissed = true;
        }

        private void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (_splash != null)
            {
                splashImageRect = _splash.ImageLocation;
                PositionImage();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadedCommand.Execute(new Action(() =>
            {
                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(ShellPage));
                ((ShellPage)rootFrame.Content).SetExtendedSplashInfo(splashImageRect, dismissed);
                Window.Current.Content = rootFrame;
            }));
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
    }
}

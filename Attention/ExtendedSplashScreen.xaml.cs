using Attention.Commons;
using Attention.ViewModels;
using System;
using System.Threading.Tasks;
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

        public ExtendedSplashScreen(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();

            ScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            _splash = splashscreen;
            _loadState = loadState;

            if (_splash != null)
            {
                _splash.Dismissed += (sender, args) =>
                {
                    if (_loadState)
                    {
                    }
                };
                PositionImage(_splash.ImageLocation);
            }

            Window.Current.SizeChanged += (sender, e) => 
            {
                if (_splash != null)
                {
                    PositionImage(_splash.ImageLocation);
                }
            };

            Loaded += async (sender, e) => 
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(ShellPage));
                Window.Current.Content = rootFrame;

                await ViewModelLocator.Current.AppSettings.InitializeAsync();
                TitleBarHelper.Instance.RefreshTitleBar();
            };
        }

        private void PositionImage(Rect splashRect)
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                extendedSplashImage.Height = splashRect.Height / ScaleFactor;
                extendedSplashImage.Width = splashRect.Width / ScaleFactor;
            }
            else
            {
                extendedSplashImage.Height = splashRect.Height;
                extendedSplashImage.Width = splashRect.Width;
            }
        }
    }
}

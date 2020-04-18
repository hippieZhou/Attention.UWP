using Windows.ApplicationModel.Activation;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.App
{
    public sealed partial class ExtendedSplashScreen : Page
    {
        private readonly SplashScreen _splash;
        private readonly double _scaleFactor;

        public ExtendedSplashScreen(SplashScreen splashScreen)
        {
            _splash = splashScreen;
            _scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            this.InitializeComponent();

            SizeChanged += (sender, e) => 
            {
                Resize();
            };
            splashImage.ImageOpened += (sender, e) => 
            {
                Resize();
                Window.Current.Activate();
            };
        }

        private void Resize()
        {
            if (_splash == null) return;

            var splashImageRect = _splash.ImageLocation;

            splashImage.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            splashImage.SetValue(Canvas.TopProperty, splashImageRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                splashImage.Height = splashImageRect.Height / _scaleFactor;
                splashImage.Width = splashImageRect.Width / _scaleFactor;
            }
            else
            {
                splashImage.Height = splashImageRect.Height;
                splashImage.Width = splashImageRect.Width;
            }

            progressRing.SetValue(Canvas.TopProperty, _splash.ImageLocation.Top + _splash.ImageLocation.Height + 50);
            progressRing.SetValue(Canvas.LeftProperty, _splash.ImageLocation.Left + _splash.ImageLocation.Width / 2 - progressRing.Width / 2);
        }
    }
}

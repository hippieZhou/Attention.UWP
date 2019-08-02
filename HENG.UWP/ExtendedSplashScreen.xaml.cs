using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP
{
    public sealed partial class ExtendedSplashScreen : Page
    {
        private Rect _splashImageBounds;

        public ExtendedSplashScreen(SplashScreen splashScreen)
        {
            InitializeComponent();

            if (splashScreen != null)
            {
                _splashImageBounds = splashScreen.ImageLocation;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage(_splashImageBounds);

            if (!(Window.Current.Content is Frame rootFrame))
            {
                Window.Current.Content = rootFrame = new Frame();
            }

            rootFrame.Content = page;
        }
    }
}

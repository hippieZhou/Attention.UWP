using Attention.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Attention
{
    public sealed partial class ShellPage : Page
    {
        internal bool Dismissed;
        internal Rect SplashImageRect;

        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
        public void SetExtendedSplashInfo(Rect splashRect, bool dismissStat)
        {
            SplashImageRect = splashRect;
            Dismissed = dismissStat;
        }
    }
}

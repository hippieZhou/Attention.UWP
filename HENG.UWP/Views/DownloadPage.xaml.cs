using HENG.UWP.ViewModels;
using Windows.UI.Xaml.Controls;


namespace HENG.UWP.Views
{

    public sealed partial class DownloadPage : Page
    {
        public DownloadViewModel ViewModel => ViewModelLocator.Current.Download;

        public DownloadPage()
        {
            this.InitializeComponent();
        }
    }
}

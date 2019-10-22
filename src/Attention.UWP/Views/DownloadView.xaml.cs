using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.Views
{
    public sealed partial class DownloadView : UserControl
    {
        public DownloadViewModel ViewModel => ViewModelLocator.Current.Download;
        public DownloadView()
        {
            this.InitializeComponent();
        }
    }
}

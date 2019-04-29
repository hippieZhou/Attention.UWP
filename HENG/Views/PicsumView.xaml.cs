using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{
    public sealed partial class PicsumView : Page
    {
        PicsumViewModel ViewModel => ViewModelLocator.Current.Picsum;
        public PicsumView()
        {
            this.InitializeComponent();
        }
    }
}

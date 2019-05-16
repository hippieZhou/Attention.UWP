using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{
    public sealed partial class LocalView : Page
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;

        public LocalView()
        {
            this.InitializeComponent();
        }
    }
}

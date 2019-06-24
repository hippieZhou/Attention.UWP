using HENG.App.ViewModels;
using Windows.UI.Xaml.Controls;


namespace HENG.App.Views
{
    public sealed partial class LocalView : UserControl
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;
        public LocalView()
        {
            this.InitializeComponent();
        }
    }
}

using HENG.App.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.App.Views
{
    public sealed partial class MoreView : UserControl
    {
        public MoreViewModel ViewModel => ViewModelLocator.Current.More;
        public MoreView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}

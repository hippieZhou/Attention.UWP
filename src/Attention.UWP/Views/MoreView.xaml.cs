using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Attention.UWP.Views
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

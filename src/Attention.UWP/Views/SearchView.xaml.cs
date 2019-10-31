using Attention.UWP.ViewModels;
using Windows.UI.Xaml.Controls;


namespace Attention.UWP.Views
{
    public sealed partial class SearchView : ContentDialog
    {
        public SearchViewModel ViewModel => ViewModelLocator.Current.Search;
        public SearchView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}

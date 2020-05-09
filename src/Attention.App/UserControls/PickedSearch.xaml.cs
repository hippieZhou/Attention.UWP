using Attention.App.ViewModels.UcViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.App.UserControls
{
    public sealed partial class PickedSearch : UserControl
    {
        public PickedSearchViewModel ConcreteDataContext { get; set; }
        public PickedSearch()
        {
            this.InitializeComponent();
            DataContextChanged += (sender, e) => ConcreteDataContext = DataContext as PickedSearchViewModel;
        }

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Translation = new Vector3(0, 0, 16);
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Translation = new Vector3(0, 0, 4);
        }

    }
}

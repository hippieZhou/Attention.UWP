using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.UserControls
{
    public sealed partial class FilterControl : UserControl
    {
        public FilterControl()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        public FilterViewModel ViewModel
        {
            get { return (FilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(FilterViewModel), typeof(FilterControl), new PropertyMetadata(null));
    }
}

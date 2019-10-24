using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.Views
{
    public sealed partial class MoreView : UserControl
    {
        public MoreView()
        {
            this.InitializeComponent();
        }

        public MoreViewModel ViewModel
        {
            get { return (MoreViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MoreViewModel), typeof(MoreViewModel), new PropertyMetadata(null));
    }
}

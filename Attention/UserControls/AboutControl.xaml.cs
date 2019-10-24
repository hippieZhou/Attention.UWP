using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.UserControls
{
    public sealed partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        public AboutViewModel ViewModel
        {
            get { return (AboutViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(AboutViewModel), typeof(AboutControl), new PropertyMetadata(null));
    }
}

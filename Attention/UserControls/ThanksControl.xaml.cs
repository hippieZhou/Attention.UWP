using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.UserControls
{
    public sealed partial class ThanksControl : UserControl
    {
        public ThanksControl()
        {
            this.InitializeComponent();
        }

        public ThanksViewModel ViewModel
        {
            get { return (ThanksViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ThanksViewModel), typeof(ThanksControl), new PropertyMetadata(null));
    }
}

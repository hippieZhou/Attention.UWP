using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.UserControls
{
    public sealed partial class ToastControl : UserControl
    {
        public ToastControl()
        {
            this.InitializeComponent();
        }

        public string Toast
        {
            get { return (string)GetValue(ToastProperty); }
            set { SetValue(ToastProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Toast.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToastProperty =
            DependencyProperty.Register("Toast", typeof(string), typeof(ToastControl), new PropertyMetadata(null));
    }
}

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.UserControls
{
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            this.InitializeComponent();
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HeaderControl), new PropertyMetadata(null));

        public ICommand NavBackCommand
        {
            get { return (ICommand)GetValue(NavBackCommandProperty); }
            set { SetValue(NavBackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavBackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavBackCommandProperty =
            DependencyProperty.Register("NavBackCommand", typeof(ICommand), typeof(HeaderControl), new PropertyMetadata(null));
    }
}

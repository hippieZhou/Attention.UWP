using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.App.UserControls
{
    public sealed partial class TitleBarControl : UserControl
    {
        public TitleBarControl()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Blur(30, 0, 0).Start();
        }

        public ICommand OpenPaneCommand
        {
            get { return (ICommand)GetValue(OpenPaneCommandProperty); }
            set { SetValue(OpenPaneCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OpenPaneCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenPaneCommandProperty =
            DependencyProperty.Register("OpenPaneCommand", typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
    }
}

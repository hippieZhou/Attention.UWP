using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.Controls
{
    public sealed class DownloadControl : Control
    {
        public DownloadControl()
        {
            this.DefaultStyleKey = typeof(DownloadControl);
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DownloadControl), new PropertyMetadata(null));

    }
}

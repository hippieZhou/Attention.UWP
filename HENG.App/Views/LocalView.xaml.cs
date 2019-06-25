using HENG.App.ViewModels;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.App.Views
{
    public sealed partial class LocalView : UserControl
    {
        public LocalViewModel ViewModel => ViewModelLocator.Current.Local;
        public LocalView()
        {
            this.InitializeComponent();
        }

        public ICommand NavToBackCommand
        {
            get { return (ICommand)GetValue(NavToBackCommandProperty); }
            set { SetValue(NavToBackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavToNackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavToBackCommandProperty =
            DependencyProperty.Register("NavToNackCommand", typeof(ICommand), typeof(LocalView), new PropertyMetadata(null));
    }
}

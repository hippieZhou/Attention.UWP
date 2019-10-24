using HENG.UWP.ViewModels;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.UWP.Views
{
    public sealed partial class SettingView : UserControl
    {
        public SettingViewModel ViewModel => ViewModelLocator.Current.Setting;

        public SettingView()
        {
            this.InitializeComponent();
        }

        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register("BackCommand", typeof(ICommand), typeof(SettingView), new PropertyMetadata(null));
    }
}

using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{

    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel => ViewModelLocator.Current.Settings;

        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}

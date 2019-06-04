using HENG.ViewModels;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{

    public sealed partial class SettingsView : Page
    {
        public SettingsViewModel ViewModel => ViewModelLocator.Current.Settings;

        public SettingsView()
        {
            this.InitializeComponent();
        }
    }
}

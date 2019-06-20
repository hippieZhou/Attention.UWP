using HENG.ViewModels;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace HENG.UserControls
{
    public sealed partial class SettingsControl : UserControl
    {
        public SettingsViewModel ViewModel => ViewModelLocator.Current.Settings;
        public ICommand BackCommand => ViewModelLocator.Current.Shell.BackCommand;
        public SettingsControl()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}

using HENG.ViewModels;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;


namespace HENG.UserControls
{
    public sealed partial class NavBackControl : UserControl
    {
        public ICommand BackCommand => ViewModelLocator.Current.Shell.NavBackCommand;
        public NavBackControl()
        {
            this.InitializeComponent();
        }
    }
}

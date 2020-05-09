using Attention.App.ViewModels.UcViewModels;
using Windows.UI.Xaml.Controls;


namespace Attention.App.UserControls
{
    public sealed partial class PickedSettings : UserControl
    {
        public PickedSettingsViewModel ConcreteDataContext { get; set; }

        public PickedSettings()
        {
            this.InitializeComponent();
            DataContextChanged += (sender, e) => ConcreteDataContext = DataContext as PickedSettingsViewModel;
        }
    }
}

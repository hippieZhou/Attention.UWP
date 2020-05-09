using Attention.App.ViewModels.UcViewModels;
using Windows.UI.Xaml.Controls;


namespace Attention.App.UserControls
{
    public sealed partial class PickedDownload : UserControl
    {
        public PickedDownloadViewModel ConcreteDataContext { get; set; }
        public PickedDownload()
        {
            this.InitializeComponent();
            DataContextChanged += (sender, e) => ConcreteDataContext = DataContext as PickedDownloadViewModel;
        }
    }
}

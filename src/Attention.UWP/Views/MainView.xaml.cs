using Attention.UWP.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Numerics;
using Windows.UI.Xaml.Controls;


namespace Attention.UWP.Views
{
    public sealed partial class MainView : UserControl
    {
        public MainViewModel ViewModel => ViewModelLocator.Current.Main;
        public MainView()
        {
            this.InitializeComponent();
        }
    }
}

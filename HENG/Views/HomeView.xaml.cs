using HENG.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HENG.Views
{
    public sealed partial class HomeView : Page
    {
        public HomeViewModel ViewModel => ViewModelLocator.Current.Home;
        public HomeView()
        {
            this.InitializeComponent();
        }
    }
}

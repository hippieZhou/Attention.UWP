using Attention.App.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Windows.Mvvm;
using System.ComponentModel;

namespace Attention.App.Views
{
    public sealed partial class HomePage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public const string connectedElement = "connectedElement";

        public HomePageViewModel ConcreteDataContext => DataContext as HomePageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;
        public HomePage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (sender, e) => 
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }

        private void AdaptiveGridView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (sender is AdaptiveGridView container && container.SelectedItem != null)
            {
                container.ScrollIntoView(container.SelectedItem);
            }
        }
    }
}

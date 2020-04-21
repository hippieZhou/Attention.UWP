using Attention.App.ViewModels;
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
    }
}

using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;

namespace Attention.App.Views
{
    public sealed partial class MainPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public MainPageViewModel ConcreteDataContext => DataContext as MainPageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (sender, e) => 
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }
    }
}

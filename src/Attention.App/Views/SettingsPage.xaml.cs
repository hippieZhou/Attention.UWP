using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;

namespace Attention.App.Views
{
    public sealed partial class SettingsPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public SettingsPageViewModel ConcreteDataContext => DataContext as SettingsPageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;
        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }
    }
}

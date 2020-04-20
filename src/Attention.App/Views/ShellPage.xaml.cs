using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;

namespace Attention.App.Views
{
    public sealed partial class ShellPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public ShellPageViewModel ConcreteDataContext => DataContext as ShellPageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ShellPage()
        {
            this.InitializeComponent();
            ConcreteDataContext.Initialize(shellFrame);
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }
    }
}

using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Attention.App.Views
{
    public sealed partial class ShellPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public ShellPageViewModel ConcreteDataContext => DataContext as ShellPageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ShellPage()
        {
            this.InitializeComponent();
            ConcreteDataContext.Initialize(shellNav, shellFrame);
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }
    }
}

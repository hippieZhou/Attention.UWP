using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Attention.App.Views
{
    public sealed partial class ShellPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public ShellPageViewModel ConcreteDataContext => DataContext as ShellPageViewModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ShellPage()
        {
            this.InitializeComponent();
            ConcreteDataContext.Initialize(shellNav,shellFrame);
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }
    }
}

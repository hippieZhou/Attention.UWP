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

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Translation = new Vector3(0, 0, 16);
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Translation = new Vector3(0, 0, 4);
        }

    }
}

using Attention.App.ViewModels;
using Prism.Windows.Mvvm;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Attention.App.Views
{
    public sealed partial class DownloadPage : SessionStateAwarePage, INotifyPropertyChanged
    {
        public DownloadPageViewModel ConcreteDataContext => DataContext as DownloadPageViewModel;
        public DownloadPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

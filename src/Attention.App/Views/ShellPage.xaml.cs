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
            this.DataContextChanged += (sender, e) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }
    }

    [ContentProperty(Name = "ItemTemplate")]
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SeparatorTemplate { get; set; }
        public DataTemplate HeaderTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item is Separator ? SeparatorTemplate : item is Header ? HeaderTemplate : ItemTemplate;
        }
    }
}

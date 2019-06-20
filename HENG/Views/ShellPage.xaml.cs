using HENG.ViewModels;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{
    /// <summary>
    /// http://meanme.com/2017/07/11/sticky-header/
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.Shell;
        public ICommand RefreshCommand => ViewModelLocator.Current.Photo.RefreshCommand;
        public ShellPage()
        {
            this.InitializeComponent();
            ViewModel.Initialize(contentControl);
        }
    }

    public class ContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HomeTemplate { get; set; }
        public DataTemplate LocalTemplate { get; set; }
        public DataTemplate SettingsTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case LocalViewModel _:
                    return LocalTemplate;
                case SettingsViewModel _:
                    return SettingsTemplate;
                default:
                    return base.SelectTemplateCore(item, container);
            }
        }
    }
}

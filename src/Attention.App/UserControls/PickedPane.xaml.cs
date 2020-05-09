using Attention.App.ViewModels.UcViewModels;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.App.UserControls
{
    public sealed partial class PickedPane : UserControl
    {
        public PickedPane()
        {
            this.InitializeComponent();
        }

        public ICommand DismissCommand
        {
            get { return (ICommand)GetValue(DismissCommandProperty); }
            set { SetValue(DismissCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DismissCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DismissCommandProperty =
            DependencyProperty.Register("DismissCommand", typeof(ICommand), typeof(PickedPane), new PropertyMetadata(default));


        public PickedPaneViewModel ViewModel
        {
            get { return (PickedPaneViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PickedPaneViewModel), typeof(PickedPane), new PropertyMetadata(default));
    }

    public class PickedDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Search { get; set; }
        public DataTemplate Download { get; set; }
        public DataTemplate Settings { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var dataTemplate = base.SelectTemplateCore(item, container);

            if (item is PickedPaneViewModel viewModel)
            {
                switch (viewModel.PaneType)
                {
                    case PaneTypes.Search:
                        dataTemplate = Search;
                        break;
                    case PaneTypes.Download:
                        dataTemplate = Download;
                        break;
                    case PaneTypes.Settings:
                        dataTemplate = Settings;
                        break;
                }
            }
            return dataTemplate;
        }
    }
}

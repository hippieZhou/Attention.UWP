using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Attention.UserControls
{
    public sealed partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        public SettingViewMode ViewModel
        {
            get { return (SettingViewMode)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SettingViewMode), typeof(SettingControl), new PropertyMetadata(null));

        private void ThemeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton btn)
            {
                ViewModel.ChangedThemeCommand.Execute(btn.DataContext);
            }
        }

        private void LanguageRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton btn)
            {
                ViewModel.ChangedLanguageCommand.Execute(btn.DataContext);
            }
        }
    }
}

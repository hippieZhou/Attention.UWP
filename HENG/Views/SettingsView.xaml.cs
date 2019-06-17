using HENG.ViewModels;
using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.Views
{

    public sealed partial class SettingsView : Page
    {
        public SettingsViewModel ViewModel => ViewModelLocator.Current.Settings;

        public SettingsView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}

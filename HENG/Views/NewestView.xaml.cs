using HENG.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HENG.Views
{
    public sealed partial class NewestView : Page
    {
        public NewestViewModel ViewModel => ViewModelLocator.Current.Newest;
        public NewestView()
        {
            this.InitializeComponent();
        }
    }
}

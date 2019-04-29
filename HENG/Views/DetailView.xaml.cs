using HENG.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
    public sealed partial class DetailView : Page
    {
        public DetailViewModel ViewModel => ViewModelLocator.Current.Detail;
        public DetailView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        public object Photo
        {
            get { return (object)GetValue(PhotoProperty); }
            set { SetValue(PhotoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Photo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoProperty =
            DependencyProperty.Register("Photo", typeof(object), typeof(DetailView), new PropertyMetadata(null,(d,e)=> 
            {
                if (d is DetailView handler)
                {
                    handler.ViewModel.Model = e.NewValue;
                    handler.ViewModel.LoadedCommand.Execute(null);
                }
            }));

        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register("BackCommand", typeof(ICommand), typeof(DetailView), new PropertyMetadata(null));
    }
}

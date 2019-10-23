﻿using Attention.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.Views
{
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            this.InitializeComponent();
        }

        public SearchViewModel ViewModel
        {
            get { return (SearchViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SearchViewModel), typeof(SearchView), new PropertyMetadata(null));
    }
}
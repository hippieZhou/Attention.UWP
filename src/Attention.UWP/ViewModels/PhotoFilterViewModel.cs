using Attention.UWP.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Orientation = PixabaySharp.Enums.Orientation;

namespace Attention.UWP.ViewModels
{
    public class PhotoFilterViewModel : ViewModelBase
    {
        public IEnumerable<Order> Orders => Enum.GetValues(typeof(Order)).Cast<Order>();
        public IEnumerable<Orientation> Orientations => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();
        public IEnumerable<ImageType> ImageTypes => Enum.GetValues(typeof(ImageType)).Cast<ImageType>();
        public IEnumerable<Category> Categories => Enum.GetValues(typeof(Category)).Cast<Category>();

        public Filter Filter { get; private set; }

        public PhotoFilterViewModel()
        {
            Filter = App.Settings.Filter;
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(() =>
                    {
                        ViewModelLocator.Current.Shell.IsPaneOpen = false;
                    });
                }
                return _backCommand;
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand<AutoSuggestBoxQuerySubmittedEventArgs>(async args =>
                    {
                        Filter.Query = args.QueryText;
                        await RefreshWallPapersAsync();
                    });
                }
                return _searchCommand;
            }
        }

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                if (_selectionChangedCommand == null)
                {
                    _selectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(async args =>
                    {
                       await RefreshWallPapersAsync();
                    });
                }
                return _selectionChangedCommand;
            }
        }

        private async Task RefreshWallPapersAsync()
        {
            App.Settings.Filter = Filter;
            await ViewModelLocator.Current.Main.PhotoGridViewModel.Items.RefreshAsync();
        }
    }
}

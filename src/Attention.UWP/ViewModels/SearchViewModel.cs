using Attention.UWP.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Orientation = PixabaySharp.Enums.Orientation;

namespace Attention.UWP.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        public IEnumerable<Order> Orders => Enum.GetValues(typeof(Order)).Cast<Order>();
        public IEnumerable<Orientation> Orientations => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();
        public IEnumerable<ImageType> ImageTypes => Enum.GetValues(typeof(ImageType)).Cast<ImageType>();
        public IEnumerable<Category> Categories => Enum.GetValues(typeof(Category)).Cast<Category>();

        public Filter Filter { get; private set; }

        public SearchViewModel()
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
                    });
                }
                return _backCommand;
            }
        }

        private ICommand _primaryButtonCommand;
        public ICommand PrimaryButtonCommand
        {
            get
            {
                if (_primaryButtonCommand == null)
                {
                    _primaryButtonCommand = new RelayCommand(() =>
                    {
                        App.Settings.Filter = Filter;
                        ViewModelLocator.Current.Main.PhotoGridViewModel.RefreshCommand.Execute(null);
                    });
                }
                return _primaryButtonCommand;
            }
        }

        private ICommand _closeButtonCommand;
        public ICommand CloseButtonCommand
        {
            get
            {
                if (_closeButtonCommand == null)
                {
                    _closeButtonCommand = new RelayCommand(() =>
                    {

                    });
                }
                return _closeButtonCommand;
            }
        }
    }
}

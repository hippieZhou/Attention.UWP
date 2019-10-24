using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.UWP.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace HENG.UWP.ViewModels
{
    public class PhotoSearchViewModel : ViewModelBase
    {
        private List<ParameterModel> _items;
        public List<ParameterModel> Items
        {
            get { return _items ?? (_items = new List<ParameterModel>()); }
            set { Set(ref _items, value); }
        }

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(ref _visibility, value); }
        }

        private ICommand _hideCommand;
        public ICommand HideCommand
        {
            get
            {
                if (_hideCommand == null)
                {
                    _hideCommand = new RelayCommand(() =>
                    {
                        Visibility = Visibility.Collapsed;
                    });
                }
                return _hideCommand;
            }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        var items = ViewModelLocator.Current.Pix.GetOptionalParameters();
                        Items.AddRange(items);
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

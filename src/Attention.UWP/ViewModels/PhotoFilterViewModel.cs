using Attention.UWP.Models;
using Attention.UWP.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PixabaySharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Orientation = PixabaySharp.Enums.Orientation;

namespace Attention.UWP.ViewModels
{
    public class PhotoFilterViewModel : ViewModelBase
    {
        private readonly PixabayService _service;

        public IEnumerable<Order> Orders => Enum.GetValues(typeof(Order)).Cast<Order>();
        public IEnumerable<Orientation> Orientations => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();
        public IEnumerable<ImageType> ImageTypes => Enum.GetValues(typeof(ImageType)).Cast<ImageType>();
        public IEnumerable<Category> Categories => Enum.GetValues(typeof(Category)).Cast<Category>();
        public IEnumerable<Language> Languages => Enum.GetValues(typeof(Language)).Cast<Language>();

        private bool _isOn = ThemeSelectorService.Theme == ElementTheme.Dark;
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                if (value != _isOn)
                {
                    Set(ref _isOn, value);
                    SwitchThemeCommand.Execute(_isOn ? ElementTheme.Dark : ElementTheme.Light);
                }
            }
        }

        public Filter Filter { get; private set; } 

        public PhotoFilterViewModel(PixabayService service)
        {
            _service = service;
            Filter = _service.DefaultFiler;
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

        private ICommand _switchThemeCommand;
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
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
                        _service.RefreshFilter(Filter);
                        await ViewModelLocator.Current.Main.PhotoGridViewModel.Items.RefreshAsync();
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
                        _service.RefreshFilter(Filter);
                        await ViewModelLocator.Current.Main.PhotoGridViewModel.Items.RefreshAsync();
                    });
                }
                return _selectionChangedCommand;
            }
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.UserControls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.ViewModels
{
    public partial class ShellViewModel : ViewModelBase
    {
        private ContentControl _root;

        private ViewModelBase _controlViewModel;
        public ViewModelBase ControlViewModel
        {
            get { return _controlViewModel; }
            set { Set(ref _controlViewModel, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        var anim = _root.Offset(0, -(float)Math.Max(Window.Current.Bounds.Height, _root.ActualHeight), 0);
                        await anim.StartAsync();
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {

                    _backCommand = new RelayCommand(async () =>
                    {
                        var anim = _root.Offset(0, -(float)Math.Max(Window.Current.Bounds.Height, _root.ActualHeight));
                        await anim.StartAsync();
                    });
                }
                return _backCommand;
            }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new RelayCommand<string>(async pageKey =>
                    {
                        if (pageKey == typeof(LocalViewModel).FullName)
                        {
                            _root.Content = new LocalControl();
                        }
                        if (pageKey == typeof(SettingsViewModel).FullName)
                        {
                            _root.Content = new SettingsControl();
                        }
                        await _root.Offset(0, duration: 1000).StartAsync();
                    });
                }
                return _itemInvokedCommand;
            }
        }

        public void Initialize(ContentControl contentControl)
        {
            _root = contentControl;
        }
    }
}

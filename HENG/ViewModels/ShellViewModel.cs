using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HENG.ViewModels
{
    public partial class ShellViewModel : ViewModelBase
    {
        private ContentControl _rootControl;

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(async () =>
                    {
                        var anim = _rootControl.Offset(0, -(float)Math.Max(Window.Current.Bounds.Height, _rootControl.ActualHeight), 0);
                        await anim.StartAsync();
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _navToCommand;
        public ICommand NavToCommand
        {
            get
            {
                if (_navToCommand == null)
                {
                    _navToCommand = new RelayCommand<string>(async controlKey =>
                    {
                        if (ViewModelLocator.ControlsByKey.TryGetValue(controlKey, out Type type))
                        {
                            _rootControl.Content = Activator.CreateInstance(type);
                            await _rootControl.Offset(0, duration: 1000).StartAsync();
                        }
                    });
                }
                return _navToCommand;
            }
        }

        private ICommand _navBackCommand;
        public ICommand NavBackCommand
        {
            get
            {
                if (_navBackCommand == null)
                {

                    _navBackCommand = new RelayCommand(async () =>
                    {
                        var anim = _rootControl.Offset(0, -(float)Math.Max(Window.Current.Bounds.Height, _rootControl.ActualHeight));
                        await anim.StartAsync();
                    });
                }
                return _navBackCommand;
            }
        }

        public void Initialize(ContentControl contentControl)
        {
            _rootControl = contentControl;
        }
    }
}

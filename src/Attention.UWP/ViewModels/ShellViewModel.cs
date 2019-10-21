using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public PhotoFilterViewModel PhotoFilterViewModel { get; } = new PhotoFilterViewModel();
        /// <summary>
        /// https://www.cnblogs.com/shaomeng/p/8678641.html
        /// https://github.com/r2d2rigo/WinCompositionTiltEffect
        /// https://docs.microsoft.com/en-us/windows/communitytoolkit/animations/scale
        /// </summary>
        public FrameworkElement UiElement { get; private set; }

        private bool _isPaneOpen = false;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<FrameworkElement>(uiElement =>
                    {
                        UiElement = uiElement;
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

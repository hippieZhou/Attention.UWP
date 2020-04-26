using Prism.Commands;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.App.ViewModels.UcViewModels
{
    public class WallpaperExploreViewModel : UcBaseViewModel
    {
        private ICommand _switchCommand;
        public ICommand SwitchCommand
        {
            get
            {
                if (_switchCommand == null)
                {
                    _switchCommand = new DelegateCommand<object>(visibility =>
                    {
                        if (visibility is Visibility vis)
                        {
                            Visibility = vis;
                        }
                    });
                }
                return _switchCommand;
            }
        }
    }
}

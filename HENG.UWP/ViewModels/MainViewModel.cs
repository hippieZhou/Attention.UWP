using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HENG.UWP.ViewModels
{

    public class NavItem: ViewModelBase
    {
        private string _header;
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        private object _viewModel;
        public object ViewModel
        {
            get { return _viewModel; }
            set { Set(ref _viewModel, value); }
        }
    }

    public class MainViewModel : ViewModelBase
    {

        private ObservableCollection<NavItem> _navItems;
        public ObservableCollection<NavItem> NavItems
        {
            get { return _navItems ?? (_navItems = new ObservableCollection<NavItem>()); }
            set { Set(ref _navItems, value); }
        }

        public void Initialize()
        {
            NavItems.Clear();
            NavItems.Add(new NavItem { Header = "Home", ViewModel = "Home" });
            NavItems.Add(new NavItem { Header = "Find", ViewModel = "Find" });
            NavItems.Add(new NavItem { Header = "Library", ViewModel = "Library" });
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
                        await Task.Yield();
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

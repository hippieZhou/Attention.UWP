using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;

namespace HENG.UWP.ViewModels
{
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
            NavItems.Add(new AllViewModel());
            NavItems.Add(new HorizontalViewModel());
            NavItems.Add(new VerticalViewModel());
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

    public class NavItem : ViewModelBase
    {
        public string Header { get; protected set; }
        public ViewModelBase ViewModel { get; set; }
        public Color Color { get; set; }
    }

    public class AllViewModel : NavItem
    {
        public AllViewModel()
        {
            Header = "HOME";
            Color = Colors.Red;
        }
    }

    public class HorizontalViewModel : NavItem
    {
        public HorizontalViewModel()
        {
            Header = "HORIZONTAL";
            Color = Colors.Yellow;
        }
    }

    public class VerticalViewModel : NavItem
    {
        public VerticalViewModel()
        {
            Header = "VERTICAL";
            Color = Colors.Green;
        }
    }
}

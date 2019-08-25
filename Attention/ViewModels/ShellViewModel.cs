using Attention.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.ViewModels
{
    public class TabItem
    {
        public string Header { get; set; }
        public ViewModelBase ViewModel { get; set; }
    }
    public class ShellViewModel : ViewModelBase
    {
        public PhotoGridViewModel PhotoGridViewModel { get; private set; } = new PhotoGridViewModel();
        public PhotoItemViewModel PhotoItemViewModel { get; private set; } = new PhotoItemViewModel();
        public SearchViewModel SearchViewModel { get; private set; }
        public DownloadViewModel DownloadViewModel { get; private set; }
        public MoreViewModel MoreViewModel { get; private set; }
        public ShellViewModel()
        {
            SearchViewModel = new SearchViewModel("search".GetLocalized());
            DownloadViewModel = new DownloadViewModel("download".GetLocalized());
            MoreViewModel = new MoreViewModel("more".GetLocalized());

            Messenger.Default.Register<NotificationMessage>(this, ViewModelLocator.Current.ToastToken, async p =>
             {
                 ToastVisibility = Visibility.Visible;

                 Toast = p.Notification;
                 await Task.Delay(2000);

                 ToastVisibility = Visibility.Collapsed;
             });
        }

        private ObservableCollection<TabItem> _tabs;
        public ObservableCollection<TabItem> Tabs
        {
            get { return _tabs ?? (_tabs = new ObservableCollection<TabItem>()); }
            set { Set(ref _tabs, value); }
        }

        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { Set(ref _selectedIndex, value); }
        }

        private Visibility _toastVisibility = Visibility.Collapsed;
        public Visibility ToastVisibility
        {
            get { return _toastVisibility; }
            set { Set(ref _toastVisibility, value); }
        }

        private string _toast;
        public string Toast
        {
            get { return _toast; }
            set { Set(ref _toast, value); }
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
                        Tabs.Clear();
                        Tabs.Add(new TabItem { Header = "ATTENTION" });
                        SelectedIndex = 0;
                        await Task.Yield();
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(() =>
                    {
                        SearchViewModel.Visibility = Visibility.Visible;
                    });
                }
                return _searchCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(() =>
                    {
                        DownloadViewModel.Visibility = Visibility.Visible;
                    });
                }
                return _downloadCommand;
            }
        }

        private ICommand _moreCommand;
        public ICommand MoreCommand
        {
            get
            {
                if (_moreCommand == null)
                {
                    _moreCommand = new RelayCommand(() =>
                    {
                        MoreViewModel.Visibility = Visibility.Visible;
                    });
                }
                return _moreCommand;
            }
        }
    }
}

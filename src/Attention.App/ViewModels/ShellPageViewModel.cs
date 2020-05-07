using Attention.App.Events;
using Attention.App.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Windows.Mvvm;
using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using winui = Microsoft.UI.Xaml.Controls;

namespace Attention.App.ViewModels
{
    public class ShellPageViewModel : ViewModelBase
    {
        private readonly ILoggerFacade _logger;
        private readonly IEventAggregator _eventAggregator;

        private Frame _shellFrame;
        private winui.NavigationView _shellNav;
        private InAppNotification _inAppNotification;

        public ShellPageViewModel(
            ILoggerFacade logger,
            IEventAggregator eventAggregator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<RaisedExceptionEvent>().Subscribe(ex =>
                        {
                            _inAppNotification?.Show("Ops!", 2000);
                            _logger.Log(ex.ToString(), Category.Exception, Priority.High);
                        });
                        _eventAggregator.GetEvent<NotificationEvent>().Subscribe(text =>
                        {
                            _inAppNotification?.Show(text, 2000);
                        });
                        var first = _shellNav.MenuItems.OfType<winui.NavigationViewItemBase>().FirstOrDefault();
                        NavigateToPage(first);
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new DelegateCommand<winui.NavigationViewItemInvokedEventArgs>(args =>
                    {
                        if (args.InvokedItemContainer is winui.NavigationViewItemBase navItem)
                        {
                            NavigateToPage(navItem);
                        }
                    });
                }
                return _itemInvokedCommand;
            }
        }

        private void NavigateToPage(winui.NavigationViewItemBase navItem)
        {
            if (navItem == null)
                throw new ArgumentNullException(nameof(navItem));

            var pageName = NavHelper.GetNavTo(navItem);
            var pageType = Type.GetType(pageName);
            _shellFrame.Navigate(pageType);
        }

        public void Initialize(winui.NavigationView shellNav, Frame frame, InAppNotification inAppNotification)
        {
            _shellNav = shellNav ?? throw new ArgumentNullException(nameof(shellNav));
            _shellFrame = frame ?? throw new ArgumentNullException(nameof(frame));
            _inAppNotification = inAppNotification;
        }
    }
}

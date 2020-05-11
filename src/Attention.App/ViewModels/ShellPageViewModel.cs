using Attention.App.Events;
using Attention.App.Helpers;
using Attention.App.ViewModels.UcViewModels;
using Attention.Core.Framework;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
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
        private FrameworkElement _shellPickedPane;
        private InAppNotification _inAppNotification;

        public ShellPageViewModel(
            ILoggerFacade logger,
            IEventAggregator eventAggregator)
        {
            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        private PickedPaneViewModel _pickedViewModel;
        public PickedPaneViewModel PickedViewModel
        {
            get { return _pickedViewModel; }
            set { SetProperty(ref _pickedViewModel, value); }
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

        private ICommand _pickPaneCommand;
        public ICommand PickPaneCommand
        {
            get
            {
                if (_pickPaneCommand == null)
                {
                    _pickPaneCommand = new DelegateCommand<string>(pickedViewModelName =>
                    {
                        var viewModel = EnginContext.Current.Resolve<PickedPaneViewModel>(pickedViewModelName);
                        if (viewModel != null)
                        {
                            PickedViewModel = viewModel;
                            _shellPickedPane.Visibility = Visibility.Visible;
                            _shellFrame
                            .Fade(0.5f)
                            .Scale(scaleX: 0.95f, scaleY: 0.95f, centerX: (float)_shellFrame.ActualWidth / 2, centerY: (float)_shellFrame.ActualHeight / 2)
                            .Start();

                            return;
                        }
                        throw new ArgumentException($"参数错误：{PickedViewModel}");
                    });
                }
                return _pickPaneCommand;
            }
        }

        private ICommand _dismissPickPaneCommand;
        public ICommand DismissPickPaneCommand
        {
            get
            {
                if (_dismissPickPaneCommand == null)
                {
                    _dismissPickPaneCommand = new DelegateCommand(() =>
                    {
                        _shellPickedPane.Visibility = Visibility.Collapsed;
                        _shellFrame
                        .Fade(1.0f)
                        .Scale(scaleX: 1.0f, scaleY: 1.0f, centerX: (float)_shellFrame.ActualWidth / 2, centerY: (float)_shellFrame.ActualHeight / 2)
                        .Start();
                    });
                }
                return _dismissPickPaneCommand;
            }
        }

        private void NavigateToPage(winui.NavigationViewItemBase navItem)
        {
            if (navItem == null)
                throw new ArgumentNullException(nameof(navItem));

            var pageName = NavigationHelper.GetNavTo(navItem);
            var pageType = Type.GetType(pageName);
            _shellFrame.Navigate(pageType);
        }

        public void Initialize(winui.NavigationView shellNav, Frame frame, FrameworkElement pickedPlacesPane, InAppNotification inAppNotification)
        {
            _shellNav = shellNav ?? throw new ArgumentNullException(nameof(shellNav));
            _shellFrame = frame ?? throw new ArgumentNullException(nameof(frame));
            _shellPickedPane = pickedPlacesPane ?? throw new ArgumentNullException(nameof(pickedPlacesPane));
            _inAppNotification = inAppNotification;
        }
    }
}

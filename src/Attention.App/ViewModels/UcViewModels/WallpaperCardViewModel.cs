using Attention.App.Extensions;
using Attention.App.Models;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Prism.Commands;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App.ViewModels.UcViewModels
{
    public class WallpaperCardViewModel : UcBaseViewModel
    {
        public event EventHandler<(WallpaperEntity, ConnectedAnimation)> TryStartBackwardsAnimation;
        private UIElement _destinationElement;

        private WallpaperEntity _entity;
        public WallpaperEntity Entity
        {
            get { return _entity; }
            set { SetProperty(ref _entity, value); }
        }

        private Visibility _avatarVisibility = Visibility.Collapsed;
        public Visibility AvatarVisibility
        {
            get { return _avatarVisibility; }
            set { SetProperty(ref _avatarVisibility, value); }
        }

        private Visibility _footerVisibility = Visibility.Collapsed;
        public Visibility FooterVisibility
        {
            get { return _footerVisibility; }
            set { SetProperty(ref _footerVisibility, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand<UIElement>(destinationElement =>
                    {
                        _destinationElement = destinationElement ?? throw new ArgumentNullException(nameof(destinationElement));
                        Visibility = Visibility.Collapsed;
                        AvatarVisibility = Visibility.Collapsed;
                        FooterVisibility = Visibility.Collapsed;
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new DelegateCommand(() =>
                    {

                    });
                }
                return _downloadCommand;
            }

        }

        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new DelegateCommand(() =>
                    {

                    });
                }
                return _browseCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new DelegateCommand(() =>
                    {

                    });
                }
                return _shareCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new DelegateCommand<TappedRoutedEventArgs>(args =>
                    {
                        var parent = LogicalTree.FindParent<Grid>(_destinationElement as FrameworkElement);
                        if (args.OriginalSource == parent)
                        {
                            var animation = _destinationElement.CreateBackwardsAnimation(() =>
                            {
                                Visibility = Visibility.Collapsed;
                                AvatarVisibility = Visibility.Collapsed;
                                FooterVisibility = Visibility.Collapsed;
                            });
                            TryStartBackwardsAnimation?.Invoke(this, (Entity, animation));
                        }
                        args.Handled = true;
                    });
                }
                return _backCommand;
            }
        }

        public void TryStartForwardAnimation(WallpaperEntity entity, ConnectedAnimation animation)
        {
            Entity = entity;
            Visibility = Visibility.Visible;
            animation.TryStart(_destinationElement);
        }
    }
}

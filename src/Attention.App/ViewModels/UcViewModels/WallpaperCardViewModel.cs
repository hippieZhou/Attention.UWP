using Attention.App.Extensions;
using Attention.App.Models;
using Attention.Core.Dtos;
using Prism.Commands;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App.ViewModels.UcViewModels
{
    public class WallpaperCardViewModel : UcBaseViewModel
    {
        private FrameworkElement _heroImage;
        private FrameworkElement _header;
        private FrameworkElement _pane2Panel;

        public event EventHandler<(WallpaperDto, ConnectedAnimation)> TryStartBackwardsAnimation;

        private WallpaperDto _entity;
        public WallpaperDto Entity
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

        public void Initialize(FrameworkElement heroImage, FrameworkElement header,FrameworkElement pane2Panel)
        {
            _heroImage = heroImage ?? throw new ArgumentNullException(nameof(heroImage));
            _header = header ?? throw new ArgumentNullException(nameof(header));
            _pane2Panel = pane2Panel ?? throw new ArgumentNullException(nameof(pane2Panel));

            Visibility = Visibility.Collapsed;
            AvatarVisibility = Visibility.Collapsed;
            FooterVisibility = Visibility.Collapsed;
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
                    _backCommand = new DelegateCommand(() =>
                    {
                        var animation = _heroImage.CreateBackwardsAnimation(() =>
                        {
                            Visibility = Visibility.Collapsed;
                            AvatarVisibility = Visibility.Collapsed;
                            FooterVisibility = Visibility.Collapsed;
                        });
                        TryStartBackwardsAnimation?.Invoke(this, (Entity, animation));
                    });
                }
                return _backCommand;
            }
        }

        public void TryStartForwardAnimation(WallpaperDto entity, ConnectedAnimation animation)
        {
            Entity = entity;
            Visibility = Visibility.Visible;
            animation.TryStart(_heroImage, new[] { _header, _pane2Panel });
        }
    }
}

using Attention.App.Extensions;
using Attention.Core.Bus;
using Attention.Core.Commands;
using Attention.Core.Dtos;
using Attention.Core.Framework;
using Prism.Commands;
using System;
using System.IO;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Attention.App.ViewModels.UcViewModels
{
    public class DetailCardViewModel : UcBaseViewModel
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

        public void Initialize(FrameworkElement heroImage, FrameworkElement header,FrameworkElement pane2Panel)
        {
            _heroImage = heroImage ?? throw new ArgumentNullException(nameof(heroImage));
            _header = header ?? throw new ArgumentNullException(nameof(header));
            _pane2Panel = pane2Panel ?? throw new ArgumentNullException(nameof(pane2Panel));
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new DelegateCommand(async () =>
                    {
                        StorageFolder folder = await App.Settings.GetSavedFolderAsync();
                        var command = new DownloadCommand()
                        {
                            FolderName = folder.Path,
                            FileName = Path.GetFileName(new Uri(Entity.ImageDownloadUri).AbsolutePath),
                            DownloadUri = Entity.ImageDownloadUri,
                        };
                        var bus = EnginContext.Current.Resolve<IMediatorHandler>();
                        if (bus == null)
                        {
                            throw new ArgumentNullException($"服务未找到：{typeof(IMediatorHandler)}");
                        }
                        var response = await bus.Send(command);
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
                        });
                        TryStartBackwardsAnimation?.Invoke(this, (Entity, animation));
                    });
                }
                return _backCommand;
            }
        }

        public void TryStartForwardAnimation(WallpaperDto entity, ConnectedAnimation animation)
        {
            animation.TryStart(_heroImage, new[] { _header, _pane2Panel });
            Entity = entity;
            Visibility = Visibility.Visible;
        }
    }
}

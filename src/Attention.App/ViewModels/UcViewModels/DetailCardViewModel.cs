using Attention.App.Extensions;
using Attention.App.UserControls;
using Attention.App.Views;
using Attention.Core.Bus;
using Attention.Core.Commands;
using Attention.Core.Dtos;
using Attention.Core.Framework;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Prism.Commands;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Attention.App.ViewModels.UcViewModels
{
    public class DetailCardViewModel : UcBaseViewModel
    {
        private DetailCardView _handler;
        private bool _firstTimeAnimation = true;

        public event EventHandler<(WallpaperDto, ConnectedAnimation)> TryStartBackwardsAnimation;

        private WallpaperDto _entity;
        public WallpaperDto Entity
        {
            get { return _entity; }
            set { SetProperty(ref _entity, value); }
        }

        public void Initialize(DetailCardView handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
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
                        await PlayStoreAnimationAsync(() => _handler.HeroImageMirror.Source = null);
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
                        var animation = _handler.HeroImage.CreateBackwardsAnimation();
                        Visibility = Visibility.Collapsed;
                        TryStartBackwardsAnimation?.Invoke(this, (Entity, animation));
                    });
                }
                return _backCommand;
            }
        }

        public void TryStartForwardAnimation(WallpaperDto entity, ConnectedAnimation animation)
        {
            animation.TryStart(_handler.HeroImage, new[] { _handler.Header });
            Entity = entity;
            Visibility = Visibility.Visible;
        }

        private async Task PlayStoreAnimationAsync(Action completed = null)
        {
            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(_handler.HeroImage);
            _handler.HeroImageMirror.Source = bitmap;
            _handler.HeroImageMirror.Width = _handler.HeroImage.ActualWidth;
            _handler.HeroImageMirror.Height = _handler.HeroImage.ActualHeight;

            var mainPage = _handler.FindAscendant<ShellPage>();
            var navView = mainPage.FindDescendant<WinUI.NavigationView>();
            if (navView.PaneCustomContent.FindDescendantByName("downloadButton") is Button downloadButton)
            {
                var dot = downloadButton.FindDescendant<Ellipse>();

                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("storeDownload", _handler.HeroImageMirror);
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("storeDownload");
                if (animation != null)
                {
                    animation.Completed += (sender, e) => completed?.Invoke();
                    animation.TryStart(dot);
                }

                dot.Visibility = Visibility.Visible;

                if (_firstTimeAnimation)
                {
                    _firstTimeAnimation = false;
                    await Task.Delay(50);
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("storeDownload", _handler.HeroImageMirror);
                    var animation1 = ConnectedAnimationService.GetForCurrentView().GetAnimation("storeDownload");
                    if (animation1 != null)
                    {
                        animation1.Completed += (sender, e) => completed?.Invoke();
                        animation1.TryStart(dot);
                    }
                }
            }
        }

        private async Task DownloadAsync()
        {
            StorageFolder folder = await App.Settings.GetSavedFolderAsync();
            var command = new DownloadCommand()
            {
                FolderName = folder.Path,
                FileName = System.IO.Path.GetFileName(new Uri(Entity.ImageDownloadUri).AbsolutePath),
                DownloadUri = Entity.ImageDownloadUri,
            };
            var bus = EnginContext.Current.Resolve<IMediatorHandler>();
            if (bus == null)
            {
                throw new ArgumentNullException($"服务未找到：{typeof(IMediatorHandler)}");
            }
            var response = await bus.Send(command);
        }
    }
}

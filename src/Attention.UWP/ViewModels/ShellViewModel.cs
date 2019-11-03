using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using System;
using Attention.UWP.Helpers;
using System.Linq;
using System.Threading.Tasks;
using Attention.UWP.Views;

namespace Attention.UWP.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public FrameworkElement MainElement { get; private set; }
        public ShellViewModel()
        {
            Messenger.Default.Register<bool>(this, nameof(App.Settings.LiveTitle), async enabled =>
            {
                await RefreshLiveTitleAsync(enabled);
            });

            Messenger.Default.Register<string>(this, nameof(AppNotification), async str =>
            {
                await Singleton<AppNotification>.Instance.ShowAsync(str, TimeSpan.FromSeconds(2.0));
            });
        }

        public void Initialize(object mainElement)
        {
            MainElement = mainElement as MainView;
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<RoutedEventArgs>(async args =>
                    {
                        MainElement.Visibility = Visibility.Visible;
                        await RefreshLiveTitleAsync(App.Settings.LiveTitle);
                    });
                }
                return _loadedCommand;
            }
        }

        private async Task RefreshLiveTitleAsync(bool enabled)
        {
            if (enabled)
            {
                new BackgroundProxy().Register();
                AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];
                bool isPinned = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
                if (isPinned)
                {
                    var result = await ViewModelLocator.Current.Pixabay.QueryImagesAsync(1, 5, App.Settings.Filter);
                    if (result != null)
                    {
                        var items = result.Images.Select(p => p.PreviewURL);
                        LiveTileHelper.UpdateLiveTile(items);
                    }
                }
            }
            else
            {
                new BackgroundProxy().UnRegister();
                LiveTileHelper.CleanUpTile();
            }
        }
    }
}

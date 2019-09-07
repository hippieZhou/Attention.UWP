using Attention.Models;
using Attention.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Attention.ViewModels
{
    public class ExtendedSplashScreenViewModel
    {
        private readonly AppSettingService _appSettingService;

        public ExtendedSplashScreenViewModel(AppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
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
                        await Task.Delay(TimeSpan.FromSeconds(2));

                        //if (string.IsNullOrWhiteSpace(_settings.DownloadPath))
                        //{
                        //    StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
                        //    _settings.DownloadPath = folder.Path;
                        //}

                        await _appSettingService.InitializeAsync();

                        Frame rootFrame = new Frame();
                        rootFrame.Navigate(typeof(ShellPage));
                        Window.Current.Content = rootFrame;
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

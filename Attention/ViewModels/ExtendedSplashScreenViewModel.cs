using Attention.Models;
using Attention.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace Attention.ViewModels
{
    public class ExtendedSplashScreenViewModel
    {
        private readonly AppSettings _settings;
        private readonly AppSettingService _appSettingService;

        public ExtendedSplashScreenViewModel(AppSettings settings, AppSettingService appSettingService)
        {
            _settings = settings;
            _appSettingService = appSettingService;
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand<Action>(async action =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2));

                        if (string.IsNullOrWhiteSpace(_settings.DownloadPath))
                        {
                            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
                            _settings.DownloadPath = folder.Path;
                        }

                        await _appSettingService.InitializeAsync();

                        action?.Invoke();
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

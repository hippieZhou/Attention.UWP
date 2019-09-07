using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using System;
using Windows.ApplicationModel;
using Windows.UI.Popups;

namespace HENG.App.ViewModels
{
    public class MoreViewModel:ViewModelBase
    {
        private ICommand _openFolerCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolerCommand == null)
                {
                    _openFolerCommand = new RelayCommand<string>(async path =>
                    {
                        var sf = await StorageFolder.GetFolderFromPathAsync(path);
                        await Launcher.LaunchFolderAsync(sf);
                    });
                }
                return _openFolerCommand;
            }
        }

        private ICommand _showStoreRatingCommand;
        public ICommand ShowStoreRatingCommand
        {
            get
            {
                if (_showStoreRatingCommand == null)
                {
                    _showStoreRatingCommand = new RelayCommand(async () =>
                    {
                        string message = "aa";
                        string okButtonText = "OK";
                        string cancelButtonText = "Cancel";

                        async void handler() => await Launcher.LaunchUriAsync(new Uri(
                            $"ms-windows-store:REVIEW?PFN={Package.Current.Id.FamilyName}"));

                        var messageDialog = new MessageDialog(message) { CancelCommandIndex = 1 };
                        messageDialog.Commands.Add(new UICommand(okButtonText, command => handler()));
                        messageDialog.Commands.Add(new UICommand(cancelButtonText));
                        await messageDialog.ShowAsync();
                    });
                }
                return _showStoreRatingCommand;
            }
        }
    }
}

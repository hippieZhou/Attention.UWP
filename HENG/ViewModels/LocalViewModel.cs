using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Models;
using HENG.Models.Shares;
using HENG.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace HENG.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {
        private ObservableCollection<DownloadItem> _photos;
        public ObservableCollection<DownloadItem> Photos
        {
            get { return _photos ?? (_photos = new ObservableCollection<DownloadItem>()); }
            set { Set(ref _photos, value); }
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
                       Photos.Clear();
                       await InitializeAsync();

                       await DispatcherHelper.RunAsync(() => 
                       {
                           var items = Singleton<DataService>.Instance.Downloads;
                           foreach (DownloadItem item in items)
                           {
                               Photos.Add(item);
                           }
                       });

                       Singleton<DataService>.Instance.DownloadsEvent += async (sender, items) => 
                       {
                           await DispatcherHelper.RunAsync(() =>
                            {
                                foreach (DownloadItem item in items)
                                {
                                    Photos.Add(item);
                                }
                            });
                       };
                   });
                }
                return _loadedCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new RelayCommand<DownloadItem>(item =>
                    {
                        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
                        dataTransferManager.DataRequested += (sender, e) =>
                        {
                            var deferral = e.Request.GetDeferral();
                            sender.TargetApplicationChosen += (s1, s2) =>
                            {
                                deferral.Complete();
                            };

                            var data = new ShareSourceData("AppDisplayName".GetLocalized());
                            if (item.RequestedUri != null)
                            {
                                data.SetWebLink(item.RequestedUri);
                            }
                            if (item.ResultFile != null)
                            {
                                data.SetImage(item.ResultFile as StorageFile);
                            }

                            if (data != null)
                            {
                                e.Request.SetData(data);
                            }
                            e.Request.Data.OperationCompleted += (s, _) =>
                            {
                                //Messenger.Default.Send(new NotificationMessageAction<string>(sender, "分享成功", reply => { Trace.WriteLine(reply); }));
                            };
                        };
                        DataTransferManager.ShowShareUI();
                    });
                }
                return _shareCommand;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<DownloadItem>(async item =>
                    {
                        await item.ResultFile.DeleteAsync(StorageDeleteOption.Default);
                        Photos.Remove(item);
                    });
                }
                return _deleteCommand;
            }
        }

        private async Task InitializeAsync()
        {
            StorageFolder downloadFolder = await StorageFolder.GetFolderFromPathAsync(App.Settings.DownloadPath);
            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string> { ".jpg", ".png" });
            IReadOnlyList<StorageFile> sfs = await downloadFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
            await DispatcherHelper.RunAsync(async () =>
             {
                 foreach (var sf in sfs)
                 {
                     BitmapImage photo = await ImageHelper.StorageFileToBitmapImage(sf);
                     Photos.Add(new DownloadItem()
                     {
                         ResultFile = sf,
                         Photo = photo
                     });
                 }
             });
        }
    }
}

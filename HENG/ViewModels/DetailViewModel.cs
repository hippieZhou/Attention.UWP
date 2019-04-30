using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using HENG.Services;
using System.Windows.Input;
using System;
using Windows.ApplicationModel.DataTransfer;
using HENG.Models.Shares;
using HENG.Helpers;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using Windows.Storage;

namespace HENG.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        private object _model;
        public object Model
        {
            get { return _model; }
            set { Set(ref _model, value); }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() =>
                    {
                        CaseModel(Model, async b1 =>
                        {
                            b1.ImageCache = b1.Url;
                            Model = b1;

                            await Singleton<DataService>.Instance.GetFromCacheAsync(b1.Url, bmp =>
                            {
                                if (bmp != null)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        b1.ImageCache = bmp;
                                        Model = b1;
                                    });
                                }
                            });
                        }, async b2 =>
                        {
                            b2.ImageCache = b2.Thumb;
                            Model = b2;

                            await Singleton<DataService>.Instance.GetFromCacheAsync(b2.Download_url, bmp =>
                            {
                                if (bmp != null)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        b2.ImageCache = bmp;
                                        Model = b2;
                                    });
                                }
                            });
                        }, async b3 =>
                        {
                            b3.ImageCache = b3.Urls.Regular;
                            Model = b3;

                            await Singleton<DataService>.Instance.GetFromCacheAsync(b3.Urls.Full, bmp =>
                            {
                                if (bmp != null)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        b3.ImageCache = bmp;
                                        Model = b3;
                                    });
                                }
                            });
                        });
                    });
                }
                return _refreshCommand;
            }
        }

        private ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(() =>
                    {
                        //var cts = new CancellationTokenSource();
                        //var bmp = await BackgroundTaskService.CacheImageAsync(Model, cts);
                    });
                }
                return _downloadCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new RelayCommand(() =>
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

                            CaseModel(Model, async b1 => 
                            {
                                data.SetWebLink(new Uri(b1.Url));
                                var sf = await Singleton<DataService>.Instance.GetFileFromCacheAsync(b1.Url);
                                if (sf != null)
                                {
                                    data.SetStorageItems(new List<IStorageFile> { sf });
                                    data.SetImage(sf);
                                }
                                data.SetText(b1.Description);
                            }, async b2 => 
                            {
                                data.SetWebLink(new Uri(b2.Download_url));
                                var sf = await Singleton<DataService>.Instance.GetFileFromCacheAsync(b2.Download_url);
                                if (sf != null)
                                {
                                    data.SetStorageItems(new List<IStorageFile> { sf });
                                    data.SetImage(sf);
                                }
                                data.SetText(b2.Author);

                            }, async b3 => 
                            {
                                data.SetWebLink(new Uri(b3.Urls.Full));
                                var sf = await Singleton<DataService>.Instance.GetFileFromCacheAsync(b3.Urls.Full);
                                if (sf != null)
                                {
                                    data.SetStorageItems(new List<IStorageFile> { sf });
                                    data.SetImage(sf);
                                }
                                data.SetText(b3.User.Name);
                            });

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

        private void CaseModel(object model, Action<BingItem> bingAction, Action<PicsumItem> PicsumAction, Action<PaperItem> PaperActon)
        {
            var type = model.GetType();

            if (typeof(BingItem) == type)
            {
                bingAction((BingItem)model);
            }
            else if (typeof(PicsumItem) == type)
            {
                PicsumAction((PicsumItem)model);
            }
            else if (typeof(PaperItem) == type)
            {
                PaperActon((PaperItem)model);
            }
        }
    }
}

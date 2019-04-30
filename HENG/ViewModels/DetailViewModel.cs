using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Models;
using HENG.Services;
using System.Windows.Input;
using System;
using Windows.ApplicationModel.DataTransfer;
using HENG.Models.Shares;
using HENG.Helpers;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Threading;

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
                    _refreshCommand = new RelayCommand(async () =>
                    {
                        if (typeof(BingItem) == Model.GetType())
                        {
                            var model = Model as BingItem;
                            model.ImageCache = model.Url;

                            await Singleton<DataService>.Instance.GetFromCacheAsync(model.Url, bmp =>
                            {
                                if (bmp != null)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() => 
                                    {
                                        model.ImageCache = bmp;
                                    });
                                }
                            });
                        }
                        else if (typeof(PicsumItem) == Model.GetType())
                        {
                            var model = Model as PicsumItem;
                            model.ImageCache = model.Thumb;

                            await Singleton<DataService>.Instance.GetFromCacheAsync(model.Download_url, bmp =>
                            {
                                if (bmp != null)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        model.ImageCache = bmp;
                                    });
                                }
                            });
                        }
                        else if (typeof(PaperItem) == Model.GetType())
                        {
                            var model = Model as PaperItem;
                            model.ImageCache = model.Urls.Regular;
                            await Singleton<DataService>.Instance.GetFromCacheAsync(model.Urls.Full, bmp =>
                            {
                                if (bmp != null)
                                {
                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        model.ImageCache = bmp;
                                    });
                                }
                            });
                        }
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
                            data.SetWebLink(new Uri("http://www.baidu.com"));
                            data.SetText("Hello World");
                            e.Request.SetData(data);
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
    }
}

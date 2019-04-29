using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Models;
using HENG.Services;
using System.Windows.Input;
using System;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel.DataTransfer;
using HENG.Models.Shares;
using HENG.Helpers;
using Windows.UI.Xaml;

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

        private object _bitmap;
        public object Bitmap
        {
            get { return _bitmap; }
            set { Set(ref _bitmap, value); }
        }


        private Visibility _progressBarVisibility = Visibility.Visible;
        public Visibility ProgressBarVisibility
        {
            get { return _progressBarVisibility; }
            set { Set(ref _progressBarVisibility, value); }
        }


        private string OriginalString = string.Empty;

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(async () =>
                    {
                        ProgressBarVisibility = Visibility.Visible;

                        if (typeof(BingItem) == Model.GetType())
                        {
                            var model = Model as BingItem;
                            OriginalString = model?.Url;
                            Bitmap = model?.Url;
                        }
                        else if (typeof(PicsumItem) == Model.GetType())
                        {
                            var model = Model as PicsumItem;
                            OriginalString = model?.Download_url;
                            Bitmap = model.Thumb;
                        }
                        else if (typeof(PaperItem) == Model.GetType())
                        {
                            var model = Model as PaperItem;
                            OriginalString = model?.Urls.Full;
                            Bitmap = model.Urls.Thumb;
                        }

                        if (string.IsNullOrWhiteSpace(OriginalString))
                        {
                            return;
                        }

                        await Singleton<DataService>.Instance.GetFromCacheAsync(OriginalString, bmp =>
                         {
                             DispatcherHelper.CheckBeginInvokeOnUI(() =>
                             {
                                 if (bmp != null)
                                 {
                                     Bitmap = bmp;
                                 }
                                 ProgressBarVisibility = Visibility.Collapsed;
                             });
                         });
                        //await BackgroundTaskService.CacheImageAsync(OriginalString, async sf =>
                        //{
                        //    if (sf != null)
                        //    {
                        //        await DispatcherHelper.RunAsync(async () =>
                        //        {
                        //            BitmapImage bmp = await BackgroundTaskService.DrawImageAsync(sf);
                        //            Progress = 100;
                        //        });
                        //    }
                        //});
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

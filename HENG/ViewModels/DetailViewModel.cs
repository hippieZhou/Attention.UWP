using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Models;
using HENG.Services;
using System.Threading;
using System.Windows.Input;
using System;
using Windows.UI.Xaml.Media.Imaging;

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

        private double _progress = 0.0;
        public double Progress
        {
            get { return _progress; }
            set { Set(ref _progress, value); }
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
                        var cts = new CancellationTokenSource();
                        await BackgroundTaskService.CacheImageAsync(Model, cts, async (sf, ex) =>
                        {
                            if (sf != null && ex == null)
                            {
                                await DispatcherHelper.RunAsync(async () =>
                                {
                                    BitmapImage bmp = await BackgroundTaskService.DrawImageAsync(sf);
                                });
                            }
                        });
                    });
                }
                return _loadedCommand;
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
                        //DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
                        //dataTransferManager.DataRequested += (sender, e) =>
                        //{
                        //    var data = new ShareSourceData("AppDisplayName".GetLocalized());
                        //    data.SetWebLink(new Uri("http://www.baidu.com"));
                        //    data.SetText("Hello World");
                        //    e.Request.SetData(data);
                        //    e.Request.Data.OperationCompleted += (s, _) =>
                        //    {
                        //        //Messenger.Default.Send(new NotificationMessageAction<string>(sender, "分享成功", reply => { Trace.WriteLine(reply); }));
                        //    };
                        //};
                        //DataTransferManager.ShowShareUI();
                    });
                }
                return _shareCommand;
            }
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

                    });
                }
                return _refreshCommand;
            }
        }

    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
                        Model.ParseModel(async b1 =>
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
                    _downloadCommand = new RelayCommand(async () =>
                    {
                        var url = string.Empty;
                        Model.ParseModel(b1 =>
                        {
                            url = b1.Url;
                        }, b2 =>
                        {
                            url = b2.Download_url;

                        }, b3 =>
                        {
                            url = b3.Urls.Full;
                        });
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            await Singleton<DataService>.Instance.DownLoad(new Uri(url));
                        }
                    });
                }
                return _downloadCommand;
            }
        }
    }
}

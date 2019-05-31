using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Services;
using System.Windows.Input;
using HENG.Helpers;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using HENG.Models;
using Microsoft.Toolkit.Uwp.UI;
using System;

namespace HENG.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        private DataItem _photo;
        public DataItem Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
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
                        if (Photo.ImageCache is string str)
                        {
                            Photo.ImageCache = await ImageCache.Instance.GetFromCacheAsync(new Uri(str));
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
                    _downloadCommand = new RelayCommand(async () =>
                    {
                        await Singleton<DataService>.Instance.DownLoad(Photo);
                        Messenger.Default.Send(new NotificationMessageAction<string>("downloading".GetLocalized(), str => { Trace.WriteLine(str); }));
                    });
                }
                return _downloadCommand;
            }
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Storage;
using GalaSoft.MvvmLight.Messaging;

namespace HENG.ViewModels
{
    public class LocalViewModel : ViewModelBase
    {
        public LocalViewModel()
        {
            Messenger.Default.Register<DownloadItem>(this, item =>
            {
                if (!Photos.Contains(item))
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => { Photos.Add(item); });
                }
            });
        }

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
                    _loadedCommand = new RelayCommand(() =>
                   {
                       DispatcherHelper.CheckBeginInvokeOnUI(() => 
                       {
                           Singleton<DataService>.Instance.Downloads.ForEach(item =>
                           {
                               if (!Photos.Contains(item))
                               {
                                   Photos.Add(item);
                               }
                           });
                       });
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
                        Singleton<DataService>.Instance.Share(item);
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
    }
}

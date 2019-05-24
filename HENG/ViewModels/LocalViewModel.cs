using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HENG.Helpers;
using HENG.Models;
using HENG.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

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

                       var hostories = await Singleton<DataService>.Instance.LoadHostoryAsync();
                       await DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                       {
                           foreach (DownloadItem item in hostories)
                           {
                               Photos.Add(item);
                           }
                       });

                       Singleton<DataService>.Instance.DownloadEvent += (sender, item) =>
                       {
                           DispatcherHelper.CheckBeginInvokeOnUI(() => { Photos.Add(item); });
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

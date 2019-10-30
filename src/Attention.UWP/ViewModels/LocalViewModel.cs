using Attention.UWP.Models;
using Attention.UWP.Models.Core;
using Attention.UWP.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.System;
using System;
using Attention.UWP.Models.Repositories;

namespace Attention.UWP.ViewModels
{
    public class LocalViewModel : BaseViewModel
    {
        private readonly DAL _dal;

        private ObservableCollection<DownloadItem> _items;
        public ObservableCollection<DownloadItem> Items
        {
            get { return _items ?? (_items = new ObservableCollection<DownloadItem>()); }
            set { Set(ref _items, value); }
        }

        public LocalViewModel(DAL dal)
        {
            _dal = dal;
            Messenger.Default.Register<DownloadItem>(this, nameof(DownloadItem), item =>
            {
                Items.Add(item);
            });
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
                        var folder = await App.Settings.GetSavingFolderAsync();
                        IEnumerable<Download> entities = await _dal.DownloadRepo.GetAllAsync();
                        IEnumerable<DownloadItem> downloads = from p in entities select new DownloadItem(p, folder);
                        foreach (var item in downloads)
                        {
                            await item.ReLoadImageSource();
                            Items.Add(item);
                        }
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand<DownloadItem>(async args =>
                    {
                        if (args != null)
                        {
                            var folder = await App.Settings.GetSavingFolderAsync();
                            await Launcher.LaunchFolderAsync(folder);
                        }
                    });
                }
                return _openCommand;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<DownloadItem>(async args =>
                    {
                        if (args != null)
                        {
                            await args.DeleteAsync();
                            Items.Remove(args);
                        }
                    });
                }
                return _deleteCommand;
            }
        }
    }
}

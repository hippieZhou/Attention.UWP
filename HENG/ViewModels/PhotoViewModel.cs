﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using HENG.Helpers;
using HENG.Services;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;

namespace HENG.ViewModels
{
    public class PhotoViewModel<TSource, IType> : ViewModelBase where TSource : IIncrementalSource<IType>
    {
        private IncrementalLoadingCollection<TSource, IType> _photos;
        public IncrementalLoadingCollection<TSource, IType> Photos
        {
            get { return _photos; }
            set { Set(ref _photos, value); }
        }

        private Visibility _loadingVisibility = Visibility.Collapsed;
        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set { Set(ref _loadingVisibility, value); }
        }

        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set { Set(ref _errorVisibility, value); }
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
                        if (Photos == null)
                        {
                            await DispatcherHelper.RunAsync(() =>
                            {
                                Photos = new IncrementalLoadingCollection<TSource, IType>(20,
                                    () =>
                                    {
                                        LoadingVisibility = Visibility.Visible;
                                        ErrorVisibility = Visibility.Collapsed;
                                    },
                                    () =>
                                    {
                                        LoadingVisibility = Visibility.Collapsed;

                                        if (Photos.Count > 0)
                                        {
                                            //Singleton<LiveTileService>.Instance.CreateLiveTitle(Photos[0]);
                                        }
                                    },
                                    ex =>
                                    {
                                        ErrorVisibility = Visibility.Visible;
                                    });
                            });
                        }
                    });
                }
                return _loadedCommand;
            }
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
                        await Photos.RefreshAsync();
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
                    _downloadCommand = new RelayCommand<IType>(async model =>
                    {
                        if (typeof(IType) == model.GetType())
                        {
                            var cts = new CancellationTokenSource();
                            await Singleton<DataService>.Instance.DownloadImageAsync(model, cts);
                            Messenger.Default.Send(new NotificationMessageAction<string>(this, "downloading".GetLocalized(), reply => { Trace.WriteLine(reply); }));
                        }
                    });
                }
                return _downloadCommand;
            }
        }

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<IType>(model =>
                    {
                        if (typeof(IType) == model.GetType())
                        {
                            Messenger.Default.Send(new GenericMessage<object>(this, model));
                        }
                    });
                }
                return _itemClickCommand;
            }
        }
    }
}

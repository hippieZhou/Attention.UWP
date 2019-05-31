﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using GalaSoft.MvvmLight.Messaging;
using HENG.Services;
using HENG.Helpers;
using System.Diagnostics;
using HENG.Models;

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
                    _loadedCommand = new RelayCommand(() =>
                    {
                        if (Photos == null)
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
                                },
                                ex =>
                                {
                                    ErrorVisibility = Visibility.Visible;
                                });
                        };
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
                    _downloadCommand = new RelayCommand<string>(async url =>
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            await Singleton<DataService>.Instance.DownLoad(new Uri(url));
                            Messenger.Default.Send(new NotificationMessageAction<string>("downloading".GetLocalized(), str => { Trace.WriteLine(str); }));
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
                        Messenger.Default.Send(model as DataItem, nameof(DataItem));
                    });
                }
                return _itemClickCommand;
            }
        }
    }
}
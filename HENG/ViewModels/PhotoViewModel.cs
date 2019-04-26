using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System.Windows.Input;
using Windows.UI.Xaml;
using System;
using HENG.Helpers;
using HENG.Services;
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

        private Visibility _footerVisibility = Visibility.Collapsed;
        public Visibility FooterVisibility
        {
            get { return _footerVisibility; }
            set { Set(ref _footerVisibility, value); }
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
                                        FooterVisibility = Visibility.Visible;
                                    },
                                    () =>
                                    {
                                        FooterVisibility = Visibility.Collapsed;

                                        if (Photos.Count > 0)
                                        {
                                            //Singleton<LiveTileService>.Instance.CreateLiveTitle(Photos[0]);
                                        }
                                    },
                                    ex =>
                                    {
                                        FooterVisibility = Visibility.Visible;
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
                            await Singleton<DataService>.Instance.DownloadImageAsync(model);
                        }
                    });
                }
                return _downloadCommand;
            }
        }
    }
}

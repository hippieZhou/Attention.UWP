using Attention.App.Businesss;
using Attention.App.Models;
using Microsoft.Toolkit.Uwp;
using Prism.Commands;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.App.ViewModels.UcViewModels
{
    public class WallpaperExploreViewModel : UcBaseViewModel
    {
        private IncrementalLoadingCollection<ExploreItemSource, ExploreEntity> _entities;
        public IncrementalLoadingCollection<ExploreItemSource, ExploreEntity> Entities
        {
            get { return _entities; }
            set { SetProperty(ref _entities, value); }
        }

        private Visibility _loadingVisibility = Visibility.Visible;
        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set { SetProperty(ref _loadingVisibility, value); }
        }

        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set { SetProperty(ref _errorVisibility, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(async () =>
                    {
                        Entities = new IncrementalLoadingCollection<ExploreItemSource, ExploreEntity>(10, () =>
                        {
                            Trace.WriteLine("1111111");
                            LoadingVisibility = Visibility.Visible;
                            ErrorVisibility = Visibility.Collapsed;
                        }, () =>
                        {
                            Trace.WriteLine("222222222");
                            LoadingVisibility = Visibility.Collapsed;
                            ErrorVisibility = Visibility.Collapsed;
                        }, ex =>
                        {
                            Trace.WriteLine("333333333");
                            LoadingVisibility = Visibility.Collapsed;
                            ErrorVisibility = Visibility.Visible;
                        });
                        await Entities.LoadMoreItemsAsync(1);
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(async () =>
                    {
                        await Entities.RefreshAsync();
                    });
                }
                return _refreshCommand;
            }
        }

        private ICommand _switchCommand;
        public ICommand SwitchCommand
        {
            get
            {
                if (_switchCommand == null)
                {
                    _switchCommand = new DelegateCommand<object>(visibility =>
                    {
                        if (visibility is Visibility vis)
                        {
                            Visibility = vis;
                        }
                    });
                }
                return _switchCommand;
            }
        }
    }
}

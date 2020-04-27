using Attention.App.Businesss;
using Attention.App.Models;
using Microsoft.Toolkit.Uwp;
using Prism.Commands;
using System;
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


        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(async () =>
                    {
                        Entities = new IncrementalLoadingCollection<ExploreItemSource, ExploreEntity>();
                        await Entities.LoadMoreItemsAsync(1);
                    });
                }
                return _loadCommand;
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

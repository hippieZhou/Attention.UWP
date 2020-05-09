using Attention.App.Businesss;
using Attention.Core.Dtos;
using Microsoft.Toolkit.Uwp;
using Prism.Commands;
using Prism.Windows.AppModel;
using System.Windows.Input;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedDownloadViewModel: PickedPaneViewModel
    {
        public PickedDownloadViewModel(IResourceLoader resourceLoader) : base(PaneTypes.Download, resourceLoader.GetString("picked_Downloads"))
        {
        }

        private IncrementalLoadingCollection<ExploreItemSource, ExploreDto> _entities;
        public IncrementalLoadingCollection<ExploreItemSource, ExploreDto> Entities
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
                        Entities = new IncrementalLoadingCollection<ExploreItemSource, ExploreDto>();
                        await Entities.RefreshAsync();
                    });
                }
                return _loadCommand;
            }
        }

    }
}

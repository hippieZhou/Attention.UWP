using Attention.Core.Common;
using Attention.Core.Dtos;
using Prism.Commands;
using Prism.Windows.AppModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedDownloadViewModel: PickedPaneViewModel
    {
        public PickedDownloadViewModel(IResourceLoader resourceLoader) : base(PaneTypes.Download, resourceLoader.GetString("picked_Downloads"))
        { 
        }

        private NotifyTaskCompletion<IEnumerable<ExploreDto>> _entities;
        public NotifyTaskCompletion<IEnumerable<ExploreDto>> Entities
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
                    _loadCommand = new DelegateCommand(() =>
                    {
                        Entities = new NotifyTaskCompletion<IEnumerable<ExploreDto>>(Task.FromResult(ExploreDto.FakeData));
                    });
                }
                return _loadCommand;
            }
        }

    }
}

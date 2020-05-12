using Attention.Core.Common;
using Attention.Core.Dtos;
using Prism.Commands;
using Prism.Windows.AppModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedDownloadViewModel: PickedPaneViewModel
    {
        public PickedDownloadViewModel(IResourceLoader resourceLoader) : base(PaneTypes.Download, resourceLoader.GetString("picked_Downloads"))
        {
        }

        private NotifyTaskCompletion<ObservableCollection<ExploreDto>> _entities;
        public NotifyTaskCompletion<ObservableCollection<ExploreDto>> Entities
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
                       var list = await Task.Run(()=> ExploreDto.GetFakeData());
                       Entities = new NotifyTaskCompletion<ObservableCollection<ExploreDto>>(
                           Task.FromResult(new ObservableCollection<ExploreDto>(list)));
                   });
                }
                return _loadCommand;
            }
        }

    }
}

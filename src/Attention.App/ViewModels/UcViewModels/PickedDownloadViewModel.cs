using Attention.Core.Common;
using Attention.Core.Dtos;
using Attention.Core.Services;
using Prism.Commands;
using Prism.Windows.AppModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using Attention.Core.Extensions;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedDownloadViewModel: PickedPaneViewModel
    {
        private readonly IDataService _dataService;

        public PickedDownloadViewModel(IDataService dataService, IResourceLoader resourceLoader) : base(PaneTypes.Download, resourceLoader.GetString("picked_Downloads"))
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        private NotifyTaskCompletion<ObservableCollection<DownloadDto>> _entities;
        public NotifyTaskCompletion<ObservableCollection<DownloadDto>> Entities
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
                       var list = await Task.Run(() => _dataService.GetDownloadItems(1, 100));
                       var task = DispatcherHelper.ExecuteOnUIThreadAsync(() => list.ToObservableCollection());
                       Entities = new NotifyTaskCompletion<ObservableCollection<DownloadDto>>(task);
                   });
                }
                return _loadCommand;
            }
        }
    }
}

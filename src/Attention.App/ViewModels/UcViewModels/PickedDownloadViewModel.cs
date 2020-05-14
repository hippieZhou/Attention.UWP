﻿using Attention.Core.Common;
using Attention.Core.Dtos;
using Attention.Core.Services;
using Prism.Commands;
using Prism.Windows.AppModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

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
                       Entities = new NotifyTaskCompletion<ObservableCollection<DownloadDto>>(
                           Task.FromResult(new ObservableCollection<DownloadDto>(list)));
                   });
                }
                return _loadCommand;
            }
        }

    }
}

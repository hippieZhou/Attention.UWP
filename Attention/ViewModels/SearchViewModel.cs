using Attention.Extensions;
using Attention.Models;
using Attention.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Attention.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(string header) : base(header) { }

        private ObservableCollection<string> _histories;
        public ObservableCollection<string> Histories
        {
            get { return _histories ?? (_histories = new ObservableCollection<string>()); }
            set { Set(ref _histories, value); }
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
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand<AutoSuggestBoxQuerySubmittedEventArgs>(args =>
                    {
                        NavBackCommand.Execute(null);
                    });
                }
                return _searchCommand; }
        }
    }
}

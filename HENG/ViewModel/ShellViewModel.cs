using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using HENG.Model;

namespace HENG.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        public ShellViewModel(  IDataService dataService,  INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }

        public async Task Initialize()
        {
            try
            {
                var item = await _dataService.GetData();
            }
            catch (Exception ex)
            {
                // Report error here
            }
        }
    }
}
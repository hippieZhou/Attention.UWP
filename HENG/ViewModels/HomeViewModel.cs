using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HENG.Core.Services;
using System.Windows.Input;

namespace HENG.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly PixabayService _service;
        public HomeViewModel(PixabayService service)
        {
            _service = service;
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
                        await _service.QueryImagesAsync();
                    });
                }
                return _loadedCommand;
            }
        }
    }
}

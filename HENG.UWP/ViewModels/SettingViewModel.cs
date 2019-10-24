using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace HENG.UWP.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
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

        private ICommand _feedbackCommand;
        public ICommand FeedbackCommand
        {
            get
            {
                if (_feedbackCommand == null)
                {
                    _feedbackCommand = new RelayCommand(() =>
                    {

                    });
                }
                return _feedbackCommand;
            }
        }

        private ICommand _rateCommand;
        public ICommand RateCommand
        {
            get
            {
                if (_rateCommand == null)
                {
                    _rateCommand = new RelayCommand(() =>
                    {

                    });
                }
                return _rateCommand;
            }
        }
    }
}

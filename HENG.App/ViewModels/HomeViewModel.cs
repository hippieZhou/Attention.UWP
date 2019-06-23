using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENG.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public PhotoGridViewModel PhotoGridViewModel { get; private set; } = new PhotoGridViewModel();
    }
}

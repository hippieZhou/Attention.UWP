using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENG.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        private object _model;
        public object Model
        {
            get { return _model; }
            set { Set(ref _model, value); }
        }
    }
}

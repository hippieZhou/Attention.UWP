using GalaSoft.MvvmLight;

namespace Attention.Models
{
    public class Defined : ObservableObject
    {
        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { Set(ref _checked, value); }
        }
    }
}

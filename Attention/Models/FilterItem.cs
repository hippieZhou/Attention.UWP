using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Attention.Models
{
    public class Filters : ObservableCollection<FilterItem>
    {
    }

    public class FilterItem : ObservableObject
    {
        public string Name { get; set; }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { Set(ref _checked, value); }
        }
    }
}

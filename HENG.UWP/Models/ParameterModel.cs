using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace HENG.UWP.Models
{
    public class ParameterModel : ObservableObject
    {
        public string Name { get; set; }

        private int _selectedIndex = 1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { Set(ref _selectedIndex, value); }
        }

        public IEnumerable<OptionalParameter> Items { get; set; }
    }

    public class OptionalParameter : ObservableObject
    {
        public string Description { get; set; }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { Set(ref _checked, value); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolkitX.Controls
{
    public class TabSelectionChangedEventArgs : EventArgs
    {
        public TabSelectionChangedEventArgs(int oldIndex, int newIndex)
        {
            OldSelectedIndex = oldIndex;
            SelectedIndex = newIndex;
        }

        public int OldSelectedIndex { get; }
        public int SelectedIndex { get; }
    }
}

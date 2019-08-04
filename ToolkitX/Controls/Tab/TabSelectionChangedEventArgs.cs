using System;

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

    public delegate void TabSelectionChangedEventHandler(object sender, TabSelectionChangedEventArgs args);
}
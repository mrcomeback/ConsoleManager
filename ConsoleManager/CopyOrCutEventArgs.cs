using System;

namespace ConsoleManager
{
    internal class CopyOrCutEventArgs : EventArgs
    {
        public ListViewItem ListViewItem;
        public bool CopyOrCut;

        public CopyOrCutEventArgs(ListViewItem listViewItem, bool copyOrCut)
        {
            ListViewItem = listViewItem;
            CopyOrCut = copyOrCut;
        }
    }
}
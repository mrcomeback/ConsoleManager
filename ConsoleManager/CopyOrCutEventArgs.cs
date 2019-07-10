using System;

namespace ConsoleManager
{
    internal class CopyOrCutEventArgs : EventArgs
    {
        public ListViewItem ListViewItem { get; set; }
        public bool CopyOrCut { get; set; }

        public CopyOrCutEventArgs(ListViewItem listViewItem, bool copyOrCut)
        {
            ListViewItem = listViewItem;
            CopyOrCut = copyOrCut;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    internal class ViewInfoEventArgs : EventArgs
    {
        public ListViewItem ListViewItem;

        public ViewInfoEventArgs(ListViewItem item)
        {
            ListViewItem = item; 
        }
    }
}

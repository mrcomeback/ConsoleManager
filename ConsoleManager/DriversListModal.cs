using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class DriversList
    {
        private List<ListViewItem> _listViewsItems;

        public  DriversList()
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();

            _listViewsItems = 
                drivers.Where(i => i.IsReady == true).Select(i =>
                new ListViewItem(
                    i,
                    i.Name,
                    i.TotalSize.ToString()
                    )).ToList(); ;
        }

        public List<ListViewItem> GetDriversList()
        {
            return _listViewsItems;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class DriversListModal
    {
        private List<ListViewItem> _listViews;

        public  DriversListModal()
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();

            _listViews = 
                drivers.Select(i =>
                new ListViewItem(
                    i,
                    i.Name,
                    i.TotalSize.ToString()
                    )).ToList(); ;
        }

        public List<ListViewItem> getDriversList()
        {
            return _listViews;
        }

    }
}

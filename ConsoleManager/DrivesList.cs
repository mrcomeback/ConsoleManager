using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    internal class DrivesList
    {
        private List<ListViewItem> _listViewDriveItems;

        public  DrivesList()
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();

            _listViewDriveItems = 
                drivers.Where(i => i.IsReady == true).Select(i =>
                new ListViewItem(
                    i,
                    i.Name,
                    i.TotalSize.ToString()
                    )).ToList(); ;
        }

        public List<ListViewItem> GetDriversList()
        {
            return _listViewDriveItems;
        }
        public static string[] GetDrivesPathes()
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();
            return drivers.Where(f => f.IsReady == true).Select(i => i.Name).ToArray();
        }
    }
}

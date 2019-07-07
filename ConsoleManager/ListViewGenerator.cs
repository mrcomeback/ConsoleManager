using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class ListViewGenerator
    {
        private const int _widthColumn1 = 35;
        private const int _widthColumn2 = 10;
        private const int _widthColumn3 = 10;
        private List<ListView> _listViews = new List<ListView>() { };

        public List<ListView> GenerateListViews(string[] pathes)
        {
            for (int i = 0; i < pathes.Length; i++)
            {
                var listView = new ListView(i > 0 ? 6 + _widthColumn1 + _widthColumn2 + _widthColumn3 : 3, 2, GetItems(pathes[i]));
                listView.SetColumnsWidth(new List<int> { _widthColumn1, _widthColumn2, _widthColumn3 });
                listView.Select += View_Selected;
                listView.Rename += View_Renamed;
                listView.Paste += View_Paste;
                if (i == 0)
                    listView.Focused = true;
                _listViews.Add(listView);
            }
            return _listViews;
        }
        
        public  List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f =>
                new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? file.Length.ToString() : "")).ToList();
        }

        private void View_Selected(object sender, EventArgs eventArgs)
        {
            var view = (ListView)sender;
            var info = view.GetSelectedItem().State;
            if (info is FileInfo file)
                Process.Start(file.FullName);
            else if (info is DirectoryInfo dir)
            {
                view.Clean();
                view.SetlistViewItems(GetItems(dir.FullName));
            }
        }

        private void View_Renamed(object sender, EventArgs eventArgs)
        {
            ModalWindow modal = new ModalWindow();
            ListView listView = (ListView)sender;
            string userInpur = modal.ShowModalWindow("Enter new file Name");
            var selectedItem = listView.GetSelectedItem();
            string newPath = Path.GetDirectoryName(listView.GetSelectedItem().State.FullName) + "\\" + userInpur;
            
            if (selectedItem.State is FileInfo)
            {
                File.Move(selectedItem.State.FullName, newPath);
            }
            else
            {
                Directory.Move(selectedItem.State.FullName, newPath);
            }
            modal.SetAppColors();
            listView.Clean();
            listView.SetlistViewItems(GetItems(Path.GetDirectoryName(listView.GetSelectedItem().State.FullName)));
        }

        private void View_Paste(object sender, CopyOrCutEventArgs eventArgs)
        {
            ListView listView = (ListView)sender;
            FileSystemInfo senderInfo = listView.GetSelectedItem().State;
            FileSystemInfo sourceInfo = eventArgs.ListViewItem.State;

            if (sourceInfo is FileInfo file)
            {
                if (eventArgs.CopyOrCut == true)
                {
                    var fileToCopy = eventArgs.ListViewItem.State.FullName;
                    var fileToPaste = Path.GetDirectoryName(senderInfo.FullName) + "\\" + Path.GetFileName(eventArgs.ListViewItem.State.FullName);

                    File.Copy(fileToCopy, fileToPaste);
                }

                else if (eventArgs.CopyOrCut == false)
                {
                    var fileToCopy = eventArgs.ListViewItem.State.FullName;
                    var fileToPaste = Path.GetDirectoryName(senderInfo.FullName) + "\\" + Path.GetFileName(eventArgs.ListViewItem.State.FullName);
                    File.Move(fileToCopy, fileToPaste);
                }
            }

            else if (sourceInfo is DirectoryInfo directoryInfo)
            {
                if (eventArgs.CopyOrCut == true)
                {
                    var folderToCopy = eventArgs.ListViewItem.State.FullName;
                    var folderToPaste = Path.GetDirectoryName(senderInfo.FullName) + "\\" + eventArgs.ListViewItem.State.Name;

                    DirectoryOperations.DirectoryCopy(folderToCopy, folderToPaste);
                }

                else if (eventArgs.CopyOrCut == false)
                {
                    var folderToCopy = eventArgs.ListViewItem.State.FullName;
                    var folderToPaste = Path.GetDirectoryName(senderInfo.FullName) + "\\" + eventArgs.ListViewItem.State.Name;

                    Directory.Move(folderToCopy, folderToPaste);
                }
            }

            foreach (ListView lv in _listViews)
            {
                lv.Clean();
                lv.SetlistViewItems(GetItems(Path.GetDirectoryName(lv.GetSelectedItem().State.FullName)));
                lv.Render();
            }
        }
    }
}

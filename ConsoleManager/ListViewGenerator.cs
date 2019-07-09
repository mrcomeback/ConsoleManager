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
        private ModalWindow _modal = new ModalWindow();

        public List<ListView> GenerateListViews(string[] pathes)
        {
            for (int i = 0; i < pathes.Length; i++)
            {
                var listView = new ListView(i > 0 ? 6 + _widthColumn1 + _widthColumn2 + _widthColumn3 : 3, 2, GetItems(pathes[i]));
                listView.SetColumnsWidth(new List<int> { _widthColumn1, _widthColumn2, _widthColumn3 });
                listView.SetCurPath(pathes[i]);
                listView.Select += View_Selected;
                listView.Rename += View_Renamed;
                listView.Paste += View_Paste;
                listView.ViewInfo += View_Info;
                listView.ViewDrives += View_Drives;
                listView.GoTo += Go_To;
                listView.CreateFolder += Create_Folder;
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
                    f is FileInfo file ? file.Length.ToString() : String.Empty)).ToList();
        }
        private void View_Selected(object sender, EventArgs eventArgs)
        {
            var listView = (ListView)sender;
            var info = listView.GetSelectedItem().State;
            if (info is FileInfo file)
            {
                Process.Start(file.FullName);
            }
            else if (info is DirectoryInfo dir)
            {
                listView.SetCurPath(dir.FullName);
                listView.Clean();
                listView.SetlistViewItems(GetItems(dir.FullName));
            }
            else if (info is DriveInfo drive)
            {
                listView.Focused = false;
                listView.IsDrives = true;
                var firstListView = _listViews[0];
                firstListView.Focused = true;
                firstListView.Clean();
                firstListView.SetlistViewItems(GetItems(drive.Name));
                _listViews.Remove(listView);
                UpdateView();
            }
        }
        private void View_Renamed(object sender, EventArgs eventArgs)
        {
            ListView listView = (ListView)sender;
            string userInpur = _modal.ShowModalWindow("Enter new file Name");
            var selectedItem = listView.GetSelectedItem();
            FileSystemInfo state = (FileSystemInfo)listView.GetSelectedItem().State;
            string newPath = Path.GetDirectoryName(state.FullName) + "\\" + userInpur;
            
            if (selectedItem.State is FileInfo)
            {
                File.Move(state.FullName, newPath);
            }
            else
            {
                Directory.Move(state.FullName, newPath);
            }
            _modal.SetAppColors();
            UpdateView();
        }
        private void View_Paste(object sender, CopyOrCutEventArgs eventArgs)
        {
            ListView listView = (ListView)sender;
            //FileSystemInfo senderInfo = (FileSystemInfo)listView.GetSelectedItem().State;
            FileSystemInfo sourceInfo = (FileSystemInfo)eventArgs.ListViewItem.State;
            
            if (sourceInfo is FileInfo file)
            {
                if (eventArgs.CopyOrCut == true)
                {
                    var fileToCopy = sourceInfo.FullName;
                    var fileToPaste = listView.GetCurPath() + "\\" + Path.GetFileName(sourceInfo.FullName);

                    File.Copy(fileToCopy, fileToPaste);
                }

                else if (eventArgs.CopyOrCut == false)
                {
                    var fileToCopy = sourceInfo.FullName;
                    var fileToPaste = listView.GetCurPath() + "\\" + Path.GetFileName(sourceInfo.FullName);
                    File.Move(fileToCopy, fileToPaste);
                }
            }

            else if (sourceInfo is DirectoryInfo directoryInfo)
            {
                if (eventArgs.CopyOrCut == true)
                {
                    var folderToCopy = sourceInfo.FullName;
                    var folderToPaste = listView.GetCurPath() + "\\" + sourceInfo.Name;

                    DirectoryOperations.DirectoryCopy(folderToCopy, folderToPaste);
                }

                else if (eventArgs.CopyOrCut == false)
                {
                    var folderToCopy = sourceInfo.FullName;
                    var folderToPaste = listView.GetCurPath() + "\\" + sourceInfo.Name;

                    Directory.Move(folderToCopy, folderToPaste);
                }
            }

            foreach (ListView lv in _listViews)
            {
                if (lv.GetCurPath() != null)
                {
                    lv.Clean();
                    lv.SetlistViewItems(GetItems(lv.GetCurPath()));
                    lv.Render();
                }
            }
        }
        private void View_Info(object sender, ViewInfoEventArgs eventArgs)
        {
            FileSystemInfo info = (FileSystemInfo)eventArgs.ListViewItem.State;
            string infoStrings = String.Empty;
            int readOnly = ((int)(info.Attributes) & (int)FileAttributes.ReadOnly);
            if (info is FileInfo file)
            {
                infoStrings = $"Name: {info.Name}\r\n" +
                    $"Parent Directory: {Path.GetDirectoryName(info.FullName)}\r\n" +
                    $"Root Directory: {Path.GetPathRoot(info.FullName)}\r\n" +
                    $"Is read only :{((readOnly == 1) ? true : false)} \r\n" +
                    $"Last read time: {info.LastAccessTime}\r\n" +
                    $"Last write time: {info.LastWriteTime}\r\n" +
                    $"Size:{(ulong)file.Length/1024} KB";
            }
            else
            {
                infoStrings = $"Name: {info.Name}\r\n" +
                   $"Parent Directory: {Path.GetDirectoryName(info.FullName)}\r\n" +
                   $"Root Directory: {Path.GetPathRoot(info.FullName)}\r\n" +
                   $"Is read only : {((readOnly == 1) ? true : false)} \r\n" +
                   $"Last read time: {info.LastAccessTime}\r\n" +
                   $"Last write time: {info.LastWriteTime}\r\n" +
                   $"Size:{DirectoryOperations.DirSize(new DirectoryInfo(info.FullName))/1024} KB\r\n" +
                   $"Files:{Directory.GetFiles(info.FullName).Count()}\r\n" +
                   $"Folders:{Directory.GetDirectories(info.FullName).Count()}";
            }
            _modal.ShowModalWindow(infoStrings);
            UpdateView();
        }
        private void View_Drives(object sender, EventArgs eventArgs)
        {
            _listViews.Find(i => i.Focused == true).Focused = false;
            var drivers = new DriversList();
            var lv = new ListView(35, 10, drivers.GetDriversList());
            lv.SetColumnsWidth(new List<int> { 35, 10, 10 });
            lv.Select += View_Selected;
            lv.Focused = true;
            _listViews.Add(lv);
            foreach (ListView list in _listViews)
            {
                list.Render();
            }
        }
        private void Go_To(object sender, RootEventArgs eventArgs)
        {
            var view = (ListView)sender;
            view.Clean();
            view.SetlistViewItems(GetItems(eventArgs.path));
        }
        private void Create_Folder(object sender, EventArgs eventArgs)
        {
            ListView listView = (ListView)sender;
            string userInput = _modal.ShowModalWindow("Enter Folder Name");
            Directory.CreateDirectory(listView.GetCurPath() + "\\" + userInput);
            _modal.SetAppColors();
            UpdateView();
        }
        private void UpdateView()
        {
            Console.Clear();
            Console.WriteLine("[F1] - Copy;[F2] - Cut;[F3] - Paste;[F4] - View File/Directory info; [F5]- Rename; [F6] - View Drives; [F7] - Go to Root");
            foreach (ListView listView in _listViews)
            {
                listView.Clean();
                if (listView.GetSelectedItem().State is FileSystemInfo fileInfo)
                {
                    FileSystemInfo state = fileInfo;
                    listView.SetlistViewItems(GetItems(Path.GetDirectoryName(state.FullName)));
                    listView.Render();
                }
            }
        }
    }
}

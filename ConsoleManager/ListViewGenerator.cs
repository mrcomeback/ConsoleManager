﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleManager
{
    internal class ListViewGenerator
    {
        private const int _widthColumn1 = 35;
        private const int _widthColumn2 = 10;
        private const int _widthColumn3 = 10;
        private List<ListView> _listViews = new List<ListView>() { };
        private ModalWindow _modal = new ModalWindow();

        public List<ListView> GenerateListViews(string[] pathes)
        {
            List<ListView> newListViews = new List<ListView>();
            for (int i = 0; i < pathes.Length; i++)
            {
                var listView = new ListView(i > 0 ? 6 + _widthColumn1 + _widthColumn2 + _widthColumn3 : 3, 2, GetItems(pathes[i]));
                listView.SetColumnsWidth(new List<int> { _widthColumn1, _widthColumn2, _widthColumn3 });
                listView.CurPath =pathes[i];
                listView.Select += View_Selected;
                listView.Rename += View_Renamed;
                listView.Paste += View_Paste;
                listView.ViewInfo += View_Info;
                listView.ViewDrives += View_Drives;
                listView.GoTo += Go_To;
                listView.CreateFolder += Create_Folder;
                if (i == 0)
                    listView.Focused = true;
                newListViews.Add(listView);
            }
            _listViews = newListViews;
            return newListViews;
        }
        
        private  List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f =>
                new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? Utils.NormalizeSize((ulong)file.Length) : String.Empty)).ToList();
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
                listView.CurPath = dir.FullName;
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
            UpdateView();
        }

        private void View_Paste(object sender, CopyOrCutEventArgs eventArgs)
        {
            ListView listView = (ListView)sender;
            FileSystemInfo sourceInfo = (FileSystemInfo)eventArgs.ListViewItem.State;
            
            if (sourceInfo is FileInfo file)
            {
                if (eventArgs.CopyOrCut == true)
                {
                    var fileToCopy = sourceInfo.FullName;
                    var fileToPaste = listView.CurPath + "\\" + Path.GetFileName(sourceInfo.FullName);

                    File.Copy(fileToCopy, fileToPaste);
                }

                else if (eventArgs.CopyOrCut == false)
                {
                    var fileToCopy = sourceInfo.FullName;
                    var fileToPaste = listView.CurPath + "\\" + Path.GetFileName(sourceInfo.FullName);

                    File.Move(fileToCopy, fileToPaste);
                }
            }

            else if (sourceInfo is DirectoryInfo directoryInfo)
            {
                if (eventArgs.CopyOrCut == true)
                {
                    var folderToCopy = sourceInfo.FullName;
                    var folderToPaste = listView.CurPath + "\\" + sourceInfo.Name;

                    Utils.DirectoryCopy(folderToCopy, folderToPaste);
                }

                else if (eventArgs.CopyOrCut == false)
                {
                    var folderToCopy = sourceInfo.FullName;
                    var folderToPaste = listView.CurPath + "\\" + sourceInfo.Name;

                    Directory.Move(folderToCopy, folderToPaste);
                }
            }

            foreach (ListView lv in _listViews)
            {
                if (lv.CurPath != null)
                {
                    lv.Clean();
                    lv.SetlistViewItems(GetItems(lv.CurPath));
                    lv.Render();
                }
            }
        }

        private void View_Info(object sender, ViewInfoEventArgs eventArgs)
        {
            FileSystemInfo info = (FileSystemInfo)eventArgs.ListViewItem.State;
            int readOnly = ((int)(info.Attributes) & (int)FileAttributes.ReadOnly);
            string infoStrings = $"Name: {info.Name}\r\n" +
                $"Parent Directory: {Path.GetDirectoryName(info.FullName)}\r\n" +
                $"Root Directory: {Path.GetPathRoot(info.FullName)}\r\n" +
                $"Is read only :{((readOnly == 1) ? true : false)} \r\n" +
                $"Last read time: {info.LastAccessTime}\r\n" +
                $"Last write time: {info.LastWriteTime}\r\n";

            if (info is FileInfo file)
            {
                infoStrings += 
                    $"Size:{Utils.NormalizeSize((ulong)file.Length)}";
            }
            else
            {
                infoStrings += 
                   $"Size:{Utils.GetDirSize(new DirectoryInfo(info.FullName))}\r\n" +
                   $"Files:{Directory.GetFiles(info.FullName, "*.*", SearchOption.AllDirectories).Count()}\r\n" +
                   $"Folders:{Directory.GetDirectories(info.FullName, "*", SearchOption.AllDirectories).Count()}";
            }
            _modal.ShowModalWindow(infoStrings);
            UpdateView();
        }

        private void View_Drives(object sender, EventArgs eventArgs)
        {
            _listViews.Find(i => i.Focused == true).Focused = false;
            DrivesList drivers = new DrivesList();
            ListView lv = new ListView(35, 10, drivers.GetDriversList());
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
            view.SetlistViewItems(GetItems(eventArgs.Path));
        }

        private void Create_Folder(object sender, EventArgs eventArgs)
        {
            ListView listView = (ListView)sender;
            string userInput = _modal.ShowModalWindow("Enter Folder Name");
            Directory.CreateDirectory(listView.CurPath + "\\" + userInput);
            UpdateView();
        }

        private void UpdateView()
        {
            Console.Clear();
            Console.WriteLine(Utils.CommandsInformation);
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

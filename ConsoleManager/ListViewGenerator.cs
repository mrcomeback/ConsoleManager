﻿using System;
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

        public List<ListView> GenerateListViews(string[] pathes)
        {
            List<ListView> listViews = new List<ListView>() { };

            for (int i = 0; i < pathes.Length; i++)
            {
                var listView = new ListView(i > 0 ? 6 + _widthColumn1 + _widthColumn2 + _widthColumn3 : 3, 2, GetItems(pathes[i]));
                listView.SetColumnsWidth(new List<int> { _widthColumn1, _widthColumn2, _widthColumn3 });
                listView.Selected += View_Selected;
                if (i == 0)
                    listView.Focused = true;
                listViews.Add(listView);
            }
            return listViews;
        }
        
        private List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f =>
                new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? file.Length.ToString() : "")).ToList();
        }

        private void View_Selected(object sender, EventArgs e)
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
    }
}

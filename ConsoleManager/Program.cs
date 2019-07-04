using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            List<ListView> listviews =  ListView.GenerateListViews(new List<string>() { "C:\\", "D:\\" });
            //var listView = new ListView(10, 2, GetItems("C:\\"));
            //listView.ColumnsWidth = new List<int> { 30, 10, 10 };
            //listView.Selected += View_Selected;


            while (true)
            {
                foreach(ListView listView in listviews)
                {
                    listView.Render();
                    var key = Console.ReadKey();
                    listView.Update(key);
                }
            }
        }

        //private static List<ListViewItem> GetItems(string path)
        //{
        //    return new DirectoryInfo(path).GetFileSystemInfos()
        //        .Select(f =>
        //        new ListViewItem(
        //            f,
        //            f.Name,
        //            f is DirectoryInfo dir ? "<dir>" : f.Extension,
        //            f is FileInfo file ? file.Length.ToString() : "")).ToList();
        //}

        //private static void View_Selected(object sender, EventArgs e)
        //{
        //    var view = (ListView)sender;
        //    var info = view._selectedItem.State;
        //    if (info is FileInfo file)
        //        Process.Start(file.FullName);
        //    else if (info is DirectoryInfo dir)
        //    {
        //        view.Clean();
        //        view._items = GetItems(dir.FullName);
        //    }
        //}
    }
}

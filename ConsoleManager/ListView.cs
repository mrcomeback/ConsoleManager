using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class ListView
    {
        private int _prevSelecteIndex;
        private int _selectedIndex;
        private bool _wasPainted;
        public List<ListViewItem> _items { get; set; }

        public List<int> ColumnsWidth { get; set; }
        public ListViewItem _selectedItem => _items[_selectedIndex];
        private bool _focused { get; set; }
        private int _x, _y;
        public void Clean()
        {
            _selectedIndex = _prevSelecteIndex = 0;
            _wasPainted = false;
            for (int i = 0; i < _items.Count; i++)
            {
                Console.CursorLeft = _x;
                Console.CursorTop = i + _y;
                _items[i].Clean(ColumnsWidth, i,_x,_y);
            }
        }

        public ListView(int x , int y, List<ListViewItem> items)
        {
            _x = x;
            _y = y;
            _items = items;
        }

        public void Render()
        {
            for (int i = 0; i < _items.Count; i++)
            {               
                if (_wasPainted && i != _selectedIndex && i != _prevSelecteIndex)
                {
                    continue;
                }

                var item = _items[i];
                var savedForeground = Console.ForegroundColor;
                var savedBackGround = Console.BackgroundColor;
                if (i == _selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.CursorLeft = _x;
                Console.CursorTop = i +_y;
                Console.Write(item);
                item.Render(ColumnsWidth, i,_x,_y);

                Console.ForegroundColor = savedForeground;
                Console.BackgroundColor = savedBackGround;
            }
            _wasPainted = true;
        }

        public void Update(ConsoleKeyInfo key)
        {
            _prevSelecteIndex = _selectedIndex;
            if (key.Key == ConsoleKey.UpArrow && _selectedIndex != 0) {
                _selectedIndex--;
            }   
            else if (key.Key == ConsoleKey.DownArrow && _selectedIndex < _items.Count - 1)
            {
                _selectedIndex++;
            }else if (key.Key == ConsoleKey.Enter)
            {
                Selected(this, EventArgs.Empty);
            }

        }

        public static List<ListView> GenerateListViews(List<string> pathes)
        {
            List<ListView> listViews = new List<ListView>() { };
            int i = 1;
            foreach (string path in pathes)
            {

                var listView = new ListView(10 + i, 2, GetItems(path));
                i = i + 50;
                listView.ColumnsWidth = new List<int> { 30, 10, 10 };
                listView.Selected += View_Selected;
                listViews.Add(listView);
            }
            return listViews;
        }
        public event EventHandler Selected;

        private static List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f =>
                new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? file.Length.ToString() : "")).ToList();
        }

        private static void View_Selected(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view._selectedItem.State;
            if (info is FileInfo file)
                Process.Start(file.FullName);
            else if (info is DirectoryInfo dir)
            {
                view.Clean();
                view._items = GetItems(dir.FullName);
            }
        }
    }
}

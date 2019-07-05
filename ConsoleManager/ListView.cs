using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleManager
{
    class ListView
    {
        private int _prevSelecteIndex;
        private int _selectedIndex;
        private bool _wasPainted;
        private List<ListViewItem> _items;
        private List<int> _columnsWidth;
        private ListViewItem _selectedItem => _items[_selectedIndex];
        private int _x, _y;
        public bool Focused { get; set; }

        public void Clean()
        {
            _selectedIndex = _prevSelecteIndex = 0;
            _wasPainted = false;
            for (int i = 0; i < _items.Count; i++)
            {
                Console.CursorLeft = _x;
                Console.CursorTop = i + _y;
                _items[i].Clean(_columnsWidth, i,_x,_y);
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
                item.Render(_columnsWidth, i,_x,_y);

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
            }else if (key.Key == ConsoleKey.F2)
            {
                Console.WriteLine(_selectedItem);
            }else if (key.Key == ConsoleKey.F5)
            {
                Console.WriteLine(_selectedItem);
                var pathWithoutName = _selectedItem.State.FullName.Substring(0, _selectedItem.State.FullName.Length - _selectedItem.State.Name.Length);
                File.Move(_selectedItem.State.FullName, pathWithoutName + "New Name222.js");


                Render();
                Console.WriteLine();
            }
        }
        public event EventHandler Selected;

        public List<ListViewItem> GetListViewItems()
        {
            return _items;
        }

        public void SetlistViewItems(List<ListViewItem> newItems)
        {
            _items = newItems;
        }

        public void SetColumnsWidth(List<int> columnsWidth)
        {
            _columnsWidth = columnsWidth;
        }

        public List<int> GetColumnsWidth()
        {
            return _columnsWidth;
        }

        public ListViewItem GetSelectedItem()
        {
            return _selectedItem;
        }

        public void CopyDir()
        {

        }

        public void CopyFile()
        {

        }

        public void Rename()
        {

        }
    }
}

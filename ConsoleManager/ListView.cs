using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleManager
{
    internal class ListView
    {
        private int _prevSelecteIndex;
        private int _selectedIndex;
        private bool _wasPainted;
        private List<ListViewItem> _items;
        private List<int> _columnsWidth;
        private ListViewItem _selectedItem => _items.Count == 0 ? null : _items[_selectedIndex];
        private int _x, _y;
        public string CurPath { get; set; }
        public bool IsDrives = false;
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
                    if (Focused == true)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                }

                Console.CursorLeft = _x;
                Console.CursorTop = i +_y;
                Console.Write(item);
                item.Render(_columnsWidth, i, _x, _y);
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
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Select?.Invoke(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F1)
            {
                ItemToOperateOn.CurrentItemToOperateOn = _selectedItem;
                ItemToOperateOn.CopyOrPaste = true;
            }
            else if (key.Key == ConsoleKey.F2)
            {
                ItemToOperateOn.CurrentItemToOperateOn = _selectedItem;
                ItemToOperateOn.CopyOrPaste = false;
            }
            else if (key.Key == ConsoleKey.F3)
            {
                Paste?.Invoke(this, new CopyOrCutEventArgs(ItemToOperateOn.CurrentItemToOperateOn, ItemToOperateOn.CopyOrPaste));
            }
            else if (key.Key == ConsoleKey.F4)
            {
                ItemToOperateOn.CurrentItemToOperateOn = _selectedItem;
                ViewInfo?.Invoke(this, new ViewInfoEventArgs(ItemToOperateOn.CurrentItemToOperateOn));
            }
            else if (key.Key == ConsoleKey.F5)
            {
                Rename?.Invoke(this, EventArgs.Empty);              
            }
            else if (key.Key == ConsoleKey.F6)
            {
                ViewDrives?.Invoke(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F7)
            {
                GoTo?.Invoke(this, new RootEventArgs(Path.GetPathRoot(CurPath)));
            }
            else if (key.Key == ConsoleKey.F8)
            {
                CreateFolder?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler Select;
        public event EventHandler Rename;
        public event EventHandler ViewDrives;
        public event EventHandler CreateFolder;
        public event EventHandler<ViewInfoEventArgs> ViewInfo;
        public event EventHandler<CopyOrCutEventArgs> Paste;
        public event EventHandler<RootEventArgs> GoTo;

        public void SetlistViewItems(List<ListViewItem> newItems)
        {
            _items = newItems;
        }
        public void SetColumnsWidth(List<int> columnsWidth)
        {
            _columnsWidth = columnsWidth;
        }
        public ListViewItem GetSelectedItem()
        {
            return _selectedItem;
        }
    }
}

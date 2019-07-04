using System;
using System.Collections.Generic;


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
        public bool _focused { get; set; }
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
        public event EventHandler Selected;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class ModalWindow
    {
        private int _cursorLeft = Console.WindowWidth / 2 - 20;
        private int _cursorTop = 10;
        private const ConsoleColor _backGroundColor = ConsoleColor.DarkGreen;
        private const ConsoleColor _foreGroundColor = ConsoleColor.Black;


        public string ShowModalWindow(string msg)
        {
            Console.CursorVisible = true;
            Console.WriteLine(" ");
            Console.CursorTop = _cursorTop;
            _setModalColors();
            for (int i = 0; i < 6; i++)
            {
                Console.CursorLeft = _cursorLeft;
                Console.WriteLine(" ".PadLeft(40));
            }
            Console.CursorLeft = _cursorLeft;
            Console.CursorTop = _cursorTop;
            Console.Write(msg);
            Console.CursorLeft = _cursorLeft;
            Console.CursorTop =_cursorTop + 1;
            return Console.ReadLine();
        }


        private void _setModalColors()
        {
            Console.BackgroundColor = _backGroundColor;
            Console.ForegroundColor = _foreGroundColor;
        }
        public void SetAppColors()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}

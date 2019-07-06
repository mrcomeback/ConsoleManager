using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class ModalWindow
    {
        private int _consoleWidth = Console.WindowWidth;
        private int _consoleHeight = Console.WindowHeight;
        private int _cursorLeft => _consoleWidth / 2 - 20;
        private int _cursorTop = 10;


        public string ShowModalWindow(string msg)
        {
            Console.CursorVisible = true;
            Console.WriteLine(" ");
            Console.CursorTop = _cursorTop;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
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
    }
}

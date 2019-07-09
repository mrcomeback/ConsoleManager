using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    internal class ModalWindow
    {
        private int _cursorLeft = Console.WindowWidth / 2 - 20;
        private int _cursorTop = 10;
        private const ConsoleColor _backGroundColor = ConsoleColor.DarkGreen;
        private const ConsoleColor _foreGroundColor = ConsoleColor.White;

        public string ShowModalWindow(string msg)
        {
            Console.CursorVisible = true;
            Console.CursorTop = _cursorTop;
            _setModalColors();
            for (int i = 0; i < 10; i++)
            {
                Console.CursorLeft = _cursorLeft;
                Console.WriteLine(" ".PadLeft(40));
            }
            Console.CursorLeft = _cursorLeft;
            Console.CursorTop = _cursorTop;
            if (msg.Contains("\r\n"))
            {
                string[] msgArr = msg.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach(string str in msgArr)
                {
                    Console.Write(str);
                    Console.CursorLeft = _cursorLeft;
                    Console.CursorTop = _cursorTop = _cursorTop + 1;
                }
            }
            else
            {
                Console.Write(msg);
                Console.CursorLeft = _cursorLeft;
                Console.CursorTop = _cursorTop + 1;
            }
            _cursorTop = 10;
            string res = Console.ReadLine();
            SetAppColors();
            return res;
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

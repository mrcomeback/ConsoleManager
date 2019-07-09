using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleManager
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            ListViewGenerator listViewGenerator = new ListViewGenerator();
            FocusManager focusManager = new FocusManager();
            ModalWindow modalWindow = new ModalWindow();
            List<ListView> listviews = listViewGenerator.GenerateListViews(new string[] { "E:\\", "E:\\"});
            Console.WriteLine("[F1] - Copy;[F2] - Cut;[F3] - Paste;[F4] - View File/Directory info; [F5]- Rename; [F6] - View Drives; [F7] - Go to Root ");

            foreach (ListView listView in listviews)
            {
                listView.Render();
            }

            while (true)
            {
                var listViewtoUpdate = listviews.Find(i => i.Focused == true);

                while (listViewtoUpdate.Focused == true)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow) {
                        listviews = focusManager.ChangeFocus(listviews, key);
                        listViewtoUpdate.Update(key);
                    }
                    else
                    {
                        listViewtoUpdate.Update(key);
                    }
                    if(listViewtoUpdate.IsDrives != true)
                    listViewtoUpdate.Render();
                }
            }
        }
    }
}

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
            //FCommandsManager fCommandsManager = new FCommandsManager();
            List<ListView> listviews = listViewGenerator.GenerateListViews(new string[] { "E:\\", "E:\\"});
            Console.WriteLine("[F2] - Copy;[F3] - Paste;[F4] - View All discs,[F5]- Rename");

            foreach (ListView listView in listviews)
            {
                listView.Render();
            }

            while (true)
            {
                var listViewtoUpdate = listviews.Find(i => i.Focused == true);

                while (listViewtoUpdate.Focused == true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow) {
                        listviews = focusManager.ChangeFocus(listviews, key);
                        listViewtoUpdate.Update(key, listViewGenerator);
                    }
                    else
                    {
                        listViewtoUpdate.Update(key, listViewGenerator);
                    }
                    listViewtoUpdate.Render();
                }
            }
        }
    }
}

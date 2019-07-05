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
            List<ListView> listviews = listViewGenerator.GenerateListViews(new string[] { "C:\\", "C:\\"});
            Console.WriteLine("[F2] - Copy;[F3] - Paste;[F5] - View All discs");

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
                        listViewtoUpdate.Update(key);
                        listViewtoUpdate.Render();
                    }
                    else
                    {
                        listViewtoUpdate.Update(key);
                        listViewtoUpdate.Render();
                    }
                }
            }
        }
    }
}

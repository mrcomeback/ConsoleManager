using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            ListViewGenerator listViewGenerator = new ListViewGenerator();
            FocusManager focusManager = new FocusManager();
            List<ListView> listviews = listViewGenerator.GenerateListViews(new string[] { "C:\\", "E:\\"});

            foreach (ListView listView in listviews)
            {
                listView.Render();
            }

            while (true)
            {
                var listViewtoUpdate = listviews.Find(i => i._focused == true);

                while (listViewtoUpdate._focused == true)
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

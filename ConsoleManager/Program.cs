using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleManager
{
    internal class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            ListViewGenerator listViewGenerator = new ListViewGenerator();
            FocusManager focusManager = new FocusManager();
            List<ListView> listViews = listViewGenerator.GenerateListViews(DrivesList.GetDrivesPathes());
            Console.WriteLine("[F1] - Copy;[F2] - Cut;[F3] - Paste;[F4] - View File/Directory info; [F5]- Rename; [F6] - View Drives; [F7] - Go to Root; [F8]- Create Folder");

            foreach (ListView listView in listViews)
            {
                listView.Render();
            }

            while (true)
            {
                try
                {
                    var listViewtoUpdate = listViews.Find(i => i.Focused == true);

                    while (listViewtoUpdate.Focused == true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                        {
                            listViews = focusManager.ChangeFocus(listViews, key);
                            listViewtoUpdate.Update(key);
                        }
                        else
                        {
                            listViewtoUpdate.Update(key);
                        }
                        if (listViewtoUpdate.IsDrives != true)
                            listViewtoUpdate.Render();
                    }
                }
                catch
                {
                    ModalWindow modal = new ModalWindow();
                    modal.ShowModalWindow("THE EXCEPTION IS HERE");
                    Console.Clear();
                    listViews = listViewGenerator.GenerateListViews(DrivesList.GetDrivesPathes());
                    foreach (ListView listView in listViews)
                    {
                        listView.Render();
                    }
                }
            }
        }
    }
}

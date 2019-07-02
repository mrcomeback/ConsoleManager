using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var listView = new ListView(new DirectoryInfo("C:\\").GetFileSystemInfos().ToList());

            while (true)
            {
                listView.Render();
                var key = Console.ReadKey();
                listView.Update(key);
            }
        }
    }
}

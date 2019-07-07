using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    internal static class DirectoryOperations
    {
        public static void DirectoryCopy(string sourceName, string destinationName, bool copySubDirs = true)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(sourceName);

            if (!directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceName);
            }

            DirectoryInfo[] dirs = directoryInfo.GetDirectories();

            if (!Directory.Exists(destinationName))
            {
                Directory.CreateDirectory(destinationName);
            }

            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationName, file.Name);
                file.CopyTo(tempPath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destinationName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}

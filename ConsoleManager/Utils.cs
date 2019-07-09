using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManager
{
    internal static class Utils
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
        public static ulong DirSize(DirectoryInfo dir)
        {
            ulong size = 0;
            FileInfo[] fis = dir.GetFiles();

            foreach (FileInfo fi in fis)
            {
                size += (ulong)fi.Length;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (DirectoryInfo di in dirs)
            {
                size += DirSize(di);
            }

            return size;
        }
        public static string GetDirSize(DirectoryInfo dir)
        {
            return NormalizeSize(DirSize(dir));
        }
        public static string NormalizeSize(this ulong bytes)
        {
            if (bytes < 1024)
                return $"{bytes} Byte";

            ulong kbytes = bytes / 1024;

            if (kbytes < 1024)
                return $"{kbytes} KB";

            ulong mbytes = kbytes / 1024;

            if (mbytes < 1024)
                return $"{mbytes} MB";

            ulong gbytes = mbytes / 1024;

            if (gbytes < 1024)
                return $"{gbytes} GB";

            ulong tbytes = gbytes / 1024;

            return $"{kbytes} TB";
        }
    }
}

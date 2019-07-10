using System;

namespace ConsoleManager
{
    internal class RootEventArgs : EventArgs
    {
        public string Path { get; set; }

        public RootEventArgs(string path)
        {
            this.Path = path;
        }
    }
}
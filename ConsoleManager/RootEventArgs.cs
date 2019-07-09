using System;

namespace ConsoleManager
{
    internal class RootEventArgs : EventArgs
    {
        public string path;

        public RootEventArgs(string path)
        {
            this.path = path;
        }
    }
}
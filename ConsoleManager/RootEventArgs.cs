using System;

namespace ConsoleManager
{
    public class RootEventArgs : EventArgs
    {
        public string path;

        public RootEventArgs(string path)
        {
            this.path = path;
        }
    }
}
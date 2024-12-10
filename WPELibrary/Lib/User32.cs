using System;
using System.Runtime.InteropServices;

namespace WPELibrary.Lib
{
    public class User32
    {
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
    }
}

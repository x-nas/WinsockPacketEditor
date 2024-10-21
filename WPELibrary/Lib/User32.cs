using System.Runtime.InteropServices;

namespace WPELibrary.Lib
{
    internal class User32
    {
        #region//SetProcessDPIAware

        [DllImport("user32.dll")]

        public static extern bool SetProcessDPIAware();

        #endregion
    }
}

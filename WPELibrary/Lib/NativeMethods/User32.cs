using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WPELibrary.Lib.NativeMethods
{
    public static class User32
    {
        public enum KeyModifiers
        {
            MOD_ALT = 0x0001,
            MOD_CONTROL = 0x0002,
            MOD_SHIFT = 0x0004,
            MOD_WIN = 0x0008
        }

        public static int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}

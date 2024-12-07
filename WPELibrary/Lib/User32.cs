using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace WPELibrary.Lib
{
    public class User32
    {
        #region//window消息及其他常量

        public class WindowsConstant
        {
            //系统消息
            public const int WM_PASTE = 0x0302;

            //按键状态
            public const int KEYEVENTF_KEYDOWN = 0x0000; //键被按下
            public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //是扩展键
            public const int KEYEVENTF_KEYUP = 0x0002; //键被释放

            public const int GWL_EXSTYLE = -20;
            public const int WS_DISABLED = 0X8000000;
            public const int WM_SETFOCUS = 0X0007;
            public const int WM_SETTEXT = 0X000C;
            public const int WM_COPYDATA = 0X004A;

            //鼠标消息
            public const int WM_MOUSEMOVE = 0x200;
            public const int WM_LBUTTONDOWN = 0x201;
            public const int WM_LBUTTONUP = 0x202;
            public const int WM_LBUTTONDBLCLK = 0x203;
            public const int WM_RBUTTONDOWN = 0x204;
            public const int WM_RBUTTONUP = 0x205;
            public const int WM_RBUTTONDBLCLK = 0x206;
            public const int WM_MBUTTONDOWN = 0x207;
            public const int WM_MBUTTONUP = 0x208;
            public const int WM_MOUSEWHEEL = 0x020A;

            //键盘状态消息
            public const int WM_KEYDOWN = 0x100;
            public const int WM_KEYUP = 0x101;
            public const int WM_SYSKEYDOWN = 0x104;
            public const int WM_SYSKEYUP = 0x105;


            // 鼠标状态
            public const int MOUSEEVENTF_MOVE = 0x0001;        // 移动鼠标位置
            public const int MOUSEEVENTF_LEFTDOWN = 0x0002;    // 按下左键
            public const int MOUSEEVENTF_LEFTUP = 0x0004;      // 松开左键
            public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;   // 按下右键
            public const int MOUSEEVENTF_RIGHTUP = 0x0010;     // 松开右键

            public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;  //中间按钮已关闭。
            public const int MOUSEEVENTF_MIDDLEUP = 0x0040;    //中间按钮已向上。
            public const int MOUSEEVENTF_WHEEL = 0x0800;       //滚轮按钮已旋转。
            public const int MOUSEEVENTF_XDOWN = 0x0080;       //按下了 X 按钮。
            public const int MOUSEEVENTF_XUP = 0x0100;         //已释放 X 按钮。
            public const int MOUSEEVENTF_HWHEEL = 0x01000;     //滚轮按钮倾斜。

            public const int MOUSEEVENTF_ABSOLUTE = 0x8000;    /* dx 和 dy 参数包含规范化的绝对坐标。
                                                            如果未设置，则这些参数包含相对数据：自上次报告的位置以来
                                                            的位置变化。无论哪种类型的鼠标或类似鼠标的设备（如果有）
                                                            连接到系统，都可以设置或不设置此标志。有关相对鼠标运动的
                                                            详细信息，请参阅以下“备注”部分。
                                                            */

            public const uint INPUT_MOUSE = 0x0004;
        }

        #endregion

        #region//虚拟代码（键盘鼠标）

        public class VKCODE
        {
            public const byte VK_LBUTTON = 0x01;//鼠标左键
            public const byte VK_RBUTTON = 0x02;//鼠标右键
            public const byte VK_CANCEL = 0x03;//控制中断处理
            public const byte VK_MBUTTON = 0x04;//鼠标中键
            public const byte VK_XBUTTON1 = 0x05;//X1 鼠标按钮
            public const byte VK_XBUTTON2 = 0x06;//X2 鼠标按钮         
            public const byte VK_BACK = 0x08;//BACKSPACE 键
            public const byte VK_TAB = 0x09;//Tab 键                                      
            public const byte VK_CLEAR = 0x0C;//CLEAR 键
            public const byte VK_RETURN = 0x0D;//Enter 键
            public const byte VK_SHIFT = 0x10;//SHIFT 键
            public const byte VK_CONTROL = 0x11;//CTRL 键
            public const byte VK_MENU = 0x12;//Alt 键
            public const byte VK_PAUSE = 0x13;//PAUSE 键
            public const byte VK_CAPITAL = 0x14;//CAPS LOCK 键
            public const byte VK_KANA = 0x15;//IME Kana 模式
            public const byte VK_HANGUL = 0x15;//IME Hanguel 模式
            public const byte VK_IME_ON = 0x16;//IME 打开
            public const byte VK_JUNJA = 0x17;//IME Junja 模式
            public const byte VK_FINAL = 0x18;//IME 最终模式
            public const byte VK_HANJA = 0x19;//IME Hanja 模式
            public const byte VK_KANJI = 0x19;//IME Kanji 模式
            public const byte VK_IME_OFF = 0x1A;//IME 关闭
            public const byte VK_ESCAPE = 0x1B;//ESC 键
            public const byte VK_CONVERT = 0x1C;//IME 转换
            public const byte VK_NONCONVERT = 0x1D;//IME 不转换
            public const byte VK_ACCEPT = 0x1E;//IME 接受
            public const byte VK_MODECHANGE = 0x1F;//IME 模式更改请求
            public const byte VK_SPACE = 0x20;//空格键
            public const byte VK_PRIOR = 0x21;//PAGE UP 键
            public const byte VK_NEXT = 0x22;//PAGE DOWN 键
            public const byte VK_END = 0x23;//END 键
            public const byte VK_HOME = 0x24;//HOME 键
            public const byte VK_LEFT = 0x25;//LEFT ARROW 键
            public const byte VK_UP = 0x26;//UP ARROW 键
            public const byte VK_RIGHT = 0x27;//RIGHT ARROW 键
            public const byte VK_DOWN = 0x28;//DOWN ARROW 键
            public const byte VK_SELECT = 0x29;//SELECT 键
            public const byte VK_PRINT = 0x2A;//PRINT 键
            public const byte VK_EXECUTE = 0x2B;//EXECUTE 键
            public const byte VK_SNAPSHOT = 0x2C;//PRINT SCREEN 键
            public const byte VK_INSERT = 0x2D;//INS 键
            public const byte VK_DELETE = 0x2E;//DEL 键
            public const byte VK_HELP = 0x2F;//HELP 键
            public const byte VK_D0 = 0x30;//0 键
            public const byte VK_D1 = 0x31;//1 键
            public const byte VK_D2 = 0x32;//2 键
            public const byte VK_D3 = 0x33;//3 键
            public const byte VK_D4 = 0x34;//4 键
            public const byte VK_D5 = 0x35;//5 键
            public const byte VK_D6 = 0x36;//6 键
            public const byte VK_D7 = 0x37;//7 键
            public const byte VK_D8 = 0x38;//8 键
            public const byte VK_D9 = 0x39;//9 键
            public const byte VK_A = 0x41;//A 键
            public const byte VK_B = 0x42;//B 键
            public const byte VK_C = 0x43;//C 键
            public const byte VK_D = 0x44;//D 键
            public const byte VK_E = 0x45;//E 键
            public const byte VK_F = 0x46;//F 键
            public const byte VK_G = 0x47;//G 键
            public const byte VK_H = 0x48;//H 键
            public const byte VK_I = 0x49;//I 键
            public const byte VK_J = 0x4A;//J 键
            public const byte VK_K = 0x4B;//K 键
            public const byte VK_L = 0x4C;//L 键
            public const byte VK_M = 0x4D;//M 键
            public const byte VK_N = 0x4E;//N 键
            public const byte VK_O = 0x4F;//O 键
            public const byte VK_P = 0x50;//P 键
            public const byte VK_Q = 0x51;//Q 键
            public const byte VK_R = 0x52;//R 键
            public const byte VK_S = 0x53;//S 键
            public const byte VK_T = 0x54;//T 键
            public const byte VK_U = 0x55;//U 键
            public const byte VK_V = 0x56;//V 键
            public const byte VK_W = 0x57;//W 键
            public const byte VK_X = 0x58;//X 键
            public const byte VK_Y = 0x59;//Y 键
            public const byte VK_Z = 0x5A;//Z 键
            public const byte VK_LWIN = 0x5B;//左 Windows 键
            public const byte VK_RWIN = 0x5C;//右侧 Windows 键
            public const byte VK_APPS = 0x5D;//应用程序密钥
            public const byte VK_SLEEP = 0x5F;//计算机休眠键
            public const byte VK_NUMPAD0 = 0x60;//数字键盘 0 键
            public const byte VK_NUMPAD1 = 0x61;//数字键盘 1 键
            public const byte VK_NUMPAD2 = 0x62;//数字键盘 2 键
            public const byte VK_NUMPAD3 = 0x63;//数字键盘 3 键
            public const byte VK_NUMPAD4 = 0x64;//数字键盘 4 键
            public const byte VK_NUMPAD5 = 0x65;//数字键盘 5 键
            public const byte VK_NUMPAD6 = 0x66;//数字键盘 6 键
            public const byte VK_NUMPAD7 = 0x67;//数字键盘 7 键
            public const byte VK_NUMPAD8 = 0x68;//数字键盘 8 键
            public const byte VK_NUMPAD9 = 0x69;//数字键盘 9 键
            public const byte VK_MULTIPLY = 0x6A;//乘号键
            public const byte VK_ADD = 0x6B;//加号键
            public const byte VK_SEPARATOR = 0x6C;//分隔符键
            public const byte VK_SUBTRACT = 0x6D;//减号键
            public const byte VK_DECIMAL = 0x6E;//句点键
            public const byte VK_DIVIDE = 0x6F;//除号键
            public const byte VK_F1 = 0x70;//F1 键
            public const byte VK_F2 = 0x71;//F2 键
            public const byte VK_F3 = 0x72;//F3 键
            public const byte VK_F4 = 0x73;//F4 键
            public const byte VK_F5 = 0x74;//F5 键
            public const byte VK_F6 = 0x75;//F6 键
            public const byte VK_F7 = 0x76;//F7 键
            public const byte VK_F8 = 0x77;//F8 键
            public const byte VK_F9 = 0x78;//F9 键
            public const byte VK_F10 = 0x79;//F10 键
            public const byte VK_F11 = 0x7A;//F11 键
            public const byte VK_F12 = 0x7B;//F12 键
            public const byte VK_F13 = 0x7C;//F13 键
            public const byte VK_F14 = 0x7D;//F14 键
            public const byte VK_F15 = 0x7E;//F15 键
            public const byte VK_F16 = 0x7F;//F16 键
            public const byte VK_F17 = 0x80;//F17 键
            public const byte VK_F18 = 0x81;//F18 键
            public const byte VK_F19 = 0x82;//F19 键
            public const byte VK_F20 = 0x83;//F20 键
            public const byte VK_F21 = 0x84;//F21 键
            public const byte VK_F22 = 0x85;//F22 键
            public const byte VK_F23 = 0x86;//F23 键
            public const byte VK_F24 = 0x87;//F24 键
            public const byte VK_NUMLOCK = 0x90;//NUM LOCK 键
            public const byte VK_SCROLL = 0x91;//SCROLL LOCK 键
            public const byte VK_LSHIFT = 0xA0;//左 SHIFT 键
            public const byte VK_RSHIFT = 0xA1;//右 SHIFT 键
            public const byte VK_LCONTROL = 0xA2;//左 Ctrl 键
            public const byte VK_RCONTROL = 0xA3;//右 Ctrl 键
            public const byte VK_LMENU = 0xA4;//左 ALT 键
            public const byte VK_RMENU = 0xA5;//右 ALT 键
            public const byte VK_BROWSER_BACK = 0xA6;//浏览器后退键
            public const byte VK_BROWSER_FORWARD = 0xA7;//浏览器前进键
            public const byte VK_BROWSER_REFRESH = 0xA8;//浏览器刷新键
            public const byte VK_BROWSER_STOP = 0xA9;//浏览器停止键
            public const byte VK_BROWSER_SEARCH = 0xAA;//浏览器搜索键
            public const byte VK_BROWSER_FAVORITES = 0xAB;//浏览器收藏键
            public const byte VK_BROWSER_HOME = 0xAC;//浏览器“开始”和“主页”键
            public const byte VK_VOLUME_MUTE = 0xAD;//静音键
            public const byte VK_VOLUME_DOWN = 0xAE;//音量减小键
            public const byte VK_VOLUME_UP = 0xAF;//音量增加键
            public const byte VK_MEDIA_NEXT_TRACK = 0xB0;//下一曲目键
            public const byte VK_MEDIA_PREV_TRACK = 0xB1;//上一曲目键
            public const byte VK_MEDIA_STOP = 0xB2;//停止媒体键
            public const byte VK_MEDIA_PLAY_PAUSE = 0xB3;//播放/暂停媒体键
            public const byte VK_LAUNCH_MAIL = 0xB4;//启动邮件键
            public const byte VK_LAUNCH_MEDIA_SELECT = 0xB5;//选择媒体键
            public const byte VK_LAUNCH_APP1 = 0xB6;//启动应用程序 1 键
            public const byte VK_LAUNCH_APP2 = 0xB7;//启动应用程序 2 键
            public const byte VK_OEM_1 = 0xBA;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键;:
            public const byte VK_OEM_PLUS = 0xBB;//对于任何国家/地区，键+
            public const byte VK_OEM_COMMA = 0xBC;//对于任何国家/地区，键,
            public const byte VK_OEM_MINUS = 0xBD;//对于任何国家/地区，键-
            public const byte VK_OEM_PERIOD = 0xBE;//对于任何国家/地区，键.
            public const byte VK_OEM_2 = 0xBF;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键/?
            public const byte VK_OEM_3 = 0xC0;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键`~
            public const byte VK_OEM_4 = 0xDB;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键[{
            public const byte VK_OEM_5 = 0xDC;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键\\|
            public const byte VK_OEM_6 = 0xDD;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键]}
            public const byte VK_OEM_7 = 0xDE;//用于杂项字符；它可能因键盘而异。 对于美国标准键盘，键'"
            public const byte VK_OEM_8 = 0xDF;//用于杂项字符；它可能因键盘而异。
            public const byte VK_OEM_102 = 0xE2;//美国标准键盘上的 <> 键，或非美国 102 键键盘上的 \\| 键
            public const byte VK_PROCESSKEY = 0xE5;//IME PROCESS 键
            public const byte VK_PACKET = 0xE7;//用于将 Unicode 字符当作键击传递。 VK_PACKET 键是用于非键盘输入法的 32 位虚拟键值的低位字。 有关更多信息，请参阅 KEYBDINPUT、SendInput、WM_KEYDOWN 和 WM_KEYUP 中的注释                                   
            public const byte VK_ATTN = 0xF6;//Attn 键
            public const byte VK_CRSEL = 0xF7;//CrSel 键
            public const byte VK_EXSEL = 0xF8;//ExSel 键
            public const byte VK_EREOF = 0xF9;//Erase EOF 键
            public const byte VK_PLAY = 0xFA;//Play 键
            public const byte VK_ZOOM = 0xFB;//Zoom 键
            public const byte VK_PA1 = 0xFD;//PA1 键
            public const byte VK_OEM_CLEAR = 0xFE;//Clear 键
        }

        #endregion

        #region//鼠标键盘输入的结构体

        [StructLayout(LayoutKind.Sequential)]
        internal struct Input
        {
            public int type;
            public InputUnion U;

        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            /* dx、dy不是以象素为单位的，而是以鼠标设备移动量为单位的，它们之间的比值受鼠标移动速度设置的影响。
             * dwFlags可以设置一个MOUSEEVENTF ABSOLUTE标志，这使得可以用另外一种方法移动标，
             * 当dwFlags设置了MOUSEEVENTF ABSOLUTE标志，dx、dy为屏幕坐标值，表示将鼠标移动到dx，dy的位置，
             * 但是这个坐标值也不是以象素为单位的。这个值的范围是0到65535(SFFFF)，当dx等于0、dy等于0时表示屏幕的最左上角，
             * 当dx等于65535、d等于65535时表示屏幕的最右下角，相当于将屏幕的宽和高分别65536等分。
             * API函数GetSystemMetrics(SM_CXSCREEN-0)可以返回屏幕的宽度，函数GetSystemMetrics(SM_CYSCREEN=1)可以返回屏幕的高度，
             * 利用屏幕的宽度和高度就可以将象素坐标换算成相应的dx、dy。注意: 这种换算最多会出现1象素的误差。
             */
            public int dx;              // 鼠标移动时的x轴坐标差(不是象素单位)，在鼠标移动时有效
            public int dy;              // 鼠标移动时的y轴坐标差(不是象素单位)，在鼠标移动时有效
            public int mouseData;       /* 鼠标滚轮滚动值，在滚动鼠标滚轮时有效。
                                       当mouseData小于0时向下滚动，当mouseData大于0时向上滚动，
                                       mouseData的绝对值一般设为120*/
            public int dwFlags;         /* dwFlags指定鼠标所进行的操作，例，MOUSEEVENTF_MOVE表示移动光标，
                                       MOUSEEVENTF_LEFTDOWN表示按下鼠标左键，MOUSEEVENTF LEFTUP表示放开鼠标左键。
                                    */
            public int time;            // 时间戳，可以使用API函数GetTickCount的返回值，
            public IntPtr dwExtraInfo;  // 扩展信息，可以使用API函教GetMessageExtralnfo的返回值。
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        internal class InputType
        {
            public const int MOUSE = 0;
            public const int KEYBOARD = 1;
            public const int HARDWARE = 2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        #endregion

        #region//用到的Window API

        internal static class NativeMethods
        {
            [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("User32.dll", EntryPoint = "SendInput", CharSet = CharSet.Auto)]
            internal static extern UInt32 SendInput(UInt32 nInputs, Input[] pInputs, Int32 cbSize);

            [DllImport("Kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Auto)]
            internal static extern int GetTickCount();

            [DllImport("User32.dll", EntryPoint = "GetKeyState", CharSet = CharSet.Auto)]
            internal static extern short GetKeyState(int nVirtKey);

            [DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);          

            [DllImport("user32.dll")]
            internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
            internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            public static extern bool SetProcessDPIAware();
        }

        #endregion

        #region//屏幕分辨率和尺寸

        /// <summary>
        /// 系统指标或系统配置设置，检索的所有维度都以像素为单位。
        /// </summary>
        public class SystemMetricsHelper
        {
            public const int SM_CXSCREEN = 0;//主显示器的屏幕宽度（以像素为单位）。 这是通过调用 GetDeviceCaps 获取的相同值，如下所示： GetDeviceCaps( hdcPrimaryMonitor, HORZRES)。
            public const int SM_CYSCREEN = 1;//主显示器的屏幕高度（以像素为单位）。 这是通过调用 GetDeviceCaps 获取的相同值，如下所示： GetDeviceCaps( hdcPrimaryMonitor, VERTRES)。
            public const int SM_CXVSCROLL = 2;//垂直滚动条的宽度（以像素为单位）。
            public const int SM_CYHSCROLL = 3;//水平滚动条的高度（以像素为单位）。
            public const int SM_CYCAPTION = 4;//描述文字区域的高度（以像素为单位）。窗口标题的高度（实际标题高度加上SM_CYBORDER）
            public const int SM_CXBORDER = 5;//窗口边框的宽度（以像素为单位）。 这等效于具有 3D 外观的窗口的 SM_CXEDGE 值。
            public const int SM_CYBORDER = 6;//窗口边框的高度（以像素为单位）。 这等效于具有 3D 外观的窗口的 SM_CYEDGE 值。
            public const int SM_CXFIXEDFRAME = 7;//窗口周围具有描述文字但不是相当大的（以像素为单位）的框架的粗细。 SM_CXFIXEDFRAME是水平边框的高度，SM_CYFIXEDFRAME是垂直边框的宽度。
            public const int SM_CXDLGFRAME = 7;//对话框边框宽度/此值与 SM_CXFIXEDFRAME 相同。
            public const int SM_CYFIXEDFRAME = 8;//窗口周围具有描述文字但不是相当大的（以像素为单位）的框架的粗细。 SM_CXFIXEDFRAME是水平边框的高度，SM_CYFIXEDFRAME是垂直边框的宽度。
            public const int SM_CYDLGFRAME = 8;//对话框边框高度/此值与 SM_CYFIXEDFRAME 相同。SM_CYFIXEDFRAME
            public const int SM_CYVTHUMB = 9;//垂直滚动条上滑块的宽度/垂直滚动条中拇指框的高度（以像素为单位）。
            public const int SM_CXHTHUMB = 10;//水平滚动条上滑块的宽度/水平滚动条中拇指框的宽度（以像素为单位）。
            public const int SM_CXICON = 11;//图标的系统大宽度（以像素为单位）。 LoadIcon 函数只能加载具有SM_CXICON和SM_CYICON指定尺寸的图标。 有关详细信息 ，请参阅图标大小 。
            public const int SM_CYICON = 12;//图标的系统高度（以像素为单位）。 LoadIcon 函数只能加载具有SM_CXICON和SM_CYICON指定尺寸的图标。 有关详细信息 ，请参阅图标大小 。
            public const int SM_CXCURSOR = 13;//光标的标称宽度（以像素为单位）。
            public const int SM_CYCURSOR = 14;//光标的标称高度（以像素为单位）。
            public const int SM_CYMENU = 15;//单行菜单栏的高度（以像素为单位）。
            public const int SM_CXFULLSCREEN = 16;//主显示器上全屏窗口的工作区宽度（以像素为单位）。 若要获取系统任务栏或应用程序桌面工具栏未遮挡的屏幕部分的坐标，请使用SPI_GETWORKAREA值调用 SystemParametersInfo 函数。
            public const int SM_CYFULLSCREEN = 17;//主显示器上全屏窗口的工作区高度（以像素为单位）。 若要获取系统任务栏或应用程序桌面工具栏未遮挡的屏幕部分的坐标，请使用 SPI_GETWORKAREA 值调用 SystemParametersInfo 函数。
            public const int SM_CYKANJIWINDOW = 18;//对于系统的双字节字符集版本，这是屏幕底部的汉字窗口的高度（以像素为单位）。
            public const int SM_MOUSEPRESENT = 19;//如果安装了鼠标，则为非零值;否则为 0。 此值很少为零，因为支持虚拟鼠标，并且某些系统检测到端口的存在，而不是鼠标的存在。
            public const int SM_CYVSCROLL = 20;//垂直滚动条上箭头位图的高度（以像素为单位）。
            public const int SM_CXHSCROLL = 21;//水平滚动条上箭头位图的宽度（以像素为单位）。
            public const int SM_DEBUG = 22;//如果安装了User.exe的调试版本，则为非零;否则为 0。
            public const int SM_SWAPBUTTON = 23;//如果交换了鼠标左键和右键的含义，则为非零值;否则为 0。

            public const int SM_CXMIN = 28;//窗口的最小宽度（以像素为单位）。
            public const int SM_CYMIN = 29;//窗口的最小高度（以像素为单位）。
            public const int SM_CXSIZE = 30;//窗口中按钮的宽度描述文字或标题栏（以像素为单位）。
            public const int SM_CYSIZE = 31;//窗口中按钮的高度描述文字或标题栏（以像素为单位）。SM_CXSIZEFRAME
            public const int SM_CXSIZEFRAME = 32;//可调整大小的窗口周边的大小边框的粗细（以像素为单位）。 SM_CXSIZEFRAME是水平边框的宽度，SM_CYSIZEFRAME是垂直边框的高度。此值与 SM_CXFRAME 相同。
            public const int SM_CXFRAME = 32;//此值与 SM_CXSIZEFRAME 相同。
            public const int SM_CYSIZEFRAME = 33;//可调整大小的窗口周边的大小边框的粗细（以像素为单位）。 SM_CXSIZEFRAME是水平边框的宽度，SM_CYSIZEFRAME是垂直边框的高度。此值与 SM_CYFRAME 相同。
            public const int SM_CYFRAME = 33;//此值与 SM_CYSIZEFRAME 相同。
            public const int SM_CXMINTRACK = 34;//窗口的最小跟踪宽度（以像素为单位）。 用户无法将窗口框架拖动到小于这些尺寸的大小。 窗口可以通过处理 WM_GETMINMAXINFO 消息来替代此值。
            public const int SM_CYMINTRACK = 35;//窗口的最小跟踪高度（以像素为单位）。 用户无法将窗口框架拖动到小于这些尺寸的大小。 窗口可以通过处理 WM_GETMINMAXINFO 消息来替代此值。
            public const int SM_CXDOUBLECLK = 36;//矩形围绕双击序列中第一次单击的位置的宽度（以像素为单位）。 第二次单击必须在由 SM_CXDOUBLECLK 和 SM_CYDOUBLECLK 定义的矩形内发生，
                                                 //系统才能将两次单击视为双击。 两次单击也必须在指定时间内发生。若要设置双击矩形的宽度，请使用SPI_SETDOUBLECLKWIDTH调用 SystemParametersInfo 。
            public const int SM_CYDOUBLECLK = 37;//矩形围绕双击序列中第一次单击的位置的高度（以像素为单位）。 第二次单击必须在由 SM_CXDOUBLECLK 定义的矩形内发生，SM_CYDOUBLECLK系统会将两次单击视为双击。
                                                 //两次单击也必须在指定时间内发生。若要设置双击矩形的高度，请使用SPI_SETDOUBLECLKHEIGHT调用 SystemParametersInfo 。
            public const int SM_CXICONSPACING = 38;//大图标视图中项的网格单元格的宽度（以像素为单位）。 每个项都适合在排列时按SM_CYICONSPACING SM_CXICONSPACING大小的矩形。 此值始终大于或等于 SM_CXICON。
            public const int SM_CYICONSPACING = 39;//大图标视图中项的网格单元格的高度（以像素为单位）。 每个项都适合在排列时按SM_CYICONSPACING SM_CXICONSPACING大小的矩形。 此值始终大于或等于 SM_CYICON。
            public const int SM_MENUDROPALIGNMENT = 40;//如果下拉菜单与相应的菜单栏项右对齐，则为非零值;如果菜单左对齐，则为 0。
            public const int SM_PENWINDOWS = 41;//如果安装了 Microsoft Windows for Pen 计算扩展，则为非零值;否则为零。
            public const int SM_DBCSENABLED = 42;//如果User32.dll支持 DBCS，则为非零值;否则为 0。
            public const int SM_CMOUSEBUTTONS = 43;//鼠标上的按钮数;如果未安装鼠标，则为零。
            public const int SM_SECURE = 44;//应忽略此系统指标;它始终返回 0。SM_CXEDGE
            public const int SM_CXEDGE = 45;//三维边框的宽度（以像素为单位）。 此指标是SM_CXBORDER的三维对应指标。
            public const int SM_CYEDGE = 46;//三维边框的高度（以像素为单位）。 这是SM_CYBORDER的三维对应项。
            public const int SM_CXMINSPACING = 47;//最小化窗口的网格单元格的宽度（以像素为单位）。 每个最小化窗口在排列时适合此大小的矩形。 此值始终大于或等于 SM_CXMINIMIZED。
            public const int SM_CYMINSPACING = 48;//最小化窗口的网格单元格的高度（以像素为单位）。 每个最小化窗口在排列时适合此大小的矩形。 此值始终大于或等于 SM_CYMINIMIZED。
            public const int SM_CXSMICON = 49;//图标的系统小宽度（以像素为单位）。 小图标通常显示在窗口标题和小图标视图中。 有关详细信息 ，请参阅图标大小 。
            public const int SM_CYSMICON = 50;//图标的系统小高度（以像素为单位）。 小图标通常显示在窗口标题和小图标视图中。 有关详细信息 ，请参阅图标大小 。
            public const int SM_CYSMCAPTION = 51;//小描述文字的高度（以像素为单位）。
            public const int SM_CXSMSIZE = 52;//小描述文字按钮的宽度（以像素为单位）。
            public const int SM_CYSMSIZE = 53;//小描述文字按钮的高度（以像素为单位）。
            public const int SM_CXMENUSIZE = 54;//菜单栏按钮的宽度，例如在多个文档界面中使用的子窗口关闭按钮（以像素为单位）。
            public const int SM_CYMENUSIZE = 55;//菜单栏按钮（例如在多个文档界面中使用的子窗口关闭按钮）的高度（以像素为单位）。
            public const int SM_ARRANGE = 56;//指定系统如何排列最小化窗口的标志。 有关详细信息，请参阅本主题中的“备注”部分。
            public const int SM_CXMINIMIZED = 57;//最小化窗口的宽度（以像素为单位）。
            public const int SM_CYMINIMIZED = 58;//最小化窗口的高度（以像素为单位）。
            public const int SM_CXMAXTRACK = 59;//具有描述文字和大小调整边框（以像素为单位）的窗口的默认最大宽度。 此指标是指整个桌面。 用户无法将窗口框架拖动到大于这些尺寸的大小。 窗口可以通过处理 WM_GETMINMAXINFO 消息来替代此值。
            public const int SM_CYMAXTRACK = 60;//具有描述文字和大小调整边框的窗口的默认最大高度（以像素为单位）。 此指标是指整个桌面。 用户无法将窗口框架拖动到大于这些尺寸的大小。 窗口可以通过处理 WM_GETMINMAXINFO 消息来替代此值。
            public const int SM_CXMAXIMIZED = 61;//主显示监视器上最大化的顶级窗口的默认宽度（以像素为单位）。
            public const int SM_CYMAXIMIZED = 62;//主显示监视器上最大化的顶级窗口的默认高度（以像素为单位）。
            public const int SM_NETWORK = 63;//如果存在网络，则设置最小有效位;否则，将清除它。 其他位保留供将来使用。

            public const int SM_CLEANBOOT = 67;//指定系统启动方式的 值：0 正常启动，1 故障安全启动，2 通过网络启动实现故障安全，故障安全启动(也称为 SafeBoot、安全模式或干净启动) 会绕过用户启动文件。
            public const int SM_CXDRAG = 68;//鼠标指针在拖动操作开始之前可以移动的鼠标向下点任一侧的像素数。 这允许用户轻松单击并释放鼠标按钮，而不会无意中启动拖动操作。 如果此值为负值，则从鼠标向下点的左侧减去该值，并将其添加到其右侧。
            public const int SM_CYDRAG = 69;//鼠标指针在拖动操作开始之前可以移动的鼠标向下点上方和下方的像素数。 这允许用户轻松单击并释放鼠标按钮，而不会无意中启动拖动操作。 如果此值为负值，则从鼠标向下点上方减去该值，并将其添加到其下方。
            public const int SM_SHOWSOUNDS = 70;//如果用户要求应用程序在仅以声音形式显示信息的情况下直观显示信息，则为非零值;否则为 0。
            public const int SM_CXMENUCHECK = 71;//默认菜单的宽度检查标记位图（以像素为单位）。
            public const int SM_CYMENUCHECK = 72;//默认菜单的高度检查标记位图（以像素为单位）。
            public const int SM_SLOWMACHINE = 73;//如果计算机具有低端 (慢) 处理器，则为非零值;否则为 0。
            public const int SM_MIDEASTENABLED = 74;//如果为希伯来语和阿拉伯语启用系统，则为非零值;否则为 0。
            public const int SM_MOUSEWHEELPRESENT = 75;//如果安装了具有垂直滚轮的鼠标，则为非零值;否则为 0。
            public const int SM_XVIRTUALSCREEN = 76;//虚拟屏幕左侧的坐标。 虚拟屏幕是所有显示监视器的边框。 SM_CXVIRTUALSCREEN指标是虚拟屏幕的宽度。
            public const int SM_YVIRTUALSCREEN = 77;//虚拟屏幕顶部的坐标。 虚拟屏幕是所有显示监视器的边框。 SM_CYVIRTUALSCREEN指标是虚拟屏幕的高度。
            public const int SM_CXVIRTUALSCREEN = 78;//虚拟屏幕的宽度（以像素为单位）。 虚拟屏幕是所有显示监视器的边框。 SM_XVIRTUALSCREEN指标是虚拟屏幕左侧的坐标。
            public const int SM_CYVIRTUALSCREEN = 79;//虚拟屏幕的高度（以像素为单位）。 虚拟屏幕是所有显示监视器的边框。 SM_YVIRTUALSCREEN指标是虚拟屏幕顶部的坐标。
            public const int SM_CMONITORS = 80;//桌面上的显示监视器数。 有关详细信息，请参阅本主题中的“备注”部分。
            public const int SM_SAMEDISPLAYFORMAT = 81;//如果所有显示监视器具有相同的颜色格式，则为非零值，否则为 0。 两个显示器可以具有相同的位深度，但颜色格式不同。 例如，红色、绿色和蓝色像素可以使用不同位数进行编码，或者这些位可以位于像素颜色值的不同位置。
            public const int SM_IMMENABLED = 82;//如果启用了输入法管理器/输入法编辑器功能，则为非零值;否则为 0。 SM_IMMENABLED指示系统是否已准备好在 Unicode 应用程序上使用基于 Unicode 的 IME。
                                                //若要确保依赖于语言的 IME 正常工作，检查 SM_DBCSENABLED和系统 ANSI 代码页。 否则，ANSI 到 Unicode 的转换可能无法正确执行，或者某些组件（如字体或注册表设置）可能不存在。
            public const int SM_CXFOCUSBORDER = 83;//DrawFocusRect 绘制的焦点矩形的左边缘和右边缘的宽度。 此值以像素为单位。Windows 2000： 不支持此值。
            public const int SM_CYFOCUSBORDER = 84;//DrawFocusRect 绘制的焦点矩形的上边缘和下边缘的高度。 此值以像素为单位。Windows 2000： 不支持此值。

            public const int SM_TABLETPC = 86;//如果当前操作系统是 Windows XP 平板电脑版本，或者当前操作系统是 Windows Vista 或 Windows 7 并且平板电脑输入服务已启动，则为非零值;否则为 0。 SM_DIGITIZER设置指示运行 Windows 7 或 Windows Server 2008 R2 的设备支持的数字化器输入类型。 有关详细信息，请参阅“备注”。
            public const int SM_MEDIACENTER = 87;//如果当前操作系统是 Windows XP，则为非零，Media Center Edition 为 0（如果不是）。
            public const int SM_STARTER = 88;//如果当前操作系统为 Windows 7 简易版 Edition、Windows Vista 入门版 或 Windows XP Starter Edition，则为非零;否则为 0。
            public const int SM_SERVERR2 = 89;//系统为 Windows Server 2003 R2 时的内部版本号;否则为 0。
            public const int SM_MOUSEHORIZONTALWHEELPRESENT = 91;//如果安装了水平滚轮的鼠标，则为非零值;否则为 0。
            public const int SM_CXPADDEDBORDER = 92;//带字幕窗口的边框填充量（以像素为单位）。Windows XP/2000： 不支持此值。

            public const int SM_DIGITIZER = 94;//如果当前操作系统是 Windows 7 或 Windows Server 2008 R2 并且平板电脑输入服务已启动，则为非零;否则为 0。 
                                               //返回值是一个位掩码，用于指定设备支持的数字化器输入的类型。 有关详细信息，请参阅“备注”。
                                               //Windows Server 2008、Windows Vista 和 Windows XP/2000： 不支持此值。
            public const int SM_MAXIMUMTOUCHES = 95;//如果系统中存在数字化器，则为非零值;否则为 0。
                                                    //SM_MAXIMUMTOUCHES返回系统中每个数字化器支持的最大接触数的聚合最大值。 如果系统只有单点触控数字化器，则返回值为 1。 如果系统具有多点触控数字化器，则返回值是硬件可以提供的同时触点数。
                                                    //Windows Server 2008、Windows Vista 和 Windows XP/2000： 不支持此值。

            public const int SM_REMOTESESSION = 0x1000;//此系统指标用于终端服务环境。 如果调用进程与终端服务客户端会话相关联，则返回值为非零值。 如果调用进程与终端服务控制台会话相关联，则返回值为 0。 Windows Server 2003 和 Windows XP： 控制台会话不一定是物理控制台。 有关详细信息，请参阅 WTSGetActiveConsoleSessionId。
            public const int SM_SHUTTINGDOWN = 0x2000;//如果当前会话正在关闭，则为非零;否则为 0。Windows 2000： 不支持此值
            public const int SM_REMOTECONTROL = 0x2001;//此系统指标在终端服务环境中用于确定是否远程控制当前终端服务器会话。 如果远程控制当前会话，则其值为非零值;否则为 0。
                                                       //可以使用终端服务管理工具（如终端服务管理器(tsadmin.msc) 和shadow.exe）来控制远程会话。 远程控制会话时，另一个用户可以查看该会话的内容，并可能与之交互。

            public const int SM_CONVERTIBLESLATEMODE = 0x2003;//反映笔记本电脑或平板模式的状态，0 表示板模式，否则为非零。 当此系统指标发生更改时，系统会通过 LPARAM 中带有“ConvertibleSlateMode”
                                                              // 的WM_SETTINGCHANGE 发送广播消息。 请注意，此系统指标不适用于台式电脑。 在这种情况下，请使用 GetAutoRotationState。
            public const int SM_SYSTEMDOCKED = 0x2004;//反映停靠模式的状态，0 表示未停靠模式，否则为非零。 当此系统指标发生更改时，系统会通过 LPARAM 中带有“SystemDockMode” 的WM_SETTINGCHANGE 发送广播消息。



            [DllImport("user32")]
            public static extern int GetSystemMetrics(int nIndex);

        }

        /// <summary>
        /// 主显示器的分辨率和屏幕尺寸
        /// </summary>
        public class MonitorHelper
        {
            private const int HORZSIZE = 4;//以毫米为单位的显示宽度
            private const int VERTSIZE = 6;//以毫米为单位的显示高度
            private const int LOGPIXELSX = 88;//像素/逻辑英寸（水平）
            private const int LOGPIXELSY = 90; //像素/逻辑英寸（垂直）
            private const int DESKTOPVERTRES = 117;//垂直分辨率
            private const int DESKTOPHORZRES = 118;//水平分辨率

            /// <summary>
            /// 获取DC句柄
            /// </summary>
            [DllImport("user32.dll")]
            static extern IntPtr GetDC(IntPtr hdc);
            /// <summary>
            /// 释放DC句柄
            /// </summary>
            [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
            static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hdc);
            /// <summary>
            /// 获取句柄指定的数据
            /// </summary>
            [DllImport("gdi32.dll")]
            static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

            /// <summary>
            /// 获取分辨率
            /// </summary>
            /// <returns></returns>
            public static Size GetResolutionRatio()
            {
                Size size = new Size();
                IntPtr hdc = GetDC(IntPtr.Zero);
                size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
                size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
            /// <summary>
            /// 获取屏幕物理尺寸(mm,mm)
            /// </summary>
            /// <returns></returns>
            public static Size GetScreenSize()
            {
                Size size = new Size();
                IntPtr hdc = GetDC(IntPtr.Zero);
                size.Width = GetDeviceCaps(hdc, HORZSIZE);
                size.Height = GetDeviceCaps(hdc, VERTSIZE);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }

            /// <summary>
            /// 获取屏幕(对角线)的尺寸---英寸
            /// </summary>
            /// <returns></returns>
            public static float GetScreenInch()
            {
                Size size = GetScreenSize();
                double inch = Math.Round(Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) / 25.4, 1);
                return (float)inch;
            }
        }

        #endregion        

        #region//鼠标结构体的dwFlags设置为MOUSEEVENTF_ABSOLUTE,将dx与dy移动量变成屏幕坐标，以像素为单位

        public class MouseStructHelper
        {
            #region 获取鼠标屏幕坐标Window API，与Control.MousePosition功能一样

            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

            /// <summary>
            /// 获取鼠标当前屏幕坐标
            /// </summary>
            public Point GetMousePosition()
            {
                Point mp = new Point();

                if (GetCursorPos(out mp))
                {
                    return mp;
                }
                else
                {
                    return Point.Empty;
                }
            }
            #endregion


            /// <summary>
            /// 当前鼠标坐标转换为鼠标结构体dx，dy
            /// </summary>
            /// <returns></returns>
            public static Point MouseDxDy()
            {
                Point Mousedxdy = new Point();
                Mousedxdy.X = Control.MousePosition.X * (65535 / SystemMetricsHelper.GetSystemMetrics(SystemMetricsHelper.SM_CXSCREEN));
                Mousedxdy.Y = Control.MousePosition.Y * (65535 / SystemMetricsHelper.GetSystemMetrics(SystemMetricsHelper.SM_CYSCREEN));

                return Mousedxdy;
            }

            /// <summary>
            /// 指定鼠标屏幕坐标转换为鼠标结构体dx，dy
            /// </summary>
            /// <param name="source"></param>
            /// <returns></returns>
            public static Point MouseDxDy(Point source)
            {
                Point Mousedxdy = new Point();
                Mousedxdy.X = source.X * (65535 / SystemMetricsHelper.GetSystemMetrics(SystemMetricsHelper.SM_CXSCREEN));
                Mousedxdy.Y = source.Y * (65535 / SystemMetricsHelper.GetSystemMetrics(SystemMetricsHelper.SM_CYSCREEN));

                return Mousedxdy;
            }

            /// <summary>
            /// 获取鼠标当前坐标到目的坐标之间的最短距离的坐标序列
            /// </summary>
            /// <param name="destination">目的点坐标</param>
            /// <returns></returns>
            public static List<Point> MovePoints(Point destination)
            {
                return MovePoints(destination, Control.MousePosition);
            }

            /// <summary>
            /// 获取鼠标起始点到目的点之间最短距离的坐标序列。以两点之间X，Y方向长为主计算。
            /// </summary>
            /// <param name="destination">目的点坐标</param>
            /// <param name="source">起始点坐标</param>
            /// <returns></returns>
            public static List<Point> MovePoints(Point destination, Point source)
            {
                List<Point> points = new List<Point>();
                //原点与目标点一样
                if (destination == source)
                {
                    points.Add(destination);
                    return points;
                }
                else if (destination.X == source.X)//垂直移动
                {
                    for (int i = 0; i <= Math.Abs(destination.Y - source.Y); i++)
                    {
                        Point tp = new Point();
                        tp.X = destination.X;
                        if (destination.Y < source.Y)
                        {
                            tp.Y = source.Y - i;
                        }
                        else
                        {
                            tp.Y = source.Y + i;
                        }
                        points.Add(tp);
                    }
                    return points;
                }
                else if (destination.Y == source.Y)//水平移动
                {
                    for (int i = 0; i <= Math.Abs(destination.X - source.X); i++)
                    {
                        Point tp = new Point();
                        tp.Y = destination.Y;
                        if (destination.Y < source.Y)
                        {
                            tp.X = source.X - i;
                        }
                        else
                        {
                            tp.X = source.X + i;
                        }
                        points.Add(tp);
                    }
                    return points;
                }

                //原点与目标点不一样，并且非水平或非垂直移动，所有有斜率K
                double K = Convert.ToDouble(Math.Abs(destination.Y - source.Y)) / Convert.ToDouble(Math.Abs(destination.X - source.X));


                if (destination.Y > source.Y)//笛卡尔坐标系第一、二象限
                {
                    if (destination.X > source.X)//笛卡尔坐标系第一象限
                    {
                        //向选择原点和目标点之间X和Y方向长边的一组移动
                        if ((destination.X - source.X) >= (destination.Y - source.Y))//X方向长
                        {
                            for (int i = 0; i <= (destination.X - source.X); i++)
                            {
                                Point tp = new Point();
                                tp.X = source.X + i;
                                tp.Y = source.Y + (int)(K * i);
                                points.Add(tp);
                            }
                        }
                        else//Y方向长
                        {
                            for (int i = 0; i <= (destination.Y - source.Y); i++)
                            {
                                Point tp = new Point();
                                tp.Y = source.Y + i;
                                tp.X = source.X + (int)(i / K);
                                points.Add(tp);
                            }
                        }
                    }
                    else//笛卡尔坐标系第二象限
                    {
                        //向选择原点和目标点之间X和Y方向长边的一组移动
                        if ((source.X - destination.X) >= (destination.Y - source.Y))//X方向长
                        {
                            for (int i = 0; i <= (source.X - destination.X); i++)
                            {
                                Point tp = new Point();
                                tp.X = source.X - i;
                                tp.Y = source.Y + (int)(K * i);
                                points.Add(tp);
                            }
                        }
                        else//Y方向长
                        {
                            for (int i = 0; i <= (destination.Y - source.Y); i++)
                            {
                                Point tp = new Point();
                                tp.Y = source.Y + i;
                                tp.X = source.X - (int)(i / K);
                                points.Add(tp);
                            }
                        }
                    }
                }
                else//笛卡尔坐标系第三、四象限
                {
                    if (destination.X < source.X)//笛卡尔坐标系第三象限
                    {
                        //向选择原点和目标点之间X和Y方向长边的一组移动
                        if ((source.X - destination.X) >= (source.Y - destination.Y))//X方向长
                        {
                            for (int i = 0; i <= (source.X - destination.X); i++)
                            {
                                Point tp = new Point();
                                tp.X = source.X - i;
                                tp.Y = source.Y - (int)(K * i);
                                points.Add(tp);
                            }
                        }
                        else//Y方向长
                        {
                            for (int i = 0; i <= (source.Y - destination.Y); i++)
                            {
                                Point tp = new Point();
                                tp.Y = source.Y - i;
                                tp.X = source.X - (int)(i / K);
                                points.Add(tp);
                            }
                        }
                    }
                    else//笛卡尔坐标系第四象限
                    {
                        //向选择原点和目标点之间X和Y方向长边的一组移动
                        if ((destination.X - source.X) >= (source.Y - destination.Y))//X方向长
                        {
                            for (int i = 0; i <= (destination.X - source.X); i++)
                            {
                                Point tp = new Point();
                                tp.X = source.X + i;
                                tp.Y = source.Y - (int)(K * i);
                                points.Add(tp);
                            }
                        }
                        else//Y方向长
                        {
                            for (int i = 0; i <= (source.Y - destination.Y); i++)
                            {
                                Point tp = new Point();
                                tp.Y = source.Y - i;
                                tp.X = source.X + (int)(i / K);
                                points.Add(tp);
                            }
                        }
                    }
                }

                return points;
            }
        }

        #endregion

        #region//模拟鼠标键盘输入

        public class SendKeyboardMouse
        {
            #region//虚拟键盘

            /// <summary>
            /// 虚拟按下键
            /// </summary>
            /// <param name="vkCode">键盘虚拟代码，可以从VKCODE类中取</param>
            public void SendKeyDown(byte vkCode)
            {
                Input[] input = new Input[1];
                input[0].type = InputType.KEYBOARD;
                input[0].U.ki.wVk = vkCode;
                input[0].U.ki.dwFlags = WindowsConstant.KEYEVENTF_KEYDOWN;
                input[0].U.ki.time = NativeMethods.GetTickCount();

                uint backNum = NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0]));
                if (backNum < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            /// <summary>
            /// 虚拟释放键
            /// </summary>
            /// <param name="vkCode">键盘虚拟代码，可以从VKCODE类中取</param>
            public void SendKeyUp(byte vkCode)
            {
                Input[] input = new Input[1];
                input[0].type = InputType.KEYBOARD;
                input[0].U.ki.wVk = vkCode;
                input[0].U.ki.dwFlags = WindowsConstant.KEYEVENTF_KEYUP;
                input[0].U.ki.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            /// <summary>
            /// 虚拟按下并释放键
            /// </summary>
            /// <param name="vkCode">键盘虚拟代码，可以从VKCODE类中取</param>
            public void SendKeyPress(byte vkCode)
            {
                SendKeyDown(vkCode);
                Thread.Sleep(200);
                SendKeyUp(vkCode);
            }

            public void SendMessageDown(byte vkCode)
            {
                if (Socket_Cache.MainHandle != IntPtr.Zero)
                {
                    NativeMethods.SendMessage(Socket_Cache.MainHandle, WindowsConstant.WM_KEYDOWN, vkCode, IntPtr.Zero);
                }                
            }

            public void SendMessageUp(byte vkCode)
            {
                if (Socket_Cache.MainHandle != IntPtr.Zero)
                {
                    NativeMethods.SendMessage(Socket_Cache.MainHandle, WindowsConstant.WM_KEYUP, vkCode, IntPtr.Zero);
                }                    
            }

            public void SendMessagePress(byte vkCode)
            {
                if (Socket_Cache.MainHandle != IntPtr.Zero)
                {
                    SendMessageDown(vkCode);
                    Thread.Sleep(200);
                    SendMessageUp(vkCode);
                }                    
            }

            public void SendCombineKey(byte[] bKeys)
            {
                if (Socket_Cache.MainHandle != IntPtr.Zero)
                {
                    for (int i = 0; i < bKeys.Length; i++)
                    {
                        SendMessageDown(bKeys[i]);
                    }

                    for (int i = bKeys.Length - 1; i > -1; i--)
                    {
                        SendMessageUp(bKeys[i]);
                    }
                }
            }            

            #endregion

            #region//虚拟鼠标

            /// <summary>
            /// 虚拟鼠标滚轮滚动
            /// </summary>
            /// <param name="delta">正数表示向上滚动，负数表示向下滚动</param>
            public void MouseWheel(int delta)
            {
                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;

                input[0].U.mi.mouseData = delta;
                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_WHEEL;
                input[0].U.mi.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            /// <summary>
            /// 虚拟将鼠标光标直接移动到指定屏幕坐标处
            /// </summary>
            /// <param name="destination">目标处的屏幕坐标</param>
            public void MouseMove(Point destination)
            {
                //将鼠标结构体dx,dy移动量转换为屏幕坐标
                Point tp = MouseStructHelper.MouseDxDy(destination);

                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;
                input[0].U.mi.dx = tp.X;
                input[0].U.mi.dy = tp.Y;
                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_ABSOLUTE | WindowsConstant.MOUSEEVENTF_MOVE;
                input[0].U.mi.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

            }

            /// <summary>
            /// 虚拟有鼠标移动轨迹的鼠标移动，参数均是屏幕像素坐标
            /// </summary>
            /// <param name="destination">鼠标要移动到的目的点坐标</param>
            /// <param name="source">鼠标移动的起点坐标，如果是鼠标当前坐标，用 Control.MousePosition</param>
            /// <param name="lowSpeed">鼠标移动速度，数字越大越慢</param>
            public void MouseMove(Point destination, Point source, uint lowSpeed = 200)
            {
                //计算鼠标起始点与目标点之间移动时最短距离的点序列
                List<Point> movePoints = MouseStructHelper.MovePoints(destination, source);

                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;

                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_ABSOLUTE | WindowsConstant.MOUSEEVENTF_MOVE;
                for (int i = 0; i < movePoints.Count; i++)
                {
                    Point tp = new Point();
                    tp = MouseStructHelper.MouseDxDy(movePoints[i]);
                    input[0].U.mi.dx = tp.X;
                    input[0].U.mi.dy = tp.Y;
                    input[0].U.mi.time = NativeMethods.GetTickCount();

                    if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }

                    //以下这段循环，用于减缓鼠标移动速度，如果用Thread.Sleep(1),虽然只有1毫秒，也显得有点缓慢，大家可以根据自己需要选择使用。
                    int j = 0;
                    while (j < lowSpeed * 1000)
                    {
                        j++;
                    }
                }

            }

            /// <summary>
            /// 虚拟鼠标左键按下
            /// </summary>
            public void MouseLeftDown()
            {
                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;
                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_LEFTDOWN;
                input[0].U.mi.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            /// <summary>
            /// 虚拟鼠标右键按下
            /// </summary>
            public void MouseRightDown()
            {
                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;
                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_RIGHTDOWN;
                input[0].U.mi.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            /// <summary>
            /// 虚拟鼠标左键释放
            /// </summary>
            public void MouseLeftUp()
            {
                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;
                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_LEFTUP;
                input[0].U.mi.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            /// <summary>
            /// 虚拟鼠标右键释放
            /// </summary>
            public void MouseRightUp()
            {
                Input[] input = new Input[1];
                input[0].type = InputType.MOUSE;
                input[0].U.mi.dwFlags = WindowsConstant.MOUSEEVENTF_RIGHTUP;
                input[0].U.mi.time = NativeMethods.GetTickCount();

                if (NativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            public void MouseLeftClick()
            {
                MouseLeftDown();
                MouseLeftUp();            
            }

            public void MouseRightClick()
            {
                MouseRightDown();
                MouseRightUp();              
            }

            /// <summary>
            /// 虚拟双击鼠标左键
            /// </summary>
            public void MouseLeftDBClick()
            {
                MouseLeftClick();
                MouseLeftClick();
            }

            /// <summary>
            /// 虚拟双击鼠标右键
            /// </summary>
            public void MouseRightDBClick()
            {
                MouseRightClick();
                MouseRightClick();
            }

            #endregion
        }

        #endregion      
    }
}

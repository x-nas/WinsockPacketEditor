using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using WPELibrary.Lib;
using EasyHook;

namespace ProcessInjector
{
    public partial class Injector_Form : Form
    {
        private string Version = "";
        private string ProcessName = "";
        private string ProcessPath = "";
        private int ProcessID = -1;

        #region//DLL 引用

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        #endregion

        #region//窗体加载
        public Injector_Form()
        {
            InitializeComponent();

            this.Text = "进程注入器（x86, x64自适应）";

            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ShowLog("当前内核版本：" + Version);            
        }
        #endregion

        #region//显示日志
        private void ShowLog(string ShowInfo)
        {
            if (string.IsNullOrEmpty(this.rtbLog.Text.Trim()))
            {
                this.rtbLog.AppendText(ShowInfo);
            }
            else
            {
                this.rtbLog.AppendText("\n" + ShowInfo);
            }            
        }
        #endregion

        #region//判断是否为64位的进程
        private bool IsWin64Process(int ProcessID)
        {
            bool bReturn = false;

            Process pProcess = Process.GetProcessById(ProcessID);

            if (pProcess != null)
            {
                if ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
                {
                    bool retVal;
                    IsWow64Process(pProcess.Handle, out retVal);
                    bReturn = !retVal;
                }
            }

            return bReturn;
        }
        #endregion

        #region//选择进程
        private void bSelectProcess_Click(object sender, EventArgs e)
        {
            ProcessList_Form plf = new ProcessList_Form();
            plf.ShowDialog();

            if (Program.PID != -1 && Program.PNAME != string.Empty)
            {
                tbProcessID.Text = Program.PNAME + " [" + Program.PID + "]";
            }
            else if (Program.PID == -1 && !string.IsNullOrEmpty(Program.PNAME) && !string.IsNullOrEmpty(Program.PATH))
            {
                tbProcessID.Text = Program.PNAME;
            }
        }
        #endregion

        #region//注入
        private void bInject_Click(object sender, EventArgs e)
        {
            this.rtbLog.Clear();
            ShowLog("当前内核版本：" + Version);

            ProcessID = Program.PID;
            ProcessPath = Program.PATH;
            ProcessName = Program.PNAME;
            string sDllName = "WPELibrary.dll";
            
            try
            {
                if (string.IsNullOrEmpty(ProcessPath) && string.IsNullOrEmpty(ProcessName))
                {
                    Socket_Operation.ShowMessageBox("请先选择要注入的进程！");                    
                }
                else
                {
                    string injectionLibrary_x86 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sDllName);
                    string injectionLibrary_x64 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sDllName);                    

                    ShowLog("开始注入目标进程 =>> " + ProcessName);

                    if (ProcessID > -1)
                    {
                        RemoteHooking.Inject(ProcessID, injectionLibrary_x86, injectionLibrary_x64, "");
                    }
                    else
                    {
                        RemoteHooking.CreateAndInject(ProcessPath, string.Empty, 0, injectionLibrary_x86, injectionLibrary_x64, out this.ProcessID, string.Empty);                                                
                    }

                    int targetPlat = IsWin64Process(ProcessID) ? 64 : 32;
                    ShowLog(string.Format("目标进程是{0}位程序，已自动调用{0}位的注入模块!", targetPlat));
                    ShowLog(string.Format("已成功注入目标进程 =>> {0}[{1}]", ProcessName, ProcessID));
                    ShowLog("注入完成，可关闭当前注入器.");
                }                
            }
            catch (Exception ex)
            {                
                ShowLog("出现错误：" + ex.Message);                
            }
        }
        #endregion
    }
}

using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ProcessInjector.Lib;
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
        private string HelpURL = "https://www.52pojie.cn/thread-1446781-1-1.html";

        private ToolTip tt = new ToolTip();

        #region//窗体加载

        public Injector_Form()
        {
            InitializeComponent();

            this.rtbLog.Clear();
            this.InitToolTip();

            Process_Injector.SetDefaultLanguage(Properties.Settings.Default.DefaultLanguage);
            MultiLanguage.LoadLanguage(this, typeof(Injector_Form));            
        }

        private void InitToolTip()
        {            
            string sTTSelectProcess = MultiLanguage.GetDefaultLanguage("选择进程", "Select Process");
            tt.SetToolTip(bSelectProcess, sTTSelectProcess);
                        
            string sTTInject = MultiLanguage.GetDefaultLanguage("注入进程", "Inject Process");
            tt.SetToolTip(bInject, sTTInject);
                        
            string sTTHelp = MultiLanguage.GetDefaultLanguage("帮助", "Help");
            tt.SetToolTip(pbHelp, sTTHelp);
                        
            string sTTLanguage = MultiLanguage.GetDefaultLanguage("选择语言", "Language");
            tt.SetToolTip(pbLanguage, sTTLanguage);
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
            string sMessage = "";

            ProcessID = Program.PID;
            ProcessPath = Program.PATH;
            ProcessName = Program.PNAME;
            string sDllName = "WPELibrary.dll";
            
            try
            {
                if (string.IsNullOrEmpty(ProcessPath) && string.IsNullOrEmpty(ProcessName))
                {
                    sMessage = MultiLanguage.GetDefaultLanguage("请先选择要注入的进程！", "Please select a process first!");
                    Socket_Operation.ShowMessageBox(sMessage);                    
                }
                else
                {
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    sMessage = MultiLanguage.GetDefaultLanguage("当前内核版本", "Current kernel version");
                    ShowLog(sMessage + "：" + Version);

                    string injectionLibrary_x86 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sDllName);
                    string injectionLibrary_x64 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sDllName);

                    sMessage = MultiLanguage.GetDefaultLanguage("开始注入目标进程", "Start injecting target process");
                    ShowLog(sMessage + " =>> " + ProcessName);

                    if (ProcessID > -1)
                    {
                        RemoteHooking.Inject(ProcessID, injectionLibrary_x86, injectionLibrary_x64, Properties.Settings.Default.DefaultLanguage);
                    }
                    else
                    {
                        RemoteHooking.CreateAndInject(ProcessPath, string.Empty, 0, injectionLibrary_x86, injectionLibrary_x64, out this.ProcessID, Properties.Settings.Default.DefaultLanguage);                                                
                    }

                    int targetPlat = Process_Injector.IsWin64Process(ProcessID) ? 64 : 32;

                    sMessage = MultiLanguage.GetDefaultLanguage("目标进程是{0}位程序，已自动调用{0}位的注入模块!", "The target process is a {0} bit program and has automatically called the {0} bit injection module!");
                    ShowLog(string.Format(sMessage, targetPlat));

                    sMessage = MultiLanguage.GetDefaultLanguage("已成功注入目标进程", "Successfully injected into the target process");
                    ShowLog(string.Format(sMessage + " =>> {0}[{1}]", ProcessName, ProcessID));

                    sMessage = MultiLanguage.GetDefaultLanguage("注入完成，可关闭当前注入器.", "Injection completed, the current injector can be closed.");
                    ShowLog(sMessage);

                    MultiLanguage.SetDefaultLanguage(Properties.Settings.Default.DefaultLanguage);
                }                
            }
            catch (Exception ex)
            {
                sMessage = MultiLanguage.GetDefaultLanguage("出现错误", "Error");
                ShowLog(sMessage + "：" + ex.Message);                
            }
        }
        #endregion

        #region//帮助, 选择语言
        private void pbHelp_Click(object sender, EventArgs e)
        {
            Process.Start(HelpURL);
        }        

        private void pbLanguage_Click(object sender, EventArgs e)
        {
            LanguageList_Form llf = new LanguageList_Form();
            llf.ShowDialog();

            this.InitToolTip();
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
    }
}

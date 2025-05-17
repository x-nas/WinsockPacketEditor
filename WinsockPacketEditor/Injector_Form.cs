using System;
using System.Windows.Forms;
using System.IO;
using WPELibrary.Lib;
using EasyHook;
using System.Reflection;
using System.Diagnostics;

namespace WinsockPacketEditor
{
    public partial class Injector_Form : Form
    {
        private int ProcessID = -1;        
        private string ProcessName = string.Empty;
        private string ProcessPath = string.Empty;        

        private readonly ToolTip tt = new ToolTip();

        #region//窗体事件

        public Injector_Form()
        {            
            InitializeComponent();            

            this.rtbLog.Clear();
            this.InitToolTip();
            this.InitLastInjection();            
        }

        private void InitToolTip()
        {
            try
            {
                tt.SetToolTip(bSelectProcess, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_1));
                tt.SetToolTip(bInject, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_2));
                                
                ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_5), Socket_Operation.AssemblyVersion));
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }            
        }

        private void Injector_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Socket_Cache.System.SaveSystemConfig_LastInjection_ToDB();
        }

        #endregion

        #region//初始化上次注入信息

        private void InitLastInjection()
        {
            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.System.LastInjection))
                {
                    Process[] plProcess = Process.GetProcessesByName(Socket_Cache.System.LastInjection);

                    if (plProcess.Length > 0)
                    { 
                        Program.PID = plProcess[0].Id;
                        Program.PNAME = plProcess[0].ProcessName;

                        this.ShowSelectProcess();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }        

        #endregion

        #region//选择进程

        private void bSelectProcess_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessList_Form plf = new ProcessList_Form();
                plf.ShowDialog();

                this.ShowSelectProcess();
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }

        private void ShowSelectProcess()
        {
            try
            {
                if (Program.PID != -1 && Program.PNAME != string.Empty)
                {
                    tbProcessID.Text = Program.PNAME + " [" + Program.PID + "]";
                }
                else if (Program.PID == -1 && !string.IsNullOrEmpty(Program.PNAME) && !string.IsNullOrEmpty(Program.PATH))
                {
                    tbProcessID.Text = Program.PNAME;
                }

                this.bInject.Focus();
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }

        #endregion

        #region//注入选择的进程

        private void bInject_Click(object sender, EventArgs e)
        {
            try
            {
                string channelName = "WPE64";
                ProcessID = Program.PID;
                ProcessPath = Program.PATH;
                ProcessName = Program.PNAME;

                if (string.IsNullOrEmpty(ProcessPath) && string.IsNullOrEmpty(ProcessName))
                {                    
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_6));
                }
                else
                {
                    string injectionLibrary_x86 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Socket_Cache.System.WPE64_DLL);
                    string injectionLibrary_x64 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Socket_Cache.System.WPE64_DLL);
                                        
                    ShowLog(DateTime.Now.ToString("G"));
                    ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_7), ProcessName));

                    if (ProcessID > -1)
                    {
                        RemoteHooking.Inject(ProcessID, injectionLibrary_x86, injectionLibrary_x64, channelName);
                    }
                    else
                    {
                        RemoteHooking.CreateAndInject(ProcessPath, string.Empty, 0, injectionLibrary_x86, injectionLibrary_x64, out this.ProcessID, channelName);
                    }

                    Socket_Cache.System.LastInjection = Program.PNAME;
                    int targetPlat = Socket_Operation.IsWin64Process(ProcessID) ? 64 : 32;

                    ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_8), targetPlat));                    
                    ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_9), ProcessName, ProcessID));
                    ShowLog(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_10));                    
                }
            }
            catch (Exception ex)
            {  
                ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_11), ex.Message));
                ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_102), Socket_Cache.System.WPE64_URL));
            }

            this.rtbLog.ScrollToCaret();            
        }

        #endregion                

        #region//显示日志

        private void ShowLog(string ShowInfo)
        {
            try
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
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }

        private void rtbLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}

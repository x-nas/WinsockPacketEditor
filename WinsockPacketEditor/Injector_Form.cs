using System;
using System.Windows.Forms;
using System.IO;
using WinsockPacketEditor.Lib;
using WPELibrary.Lib;
using EasyHook;
using System.Reflection;
using System.Net;

namespace WinsockPacketEditor
{
    public partial class Injector_Form : Form
    {        
        private string ProcessName = "";
        private string ProcessPath = "";        
        private int ProcessID = -1;
        private string sDllName = "WPELibrary.dll";
        private string sWPE64_URL = "https://www.wpe64.com";
        private string sWPE64_IP = "http://101.132.222.195";

        private ToolTip tt = new ToolTip();

        #region//窗体加载

        public Injector_Form()
        {
            Process_Injector.SetDefaultLanguage(Properties.Settings.Default.DefaultLanguage);

            InitializeComponent();                        
            
            this.rtbLog.Clear();
            this.InitToolTip();
        }

        private void InitToolTip()
        {
            try
            {
                tt.SetToolTip(bSelectProcess, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_1));
                tt.SetToolTip(bInject, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_2));                
                tt.SetToolTip(pbAbout, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_3));                
                tt.SetToolTip(pbLanguage, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_4));
                                
                ShowLog(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_5) + "：" + Process_Injector.AssemblyVersion);
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

                if (Program.PID != -1 && Program.PNAME != string.Empty)
                {
                    tbProcessID.Text = Program.PNAME + " [" + Program.PID + "]";
                }
                else if (Program.PID == -1 && !string.IsNullOrEmpty(Program.PNAME) && !string.IsNullOrEmpty(Program.PATH))
                {
                    tbProcessID.Text = Program.PNAME;
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }

        #endregion

        #region//注入

        private void bInject_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessID = Program.PID;
                ProcessPath = Program.PATH;
                ProcessName = Program.PNAME;

                if (string.IsNullOrEmpty(ProcessPath) && string.IsNullOrEmpty(ProcessName))
                {                    
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_6));
                }
                else
                {
                    string injectionLibrary_x86 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sDllName);
                    string injectionLibrary_x64 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sDllName);
                                        
                    ShowLog(DateTime.Now.ToString("G"));
                    ShowLog(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_7) + " =>> " + ProcessName);

                    if (ProcessID > -1)
                    {
                        RemoteHooking.Inject(ProcessID, injectionLibrary_x86, injectionLibrary_x64, Properties.Settings.Default.DefaultLanguage);
                    }
                    else
                    {
                        RemoteHooking.CreateAndInject(ProcessPath, string.Empty, 0, injectionLibrary_x86, injectionLibrary_x64, out this.ProcessID, Properties.Settings.Default.DefaultLanguage);
                    }

                    int targetPlat = Process_Injector.IsWin64Process(ProcessID) ? 64 : 32;
                    
                    ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_8), targetPlat));                    
                    ShowLog(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_9) + " =>> {0}[{1}]", ProcessName, ProcessID));
                    ShowLog(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_10));

                    MultiLanguage.SetDefaultLanguage(Properties.Settings.Default.DefaultLanguage);
                }
            }
            catch (Exception ex)
            {                
                ShowLog(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_11) + "：" + ex.Message);
            }
        }

        #endregion

        #region//关于, 选择语言

        private void pbAbout_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bgwCheckURL.IsBusy)
                {
                    bgwCheckURL.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }        

        private void pbLanguage_Click(object sender, EventArgs e)
        {
            try
            {
                LanguageList_Form llf = new LanguageList_Form();
                llf.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }            
        }

        private bool CheckWebSiteIsOK(string sURL)
        {
            bool bReturn = false;

            try
            {
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(sURL);
                HttpWebResponse resp = (HttpWebResponse)hwr.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }        

            return bReturn;
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

        #endregion

        #region//检测并打开网站（异步）

        private void bgwCheckURL_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                e.Result = this.CheckWebSiteIsOK(sWPE64_URL);                
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }

        private void bgwCheckURL_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                bool bWPE64_URL = (bool)e.Result;

                if (bWPE64_URL)
                {
                    System.Diagnostics.Process.Start(sWPE64_URL);
                }
                else
                {
                    System.Diagnostics.Process.Start(sWPE64_IP);
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }            
        }

        #endregion
    }
}

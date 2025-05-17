using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WinsockPacketEditor
{
    public partial class SystemMode_Form : Form
    {        
        private string SelectLanguage = string.Empty;
        private string WebSiteURL = string.Empty;
        private string RemoteIP = string.Empty;        

        #region//窗体加载

        public SystemMode_Form()
        {
            MultiLanguage.SetDefaultLanguage(Socket_Cache.System.DefaultLanguage);
            InitializeComponent();
        }

        private async void SystemMode_Form_Load(object sender, EventArgs e)
        {
            try
            {
                IPAddress[] ipAddresses = await Socket_Operation.GetLocalIPAddress();

                if (ipAddresses.Length > 0)
                {
                    this.RemoteIP = ipAddresses[0].ToString();
                }
                else
                {
                    this.RemoteIP = "127.0.0.1";
                }

                bool bWPE64_URL = await Socket_Operation.CheckWebSite(Socket_Cache.System.WPE64_URL);

                if (bWPE64_URL)
                {
                    this.WebSiteURL = Socket_Cache.System.WPE64_URL;
                }
                else
                {
                    this.WebSiteURL = Socket_Cache.System.WPE64_IP;
                }

                this.cbIsRemote.Checked = Socket_Cache.System.IsRemote;
                this.txtRemote_UserName.Text = Socket_Cache.System.Remote_UserName;
                this.txtRemote_PassWord.Text = Socket_Cache.System.Remote_PassWord;
                this.nudRemote_Port.Value = Socket_Cache.System.Remote_Port;

                this.IsRemote_Changed();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void SystemMode_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Socket_Cache.System.SaveSystemConfig_ToDB();
        }

        #endregion        

        #region//启动按钮

        private void bProxy_Start_Click(object sender, EventArgs e)
        {
            Socket_Cache.System.StartMode = Socket_Cache.System.SystemMode.Proxy;
            this.ExitMainForm();
        }

        private void bProcess_Start_Click(object sender, EventArgs e)
        {
            Socket_Cache.System.StartMode = Socket_Cache.System.SystemMode.Process;
            this.ExitMainForm();
        }

        private void ExitMainForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region//选择语言

        private void tsmiChinese_Click(object sender, EventArgs e)
        {
            this.SelectLanguage = "zh-CN";
            this.LanguageChange();
        }

        private void tsmiEnglish_Click(object sender, EventArgs e)
        {
            this.SelectLanguage = "en-US";
            this.LanguageChange();
        }

        private void LanguageChange()
        {
            try
            {
                if (!Socket_Cache.System.DefaultLanguage.Equals(this.SelectLanguage))
                {
                    Socket_Cache.System.DefaultLanguage = this.SelectLanguage;
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_12));

                    Application.Restart();
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//关于

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            Process.Start(this.WebSiteURL);
        }

        #endregion

        #region//远程管理

        private void cbIsRemote_Click(object sender, EventArgs e)
        {
            string Remote_UserName = this.txtRemote_UserName.Text.Trim();
            string Remote_PassWord = this.txtRemote_PassWord.Text.Trim();

            if (string.IsNullOrEmpty(Remote_UserName) || string.IsNullOrEmpty(Remote_PassWord))
            {
                this.cbIsRemote.Checked = false;
                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_180));
                return;
            }

            this.IsRemote_Changed();
        }

        private void IsRemote_Changed()
        {
            try
            {
                string RemoteURL = this.GetRemoteURL();

                if (!string.IsNullOrEmpty(RemoteURL))
                {
                    this.lRemoteURL.Text = RemoteURL;

                    Socket_Cache.System.IsRemote = this.cbIsRemote.Checked;
                    Socket_Cache.System.Remote_UserName = this.txtRemote_UserName.Text.Trim();
                    Socket_Cache.System.Remote_PassWord = this.txtRemote_PassWord.Text.Trim();
                    Socket_Cache.System.Remote_Port = ((ushort)this.nudRemote_Port.Value);
                    Socket_Cache.System.Remote_URL = RemoteURL;
                }
            }
            catch (Exception ex)
            {
                this.cbIsRemote.Checked = false;
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            this.tlpRemoteInfo.Enabled = !this.cbIsRemote.Checked;
            this.lRemoteMGT.Visible = this.lRemoteURL.Visible = this.cbIsRemote.Checked;          
        }

        private string GetRemoteURL()
        {
            string sReturn = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(this.RemoteIP))
                {
                    string RemotePort = this.nudRemote_Port.Value.ToString();
                    sReturn = string.Format("http://{0}:{1}", this.RemoteIP, RemotePort);                    
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        private void lRemoteURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(this.lRemoteURL.Text);
        }

        #endregion               
    }
}

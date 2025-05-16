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
        private string DefaultLanguage = string.Empty;
        private string SelectLanguage = string.Empty;
        private string WebSiteURL = string.Empty;
        private string RemoteIP = string.Empty;        

        #region//窗体加载

        public SystemMode_Form()
        {
            this.DefaultLanguage = Properties.Settings.Default.DefaultLanguage;
            MultiLanguage.SetDefaultLanguage(this.DefaultLanguage);

            InitializeComponent();

            this.LoadSystemSet();
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

                bool bWPE64_URL = await Socket_Operation.CheckWebSite(Properties.Settings.Default.WPE64_URL);

                if (bWPE64_URL)
                {
                    this.WebSiteURL = Properties.Settings.Default.WPE64_URL;
                }
                else
                {
                    this.WebSiteURL = Properties.Settings.Default.WPE64_IP;
                }

                this.IsRemote_Changed();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void SystemMode_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveSystemSet();
        }

        #endregion        

        #region//启动按钮

        private void bProxy_Start_Click(object sender, EventArgs e)
        {
            Socket_Cache.SelectMode = Socket_Cache.SystemMode.Proxy;
            this.ExitMainForm();
        }

        private void bProcess_Start_Click(object sender, EventArgs e)
        {
            Socket_Cache.SelectMode = Socket_Cache.SystemMode.Process;
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
                if (!this.DefaultLanguage.Equals(this.SelectLanguage))
                {
                    Properties.Settings.Default.DefaultLanguage = this.SelectLanguage;
                    Properties.Settings.Default.Save();

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
                    Socket_Cache.IsRemote = this.cbIsRemote.Checked;
                    Socket_Cache.Remote_Port = ((ushort)this.nudRemote_Port.Value);
                    Socket_Cache.Remote_URL = RemoteURL;
                    Socket_Cache.Remote_UserName = this.txtRemote_UserName.Text.Trim();
                    Socket_Cache.Remote_PassWord = this.txtRemote_PassWord.Text.Trim();

                    this.lRemoteURL.Text = RemoteURL;                    
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

        #region//保存参数配置

        private void SaveSystemSet()
        {
            try
            {
                Properties.Settings.Default.Remote_IsEnable = this.cbIsRemote.Checked;
                Properties.Settings.Default.Remote_UserName = this.txtRemote_UserName.Text.Trim();
                Properties.Settings.Default.Remote_PassWord = this.txtRemote_PassWord.Text.Trim();
                Properties.Settings.Default.Remote_Port = ((ushort)this.nudRemote_Port.Value);

                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//加载参数配置

        private void LoadSystemSet()
        {
            try
            {
                this.cbIsRemote.Checked = Properties.Settings.Default.Remote_IsEnable;
                this.txtRemote_UserName.Text = Properties.Settings.Default.Remote_UserName;
                this.txtRemote_PassWord.Text = Properties.Settings.Default.Remote_PassWord;
                this.nudRemote_Port.Value = Properties.Settings.Default.Remote_Port;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion        
    }
}

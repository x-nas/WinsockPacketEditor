using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WinsockPacketEditor
{
    public partial class SystemMode_Form : Form
    {        
        private string WebSiteURL = string.Empty;        

        #region//窗体加载

        public SystemMode_Form()
        {
            MultiLanguage.SetDefaultLanguage(Properties.Settings.Default.DefaultLanguage);
            InitializeComponent();
        }

        private async void SystemMode_Form_Load(object sender, EventArgs e)
        {
            try
            {
                bool bWPE64_URL = await Socket_Operation.CheckWebSite(Properties.Settings.Default.WPE64_URL);

                if (bWPE64_URL)
                {
                    this.WebSiteURL = Properties.Settings.Default.WPE64_URL;
                }
                else
                {
                    this.WebSiteURL = Properties.Settings.Default.WPE64_IP;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//启动按钮

        private void bProxy_Start_Click(object sender, EventArgs e)
        {
            Program.Mode = Socket_Cache.SystemMode.Proxy;
            this.ExitMainForm();
        }

        private void bProcess_Start_Click(object sender, EventArgs e)
        {
            Program.Mode = Socket_Cache.SystemMode.Process;
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
            this.LanguageChange("zh-CN");
        }

        private void tsmiEnglish_Click(object sender, EventArgs e)
        {
            this.LanguageChange("en-US");
        }

        private void LanguageChange(string SelectLanguage)
        {
            try
            {
                if (!Properties.Settings.Default.DefaultLanguage.Equals(SelectLanguage))
                {
                    Program.SaveDefaultLanguage(SelectLanguage);

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
    }
}

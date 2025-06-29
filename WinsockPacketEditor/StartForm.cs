using AntdUI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;
using WPE.Lib.Controls;

namespace WinsockPacketEditor
{
    public partial class StartForm : AntdUI.Window
    {
        private string RemoteIP = string.Empty;
        private bool isProcessingCheckChange = false;

        #region//窗体事件

        public StartForm()
        {
            InitializeComponent();
            this.Dark_Changed();
            this.InitForm();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            try
            {
                IPAddress[] ipAddresses = Operate.SystemConfig.GetLocalIPAddress();
                if (ipAddresses.Length > 0)
                {
                    this.RemoteIP = ipAddresses[0].ToString();
                }
                else
                {
                    this.RemoteIP = "127.0.0.1";
                }
            }
            catch
            {
                this.RemoteIP = "127.0.0.1";
            }
            finally
            {
                this.InitRemote();
            }
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemConfig_ToDB();
        }        

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("StartForm", "启动");
            this.pageHeader.Text = "WPE x64";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;

            btn_global.Items.AddRange(
                new AntdUI.ISelectItem[]
                {
                    new AntdUI.SelectItem("中文", "zh-CN"),
                    new AntdUI.SelectItem("English", "en-US")
                });

            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                btn_global.SelectedValue = btn_global.Items[1];
            }
            else
            {
                btn_global.SelectedValue = btn_global.Items[0];
            }            
        }

        private void InitRemote()
        {
            this.txtRemote_UserName.Text = Operate.SystemConfig.Remote_UserName;
            this.txtRemote_PassWord.Text = Operate.SystemConfig.Remote_PassWord;
            this.nudRemote_Port.Value = Operate.SystemConfig.Remote_Port;
            this.cbIsRemote.Checked = Operate.SystemConfig.IsRemote;

            this.IsRemote_Changed();
        }

        private void txtRemote_UserName_TextChanged(object sender, EventArgs e)
        {
            string username = this.txtRemote_UserName.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                this.txtRemote_UserName.Status = TType.Error;
            }
            else
            {
                this.txtRemote_UserName.Status = TType.Success;
            }
        }

        private void txtRemote_PassWord_TextChanged(object sender, EventArgs e)
        {
            string password = this.txtRemote_PassWord.Text.Trim();
            if (string.IsNullOrEmpty(password))
            {
                this.txtRemote_PassWord.Status = TType.Error;
            }
            else
            {
                this.txtRemote_PassWord.Status = TType.Success;
            }
        }

        #endregion

        #region//切换主题模式

        private void btn_mode_Click(object sender, EventArgs e)
        {
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;

            this.Dark_Changed();
            OnSizeChanged(e);
        }

        private void Dark_Changed()
        {
            this.Dark = AntdUI.Config.IsDark;
            btn_mode.Toggle = Dark;

            if (Dark)
            {
                this.BackColor = Color.FromArgb(30, 30, 30);
                this.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
            }
        }

        #endregion

        #region//切换语言

        private void btn_global_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.SelectItem value)
            {
                if (btn_global.Tag == value)
                {
                    return;
                }
                
                btn_global.Tag = value;
                btn_global.Loading = true;

                string lang = value.Tag.ToString();
                if (lang.StartsWith("en"))
                {
                    AntdUI.Localization.Provider = new Localizer();
                }
                else
                {
                    AntdUI.Localization.Provider = null;
                } 

                AntdUI.Localization.SetLanguage(lang);
                this.Text = "WPE x64 - " + AntdUI.Localization.Get("StartForm", "启动");
                Refresh();

                btn_global.Loading = false;
            }
        }

        #endregion

        #region//远程管理

        private void cbIsRemote_CheckedChanged(object sender, BoolEventArgs e)
        {
            if (isProcessingCheckChange)
            {
                return;
            } 

            try
            {
                isProcessingCheckChange = true;

                string Remote_UserName = this.txtRemote_UserName.Text.Trim();
                string Remote_PassWord = this.txtRemote_PassWord.Text.Trim();
                if (string.IsNullOrEmpty(Remote_UserName) || string.IsNullOrEmpty(Remote_PassWord))
                {
                    this.cbIsRemote.Checked = false;

                    AntdUI.Message.open(new AntdUI.Message.Config(this, "管理账号或密码为空", TType.Error)
                    {
                        LocalizationText = "StartForm.RemoteEmpty"
                    });

                    return;
                }

                if (this.cbIsRemote.Checked)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "远程管理已启用", TType.Success)
                    {
                        LocalizationText = "StartForm.RemoteEnable"
                    });
                }
                else
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "远程管理已关闭", TType.Success)
                    {
                        LocalizationText = "StartForm.RemoteDisable"
                    });
                }

                this.IsRemote_Changed();
            }
            finally
            {
                isProcessingCheckChange = false;
            }
        }

        private void IsRemote_Changed()
        {
            try
            {
                string RemoteURL = this.GetRemoteURL();

                if (!string.IsNullOrEmpty(RemoteURL))
                {
                    this.lRemoteURL.Text = RemoteURL;

                    Operate.SystemConfig.IsRemote = this.cbIsRemote.Checked;
                    Operate.SystemConfig.Remote_UserName = this.txtRemote_UserName.Text.Trim();
                    Operate.SystemConfig.Remote_PassWord = this.txtRemote_PassWord.Text.Trim();
                    Operate.SystemConfig.Remote_Port = ((ushort)this.nudRemote_Port.Value);
                    Operate.SystemConfig.Remote_URL = RemoteURL;
                }
            }
            catch (Exception ex)
            {
                this.cbIsRemote.Checked = false;
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        private void lRemoteURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(this.lRemoteURL.Text.Trim());
        }

        #endregion

        #region//选择模式

        private void bProxy_Click(object sender, EventArgs e)
        {
            Operate.SystemConfig.StartMode = Operate.SystemConfig.SystemMode.Proxy;
            this.ExitMainForm();
        }

        private void bInject_Click(object sender, EventArgs e)
        {
            SelectProcessForm selectProcessForm = new SelectProcessForm();
            selectProcessForm.ShowDialog();

            if (selectProcessForm.DialogResult == DialogResult.OK)
            {
                Operate.SystemConfig.StartMode = Operate.SystemConfig.SystemMode.Process;
                this.ExitMainForm();
            }            
        }

        private void ExitMainForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region//系统设置

        private void btn_setting_Click(object sender, EventArgs e)
        {
            var setting = new SystemSetting();
            if (AntdUI.Modal.open(this, AntdUI.Localization.Get("Setting", "设置"), setting) == DialogResult.OK)
            {                
                AntdUI.Config.Animation = setting.Animation;
                AntdUI.Config.ShadowEnabled = setting.ShadowEnabled;
                AntdUI.Config.ShowInWindow = setting.ShowInWindow;
                AntdUI.Config.ScrollBarHide = setting.ScrollBarHide;
                AntdUI.Config.TextRenderingHighQuality = setting.TextRenderingHighQuality;
                if (AntdUI.Config.TextRenderingHighQuality == setting.TextRenderingHighQuality)
                {
                    return;
                } 
                
                Refresh();
            }
        }

        #endregion
    }
}

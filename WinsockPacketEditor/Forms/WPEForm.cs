using AntdUI;
using System;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor.Forms
{
    public partial class WPEForm : AntdUI.BorderlessForm
    {
        private string RemoteIP = string.Empty;

        #region//窗体初始化

        public WPEForm()
        {
            InitializeComponent();
        }

        private void WPEForm_Load(object sender, EventArgs e)
        {
            this.pageHeader.Text = null;
            this.tabWPEForm.TabMenuVisible = false;
            this.tabWPEForm.SelectTab("tpLogin");

            this.txtRemote_UserName.Text = Operate.SystemConfig.Remote_UserName;
            this.txtRemote_PassWord.Text = Operate.SystemConfig.Remote_PassWord;
            this.nudRemote_Port.Value = Operate.SystemConfig.Remote_Port;
            this.cbIsRemote.Checked = Operate.SystemConfig.IsRemote;

            this.InitStartMode();
            this.InitGlobal();
            this.InitRemote();
        }

        private void WPEForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemConfig_ToDB();
        }        

        private void InitStartMode()
        {
            this.ddlStartMode.Items.Clear();
            this.ddlStartMode.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("代理模式")
                {
                    LocalizationText = "WPEForm.ProxyMode",
                },
                new AntdUI.SelectItem("注入模式")
                {
                    LocalizationText = "WPEForm.InjectMode",
                },
            });

            this.ddlStartMode.SelectedIndex = 0;
        }

        private void InitGlobal()
        {
            var globals = new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("中文","zh-CN"),
                new AntdUI.SelectItem("English","en-US")
            };

            btn_global.Items.AddRange(globals);

            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                btn_global.SelectedValue = globals[1].Tag;
            }
            else
            {
                btn_global.SelectedValue = globals[0].Tag;
            }
        }

        private void InitRemote()
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
                string RemoteURL = this.GetRemoteURL();
                if (!string.IsNullOrEmpty(RemoteURL))
                {
                    this.lRemote.Text = RemoteURL;
                }
            }            
        }

        #endregion

        #region//切换语言

        private void btn_global_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is string lang)
            {
                btn_global.Loading = true;

                if (lang.StartsWith("en"))
                {
                    AntdUI.Localization.Provider = new Localizer();
                }
                else
                {
                    AntdUI.Localization.Provider = null;
                }

                AntdUI.Localization.SetLanguage(lang);
                Refresh();
                btn_global.Loading = false;
            }
        }

        #endregion

        #region//远程管理

        private void bRemote_Click(object sender, EventArgs e)
        {
            this.tabWPEForm.SelectTab("tpRemote");
        }

        private void bSaveRemote_Click(object sender, EventArgs e)
        {
            string Remote_UserName = this.txtRemote_UserName.Text.Trim();
            string Remote_PassWord = this.txtRemote_PassWord.Text.Trim();
            string RemoteURL = this.GetRemoteURL();

            if (this.cbIsRemote.Checked)
            {  
                if (string.IsNullOrEmpty(Remote_UserName))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "管理员账号为空", TType.Error)
                    {
                        LocalizationText = "StartForm.RemoteEmpty"
                    });

                    return;
                }
                
                if (string.IsNullOrEmpty(Remote_PassWord))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "账号密码为空", TType.Error)
                    {
                        LocalizationText = "StartForm.RemoteEmpty"
                    });

                    return;
                }
                
                if (string.IsNullOrEmpty(RemoteURL))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "管理后台地址错误", TType.Error)
                    {
                        LocalizationText = "StartForm.RemoteError"
                    });

                    return;
                }                

                AntdUI.Message.open(new AntdUI.Message.Config(this, "远程管理已启用", TType.Success)
                {
                    LocalizationText = "StartForm.RemoteEnable"
                });
            }
            else
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "远程管理已关闭", TType.Error)
                {
                    LocalizationText = "StartForm.RemoteDisable"
                });                
            }

            Operate.SystemConfig.IsRemote = this.cbIsRemote.Checked;
            Operate.SystemConfig.Remote_UserName = Remote_UserName;
            Operate.SystemConfig.Remote_PassWord = Remote_PassWord;
            Operate.SystemConfig.Remote_Port = ((ushort)this.nudRemote_Port.Value);
            Operate.SystemConfig.Remote_URL = RemoteURL;

            this.tabWPEForm.SelectTab("tpLogin");
        }

        private void nudRemote_Port_ValueChanged(object sender, DecimalEventArgs e)
        {
            string RemoteURL = this.GetRemoteURL();
            if (!string.IsNullOrEmpty(RemoteURL))
            {
                this.lRemote.Text = RemoteURL;
            }
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

        #endregion        
    }
}

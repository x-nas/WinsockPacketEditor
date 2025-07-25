using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace WPE.Lib
{
    public partial class ExternalProxySettingsForm : Form
    {
        #region//窗体事件

        public ExternalProxySettingsForm()
        {
            InitializeComponent();
        }

        private void ExternalProxySettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("ExternalProxySettingsForm", "外部代理设置");

            this.cbEnable_ExternalProxy.Checked = Operate.ProxyConfig.Proxy.Enable_ExternalProxy;
            this.txtExternalProxy_IP.Text = Operate.ProxyConfig.Proxy.ExternalProxy_IP;
            this.txtExternalProxy_Port.Text = Operate.ProxyConfig.Proxy.ExternalProxy_Port.ToString();
            this.cbExternalProxy_AppointPort.Checked = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort;
            this.txtExternalProxy_AppointPort.Text = Operate.ProxyConfig.Proxy.ExternalProxy_AppointPort;
            this.cbExternalProxy_EnableAuth.Checked = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth;
            this.txtExternalProxy_UserName.Text = Operate.ProxyConfig.Proxy.ExternalProxy_UserName;
            this.txtExternalProxy_PassWord.Text = Operate.ProxyConfig.Proxy.ExternalProxy_PassWord;
        }

        #endregion

        #region//启用外部代理

        private void cbEnable_ExternalProxy_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.Enable_ExternalProxyChanged();
        }

        private void Enable_ExternalProxyChanged()
        {
            this.tlpServerInfo.Enabled = this.cbEnable_ExternalProxy.Checked;
        }

        private void txtExternalProxy_IP_TextChanged(object sender, EventArgs e)
        {
            if (this.cbEnable_ExternalProxy.Checked)
            {
                if (string.IsNullOrEmpty(this.txtExternalProxy_IP.Text.Trim()))
                {
                    this.txtExternalProxy_IP.Status = TType.Error;
                }
                else
                {
                    this.txtExternalProxy_IP.Status = TType.Success;
                }
            }
        }

        #endregion

        #region//指定端口

        private void cbExternalProxy_AppointPort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.ExternalProxy_AppointPort_Changed();
        }

        private void ExternalProxy_AppointPort_Changed()
        {
            this.txtExternalProxy_AppointPort.Enabled = this.cbExternalProxy_AppointPort.Checked;
        }

        private void txtExternalProxy_AppointPort_TextChanged(object sender, EventArgs e)
        {
            if (this.cbExternalProxy_AppointPort.Checked)
            {
                if (string.IsNullOrEmpty(this.txtExternalProxy_AppointPort.Text.Trim()))
                {
                    this.txtExternalProxy_AppointPort.Status = TType.Error;
                }
                else
                {
                    this.txtExternalProxy_AppointPort.Status = TType.Success;
                }
            }
        }

        #endregion

        #region//外部代理认证

        private void cbExternalProxy_EnableAuth_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.ExternalProxy_EnableAuth_Changed();
        }

        private void ExternalProxy_EnableAuth_Changed()
        {
            this.txtExternalProxy_UserName.Enabled = 
                this.txtExternalProxy_PassWord.Enabled = 
                this.cbExternalProxy_EnableAuth.Checked;
        }

        private void txtExternalProxy_UserName_TextChanged(object sender, EventArgs e)
        {
            if (this.cbExternalProxy_EnableAuth.Checked)
            {
                if (string.IsNullOrEmpty(this.txtExternalProxy_UserName.Text.Trim()))
                {
                    this.txtExternalProxy_UserName.Status = TType.Error;
                }
                else
                {
                    this.txtExternalProxy_UserName.Status = TType.Success;
                }
            }
        }

        private void txtExternalProxy_PassWord_TextChanged(object sender, EventArgs e)
        {
            if (this.cbExternalProxy_EnableAuth.Checked)
            {
                if (string.IsNullOrEmpty(this.txtExternalProxy_PassWord.Text.Trim()))
                {
                    this.txtExternalProxy_PassWord.Status = TType.Error;
                }
                else
                {
                    this.txtExternalProxy_PassWord.Status = TType.Success;
                }
            }
        }

        #endregion

        #region//外部代理设置有效性

        private bool CheckExternalProxySet()
        {
            try
            {
                //启用外部代理
                if (this.cbEnable_ExternalProxy.Checked)
                {
                    string ExternalProxyIP = this.txtExternalProxy_IP.Text.Trim();
                    if (string.IsNullOrEmpty(ExternalProxyIP))
                    {
                        this.txtExternalProxy_IP.Status = TType.Error;

                        AntdUI.Message.open(new AntdUI.Message.Config(this, "外部代理地址为空", TType.Error)
                        {
                            LocalizationText = "ExternalProxySettingsForm.ServerAddress.Empty"
                        });                        

                        return false;
                    }

                    Operate.ProxyConfig.Proxy.AddressType atExternalProxy = Operate.ProxyConfig.Proxy.GetAddressType_ByString(ExternalProxyIP);
                    if (atExternalProxy != Operate.ProxyConfig.Proxy.AddressType.IPv4 && atExternalProxy != Operate.ProxyConfig.Proxy.AddressType.Domain)
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "外部代理地址错误", TType.Error)
                        {
                            LocalizationText = "ExternalProxySettingsForm.ServerAddress.Error"
                        });

                        return false;
                    }

                    //指定端口
                    if (this.cbExternalProxy_AppointPort.Checked)
                    {
                        string ExternalProxy_AppointPort = this.txtExternalProxy_AppointPort.Text.Trim();
                        if (string.IsNullOrEmpty(ExternalProxy_AppointPort))
                        {
                            this.txtExternalProxy_AppointPort.Status = TType.Error;

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "指定端口为空", TType.Error)
                            {
                                LocalizationText = "ExternalProxySettingsForm.AppointPort.Empty"
                            });

                            return false;
                        }
                    }

                    //外部代理认证
                    if (this.cbExternalProxy_EnableAuth.Checked)
                    {
                        string UserName = this.txtExternalProxy_UserName.Text.Trim();
                        if (string.IsNullOrEmpty(UserName))
                        {
                            this.txtExternalProxy_UserName.Status = TType.Error;

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "认证账号为空", TType.Error)
                            {
                                LocalizationText = "ExternalProxySettingsForm.UserName.Empty"
                            });

                            return false;
                        }

                        string PassWord = this.txtExternalProxy_PassWord.Text.Trim();
                        if (string.IsNullOrEmpty(PassWord))
                        {
                            this.txtExternalProxy_PassWord.Status = TType.Error;

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "认证密码为空", TType.Error)
                            {
                                LocalizationText = "ExternalProxySettingsForm.PassWord.Empty"
                            });

                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region//检测

        private async void bExternalProxy_Detection_Click(object sender, EventArgs e)
        {
            if (!this.CheckExternalProxySet())
            {
                return;
            }

            this.bExternalProxy_Detection.Loading = true;
            bool Result = await Operate.ProxyConfig.Proxy.DetectionExternalProxy(this);

            if (Result)
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "外部代理服务器连接成功", TType.Success)
                {
                    LocalizationText = "ExternalProxySettingsForm.Success"
                });
            }

            this.bExternalProxy_Detection.Loading = false;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!this.CheckExternalProxySet())
            {
                return;
            }

            Operate.ProxyConfig.Proxy.Enable_ExternalProxy = this.cbEnable_ExternalProxy.Checked;
            Operate.ProxyConfig.Proxy.ExternalProxy_IP = this.txtExternalProxy_IP.Text.Trim();
            Operate.ProxyConfig.Proxy.ExternalProxy_Port = ushort.Parse(this.txtExternalProxy_Port.Text.Trim());
            Operate.ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort = this.cbExternalProxy_AppointPort.Checked;
            Operate.ProxyConfig.Proxy.ExternalProxy_AppointPort = this.txtExternalProxy_AppointPort.Text.Trim();
            Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth = this.cbExternalProxy_EnableAuth.Checked;
            Operate.ProxyConfig.Proxy.ExternalProxy_UserName = this.txtExternalProxy_UserName.Text.Trim();
            Operate.ProxyConfig.Proxy.ExternalProxy_PassWord = this.txtExternalProxy_PassWord.Text.Trim();

            AntdUI.Message.open(new AntdUI.Message.Config(this, "外部代理设置保存成功", TType.Success)
            {
                LocalizationText = "ExternalProxySettingsForm.Success"
            });
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion        
    }
}

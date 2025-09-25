using AntdUI;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProxySetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public ProxySetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void ProxySetting_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = AntdUI.Localization.Get("ProxySettingsForm", "代理设置");
                this.cbProxyIP_Auto.Checked = Operate.ProxyConfig.Proxy.ProxyIP_Auto;
                this.cbEnable_SOCKS5.Checked = Operate.ProxyConfig.Proxy.Enable_SOCKS5;
                this.nudSOCKS5Port.Value = Operate.ProxyConfig.Proxy.ProxyPort;
                this.cbEnable_Auth.Checked = Operate.ProxyConfig.Proxy.Enable_Auth;
                this.switchSystemProxy.Checked = Operate.ProxyConfig.Proxy.Enable_SystemProxy;
                this.txtMaxConnectionNumber.Value = Operate.ProxyConfig.Proxy.MaxConnectionNumber;

                if (Operate.ProxyConfig.Proxy.ProxyServerIP == null)
                {
                    Operate.ProxyConfig.Proxy.ProxyServerIP = Operate.SystemConfig.GetLocalIPAddress();
                }

                this.ddlProxyIP_Appoint.Items.Clear();
                this.ddlProxyIP_Appoint.Items.AddRange(Operate.ProxyConfig.Proxy.ProxyServerIP.Select(ip => new SelectItem(ip.ToString(), ip)).ToArray());

                if (IPAddress.TryParse(Operate.ProxyConfig.Proxy.ProxyIP, out IPAddress ipRemoteIP))
                {
                    this.ddlProxyIP_Appoint.SelectedValue = ipRemoteIP;
                }

                if (this.ddlProxyIP_Appoint.SelectedValue == null)
                {
                    this.ddlProxyIP_Appoint.SelectedIndex = 0;
                }

                this.ddlAuthType.Items.Clear();
                this.ddlAuthType.Items.Add(AntdUI.Localization.Get("ProxySettingsForm.UNPW", "用户名 / 密码"));

                if (this.ddlAuthType.Items.Count > 0)
                {
                    this.ddlAuthType.SelectedIndex = 0;
                }

                this.ProxyIP_Appoint_Changed();
                this.EnableSOCKS5_Changed();
                this.Enable_Auth_Changed();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ProxySetting_Load), ex.Message);
            }
        }

        #endregion

        #region//代理服务IP地址

        private void cbProxyIP_Auto_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.ProxyIP_Appoint_Changed();
        }

        private void ProxyIP_Appoint_Changed()
        {
            this.ddlProxyIP_Appoint.Enabled = !this.cbProxyIP_Auto.Checked;
        }

        #endregion        

        #region//代理类型

        private void cbEnable_SOCKS5_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.EnableSOCKS5_Changed();
        }

        private void EnableSOCKS5_Changed()
        {
            this.nudSOCKS5Port.Enabled = this.cbEnable_SOCKS5.Checked;
        }

        #endregion        

        #region//代理认证

        private void cbEnable_Auth_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.Enable_Auth_Changed();
        }

        private void Enable_Auth_Changed()
        {
            this.ddlAuthType.Enabled = this.cbEnable_Auth.Checked;
        }

        #endregion

        #region//系统代理

        private void switchSystemProxy_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.EnableSystemProxy_Changed();
        }

        private void EnableSystemProxy_Changed()
        {
            try
            {
                if (this.switchSystemProxy.Checked)
                {
                    Operate.ProxyConfig.Proxy.Enable_SystemProxy = true;
                    Operate.ProxyConfig.Proxy.EnableSystemProxy(this.form);
                }
                else
                {
                    Operate.ProxyConfig.Proxy.Enable_SystemProxy = false;
                    Operate.ProxyConfig.Proxy.DisableSystemProxy(this.form);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(EnableSystemProxy_Changed), ex.Message);
            }
        }

        #endregion        

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.cbEnable_SOCKS5.Checked)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "代理类型未设置", TType.Error)
                    {
                        LocalizationText = "ProxySettingsForm.ProxyType.Error"
                    });

                    return;
                }

                Operate.ProxyConfig.Proxy.ProxyIP_Auto = this.cbProxyIP_Auto.Checked;
                Operate.ProxyConfig.Proxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Operate.ProxyConfig.Proxy.ProxyIP = this.ddlProxyIP_Appoint.SelectedValue.ToString();
                Operate.ProxyConfig.Proxy.ProxyPort = ((ushort)this.nudSOCKS5Port.Value);
                Operate.ProxyConfig.Proxy.Enable_Auth = this.cbEnable_Auth.Checked;
                Operate.ProxyConfig.Proxy.MaxConnectionNumber = ((int)this.txtMaxConnectionNumber.Value);

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "代理设置保存成功", TType.Success)
                {
                    LocalizationText = "ProxySettingsForm.Success"
                });

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex.Message);
            }
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

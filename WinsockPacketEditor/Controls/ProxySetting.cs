using AntdUI;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
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

                switch (Operate.ProxyConfig.Proxy.MaxConnectionNumber)
                {
                    case 1000:
                        this.sliderMaxLinks.Value = 0;
                        break;

                    case 2000:
                        this.sliderMaxLinks.Value = 1;
                        break;

                    case 3000:
                        this.sliderMaxLinks.Value = 2;
                        break;

                    case 5000:
                        this.sliderMaxLinks.Value = 3;
                        break;

                    case 8000:
                        this.sliderMaxLinks.Value = 4;
                        break;

                    case 12000:
                        this.sliderMaxLinks.Value = 5;
                        break;

                    case 15000:
                        this.sliderMaxLinks.Value = 6;
                        break;

                    case 20000:
                        this.sliderMaxLinks.Value = 7;
                        break;

                    case 30000:
                        this.sliderMaxLinks.Value = 8;
                        break;

                    case 50000:
                        this.sliderMaxLinks.Value = 9;
                        break;
                }

                switch (Operate.ProxyConfig.Proxy.BufferSize)
                {
                    case 8192:
                        this.sliderBufferSize.Value = 0;
                        break;
                    case 16384:
                        this.sliderBufferSize.Value = 2;
                        break;
                    case 32768:
                        this.sliderBufferSize.Value = 4;
                        break;
                    case 65535:
                        this.sliderBufferSize.Value = 6;
                        break;
                    case 131072:
                        this.sliderBufferSize.Value = 8;
                        break;
                    case 262144:
                        this.sliderBufferSize.Value = 10;
                        break;
                }

                this.ProxyIP_Appoint_Changed();
                this.EnableSOCKS5_Changed();
                this.Enable_Auth_Changed();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                switch (this.sliderMaxLinks.Value)
                { 
                    case 0:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 1000;
                        break;

                    case 1:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 2000;
                        break;

                    case 2:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 3000;
                        break;

                    case 3:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 5000;
                        break;

                    case 4:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 8000;
                        break;

                    case 5:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 12000;
                        break;

                    case 6:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 15000;
                        break;

                    case 7:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 20000;
                        break;

                    case 8:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 30000;
                        break;

                    case 9:
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = 50000;
                        break;
                }

                switch (this.sliderBufferSize.Value)
                {
                    case 0:
                        Operate.ProxyConfig.Proxy.BufferSize = 8192;
                        break;
                    case 2:
                        Operate.ProxyConfig.Proxy.BufferSize = 16384;
                        break;
                    case 4:
                        Operate.ProxyConfig.Proxy.BufferSize = 32768;
                        break;
                    case 6:
                        Operate.ProxyConfig.Proxy.BufferSize = 65535;
                        break;
                    case 8:
                        Operate.ProxyConfig.Proxy.BufferSize = 131072;
                        break;
                    case 10:
                        Operate.ProxyConfig.Proxy.BufferSize = 262144;
                        break;
                }

                Operate.ProxyConfig.Proxy.ProxyIP_Auto = this.cbProxyIP_Auto.Checked;
                Operate.ProxyConfig.Proxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Operate.ProxyConfig.Proxy.ProxyIP = this.ddlProxyIP_Appoint.SelectedValue.ToString();
                Operate.ProxyConfig.Proxy.ProxyPort = ((ushort)this.nudSOCKS5Port.Value);
                Operate.ProxyConfig.Proxy.Enable_Auth = this.cbEnable_Auth.Checked;

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "代理设置保存成功", TType.Success)
                {
                    LocalizationText = "ProxySettingsForm.Success"
                });

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

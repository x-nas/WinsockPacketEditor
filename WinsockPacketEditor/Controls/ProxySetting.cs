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
                this.nudSOCKS5Port.Value = Operate.ProxyConfig.Proxy.SOCKS5_Port;
                this.cbEnable_Auth.Checked = Operate.ProxyConfig.Proxy.Enable_Auth;
                this.cbEnable_HTTP.Checked = Operate.ProxyConfig.Proxy.Enable_HTTP;                
                this.nudHTTPPort.Value = Operate.ProxyConfig.Proxy.HTTP_Port;                
                this.switchSystemProxy.Checked = Operate.ProxyConfig.Proxy.Enable_SystemProxy;
                this.txtMaxConnectionNumber.Value = Operate.ProxyConfig.Proxy.MaxConnectionNumber;

                this.InitProxyIP();
                this.InitAuthType();
                this.InitCertType();
                this.ProxyIP_Appoint_Changed();
                this.EnableSOCKS5_Changed();
                this.Enable_Auth_Changed();
                this.Enable_HTTP_Changed();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ProxySetting_Load), ex);
            }
        }

        private void InitProxyIP()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitProxyIP), ex);
            }            
        }

        private void InitAuthType()
        {
            try
            {
                this.ddlAuthType.Items.Clear();
                this.ddlAuthType.Items.Add(AntdUI.Localization.Get("ProxySettingsForm.UNPW", "用户名 / 密码"));

                if (this.ddlAuthType.Items.Count > 0)
                {
                    this.ddlAuthType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitAuthType), ex);
            }            
        }

        private void InitCertType()
        {
            try
            {
                this.ddlCertType.Items.Clear();
                this.ddlCertType.Items.Add(AntdUI.Localization.Get("CerFile.Binary", "Cer 证书 ( 二进制格式 )"));
                this.ddlCertType.Items.Add(AntdUI.Localization.Get("CerFile.Base64", "Cer 证书 ( 文本格式 )"));
                this.ddlCertType.Items.Add(AntdUI.Localization.Get("CrtFile.Binary", "Crt 证书 ( 二进制格式 )"));
                this.ddlCertType.Items.Add(AntdUI.Localization.Get("CrtFile.Base64", "Crt 证书 ( 文本格式 )"));
                this.ddlCertType.Items.Add(AntdUI.Localization.Get("PemFile.Base64", "Pem 证书 ( 文本格式 )"));
                this.ddlCertType.Items.Add(AntdUI.Localization.Get("AndroidFile.Hash", "安卓系统 证书 ( 9a7ae4b0.0 )"));

                if (this.ddlCertType.Items.Count > 0)
                {
                    this.ddlCertType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitCertType), ex);
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

        #region//SOCKS 代理

        private void cbEnable_SOCKS5_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.EnableSOCKS5_Changed();
        }

        private void EnableSOCKS5_Changed()
        {
            this.nudSOCKS5Port.Enabled = this.cbEnable_SOCKS5.Checked;
        }

        #endregion        

        #region//HTTP 代理

        private void cbEnable_HTTP_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.Enable_HTTP_Changed();
        }

        private void Enable_HTTP_Changed()
        {
            this.nudHTTPPort.Enabled = this.cbEnable_HTTP.Checked;
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
                Operate.DoLog(nameof(EnableSystemProxy_Changed), ex);
            }
        }

        #endregion

        #region//导出证书

        private void bExportCert_Click(object sender, EventArgs e)
        {
            try
            {
                int CerType = this.ddlCertType.SelectedIndex;
                string CertName = string.Empty;

                if (CerType == 5)
                {
                    CertName = "9a7ae4b0";
                }
                else
                {
                    CertName = "WPE64";
                }

                Operate.ProxyConfig.Proxy.SaveCertToFile_Dialog(this.form, CerType, CertName);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bExportCert_Click), ex);
            }
        }

        #endregion

        #region//检查保存设置

        private bool CheckSetting()
        {
            try
            {
                if (!this.cbEnable_SOCKS5.Checked)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "代理类型未设置", TType.Error)
                    {
                        LocalizationText = "ProxySettingsForm.ProxyType.Error"
                    });

                    return false;
                }

                if (this.cbEnable_HTTP.Checked)
                {
                    if (this.nudSOCKS5Port.Value == this.nudHTTPPort.Value)
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "SOCKS 和 HTTP 端口不能相同", TType.Error)
                        {
                            LocalizationText = "ProxySettingsForm.ProxyType.Error"
                        });

                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CheckSetting), ex);
            }

            return false;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckSetting())
                {
                    return;
                }                

                Operate.ProxyConfig.Proxy.ProxyIP_Auto = this.cbProxyIP_Auto.Checked;
                Operate.ProxyConfig.Proxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Operate.ProxyConfig.Proxy.ProxyIP = this.ddlProxyIP_Appoint.SelectedValue.ToString();
                Operate.ProxyConfig.Proxy.SOCKS5_Port = ((ushort)this.nudSOCKS5Port.Value);
                Operate.ProxyConfig.Proxy.Enable_Auth = this.cbEnable_Auth.Checked;
                Operate.ProxyConfig.Proxy.MaxConnectionNumber = ((int)this.txtMaxConnectionNumber.Value);
                Operate.ProxyConfig.Proxy.Enable_HTTP = this.cbEnable_HTTP.Checked;                
                Operate.ProxyConfig.Proxy.HTTP_Port = ((ushort)this.nudHTTPPort.Value);

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "代理设置保存成功", TType.Success)
                {
                    LocalizationText = "ProxySettingsForm.Success"
                });

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);
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

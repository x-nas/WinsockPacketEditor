using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ListSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public ListSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void ListSetting_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("ListSettingsForm", "列表设置");
            this.tabListSettings.TabMenuVisible = false;

            if (this.form is InterfaceInfo.IInjectMode)
            {
                this.tabListSettings.SelectTab(0);

                this.cbIsShow_ProxyTime_Inject.Checked = Operate.PacketConfig.List.IsShow_ProxyTime;
                this.cbIsShow_PacketSocket_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketSocket;
                this.cbIsShow_PacketType_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketType;
                this.cbIsShow_ClientAddr_Inject.Checked = Operate.PacketConfig.List.IsShow_ClientAddr;
                this.cbIsShow_ClientLocation_Inject.Checked = Operate.PacketConfig.List.IsShow_ClientLocation;
                this.cbIsShow_ServerAddr_Inject.Checked = Operate.PacketConfig.List.IsShow_ServerAddr;
                this.cbIsShow_ServerLocation_Inject.Checked = Operate.PacketConfig.List.IsShow_ServerLocation;
                this.cbIsShow_PacketLen_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketLen;
                this.cbIsShow_PacketData_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketData;
            }
            else if (this.form is InterfaceInfo.IProxyMode)
            {
                this.tabListSettings.SelectTab(1);

                this.cbIsShow_ID_Proxy.Checked = Operate.ProxyConfig.List.IsShow_ID;
                this.cbIsShow_ProxyTime_Proxy.Checked = Operate.ProxyConfig.List.IsShow_ProxyTime;
                this.cbIsShow_PacketSocket_Proxy.Checked = Operate.ProxyConfig.List.IsShow_PacketSocket;
                this.cbIsShow_PacketType_Proxy.Checked = Operate.ProxyConfig.List.IsShow_PacketType;
                this.cbIsShow_ClientAddr_Proxy.Checked = Operate.ProxyConfig.List.IsShow_ClientAddr;
                this.cbIsShow_ClientLocation_Proxy.Checked = Operate.ProxyConfig.List.IsShow_ClientLocation;
                this.cbIsShow_ServerAddr_Proxy.Checked = Operate.ProxyConfig.List.IsShow_ServerAddr;
                this.cbIsShow_ServerLocation_Proxy.Checked = Operate.ProxyConfig.List.IsShow_ServerLocation;
                this.cbIsShow_PacketLen_Proxy.Checked = Operate.ProxyConfig.List.IsShow_PacketLen;
                this.cbIsShow_PacketData_Proxy.Checked = Operate.ProxyConfig.List.IsShow_PacketData;
            }
        }

        #endregion      

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.form is InterfaceInfo.IInjectMode injectForm)
                {
                    Operate.PacketConfig.List.IsShow_ID = this.cbIsShow_ID_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_ProxyTime = this.cbIsShow_ProxyTime_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_PacketSocket = this.cbIsShow_PacketSocket_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_PacketType = this.cbIsShow_PacketType_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_ClientAddr = this.cbIsShow_ClientAddr_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_ClientLocation = this.cbIsShow_ClientLocation_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_ServerAddr = this.cbIsShow_ServerAddr_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_ServerLocation = this.cbIsShow_ServerLocation_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_PacketLen = this.cbIsShow_PacketLen_Inject.Checked;
                    Operate.PacketConfig.List.IsShow_PacketData = this.cbIsShow_PacketData_Inject.Checked;

                    injectForm.SetColumnVisible_PacketList();
                }
                else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                {
                    Operate.ProxyConfig.List.IsShow_ID = this.cbIsShow_ID_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_ProxyTime = this.cbIsShow_ProxyTime_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_PacketSocket = this.cbIsShow_PacketSocket_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_PacketType = this.cbIsShow_PacketType_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_ClientAddr = this.cbIsShow_ClientAddr_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_ClientLocation = this.cbIsShow_ClientLocation_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_ServerAddr = this.cbIsShow_ServerAddr_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_ServerLocation = this.cbIsShow_ServerLocation_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_PacketLen = this.cbIsShow_PacketLen_Proxy.Checked;
                    Operate.ProxyConfig.List.IsShow_PacketData = this.cbIsShow_PacketData_Proxy.Checked;

                    proxyForm.SetColumnVisible_ProxyList();
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "列表设置保存成功", TType.Success)
                {
                    LocalizationText = "ListSettingsForm.Success"
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

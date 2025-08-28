using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ListSettingsForm : Form
    {
        private Form form;

        #region//窗体事件

        public ListSettingsForm(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void ListSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("ListSettingsForm", "列表设置");

            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    this.tabListSettings.SelectTab(0);

                    this.cbPacketList_AutoRoll.Checked = Operate.PacketConfig.List.AutoRoll;
                    this.cbPacketList_AutoClear.Checked = Operate.PacketConfig.List.AutoClear;
                    this.txtPacketList_AutoClear.Value = Operate.PacketConfig.List.AutoClear_Value;
                    this.cbIsShow_ProxyTime_Inject.Checked = Operate.PacketConfig.List.IsShow_ProxyTime;
                    this.cbIsShow_PacketSocket_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketSocket;
                    this.cbIsShow_PacketType_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketType;
                    this.cbIsShow_ClientAddr_Inject.Checked = Operate.PacketConfig.List.IsShow_ClientAddr;
                    this.cbIsShow_ClientLocation_Inject.Checked = Operate.PacketConfig.List.IsShow_ClientLocation;
                    this.cbIsShow_ServerAddr_Inject.Checked = Operate.PacketConfig.List.IsShow_ServerAddr;
                    this.cbIsShow_ServerLocation_Inject.Checked = Operate.PacketConfig.List.IsShow_ServerLocation;
                    this.cbIsShow_PacketLen_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketLen;
                    this.cbIsShow_PacketData_Inject.Checked = Operate.PacketConfig.List.IsShow_PacketData;

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    this.tabListSettings.SelectTab(1);

                    this.cbPacketList_AutoRoll.Checked = Operate.ProxyConfig.List.AutoRoll;
                    this.cbPacketList_AutoClear.Checked = Operate.ProxyConfig.List.AutoClear;
                    this.txtPacketList_AutoClear.Value = Operate.ProxyConfig.List.AutoClear_Value;
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

                    break;
            }
            
            this.tabListSettings.TabMenuVisible = false;
            this.cbLogList_AutoRoll.Checked = Operate.LogConfig.List.AutoRoll;
            this.cbLogList_AutoClear.Checked = Operate.LogConfig.List.AutoClear;
            this.txtLogList_AutoClear.Value = Operate.LogConfig.List.AutoClear_Value;         

            this.PacketList_AutoClear_Changed();
            this.LogList_AutoClear_Changed();
        }

        #endregion

        #region//代理列表

        private void cbPacketList_AutoClear_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.PacketList_AutoClear_Changed();
        }

        private void PacketList_AutoClear_Changed()
        {
            this.txtPacketList_AutoClear.Enabled = this.cbPacketList_AutoClear.Checked;
        }

        #endregion

        #region//日志列表

        private void cbLogList_AutoClear_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.LogList_AutoClear_Changed();
        }

        private void LogList_AutoClear_Changed()
        {
            this.txtLogList_AutoClear.Enabled = this.cbLogList_AutoClear.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    Operate.PacketConfig.List.AutoRoll = this.cbPacketList_AutoRoll.Checked;
                    Operate.PacketConfig.List.AutoClear = this.cbPacketList_AutoClear.Checked;
                    Operate.PacketConfig.List.AutoClear_Value = this.txtPacketList_AutoClear.Value;
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

                    ((InterfaceInfo.IInjectMode)form).SetColumnVisible_ProxyList();

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    Operate.ProxyConfig.List.AutoRoll = this.cbPacketList_AutoRoll.Checked;
                    Operate.ProxyConfig.List.AutoClear = this.cbPacketList_AutoClear.Checked;
                    Operate.ProxyConfig.List.AutoClear_Value = this.txtPacketList_AutoClear.Value;
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

                    ((InterfaceInfo.IProxyMode)form).SetColumnVisible_ProxyList();

                    break;
            }
            
            Operate.LogConfig.List.AutoRoll = this.cbLogList_AutoRoll.Checked;
            Operate.LogConfig.List.AutoClear = this.cbLogList_AutoClear.Checked;
            Operate.LogConfig.List.AutoClear_Value = this.txtLogList_AutoClear.Value;        

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "列表设置保存成功", TType.Success)
            {
                LocalizationText = "ListSettingsForm.Success"
            });

            this.Dispose();
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

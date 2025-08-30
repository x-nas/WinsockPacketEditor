using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class HookSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public HookSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void HookSetting_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("HookSettingsForm", "拦截设置");
            this.tabHookSettings.TabMenuVisible = false;

            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:
                    this.tabHookSettings.SelectTab("tpInjectMode");
                    break;

                case Operate.SystemConfig.SystemMode.Proxy:
                    this.tabHookSettings.SelectTab("tpProxyMode");
                    break;
            }

            this.cbHookWS1_Send.Checked = Operate.PacketConfig.Packet.HookWS1_Send;
            this.cbHookWS1_SendTo.Checked = Operate.PacketConfig.Packet.HookWS1_SendTo;
            this.cbHookWS1_Recv.Checked = Operate.PacketConfig.Packet.HookWS1_Recv;
            this.cbHookWS1_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWS1_RecvFrom;
            this.cbHookWS2_Send.Checked = Operate.PacketConfig.Packet.HookWS2_Send;
            this.cbHookWS2_SendTo.Checked = Operate.PacketConfig.Packet.HookWS2_SendTo;
            this.cbHookWS2_Recv.Checked = Operate.PacketConfig.Packet.HookWS2_Recv;
            this.cbHookWS2_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWS2_RecvFrom;
            this.cbHookWSA_Send.Checked = Operate.PacketConfig.Packet.HookWSA_Send;
            this.cbHookWSA_SendTo.Checked = Operate.PacketConfig.Packet.HookWSA_SendTo;
            this.cbHookWSA_Recv.Checked = Operate.PacketConfig.Packet.HookWSA_Recv;
            this.cbHookWSA_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWSA_RecvFrom;
            this.cbTCP_Req.Checked = Operate.ProxyConfig.Proxy.HookTCP_Req;
            this.cbTCP_Resp.Checked = Operate.ProxyConfig.Proxy.HookTCP_Resp;
            this.cbUDP_Req.Checked = Operate.ProxyConfig.Proxy.HookUDP_Req;
            this.cbUDP_Resp.Checked = Operate.ProxyConfig.Proxy.HookUDP_Resp;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.PacketConfig.Packet.HookWS1_Send = this.cbHookWS1_Send.Checked;
            Operate.PacketConfig.Packet.HookWS1_SendTo = this.cbHookWS1_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWS1_Recv = this.cbHookWS1_Recv.Checked;
            Operate.PacketConfig.Packet.HookWS1_RecvFrom = this.cbHookWS1_RecvFrom.Checked;
            Operate.PacketConfig.Packet.HookWS2_Send = this.cbHookWS2_Send.Checked;
            Operate.PacketConfig.Packet.HookWS2_SendTo = this.cbHookWS2_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWS2_Recv = this.cbHookWS2_Recv.Checked;
            Operate.PacketConfig.Packet.HookWS2_RecvFrom = this.cbHookWS2_RecvFrom.Checked;
            Operate.PacketConfig.Packet.HookWSA_Send = this.cbHookWSA_Send.Checked;
            Operate.PacketConfig.Packet.HookWSA_SendTo = this.cbHookWSA_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWSA_Recv = this.cbHookWSA_Recv.Checked;
            Operate.PacketConfig.Packet.HookWSA_RecvFrom = this.cbHookWSA_RecvFrom.Checked;
            Operate.ProxyConfig.Proxy.HookTCP_Req = this.cbTCP_Req.Checked;
            Operate.ProxyConfig.Proxy.HookTCP_Resp = this.cbTCP_Resp.Checked;
            Operate.ProxyConfig.Proxy.HookUDP_Req = this.cbUDP_Req.Checked;
            Operate.ProxyConfig.Proxy.HookUDP_Resp = this.cbUDP_Resp.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "拦截设置保存成功", TType.Success)
            {
                LocalizationText = "HookSettingsForm.Success"
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

using AntdUI;
using System;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class HookSettingsForm : Form
    {
        #region//窗体事件

        public HookSettingsForm()
        {
            InitializeComponent();
        }

        private void HookSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("HookSettingsForm", "拦截设置");

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
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.PacketConfig.Packet.HookWS1_Send = cbHookWS1_Send.Checked;
            Operate.PacketConfig.Packet.HookWS1_SendTo = cbHookWS1_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWS1_Recv = cbHookWS1_Recv.Checked;
            Operate.PacketConfig.Packet.HookWS1_RecvFrom = cbHookWS1_RecvFrom.Checked;
            Operate.PacketConfig.Packet.HookWS2_Send = cbHookWS2_Send.Checked;
            Operate.PacketConfig.Packet.HookWS2_SendTo = cbHookWS2_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWS2_Recv = cbHookWS2_Recv.Checked;
            Operate.PacketConfig.Packet.HookWS2_RecvFrom = cbHookWS2_RecvFrom.Checked;
            Operate.PacketConfig.Packet.HookWSA_Send = cbHookWSA_Send.Checked;
            Operate.PacketConfig.Packet.HookWSA_SendTo = cbHookWSA_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWSA_Recv = cbHookWSA_Recv.Checked;
            Operate.PacketConfig.Packet.HookWSA_RecvFrom = cbHookWSA_RecvFrom.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this, "拦截设置保存成功", TType.Success)
            {
                LocalizationText = "HookSettingsForm.Success"
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

using AntdUI;
using System;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class ListSettingsForm : Form
    {
        #region//窗体事件

        public ListSettingsForm()
        {
            InitializeComponent();
        }

        private void ListSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("ListSettingsForm", "列表设置");

            this.cbPacketList_AutoRoll.Checked = Operate.PacketConfig.List.AutoRoll;
            this.cbPacketList_AutoClear.Checked = Operate.PacketConfig.List.AutoClear;
            this.txtPacketList_AutoClear.Value = Operate.PacketConfig.List.AutoClear_Value;
            this.PacketList_AutoClear_Changed();

            this.cbLogList_AutoRoll.Checked = Operate.LogConfig.AutoRoll;
            this.cbLogList_AutoClear.Checked = Operate.LogConfig.AutoClear;
            this.txtLogList_AutoClear.Value = Operate.LogConfig.AutoClear_Value;
            this.LogList_AutoClear_Changed();
        }

        private void cbPacketList_AutoClear_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.PacketList_AutoClear_Changed();
        }

        private void PacketList_AutoClear_Changed()
        {
            this.txtPacketList_AutoClear.Enabled = this.cbPacketList_AutoClear.Checked;
        }

        private void cbLogList_AutoClear_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
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
            Operate.PacketConfig.List.AutoRoll = this.cbPacketList_AutoRoll.Checked;
            Operate.PacketConfig.List.AutoClear = this.cbPacketList_AutoClear.Checked;
            Operate.PacketConfig.List.AutoClear_Value = this.txtPacketList_AutoClear.Value;

            Operate.LogConfig.AutoRoll = this.cbLogList_AutoRoll.Checked;
            Operate.LogConfig.AutoClear = this.cbLogList_AutoClear.Checked;
            Operate.LogConfig.AutoClear_Value = this.txtLogList_AutoClear.Value;

            AntdUI.Message.open(new AntdUI.Message.Config(this, "列表设置保存成功", TType.Success)
            {
                LocalizationText = "ListSettingsForm.Success"
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

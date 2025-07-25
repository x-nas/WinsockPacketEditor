using AntdUI;
using System;
using System.Windows.Forms;

namespace WPE.Lib
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
            this.Text = AntdUI.Localization.Get("ListSettingsForm", "列表设置");

            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    this.cbPacketList_AutoRoll.Checked = Operate.PacketConfig.List.AutoRoll;
                    this.cbPacketList_AutoClear.Checked = Operate.PacketConfig.List.AutoClear;
                    this.txtPacketList_AutoClear.Value = Operate.PacketConfig.List.AutoClear_Value;

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    this.cbPacketList_AutoRoll.Checked = Operate.ProxyConfig.List.AutoRoll;
                    this.cbPacketList_AutoClear.Checked = Operate.ProxyConfig.List.AutoClear;
                    this.txtPacketList_AutoClear.Value = Operate.ProxyConfig.List.AutoClear_Value;

                    break;
            }
            
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

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    Operate.ProxyConfig.List.AutoRoll = this.cbPacketList_AutoRoll.Checked;
                    Operate.ProxyConfig.List.AutoClear = this.cbPacketList_AutoClear.Checked;
                    Operate.ProxyConfig.List.AutoClear_Value = this.txtPacketList_AutoClear.Value;

                    break;
            }
            
            Operate.LogConfig.List.AutoRoll = this.cbLogList_AutoRoll.Checked;
            Operate.LogConfig.List.AutoClear = this.cbLogList_AutoClear.Checked;
            Operate.LogConfig.List.AutoClear_Value = this.txtLogList_AutoClear.Value;        

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

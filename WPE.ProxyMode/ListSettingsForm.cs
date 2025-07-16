using AntdUI;
using System;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.ProxyMode
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
            
            this.cbLogList_AutoRoll.Checked = Operate.LogConfig.List.AutoRoll;
            this.cbLogList_AutoClear.Checked = Operate.LogConfig.List.AutoClear;
            this.txtLogList_AutoClear.Value = Operate.LogConfig.List.AutoClear_Value;
            this.cbNoRecordData.Checked = Operate.ProxyConfig.Proxy.NoRecord;
            this.cbDeleteClosed.Checked = Operate.ProxyConfig.Proxy.DelClosed;

            this.ProxyList_AutoClear_Changed();
            this.LogList_AutoClear_Changed();
        }

        #endregion

        #region//代理列表

        private void cbProxyList_AutoClear_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ProxyList_AutoClear_Changed();
        }

        private void ProxyList_AutoClear_Changed()
        {
            this.txtProxyList_AutoClear.Enabled = this.cbProxyList_AutoClear.Checked;
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
            Operate.LogConfig.List.AutoRoll = this.cbLogList_AutoRoll.Checked;
            Operate.LogConfig.List.AutoClear = this.cbLogList_AutoClear.Checked;
            Operate.LogConfig.List.AutoClear_Value = this.txtLogList_AutoClear.Value;
            Operate.ProxyConfig.Proxy.NoRecord = this.cbNoRecordData.Checked;
            Operate.ProxyConfig.Proxy.DelClosed = this.cbDeleteClosed.Checked;

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

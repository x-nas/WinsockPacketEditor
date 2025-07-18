using AntdUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.ProxyMode
{
    public partial class LimitDevicesForm : Form
    {
        private List<AccountInfo> aiList;

        #region//窗体事件

        public LimitDevicesForm(ProxyModeForm form, List<AccountInfo> aiList)
        {
            InitializeComponent();
            this.aiList = aiList;
        }

        private void LimitDevicesForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("LimitDevicesForm", "调整设备数");
            this.lAccountCNT.Text = string.Format(AntdUI.Localization.Get("ProxyMode.AccountCNT", "批量调整 ( {0} ) 个账号"), this.aiList.Count);

            this.IsLimitDevices_Changed();
        }

        private void cbIsLimitDevices_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.IsLimitDevices_Changed();
        }

        private void IsLimitDevices_Changed()
        { 
            this.nudLimitDevices.Enabled = this.cbIsLimitDevices.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.aiList.Count > 0)
                {
                    bool IsLimitDevices = this.cbIsLimitDevices.Checked;
                    int LimitDevices = ((int)this.nudLimitDevices.Value);

                    Operate.ProxyConfig.Account.AdjustLimitDevices(this.aiList, IsLimitDevices, LimitDevices);

                    AntdUI.Message.open(new AntdUI.Message.Config(this, "批量调整完成", TType.Success)
                    {
                        LocalizationText = "ProxyMode.Adjust.Success"
                    });
                }
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

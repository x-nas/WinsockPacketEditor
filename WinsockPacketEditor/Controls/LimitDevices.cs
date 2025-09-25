using AntdUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LimitDevices : UserControl
    {
        private Form form;
        private List<AccountInfo> aiList;

        #region//窗体事件

        public LimitDevices(Form form, List<AccountInfo> aiList)
        {
            InitializeComponent();
            this.form = form;
            this.aiList = aiList;
        }

        private void LimitDevices_Load(object sender, EventArgs e)
        {
            this.lAccountCNT.Text = string.Format(AntdUI.Localization.Get("BatchAccounts", "批量调整 ( {0} ) 个账号"), this.aiList.Count);

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

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "批量调整完成", TType.Success)
                    {
                        LocalizationText = "BatchSuccess"
                    });

                    if (this.form is InterfaceInfo.IAccountList alForm)
                    {
                        Operate.ProxyConfig.Account.NeedSave = true;
                        alForm.RefreshAccountList();
                    }

                    this.Dispose();
                }
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

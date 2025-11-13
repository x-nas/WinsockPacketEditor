using AntdUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LimitLinks : UserControl
    {
        private Form form;
        private List<AccountInfo> aiList;

        #region//窗体事件

        public LimitLinks(Form form, List<AccountInfo> aiList)
        {
            InitializeComponent();
            this.form = form;
            this.aiList = aiList;
        }

        private void LimitLinks_Load(object sender, EventArgs e)
        {
            this.lAccountCNT.Text = string.Format(AntdUI.Localization.Get("BatchAccounts", "批量调整 ( {0} ) 个账号"), this.aiList.Count);

            this.IsLimitLinks_Changed();
        }

        private void cbIsLimitLinks_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.IsLimitLinks_Changed();
        }

        private void IsLimitLinks_Changed()
        {
            this.nudLimitLinks.Enabled = this.cbIsLimitLinks.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.aiList.Count > 0)
                {
                    bool IsLimitLinks = this.cbIsLimitLinks.Checked;
                    int LimitLinks = ((int)this.nudLimitLinks.Value);

                    Operate.ProxyConfig.Account.AdjustLimitLinks(this.aiList, IsLimitLinks, LimitLinks);

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "批量调整完成", TType.Success)
                    {
                        LocalizationText = "BatchSuccess"
                    });

                    if (this.form is InterfaceInfo.IProxyMode pmForm)
                    {
                        pmForm.RefreshAccountList();
                    }

                    this.Dispose();
                }
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

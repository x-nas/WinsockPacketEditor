using AntdUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LimitLinksForm : Form
    {
        private List<AccountInfo> aiList;

        #region//窗体事件

        public LimitLinksForm(ProxyModeForm form, List<AccountInfo> aiList)
        {
            InitializeComponent();
            this.aiList = aiList;
        }

        private void LimitLinksForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("LimitLinksForm", "调整链接数");
            this.lAccountCNT.Text = string.Format(AntdUI.Localization.Get("ProxyMode.AccountCNT", "批量调整 ( {0} ) 个账号"), this.aiList.Count);

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

                    AntdUI.Message.open(new AntdUI.Message.Config(this, "批量调整完成", TType.Success)
                    {
                        LocalizationText = "ProxyMode.Adjust.Success"
                    });

                    Operate.ProxyConfig.Account.NeedSave = true;
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

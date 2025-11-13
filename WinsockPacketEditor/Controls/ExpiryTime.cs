using AntdUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ExpiryTime : UserControl
    {
        private Form form;
        private List<AccountInfo> aiList;

        #region//窗体事件

        public ExpiryTime(Form form, List<AccountInfo> aiList)
        {
            InitializeComponent();
            this.form = form;
            this.aiList = aiList;
        }

        private void ExpiryTime_Load(object sender, EventArgs e)
        {
            this.lAccountCNT.Text = string.Format(AntdUI.Localization.Get("BatchAccounts", "批量调整 ( {0} ) 个账号"), this.aiList.Count);
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.aiList.Count > 0)
                {
                    int AddHours = ((int)this.nudAddTime.Value);
                    if (this.rbAddDay.Checked)
                    {
                        AddHours = AddHours * 24;
                    }

                    int AddType = 0;
                    if (this.rbFromNow.Checked)
                    {
                        AddType = 1;
                    }

                    Operate.ProxyConfig.Account.AdjustExpiryTime(this.aiList, AddType, AddHours);

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

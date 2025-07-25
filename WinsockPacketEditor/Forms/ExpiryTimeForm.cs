using AntdUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ExpiryTimeForm : Form
    {
        private List<AccountInfo> aiList;

        #region//窗体事件

        public ExpiryTimeForm(ProxyModeForm form, List<AccountInfo> aiList)
        {
            InitializeComponent();
            this.aiList = aiList;
        }

        private void ExpiryTimeForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("ExpiryTimeForm", "调整过期时间");

            this.lAccountCNT.Text = string.Format(AntdUI.Localization.Get("ProxyMode.ExpiryTime", "批量调整 ( {0} ) 个账号"), this.aiList.Count);
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

                    AntdUI.Message.open(new AntdUI.Message.Config(this, "批量调整完成", TType.Success)
                    {
                        LocalizationText = "ExpiryTimeForm.Success"
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

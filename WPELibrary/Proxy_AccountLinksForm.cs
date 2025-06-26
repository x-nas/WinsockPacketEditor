using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountLinksForm : Form
    {
        private readonly List<Guid> gList = new List<Guid>();

        #region//初始化

        public Proxy_AccountLinksForm(List<Guid> gList)
        {
            InitializeComponent();

            if (gList != null)
            {
                this.gList = gList;

                this.InitForm();
            }
        }

        private void cbIsLimitLinks_CheckedChanged(object sender, EventArgs e)
        {
            this.nudLimitLinks.Enabled = cbIsLimitLinks.Checked;
        }

        private void InitForm()
        {
            try
            {
                string sAccountInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_171), this.gList.Count);
                this.lAccountCNT.Text = sAccountInfo;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//确定

        private void bSure_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gList.Count > 0)
                {
                    bool IsLimitLinks = this.cbIsLimitLinks.Checked;
                    int LimitLinks = ((int)this.nudLimitLinks.Value);

                    Operate.ProxyConfig.ProxyAccount.ProxyAccountLinks_Dialog(this.gList, IsLimitLinks, LimitLinks);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//取消

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion        
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountDevicesForm : Form
    {
        private readonly List<Guid> gList = new List<Guid>();

        #region//初始化

        public Proxy_AccountDevicesForm(List<Guid> gList)
        {
            InitializeComponent();

            if (gList != null)
            {
                this.gList = gList;

                this.InitForm();
            }
        }

        private void cbIsLimitDevices_CheckedChanged(object sender, EventArgs e)
        {
            this.nudLimitDevices.Enabled = this.cbIsLimitDevices.Checked;
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    bool IsLimitDevices = this.cbIsLimitDevices.Checked;
                    int LimitDevices = ((int)this.nudLimitDevices.Value);
                 
                    Socket_Cache.ProxyAccount.ProxyAccountDevices_Dialog(this.gList, IsLimitDevices, LimitDevices);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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

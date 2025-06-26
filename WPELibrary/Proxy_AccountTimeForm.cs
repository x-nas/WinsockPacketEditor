using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountTimeForm : Form
    {
        private readonly List<Guid> gList = new List<Guid>();

        #region//初始化

        public Proxy_AccountTimeForm(List<Guid> gList)
        {
            InitializeComponent();

            if (gList != null)
            { 
                this.gList = gList;

                this.InitForm();
            }
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
                    int AddHours = ((int)this.nudAddTime_Value.Value);
                
                    if (this.rbAddDay.Checked)
                    {
                        AddHours = AddHours * 24;
                    }

                    Operate.ProxyConfig.ProxyAccount.ProxyAccountAddTime_Dialog(this.gList, AddHours);
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

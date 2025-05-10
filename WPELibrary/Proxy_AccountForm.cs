using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountForm : Form
    {
        private int AccountIndex = -1;

        #region//初始化

        public Proxy_AccountForm(int AccountIndex)
        {
            InitializeComponent();

            if (AccountIndex > -1)
            { 
                this.AccountIndex = AccountIndex;
            }

            this.InitForm();
        }        

        private void InitForm()
        {
            try
            {
                if (this.AccountIndex > -1 && this.AccountIndex < Socket_Cache.ProxyAccount.lstProxyAccount.Count)
                {
                    bool IsEnable = Socket_Cache.ProxyAccount.lstProxyAccount[this.AccountIndex].IsEnable;
                    string UserName = Socket_Cache.ProxyAccount.lstProxyAccount[this.AccountIndex].UserName;
                    string PassWord = Socket_Cache.ProxyAccount.lstProxyAccount[this.AccountIndex].PassWord;
                    bool IsExpiry = Socket_Cache.ProxyAccount.lstProxyAccount[this.AccountIndex].IsExpiry;
                    DateTime ExpiryTime = Socket_Cache.ProxyAccount.lstProxyAccount[this.AccountIndex].ExpiryTime;

                    this.cbIsEnable.Checked = IsEnable;
                    this.txtUserName.Text = UserName;
                    this.txtPassWord.Text = PassWord;
                    this.cbIsExpiry.Checked = IsExpiry;
                    this.dtpExpiryTime.Value = ExpiryTime;

                    this.txtUserName.Enabled = false;
                }
                else
                {
                    this.dtpExpiryTime.Value = DateTime.Now;
                    this.txtUserName.Enabled = true;
                }

                this.IsEnable_CheckedChanged();
                this.ExpiryTime_CheckedChanged();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckAccountInfo())
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_176));
                    return;
                }

                bool IsEnable = this.cbIsEnable.Checked;
                string UserName = this.txtUserName.Text.Trim();
                string Password = this.txtPassWord.Text.Trim();
                bool IsExpiry = this.cbIsExpiry.Checked;
                DateTime ExpiryTime = this.dtpExpiryTime.Value;

                if (this.AccountIndex > -1)
                {
                    Socket_Cache.ProxyAccount.UpdateProxyAccount_ByAccountIndex(this.AccountIndex, IsEnable, UserName, Password, IsExpiry, ExpiryTime);                    
                }
                else
                {
                    if (Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(UserName))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
                        return;
                    }

                    Socket_Cache.ProxyAccount.AddProxyAccount(Guid.NewGuid(), IsEnable, UserName, Password, string.Empty, IsExpiry, ExpiryTime, DateTime.Now);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private bool CheckAccountInfo()
        {
            bool bReturn = true;

            try
            {
                string UserName = this.txtUserName.Text.Trim();
                if (string.IsNullOrEmpty(UserName))
                { 
                    return false;
                }

                string Password = this.txtPassWord.Text.Trim();
                if (string.IsNullOrEmpty(Password))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//取消

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region//启用

        private void cbIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.IsEnable_CheckedChanged();
        }

        private void IsEnable_CheckedChanged()
        { 
            this.tlpAccountInfo.Enabled = this.cbIsEnable.Checked;
        }

        #endregion        

        #region//到期时间

        private void cbExpiryTime_CheckedChanged(object sender, EventArgs e)
        {
            this.ExpiryTime_CheckedChanged();
        }

        private void ExpiryTime_CheckedChanged()
        {
            this.dtpExpiryTime.Enabled = this.cbIsExpiry.Checked;
        }

        #endregion
    }
}

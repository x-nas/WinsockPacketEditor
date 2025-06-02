using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountForm : Form
    {
        private Guid SelectAID;

        #region//初始化

        public Proxy_AccountForm(Guid AID)
        {
            InitializeComponent();

            if (AID != null)
            { 
                this.SelectAID = AID;
            }

            this.InitForm();
        }        

        private void InitForm()
        {
            try
            {
                if (this.SelectAID == null || this.SelectAID == Guid.Empty)
                {
                    this.dtpExpiryTime.Value = DateTime.Now;
                    this.txtUserName.Enabled = true;
                }
                else
                {
                    Proxy_AccountInfo pai = Socket_Cache.ProxyAccount.GetProxyAccount_ByAccountID(this.SelectAID);

                    if (pai != null)
                    {
                        this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_136), pai.CreateTime.ToString("yyyy-MM-dd HH:mm"));
                        this.cbIsEnable.Checked = pai.IsEnable;
                        this.txtUserName.Text = pai.UserName;
                        this.txtPassWord.Text = Socket_Operation.PassWord_Decrypt(pai.PassWord);
                        this.cbIsLimitLinks.Checked = pai.IsLimitLinks;

                        if (pai.LimitLinks > 0)
                        {
                            this.nudLimitLinks.Value = pai.LimitLinks;                            
                        }

                        this.cbIsExpiry.Checked = pai.IsExpiry;

                        if (pai.ExpiryTime > this.dtpExpiryTime.MaxDate)
                        {
                            pai.ExpiryTime = this.dtpExpiryTime.MaxDate;
                        }

                        this.dtpExpiryTime.Value = pai.ExpiryTime;
                        this.txtUserName.Enabled = false;                 
                    }                    
                }

                this.LimitLinks_CheckedChanged();
                this.ExpiryTime_CheckedChanged();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//限制链接数

        private void cbIsLimitLinks_CheckedChanged(object sender, EventArgs e)
        {
            this.LimitLinks_CheckedChanged();
        }

        private void LimitLinks_CheckedChanged()
        {
            this.nudLimitLinks.Enabled = this.cbIsLimitLinks.Checked;
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
                string PassWord = this.txtPassWord.Text.Trim();
                PassWord = Socket_Operation.PassWord_Encrypt(PassWord);
                bool IsLimitLinks = this.cbIsLimitLinks.Checked;
                int LimitLinks = ((int)this.nudLimitLinks.Value);
                bool IsExpiry = this.cbIsExpiry.Checked;
                DateTime LoginTime = DateTime.MinValue;

                DateTime ExpiryTime;
                if (IsExpiry)
                {
                    ExpiryTime = this.dtpExpiryTime.Value;
                }
                else
                {
                    ExpiryTime = this.dtpExpiryTime.MaxDate;
                }

                if (this.SelectAID == null || this.SelectAID == Guid.Empty)
                {
                    if (Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(UserName))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
                        return;
                    }

                    Socket_Cache.ProxyAccount.AddProxyAccount(
                        Guid.NewGuid(), 
                        IsEnable, 
                        UserName, 
                        PassWord, 
                        LoginTime, 
                        string.Empty, 
                        string.Empty, 
                        IsLimitLinks,
                        LimitLinks,
                        IsExpiry, 
                        ExpiryTime, 
                        DateTime.Now);
                }
                else
                {
                    Socket_Cache.ProxyAccount.UpdateProxyAccount_ByAccountID(
                        this.SelectAID, 
                        IsEnable, 
                        PassWord, 
                        IsLimitLinks,
                        LimitLinks,
                        IsExpiry, 
                        ExpiryTime);
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
    }
}

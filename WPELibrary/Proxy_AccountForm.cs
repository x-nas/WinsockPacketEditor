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
                        this.Text += " - " + pai.AID.ToString().ToUpper();

                        bool IsEnable = pai.IsEnable;
                        string UserName = pai.UserName;
                        string PassWord = Socket_Operation.PassWord_Decrypt(pai.PassWord);
                        bool IsExpiry = pai.IsExpiry;
                        DateTime ExpiryTime = pai.ExpiryTime;

                        this.cbIsEnable.Checked = IsEnable;
                        this.txtUserName.Text = UserName;
                        this.txtPassWord.Text = PassWord;
                        this.cbIsExpiry.Checked = IsExpiry;
                        this.dtpExpiryTime.Value = ExpiryTime;

                        this.txtUserName.Enabled = false;
                    }                    
                }

                this.ExpiryTime_CheckedChanged();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                string PassWord = this.txtPassWord.Text.Trim();
                PassWord = Socket_Operation.PassWord_Encrypt(PassWord);
                bool IsExpiry = this.cbIsExpiry.Checked;
                DateTime ExpiryTime = this.dtpExpiryTime.Value;

                if (this.SelectAID == null || this.SelectAID == Guid.Empty)
                {
                    if (Socket_Cache.ProxyAccount.CheckProxyAccount_Exist(UserName))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
                        return;
                    }                    

                    Socket_Cache.ProxyAccount.AddProxyAccount(Guid.NewGuid(), IsEnable, UserName, PassWord, string.Empty, IsExpiry, ExpiryTime, DateTime.Now);
                }
                else
                {
                    Socket_Cache.ProxyAccount.UpdateProxyAccount_ByAccountID(this.SelectAID, IsEnable, PassWord, IsExpiry, ExpiryTime);                    
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

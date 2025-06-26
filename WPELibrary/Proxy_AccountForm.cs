using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

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
                    this.cbIsLimitDevices.Checked = true;
                    this.dtpExpiryTime.Value = DateTime.Now;
                    this.txtUserName.Enabled = true;
                }
                else
                {
                    Proxy_AccountInfo pai = Operate.ProxyConfig.ProxyAccount.GetProxyAccount_ByAccountID(this.SelectAID);

                    if (pai != null)
                    {
                        this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_136), pai.CreateTime.ToString("yyyy-MM-dd HH:mm"));
                        this.cbIsEnable.Checked = pai.IsEnable;
                        this.txtUserName.Text = pai.UserName;
                        this.txtPassWord.Text = Socket_Operation.PassWord_Decrypt(pai.PassWord);
                        this.cbIsLimitLinks.Checked = pai.IsLimitLinks;
                        this.cbIsLimitDevices.Checked = pai.IsLimitDevices;

                        if (pai.LimitLinks > 0)
                        {
                            this.nudLimitLinks.Value = pai.LimitLinks;                            
                        }

                        if (pai.LimitDevices > 0)
                        {
                            this.nudLimitDevices.Value = pai.LimitDevices;
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

                this.LimitLinks_Changed();
                this.LimitDevices_Changed();
                this.ExpiryTime_Changed();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//限制链接数

        private void cbIsLimitLinks_CheckedChanged(object sender, EventArgs e)
        {
            this.LimitLinks_Changed();
        }

        private void LimitLinks_Changed()
        {
            this.nudLimitLinks.Enabled = this.cbIsLimitLinks.Checked;
        }

        #endregion

        #region//限制设备数

        private void cbIsLimitDevices_CheckedChanged(object sender, EventArgs e)
        {
            this.LimitDevices_Changed();
        }

        private void LimitDevices_Changed()
        {
            this.nudLimitDevices.Enabled = this.cbIsLimitDevices.Checked;
        }

        #endregion

        #region//到期时间

        private void cbExpiryTime_CheckedChanged(object sender, EventArgs e)
        {
            this.ExpiryTime_Changed();
        }

        private void ExpiryTime_Changed()
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
                bool IsLimitDevices = this.cbIsLimitDevices.Checked;
                int LimitDevices = ((int)this.nudLimitDevices.Value);
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
                    if (Operate.ProxyConfig.ProxyAccount.CheckProxyAccount_Exist(UserName))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_177));
                        return;
                    }

                    Operate.ProxyConfig.ProxyAccount.AddProxyAccount(
                        Guid.NewGuid(), 
                        IsEnable, 
                        UserName, 
                        PassWord, 
                        LoginTime, 
                        string.Empty, 
                        string.Empty, 
                        IsLimitLinks,
                        LimitLinks,
                        IsLimitDevices,
                        LimitDevices,
                        IsExpiry, 
                        ExpiryTime, 
                        DateTime.Now);
                }
                else
                {
                    Operate.ProxyConfig.ProxyAccount.UpdateProxyAccount_ByAccountID(
                        this.SelectAID, 
                        IsEnable, 
                        PassWord, 
                        IsLimitLinks,
                        LimitLinks,
                        IsLimitDevices,
                        LimitDevices,
                        IsExpiry, 
                        ExpiryTime);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

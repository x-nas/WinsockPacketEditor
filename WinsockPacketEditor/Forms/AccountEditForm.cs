using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;

namespace WinsockPacketEditor
{
    public partial class AccountEditForm : Form
    {
        private AccountInfo aiSelect;
        private Form form;

        #region//窗体事件

        public AccountEditForm(Form _form, AccountInfo ai)
        {
            InitializeComponent();
            this.aiSelect = ai;
            this.form = _form;
        }

        private void AccountEditForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("AccountEditForm", "账号编辑");

            if (this.aiSelect == null)
            {
                this.cbIsEnable.Checked = true;
                this.txtUserName.Enabled = true;
                this.dtpExpiryTime.Value = DateTime.Now;                
            }
            else
            {
                this.cbIsEnable.Checked = aiSelect.IsEnable;                
                this.txtUserName.Text = aiSelect.UserName;
                this.txtUserName.Enabled = false;
                this.txtPassword.Text = Operate.SystemConfig.PassWord_Decrypt(aiSelect.Password);
                this.cbIsLimitLinks.Checked = aiSelect.IsLimitLinks;
                this.cbIsLimitDevices.Checked = aiSelect.IsLimitDevices;

                if (aiSelect.LimitLinks > 0)
                {
                    this.nudLimitLinks.Value = aiSelect.LimitLinks;
                }

                if (aiSelect.LimitDevices > 0)
                {
                    this.nudLimitDevices.Value = aiSelect.LimitDevices;
                }

                this.cbIsExpiry.Checked = aiSelect.IsExpiry;

                if (this.aiSelect.ExpiryTime > this.dtpExpiryTime.MaxDate)
                {
                    this.dtpExpiryTime.Value = this.dtpExpiryTime.MaxDate;
                }
                else
                {
                    this.dtpExpiryTime.Value = aiSelect.ExpiryTime;
                }                                    
            }

            this.IsEnable_Changed();
            this.IsLimitLinks_Changed();
            this.IsLimitDevices_Changed();
            this.IsExpiry_Changed();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (this.cbIsEnable.Checked)
            {
                if (string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
                {
                    this.txtUserName.Status = TType.Error;
                }
                else
                {
                    this.txtUserName.Status = TType.Success;
                }
            }            
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (this.cbIsEnable.Checked)
            {
                if (string.IsNullOrEmpty(this.txtPassword.Text.Trim()))
                {
                    this.txtPassword.Status = TType.Error;
                }
                else
                {
                    this.txtPassword.Status = TType.Success;
                }
            }            
        }

        #endregion

        #region//启用

        private void cbIsEnable_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.IsEnable_Changed();
        }

        private void IsEnable_Changed()
        {
            this.tlpAccountInfo.Enabled = this.cbIsEnable.Checked;
        }


        #endregion

        #region//限制链接数

        private void cbIsLimitLinks_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.IsLimitLinks_Changed();
        }

        private void IsLimitLinks_Changed()
        {
            this.nudLimitLinks.Enabled = this.cbIsLimitLinks.Checked;
        }

        #endregion

        #region//限制设备数

        private void cbIsLimitDevices_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.IsLimitDevices_Changed();
        }

        private void IsLimitDevices_Changed()
        {
            this.nudLimitDevices.Enabled = this.cbIsLimitDevices.Checked;
        }

        #endregion

        #region//过期时间

        private void cbIsExpiry_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.IsExpiry_Changed();
        }

        private void IsExpiry_Changed()
        {
            this.dtpExpiryTime.Enabled = this.cbIsExpiry.Checked;
        }

        #endregion        

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
                {
                    this.txtUserName.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "用户名为空", TType.Error)
                    {
                        LocalizationText = "AccountEditForm.UserName.Empty"
                    });

                    return;
                }

                if (string.IsNullOrEmpty(this.txtPassword.Text.Trim()))
                {
                    this.txtPassword.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "密码为空", TType.Error)
                    {
                        LocalizationText = "AccountEditForm.Password.Empty"
                    });

                    return;
                }

                bool IsEnable = this.cbIsEnable.Checked;
                string UserName = this.txtUserName.Text.Trim();
                string PassWord = this.txtPassword.Text.Trim();
                PassWord = Operate.SystemConfig.PassWord_Encrypt(PassWord);
                bool IsLimitLinks = this.cbIsLimitLinks.Checked;
                int LimitLinks = ((int)this.nudLimitLinks.Value);
                bool IsLimitDevices = this.cbIsLimitDevices.Checked;
                int LimitDevices = ((int)this.nudLimitDevices.Value);
                bool IsExpiry = this.cbIsExpiry.Checked;
                DateTime LoginTime = DateTime.MinValue;

                DateTime ExpiryTime;
                if (IsExpiry)
                {
                    ExpiryTime = this.dtpExpiryTime.Value.Value;
                }
                else
                {
                    ExpiryTime = this.dtpExpiryTime.MaxDate.Value;
                }

                if (this.aiSelect == null)
                {
                    if (Operate.ProxyConfig.Account.CheckProxyAccount_Exist(UserName))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "用户名已存在", TType.Error)
                        {
                            LocalizationText = "AccountEditForm.UserName.Error"
                        });

                        return;
                    }

                    Operate.ProxyConfig.Account.AddProxyAccount(
                        Guid.NewGuid(),
                        IsEnable,
                        UserName,
                        PassWord,
                        new BindingList<AccountIPInfo>(),
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
                    Operate.ProxyConfig.Account.UpdateProxyAccount_ByAccountID(
                        this.aiSelect.AID,
                        IsEnable,
                        PassWord,
                        IsLimitLinks,
                        LimitLinks,
                        IsLimitDevices,
                        LimitDevices,
                        IsExpiry,
                        ExpiryTime);
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "账号信息保存成功", TType.Success)
                {
                    LocalizationText = "AccountEditForm.Success"
                });

                if (this.form is InterfaceInfo.IProxyMode proxyForm)
                {
                    Operate.ProxyConfig.Account.NeedSave = true;
                    proxyForm.RefreshAccountList();
                }

                this.Dispose();
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

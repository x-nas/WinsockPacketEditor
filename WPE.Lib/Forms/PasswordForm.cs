using System;
using System.Reflection;
using System.Windows.Forms;

namespace WPE.Lib.Forms
{
    public partial class PasswordForm : Form
    {
        private string Password = string.Empty;
        private readonly Operate.SystemConfig.PWType PWType;

        #region//初始化

        public PasswordForm(Operate.SystemConfig.PWType PWType)
        {
            InitializeComponent();

            this.PWType = PWType;
            InitForm();
        }

        private void InitForm()
        {
            try
            {
                string sTitle = string.Empty;
                string sShow = string.Empty;

                switch (PWType)
                {
                    case Operate.SystemConfig.PWType.FilterList_Import:
                    case Operate.SystemConfig.PWType.RobotList_Import:
                    case Operate.SystemConfig.PWType.SendList_Import:
                    case Operate.SystemConfig.PWType.SendCollection_Import:
                    case Operate.SystemConfig.PWType.ProxyAccount_Import:
                    case Operate.SystemConfig.PWType.SystemBackUp_Import:
                    case Operate.SystemConfig.PWType.MapLocal_Import:
                    case Operate.SystemConfig.PWType.MapRemote_Import:
                        sTitle = AntdUI.Localization.Get("ImportListFile", "导入列表文件");
                        sShow = AntdUI.Localization.Get("InputPassword", "请输入密码");
                        break;

                    case Operate.SystemConfig.PWType.FilterList_Export:
                    case Operate.SystemConfig.PWType.RobotList_Export:
                    case Operate.SystemConfig.PWType.SendList_Export:
                    case Operate.SystemConfig.PWType.SendCollection_Export:
                    case Operate.SystemConfig.PWType.ProxyAccount_Export:
                    case Operate.SystemConfig.PWType.SystemBackUp_Export:
                    case Operate.SystemConfig.PWType.MapLocal_Export:
                    case Operate.SystemConfig.PWType.MapRemote_Export:
                        sTitle = AntdUI.Localization.Get("ExportListFile", "导出列表文件");
                        sShow = AntdUI.Localization.Get("PasswordEncryption", "请输入密码, 此密码在导入列表文件时会要求输入验证!\r\n 如无需设置密码，请点击 [ 取消 ] 按钮!");
                        break;
                }

                this.Text = sTitle;
                this.rtbShowInfo.Text = sShow;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void PasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (PWType)
            {
                case Operate.SystemConfig.PWType.FilterList_Import:
                case Operate.SystemConfig.PWType.FilterList_Export:
                    WPE.Lib.Operate.FilterConfig.List.AESKey = Password;
                    break;

                case Operate.SystemConfig.PWType.RobotList_Import:
                case Operate.SystemConfig.PWType.RobotList_Export:
                    WPE.Lib.Operate.RobotConfig.RobotList.AESKey = Password;
                    break;

                case Operate.SystemConfig.PWType.SendList_Import:
                case Operate.SystemConfig.PWType.SendList_Export:
                    WPE.Lib.Operate.SendConfig.SendList.AESKey = Password;
                    break;

                case Operate.SystemConfig.PWType.SendCollection_Import:
                case Operate.SystemConfig.PWType.SendCollection_Export:
                    WPE.Lib.Operate.SendConfig.Send.AESKey = Password;
                    break;

                case Operate.SystemConfig.PWType.ProxyAccount_Import:
                case Operate.SystemConfig.PWType.ProxyAccount_Export:
                    WPE.Lib.Operate.ProxyConfig.ProxyAccount.AESKey = Password;
                    break;

                case Operate.SystemConfig.PWType.SystemBackUp_Import:
                case Operate.SystemConfig.PWType.SystemBackUp_Export:
                    Operate.SystemConfig.AESKey = Password;
                    break;

                case Operate.SystemConfig.PWType.MapLocal_Import:
                case Operate.SystemConfig.PWType.MapLocal_Export:
                case Operate.SystemConfig.PWType.MapRemote_Import:
                case Operate.SystemConfig.PWType.MapRemote_Export:
                    WPE.Lib.Operate.ProxyConfig.ProxyMapping.AESKey = Password;
                    break;
            }
        }

        #endregion

        #region//确定，取消

        private void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                string sPassword = this.txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(sPassword))
                {
                    this.txtPassword.Text = string.Empty;
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_89));
                }
                else
                {
                    this.Password = sPassword;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}

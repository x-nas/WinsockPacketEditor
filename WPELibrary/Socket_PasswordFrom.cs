using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Socket_PasswordFrom : Form
    {
        private string SocketList_Password = string.Empty;
        private readonly Socket_Cache.PWType PWType;

        #region//初始化

        public Socket_PasswordFrom(Socket_Cache.PWType PWType)
        {
            MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
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
                    case Socket_Cache.PWType.FilterList_Import:
                    case Socket_Cache.PWType.RobotList_Import:
                    case Socket_Cache.PWType.SendList_Import:
                    case Socket_Cache.PWType.SendCollection_Import:
                        sTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_90);
                        sShow = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_91);
                        break;

                    case Socket_Cache.PWType.FilterList_Export:
                    case Socket_Cache.PWType.RobotList_Export:
                    case Socket_Cache.PWType.SendList_Export:
                    case Socket_Cache.PWType.SendCollection_Export:
                        sTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_36);
                        sShow = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_88);
                        break;
                }

                this.Text = sTitle;
                this.rtbShowInfo.Text = sShow;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Socket_PasswordFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (PWType)
            {
                case Socket_Cache.PWType.FilterList_Import:
                case Socket_Cache.PWType.FilterList_Export:
                    Socket_Cache.FilterList.AESKey = SocketList_Password;
                    break;

                case Socket_Cache.PWType.RobotList_Import:
                case Socket_Cache.PWType.RobotList_Export:
                    Socket_Cache.RobotList.AESKey = SocketList_Password;
                    break;

                case Socket_Cache.PWType.SendList_Import:
                case Socket_Cache.PWType.SendList_Export:
                    Socket_Cache.SendList.AESKey = SocketList_Password;
                    break;

                case Socket_Cache.PWType.SendCollection_Import:
                case Socket_Cache.PWType.SendCollection_Export:
                    Socket_Cache.Send.AESKey = SocketList_Password;
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
                    this.SocketList_Password = sPassword;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}

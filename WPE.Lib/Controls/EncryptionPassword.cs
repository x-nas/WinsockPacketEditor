using System;
using System.Windows.Forms;

namespace WPE.Lib.Controls
{
    public partial class EncryptionPassword : UserControl
    {
        #region//初始化

        public EncryptionPassword(Operate.SystemConfig.PWType PWType)
        {
            InitializeComponent();         
            this.InitEncryptionInfo(PWType);            
        }

        private void InitEncryptionInfo(Operate.SystemConfig.PWType PWType)
        {
            switch (PWType)
            {
                case Operate.SystemConfig.PWType.Import:
                    this.lEncryption.Text = AntdUI.Localization.Get("InputPassword", "请输入密码!");
                    break;

                case Operate.SystemConfig.PWType.Export:
                    this.lEncryption.Text = AntdUI.Localization.Get("PasswordEncryption", "请输入密码! 如无需设置密码, 直接点击 [ 取消 ] 按钮!");
                    break;
            }
        }

        private void txtEncryption_TextChanged(object sender, EventArgs e)
        {
            this.EncryptionText_Changed();
        }

        #endregion

        #region//验证密码

        public void EncryptionText_Changed()
        {
            if (string.IsNullOrEmpty(this.txtEncryption.Text.Trim()))
            {
                this.txtEncryption.Status = AntdUI.TType.Error;
            }
            else
            {
                this.txtEncryption.Status = AntdUI.TType.Success;
            }
        }

        #endregion

        #region//获取密码

        public string GetPassword()
        {
            return this.txtEncryption.Text.Trim();
        }

        #endregion
    }
}

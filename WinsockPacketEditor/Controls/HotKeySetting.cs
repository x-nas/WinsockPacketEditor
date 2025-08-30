using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class HotKeySetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public HotKeySetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void HotKeySetting_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("HotKeyForm", "快捷键设置");

            this.txtHotKey1.BackColor = null;
            this.txtHotKey1.ForeColor = null;
            this.txtHotKey2.BackColor = null;
            this.txtHotKey2.ForeColor = null;
            this.txtHotKey3.BackColor = null;
            this.txtHotKey3.ForeColor = null;
            this.txtHotKey4.BackColor = null;
            this.txtHotKey4.ForeColor = null;
            this.txtHotKey5.BackColor = null;
            this.txtHotKey5.ForeColor = null;
            this.txtHotKey6.BackColor = null;
            this.txtHotKey6.ForeColor = null;
            this.txtHotKey7.BackColor = null;
            this.txtHotKey7.ForeColor = null;
            this.txtHotKey8.BackColor = null;
            this.txtHotKey8.ForeColor = null;
            this.txtHotKey9.BackColor = null;
            this.txtHotKey9.ForeColor = null;
            this.txtHotKey10.BackColor = null;
            this.txtHotKey10.ForeColor = null;
            this.txtHotKey11.BackColor = null;
            this.txtHotKey11.ForeColor = null;
            this.txtHotKey12.BackColor = null;
            this.txtHotKey12.ForeColor = null;

            this.txtHotKey1.Text = Operate.SystemConfig.HotKey1;
            this.txtHotKey2.Text = Operate.SystemConfig.HotKey2;
            this.txtHotKey3.Text = Operate.SystemConfig.HotKey3;
            this.txtHotKey4.Text = Operate.SystemConfig.HotKey4;
            this.txtHotKey5.Text = Operate.SystemConfig.HotKey5;
            this.txtHotKey6.Text = Operate.SystemConfig.HotKey6;
            this.txtHotKey7.Text = Operate.SystemConfig.HotKey7;
            this.txtHotKey8.Text = Operate.SystemConfig.HotKey8;
            this.txtHotKey9.Text = Operate.SystemConfig.HotKey9;
            this.txtHotKey10.Text = Operate.SystemConfig.HotKey10;
            this.txtHotKey11.Text = Operate.SystemConfig.HotKey11;
            this.txtHotKey12.Text = Operate.SystemConfig.HotKey12;

            this.bExit.Select();
        }

        #endregion        

        #region//快捷键设置

        private void HotKeySuccess()
        {
            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "快捷键设置成功", TType.Success)
            {
                LocalizationText = "HotKeyForm.Success"
            });
        }

        private void HotKeyError()
        {
            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "快捷键设置失败", TType.Error)
            {
                LocalizationText = "HotKeyForm.Error"
            });
        }

        private void bHotKey1_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9001, this.txtHotKey1.Text.Trim()))
            {
                this.txtHotKey1.Status = TType.Success;
                Operate.SystemConfig.HotKey1 = this.txtHotKey1.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey1.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey2_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9002, this.txtHotKey2.Text.Trim()))
            {
                this.txtHotKey2.Status = TType.Success;
                Operate.SystemConfig.HotKey2 = this.txtHotKey2.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey2.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey3_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9003, this.txtHotKey3.Text.Trim()))
            {
                this.txtHotKey3.Status = TType.Success;
                Operate.SystemConfig.HotKey3 = this.txtHotKey3.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey3.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey4_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9004, this.txtHotKey4.Text.Trim()))
            {
                this.txtHotKey4.Status = TType.Success;
                Operate.SystemConfig.HotKey4 = this.txtHotKey4.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey4.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey5_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9005, this.txtHotKey5.Text.Trim()))
            {
                this.txtHotKey5.Status = TType.Success;
                Operate.SystemConfig.HotKey5 = this.txtHotKey5.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey5.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey6_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9006, this.txtHotKey6.Text.Trim()))
            {
                this.txtHotKey6.Status = TType.Success;
                Operate.SystemConfig.HotKey6 = this.txtHotKey6.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey6.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey7_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9007, this.txtHotKey7.Text.Trim()))
            {
                this.txtHotKey7.Status = TType.Success;
                Operate.SystemConfig.HotKey7 = this.txtHotKey7.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey7.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey8_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9008, this.txtHotKey8.Text.Trim()))
            {
                this.txtHotKey8.Status = TType.Success;
                Operate.SystemConfig.HotKey8 = this.txtHotKey8.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey8.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey9_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9009, this.txtHotKey9.Text.Trim()))
            {
                this.txtHotKey9.Status = TType.Success;
                Operate.SystemConfig.HotKey9 = this.txtHotKey9.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey9.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey10_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9010, this.txtHotKey10.Text.Trim()))
            {
                this.txtHotKey10.Status = TType.Success;
                Operate.SystemConfig.HotKey10 = this.txtHotKey10.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey10.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey11_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9011, this.txtHotKey11.Text.Trim()))
            {
                this.txtHotKey11.Status = TType.Success;
                Operate.SystemConfig.HotKey11 = this.txtHotKey11.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey11.Status = TType.Error;
                this.HotKeyError();
            }
        }

        private void bHotKey12_Click(object sender, EventArgs e)
        {
            if (Operate.SystemConfig.RegisterHotkey_FromText(9012, this.txtHotKey12.Text.Trim()))
            {
                this.txtHotKey12.Status = TType.Success;
                Operate.SystemConfig.HotKey12 = this.txtHotKey12.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey12.Status = TType.Error;
                this.HotKeyError();
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

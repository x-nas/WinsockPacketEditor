using AntdUI;
using System;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class HotKeyForm : Form
    {
        #region//窗体事件

        public HotKeyForm()
        {
            InitializeComponent();
        }

        private void HotKeyForm_Load(object sender, EventArgs e)
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("HotKeyForm", "快捷键设置");

            this.txtHotKey1.Text = Operate.PacketConfig.Packet.HotKey1;
            this.txtHotKey2.Text = Operate.PacketConfig.Packet.HotKey2;
            this.txtHotKey3.Text = Operate.PacketConfig.Packet.HotKey3;
            this.txtHotKey4.Text = Operate.PacketConfig.Packet.HotKey4;
            this.txtHotKey5.Text = Operate.PacketConfig.Packet.HotKey5;
            this.txtHotKey6.Text = Operate.PacketConfig.Packet.HotKey6;
            this.txtHotKey7.Text = Operate.PacketConfig.Packet.HotKey7;
            this.txtHotKey8.Text = Operate.PacketConfig.Packet.HotKey8;
            this.txtHotKey9.Text = Operate.PacketConfig.Packet.HotKey9;
            this.txtHotKey10.Text = Operate.PacketConfig.Packet.HotKey10;
            this.txtHotKey11.Text = Operate.PacketConfig.Packet.HotKey11;
            this.txtHotKey12.Text = Operate.PacketConfig.Packet.HotKey12;

            this.bExit.Select();
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region//快捷键设置

        private void HotKeySuccess()
        {
            AntdUI.Message.open(new AntdUI.Message.Config(this, "快捷键设置成功", TType.Success)
            {
                LocalizationText = "HotKeyForm.Success"
            });
        }

        private void HotKeyError()
        {
            AntdUI.Message.open(new AntdUI.Message.Config(this, "快捷键设置失败", TType.Error)
            {
                LocalizationText = "HotKeyForm.Error"
            });
        }

        private void bHotKey1_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey1.RegisterHotkeyFromText(9001))
            {
                this.txtHotKey1.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey1 = this.txtHotKey1.Text.Trim();
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
            if (this.txtHotKey2.RegisterHotkeyFromText(9002))
            {
                this.txtHotKey2.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey2 = this.txtHotKey2.Text.Trim();
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
            if (this.txtHotKey3.RegisterHotkeyFromText(9003))
            {
                this.txtHotKey3.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey3 = this.txtHotKey3.Text.Trim();
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
            if (this.txtHotKey4.RegisterHotkeyFromText(9004))
            {
                this.txtHotKey4.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey4 = this.txtHotKey4.Text.Trim();
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
            if (this.txtHotKey5.RegisterHotkeyFromText(9005))
            {
                this.txtHotKey5.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey5 = this.txtHotKey5.Text.Trim();
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
            if (this.txtHotKey6.RegisterHotkeyFromText(9006))
            {
                this.txtHotKey6.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey6 = this.txtHotKey6.Text.Trim();
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
            if (this.txtHotKey7.RegisterHotkeyFromText(9007))
            {
                this.txtHotKey7.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey7 = this.txtHotKey7.Text.Trim();
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
            if (this.txtHotKey8.RegisterHotkeyFromText(9008))
            {
                this.txtHotKey8.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey8 = this.txtHotKey8.Text.Trim();
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
            if (this.txtHotKey9.RegisterHotkeyFromText(9009))
            {
                this.txtHotKey9.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey9 = this.txtHotKey9.Text.Trim();
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
            if (this.txtHotKey10.RegisterHotkeyFromText(9010))
            {
                this.txtHotKey10.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey1 = this.txtHotKey10.Text.Trim();
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
            if (this.txtHotKey11.RegisterHotkeyFromText(9011))
            {
                this.txtHotKey11.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey1 = this.txtHotKey11.Text.Trim();
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
            if (this.txtHotKey12.RegisterHotkeyFromText(9012))
            {
                this.txtHotKey12.Status = TType.Success;
                Operate.PacketConfig.Packet.HotKey12 = this.txtHotKey12.Text.Trim();
                this.HotKeySuccess();
            }
            else
            {
                this.txtHotKey12.Status = TType.Error;
                this.HotKeyError();
            }
        }

        #endregion
    }
}

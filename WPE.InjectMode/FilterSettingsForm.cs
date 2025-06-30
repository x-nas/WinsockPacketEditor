using System;
using System.Windows.Forms;
using AntdUI;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class FilterSettingsForm : Form
    {
        #region//窗体事件

        public FilterSettingsForm()
        {
            InitializeComponent();
        }

        private void FilterSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("FilterSettingsForm", "过滤设置");

            this.cbCheckSocket.Checked = Operate.PacketConfig.Packet.CheckSocket;
            this.cbCheckIP.Checked = Operate.PacketConfig.Packet.CheckIP;
            this.cbCheckPort.Checked = Operate.PacketConfig.Packet.CheckPort;
            this.cbCheckHead.Checked = Operate.PacketConfig.Packet.CheckHead;
            this.cbCheckData.Checked = Operate.PacketConfig.Packet.CheckData;
            this.cbCheckLen.Checked = Operate.PacketConfig.Packet.CheckLen;
            this.txtCheckSocket.Text = Operate.PacketConfig.Packet.CheckSocket_Value;
            this.txtCheckLen.Text = Operate.PacketConfig.Packet.CheckLength_Value;
            this.txtCheckIP.Text = Operate.PacketConfig.Packet.CheckIP_Value;
            this.txtCheckPort.Text = Operate.PacketConfig.Packet.CheckPort_Value;
            this.txtCheckHead.Text = Operate.PacketConfig.Packet.CheckHead_Value;
            this.txtCheckData.Text = Operate.PacketConfig.Packet.CheckData_Value;
        }

        #endregion        

        #region//套接字

        private void cbCheckSocket_CheckedChanged(object sender, BoolEventArgs e)
        {            
            this.CheckSocket_Changed();
        }

        private void txtCheckSocket_TextChanged(object sender, EventArgs e)
        {
            this.CheckSocket_Changed();
        }

        private void CheckSocket_Changed()
        {
            if (this.cbCheckSocket.Checked)
            {
                if (string.IsNullOrEmpty(this.txtCheckSocket.Text.Trim()))
                {
                    this.txtCheckSocket.Status = TType.Error;
                }
                else
                {
                    this.txtCheckSocket.Status = TType.Success;
                }
            }
            else
            {
                this.txtCheckSocket.Status = TType.None;
            }
        }

        #endregion

        #region//长度

        private void cbCheckLen_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckLen_Changed();
        }

        private void txtCheckLen_TextChanged(object sender, EventArgs e)
        {
            this.CheckLen_Changed();
        }

        private void CheckLen_Changed()
        {
            if (this.cbCheckLen.Checked)
            {
                if (string.IsNullOrEmpty(this.txtCheckLen.Text.Trim()))
                {
                    this.txtCheckLen.Status = TType.Error;
                }
                else
                {
                    this.txtCheckLen.Status = TType.Success;
                }
            }
            else
            {
                this.txtCheckLen.Status = TType.None;
            }
        }

        #endregion

        #region//IP地址

        private void cbCheckIP_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckIP_Changed();
        }        

        private void txtCheckIP_TextChanged(object sender, EventArgs e)
        {
            this.CheckIP_Changed();
        }

        private void CheckIP_Changed()
        {
            if (this.cbCheckIP.Checked)
            {
                if (string.IsNullOrEmpty(this.txtCheckIP.Text.Trim()))
                {
                    this.txtCheckIP.Status = TType.Error;
                }
                else
                {
                    this.txtCheckIP.Status = TType.Success;
                }
            }
            else
            {
                this.txtCheckIP.Status = TType.None;
            }
        }

        #endregion

        #region//端口号

        private void cbCheckPort_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckPort_Changed();
        }

        private void txtCheckPort_TextChanged(object sender, EventArgs e)
        {
            this.CheckPort_Changed();
        }

        private void CheckPort_Changed()
        {
            if (this.cbCheckPort.Checked)
            {
                if (string.IsNullOrEmpty(this.txtCheckPort.Text.Trim()))
                {
                    this.txtCheckPort.Status = TType.Error;
                }
                else
                {
                    this.txtCheckPort.Status = TType.Success;
                }
            }
            else
            {
                this.txtCheckPort.Status = TType.None;
            }
        }

        #endregion

        #region//指定包头

        private void cbCheckHead_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckHead_Changed();
        }

        private void txtCheckHead_TextChanged(object sender, EventArgs e)
        {
            this.CheckHead_Changed();
        }

        private void CheckHead_Changed()
        {
            if (this.cbCheckHead.Checked)
            {
                if (string.IsNullOrEmpty(this.txtCheckHead.Text.Trim()))
                {
                    this.txtCheckHead.Status = TType.Error;
                }
                else
                {
                    this.txtCheckHead.Status = TType.Success;
                }
            }
            else
            {
                this.txtCheckHead.Status = TType.None;
            }
        }

        #endregion

        #region//指定内容

        private void cbCheckData_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckData_Changed();
        }

        private void txtCheckData_TextChanged(object sender, EventArgs e)
        {
            this.CheckData_Changed();
        }

        private void CheckData_Changed()
        {
            if (this.cbCheckData.Checked)
            {
                if (string.IsNullOrEmpty(this.txtCheckData.Text.Trim()))
                {
                    this.txtCheckData.Status = TType.Error;
                }
                else
                {
                    this.txtCheckData.Status = TType.Success;
                }
            }
            else
            {
                this.txtCheckData.Status = TType.None;
            }
        }


        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (this.cbCheckSocket.Checked && string.IsNullOrEmpty(this.txtCheckSocket.Text.Trim()) || 
                this.cbCheckLen.Checked && string.IsNullOrEmpty(this.txtCheckLen.Text.Trim()) ||
                this.cbCheckIP.Checked && string.IsNullOrEmpty(this.txtCheckIP.Text.Trim()) ||
                this.cbCheckPort.Checked && string.IsNullOrEmpty(this.txtCheckPort.Text.Trim()) ||
                this.cbCheckHead.Checked && string.IsNullOrEmpty(this.txtCheckHead.Text.Trim()) ||
                this.cbCheckData.Checked && string.IsNullOrEmpty(this.txtCheckData.Text.Trim()))
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "过滤设置为空", TType.Error)
                {
                    LocalizationText = "FilterSettingsForm.FilterEmpty"
                });

                return;
            }

            Operate.PacketConfig.Packet.CheckNotShow = !sIsShow.Checked;
            Operate.PacketConfig.Packet.CheckSocket = cbCheckSocket.Checked;
            Operate.PacketConfig.Packet.CheckIP = cbCheckIP.Checked;
            Operate.PacketConfig.Packet.CheckPort = cbCheckPort.Checked;
            Operate.PacketConfig.Packet.CheckHead = cbCheckHead.Checked;
            Operate.PacketConfig.Packet.CheckData = cbCheckData.Checked;
            Operate.PacketConfig.Packet.CheckLen = cbCheckLen.Checked;
            Operate.PacketConfig.Packet.CheckSocket_Value = this.txtCheckSocket.Text.Trim();
            Operate.PacketConfig.Packet.CheckLength_Value = this.txtCheckLen.Text.Trim();
            Operate.PacketConfig.Packet.CheckIP_Value = this.txtCheckIP.Text.Trim();
            Operate.PacketConfig.Packet.CheckPort_Value = this.txtCheckPort.Text.Trim();
            Operate.PacketConfig.Packet.CheckHead_Value = this.txtCheckHead.Text.Trim();
            Operate.PacketConfig.Packet.CheckData_Value = this.txtCheckData.Text.Trim();

            AntdUI.Message.open(new AntdUI.Message.Config(this, "过滤设置保存成功", TType.Success)
            {
                LocalizationText = "FilterSettingsForm.Success"
            });
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

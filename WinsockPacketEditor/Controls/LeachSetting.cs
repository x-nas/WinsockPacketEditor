using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LeachSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public LeachSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void LeachSetting_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("LeachSetting", "过滤设置");
            this.tabPacketType.TabMenuVisible = false;

            if (this.form is InterfaceInfo.IInjectMode)
            {
                this.tabPacketType.SelectTab("tpInject");
            }
            else if (this.form is InterfaceInfo.IProxyMode)
            {
                this.tabPacketType.SelectTab("tpProxy");
            }

            this.sIsShow.Checked = !Operate.SystemConfig.CheckNotShow;
            this.cbCheckSocket.Checked = Operate.SystemConfig.CheckSocket;
            this.cbCheckIP.Checked = Operate.SystemConfig.CheckIP;
            this.cbCheckPort.Checked = Operate.SystemConfig.CheckPort;
            this.cbCheckHead.Checked = Operate.SystemConfig.CheckHead;
            this.cbCheckData.Checked = Operate.SystemConfig.CheckData;
            this.cbCheckLen.Checked = Operate.SystemConfig.CheckLen;
            this.txtCheckSocket.Text = Operate.SystemConfig.CheckSocket_Value;
            this.txtCheckLen.Text = Operate.SystemConfig.CheckLength_Value;
            this.txtCheckIP.Text = Operate.SystemConfig.CheckIP_Value;
            this.txtCheckPort.Text = Operate.SystemConfig.CheckPort_Value;
            this.txtCheckHead.Text = Operate.SystemConfig.CheckHead_Value;
            this.txtCheckData.Text = Operate.SystemConfig.CheckData_Value;

            this.CheckSocket_Changed();
            this.CheckLen_Changed();
            this.CheckIP_Changed();
            this.CheckPort_Changed();
            this.CheckHead_Changed();
            this.CheckData_Changed();
            this.CheckType_Changed();
        }

        #endregion        

        #region//套接字

        private void cbCheckSocket_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckSocket_Changed();
        }

        private void CheckSocket_Changed()
        {
            this.txtCheckSocket.Enabled = this.cbCheckSocket.Checked;
        }

        #endregion

        #region//长度

        private void cbCheckLen_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckLen_Changed();
        }

        private void CheckLen_Changed()
        {
            this.txtCheckLen.Enabled = this.cbCheckLen.Checked;
        }

        #endregion

        #region//IP地址

        private void cbCheckIP_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckIP_Changed();
        }

        private void CheckIP_Changed()
        {
            this.txtCheckIP.Enabled = this.cbCheckIP.Checked;
        }

        #endregion

        #region//端口号

        private void cbCheckPort_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckPort_Changed();
        }

        private void CheckPort_Changed()
        {
            this.txtCheckPort.Enabled = this.cbCheckPort.Checked;
        }

        #endregion

        #region//指定包头

        private void cbCheckHead_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckHead_Changed();
        }

        private void CheckHead_Changed()
        {
            this.txtCheckHead.Enabled = this.cbCheckHead.Checked;
        }

        #endregion

        #region//指定内容

        private void cbCheckData_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckData_Changed();
        }

        private void CheckData_Changed()
        {
            this.txtCheckData.Enabled = this.cbCheckData.Checked;
        }

        #endregion

        #region//指定类别

        private void cbCheckType_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.CheckType_Changed();
        }

        private void CheckType_Changed()
        {
            this.tlpInject.Enabled = this.tlpProxy.Enabled = this.cbCheckType.Checked;
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
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "过滤设置为空", TType.Error)
                {
                    LocalizationText = "LeachSetting.Empty"
                });

                return;
            }

            Operate.SystemConfig.CheckNotShow = !sIsShow.Checked;
            Operate.SystemConfig.CheckSocket = cbCheckSocket.Checked;
            Operate.SystemConfig.CheckIP = cbCheckIP.Checked;
            Operate.SystemConfig.CheckPort = cbCheckPort.Checked;
            Operate.SystemConfig.CheckHead = cbCheckHead.Checked;
            Operate.SystemConfig.CheckData = cbCheckData.Checked;
            Operate.SystemConfig.CheckLen = cbCheckLen.Checked;
            Operate.SystemConfig.CheckSocket_Value = this.txtCheckSocket.Text.Trim();
            Operate.SystemConfig.CheckLength_Value = this.txtCheckLen.Text.Trim();
            Operate.SystemConfig.CheckIP_Value = this.txtCheckIP.Text.Trim();
            Operate.SystemConfig.CheckPort_Value = this.txtCheckPort.Text.Trim();
            Operate.SystemConfig.CheckHead_Value = this.txtCheckHead.Text.Trim();
            Operate.SystemConfig.CheckData_Value = this.txtCheckData.Text.Trim();

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "过滤设置保存成功", TType.Success)
            {
                LocalizationText = "LeachSetting.Success"
            });

            this.Dispose();
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

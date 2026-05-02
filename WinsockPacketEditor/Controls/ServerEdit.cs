using AntdUI;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ServerEdit : UserControl
    {
        private ServerInfo siSelect;
        private Form form;

        #region//窗体事件

        public ServerEdit(Form form, ServerInfo si)
        {
            this.form = form;
            this.siSelect = si;
            InitializeComponent();
        }

        private void ServerEdit_Load(object sender, System.EventArgs e)
        {
            if (this.siSelect == null)
            {
                this.cbIsEnable.Checked = true;
            }
            else
            {
                this.cbIsEnable.Checked = this.siSelect.IsEnable;
                this.txtServerName.Text = this.siSelect.ServerName;
                this.txtServerIP.Text = this.siSelect.ServerIP;
                this.nudServerPort.Value = this.siSelect.ServerPort;
                this.txtForgotURL.Text = this.siSelect.ForgotURL;
                this.txtRegisterURL.Text = this.siSelect.RegisterURL;
            }

            this.IsEnable_Changed();
        }

        private void txtServerName_TextChanged(object sender, EventArgs e)
        {
            if (this.cbIsEnable.Checked)
            {
                if (string.IsNullOrEmpty(this.txtServerName.Text.Trim()))
                {
                    this.txtServerName.Status = TType.Error;
                }
                else
                {
                    this.txtServerName.Status = TType.Success;
                }
            }
        }

        private void txtServerIP_TextChanged(object sender, EventArgs e)
        {
            if (this.cbIsEnable.Checked)
            {
                if (string.IsNullOrEmpty(this.txtServerIP.Text.Trim()))
                {
                    this.txtServerIP.Status = TType.Error;
                }
                else
                {
                    this.txtServerIP.Status = TType.Success;
                }
            }
        }

        #endregion

        #region//启用

        private void cbIsEnable_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.IsEnable_Changed();
        }

        private void IsEnable_Changed()
        {
            this.tlpServerInfo.Enabled = this.cbIsEnable.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtServerName.Text.Trim()))
                {
                    this.txtServerName.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "服务器名称为空", TType.Error)
                    {
                        LocalizationText = "ServerEditForm.ServerName.Empty"
                    });

                    return;
                }

                if (string.IsNullOrEmpty(this.txtServerIP.Text.Trim()))
                {
                    this.txtServerIP.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "服务器IP为空", TType.Error)
                    {
                        LocalizationText = "ServerEditForm.ServerIP.Empty"
                    });

                    return;
                }

                bool IsEnable = this.cbIsEnable.Checked;
                string ServerName = this.txtServerName.Text.Trim();
                string ServerIP = this.txtServerIP.Text.Trim();
                int ServerPort = (int)this.nudServerPort.Value;
                string ForgotURL = this.txtForgotURL.Text.Trim();
                string RegisterURL = this.txtRegisterURL.Text.Trim();            

                if (this.siSelect == null)
                {
                    Operate.WPCConfig.ServerList.AddServer(IsEnable, Guid.NewGuid(), ServerName, ServerIP, ServerPort, ForgotURL, RegisterURL, new BindingList<RuleInfo>());
                }
                else
                {
                    Operate.WPCConfig.ServerList.UpdateServer_ByServerID(this.siSelect.SID, IsEnable, ServerName, ServerIP, ServerPort, ForgotURL, RegisterURL);
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "服务器信息保存成功", TType.Success)
                {
                    LocalizationText = "ServerEditForm.Success"
                });

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);
            }
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        #endregion        
    }
}

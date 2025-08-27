using AntdUI;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class RemoteMGTSetting : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public RemoteMGTSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void RemoteMGTSetting_Load(object sender, EventArgs e)
        {
            this.cbIsRemote.Checked = Operate.SystemConfig.IsRemote;
            this.nudRemote_Port.Value = Operate.SystemConfig.Remote_Port;
            this.txtRemote_UserName.Text = Operate.SystemConfig.Remote_UserName;
            this.txtRemote_PassWord.Text = Operate.SystemConfig.Remote_PassWord;

            this.InitRemoteIP();
            this.IsRemote_Changed();            
        }

        private void InitRemoteIP()
        {
            try
            {
                IPAddress[] ipAddresses = Operate.SystemConfig.GetLocalIPAddress();

                this.ddlRemoteIP.Items.Clear();
                this.ddlRemoteIP.Items.AddRange(ipAddresses.Select(ip => new SelectItem(ip.ToString(), ip)).ToArray());

                if (IPAddress.TryParse(Operate.SystemConfig.Remote_IP, out IPAddress ipRemoteIP))
                {
                    this.ddlRemoteIP.SelectedValue = ipRemoteIP;
                }

                if (this.ddlRemoteIP.SelectedValue == null)
                {
                    this.ddlRemoteIP.SelectedIndex = 0;
                }

                this.SetRemoteMGT_URL();
            }
            catch(Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ddlRemoteIP_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            this.SetRemoteMGT_URL();
        }

        private void nudRemote_Port_ValueChanged(object sender, DecimalEventArgs e)
        {
            this.SetRemoteMGT_URL();
        }

        private void SetRemoteMGT_URL()
        {
            string RemoteIP = this.ddlRemoteIP.SelectedValue.ToString();
            string RemotePort = this.nudRemote_Port.Value.ToString();

            this.lRemote.Text = Operate.SystemConfig.GetRemoteMGT_URL(RemoteIP, RemotePort);
        }

        #endregion

        #region//是否启用

        private void cbIsRemote_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.IsRemote_Changed();
        }

        private void IsRemote_Changed()
        {
            this.ddlRemoteIP.Enabled =
                this.txtRemote_UserName.Enabled =
                this.txtRemote_PassWord.Enabled =
                this.nudRemote_Port.Enabled =
                this.lRemote.Visible =
                this.cbIsRemote.Checked;
        }

        #endregion        

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            string Remote_IP = this.ddlRemoteIP.SelectedValue.ToString();
            ushort Remote_Port = ((ushort)this.nudRemote_Port.Value);
            string Remote_UserName = this.txtRemote_UserName.Text.Trim();
            string Remote_PassWord = this.txtRemote_PassWord.Text.Trim();

            if (this.cbIsRemote.Checked)
            {
                if (string.IsNullOrEmpty(Remote_UserName))
                {
                    this.txtRemote_UserName.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "管理员账号为空", TType.Error)
                    {
                        LocalizationText = "WPEForm.UserName.Empty"
                    });

                    return;
                }
                else
                {
                    this.txtRemote_UserName.Status = TType.Success;
                }

                if (string.IsNullOrEmpty(Remote_PassWord))
                {
                    this.txtRemote_PassWord.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "账号密码为空", TType.Error)
                    {
                        LocalizationText = "WPEForm.PassWord.Empty"
                    });

                    return;
                }
                else
                {
                    this.txtRemote_PassWord.Status = TType.Success;
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "远程管理已启用", TType.Success)
                {
                    LocalizationText = "WPEForm.RemoteEnable"
                });
            }
            else
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "远程管理已关闭", TType.Error)
                {
                    LocalizationText = "WPEForm.RemoteDisable"
                });
            }

            Operate.SystemConfig.IsRemote = this.cbIsRemote.Checked;
            Operate.SystemConfig.Remote_IP = Remote_IP;
            Operate.SystemConfig.Remote_Port = Remote_Port;
            Operate.SystemConfig.Remote_UserName = Remote_UserName;
            Operate.SystemConfig.Remote_PassWord = Remote_PassWord;

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

using AntdUI;
using System;
using System.Linq;
using System.Net;
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
            try
            {
                this.InitRemoteIP();
                this.nudRemote_Port.Value = Operate.SystemConfig.Remote_Port;

                this.cbIsRemote.Checked = Operate.SystemConfig.IsRemote;
                this.IsRemote_Changed();
                
                this.txtRemote_UserName.Text = Operate.SystemConfig.Remote_UserName;
                this.txtRemote_PassWord.Text = Operate.SystemConfig.Remote_PassWord;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(RemoteMGTSetting_Load), ex.Message);
            }                        
        }

        private void InitRemoteIP()
        {
            try
            {
                IPAddress[] ipAddresses = Operate.SystemConfig.GetLocalIPAddress();

                this.ddlRemoteIP.Items.Clear();
                this.ddlRemoteIP.Items.AddRange(ipAddresses.Select(ip => new SelectItem(ip.ToString(), ip)).ToArray());

                if (this.ddlRemoteIP.Items.Count > 0)
                {
                    if (IPAddress.TryParse(Operate.SystemConfig.Remote_IP, out IPAddress ipRemoteIP))
                    {
                        this.ddlRemoteIP.SelectedValue = ipRemoteIP;
                    }

                    if (this.ddlRemoteIP.SelectedValue == null)
                    {
                        this.ddlRemoteIP.SelectedIndex = 0;
                    }
                }
                else
                {
                    this.ddlRemoteIP.Items.Add(new SelectItem("127.0.0.1", IPAddress.Loopback));
                    this.ddlRemoteIP.SelectedIndex = 0;
                }

                this.SetRemoteMGT_URL();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitRemoteIP), ex.Message);
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
            try
            {
                if (this.ddlRemoteIP.Items.Count > 0 && this.ddlRemoteIP.SelectedValue != null)
                {
                    string RemoteIP = this.ddlRemoteIP.SelectedValue.ToString();
                    string RemotePort = this.nudRemote_Port.Value.ToString();
                    string RemoteURL = Operate.SystemConfig.GetRemoteMGT_URL(RemoteIP, RemotePort);

                    this.lRemote.Text = "<a href='" + RemoteURL + "'>" + RemoteURL + "</a>";
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(SetRemoteMGT_URL), ex.Message);
            }                       
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

        private bool CheckRemoteMGT()
        {
            try
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
                            LocalizationText = "RemoteMGTSetting.UserName.Empty"
                        });

                        return false;
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
                            LocalizationText = "RemoteMGTSetting.PassWord.Empty"
                        });

                        return false;
                    }
                    else
                    {
                        this.txtRemote_PassWord.Status = TType.Success;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CheckRemoteMGT), ex.Message);
            }
            
            return false;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (this.CheckRemoteMGT())
            {
                Operate.SystemConfig.IsRemote = this.cbIsRemote.Checked;
                Operate.SystemConfig.Remote_IP = this.ddlRemoteIP.SelectedValue.ToString();
                Operate.SystemConfig.Remote_Port = ((ushort)this.nudRemote_Port.Value);
                Operate.SystemConfig.Remote_UserName = this.txtRemote_UserName.Text.Trim();
                Operate.SystemConfig.Remote_PassWord = this.txtRemote_PassWord.Text.Trim();

                if (Operate.SystemConfig.IsRemote)
                {
                    Operate.SystemConfig.StartRemoteMGT(this.form);
                }
                else
                {
                    Operate.SystemConfig.StopRemoteMGT(this.form);
                }

                this.Dispose();
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

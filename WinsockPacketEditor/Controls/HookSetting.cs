using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class HookSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public HookSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void HookSetting_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = AntdUI.Localization.Get("HookSettingsForm", "拦截设置");
                this.tabHookSettings.TabMenuVisible = false;

                if (this.form is InterfaceInfo.IInjectMode)
                {
                    this.tabHookSettings.SelectTab("tpInjectMode");
                }
                else if (this.form is InterfaceInfo.IProxyMode)
                {
                    this.tabHookSettings.SelectTab("tpProxyMode");
                }

                this.cbHookWS1_Send.Checked = Operate.PacketConfig.Packet.HookWS1_Send;
                this.cbHookWS1_SendTo.Checked = Operate.PacketConfig.Packet.HookWS1_SendTo;
                this.cbHookWS1_Recv.Checked = Operate.PacketConfig.Packet.HookWS1_Recv;
                this.cbHookWS1_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWS1_RecvFrom;
                this.cbHookWS2_Send.Checked = Operate.PacketConfig.Packet.HookWS2_Send;
                this.cbHookWS2_SendTo.Checked = Operate.PacketConfig.Packet.HookWS2_SendTo;
                this.cbHookWS2_Recv.Checked = Operate.PacketConfig.Packet.HookWS2_Recv;
                this.cbHookWS2_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWS2_RecvFrom;
                this.cbHookWSA_Send.Checked = Operate.PacketConfig.Packet.HookWSA_Send;
                this.cbHookWSA_SendTo.Checked = Operate.PacketConfig.Packet.HookWSA_SendTo;
                this.cbHookWSA_Recv.Checked = Operate.PacketConfig.Packet.HookWSA_Recv;
                this.cbHookWSA_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWSA_RecvFrom;
                this.cbTCP_Req.Checked = Operate.ProxyConfig.Proxy.HookTCP_Req;
                this.cbTCP_Resp.Checked = Operate.ProxyConfig.Proxy.HookTCP_Resp;
                this.cbUDP_Req.Checked = Operate.ProxyConfig.Proxy.HookUDP_Req;
                this.cbUDP_Resp.Checked = Operate.ProxyConfig.Proxy.HookUDP_Resp;
                this.cbEnable_UnPack.Checked = Operate.ProxyConfig.Proxy.Enable_UnPack;
                this.txtUnPack_Head.Text = Operate.ProxyConfig.Proxy.UnPack_Head;
                this.txtUnPack_Length.Text = Operate.ProxyConfig.Proxy.UnPack_Length;

                this.Enable_UnPack_Changed();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(HookSetting_Load), ex);
            }                        
        }

        #endregion

        #region//启用拆包

        private void cbEnable_UnPack_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.Enable_UnPack_Changed();
        }

        private void Enable_UnPack_Changed()
        {
            this.txtUnPack_Head.Enabled = this.txtUnPack_Length.Enabled = this.cbEnable_UnPack.Checked;
        }

        #endregion

        #region//保存设置完整性检测

        private bool CheckSetting()
        {
            try
            {
                if (this.cbEnable_UnPack.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtUnPack_Head.Text.Trim()))
                    { 
                        this.txtUnPack_Head.Status = TType.Error;

                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "拆包设置不正确", TType.Error)
                        {
                            LocalizationText = "HookSettingsForm.UnPack.Error"
                        });

                        return false;
                    }

                    if (string.IsNullOrEmpty(this.txtUnPack_Length.Text.Trim()))
                    {
                        this.txtUnPack_Length.Status = TType.Error;

                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "拆包设置不正确", TType.Error)
                        {
                            LocalizationText = "HookSettingsForm.UnPack.Error"
                        });

                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CheckSetting), ex);
            }

            return false;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!this.CheckSetting())
            { 
                return;
            }

            Operate.PacketConfig.Packet.HookWS1_Send = this.cbHookWS1_Send.Checked;
            Operate.PacketConfig.Packet.HookWS1_SendTo = this.cbHookWS1_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWS1_Recv = this.cbHookWS1_Recv.Checked;
            Operate.PacketConfig.Packet.HookWS1_RecvFrom = this.cbHookWS1_RecvFrom.Checked;
            Operate.PacketConfig.Packet.HookWS2_Send = this.cbHookWS2_Send.Checked;
            Operate.PacketConfig.Packet.HookWS2_SendTo = this.cbHookWS2_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWS2_Recv = this.cbHookWS2_Recv.Checked;
            Operate.PacketConfig.Packet.HookWS2_RecvFrom = this.cbHookWS2_RecvFrom.Checked;
            Operate.PacketConfig.Packet.HookWSA_Send = this.cbHookWSA_Send.Checked;
            Operate.PacketConfig.Packet.HookWSA_SendTo = this.cbHookWSA_SendTo.Checked;
            Operate.PacketConfig.Packet.HookWSA_Recv = this.cbHookWSA_Recv.Checked;
            Operate.PacketConfig.Packet.HookWSA_RecvFrom = this.cbHookWSA_RecvFrom.Checked;
            Operate.ProxyConfig.Proxy.HookTCP_Req = this.cbTCP_Req.Checked;
            Operate.ProxyConfig.Proxy.HookTCP_Resp = this.cbTCP_Resp.Checked;
            Operate.ProxyConfig.Proxy.HookUDP_Req = this.cbUDP_Req.Checked;
            Operate.ProxyConfig.Proxy.HookUDP_Resp = this.cbUDP_Resp.Checked;
            Operate.ProxyConfig.Proxy.Enable_UnPack = this.cbEnable_UnPack.Checked;
            Operate.ProxyConfig.Proxy.UnPack_Head = this.txtUnPack_Head.Text.Trim();
            Operate.ProxyConfig.Proxy.UnPack_Length = this.txtUnPack_Length.Text.Trim();

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "拦截设置保存成功", TType.Success)
            {
                LocalizationText = "HookSettingsForm.Success"
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

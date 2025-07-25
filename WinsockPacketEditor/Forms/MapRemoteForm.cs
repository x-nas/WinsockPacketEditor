using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class MapRemoteForm : Form
    {
        private MapRemote mrSelect;

        #region//窗体事件

        public MapRemoteForm(MapRemote mr)
        {
            InitializeComponent();
            this.mrSelect = mr;
        }

        private void MapRemoteForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("MapRemoteForm", "远程映射设置");

            this.ddlProtocolFrom.SelectedIndex = 0;
            this.ddlProtocolTo.SelectedIndex = 0;

            if (this.mrSelect != null)
            {
                if (this.mrSelect.ProtocolTypeFrom == Operate.ProxyConfig.Proxy.MapProtocol.Http)
                {
                    this.ddlProtocolFrom.SelectedIndex = 0;
                }

                this.txtHostFrom.Text = this.mrSelect.HostFrom;
                this.nudPortFrom.Value = this.mrSelect.PortFrom;
                this.txtPathFrom.Text = this.mrSelect.PathFrom;

                if (this.mrSelect.ProtocolTypeTo == Operate.ProxyConfig.Proxy.MapProtocol.Http)
                {
                    this.ddlProtocolTo.SelectedIndex = 0;
                }
                else if (this.mrSelect.ProtocolTypeTo == Operate.ProxyConfig.Proxy.MapProtocol.Https)
                {
                    this.ddlProtocolTo.SelectedIndex = 1;
                }

                this.txtHostTo.Text = this.mrSelect.HostTo;
                this.nudPortTo.Value = this.mrSelect.PortTo;
                this.txtPathTo.Text = this.mrSelect.PathTo;
            }

            this.ProtocolTo_Changed();
        }

        private void txtHostFrom_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHostFrom.Text.Trim()))
            {
                this.txtHostFrom.Status = TType.Error;
            }
            else
            {
                this.txtHostFrom.Status = TType.Success;
            }
        }

        private void txtHostTo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHostTo.Text.Trim()))
            {
                this.txtHostTo.Status = TType.Error;
            }
            else
            {
                this.txtHostTo.Status = TType.Success;
            }
        }

        #endregion

        #region//协议类型

        private void ddlProtocolTo_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            this.ProtocolTo_Changed();
        }

        private void ProtocolTo_Changed()
        {
            if (this.ddlProtocolTo.SelectedIndex == 0)
            {
                this.txtHostTo.PrefixText = "http://";
            }
            else if (this.ddlProtocolTo.SelectedIndex == 1)
            {
                this.txtHostTo.PrefixText = "https://";
            }
            else
            {
                this.txtHostTo.PrefixText = string.Empty;
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                string HostFrom_New = this.txtHostFrom.Text.Trim();
                if (string.IsNullOrEmpty(HostFrom_New))
                {
                    this.txtHostFrom.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "映射数据为空", TType.Error)
                    {
                        LocalizationText = "MapRemoteForm.Empty"
                    });

                    return;
                }

                string HostTo_New = this.txtHostTo.Text.Trim();
                if (string.IsNullOrEmpty(HostTo_New))
                {
                    this.txtHostTo.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "映射数据为空", TType.Error)
                    {
                        LocalizationText = "MapRemoteForm.Empty"
                    });

                    return;
                }

                Operate.ProxyConfig.Proxy.MapProtocol ProtocolFrom_New = new Operate.ProxyConfig.Proxy.MapProtocol();
                if (this.ddlProtocolFrom.SelectedIndex == 0)
                {
                    ProtocolFrom_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }
                else
                {
                    ProtocolFrom_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }

                Operate.ProxyConfig.Proxy.MapProtocol ProtocolTo_New = new Operate.ProxyConfig.Proxy.MapProtocol();
                if (this.ddlProtocolTo.SelectedIndex == 0)
                {
                    ProtocolTo_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }
                else if (this.ddlProtocolTo.SelectedIndex == 1)
                {
                    ProtocolTo_New = Operate.ProxyConfig.Proxy.MapProtocol.Https;
                }
                
                int PortFrom_New = ((int)this.nudPortFrom.Value);
                int PortTo_New = ((int)this.nudPortTo.Value);
                string PathFrom_New = this.txtPathFrom.Text.Trim();
                string PathTo_New = this.txtPathTo.Text.Trim();

                if (this.mrSelect == null)
                {
                    Operate.ProxyConfig.Mapping.AddMapRemote(
                        false,
                        ProtocolFrom_New,
                        HostFrom_New,
                        PortFrom_New,
                        PathFrom_New,
                        ProtocolTo_New,
                        HostTo_New,
                        PortTo_New,
                        PathTo_New);
                }
                else
                {
                    Operate.ProxyConfig.Mapping.UpdateMapRemote(
                        this.mrSelect,
                        ProtocolFrom_New,
                        HostFrom_New,
                        PortFrom_New,
                        PathFrom_New,
                        ProtocolTo_New,
                        HostTo_New,
                        PortTo_New,
                        PathTo_New);
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this, "远程映射保存成功", TType.Success)
                {
                    LocalizationText = "MapRemoteForm.Success"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

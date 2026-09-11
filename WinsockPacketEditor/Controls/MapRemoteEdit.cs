using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class MapRemoteEdit : UserControl
    {
        private Form form;
        private MapSetting msForm;
        private MapRemote mrSelect;

        #region//窗体事件

        public MapRemoteEdit(Form form, MapSetting msForm, MapRemote mr)
        {
            InitializeComponent();
            this.mrSelect = mr;
            this.form = form;
            this.msForm = msForm;
        }

        private void MapRemoteEdit_Load(object sender, EventArgs e)
        {
            try
            {
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

                    //⚠️ 老库里可能存着 ProtocolTo=Https（外壳早先的下拉能选，那是个装饰项，
                    //已于 2026-09-09 去掉）。这个下拉只有一项，原来那支会把 SelectedIndex
                    //指到不存在的 1。一律回到 0，改一次就归正。
                    this.ddlProtocolTo.SelectedIndex = 0;

                    this.txtHostTo.Text = this.mrSelect.HostTo;
                    this.nudPortTo.Value = this.mrSelect.PortTo;
                    this.txtPathTo.Text = this.mrSelect.PathTo;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(MapRemoteEdit_Load), ex);
            }
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

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                string HostFrom_New = this.txtHostFrom.Text.Trim();
                if (string.IsNullOrEmpty(HostFrom_New))
                {
                    this.txtHostFrom.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "映射数据为空", TType.Error)
                    {
                        LocalizationText = "MapRemoteForm.Empty"
                    });

                    return;
                }

                string HostTo_New = this.txtHostTo.Text.Trim();
                if (string.IsNullOrEmpty(HostTo_New))
                {
                    this.txtHostTo.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "映射数据为空", TType.Error)
                    {
                        LocalizationText = "MapRemoteForm.Empty"
                    });

                    return;
                }

                //两个下拉都只有 "http" 一项（见 Designer 的 Items.AddRange），
                //SelectedIndex 恒为 0，原来那两组分支走不到。
                //映射端的 https 尤其不能给：ProtocolTypeTo 在任何数据路径上都没被读过 ——
                //ConnectToTarget 开的是明文 TCP，ModifyRequestHostAndPath 拼的是明文 HTTP 请求，
                //选了 https 只会把请求明文发到一个 TLS 端口上。
                Operate.ProxyConfig.Proxy.MapProtocol ProtocolFrom_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                Operate.ProxyConfig.Proxy.MapProtocol ProtocolTo_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;

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

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "远程映射保存成功", TType.Success)
                {
                    LocalizationText = "MapRemoteForm.Success"
                });

                this.msForm.RefreshMapRemote();
                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);
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

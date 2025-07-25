using AntdUI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class MapLocalForm : Form
    {
        private MapLocal mlSelect;

        #region//窗体事件

        public MapLocalForm(MapLocal ml)
        {
            InitializeComponent();
            this.mlSelect = ml;
        }

        private void MapLocalForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("MapLocalForm", "本地映射设置");

            this.ddlProtocolType.SelectedIndex = 0;
            this.udLocalPath.Filter = "All Files (*.*)|*.*";
            this.udLocalPath.HandDragFolder = true;
            this.udLocalPath.UseAdmin();

            if (this.mlSelect != null)
            {
                if (this.mlSelect.ProtocolType == Operate.ProxyConfig.Proxy.MapProtocol.Http)
                {
                    this.ddlProtocolType.SelectedIndex = 0;
                }

                this.txtHost.Text = this.mlSelect.Host;
                this.nudPort.Value = this.mlSelect.Port;
                this.txtRemotePath.Text = this.mlSelect.RemotePath;
                this.txtLocalPath.Text = this.mlSelect.LocalPath;
            }
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHost.Text.Trim()))
            {
                this.txtHost.Status = TType.Error;
            }
            else
            {
                this.txtHost.Status = TType.Success;
            }
        }

        private void txtLocalPath_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtLocalPath.Text.Trim()))
            {
                this.txtLocalPath.Status = TType.Error;
            }
            else
            {
                this.txtLocalPath.Status = TType.Success;
            }
        }

        #endregion

        #region//上传文件

        private void udLocalPath_DragChanged(object sender, AntdUI.StringsEventArgs e)
        {
            string FilePath = e.Value[0];

            if (!string.IsNullOrEmpty(FilePath))
            {
                if (File.Exists(FilePath))
                {
                    this.txtLocalPath.Text = FilePath;
                }
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Host_New = this.txtHost.Text.Trim();
                if (string.IsNullOrEmpty(Host_New))
                {
                    this.txtHost.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "映射数据为空", TType.Error)
                    {
                        LocalizationText = "MapLocalForm.Empty"
                    });

                    return;
                }

                string LocalPath_New = this.txtLocalPath.Text.Trim();
                if (string.IsNullOrEmpty(LocalPath_New))
                {
                    this.txtLocalPath.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "映射数据为空", TType.Error)
                    {
                        LocalizationText = "MapLocalForm.Empty"
                    });

                    return;
                }

                Operate.ProxyConfig.Proxy.MapProtocol ProtocolType_New = new Operate.ProxyConfig.Proxy.MapProtocol();
                if (this.ddlProtocolType.SelectedIndex == 0)
                {
                    ProtocolType_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }
                else
                {
                    ProtocolType_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }
                
                int port_New = ((int)this.nudPort.Value);
                string RemotePath_New = this.txtRemotePath.Text.Trim();

                if (this.mlSelect == null)
                {
                    Operate.ProxyConfig.Mapping.AddMapLocal(false, ProtocolType_New, Host_New, port_New, RemotePath_New, LocalPath_New);
                }
                else
                {
                    Operate.ProxyConfig.Mapping.UpdateMapLocal(this.mlSelect, ProtocolType_New, Host_New, port_New, RemotePath_New, LocalPath_New);
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this, "本地映射保存成功", TType.Success)
                {
                    LocalizationText = "MapLocalForm.Success"
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
            this.Close();
        }

        #endregion        
    }
}

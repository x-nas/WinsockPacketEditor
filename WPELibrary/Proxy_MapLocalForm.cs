using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_MapLocalForm : Form
    {
        private Proxy_MapLocal pmlSelect;

        #region//窗体事件

        public Proxy_MapLocalForm(Proxy_MapLocal pml)
        {
            InitializeComponent();

            this.pmlSelect = pml;
        }

        private void Proxy_MapLocalForm_Load(object sender, EventArgs e)
        {
            this.cbbProtocol.SelectedIndex = 0;

            if (this.pmlSelect != null)
            {
                if (this.pmlSelect.ProtocolType == Operate.ProxyConfig.SocketProxy.MapProtocol.Http)
                {
                    this.cbbProtocol.SelectedIndex = 0;
                }

                this.txtHost.Text = this.pmlSelect.Host;
                this.nudPort.Value = this.pmlSelect.Port;
                this.txtRemotePath.Text = this.pmlSelect.RemotePath;
                this.txtLocalPath.Text = this.pmlSelect.LocalPath;
            }
        }

        #endregion

        #region//确定

        private void bSure_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.ProxyConfig.SocketProxy.MapProtocol ProtocolType_New = new Operate.ProxyConfig.SocketProxy.MapProtocol();
                if (this.cbbProtocol.SelectedIndex == 0)
                {
                    ProtocolType_New = Operate.ProxyConfig.SocketProxy.MapProtocol.Http;
                }
                else
                {
                    ProtocolType_New = Operate.ProxyConfig.SocketProxy.MapProtocol.Http;
                }

                string Host_New = this.txtHost.Text.Trim();
                int port_New = ((int)this.nudPort.Value);
                string RemotePath_New = this.txtRemotePath.Text.Trim();
                string LocalPath_New = this.txtLocalPath.Text.Trim();

                if (string.IsNullOrEmpty(Host_New) || string.IsNullOrEmpty(LocalPath_New))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_219));
                    return;
                }

                if (this.pmlSelect == null)
                {
                    Operate.ProxyConfig.ProxyMapping.AddMapLocal(false, ProtocolType_New, Host_New, port_New, RemotePath_New, LocalPath_New);
                }
                else
                {
                    Operate.ProxyConfig.ProxyMapping.UpdateMapLocal(this.pmlSelect, ProtocolType_New, Host_New, port_New, RemotePath_New, LocalPath_New);
                }

                this.Close();                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//取消

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region//选择文件

        private void bLocalPath_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_218);
                ofd.Multiselect = false;
                ofd.InitialDirectory = string.Empty;                
                ofd.ShowDialog();

                this.txtLocalPath.Text = ofd.FileName;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}

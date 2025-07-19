using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_MapRemoteForm : Form
    {
        private MapRemote pmrSelect;

        #region//窗体事件

        public Proxy_MapRemoteForm(MapRemote pmr)
        {
            InitializeComponent();

            this.pmrSelect = pmr;
        }

        private void Proxy_MapRemoteForm_Load(object sender, EventArgs e)
        {
            this.cbbProtocol_From.SelectedIndex = 0;
            this.cbbProtocol_To.SelectedIndex = 0;

            if (this.pmrSelect != null)
            {
                if (this.pmrSelect.ProtocolTypeFrom == Operate.ProxyConfig.Proxy.MapProtocol.Http)
                {
                    this.cbbProtocol_From.SelectedIndex = 0;
                }

                this.txtHost_From.Text = this.pmrSelect.HostFrom;
                this.nudPort_From.Value = this.pmrSelect.PortFrom;
                this.txtPath_From.Text = this.pmrSelect.PathFrom;

                if (this.pmrSelect.ProtocolTypeTo == Operate.ProxyConfig.Proxy.MapProtocol.Http)
                {
                    this.cbbProtocol_To.SelectedIndex = 0;
                }
                else if (this.pmrSelect.ProtocolTypeTo == Operate.ProxyConfig.Proxy.MapProtocol.Https)
                {
                    this.cbbProtocol_To.SelectedIndex = 1;
                }

                this.txtHost_To.Text = this.pmrSelect.HostTo;
                this.nudPort_To.Value = this.pmrSelect.PortTo;
                this.txtPath_To.Text = this.pmrSelect.PathTo;
            }
        }

        #endregion

        #region//确定

        private void bSure_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.ProxyConfig.Proxy.MapProtocol ProtocolType_From_New = new Operate.ProxyConfig.Proxy.MapProtocol();
                if (this.cbbProtocol_From.SelectedIndex == 0)
                {
                    ProtocolType_From_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }
                else
                {
                    ProtocolType_From_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }

                Operate.ProxyConfig.Proxy.MapProtocol ProtocolType_To_New = new Operate.ProxyConfig.Proxy.MapProtocol();
                if (this.cbbProtocol_To.SelectedIndex == 0)
                {
                    ProtocolType_To_New = Operate.ProxyConfig.Proxy.MapProtocol.Http;
                }
                else if(this.cbbProtocol_To.SelectedIndex == 1)
                {
                    ProtocolType_To_New = Operate.ProxyConfig.Proxy.MapProtocol.Https;
                }

                string Host_From_New = this.txtHost_From.Text.Trim();
                int Port_From_New = ((int)this.nudPort_From.Value);
                string Path_From_New = this.txtPath_From.Text.Trim();

                string Host_To_New = this.txtHost_To.Text.Trim();
                int Port_To_New = ((int)this.nudPort_To.Value);
                string Path_To_New = this.txtPath_To.Text.Trim();


                if (string.IsNullOrEmpty(Host_From_New) || string.IsNullOrEmpty(Host_To_New))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_219));
                    return;
                }

                if (this.pmrSelect == null)
                {
                    Operate.ProxyConfig.Mapping.AddMapRemote(
                        false, 
                        ProtocolType_From_New, 
                        Host_From_New, 
                        Port_From_New, 
                        Path_From_New, 
                        ProtocolType_To_New, 
                        Host_To_New, 
                        Port_To_New, 
                        Path_To_New);
                }
                else
                {
                    Operate.ProxyConfig.Mapping.UpdateMapRemote(
                        this.pmrSelect, 
                        ProtocolType_From_New, 
                        Host_From_New, 
                        Port_From_New, 
                        Path_From_New, 
                        ProtocolType_To_New, 
                        Host_To_New, 
                        Port_To_New, 
                        Path_To_New );
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
    }
}

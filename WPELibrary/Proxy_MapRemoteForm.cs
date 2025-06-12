using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_MapRemoteForm : Form
    {
        private Proxy_MapRemote pmrSelect;

        #region//窗体事件

        public Proxy_MapRemoteForm(Proxy_MapRemote pmr)
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
                if (this.pmrSelect.ProtocolType_From == Socket_Cache.SocketProxy.MapProtocol.Http)
                {
                    this.cbbProtocol_From.SelectedIndex = 0;
                }

                this.txtHost_From.Text = this.pmrSelect.Host_From;
                this.nudPort_From.Value = this.pmrSelect.Port_From;
                this.txtPath_From.Text = this.pmrSelect.Path_From;

                if (this.pmrSelect.ProtocolType_To == Socket_Cache.SocketProxy.MapProtocol.Http)
                {
                    this.cbbProtocol_To.SelectedIndex = 0;
                }
                else if (this.pmrSelect.ProtocolType_To == Socket_Cache.SocketProxy.MapProtocol.Https)
                {
                    this.cbbProtocol_To.SelectedIndex = 1;
                }

                this.txtHost_To.Text = this.pmrSelect.Host_To;
                this.nudPort_To.Value = this.pmrSelect.Port_To;
                this.txtPath_To.Text = this.pmrSelect.Path_To;
            }
        }

        #endregion

        #region//确定

        private void bSure_Click(object sender, EventArgs e)
        {
            try
            {
                Socket_Cache.SocketProxy.MapProtocol ProtocolType_From_New = new Socket_Cache.SocketProxy.MapProtocol();
                if (this.cbbProtocol_From.SelectedIndex == 0)
                {
                    ProtocolType_From_New = Socket_Cache.SocketProxy.MapProtocol.Http;
                }
                else
                {
                    ProtocolType_From_New = Socket_Cache.SocketProxy.MapProtocol.Http;
                }

                Socket_Cache.SocketProxy.MapProtocol ProtocolType_To_New = new Socket_Cache.SocketProxy.MapProtocol();
                if (this.cbbProtocol_To.SelectedIndex == 0)
                {
                    ProtocolType_To_New = Socket_Cache.SocketProxy.MapProtocol.Http;
                }
                else if(this.cbbProtocol_To.SelectedIndex == 1)
                {
                    ProtocolType_To_New = Socket_Cache.SocketProxy.MapProtocol.Https;
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
                    Socket_Cache.ProxyMapping.AddMapRemote(
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
                    Socket_Cache.ProxyMapping.UpdateMapRemote(
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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

using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ClientListForm : AntdUI.Window
    {
        private ClientList cClientList = null;

        public ClientListForm()
        {
            Operate.ProxyConfig.List.IsClientListFormShow = true;

            InitializeComponent();

            Theme()
                .Light(Color.White, Color.Black)
                .Dark(Color.Black, Color.White)
                .Call(isDark =>
                {
                    this.Dark_Changed();
                });
        }

        private void ClientListForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("ClientList", "客户端列表");

            cClientList = new ClientList();
            cClientList.Dock = DockStyle.Fill;
            this.tlpClientList.Controls.Add(cClientList);

            this.Dark_Changed();
        }

        private void ClientListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.ProxyConfig.List.IsClientListFormShow = false;
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                BackColor = Operate.SystemConfig.Color_30;
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Operate.SystemConfig.Color_250;
                ForeColor = Color.Black;
            }

            this.cClientList?.Dark_Changed();
        }

        private async void timerClientList_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerClientList.Stop();

                await Task.Run(() =>
                {
                    this.cClientList?.RefreshClientList();
                    this.cClientList?.RefreshAuthList();
                });

                await Operate.ProxyConfig.Proxy.CheckUDPTimeOutAsync();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                this.timerClientList.Start();
            }
        }
    }
}

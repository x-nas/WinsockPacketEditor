using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SendListForm : AntdUI.Window, InterfaceInfo.ISendList
    {
        private SendList cSendList = null;

        public SendListForm()
        {
            Operate.SendConfig.List.IsSendListFormShow = true;

            InitializeComponent();

            Theme()
               .Light(Color.White, Color.Black)
               .Dark(Color.Black, Color.White)
               .Call(isDark =>
               {
                   this.Dark_Changed();
               });
        }

        private void SendListForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("SendList", "发送列表");

            cSendList = new SendList(this);
            cSendList.Dock = DockStyle.Fill;
            this.tlpSendList.Controls.Add(cSendList);

            this.Dark_Changed();
        }

        private void SendListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.SendConfig.List.IsSendListFormShow = false;
        }

        public void RefreshSendList()
        {
            this.cSendList?.RefreshSendList();
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

            this.cSendList?.Dark_Changed();
        }
    }
}

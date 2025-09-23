using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class AccountListForm : AntdUI.Window, InterfaceInfo.IAccountList
    {
        private AccountList cAccountList = null;

        public AccountListForm()
        {
            Operate.ProxyConfig.Account.IsAccountListFormShow = true;

            InitializeComponent();

            Theme()
                .Light(Color.White, Color.Black)
                .Dark(Color.Black, Color.White)
                .Call(isDark =>
                {
                    this.Dark_Changed();
                });
        }

        private void AccountListForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("AccountList", "账号列表");

            cAccountList = new AccountList(this);
            cAccountList.Dock = DockStyle.Fill;
            this.tlpAccountList.Controls.Add(cAccountList);

            this.Dark_Changed();
        }

        private void AccountListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.ProxyConfig.Account.IsAccountListFormShow = false;
        }

        public void RefreshAccountList()
        {
            this.cAccountList?.RefreshAccountList();
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

            this.cAccountList?.Dark_Changed();
        }
    }
}

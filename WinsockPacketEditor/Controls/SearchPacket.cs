using AntdUI;
using Be.Windows.Forms;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SearchPacket : UserControl
    {
        private Form form;

        #region//窗体初始化

        public SearchPacket(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void SearchPacket_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = AntdUI.Localization.Get("SearchPacketForm", "查找封包");
                this.txtFind.Text = Operate.PacketConfig.List.FindRegex;

                if (Operate.PacketConfig.List.FindOptions.Type == FindType.Text)
                {
                    this.rbString.Checked = true;
                }
                else if (Operate.PacketConfig.List.FindOptions.Type == FindType.Hex)
                {
                    this.rbHex.Checked = true;                    
                }                

                this.Dark_Changed();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(SearchPacket_Load), ex);
            }
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.txtFind.BackColor = Operate.SystemConfig.Color_30;
            }
            else
            {
                this.txtFind.BackColor = null;
            }
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            if (this.txtFind.Text.Length > 0)
            {
                this.txtFind.Status = TType.Success;
            }
            else
            {
                this.txtFind.Status = TType.Error;
            }
        }

        #endregion        

        #region//查找下一个

        private void bSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtFind.Text.Trim()))
                {
                    Operate.PacketConfig.List.FindOptions.IsValid = false;

                    this.txtFind.Status = TType.Error;
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "查找内容为空", TType.Error)
                    {
                        LocalizationText = "SearchPacketForm.Empty"
                    });

                    return;
                }

                if (rbString.Checked)
                {
                    Operate.PacketConfig.List.FindOptions.Type = FindType.Text;
                }
                else
                {
                    Operate.PacketConfig.List.FindOptions.Type = FindType.Hex;
                }

                Operate.PacketConfig.List.FindRegex = this.txtFind.Text;
                Operate.PacketConfig.List.FindOptions.IsValid = true;

                bool FromHead = this.rbFromHead.Checked;
                if (FromHead)
                {
                    this.rbFromIndex.Checked = true;
                }                

                if (this.form is InterfaceInfo.IInjectMode injectForm)
                {
                    injectForm.SearchPacketList(FromHead);
                }
                else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                {
                    proxyForm.SearchProxyList(FromHead);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSearch_Click), ex);
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

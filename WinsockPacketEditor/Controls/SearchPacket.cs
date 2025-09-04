using AntdUI;
using Be.Windows.Forms;
using System;
using System.Reflection;
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

                if (Operate.PacketConfig.List.FindOptions.Type == FindType.Text)
                {
                    this.rbString.Checked = true;
                    this.txtFind.Text = Operate.PacketConfig.List.FindOptions.Text;
                }
                else if (Operate.PacketConfig.List.FindOptions.Type == FindType.Hex)
                {
                    this.rbHex.Checked = true;
                    this.txtFind.Text = Operate.PacketConfig.List.FindRegex;
                }

                this.Dark_Changed();
                this.FindTypeChanged();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            if (this.rbString.Checked)
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
        }

        #endregion

        #region//搜索类型切换

        private void rbString_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.FindTypeChanged();
        }

        private void FindTypeChanged()
        {
            try
            {
                if (rbString.Checked)
                {
                    this.txtFind.PlaceholderText = "请输入文本";
                    this.txtFind.LocalizationPlaceholderText = "Input.Text";
                    this.txtFind.Focus();
                }
                else if (rbHex.Checked)
                {
                    this.txtFind.PlaceholderText = "请输入正则表达式";
                    this.txtFind.LocalizationPlaceholderText = "Input.Regex";
                    this.txtFind.Focus();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.PacketConfig.List.FindOptions.Text = txtFind.Text;
                }
                else
                {
                    Operate.PacketConfig.List.FindOptions.Type = FindType.Hex;
                    Operate.PacketConfig.List.FindRegex = this.txtFind.Text;
                }

                Operate.PacketConfig.List.FindOptions.IsValid = true;

                bool FromHead = this.rbFromHead.Checked;
                if (FromHead)
                {
                    this.rbFromIndex.Checked = true;
                }

                switch (Operate.SystemConfig.StartMode)
                {
                    case Operate.SystemConfig.SystemMode.Process:

                        ((InterfaceInfo.IInjectMode)form).SearchPacketList(FromHead);

                        break;

                    case Operate.SystemConfig.SystemMode.Proxy:

                        ((InterfaceInfo.IProxyMode)form).SearchProxyList(FromHead);

                        break;
                }
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

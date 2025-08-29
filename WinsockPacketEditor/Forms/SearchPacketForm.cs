using AntdUI;
using Be.Windows.Forms;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SearchPacketForm : Form
    {
        private Form form;

        #region//窗体初始化

        public SearchPacketForm(Form form)
        {  
            InitializeComponent();
            this.form = form;            
        }

        private void SearchPacketForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = AntdUI.Localization.Get("SearchPacketForm", "查找封包");
                this.tabSearchType.TabMenuVisible = false;

                if (Operate.PacketConfig.List.FindOptions.Type == FindType.Text)
                {
                    this.rbString.Checked = true;
                }
                else if (Operate.PacketConfig.List.FindOptions.Type == FindType.Hex)
                {
                    this.rbHex.Checked = true;
                }

                this.txtFind.Text = Operate.PacketConfig.List.FindOptions.Text;
                //this.chkMatchCase.Checked = Operate.PacketConfig.List.FindOptions.MatchCase;              

                if (Operate.PacketConfig.List.FindOptions.Hex != null && Operate.PacketConfig.List.FindOptions.Hex.Length > 0)
                {
                    this.hexFind.ByteProvider = new DynamicByteProvider(Operate.PacketConfig.List.FindOptions.Hex);
                }
                else
                {
                    byte[] bNew = new byte[0];
                    hexFind.ByteProvider = new DynamicByteProvider(bNew);
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
                this.hexFind.BackColor = Operate.SystemConfig.Color_30;
                this.hexFind.ForeColor = Color.Silver;
            }
            else
            {
                this.txtFind.BackColor = null;
                this.hexFind.BackColor = Color.White;
                this.hexFind.ForeColor = Color.Black;
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
                    this.tabSearchType.SelectTab(0);
                    this.txtFind.Focus();
                }
                else if (rbHex.Checked)
                {
                    this.tabSearchType.SelectTab(1);
                    this.hexFind.Focus();
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
            string findText = txtFind.Text.Trim();
            if (rbString.Checked && string.IsNullOrEmpty(findText))
            {
                Operate.PacketConfig.List.FindOptions.IsValid = false;

                this.txtFind.Status = TType.Error;
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "查找内容为空", TType.Error)
                {
                    LocalizationText = "SearchPacketForm.Empty"
                });

                return;
            }

            if (rbHex.Checked && hexFind.ByteProvider.Length == 0)
            {
                Operate.PacketConfig.List.FindOptions.IsValid = false;

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

            Operate.PacketConfig.List.FindOptions.Text = txtFind.Text;
            //Operate.PacketConfig.List.FindOptions.MatchCase = chkMatchCase.Checked;

            DynamicByteProvider dbp = this.hexFind.ByteProvider as DynamicByteProvider;
            if (dbp != null && dbp.Bytes.Count > 0)
            {
                Operate.PacketConfig.List.FindOptions.Hex = dbp.Bytes.ToArray();
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

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}

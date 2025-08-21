using AntdUI;
using Be.Windows.Forms;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class XORCalculation : UserControl
    {
        private Form form;

        #region//窗体事件

        public XORCalculation(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void XORCalculation_Load(object sender, EventArgs e)
        {
            this.hbXOR_From.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbXOR_From.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.hbXOR_To.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            this.Dark_Changed();
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.hbXOR_From.BackColor =
                    this.hbXOR_To.BackColor =
                    Color.FromArgb(30, 30, 30);

                this.hbXOR_From.ForeColor =
                    this.hbXOR_To.ForeColor =
                    Color.Silver;
            }
            else
            {
                this.hbXOR_From.BackColor =
                    this.hbXOR_To.BackColor =
                    Color.White;

                this.hbXOR_From.ForeColor =
                    this.hbXOR_To.ForeColor =
                    Color.Black;
            }
        }

        #endregion

        #region//异或计算

        private void txtXOR_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtXOR.Text.Trim()))
            {
                this.txtXOR.Status = TType.Error;
            }
            else
            {
                this.txtXOR.Status = TType.Success;
            }
        }

        private void bXOR_Click(object sender, EventArgs e)
        {
            try
            {
                DynamicByteProvider dbpXOR_From = this.hbXOR_From.ByteProvider as DynamicByteProvider;
                if (dbpXOR_From == null)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "异或值为空", TType.Error)
                    {
                        LocalizationText = "XORCalculation.XOREmpty"
                    });

                    return;
                }

                byte[] blXOR_From = dbpXOR_From.Bytes.ToArray();
                if (blXOR_From.Length == 0)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "异或值为空", TType.Error)
                    {
                        LocalizationText = "XORCalculation.XOREmpty"
                    });

                    return;
                }

                if (string.IsNullOrEmpty(this.txtXOR.Text.Trim()))
                {
                    this.txtXOR.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "异或值为空", TType.Error)
                    {
                        LocalizationText = "XORCalculation.XOREmpty"
                    });

                    return;
                }

                if (!Operate.SystemConfig.IsHexString(this.txtXOR.Text.Trim()))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "异或值不是十六进制", TType.Error)
                    {
                        LocalizationText = "XORCalculation.XORError"
                    });

                    return;
                }

                string[] slXOR_Value = this.txtXOR.Text.Trim().Split(' ');
                byte[] blXOR_To = new byte[blXOR_From.Length];
                int j = 0;

                foreach (byte bXOR_From in blXOR_From)
                {
                    if (j == slXOR_Value.Length)
                    {
                        j = 0;
                    }

                    if (!Byte.TryParse(slXOR_Value[j], System.Globalization.NumberStyles.HexNumber, null, out byte bXOR_Value))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "异或值不是十六进制", TType.Error)
                        {
                            LocalizationText = "XORCalculation.XORError"
                        });

                        return;
                    }

                    blXOR_To[j] = (byte)(bXOR_From ^ bXOR_Value);
                    j++;
                }

                DynamicByteProvider dbpXOR_To = new DynamicByteProvider(blXOR_To);
                this.hbXOR_To.ByteProvider = dbpXOR_To;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bXOR_ClearUp_Click(object sender, EventArgs e)
        {
            this.hbXOR_From.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.txtXOR.Clear();
        }

        private void hbXOR_From_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbXOR_From, (item) =>
                {
                    DynamicByteProvider dbp = hbXOR_From.ByteProvider as DynamicByteProvider;

                    switch (item.ID)
                    {
                        case "Cut":

                            this.hbXOR_From.Cut();

                            break;

                        case "Copy":

                            this.hbXOR_From.Copy();

                            break;

                        case "Paste":

                            this.hbXOR_From.Paste();

                            break;

                        case "SelectAll":

                            this.hbXOR_From.SelectAll();

                            break;
                    }
                }, Operate.SystemConfig.GetCMS_XOR(this.hbXOR_From)));
            }
        }

        private void hbXOR_To_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbXOR_To, (item) =>
                {
                    DynamicByteProvider dbp = hbXOR_To.ByteProvider as DynamicByteProvider;

                    switch (item.ID)
                    {
                        case "Cut":

                            this.hbXOR_To.Cut();

                            break;

                        case "Copy":

                            this.hbXOR_To.Copy();

                            break;

                        case "Paste":

                            this.hbXOR_To.Paste();

                            break;

                        case "SelectAll":

                            this.hbXOR_To.SelectAll();

                            break;
                    }
                }, Operate.SystemConfig.GetCMS_XOR(this.hbXOR_To)));
            }
        }

        #endregion        
    }
}

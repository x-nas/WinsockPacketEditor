using Be.Windows.Forms;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class PacketModificationForm : Form
    {
        private InjectModeForm imForm;
        private readonly PacketInfo piSelect;

        #region//窗体事件

        public PacketModificationForm(InjectModeForm form, PacketInfo pi)
        {
            InitializeComponent();

            if (pi == null)
            {
                string Title = AntdUI.Localization.Get("InjectModeForm.EditPacket.Error", "加载封包数据出错");
                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.piSelect = pi;
                this.imForm = form;
                this.Dark_Changed();
            }
        }

        private void PacketModificationForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = AntdUI.Localization.Get("PacketModificationForm", "封包修改");
                this.hbPacketData_Raw.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                this.hbPacketData_New.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                this.lPacketData_Raw.Text = string.Format(AntdUI.Localization.Get("System.PacketDataRaw", "原始封包数据  ( 长度 {0} )"), this.piSelect.RawBuffer.Length);
                this.lPacketData_New.Text = string.Format(AntdUI.Localization.Get("System.PacketDataNew", "修改后封包数据  ( 长度 {0} )"), this.piSelect.PacketBuffer.Length);

                if (this.piSelect.RawBuffer.Length > 0)
                {
                    hbPacketData_Raw.ByteProvider = new DynamicByteProvider(this.piSelect.RawBuffer);
                }

                if (this.piSelect.PacketBuffer.Length > 0)
                {
                    hbPacketData_New.ByteProvider = new DynamicByteProvider(this.piSelect.PacketBuffer);
                }
                
                this.txtModification_Result.Spin(AntdUI.Localization.Get("Loading", "正在加载..."), config =>
                {
                    this.txtModification_Result.Clear();

                    string sRawData = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.piSelect.RawBuffer);
                    string sModifiedData = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.piSelect.PacketBuffer);

                    if (!string.IsNullOrEmpty(sRawData) || !string.IsNullOrEmpty(sModifiedData))
                    {
                        string rtfString = Operate.SystemConfig.CompareData(this.Font, sRawData, sModifiedData);
                        var styles = Operate.SystemConfig.ConvertRtfToTextStyles(rtfString);

                        using (var rtb = new RichTextBox())
                        {
                            rtb.Rtf = rtfString;
                            this.txtModification_Result.Text = rtb.Text;
                        }

                        foreach (var style in styles)
                        {
                            if (style.Fore == Color.Red || style.Fore == Color.Green)
                            {
                                this.txtModification_Result.SetStyle(style.Start, style.Length, this.Font, style.Fore, null);
                            }
                            else
                            {
                                this.txtModification_Result.SetStyle(style.Start, style.Length, this.Font, null, null);
                            }
                        }
                    }
                }, () =>
                {
                    this.bExit.Enabled = true;
                });
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
                this.hbPacketData_Raw.BackColor = Color.FromArgb(30, 30, 30);
                this.hbPacketData_Raw.ForeColor = Color.Silver;
                this.hbPacketData_New.BackColor = Color.FromArgb(30, 30, 30);
                this.hbPacketData_New.ForeColor = Color.Silver;
            }
            else
            {
                this.hbPacketData_Raw.BackColor = Color.White;
                this.hbPacketData_Raw.ForeColor = Color.Black;
                this.hbPacketData_New.BackColor = Color.White;
                this.hbPacketData_New.ForeColor = Color.Black;
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

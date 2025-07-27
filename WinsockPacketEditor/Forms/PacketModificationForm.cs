using Be.Windows.Forms;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class PacketModificationForm : Form
    {
        private string sRawData = string.Empty;
        private string sModifiedData = string.Empty;
        private Form form;
        private PacketInfo packetInfo;
        private ProxyInfo proxyInfo;

        #region//窗体事件

        public PacketModificationForm(Form form, PacketInfo packetInfo, ProxyInfo proxyInfo)
        {
            InitializeComponent();

            if (packetInfo == null && proxyInfo == null)
            {
                string Title = AntdUI.Localization.Get("InjectModeForm.EditPacket.Error", "加载封包数据出错");
                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.packetInfo = packetInfo;
                this.proxyInfo = proxyInfo;
                this.form = form;
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

                switch (Operate.SystemConfig.StartMode)
                {
                    case Operate.SystemConfig.SystemMode.Process:

                        this.lPacketData_Raw.Text = string.Format(AntdUI.Localization.Get("System.PacketDataRaw", "原始封包数据  ( 长度 {0} )"), this.packetInfo.RawBuffer.Length);
                        this.lPacketData_New.Text = string.Format(AntdUI.Localization.Get("System.PacketDataNew", "修改后封包数据  ( 长度 {0} )"), this.packetInfo.PacketBuffer.Length);

                        if (this.packetInfo.RawBuffer.Length > 0)
                        {
                            hbPacketData_Raw.ByteProvider = new DynamicByteProvider(this.packetInfo.RawBuffer);
                            sRawData = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.packetInfo.RawBuffer);
                        }

                        if (this.packetInfo.PacketBuffer.Length > 0)
                        {
                            hbPacketData_New.ByteProvider = new DynamicByteProvider(this.packetInfo.PacketBuffer);
                            sModifiedData = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.packetInfo.PacketBuffer);
                        }

                        break;

                    case Operate.SystemConfig.SystemMode.Proxy:

                        this.lPacketData_Raw.Text = string.Format(AntdUI.Localization.Get("System.PacketDataRaw", "原始封包数据  ( 长度 {0} )"), this.proxyInfo.RawBuffer.Length);
                        this.lPacketData_New.Text = string.Format(AntdUI.Localization.Get("System.PacketDataNew", "修改后封包数据  ( 长度 {0} )"), this.proxyInfo.PacketBuffer.Length);

                        if (this.proxyInfo.RawBuffer.Length > 0)
                        {
                            hbPacketData_Raw.ByteProvider = new DynamicByteProvider(this.proxyInfo.RawBuffer);
                            sRawData = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.proxyInfo.RawBuffer);
                        }

                        if (this.proxyInfo.PacketBuffer.Length > 0)
                        {
                            hbPacketData_New.ByteProvider = new DynamicByteProvider(this.proxyInfo.PacketBuffer);
                            sModifiedData = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.proxyInfo.PacketBuffer);
                        }

                        break;
                }                
                
                this.txtModification_Result.Spin(AntdUI.Localization.Get("Loading", "正在加载..."), config =>
                {
                    this.txtModification_Result.Clear();                    

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

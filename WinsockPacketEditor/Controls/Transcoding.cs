using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class Transcoding : UserControl
    {
        #region//窗体事件

        public Transcoding()
        {
            InitializeComponent();
            this.Dark_Changed();
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.pTranscoding_Result.Back = Operate.SystemConfig.Color_40;
                this.txtTranscoding.BackColor = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.pTranscoding_Result.Back = null;
                this.txtTranscoding.BackColor = null;
            }
        }

        #endregion

        #region//编码转换

        private void bEncoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sEncodingText = this.txtTranscoding.Text.Trim();

                this.txtBytes.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Bytes, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sEncodingText));
                this.txtANSIGBK.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.GBK, sEncodingText));

                this.txtUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF7, sEncodingText));
                this.txtANSIUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF7, sEncodingText));

                this.txtUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sEncodingText));
                this.txtANSIUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sEncodingText));

                this.txtUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF16, sEncodingText));
                this.txtANSIUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF16, sEncodingText));

                this.txtUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF32, sEncodingText));
                this.txtANSIUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF32, sEncodingText));

                this.txtUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Unicode, sEncodingText));
                this.txtANSIUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Unicode, sEncodingText));

                string sBase64 = Operate.SystemConfig.Base64_Encoding(sEncodingText);
                this.txtbase64.Text = sBase64;
                this.txtANSIbase64.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sBase64));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bDecoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sDecodingText = this.txtTranscoding.Text;

                this.txtBytes.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Bytes, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIGBK.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.GBK, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF7, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF7, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF16, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF16, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF32, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF32, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Unicode, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Unicode, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtbase64.Text = Operate.SystemConfig.Base64_Decoding(sDecodingText);
                this.txtANSIbase64.Text = Operate.SystemConfig.Base64_Decoding(Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText)));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtTranscoding_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTranscoding.Text.Trim()))
            {
                this.txtTranscoding.Status = TType.Error;
            }
            else
            {
                this.txtTranscoding.Status = TType.Success;
            }
        }

        #endregion
    }
}

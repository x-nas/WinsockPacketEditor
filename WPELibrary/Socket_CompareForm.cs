using Be.Windows.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Socket_CompareForm : Form
    {
        private readonly PacketInfo SPI;      

        #region//窗体加载

        public Socket_CompareForm(PacketInfo spi)
        {
            InitializeComponent();

            if (spi != null)
            { 
                this.SPI = spi;
                this.InitForm();
            }
        }

        #endregion

        #region//初始化

        private async void InitForm()
        {
            try
            {
                this.rtbCompare.Clear();
                this.rtbCompare.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_155);      

                string sRawData = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.SPI.RawBuffer);
                string sModifiedData = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.SPI.PacketBuffer);                

                this.lRawData.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_134), this.SPI.RawBuffer.Length);
                this.lModifiedData.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_135), this.SPI.PacketBuffer.Length);

                if (this.SPI.RawBuffer.Length > 0)
                {
                    hbRawData.ByteProvider = new DynamicByteProvider(this.SPI.RawBuffer);
                }

                if (this.SPI.PacketBuffer.Length > 0)
                {
                    hbModifiedData.ByteProvider = new DynamicByteProvider(this.SPI.PacketBuffer);
                }

                this.rtbCompare.Rtf = await Socket_Operation.CompareData(this.Font, sRawData, sModifiedData);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}

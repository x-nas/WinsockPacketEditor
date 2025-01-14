using Be.Windows.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Socket_CompareForm : Form
    {
        private int Select_Index = -1;
        private byte[] bRawBuffer;
        private byte[] bModifiedBuffer;

        #region//窗体加载

        public Socket_CompareForm(int SelectIndex)
        {
            InitializeComponent();
            this.Select_Index = SelectIndex;
        }

        private void Socket_CompareForm_Load(object sender, EventArgs e)
        {
            if (this.Select_Index > -1)
            {
                this.InitForm();
            }
            else
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_28));
                this.Close();
            }
        }

        #endregion

        #region//初始化

        private async void InitForm()
        {
            try
            {
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_136), Select_Index + 1);

                this.rtbCompare.Clear();
                this.rtbCompare.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_155);

                this.bRawBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].RawBuffer;
                this.bModifiedBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer;                

                string sRawData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, this.bRawBuffer);
                string sModifiedData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, this.bModifiedBuffer);                

                this.lRawData.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_134), bRawBuffer.Length);
                this.lModifiedData.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_135), bModifiedBuffer.Length);

                if (bRawBuffer.Length > 0)
                {
                    hbRawData.ByteProvider = new DynamicByteProvider(bRawBuffer);
                }

                if (bModifiedBuffer.Length > 0)
                {
                    hbModifiedData.ByteProvider = new DynamicByteProvider(bModifiedBuffer);
                }

                this.rtbCompare.Rtf = await Socket_Operation.CompareData(this.Font, sRawData, sModifiedData);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}

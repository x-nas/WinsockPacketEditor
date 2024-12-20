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

        #region//窗体加载

        public Socket_CompareForm(int SelectIndex)
        {
            InitializeComponent();

            if (SelectIndex > -1)
            { 
                this.Select_Index = SelectIndex;
                this.InitForm();
            }
        }

        #endregion

        #region//初始化

        private void InitForm()
        {
            try
            {
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_136), Select_Index + 1);

                byte[] bRawBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].RawBuffer;
                byte[] bModifiedBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer;

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

                string sRawData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bRawBuffer);
                string sModifiedData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bModifiedBuffer);

                Socket_Operation.CompareData(this.rtbCompare, sRawData, sModifiedData);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}

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

                this.bRawBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].RawBuffer;
                this.bModifiedBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer;

                this.rtbCompare.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_155);

                if (!bgwCompare.IsBusy)
                {
                    bgwCompare.RunWorkerAsync();
                }

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
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示数据比对结果（异步）

        private void bgwCompare_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string sRawData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, this.bRawBuffer);
                string sModifiedData = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, this.bModifiedBuffer);

                e.Result = Socket_Operation.CompareData(this.Font, sRawData, sModifiedData);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bgwCompare_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.rtbCompare.Rtf = (string)e.Result;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//关闭窗体

        private void Socket_CompareForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgwCompare.IsBusy)
            {
                bgwCompare.CancelAsync();
            }
        }

        #endregion
    }
}

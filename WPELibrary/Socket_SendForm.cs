using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using WPELibrary.Lib;
using System.Diagnostics;
using System.Reflection;
using Be.Windows.Forms;

namespace WPELibrary
{
    public partial class Socket_SendForm : Form
    {
        private int Send_Socket = 0;
        private int Send_Times = 0;
        private int Send_Interval = 0;
        private int Send_CNT = 0;
        private int Send_Success = 0;
        private int Send_Fail = 0;
        private int Select_Index = 0;
        private byte[] Send_Bytes = null;

        #region//窗体加载

        public Socket_SendForm(int iSelectIndex)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);                
                InitializeComponent();

                this.Select_Index = iSelectIndex;
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_32), Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketIndex.ToString());

                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;

                this.InitHexBox();
                this.InitSendInfo();
                this.InitSendParameters();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化

        private void InitHexBox()
        {
            try
            {
                if (Select_Index < Socket_Cache.SocketList.lstRecPacket.Count)
                {
                    byte[] bSelected = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer;

                    DynamicByteProvider dbp = new DynamicByteProvider(bSelected);
                    dbp.Changed += new EventHandler(ByteProvider_Changed);
                    dbp.LengthChanged += new EventHandler(ByteProvider_LengthChanged);
                    hbPacketData.ByteProvider = dbp;

                    DefaultByteCharConverter defConverter = new DefaultByteCharConverter();
                    EbcdicByteCharProvider ebcdicConverter = new EbcdicByteCharProvider();
                    tscbEncoding.Items.Add(defConverter);
                    tscbEncoding.Items.Add(ebcdicConverter);
                    tscbEncoding.SelectedIndex = 0;
                    tscbPerLine.SelectedIndex = 1;                    

                    this.HexBox_LinePositionChanged();
                    this.HexBox_UpdatePacketLen();
                    this.HexBox_ManageAbility();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitSendInfo()
        {
            try
            {
                this.txtPacketTime.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTime.ToString("HH: mm: ss: fff");
                this.txtIPFrom.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketFrom;
                this.txtIPTo.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo;
                this.txtSocketType.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketType.ToString();
                this.nudSendLen.Value = this.hbPacketData.ByteProvider.Length;
                this.nudSendSocket.Value = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketSocket;                
                this.txtSendSocket_IP.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo.Split(':')[0];
                this.txtSendSocket_Port.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo.Split(':')[1];
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitSendParameters()
        {
            try
            {
                this.SendSocketChanged();
                this.SendTypeChanged();
                this.SendStepChanged();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送按钮

        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckSendPacket())
                {
                    this.bSend.Enabled = false;
                    this.bSendStop.Enabled = true;

                    this.gbSendSocket.Enabled = false;                    
                    this.gbSendStep.Enabled = false;
                    this.gbSendType.Enabled = false;

                    this.Send_CNT = 0;
                    this.Send_Success = 0;
                    this.Send_Fail = 0;

                    this.Send_Socket = (int)this.nudSendSocket.Value;
                    this.Send_Interval = (int)this.nudSendType_Interval.Value;
                    this.Send_Times = (int)this.nudSendType_Times.Value;

                    DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;
                    this.Send_Bytes = dbp.Bytes.ToArray();

                    if (!bgwSendPacket.IsBusy)
                    {
                        bgwSendPacket.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送封包（异步）

        private void bgwSendPacket_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (this.rbSendType_Continuously.Checked)
                {
                    while (!bgwSendPacket.CancellationPending)
                    {
                        bool bSendOK = this.SendPacket(this.Send_Socket, this.Send_Bytes);

                        if (bSendOK)
                        {
                            this.Send_Success++;
                        }
                        else
                        {
                            this.Send_Fail++;
                        }

                        this.Send_CNT++;

                        bgwSendPacket.ReportProgress(this.Send_CNT);
                        Thread.Sleep(Send_Interval);
                    }
                }
                else
                {
                    for (int i = 0; i < Send_Times; i++)
                    {
                        if (bgwSendPacket.CancellationPending)
                        {
                            return;
                        }
                        else
                        {
                            bool bSendOK = this.SendPacket(this.Send_Socket, this.Send_Bytes);

                            if (bSendOK)
                            {
                                this.Send_Success++;                                
                            }
                            else
                            {
                                this.Send_Fail++;                                
                            }

                            this.Send_CNT++;

                            bgwSendPacket.ReportProgress(this.Send_CNT);
                            Thread.Sleep(Send_Interval);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSendPacket_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            try
            {
                this.tlSendTimes_Value.Text = this.Send_CNT.ToString();
                this.tlSend_Success_Value.Text = this.Send_Success.ToString();
                this.tlSend_Fail_Value.Text = this.Send_Fail.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSendPacket_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;
                this.gbSendSocket.Enabled = true;                
                this.gbSendStep.Enabled = true;
                this.gbSendType.Enabled = true;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private bool SendPacket(int iSocket, byte[] bSendBuffer)
        {
            bool bReturn = false;

            try
            {
                if (iSocket >0 && bSendBuffer.Length > 0)
                {
                    if (this.cbSendStep.Checked)
                    {
                        int iStepIndex = (int)this.nudSendStep_Position.Value;
                        int iStepLen = ((int)this.nudSendStep_Len.Value) * this.Send_CNT;

                        bSendBuffer = Socket_Operation.GetStepBytes(bSendBuffer, iStepIndex, iStepLen);
                    }

                    IntPtr ipSend = Marshal.AllocHGlobal(bSendBuffer.Length);
                    Marshal.Copy(bSendBuffer, 0, ipSend, bSendBuffer.Length);

                    bReturn = WinSockHook.SendPacket(iSocket, ipSend, bSendBuffer.Length);
                }
            }
            catch (Exception ex)
            {                
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//停止按钮

        private void bSendStop_Click(object sender, EventArgs e)
        {
            bgwSendPacket.CancelAsync();
        }

        #endregion                

        #region//保存按钮

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hbPacketData.ByteProvider != null)
                {
                    DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;

                    byte[] bNewBuff = dbp.Bytes.ToArray();
                    int iNewLen = bNewBuff.Length;
                    string sNewPacketData_Hex = Socket_Operation.GetPacketData_Hex(bNewBuff, Socket_Cache.SocketPacket.PacketData_MaxLen);

                    Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer = bNewBuff;
                    Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketData = sNewPacketData_Hex;
                    Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketLen = iNewLen;

                    dbp.ApplyChanges();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                HexBox_ManageAbility();
            }
        }

        #endregion

        #region//关闭按钮

        private void bClose_Click(object sender, EventArgs e)
        {
            if (bgwSendPacket.IsBusy)
            {
                bgwSendPacket.CancelAsync();
            }

            this.Close();
        }

        #endregion

        #region//显示递进数据
        
        private void nudStepIndex_ValueChanged(object sender, EventArgs e)
        {            
            this.ShowStepValue();
        }
     
        private void nudStepLen_ValueChanged(object sender, EventArgs e)
        {
            this.ShowStepValue();
        }

        private void ShowStepValue()
        {            
            try
            {
                string sStepHex = string.Empty;
                string sStepHex_New = string.Empty;

                int iSelectIndex = (int)this.nudSendStep_Position.Value;
                int iPacketLen = (int)hbPacketData.ByteProvider.Length;

                if (iPacketLen >= iSelectIndex + 1)
                {
                    int iStepPosition = (int)this.nudSendStep_Position.Value;
                    int iStepLen = (int)this.nudSendStep_Len.Value;

                    if (hbPacketData.ByteProvider != null && hbPacketData.ByteProvider.Length > iStepPosition)
                    {
                        byte bStepBuffer = hbPacketData.ByteProvider.ReadByte(iStepPosition);
                        byte bStepBuffer_New = Socket_Operation.GetStepByte(bStepBuffer, iStepLen);

                        sStepHex = bStepBuffer.ToString("X2");
                        sStepHex_New = bStepBuffer_New.ToString("X2");
                    }
                }

                this.lSendStep_Position_Value.Text = sStepHex;
                this.lSendStep_Len_Value.Text = sStepHex_New;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion        

        #region//右键菜单

        private void cmsHexBox_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string sItemText = e.ClickedItem.Name;
                this.cmsHexBox.Close();

                int iPacketIndex = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketIndex;

                DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;                
                byte[] bBuffer = dbp.Bytes.ToArray();
                string sHex = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bBuffer);

                switch (sItemText)
                {                    
                    case "cmsHexBox_SendList":
                        
                        int iSocket = (int)this.nudSendSocket.Value;
                        string sIPTo = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo;

                        Socket_Cache.SendList.AddToSendList_New(iPacketIndex, "", iSocket, sIPTo, sHex, bBuffer);
                        Socket_Operation.ShowSendListForm();                       

                        break;
                    
                    case "cmsHexBox_FilterList":

                        string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + iPacketIndex.ToString() + "]";
                        Socket_Cache.FilterList.AddToFilterList_New(sFName, bBuffer);

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//检查发送数据

        private bool CheckSendPacket()
        { 
            try
            {
                int iSocket = (int)this.nudSendSocket.Value;

                if (iSocket == 0)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_45));
                    return false;
                }                

                if (hbPacketData.ByteProvider.Length == 0)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_46));
                    return false;
                }

                if (this.cbSendStep.Checked)
                {
                    if (this.lSendStep_Position_Value.Text.Equals("") || this.lSendStep_Len_Value.Text.Equals(""))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_47));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region//封包编辑器

        #region//可用性

        private void hbPacketData_CurrentLineChanged(object sender, EventArgs e)
        {
            this.HexBox_LinePositionChanged();
        }

        private void hbPacketData_CurrentPositionInLineChanged(object sender, EventArgs e)
        {
            this.HexBox_LinePositionChanged();
        }

        private void hbPacketData_Copied(object sender, EventArgs e)
        {
            this.HexBox_ManageAbilityForCopyAndPaste();
        }

        private void hbPacketData_CopiedHex(object sender, EventArgs e)
        {
            this.HexBox_ManageAbilityForCopyAndPaste();
        }

        private void hbPacketData_SelectionLengthChanged(object sender, EventArgs e)
        {
            this.HexBox_ManageAbilityForCopyAndPaste();
        }

        private void hbPacketData_SelectionStartChanged(object sender, EventArgs e)
        {
            this.HexBox_ManageAbilityForCopyAndPaste();
        }

        private void ByteProvider_Changed(object sender, EventArgs e)
        {
            this.HexBox_ManageAbility();
        }

        private void ByteProvider_LengthChanged(object sender, EventArgs e)
        {
            this.HexBox_UpdatePacketLen();
        }

        private void HexBox_UpdatePacketLen()
        {
            try
            {
                this.nudSendLen.Value = this.hbPacketData.ByteProvider.Length;
                this.nudSendStep_Position.Maximum = this.hbPacketData.ByteProvider.Length;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HexBox_LinePositionChanged()
        {
            try
            {
                int iSelectIndex = (int)hbPacketData.SelectionStart;

                if (iSelectIndex >= 0)
                {
                    this.nudSendStep_Position.Value = iSelectIndex;
                }                

                string sPacketDataPosition = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_24), hbPacketData.CurrentLine, hbPacketData.CurrentPositionInLine, iSelectIndex);

                string sBits_Value = string.Empty;
                string sChar_Value = string.Empty;
                string sByte_Value = string.Empty;
                string sShort_Value = string.Empty;
                string sUShort_Value = string.Empty;
                string sInt32_Value = string.Empty;
                string sUInt32_Value = string.Empty;
                string sInt64_Value = string.Empty;
                string sUInt64_Value = string.Empty;
                string sFloat_Value = string.Empty;
                string sDouble_Value = string.Empty;

                if (hbPacketData.ByteProvider != null && hbPacketData.ByteProvider.Length > hbPacketData.SelectionStart)
                {
                    byte bSelected = hbPacketData.ByteProvider.ReadByte(hbPacketData.SelectionStart);

                    Socket_BitInfo bitInfo = new Socket_BitInfo(bSelected, hbPacketData.SelectionStart);

                    if (bitInfo != null)
                    {
                        long start = hbPacketData.SelectionStart;
                        long selected = hbPacketData.SelectionLength;

                        if (selected == 0 || selected > 8)
                        {
                            selected = 8;
                        }

                        long last = hbPacketData.ByteProvider.Length;
                        long end = Math.Min(start + selected, last);

                        byte[] buffer64 = new byte[8];
                        int iBuffIndex = 0;

                        for (long i = start; i < end; i++)
                        {
                            buffer64[iBuffIndex] = hbPacketData.ByteProvider.ReadByte(i);
                            iBuffIndex++;
                        }

                        sBits_Value = bitInfo.ToString();
                        sChar_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Char, buffer64);
                        sByte_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Byte, buffer64);
                        sShort_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Short, buffer64);
                        sUShort_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UShort, buffer64);
                        sInt32_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Int32, buffer64);
                        sUInt32_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UInt32, buffer64);
                        sInt64_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Int64, buffer64);
                        sUInt64_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UInt64, buffer64);
                        sFloat_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Float, buffer64);
                        sDouble_Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Double, buffer64);
                    }
                }

                this.lHexBox_Position.Text = sPacketDataPosition;

                this.lBits_Value.Text = sBits_Value;
                this.lChar_Value.Text = sChar_Value;
                this.lByte_Value.Text = sByte_Value;
                this.lShort_Value.Text = sShort_Value;
                this.lUShort_Value.Text = sUShort_Value;
                this.lInt32_Value.Text = sInt32_Value;
                this.lUInt32_Value.Text = sUInt32_Value;
                this.lInt64_Value.Text = sInt64_Value;
                this.lUInt64_Value.Text = sUInt64_Value;
                this.lFloat_Value.Text = sFloat_Value;
                this.lDouble_Value.Text = sDouble_Value;                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void HexBox_ManageAbility()
        {
            try
            {
                if (hbPacketData.ByteProvider == null)
                {
                    this.bSave.Enabled = false;
                    tsPacketData_Find.Enabled = false;
                    tsPacketData_FindNext.Enabled = false;
                    tscbEncoding.Enabled = false;
                    tscbPerLine.Enabled = false;
                }
                else
                {
                    this.bSave.Enabled = hbPacketData.ByteProvider.HasChanges();
                    tsPacketData_Find.Enabled = true;
                    tsPacketData_FindNext.Enabled = true;
                    tscbEncoding.Enabled = true;
                    tscbPerLine.Enabled = true;
                }

                HexBox_ManageAbilityForCopyAndPaste();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HexBox_ManageAbilityForCopyAndPaste()
        {
            try
            {
                tsPacketData_Copy.Enabled = hbPacketData.CanCopy();
                tsPacketData_Cut.Enabled = hbPacketData.CanCut();
                tsPacketData_Paste.Enabled = hbPacketData.CanPaste();
                tsPacketData_Paste_PasteHex.Enabled = hbPacketData.CanPasteHex();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//剪切

        private void tsPacketData_Cut_Click(object sender, EventArgs e)
        {
            this.hbPacketData.Cut();
        }

        #endregion

        #region//复制

        private void tsPacketData_Copy_ButtonClick(object sender, EventArgs e)
        {
            this.hbPacketData.Copy();
        }

        private void tsPacketData_Copy_Copy_Click(object sender, EventArgs e)
        {
            this.hbPacketData.Copy();
        }

        private void tsPacketData_Copy_CopyHex_Click(object sender, EventArgs e)
        {
            this.hbPacketData.CopyHex();
        }

        #endregion

        #region//粘贴

        private void tsPacketData_Paste_ButtonClick(object sender, EventArgs e)
        {
            this.hbPacketData.Paste();
        }

        private void tsPacketData_Paste_Paste_Click(object sender, EventArgs e)
        {
            this.hbPacketData.Paste();
        }

        private void tsPacketData_Paste_PasteHex_Click(object sender, EventArgs e)
        {
            this.hbPacketData.PasteHex();
        }



        #endregion

        #region//查找

        private void tsPacketData_Find_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowFindForm();

                if (Socket_Cache.DoSearch)
                {
                    this.HexBox_FindNext();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsPacketData_FindNext_Click(object sender, EventArgs e)
        {
            this.HexBox_FindNext();
        }

        private void ShowFindForm()
        {
            try
            {
                Socket_FindForm sffFindForm = new Socket_FindForm();
                sffFindForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HexBox_FindNext()
        {
            try
            {
                if (Socket_Cache.FindOptions.IsValid)
                {
                    long res = hbPacketData.Find(Socket_Cache.FindOptions);

                    if (res == -1)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_23));
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//编码

        private void tscbEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hbPacketData.ByteCharConverter = tscbEncoding.SelectedItem as IByteCharConverter;
                this.hbPacketData.Focus();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//排列

        private void tscbPerLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int iIndex = tscbPerLine.SelectedIndex;

                if (iIndex == 0)
                {
                    this.hbPacketData.UseFixedBytesPerLine = false;
                }
                else if (iIndex == 1)
                {
                    this.hbPacketData.UseFixedBytesPerLine = true;
                }

                this.hbPacketData.Focus();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        #endregion

        #endregion

        #region//套接字

        private void rbSendSocket_UseSocket_CheckedChanged(object sender, EventArgs e)
        {
            this.SendSocketChanged();
        }

        private void rbSendSocket_NewSocket_CheckedChanged(object sender, EventArgs e)
        {
            this.SendSocketChanged();
        }

        private void SendSocketChanged()
        {
            try
            {
                if (this.rbSendSocket_UseSocket.Checked)
                {
                    this.nudSendSocket.Enabled = true;
                    this.txtSendSocket_IP.Enabled = false;
                    this.txtSendSocket_Port.Enabled = false;
                }
                else
                {
                    this.nudSendSocket.Enabled = false;
                    this.txtSendSocket_IP.Enabled = true;
                    this.txtSendSocket_Port.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送类型

        private void rbSendType_Continuously_CheckedChanged(object sender, EventArgs e)
        {
            this.SendTypeChanged();
        }

        private void rbSendType_Times_CheckedChanged(object sender, EventArgs e)
        {
            this.SendTypeChanged();
        }

        private void SendTypeChanged()
        {
            try
            {
                if (this.rbSendType_Continuously.Checked)
                {
                    this.nudSendType_Times.Enabled = false;
                }
                else
                {
                    this.nudSendType_Times.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//递进

        private void cbSendStep_CheckedChanged(object sender, EventArgs e)
        {
            this.SendStepChanged();
        }

        private void SendStepChanged()
        {
            try
            {
                if (this.cbSendStep.Checked)
                {
                    this.nudSendStep_Position.Enabled = true;
                    this.nudSendStep_Len.Enabled = true;
                }
                else
                {
                    this.nudSendStep_Position.Enabled = false;
                    this.nudSendStep_Len.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}

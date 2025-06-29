using Be.Windows.Forms;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Socket_SendForm : Form
    {
        private PacketInfo SPI;
        private int Send_CNT = 0;
        private int Send_Success = 0;
        private int Send_Fail = 0;        
        private CancellationTokenSource cts;

        #region//窗体加载

        public Socket_SendForm(PacketInfo spi)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);                
                InitializeComponent();

                if (spi != null)
                { 
                    this.SPI = spi;
                }  
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化

        private void Socket_SendForm_Load(object sender, EventArgs e)
        {
            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;

            this.InitHexBox();
            this.InitSendInfo();            
            this.SendTypeChanged();
            this.ProgressionPositionChange();
        }

        private void Socket_SendForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopSend();
        }

        private void InitSendInfo()
        {
            try
            {  
                this.txtPacketTime.Text = this.SPI.PacketTime.ToString("HH: mm: ss: fffffff");              
                this.txtPacketType.Text = Operate.PacketConfig.Packet.GetName_ByPacketType(this.SPI.PacketType);

                this.txtIPFrom.Text = this.SPI.PacketFrom;
                this.txtIPTo.Text = this.SPI.PacketTo;
                this.pbSocketType.Image = Operate.PacketConfig.Packet.GetImg_ByPacketType(this.SPI.PacketType);
                
                this.nudSendSocket_Len.Value = hbPacketData.ByteProvider.Length;
                this.nudSendSocket_Socket.Value = this.SPI.PacketSocket;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitHexBox()
        {
            try
            {
                DynamicByteProvider dbp = new DynamicByteProvider(this.SPI.PacketBuffer);
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
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//递进设置

        private void cbProgressionPosition_CheckedChanged(object sender, EventArgs e)
        {
            this.ProgressionPositionChange();
        }

        private void ProgressionPositionChange()
        {
            if (this.cbProgressionPosition.Checked)
            {
                this.cbProgressionCarry.Enabled = true;
                this.nudProgressionPosition.Enabled = true;
                this.nudProgressionStep.Enabled = true;

                this.ProgressionCarryChange();
            }
            else
            {
                this.cbProgressionCarry.Enabled = false;
                this.nudProgressionPosition.Enabled = false;
                this.nudProgressionStep.Enabled = false;
                this.nudProgressionCarry.Enabled = false;
            }          
        }

        private void cbProgressionCarry_CheckedChanged(object sender, EventArgs e)
        {
            this.ProgressionCarryChange();
        }

        private void ProgressionCarryChange()
        {
            this.nudProgressionCarry.Enabled = this.cbProgressionCarry.Checked;
        }

        #endregion        

        #region//发送类型参数

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
            this.nudSendType_Times.Enabled = !this.rbSendType_Continuously.Checked;            
        }

        #endregion        

        #region//检查发送数据

        private bool CheckSendPacket()
        {
            try
            {
                int iSocket = (int)this.nudSendSocket_Socket.Value;

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

                if (this.cbProgressionPosition.Checked)
                {
                    int iProgressionPosition = (int)this.nudProgressionPosition.Value;

                    if (iProgressionPosition >= hbPacketData.ByteProvider.Length)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_47));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region//发送封包（异步）

        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckSendPacket())
                {
                    if (!bgwSendPacket.IsBusy)
                    {
                        this.bSend.Enabled = false;
                        this.bSendStop.Enabled = true;

                        this.gbSendSocket.Enabled = false;
                        this.gbSendStep.Enabled = false;
                        this.gbSendType.Enabled = false;

                        this.Send_CNT = 0;
                        this.Send_Success = 0;
                        this.Send_Fail = 0;
                        this.tlSendTimes_Value.Text = this.Send_CNT.ToString();
                        this.tlSend_Success_Value.Text = this.Send_Success.ToString();
                        this.tlSend_Fail_Value.Text = this.Send_Fail.ToString();

                        this.cts =new CancellationTokenSource();
                        this.bgwSendPacket.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSendPacket_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                int iSocket = (int)this.nudSendSocket_Socket.Value;
                int iSend_Interval = (int)this.nudSendType_Interval.Value;
                int iSend_Times = (int)this.nudSendType_Times.Value;
                string sIPFrom = this.txtIPFrom.Text.Trim();
                string sIPTo = this.txtIPTo.Text.Trim();

                DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;
                byte[] bBuff = dbp.Bytes.ToArray();

                if (this.rbSendType_Continuously.Checked)
                {
                    int iSendCount = 0;
                    while (!bgwSendPacket.CancellationPending)
                    {
                        this.DoSendPacket(iSocket, sIPFrom, sIPTo, bBuff, iSendCount);
                        iSendCount++;

                        if (iSend_Interval > 0)
                        {
                            Socket_Operation.DoSleepAsync(iSend_Interval, this.cts.Token).Wait();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < iSend_Times; i++)
                    {
                        if (bgwSendPacket.CancellationPending)
                        {
                            return;
                        }
                        else
                        {
                            this.DoSendPacket(iSocket, sIPFrom, sIPTo, bBuff, i);

                            if (iSend_Interval > 0)
                            {
                                Socket_Operation.DoSleepAsync(iSend_Interval, this.cts.Token).Wait();
                            }                                
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSendPacket_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tlSendTimes_Value.Text = this.Send_CNT.ToString();
                this.tlSend_Success_Value.Text = this.Send_Success.ToString();
                this.tlSend_Fail_Value.Text = this.Send_Fail.ToString();

                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;
                this.gbSendSocket.Enabled = true;                
                this.gbSendStep.Enabled = true;
                this.gbSendType.Enabled = true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void DoSendPacket(int iSocket, string sIPFrom, string sIPTo, byte[] bSendBuff, int SendCount)
        {
            try
            {
                if (this.cbProgressionPosition.Checked)
                {
                    int iCarryCount = 0;
                    int iIndex = (int)this.nudProgressionPosition.Value;
                    int iStep = (int)this.nudProgressionStep.Value;

                    byte bValue = bSendBuff[iIndex];
                    bValue = Socket_Operation.GetStepByte(bValue, iStep, out iCarryCount);
                    bSendBuff[iIndex] = bValue;

                    if (this.cbProgressionCarry.Checked && iCarryCount > 0)
                    {
                        for (int i = 0; i < this.nudProgressionCarry.Value; i++)
                        {
                            int iIndexPre = iIndex - (i + 1);

                            if (iIndexPre > -1)
                            {
                                byte bValuePrev = bSendBuff[iIndexPre];
                                bValuePrev = Socket_Operation.GetStepByte(bValuePrev, iCarryCount, out iCarryCount);
                                bSendBuff[iIndexPre] = bValuePrev;

                                if (iCarryCount == 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                bool bSendOK = Socket_Operation.SendPacket(iSocket, this.SPI.PacketType, sIPFrom, sIPTo, bSendBuff);

                if (bSendOK)
                {
                    this.Send_Success++;
                }
                else
                {
                    this.Send_Fail++;
                }

                this.Send_CNT++;                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//停止按钮

        private void bSendStop_Click(object sender, EventArgs e)
        {
            this.StopSend();
        }

        private void StopSend()
        {
            try
            {
                if (this.bgwSendPacket.IsBusy)
                {
                    if (this.cts != null)
                    {
                        this.cts.Cancel();
                    }
                    
                    this.bgwSendPacket.CancelAsync();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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

                    if (dbp != null)
                    {
                        byte[] bNewBuff = dbp.Bytes.ToArray();
                        int iNewLen = bNewBuff.Length;
                        Span<byte> bufferSpan = bNewBuff.AsSpan();
                        string sNewPacketData_Hex = Operate.PacketConfig.Packet.GetPacketData_Hex(bufferSpan, Operate.PacketConfig.Packet.PacketData_MaxLen);

                        this.SPI.PacketSocket = (int)this.nudSendSocket_Socket.Value;
                        this.SPI.PacketBuffer = bNewBuff;
                        this.SPI.PacketData = sNewPacketData_Hex;
                        this.SPI.PacketLen = iNewLen;

                        dbp.ApplyChanges();
                    }                    
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            this.Close();
        }

        #endregion

        #region//右键菜单

        private void cmsHexBox_Opening(object sender, CancelEventArgs e)
        {
            Socket_Operation.InitSendListComboBox(this.tscbSendList);
        }

        private void tscbSendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.tscbSendList.SelectedItem != null)
                {
                    Operate.SendConfig.SendList.SendListItem item = (Operate.SendConfig.SendList.SendListItem)this.tscbSendList.SelectedItem;
                    Guid SID = item.SID;
                    BindingList<PacketInfo> SCollection = Operate.SendConfig.Send.GetSendCollection_ByGuid(SID);

                    if (SCollection != null)
                    {
                        int iSocket = (int)this.nudSendSocket_Socket.Value;
                        string sIPFrom = this.txtIPFrom.Text.Trim();
                        string sIPTo = this.txtIPTo.Text.Trim();

                        byte[] bBuffer = null;

                        if (this.hbPacketData.CanCopy())
                        {
                            this.hbPacketData.CopyHex();
                            bBuffer = Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());                            
                        }
                        else
                        {
                            DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;
                            bBuffer = dbp.Bytes.ToArray();
                        }                        

                        Operate.SendConfig.Send.AddSendCollection(SCollection, iSocket, this.SPI.PacketType, sIPFrom, sIPTo, bBuffer);                      
                    }                                       

                    this.cmsHexBox.Close();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cmsHexBox_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string sItemText = e.ClickedItem.Name;
                this.cmsHexBox.Close();

                DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;                
                byte[] bBuffer = dbp.Bytes.ToArray();

                switch (sItemText)
                {  
                    case "cmsHexBox_FilterList":

                        if (this.hbPacketData.CanCopy())
                        {
                            this.hbPacketData.CopyHex();

                            byte[] bBufferCopy = Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                            Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(this.SPI, bBufferCopy);
                        }
                        else
                        {
                            Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(this.SPI, bBuffer);
                        }

                        break;

                    case "cmsHexBox_SelectAll":

                        this.hbPacketData.SelectAll();

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
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
                this.nudSendSocket_Len.Value = this.hbPacketData.ByteProvider.Length;                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HexBox_LinePositionChanged()
        {
            try
            {
                int iSelectIndex = (int)hbPacketData.SelectionStart;
                this.nudProgressionPosition.Value = iSelectIndex;                  

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
                        sChar_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Char, buffer64);
                        sByte_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Byte, buffer64);
                        sShort_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Short, buffer64);
                        sUShort_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UShort, buffer64);
                        sInt32_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Int32, buffer64);
                        sUInt32_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UInt32, buffer64);
                        sInt64_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Int64, buffer64);
                        sUInt64_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UInt64, buffer64);
                        sFloat_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Float, buffer64);
                        sDouble_Value = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Double, buffer64);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    this.bSave.Enabled = true;
                    tsPacketData_Find.Enabled = true;
                    tsPacketData_FindNext.Enabled = true;
                    tscbEncoding.Enabled = true;
                    tscbPerLine.Enabled = true;
                }

                HexBox_ManageAbilityForCopyAndPaste();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                if (Operate.PacketConfig.List.DoSearch)
                {
                    this.HexBox_FindNext();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HexBox_FindNext()
        {
            try
            {
                if (Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    long res = hbPacketData.Find(Operate.PacketConfig.List.FindOptions);

                    if (res == -1)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_23));
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #endregion        
    }
}

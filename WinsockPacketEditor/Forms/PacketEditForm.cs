using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class PacketEditForm : Form
    {
        private Form form;
        private PacketInfo packetInfo;
        private ProxyInfo proxyInfo;
        private int Send_CNT = 0;
        private int Send_Success = 0;
        private int Send_Fail = 0;
        private int SendSocket = 0;
        private int SendCNT = 0;
        private int SendINT = 0;

        #region//窗体事件

        public PacketEditForm(Form form, PacketInfo packetInfo, ProxyInfo proxyInfo)
        {
            InitializeComponent();
            this.packetInfo = packetInfo;
            this.proxyInfo = proxyInfo;
            this.form = form;
        }

        private void PacketEditForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("PacketEditForm", "封包编辑");

            this.hbPacketEdit.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            DynamicByteProvider dbp = null;

            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    this.nudPacketSocket.Value = this.packetInfo.PacketSocket;
                    this.nudPacketLength.Value = this.packetInfo.PacketLen;
                    this.txtPacketTo.Text = this.packetInfo.PacketTo;

                    dbp = new DynamicByteProvider(this.packetInfo.PacketBuffer);

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    this.nudPacketSocket.Value = this.proxyInfo.PacketSocket;
                    this.nudPacketLength.Value = this.proxyInfo.PacketLen;
                    this.txtPacketTo.Text = this.proxyInfo.ServerAddr;

                    dbp = new DynamicByteProvider(this.proxyInfo.PacketBuffer);

                    break;
            }
                        
            dbp.LengthChanged += new EventHandler(ByteProvider_LengthChanged);
            hbPacketEdit.ByteProvider = dbp;

            DefaultByteCharConverter defConverter = new DefaultByteCharConverter();
            EbcdicByteCharProvider ebcdicConverter = new EbcdicByteCharProvider();         
            
            this.ProgressionPosition_Change();
            this.Dark_Changed();
        }

        private void PacketEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopSend();
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.hbPacketEdit.BackColor = Color.FromArgb(30, 30, 30);
                this.hbPacketEdit.ForeColor = Color.Silver;
            }
            else
            {
                this.hbPacketEdit.BackColor = Color.White;
                this.hbPacketEdit.ForeColor = Color.Black;
            }
        }

        #endregion        

        #region//发送类型

        private void rbSendType_Times_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.SendType_Changed();
        }

        private void SendType_Changed()
        {
            this.nudSendType_Times.Enabled = this.rbSendType_Times.Checked;
        }

        #endregion

        #region//递进

        private void cbProgressionCarry_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ProgressionCarry_Change();
        }

        private void ProgressionCarry_Change()
        {
            this.nudProgressionCarry.Enabled = this.cbProgressionCarry.Checked;
        }

        private void cbProgressionPosition_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ProgressionPosition_Change();
        }

        private void ProgressionPosition_Change()
        {
            this.cbProgressionCarry.Enabled =
                this.nudProgressionPosition.Enabled =
                this.nudProgressionStep.Enabled =
                this.nudProgressionCarry.Enabled =
                this.cbProgressionPosition.Checked;

            if (this.cbProgressionPosition.Checked)
            {
                this.ProgressionCarry_Change();
            }
        }

        #endregion

        #region//检查发送数据

        private bool CheckSendPacket()
        {
            if ((int)this.nudPacketSocket.Value == 0)
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "套接字设置错误", TType.Error)
                {
                    LocalizationText = "PacketEditForm.Socket.Error"
                });

                return false;
            }

            if (hbPacketEdit.ByteProvider.Length == 0)
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "封包数据为空", TType.Error)
                {
                    LocalizationText = "PacketEditForm.Packet.Empty"
                });

                return false;
            }

            if (this.cbProgressionPosition.Checked)
            {
                if ((int)this.nudProgressionPosition.Value >= hbPacketEdit.ByteProvider.Length)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "递进位置错误", TType.Error)
                    {
                        LocalizationText = "PacketEditForm.Position.Error"
                    });

                    return false;
                }
            }

            return true;
        }

        #endregion

        #region//编辑器事件

        private void ByteProvider_LengthChanged(object sender, EventArgs e)
        {
            this.HexBox_UpdatePacketLen();
        }        

        private void HexBox_UpdatePacketLen()
        {
            this.nudPacketLength.Value = this.hbPacketEdit.ByteProvider.Length;
        }

        private void hbPacketEdit_CurrentPositionInLineChanged(object sender, EventArgs e)
        {
            this.nudProgressionPosition.Value = (int)hbPacketEdit.SelectionStart;
        }

        #endregion

        #region//右键菜单

        private void hbPacketEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DynamicByteProvider dbp = this.hbPacketEdit.ByteProvider as DynamicByteProvider;
                if (dbp == null || dbp.Bytes.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbPacketEdit, (item) =>
                {
                    switch (item.ID)
                    {
                        case "ToFilterList":
                            
                            byte[] bBufferToFilter = null;
                            if (this.hbPacketEdit.CanCopy())
                            {
                                this.hbPacketEdit.CopyHex();
                                bBufferToFilter = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());                                                                
                            }
                            else
                            {
                                bBufferToFilter = dbp.Bytes.ToArray();
                            }

                            bool bOK = false;
                            switch (Operate.SystemConfig.StartMode)
                            {
                                case Operate.SystemConfig.SystemMode.Process:
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(this.packetInfo, bBufferToFilter);
                                    break;

                                case Operate.SystemConfig.SystemMode.Proxy:
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(this.proxyInfo, bBufferToFilter);
                                    break;
                            }

                            if (bOK)
                            {
                                AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到滤镜列表", TType.Success)
                                {
                                    LocalizationText = "ToFilterList.Success"
                                });
                            }
                            else
                            {
                                AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到滤镜列表出错", TType.Error)
                                {
                                    LocalizationText = "ToFilterList.Error"
                                });
                            }

                            break;

                        case "Cut":

                            this.hbPacketEdit.Cut();

                            break;

                        case "Copy_Text":

                            this.hbPacketEdit.Copy();

                            break;

                        case "Copy_Hex":

                            this.hbPacketEdit.CopyHex();

                            break;

                        case "Paste_Text":

                            this.hbPacketEdit.Paste();

                            break;

                        case "Paste_Hex":

                            this.hbPacketEdit.PasteHex();

                            break;

                        case "SelectAll":

                            this.hbPacketEdit.SelectAll();

                            break;

                        default:

                            if (Guid.TryParse(item.ID, out Guid SID))
                            {
                                SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                if (si != null)
                                {
                                    int iSocket = (int)this.nudPacketSocket.Value;

                                    byte[] bBuffer = null;
                                    if (this.hbPacketEdit.CanCopy())
                                    {
                                        this.hbPacketEdit.CopyHex();
                                        bBuffer = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    }
                                    else
                                    {
                                        bBuffer = dbp.Bytes.ToArray();
                                    }

                                    bool bAddOK = false;
                                    switch (Operate.SystemConfig.StartMode)
                                    {
                                        case Operate.SystemConfig.SystemMode.Process:

                                            List<PacketInfo> lstPacketInfo = new List<PacketInfo>();
                                            PacketInfo packetInfo = new PacketInfo();
                                            packetInfo.PacketSocket = this.packetInfo.PacketSocket;
                                            packetInfo.PacketType = this.packetInfo.PacketType;
                                            packetInfo.PacketFrom = this.packetInfo.PacketFrom;
                                            packetInfo.PacketTo = this.packetInfo.PacketTo;
                                            packetInfo.PacketBuffer = bBuffer;
                                            packetInfo.PacketLen = bBuffer.Length;
                                            packetInfo.PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bBuffer, Operate.PacketConfig.Packet.PacketData_MaxLen);
                                            lstPacketInfo.Add(packetInfo);

                                            bAddOK = Operate.SendConfig.Send.AddSendCollection_ByPacketInfo(SID, lstPacketInfo);

                                            break;

                                        case Operate.SystemConfig.SystemMode.Proxy:

                                            List<ProxyInfo> lstProxyInfo = new List<ProxyInfo>();
                                            ProxyInfo proxyInfo = new ProxyInfo();
                                            proxyInfo.PacketSocket = this.proxyInfo.PacketSocket;
                                            proxyInfo.PacketType = this.proxyInfo.PacketType;
                                            proxyInfo.ClientAddr = this.proxyInfo.ClientAddr;
                                            proxyInfo.ServerAddr = this.proxyInfo.ServerAddr;
                                            proxyInfo.PacketBuffer = bBuffer;
                                            proxyInfo.PacketLen = bBuffer.Length;
                                            proxyInfo.PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bBuffer, Operate.PacketConfig.Packet.PacketData_MaxLen);
                                            lstProxyInfo.Add(proxyInfo);

                                            bAddOK = Operate.SendConfig.Send.AddSendCollection_ByProxyInfo(SID, lstProxyInfo);

                                            break;
                                    }

                                    if (bAddOK)
                                    {
                                        string sText = string.Format(AntdUI.Localization.Get("ToSendList.Success", "已添加到: {0}"), item.Text);
                                        AntdUI.Message.open(new AntdUI.Message.Config(this, sText, TType.Success));                                     
                                    }
                                    else
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到发送列表出错", TType.Error)
                                        {
                                            LocalizationText = "ToSendList.Error",
                                        });
                                    }
                                }
                            }

                            break;
                    }
                }, Operate.PacketConfig.Packet.GetCMS_PacketEdit(this.hbPacketEdit)));
            }
        }

        #endregion

        #region//发送

        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckSendPacket())
                {
                    return;
                }

                if (!bgwSendPacket.IsBusy)
                {
                    this.bSend.Loading = true;
                    this.bStop.Enabled = true;

                    this.pPacketSocket.Enabled = false;
                    this.pPacketSend.Enabled = false;
                    this.pProgression.Enabled = false;

                    this.Send_CNT = 0;
                    this.Send_Success = 0;
                    this.Send_Fail = 0;

                    this.lTotal_Send_CNT.Text = this.Send_CNT.ToString();
                    this.lSend_Success_CNT.Text = this.Send_Success.ToString();
                    this.lSend_Fail_CNT.Text = this.Send_Fail.ToString();

                    this.SendSocket = (int)this.nudPacketSocket.Value;
                    this.SendINT = (int)this.nudSendType_Interval.Value;
                    this.SendCNT = (int)this.nudSendType_Times.Value;

                    this.bgwSendPacket.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送封包（异步）

        private void bgwSendPacket_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                DynamicByteProvider dbp = hbPacketEdit.ByteProvider as DynamicByteProvider;
                byte[] bBuff = dbp.Bytes.ToArray();

                if (this.rbSendType_Continuously.Checked)
                {
                    int iSendCount = 0;
                    while (!bgwSendPacket.CancellationPending)
                    {
                        switch (Operate.SystemConfig.StartMode)
                        {
                            case Operate.SystemConfig.SystemMode.Process:
                                this.DoSendPacket(this.SendSocket, this.packetInfo.PacketFrom, this.packetInfo.PacketTo, bBuff, iSendCount, this.packetInfo.PacketType);
                                break;

                            case Operate.SystemConfig.SystemMode.Proxy:
                                this.DoSendPacket(this.SendSocket, this.proxyInfo.ClientAddr, this.proxyInfo.ServerAddr, bBuff, iSendCount, this.proxyInfo.PacketType);
                                break;
                        }
                        
                        iSendCount++;

                        if (this.SendINT > 0)
                        {
                            bgwSendPacket.ReportProgress(iSendCount);
                            Thread.Sleep(this.SendINT);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.SendCNT; i++)
                    {
                        if (bgwSendPacket.CancellationPending)
                        {
                            return;
                        }
                        else
                        {
                            switch (Operate.SystemConfig.StartMode)
                            {
                                case Operate.SystemConfig.SystemMode.Process:
                                    this.DoSendPacket(this.SendSocket, this.packetInfo.PacketFrom, this.packetInfo.PacketTo, bBuff, i, this.packetInfo.PacketType);
                                    break;

                                case Operate.SystemConfig.SystemMode.Proxy:
                                    this.DoSendPacket(this.SendSocket, this.proxyInfo.ClientAddr, this.proxyInfo.ServerAddr, bBuff, i, this.proxyInfo.PacketType);
                                    break;
                            }                            

                            if (this.SendINT > 0)
                            {
                                bgwSendPacket.ReportProgress(i); 
                                Thread.Sleep(this.SendINT);
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

        private void bgwSendPacket_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.lTotal_Send_CNT.Text = this.Send_CNT.ToString();
            this.lSend_Success_CNT.Text = this.Send_Success.ToString();
            this.lSend_Fail_CNT.Text = this.Send_Fail.ToString();
        }

        private void bgwSendPacket_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.bSend.Loading = false;
            this.bStop.Enabled = false;

            this.pPacketSocket.Enabled = true;
            this.pPacketSend.Enabled = true;
            this.pProgression.Enabled = true;

            this.lTotal_Send_CNT.Text = this.Send_CNT.ToString();
            this.lSend_Success_CNT.Text = this.Send_Success.ToString();
            this.lSend_Fail_CNT.Text = this.Send_Fail.ToString();
        }

        private void DoSendPacket(
            int iSocket, 
            string sIPFrom, 
            string sIPTo, 
            byte[] bSendBuff, 
            int SendCount, 
            Operate.PacketConfig.Packet.PacketType ptType)
        {
            try
            {
                if (this.cbProgressionPosition.Checked)
                {
                    int iCarryCount = 0;
                    int iIndex = (int)this.nudProgressionPosition.Value;
                    int iStep = (int)this.nudProgressionStep.Value;

                    byte bValue = bSendBuff[iIndex];
                    bValue = Operate.SystemConfig.GetStepByte(bValue, iStep, out iCarryCount);
                    bSendBuff[iIndex] = bValue;

                    if (this.cbProgressionCarry.Checked && iCarryCount > 0)
                    {
                        for (int i = 0; i < this.nudProgressionCarry.Value; i++)
                        {
                            int iIndexPre = iIndex - (i + 1);

                            if (iIndexPre > -1)
                            {
                                byte bValuePrev = bSendBuff[iIndexPre];
                                bValuePrev = Operate.SystemConfig.GetStepByte(bValuePrev, iCarryCount, out iCarryCount);
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

                bool bSendOK = bSendOK = Operate.PacketConfig.Packet.SendPacket(iSocket, ptType, sIPFrom, sIPTo, bSendBuff);
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

        #region//停止

        private void bStop_Click(object sender, EventArgs e)
        {
            this.StopSend();
        }

        private void StopSend()
        {
            if (this.bgwSendPacket.IsBusy)
            {
                this.bgwSendPacket.CancelAsync();
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hbPacketEdit.ByteProvider != null)
                {
                    DynamicByteProvider dbp = hbPacketEdit.ByteProvider as DynamicByteProvider;
                    if (dbp != null)
                    {
                        dbp.ApplyChanges();
                        byte[] bNewBuff = dbp.Bytes.ToArray();

                        switch (Operate.SystemConfig.StartMode)
                        {
                            case Operate.SystemConfig.SystemMode.Process:

                                this.packetInfo.PacketSocket = ((int)this.nudPacketSocket.Value);
                                this.packetInfo.PacketBuffer = bNewBuff;
                                this.packetInfo.PacketLen = bNewBuff.Length;
                                this.packetInfo.PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bNewBuff, Operate.PacketConfig.Packet.PacketData_MaxLen);

                                break;

                            case Operate.SystemConfig.SystemMode.Proxy:

                                this.proxyInfo.PacketSocket = ((int)this.nudPacketSocket.Value);
                                this.proxyInfo.PacketBuffer = bNewBuff;
                                this.proxyInfo.PacketLen = bNewBuff.Length;
                                this.proxyInfo.PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bNewBuff, Operate.PacketConfig.Packet.PacketData_MaxLen);

                                break;
                        }                        

                        switch (Operate.SystemConfig.StartMode)
                        {
                            case Operate.SystemConfig.SystemMode.Process:

                                ((InterfaceInfo.IInjectMode)form).RefreshPacketData();

                                break;

                            case Operate.SystemConfig.SystemMode.Proxy:

                                ((InterfaceInfo.IProxyMode)form).RefreshProxyData();

                                break;
                        }

                        this.Close();
                    }
                    else
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "封包数据为空", TType.Error)
                        {
                            LocalizationText = "PacketEditForm.Packet.Empty",
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                AntdUI.Message.open(new AntdUI.Message.Config(this, "封包保存出错", TType.Error)
                {
                    LocalizationText = "PacketEditForm.Save.Error",
                });
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

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using WPELibrary.Lib;
using EasyHook;
using Be.Windows.Forms;
using static WPELibrary.Lib.Socket_Cache.SocketPacket;

namespace WPELibrary
{
    public partial class Socket_Form : Form
    {
        private WinSockHook ws = new WinSockHook();

        private int Select_Index = -1;
        private int Search_Index = -1;
        private int Max_DataLen = 60;
        private int FilterMAXNum = 3;
        private bool bWakeUp = true;        
        public bool Search_SocketList = false;
        
        private ToolTip tt = new ToolTip();

        #region//加载窗体

        public Socket_Form(string sLanguage)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(sLanguage);
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }
        }        

        private void DLL_Form_Load(object sender, EventArgs e)
        {
            try
            {                
                this.InitSocketDGV();
                this.InitSocketForm();
                this.CleanUp_Conversion();
                this.HexBox_ManageAbility();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//窗体关闭

        private void DLL_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ws.ExitHook();       
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化窗体

        private void InitSocketForm()
        {
            try
            {
                Process pProcess = Process.GetCurrentProcess();
                Socket_Operation.InitProcessWinSockSupport();                

                string sProcessName = string.Format("{0}{1} [{2}]", MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_20), pProcess.ProcessName, RemoteHooking.GetCurrentProcessId());                                
                this.tsslProcessName.Text = sProcessName;

                string sProcessInfo = string.Format("{0} 句柄: {1}", pProcess.MainWindowTitle, pProcess.MainWindowHandle.ToString());
                this.tsslProcessInfo.Text = sProcessInfo;

                string sWinSock = "WinSock";
                if (Socket_Cache.Support_WS1)
                {
                    sWinSock += " 1.1";
                }

                if (Socket_Cache.Support_WS2)
                {
                    sWinSock += " 2.0";
                }
                this.tsslWinSock.Text = sWinSock;                

                tt.SetToolTip(bSearch, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_25));
                tt.SetToolTip(bSearchNext, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_26));

                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;
                this.tSocketInfo.Enabled = true;
                this.tslPacketLen.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), 0);

                Socket_Cache.SendList.InitSendList();
                Socket_Cache.FilterList.InitFilterList(FilterMAXNum);

                DefaultByteCharConverter defConverter = new DefaultByteCharConverter();
                EbcdicByteCharProvider ebcdicConverter = new EbcdicByteCharProvider();
                tscbEncoding.Items.Add(defConverter);
                tscbEncoding.Items.Add(ebcdicConverter);          
                tscbEncoding.SelectedIndex = 0;
                tscbPerLine.SelectedIndex = 1;                

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sProcessName);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化数据表

        private void InitSocketDGV()
        {
            try
            {
                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstRecLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
                Socket_Cache.LogList.RecSocketLog += new Socket_Cache.LogList.SocketLogReceived(Event_RecSocketLog);

                dgvSocketList.AutoGenerateColumns = false;
                dgvSocketList.DataSource = Socket_Cache.SocketList.lstRecPacket;
                dgvSocketList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketList, true, null);
                Socket_Cache.SocketList.RecSocketPacket += new Socket_Cache.SocketList.SocketPacketReceived(Event_RecSocketPacket);

                dgvFilterList.AutoGenerateColumns = false;
                dgvFilterList.DataSource = Socket_Cache.FilterList.lstFilter;
                dgvFilterList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterList, true, null);
                                
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_21));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                Socket_Operation.ShowMessageBox(ex.Message);
            }            
        }

        #endregion

        #region//设置拦截参数
        private void SetSocketParam()
        {
            try
            {  
                Socket_Cache.Hook_Send = cbHook_Send.Checked;
                Socket_Cache.Hook_SendTo = cbHook_SendTo.Checked;
                Socket_Cache.Hook_Recv = cbHook_Recv.Checked;
                Socket_Cache.Hook_RecvFrom = cbHook_RecvFrom.Checked;
                Socket_Cache.Hook_WSASend = cbHook_WSASend.Checked;
                Socket_Cache.Hook_WSASendTo = cbHook_WSASendTo.Checked;
                Socket_Cache.Hook_WSARecv = cbHook_WSARecv.Checked;
                Socket_Cache.Hook_WSARecvFrom = cbHook_WSARecvFrom.Checked;
                
                Socket_Cache.Check_Socket = cbCheck_Socket.Checked;
                Socket_Cache.Check_IP = cbCheck_IP.Checked;
                Socket_Cache.Check_Packet = cbCheck_Packet.Checked;
                Socket_Cache.Check_Size = cbCheck_Size.Checked;

                Socket_Cache.txtCheck_Socket = this.txtCheck_Socket.Text.Trim();
                Socket_Cache.txtCheck_IP = this.txtCheck_IP.Text.Trim();
                Socket_Cache.txtCheck_Packet = this.txtCheck_Packet.Text.Trim();
                Socket_Cache.txtCheck_Size_From = this.nudCheck_Size_From.Value;
                Socket_Cache.txtCheck_Size_To = this.nudCheck_Size_To.Value;                

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_22));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }
        #endregion

        #region//清空数据

        private void bCleanUp_Click(object sender, EventArgs e)
        {
            this.CleanUp_SocketList();
            this.CleanUp_HexBox();
            this.CleanUp_Conversion();
        }

        private void CleanUp_Conversion()
        {
            try
            {
                this.lPacketDataPosition.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_24), 0, 0, 0);
                this.lBits_Value.Text = string.Empty;
                this.lByte_Value.Text = string.Empty;
                this.lShort_Value.Text = string.Empty;
                this.lUShort_Value.Text = string.Empty;
                this.lInt32_Value.Text = string.Empty;
                this.lUInt32_Value.Text = string.Empty;
                this.lInt64_Value.Text = string.Empty;
                this.lUInt64_Value.Text = string.Empty;
                this.lFloat_Value.Text = string.Empty;
                this.lDouble_Value.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_SocketList()
        {
            try
            {
                this.Select_Index = -1;
                this.dgvSocketList.Rows.Clear();
                Socket_Cache.SocketQueue.ResetSocketQueue();

                Socket_Cache.SocketQueue.Filter_CNT = 0;
                Socket_Cache.SocketQueue.Send_CNT = 0;
                Socket_Cache.SocketQueue.Recv_CNT = 0;
                Socket_Cache.SocketQueue.SendTo_CNT = 0;
                Socket_Cache.SocketQueue.RecvFrom_CNT = 0;
                Socket_Cache.SocketQueue.WSASend_CNT = 0;
                Socket_Cache.SocketQueue.WSARecv_CNT = 0;
                Socket_Cache.SocketQueue.WSASendTo_CNT = 0;
                Socket_Cache.SocketQueue.WSARecvFrom_CNT = 0;
                Socket_Cache.SocketQueue.Total_SendBytes = 0;
                Socket_Cache.SocketQueue.Total_RecvBytes = 0;
            }
            catch(Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_HexBox()
        {
            if (hbPacketData.ByteProvider != null)
            {
                IDisposable byteProvider = hbPacketData.ByteProvider as IDisposable;

                if (byteProvider != null)
                {
                    byteProvider.Dispose();
                }

                hbPacketData.ByteProvider = null;
            }
            
            this.tslPacketLen.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), 0);
            this.HexBox_ManageAbility();
        }

        #endregion

        #region//开始拦截

        private void bStartHook_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetSocketParam();

                this.tlpFilterSet.Enabled = false;                                
                this.gbHookType.Enabled = false;
                this.bStartHook.Enabled = false;
                this.bStopHook.Enabled = true;

                ws.StartHook();

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }
                                
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//结束拦截

        private void bStopHook_Click(object sender, EventArgs e)
        {
            try
            {
                this.tlpFilterSet.Enabled = true;
                this.gbHookType.Enabled = true;
                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;

                ws.StopHook();

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_36));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        #endregion

        #region//计时器

        private void tSocketInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                int iQueue_CNT = Socket_Cache.SocketQueue.qSocket_PacketInfo.Count;
                int iFilter_CNT = Socket_Cache.SocketQueue.Filter_CNT;
                int iSend_CNT = Socket_Cache.SocketQueue.Send_CNT;
                int iRecv_CNT = Socket_Cache.SocketQueue.Recv_CNT;
                int iSendTo_CNT = Socket_Cache.SocketQueue.SendTo_CNT;
                int iRecvFrom_CNT = Socket_Cache.SocketQueue.RecvFrom_CNT;
                int iWSASend_CNT = Socket_Cache.SocketQueue.WSASend_CNT;
                int iWSARecv_CNT = Socket_Cache.SocketQueue.WSARecv_CNT;
                int iWSASendTo_CNT = Socket_Cache.SocketQueue.WSASendTo_CNT;
                int iWSARecvFrom_CNT = Socket_Cache.SocketQueue.WSARecvFrom_CNT;
                int iAll_CNT = iSend_CNT + iRecv_CNT + iSendTo_CNT + iRecvFrom_CNT + iWSASend_CNT + iWSARecv_CNT + iWSASendTo_CNT + iWSARecvFrom_CNT;
                int iTotal_SendBytes = Socket_Cache.SocketQueue.Total_SendBytes;
                int iTotal_RecvBytes = Socket_Cache.SocketQueue.Total_RecvBytes;

                this.tlALL_CNT.Text = iAll_CNT.ToString();
                this.tlQueue_CNT.Text = iQueue_CNT.ToString();
                this.tlSend_CNT.Text = iSend_CNT.ToString();
                this.tlRecv_CNT.Text = iRecv_CNT.ToString();
                this.tlSendTo_CNT.Text = iSendTo_CNT.ToString();
                this.tlRecvFrom_CNT.Text = iRecvFrom_CNT.ToString();
                this.tlWSASend_CNT.Text = iWSASend_CNT.ToString();
                this.tlWSARecv_CNT.Text = iWSARecv_CNT.ToString();
                this.tlWSASendTo_CNT.Text = iWSASendTo_CNT.ToString();
                this.tlWSARecvFrom_CNT.Text = iWSARecvFrom_CNT.ToString();
                this.tlFilter_CNT.Text = iFilter_CNT.ToString();
                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), iTotal_SendBytes.ToString(), iTotal_RecvBytes.ToString());                

                this.dgvFilterList.Refresh();

                if (!bgwSocketList.IsBusy)
                {
                    if (Socket_Cache.SocketQueue.qSocket_PacketInfo.Count > 0)
                    {
                        bgwSocketList.RunWorkerAsync();
                    }
                }

                if (!bgwLogList.IsBusy)
                {
                    if (Socket_Cache.LogQueue.qSocket_Log.Count > 0)
                    {
                        bgwLogList.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//右键菜单

        #region//封包列表菜单

        private void cmsSocketList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsSocketList.Close();

            try
            {
                switch (sItemText)
                {
                    case "tsmiShowBatchSend":

                        #region//查看发送列表

                        if (Socket_Cache.SendList.bShow_SendListForm)
                        {
                            Socket_SendListForm sslForm = new Socket_SendListForm();
                            sslForm.Show();
                        };

                        #endregion

                        break;

                    case "tsmiBatchSend":

                        #region//添加到发送列表

                        if (Select_Index > -1)
                        {
                            Socket_Cache.SendList.AddSendList_BySocketListIndex(Select_Index);

                            if (Socket_Cache.SendList.bShow_SendListForm)
                            {
                                Socket_SendListForm sslForm = new Socket_SendListForm();
                                sslForm.Show();
                            };
                        }

                        #endregion

                        break;

                    case "tsmiAddToFilter":

                        #region//添加到滤镜列表

                        if (Select_Index > -1)
                        {
                            int iIndex = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketIndex;
                            string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + iIndex.ToString() + "]";
                            Socket_FilterInfo.FilterMode FMode = Socket_FilterInfo.FilterMode.Normal;                            
                            Socket_FilterInfo.StartFrom FStartFrom = Socket_FilterInfo.StartFrom.Head;
                            int iFModifyCNT = 1;
                            byte[] bBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer;
                            string sData = Socket_Operation.BytesToString(EncodingFormat.Hex, bBuffer);
                            string sFSearch = Socket_Operation.GetFilterString_ByHEX(sData);
                            int iFSearchLen = bBuffer.Length;

                            Socket_Cache.FilterList.AddFilter_New(sFName, FMode, FStartFrom, iFModifyCNT, sFSearch, iFSearchLen, "", iFSearchLen, false);

                            Socket_Operation.ShowMessageBox(String.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_27), sFName));
                        }  

                        #endregion

                        break;

                    case "tsmiSend":

                        #region//发送

                        if (Select_Index > -1)
                        {
                            bgwSendFrom.RunWorkerAsync();
                        }

                        #endregion

                        break;

                    case "tsmiUseSocket":

                        #region//使用此套接字

                        if (Select_Index > -1)
                        {
                            Socket_Cache.SendList.UseSocket = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketSocket;
                        }

                        #endregion

                        break;

                    case "tsmiSaveSocketInfo":

                        #region//导出到Excel                        

                        if (dgvSocketList.Rows.Count > 0)
                        {
                            Socket_Operation.SaveSocketListToExcel();
                        }                        

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            } 
        }

        #endregion

        #region//日志菜单

        private void cmsLogList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsLogList.Close();

            try
            {
                switch (sItemText)
                {
                    case "tslToExcel":

                        #region//导出到Excel

                        if (dgvLogList.Rows.Count > 0)
                        {
                            Socket_Operation.SaveLogListToExcel();
                        }                        

                        #endregion

                        break;

                    case "tslClearList":

                        #region//清空此列表

                        if (dgvLogList.Rows.Count > 0)
                        {
                            Socket_Cache.LogQueue.ResetLogQueue();
                            this.dgvLogList.Rows.Clear();
                        }                        

                        #endregion

                        break;                   
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//滤镜菜单

        private void cmsFilterList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.dgvFilterList.EndEdit();
            string sItemText = e.ClickedItem.Name;            

            cmsFilterList.Close();

            try
            {
                switch (sItemText)
                {
                    case "tsmiShowFilter":

                        #region//查看选中滤镜

                        try
                        {
                            if (dgvFilterList.Rows.Count > 0)
                            {                                
                                int iFNum = int.Parse(this.dgvFilterList.CurrentRow.Cells["cFNum"].Value.ToString().Trim());                                

                                if (iFNum > 0)
                                {
                                    Socket_FilterForm fFilterForm = new Socket_FilterForm(iFNum);
                                    fFilterForm.Show();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }

                        #endregion

                        break;

                    case "tsmiSaveFilterList":

                        #region//保存此列表数据                        

                        if (dgvFilterList.Rows.Count > 0)
                        {
                            Socket_Operation.SaveDialog_FilterList();
                        }

                        #endregion

                        break;

                    case "tsmiLoadFilterList":

                        #region//加载滤镜列表

                        Socket_Operation.LoadDialog_FilterList();

                        #endregion

                        break;

                    case "tsmiAddFilter":

                        #region//添加新滤镜

                        Socket_Cache.FilterList.AddFilter_New();
                        Socket_Operation.SaveFilterList("");

                        #endregion

                        break;

                    case "tsmiDeleteFilter":

                        #region//删除选中滤镜

                        try
                        {
                            if (dgvFilterList.Rows.Count > 0)
                            {
                                DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_37));

                                if (dr.Equals(DialogResult.OK))
                                {
                                    int iFNum = int.Parse(this.dgvFilterList.CurrentRow.Cells["cFNum"].Value.ToString().Trim());

                                    if (iFNum > 0)
                                    {
                                        Socket_Cache.FilterList.DeleteFilter_ByFilterNum(iFNum);
                                        Socket_Operation.SaveFilterList("");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                        
                        #endregion

                        break;

                    case "tsmiClearAll":

                        #region//清除所有滤镜

                        if (dgvFilterList.Rows.Count > 0)
                        {                            
                            DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_38));

                            if (dr.Equals(DialogResult.OK))
                            {
                                Socket_Cache.FilterList.FilterListClear();
                                Socket_Operation.SaveFilterList("");
                            }
                        }                        

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #endregion        

        #region//查找封包（异步）

        private void bSearch_Click(object sender, EventArgs e)
        {
            this.ShowFindForm();

            if (Socket_Cache.DoSearch)
            {
                this.bSearchNext.Focus();
                this.SearchSocketListNext();
            }
        }

        private void bSearchNext_Click(object sender, EventArgs e)
        {
            this.SearchSocketListNext();
        }

        private void SearchSocketListNext()
        {
            try
            {
                if (dgvSocketList.Rows.Count > 0)
                {
                    if (Socket_Cache.FindOptions.IsValid)
                    {
                        string sSearch_Text = string.Empty;
                        string sSearch_Type = string.Empty;

                        FindType fType = Socket_Cache.FindOptions.Type;

                        EncodingFormat efFormat = new EncodingFormat();

                        switch (fType)
                        {
                            case FindType.Text:
                                efFormat = EncodingFormat.UTF7;
                                sSearch_Text = Socket_Cache.FindOptions.Text;
                                break;

                            case FindType.Hex:
                                efFormat = EncodingFormat.Hex;
                                byte[] bSearch_Hex = Socket_Cache.FindOptions.Hex;
                                sSearch_Text = Socket_Operation.BytesToString(EncodingFormat.Hex, bSearch_Hex);
                                break;
                        }

                        if (rbFromHead.Checked)
                        {
                            Search_Index = 0;
                            this.rbFromIndex.Checked = true;
                        }

                        int iIndex = Socket_Operation.FindSocketList(efFormat, Search_Index, sSearch_Text, Socket_Cache.FindOptions.MatchCase);

                        if (iIndex >= 0)
                        {
                            this.dgvSocketList.Rows[iIndex].Selected = true;
                            this.dgvSocketList.CurrentCell = dgvSocketList.Rows[iIndex].Cells[0];

                            this.HexBox_FindNext(true);
                        }
                        else
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_23));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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

        private void bgwSearchPacketData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = hbPacketData.Find(Socket_Cache.FindOptions);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearchPacketData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                long res = (long)e.Result;

                if (res == -1)
                {
                    if (Search_SocketList)
                    {
                        Search_Index += 1;
                        this.SearchSocketListNext();
                    }
                    else
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_23));
                    }
                }

                this.HexBox_ManageAbilityForCopyAndPaste();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示封包列表（异步）

        private void bgwSocketList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Socket_Cache.SocketList.SocketToList(Max_DataLen);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwLogList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Socket_Cache.LogList.LogToList();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void Event_RecSocketPacket(Socket_PacketInfo spi)
        {
            try
            {
                dgvSocketList.Invoke(new MethodInvoker(delegate
                {
                    Socket_Cache.SocketList.lstRecPacket.Add(spi);
                }));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Event_RecSocketLog(Socket_LogInfo sli)
        {
            try
            {
                if (!IsDisposed)
                {
                    dgvLogList.Invoke(new MethodInvoker(delegate
                    {
                        Socket_Cache.LogList.lstRecLog.Add(sli);
                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSocketList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvSocketList.Columns["cTypeImg"].Index)
                {
                    Socket_Cache.SocketPacket.SocketType stType = (Socket_Cache.SocketPacket.SocketType)dgvSocketList.Rows[e.RowIndex].Cells["cType"].Value;
                    e.Value = Socket_Operation.GetPacketTypeImg(stType);
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示封包数据        

        private void dgvSocketInfo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvSocketList.SelectedRows.Count == 1)
                {
                    Select_Index = dgvSocketList.SelectedRows[0].Index;

                    Search_Index = Select_Index;

                    if (Select_Index < Socket_Cache.SocketList.lstRecPacket.Count)
                    {
                        byte[] bSelected = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer;

                        DynamicByteProvider dbp = new DynamicByteProvider(bSelected);
                        dbp.Changed += new EventHandler(ByteProvider_Changed);
                        dbp.LengthChanged += new EventHandler(ByteProvider_LengthChanged);
                        hbPacketData.ByteProvider = dbp;                        

                        this.HexBox_LinePositionChanged();                        
                        this.UpdatePacketLenStatus();
                        this.HexBox_ManageAbility();
                    }                  
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion                

        #region//打开发送窗体（异步）

        private void bgwSendFrom_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Socket_SendForm ssForm = new Socket_SendForm(Select_Index);
                e.Result = ssForm;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSendFrom_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Socket_SendForm ssForm = e.Result as Socket_SendForm;
                ssForm.Show();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//滤镜列表操作

        private void dgvFilterList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvFilterList.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    bool bCheck = !bool.Parse(dgvFilterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    dgvFilterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bCheck;

                    int FIndex = e.RowIndex;
                    int FNum = Socket_Cache.FilterList.GetFilterNum_ByFilterIndex(FIndex);

                    Socket_Cache.FilterList.SetIsCheck_ByFilterNum(FNum, bCheck);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }




        #endregion

        #region//封包编辑器

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
            UpdatePacketLenStatus();
        }

        private void UpdatePacketLenStatus()
        {
            if (this.hbPacketData.ByteProvider == null)
            {
                this.tslPacketLen.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), 0);
            }
            else
            {
                this.tslPacketLen.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), this.hbPacketData.ByteProvider.Length);
            }
        }

        private void HexBox_LinePositionChanged()
        {
            try
            {
                string bitPresentation = string.Empty;
                string sConversion = string.Empty;                

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

                        this.lPacketDataPosition.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_24), hbPacketData.CurrentLine, hbPacketData.CurrentPositionInLine, hbPacketData.SelectionStart);

                        this.lBits_Value.Text = bitInfo.ToString();                        
                        this.lByte_Value.Text = Socket_Operation.BytesToString(EncodingFormat.Byte, buffer64);
                        this.lShort_Value.Text = Socket_Operation.BytesToString(EncodingFormat.Short, buffer64);
                        this.lUShort_Value.Text = Socket_Operation.BytesToString(EncodingFormat.UShort, buffer64);
                        this.lInt32_Value.Text = Socket_Operation.BytesToString(EncodingFormat.Int32, buffer64);
                        this.lUInt32_Value.Text = Socket_Operation.BytesToString(EncodingFormat.UInt32, buffer64);
                        this.lInt64_Value.Text = Socket_Operation.BytesToString(EncodingFormat.Int64, buffer64);
                        this.lUInt64_Value.Text = Socket_Operation.BytesToString(EncodingFormat.UInt64, buffer64);
                        this.lFloat_Value.Text = Socket_Operation.BytesToString(EncodingFormat.Float, buffer64);
                        this.lDouble_Value.Text = Socket_Operation.BytesToString(EncodingFormat.Double, buffer64);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包编辑器功能按钮

        #region//保存

        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hbPacketData.ByteProvider != null)
                {
                    DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;

                    byte[] bNewBuff = dbp.Bytes.ToArray();
                    int iNewLen = bNewBuff.Length;
                    string sNewPacketData_Hex = Socket_Operation.GetPacketData_Hex(bNewBuff, Max_DataLen);

                    Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer = bNewBuff;
                    Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketData = sNewPacketData_Hex;
                    Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketLen = iNewLen;

                    dbp.ApplyChanges();

                    this.dgvSocketList.Refresh();
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

        #region//剪切

        private void tsbCut_Click(object sender, EventArgs e)
        {
            this.hbPacketData.Cut();
        }

        #endregion

        #region//复制

        private void tsbCopy_ButtonClick(object sender, EventArgs e)
        {
            this.hbPacketData.Copy();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            this.hbPacketData.Copy();
        }

        private void tsmiCopyHex_Click(object sender, EventArgs e)
        {
            this.hbPacketData.CopyHex();
        }

        #endregion

        #region//粘贴

        private void tsbPaste_ButtonClick(object sender, EventArgs e)
        {
            this.hbPacketData.Paste();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            this.hbPacketData.Paste();
        }

        private void tsmiPasteHex_Click(object sender, EventArgs e)
        {
            this.hbPacketData.PasteHex();            
        }

        #endregion

        #region//查找

        private void tsbFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowFindForm();

                if (Socket_Cache.DoSearch)
                {
                    this.HexBox_FindNext(false);
                }  
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }      

        #endregion

        #region//查找下一个

        private void tsbFindNext_Click(object sender, EventArgs e)
        {
            this.HexBox_FindNext(false);
        }

        private void HexBox_FindNext(bool bSearchFrom_SocketList)
        {
            try
            {
                if (Socket_Cache.FindOptions.IsValid)
                {
                    Search_SocketList = bSearchFrom_SocketList;

                    if (!bgwSearchPacketData.IsBusy)
                    {
                        bgwSearchPacketData.RunWorkerAsync();
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
            hbPacketData.ByteCharConverter = tscbEncoding.SelectedItem as IByteCharConverter;

            this.hbPacketData.Focus();
        }

        #endregion

        #region//排列

        private void tscbPerLine_SelectedIndexChanged(object sender, EventArgs e)
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

        #endregion

        #region//可用性

        private void HexBox_ManageAbility()
        {
            try
            {
                if (hbPacketData.ByteProvider == null)
                {
                    tsbSave.Enabled = false;

                    tsbFind.Enabled = false;
                    tsbFindNext.Enabled = false;
                    tscbEncoding.Enabled = false;
                    tscbPerLine.Enabled = false;
                }
                else
                {
                    tsbSave.Enabled = hbPacketData.ByteProvider.HasChanges();

                    tsbFind.Enabled = true;
                    tsbFindNext.Enabled = true;
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
                if (!this.bgwSearchPacketData.IsBusy)
                {
                    tsbCopy.Enabled = hbPacketData.CanCopy();
                    tsbCut.Enabled = hbPacketData.CanCut();
                    tsbPaste.Enabled = hbPacketData.CanPaste();
                    tsmiPasteHex.Enabled = hbPacketData.CanPasteHex();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #endregion
    }
}

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using WPELibrary.Lib;
using EasyHook;
using Be.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace WPELibrary
{
    public partial class Socket_Form : Form
    {
        private WinSockHook ws = new WinSockHook();

        private int Select_Index = -1;
        private int Search_Index = -1;
        private bool bWakeUp = true;        

        private Color col_Del = Color.Red;
        private Color col_Add = Color.Green;

        private ToolTip tt = new ToolTip();

        #region//加载窗体

        public Socket_Form(string sLanguage)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(sLanguage);
                InitializeComponent();

                this.InitSocketForm();
                this.InitSocketDGV();
                this.InitHexBox();

                Socket_Cache.SendList.InitSendList();
                Socket_Cache.FilterList.InitFilterList(Socket_Cache.FilterList.Filter_MaxNum);                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }
        }

        #endregion

        #region//窗体事件

        private void DLL_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ExitMainForm();
        }

        private void Socket_Form_Resize(object sender, EventArgs e)
        {
            try
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void niWPE_Click(object sender, EventArgs e)
        {
            try
            {
                if (((MouseEventArgs)e).Button == MouseButtons.Left)
                {
                    this.ShowMainForm();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ShowMainForm()
        {
            try
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ExitMainForm()
        {
            try
            {
                ws.ExitHook();

                Socket_Operation.SaveFilterList(string.Empty);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化

        private void InitSocketForm()
        {
            try
            {
                Process pProcess = Process.GetCurrentProcess();
                Socket_Operation.InitProcessWinSockSupport();                

                string sProcessName = string.Format("{0}{1} [{2}]", MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_20), pProcess.ProcessName, RemoteHooking.GetCurrentProcessId());                                
                this.tsslProcessName.Text = sProcessName;
                this.niWPE.Text = "Winsock Packet Editor" + "\r\n" + sProcessName;

                string sMainWindowTitle = pProcess.MainWindowTitle;
                string sMainWindowHandle = pProcess.MainWindowHandle.ToString();
                string sProcessInfo = string.Empty;

                if (String.IsNullOrEmpty(sMainWindowTitle))
                {
                    sProcessInfo = pProcess.MainModule.ModuleName;
                }
                else
                {
                    sProcessInfo = string.Format("{0} 句柄: {1}", pProcess.MainWindowTitle, pProcess.MainWindowHandle.ToString());
                }
                
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
                this.cmsIcon_StartHook.Enabled = true;
                this.cmsIcon_StopHook.Enabled = false;
                this.tSocketInfo.Enabled = true;

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sProcessName);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitHexBox()
        {
            try
            {  
                this.hbXOR_From.ByteProvider = new DynamicByteProvider(new byte[0]);
                this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);
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
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
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
                
                Socket_Cache.Check_NotShow = rbFilterSet_Function_NotShow.Checked;
                Socket_Cache.Check_Socket = cbCheck_Socket.Checked;
                Socket_Cache.Check_IP = cbCheck_IP.Checked;
                Socket_Cache.Check_Port = cbCheck_Port.Checked;
                Socket_Cache.Check_Packet = cbCheck_Packet.Checked;
                Socket_Cache.Check_Size = cbCheck_Size.Checked;

                Socket_Cache.txtCheck_Socket = this.txtCheck_Socket.Text.Trim();
                Socket_Cache.txtCheck_IP = this.txtCheck_IP.Text.Trim();
                Socket_Cache.txtCheck_Port = this.txtCheck_Port.Text.Trim();
                Socket_Cache.txtCheck_Packet = this.txtCheck_Packet.Text.Trim();
                Socket_Cache.txtCheck_Size_From = this.nudCheck_Size_From.Value;
                Socket_Cache.txtCheck_Size_To = this.nudCheck_Size_To.Value;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        #endregion        

        #region//检测过滤参数输入

        private void txtCheck_Socket_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Socket_Operation.CheckTextInput_IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtCheck_Packet_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Socket_Operation.CheckTextInput_IsHex(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtCheck_IP_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Socket_Operation.CheckTextInput_IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtCheck_Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Socket_Operation.CheckTextInput_IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
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
            this.CleanUp_MainForm();
        }

        private void CleanUp_MainForm()
        {
            this.CleanUp_SocketList();
            this.CleanUp_HexBox();         
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
        }

        private void CleanUp_Comparison()
        {
            try
            {
                this.rtbComparison_A.Clear();
                this.rtbComparison_B.Clear();
                this.rtbComparison_Result.Clear();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//开始拦截

        private void bStartHook_Click(object sender, EventArgs e)
        {
            this.StartHook_MainForm();
        }

        private void StartHook_MainForm()
        {
            try
            {
                this.SetSocketParam();

                this.tlpFilterSet.Enabled = false;
                this.gbHookType.Enabled = false;
                this.bStartHook.Enabled = false;
                this.bStopHook.Enabled = true;
                this.cmsIcon_StartHook.Enabled = false;
                this.cmsIcon_StopHook.Enabled = true;

                ws.StartHook();

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }
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
            this.StopHook_MainForm();                      
        }

        private void StopHook_MainForm()
        {
            try
            {
                this.tlpFilterSet.Enabled = true;
                this.gbHookType.Enabled = true;
                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;
                this.cmsIcon_StartHook.Enabled = true;
                this.cmsIcon_StopHook.Enabled = false;

                ws.StopHook();
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
                int iIntercept_CNT = Socket_Cache.SocketQueue.Intercept_CNT;
                int iSend_CNT = Socket_Cache.SocketQueue.Send_CNT;
                int iRecv_CNT = Socket_Cache.SocketQueue.Recv_CNT;
                int iSendTo_CNT = Socket_Cache.SocketQueue.SendTo_CNT;
                int iRecvFrom_CNT = Socket_Cache.SocketQueue.RecvFrom_CNT;
                int iWSASend_CNT = Socket_Cache.SocketQueue.WSASend_CNT;
                int iWSARecv_CNT = Socket_Cache.SocketQueue.WSARecv_CNT;
                int iWSASendTo_CNT = Socket_Cache.SocketQueue.WSASendTo_CNT;
                int iWSARecvFrom_CNT = Socket_Cache.SocketQueue.WSARecvFrom_CNT;                
                int iTotal_SendBytes = Socket_Cache.SocketQueue.Total_SendBytes;
                int iTotal_RecvBytes = Socket_Cache.SocketQueue.Total_RecvBytes;

                int iAll_CNT = iSend_CNT + iRecv_CNT + iSendTo_CNT + iRecvFrom_CNT + iWSASend_CNT + iWSARecv_CNT + iWSASendTo_CNT + iWSARecvFrom_CNT;

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
                this.tlIntercept_CNT.Text = iIntercept_CNT.ToString();
                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), iTotal_SendBytes.ToString(), iTotal_RecvBytes.ToString());

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

        #region//封包编辑器菜单

        private void cmsHexBox_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsHexBox.Close();

            try
            {
                switch (sItemText)
                {
                    case "cmsHexBox_Send":

                        if (Select_Index > -1)
                        {
                            Socket_Operation.ShowSendForm(Select_Index);                            
                        }

                        break;

                    case "cmsHexBox_SendList":

                        if (Select_Index > -1)
                        {
                            Socket_Cache.SendList.AddToSendList_BySocketListIndex(Select_Index);
                            Socket_Operation.ShowSendListForm();
                        }

                        break;

                    case "cmsHexBox_FilterList":

                        if (Select_Index > -1)
                        {
                            int iSelectLen = ((int)hbPacketData.SelectionLength);
                            int iStart = ((int)hbPacketData.SelectionStart);
                            int iEnd = iStart + iSelectLen;

                            byte[] bBuffer = new byte[hbPacketData.SelectionLength];

                            int iIndex = 0;
                            for (int i = iStart; i < iEnd; i++)
                            {                                
                                bBuffer[iIndex] = hbPacketData.ByteProvider.ReadByte(i);
                                iIndex++;
                            }                          

                            Socket_Cache.FilterList.AddToFilterList_BySocketListIndex(Select_Index, bBuffer);
                        }

                        break;

                    case "cmsHexBox_CopyHex":

                        this.hbPacketData.CopyHex();

                        break;

                    case "cmsHexBox_CopyText":

                        this.hbPacketData.Copy();

                        break;

                    case "cmsHexBox_SelectAll":

                        this.hbPacketData.SelectAll();

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//图标菜单

        private void cmsIcon_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsIcon.Close();

            try
            {
                switch (sItemText)
                {
                    case "cmsIcon_Show":
                        this.ShowMainForm();
                        break;

                    case "cmsIcon_StartHook":
                        this.StartHook_MainForm();
                        break;

                    case "cmsIcon_StopHook":
                        this.StopHook_MainForm();
                        break;

                    case "cmsIcon_CleanUp":
                        this.CleanUp_MainForm();
                        break;

                    case "cmsIcon_ShowSendList":
                        Socket_Operation.ShowSendListForm();
                        break;

                    case "cmsIcon_Exit":
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包列表菜单

        private void cmsSocketList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsSocketList.Close();

            try
            {
                switch (sItemText)
                {
                    case "cmsSocketList_Send":

                        if (Select_Index > -1)
                        {
                            Socket_Operation.ShowSendForm(Select_Index);
                        }

                        break;

                    case "cmsSocketList_ShowSendList":

                        Socket_Operation.ShowSendListForm();

                        break;

                    case "cmsSocketList_UseSocket":

                        if (Select_Index > -1)
                        {
                            Socket_Cache.SendList.UseSocket = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketSocket;
                        }

                        break;

                    case "cmsSocketList_ToExcel":

                        if (dgvSocketList.Rows.Count > 0)
                        {
                            Socket_Operation.SaveSocketListToExcel();
                        }

                        break;

                    case "cmsSocketList_Comparison_A":

                        if (Select_Index > -1)
                        {
                            string sPacketHex = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer);
                            this.rtbComparison_A.Text = sPacketHex;
                        }

                        break;

                    case "cmsSocketList_Comparison_B":

                        if (Select_Index > -1)
                        {
                            string sPacketHex = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer);
                            this.rtbComparison_B.Text = sPacketHex;
                        }

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
                    case "cmsLogList_ToExcel":
                        if (dgvLogList.Rows.Count > 0)
                        {
                            Socket_Operation.SaveLogListToExcel();
                        }
                        break;

                    case "cmsLogList_CleanUp":
                        if (dgvLogList.Rows.Count > 0)
                        {
                            Socket_Cache.LogQueue.ResetLogQueue();
                            this.dgvLogList.Rows.Clear();
                        }
                        break;                   
                }
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
                Socket_Cache.SocketList.SocketToList(Socket_Cache.SocketPacket.PacketData_MaxLen);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSocketList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.cbSocketList_Set_Roll.Checked)
                {
                    if (dgvSocketList.Rows.Count > 0)
                    {
                        dgvSocketList.FirstDisplayedScrollingRowIndex = dgvSocketList.RowCount - 1;
                    }
                }
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

        private void dgvSocketList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvSocketList.Columns["cTypeImg"].Index)
                {
                    Socket_Cache.SocketPacket.PacketType stType = (Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value;
                    e.Value = Socket_Operation.GetImg_BySocketType(stType);
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示日志列表（异步）

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
                        hbPacketData.ByteProvider = dbp;                     
                    }                  
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//搜索封包内容（异步）

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

        private void HexBox_FindNext()
        {
            try
            {
                if (Socket_Cache.FindOptions.IsValid)
                {
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

                        Socket_Cache.SocketPacket.EncodingFormat efFormat = new Socket_Cache.SocketPacket.EncodingFormat();

                        switch (fType)
                        {
                            case FindType.Text:
                                efFormat = Socket_Cache.SocketPacket.EncodingFormat.UTF7;
                                sSearch_Text = Socket_Cache.FindOptions.Text;
                                break;

                            case FindType.Hex:
                                efFormat = Socket_Cache.SocketPacket.EncodingFormat.Hex;
                                byte[] bSearch_Hex = Socket_Cache.FindOptions.Hex;
                                sSearch_Text = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bSearch_Hex);
                                break;
                        }

                        if (rbFromHead.Checked)
                        {
                            Search_Index = 0;
                            this.rbFromIndex.Checked = true;
                            this.hbPacketData.SelectionStart = 0;
                        }

                        int iIndex = Socket_Operation.FindSocketList(efFormat, Search_Index, sSearch_Text, Socket_Cache.FindOptions.MatchCase);

                        if (iIndex >= 0)
                        {
                            this.dgvSocketList.Rows[iIndex].Selected = true;
                            this.dgvSocketList.CurrentCell = dgvSocketList.Rows[iIndex].Cells[0];

                            this.HexBox_FindNext();
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
                    Search_Index += 1;
                    this.SearchSocketListNext();
                }
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

        private void dgvFilterList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvFilterList.Rows.Count > 0)
                {
                    int iFNum = (int)this.dgvFilterList.CurrentRow.Cells["cFNum"].Value;

                    if (iFNum > 0)
                    {
                        Socket_Operation.ShowFilterForm_Dialog(iFNum);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//滤镜功能按钮

        private void tsFilterList_Load_Click(object sender, EventArgs e)
        {
            Socket_Operation.LoadFilterList_Dialog();
        }

        private void tsFilterList_Save_Click(object sender, EventArgs e)
        {
            if (dgvFilterList.Rows.Count > 0)
            {
                Socket_Operation.SaveFilterList_Dialog();
            }
        }

        private void tsFilterList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.FilterList.AddFilter_New();
            Socket_Operation.SaveFilterList("");
        }

        private void tsFilterList_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvFilterList.Rows.Count > 0)
                {
                    int iFNum = (int)this.dgvFilterList.CurrentRow.Cells["cFNum"].Value;

                    if (iFNum > 0)
                    {
                        Socket_Operation.ShowFilterForm_Dialog(iFNum);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsFilterList_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvFilterList.Rows.Count > 0)
                {
                    int iFNum = (int)this.dgvFilterList.CurrentRow.Cells["cFNum"].Value;

                    if (iFNum > 0)
                    {
                        Socket_Operation.DeleteFilterByFilterNum_Dialog(iFNum);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsFilterList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvFilterList.Rows.Count > 0)
            {
                Socket_Operation.CleanUpFilterList_Dialog();
            }
        }

        #endregion

        #region//文本对比

        private void rtbComparison_A_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int iTextLen = rtbComparison_A.Text.Length;
                this.lComparison_A.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_33), iTextLen);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void rtbComparison_B_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int iTextLen = rtbComparison_B.Text.Length;
                this.lComparison_B.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_34), iTextLen);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bComparison_Click(object sender, EventArgs e)
        {
            try
            {
                this.rtbComparison_Result.Clear();

                string sText_A = this.rtbComparison_A.Text;
                string sText_B = this.rtbComparison_B.Text;

                if (sText_A == sText_B)
                {
                    AppendColoredText(rtbComparison_Result, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_29), Color.Blue);
                    return;
                }

                string[] linesA = sText_A.Split('\n').Select(s => s.Trim()).ToArray();
                string[] linesB = sText_B.Split('\n').Select(s => s.Trim()).ToArray();

                int la = 0;
                int lb = 0;

                while (la < linesA.Length)
                {
                    if (lb >= linesB.Length)
                    { 
                        AppendColoredText(rtbComparison_Result, linesA[la], col_Del);
                    }
                    else if (linesA[la] == linesB[lb])
                    {                        
                        AppendColoredText(rtbComparison_Result, linesA[la], rtbComparison_Result.ForeColor);
                    }
                    else
                    {  
                        if ((lb + 1 < linesB.Length) && (linesA[la] == linesB[lb + 1]))
                        {  
                            AppendColoredText(rtbComparison_Result, linesB[lb], col_Add);
                            AppendColoredText(rtbComparison_Result, "\n" + linesA[la], rtbComparison_Result.ForeColor);

                            lb++;
                        }                        
                        else if ((la + 1 < linesA.Length) && (linesA[la + 1] == linesB[lb]))
                        {  
                            AppendColoredText(rtbComparison_Result, linesA[la], col_Del);
                            AppendColoredText(rtbComparison_Result, "\n" + linesB[lb], rtbComparison_Result.ForeColor);

                            la++;
                        }                        
                        else
                        {
                            string[] wordsA = linesA[la].Split(' ').Select(s => s.Trim()).ToArray();
                            string[] wordsB = linesB[lb].Split(' ').Select(s => s.Trim()).ToArray();

                            int wa = 0;
                            int wb = 0;
                            while (wa < wordsA.Length)
                            {
                                if (wb >= wordsB.Length)
                                {  
                                    AppendColoredText(rtbComparison_Result, wordsA[wa], col_Del);
                                }
                                else if (wordsA[wa] == wordsB[wb])
                                {
                                    AppendColoredText(rtbComparison_Result, wordsA[wa], rtbComparison_Result.ForeColor);
                                }
                                else
                                {
                                    if ((wb + 1 < wordsB.Length) && (wordsA[wa] == wordsB[wb + 1]))
                                    {
                                        AppendColoredText(rtbComparison_Result, wordsB[wb], col_Add);
                                        AppendColoredText(rtbComparison_Result, " " + wordsA[wa], rtbComparison_Result.ForeColor);

                                        wb++;
                                    }                                    
                                    else if ((wa + 1 < wordsA.Length) && (wordsA[wa + 1] == wordsB[wb]))
                                    {                                        
                                        AppendColoredText(rtbComparison_Result, wordsA[wa], col_Del);
                                        AppendColoredText(rtbComparison_Result, " " + wordsB[wb], rtbComparison_Result.ForeColor);

                                        wa++;
                                    }                                    
                                    else
                                    {  
                                        AppendColoredText(rtbComparison_Result, wordsA[wa], col_Del);
                                        AppendColoredText(rtbComparison_Result, wordsB[wb], col_Add);
                                    }
                                }
                                if (wa + 1 < wordsA.Length) AppendColoredText(rtbComparison_Result, " ", rtbComparison_Result.ForeColor);

                                if ((wordsB.Length >= wordsA.Length) && (wa + 1 == wordsA.Length))
                                {  
                                    while (wb + 1 < wordsB.Length)
                                    {
                                        wb++;
                                        
                                        AppendColoredText(rtbComparison_Result, " ", rtbComparison_Result.ForeColor);
                                        AppendColoredText(rtbComparison_Result, wordsB[wb], col_Add);
                                    }
                                }

                                wa++;
                                wb++;
                            }
                        }
                    }

                    if (la + 1 < linesA.Length)
                    {
                        AppendColoredText(rtbComparison_Result, "\n", rtbComparison_Result.ForeColor);
                    } 

                    if ((linesB.Length >= linesA.Length) && (la + 1 == linesA.Length))
                    {  
                        while (lb + 1 < linesB.Length)
                        {
                            lb++;
                            
                            AppendColoredText(rtbComparison_Result, "\n", rtbComparison_Result.ForeColor);
                            AppendColoredText(rtbComparison_Result, linesB[lb], col_Add);
                        }
                    }

                    la++;
                    lb++;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void AppendColoredText(RichTextBox box, string text, Color color)
        {
            try
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = text.Length;

                if (color == col_Add)
                {
                    box.SelectionFont = new Font(box.SelectionFont, FontStyle.Underline);
                }

                if (color == col_Del)
                {
                    box.SelectionFont = new Font(box.SelectionFont, FontStyle.Strikeout);
                }

                box.SelectionColor = color;
                box.AppendText(text);

                box.SelectionFont = box.Font;
                box.SelectionColor = box.ForeColor;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bComparison_Exchange_Click(object sender, EventArgs e)
        {
            try
            {
                string sText_A = this.rtbComparison_A.Text;
                string sText_B = this.rtbComparison_B.Text;

                this.rtbComparison_A.Text = sText_B;
                this.rtbComparison_B.Text = sText_A;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bComparison_Clear_Click(object sender, EventArgs e)
        {
            this.CleanUp_Comparison();
        }

        #endregion

        #region//编码转换

        private void bPacketInfo_Encoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sEncodingText = this.rtbPacketInfo_Encoding.Text;

                string sBytes = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Bytes, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sEncodingText));
                string sANSI_GBK = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.GBK, sEncodingText));

                string sUTF7 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF7, sEncodingText));
                string sANSI_UTF7 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF7, sEncodingText));

                string sUTF8 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF8, sEncodingText));
                string sANSI_UTF8 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF8, sEncodingText));

                string sUTF16 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF16, sEncodingText));
                string sANSI_UTF16 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF16, sEncodingText));

                string sUTF32 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF32, sEncodingText));
                string sANSI_UTF32 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF32, sEncodingText));

                string sUnicode = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Unicode, sEncodingText));
                string sANSI_Unicode = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Unicode, sEncodingText));

                string sBase64 = Socket_Operation.Base64_Encoding(sEncodingText);
                string sANSI_Base64 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sBase64));

                this.txtPacketInfo_Encoding_Bytes.Text = sBytes;
                this.txtPacketInfo_Encoding_ANSIGBK.Text = sANSI_GBK;
                this.txtPacketInfo_Encoding_UTF7.Text = sUTF7;
                this.txtPacketInfo_Encoding_ANSIUTF7.Text = sANSI_UTF7;
                this.txtPacketInfo_Encoding_UTF8.Text = sUTF8;
                this.txtPacketInfo_Encoding_ANSIUTF8.Text = sANSI_UTF8;
                this.txtPacketInfo_Encoding_UTF16.Text = sUTF16;
                this.txtPacketInfo_Encoding_ANSIUTF16.Text = sANSI_UTF16;
                this.txtPacketInfo_Encoding_UTF32.Text = sUTF32;
                this.txtPacketInfo_Encoding_ANSIUTF32.Text = sANSI_UTF32;
                this.txtPacketInfo_Encoding_Unicode.Text = sUnicode;
                this.txtPacketInfo_Encoding_ANSIUnicode.Text = sANSI_Unicode;                
                this.txtPacketInfo_Encoding_base64.Text = sBase64;
                this.txtPacketInfo_Encoding_ANSIbase64.Text = sANSI_Base64;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bPacketInfo_Decoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sDecodingText = this.rtbPacketInfo_Encoding.Text;

                string sBytes = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Bytes, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_GBK = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.GBK, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF7 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF7, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF7 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF7, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF8 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF8 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF16 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF16, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF16 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF16, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF32 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF32, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF32 = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF32, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUnicode = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Unicode, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_Unicode = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Unicode, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sBase64 = Socket_Operation.Base64_Decoding(sDecodingText);
                string sANSI_Base64 = Socket_Operation.Base64_Decoding(Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText)));

                this.txtPacketInfo_Encoding_Bytes.Text = sBytes;
                this.txtPacketInfo_Encoding_ANSIGBK.Text = sANSI_GBK;
                this.txtPacketInfo_Encoding_UTF7.Text = sUTF7;
                this.txtPacketInfo_Encoding_ANSIUTF7.Text = sANSI_UTF7;
                this.txtPacketInfo_Encoding_UTF8.Text = sUTF8;
                this.txtPacketInfo_Encoding_ANSIUTF8.Text = sANSI_UTF8;
                this.txtPacketInfo_Encoding_UTF16.Text = sUTF16;
                this.txtPacketInfo_Encoding_ANSIUTF16.Text = sANSI_UTF16;
                this.txtPacketInfo_Encoding_UTF32.Text = sUTF32;
                this.txtPacketInfo_Encoding_ANSIUTF32.Text = sANSI_UTF32;
                this.txtPacketInfo_Encoding_Unicode.Text = sUnicode;
                this.txtPacketInfo_Encoding_ANSIUnicode.Text = sANSI_Unicode;               
                this.txtPacketInfo_Encoding_base64.Text = sBase64;
                this.txtPacketInfo_Encoding_ANSIbase64.Text = sANSI_Base64;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//异或计算

        private void bXOR_Click(object sender, EventArgs e)
        {
            try
            {
                this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);

                DynamicByteProvider dbpXOR_From = this.hbXOR_From.ByteProvider as DynamicByteProvider;
                byte[] blXOR_From = dbpXOR_From.Bytes.ToArray();

                string sXOR_Value = this.txtXOR.Text.Trim();
                string[] slXOR_Value = sXOR_Value.Split(' ');

                if (blXOR_From != null && blXOR_From.Length > 0 && !string.IsNullOrEmpty(sXOR_Value) && slXOR_Value.Length > 0)
                {
                    int j = 0;

                    byte[] blXOR_To = new byte[blXOR_From.Length];

                    for (int i = 0; i < blXOR_From.Length; i++)
                    {
                        if (j == slXOR_Value.Length)
                        {
                            j = 0;
                        }

                        byte bXOR_From = blXOR_From[i];

                        byte bXOR_Value = new byte();

                        bool bOK = Byte.TryParse(slXOR_Value[j], System.Globalization.NumberStyles.HexNumber, null, out bXOR_Value);

                        if (bOK)
                        {
                            blXOR_To[i] = (byte)(bXOR_From ^ bXOR_Value);

                            j++;
                        }
                        else
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_21));
                            return;
                        }
                    }

                    DynamicByteProvider dbpXOR_To = new DynamicByteProvider(blXOR_To);
                    hbXOR_To.ByteProvider = dbpXOR_To;
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtXOR_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Socket_Operation.CheckTextInput_IsHex(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bXOR_Clear_Click(object sender, EventArgs e)
        {
            this.InitHexBox();
            this.txtXOR.Clear();
        }

        #endregion
    }
}

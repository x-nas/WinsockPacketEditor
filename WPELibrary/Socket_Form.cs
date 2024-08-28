using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using WPELibrary.Lib;
using EasyHook;

namespace WPELibrary
{
    public partial class Socket_Form : Form
    {
        private WinSockHook ws = new WinSockHook();        

        private int Select_Index = -1;
        private int Max_DataLen = 50;
        private int FilterMAXNum = 3;
        private bool bWakeUp = true;

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
                LocalHook.Release();                
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
                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;
                this.tSocketInfo.Enabled = true;

                this.pbLoading.Top = (ClientRectangle.Height - pbLoading.Height) / 2;
                this.pbLoading.Left = (ClientRectangle.Width - pbLoading.Width) / 2;
                this.pbLoading.Visible = false;

                this.cbPacketInfo_Left.SelectedIndex = 0;
                this.cbPacketInfo_Right.SelectedIndex = 1;
                this.cbSearchType.SelectedIndex = 0;
                this.cbSearchOrder.SelectedIndex = 0;
                
                string sInjectInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_20) + " {0} [{1}]", Process.GetCurrentProcess().ProcessName, RemoteHooking.GetCurrentProcessId());
                this.tlSystemInfo.Text = sInjectInfo;

                Socket_Cache.SocketSendList.InitSendList();
                Socket_Cache.SocketFilterList.InitFilterList(FilterMAXNum);            

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sInjectInfo);
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
                Socket_Operation.InitPacketFormat();
                this.cbSearchType.DataSource = Socket_Operation.dtPacketFormat.Copy();
                this.cbSearchType.ValueMember = "Key";
                this.cbSearchType.DisplayMember = "Value";
                this.cbPacketInfo_Left.DataSource = Socket_Operation.dtPacketFormat.Copy();
                this.cbPacketInfo_Left.ValueMember = "Key";
                this.cbPacketInfo_Left.DisplayMember = "Value";
                this.cbPacketInfo_Right.DataSource = Socket_Operation.dtPacketFormat.Copy();
                this.cbPacketInfo_Right.ValueMember = "Key";
                this.cbPacketInfo_Right.DisplayMember = "Value";

                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstRecLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
                Socket_Cache.LogList.RecSocketLog += new Socket_Cache.LogList.SocketLogReceived(Event_RecSocketLog);

                dgvSocketList.AutoGenerateColumns = false;
                dgvSocketList.DataSource = Socket_Cache.SocketList.lstRecPacket;
                dgvSocketList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketList, true, null);
                Socket_Cache.SocketList.RecSocketPacket += new Socket_Cache.SocketList.SocketPacketReceived(Event_RecSocketPacket);

                dgvFilterList.AutoGenerateColumns = false;
                dgvFilterList.DataSource = Socket_Cache.SocketFilterList.lstFilter;
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

        #region//设置拦截选项

        //Send
        private void cbHook_Send_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_Send = this.cbHook_Send.Checked;            
        }

        //SendTo
        private void cbHook_SendTo_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_SendTo = this.cbHook_SendTo.Checked;            
        }

        //Recv
        private void cbHook_Recv_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_Recv = this.cbHook_Recv.Checked;            
        }

        //RecvFrom
        private void cbHook_RecvFrom_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_RecvFrom = this.cbHook_RecvFrom.Checked;            
        }

        //WSASend
        private void cbHook_WSASend_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_WSASend = this.cbHook_WSASend.Checked;
        }

        //WSASendTo
        private void cbHook_WSASendTo_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_WSASendTo = this.cbHook_WSASendTo.Checked;
        }

        //WSARecv
        private void cbHook_WSARecv_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_WSARecv = this.cbHook_WSARecv.Checked;
        }

        //WSARecvFrom
        private void cbHook_WSARecvFrom_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Hook_WSARecvFrom = this.cbHook_WSARecvFrom.Checked;
        }

        //过滤封包大小
        private void cbCheck_Size_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Check_Size = this.cbCheck_Size.Checked;            
        }

        //过滤套接字
        private void cbCheck_Socket_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Check_Socket = this.cbCheck_Socket.Checked;            
        }

        //过滤IP
        private void cbCheck_IP_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Check_IP = this.cbCheck_IP.Checked;            
        }

        //过滤封包数据
        private void cbCheck_Packet_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Check_Packet = this.cbCheck_Packet.Checked;            
        }

        #endregion

        #region//清空数据

        private void bClear_Click(object sender, EventArgs e)
        {
            this.ClearSocketList();
        }

        private void ClearSocketList()
        {
            try
            {
                this.Select_Index = -1;

                this.rtbPackInfo_Left.Clear();
                this.rtbPacketInfo_Right.Clear();
                this.dgvSocketList.Rows.Clear();

                Socket_Cache.SocketQueue.ResetSocketQueue();
            }
            catch(Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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
                this.tlQueue_CNT.Text = Socket_Cache.SocketQueue.qSocket_Packet.Count.ToString();
                this.tlALL_CNT.Text = (Socket_Cache.SocketQueue.Recv_CNT + Socket_Cache.SocketQueue.Send_CNT).ToString();
                this.tlRecv_CNT.Text = Socket_Cache.SocketQueue.Recv_CNT.ToString();
                this.tlSend_CNT.Text = Socket_Cache.SocketQueue.Send_CNT.ToString();                
                this.tlCheck_CNT.Text = Socket_Cache.SocketQueue.Filter_CNT.ToString();

                this.dgvFilterList.Refresh();

                if (!bgwSocketList.IsBusy)
                {
                    if (Socket_Cache.SocketQueue.qSocket_Packet.Count > 0)
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

        #region//发送列表菜单
        private void cmsSocketInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsSocketList.Close();

            try
            {
                switch (sItemText)
                {
                    case "tsmiShowBatchSend":

                        #region//查看发送列表

                        if (Socket_Cache.SocketSendList.bShow_SendListForm)
                        {
                            Socket_SendList_Form sslForm = new Socket_SendList_Form();
                            sslForm.Show();
                        };

                        #endregion

                        break;

                    case "tsmiBatchSend":

                        #region//添加到发送列表

                        if (Select_Index > -1)
                        {
                            Socket_Cache.SocketSendList.AddSendList_BySocketListIndex(Select_Index);

                            if (Socket_Cache.SocketSendList.bShow_SendListForm)
                            {
                                Socket_SendList_Form sslForm = new Socket_SendList_Form();
                                sslForm.Show();
                            };
                        }

                        #endregion

                        break;

                    case "tsmiAddToFilter":

                        #region//添加到滤镜列表

                        if (Select_Index > -1)
                        {
                            int iIndex = Socket_Cache.SocketList.lstRecPacket[Select_Index].Index;
                            string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + iIndex.ToString() + "]";
                            Socket_Filter_Info.FilterMode FMode = Socket_Filter_Info.FilterMode.Normal;                            
                            Socket_Filter_Info.StartFrom FStartFrom = Socket_Filter_Info.StartFrom.Head;
                            byte[] bBuffer = Socket_Cache.SocketList.lstRecPacket[Select_Index].Buffer;
                            string sData = Socket_Operation.ByteToString("HEX", bBuffer);
                            string sFSearch = Socket_Operation.GetFilterString_ByHEX(sData);
                            int iFSearchLen = bBuffer.Length;

                            Socket_Cache.SocketFilterList.AddFilter_New(sFName, FMode, FStartFrom, sFSearch, iFSearchLen, "", iFSearchLen, false);

                            Socket_Operation.ShowMessageBox(String.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_27), sFName));
                        }  

                        #endregion

                        break;

                    case "tsmiSend":

                        #region//发送
                        if (Select_Index > -1)
                        {
                            this.pbLoading.Visible = true;

                            bgwSendFrom.RunWorkerAsync();
                        }
                        #endregion

                        break;

                    case "tsmiUseSocket":

                        #region//使用此套接字

                        if (Select_Index > -1)
                        {
                            Socket_Cache.SocketSendList.UseSocket = Socket_Cache.SocketList.lstRecPacket[Select_Index].Socket;
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
                                    Socket_Filter_Form fFilterForm = new Socket_Filter_Form(iFNum);
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
                            Socket_Operation.SaveFilterListToFile();
                        }

                        #endregion

                        break;

                    case "tsmiLoadFilterList":

                        #region//加载滤镜列表

                        Socket_Operation.LoadFileToFilterList();

                        #endregion

                        break;

                    case "tsmiAddFilter":

                        #region//添加新滤镜

                        Socket_Cache.SocketFilterList.AddFilter_New();

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
                                        Socket_Cache.SocketFilterList.DeleteFilter_ByFilterNum(iFNum);
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
                                Socket_Cache.SocketFilterList.FilterListClear();
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

        #region//搜索按钮
        private void bSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                int iFrom = 0;
                string sSearchType = this.cbSearchType.SelectedValue.ToString();
                string sSearchData = this.txtSearchData.Text.Trim();
                int iSearchOrder = this.cbSearchOrder.SelectedIndex;

                if (!string.IsNullOrEmpty(sSearchData))
                {
                    if (iSearchOrder == 1)
                    {
                        iFrom = Select_Index + 1;
                    }

                    int iIndex = Socket_Operation.SearchSocketList(sSearchType, iFrom, sSearchData);

                    if (iIndex >= 0)
                    {
                        this.dgvSocketList.Rows[iIndex].Selected = true;
                        this.dgvSocketList.CurrentCell = dgvSocketList.Rows[iIndex].Cells[0];
                    }
                    else
                    {                        
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_39));
                    }
                }
                else
                {                    
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_40));
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

        private void Event_RecSocketPacket(Socket_Packet_Info spi)
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

        private void Event_RecSocketLog(Socket_Log_Info sli)
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

        #region//显示封包数据（异步）
        private void cbPacketInfo_Left_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Select_Index > -1 && !bgwSocketInfo.IsBusy)
            {
                this.rtbPackInfo_Left.Clear();
                this.rtbPacketInfo_Right.Clear();
                this.pbLoading.Visible = true;

                bgwSocketInfo.RunWorkerAsync();
            }
        }

        private void cbPacketInfo_Right_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Select_Index > -1 && !bgwSocketInfo.IsBusy)
            {
                this.rtbPackInfo_Left.Clear();
                this.rtbPacketInfo_Right.Clear();
                this.pbLoading.Visible = true;

                bgwSocketInfo.RunWorkerAsync();
            }
        }

        private void dgSocketInfo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvSocketList.SelectedRows.Count == 1)
                {
                    Select_Index = dgvSocketList.SelectedRows[0].Index;

                    if (Select_Index > -1 && !bgwSocketInfo.IsBusy)
                    {
                        this.rtbPackInfo_Left.Clear();
                        this.rtbPacketInfo_Right.Clear();
                        this.pbLoading.Visible = true;

                        bgwSocketInfo.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void bgwSocketInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.ShowPacketInfo();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSocketInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.pbLoading.Visible = false;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ShowPacketInfo()
        {
            try
            {
                if (Select_Index > -1)
                {
                    if (Select_Index < Socket_Cache.SocketList.lstRecPacket.Count)
                    {
                        byte[] bSelected = Socket_Cache.SocketList.lstRecPacket[Select_Index].Buffer;

                        string sKey_Left = this.cbPacketInfo_Left.SelectedValue.ToString();
                        string sLey_Right =this.cbPacketInfo_Right.SelectedValue.ToString();

                        this.rtbPackInfo_Left.Invoke((MethodInvoker)delegate { this.rtbPackInfo_Left.Text = Socket_Operation.ByteToString(sKey_Left, bSelected); });
                        this.rtbPacketInfo_Right.Invoke((MethodInvoker)delegate { this.rtbPacketInfo_Right.Text = Socket_Operation.ByteToString(sLey_Right, bSelected); });
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
                Socket_Send_Form ssForm = new Socket_Send_Form(Select_Index);
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
                this.pbLoading.Visible = false;

                Socket_Send_Form ssForm = e.Result as Socket_Send_Form;
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
                    int FNum = Socket_Cache.SocketFilterList.GetFilterNum_ByFilterIndex(FIndex);

                    Socket_Cache.SocketFilterList.SetIsCheck_ByFilterNum(FNum, bCheck);
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

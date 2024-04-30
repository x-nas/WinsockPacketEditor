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
        private int FilterMAXNum = 10;
        private bool bWakeUp = true;        
        private string sLanguage_UI = "";

        #region//加载窗体
        public Socket_Form(string sLanguage)
        {
            MultiLanguage.SetDefaultLanguage(sLanguage);
            InitializeComponent();
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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("目标进程：", "Process: ");
                string sInjectInfo = string.Format(sLanguage_UI + " {0} [{1}]", Process.GetCurrentProcess().ProcessName, RemoteHooking.GetCurrentProcessId());
                this.tlSystemInfo.Text = sInjectInfo;

                Socket_Cache.SocketSendList.InitSendList();
                Socket_Cache.SocketFilterList.InitFilterList(FilterMAXNum);             

                Socket_Operation.DoLog(sInjectInfo);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                dgvFilterList.DataSource = Socket_Cache.SocketFilterList.lstFilter;
                dgvFilterList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterList, true, null);

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("初始化数据表完成!", "Initializing data table completed!");
                Socket_Operation.DoLog(sLanguage_UI);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }
        #endregion

        #region//设置拦截参数
        private void SetSocketParam()
        {
            try
            {
                Socket_Cache.Interecept_Recv = cbInterecept_Recv.Checked;
                Socket_Cache.Interecept_RecvFrom = cbInterecept_RecvFrom.Checked;
                Socket_Cache.Interecept_Send = cbInterecept_Send.Checked;
                Socket_Cache.Interecept_SendTo = cbInterecept_SendTo.Checked;
                Socket_Cache.Display_Recv = cbDisplay_Recv.Checked;
                Socket_Cache.Display_RecvFrom = cbDisplay_RecvFrom.Checked;
                Socket_Cache.Display_Send = cbDisplay_Send.Checked;
                Socket_Cache.Display_SendTo = cbDisplay_SendTo.Checked;
                Socket_Operation.IsCheck_Size = cbCheck_Size.Checked;
                Socket_Operation.IsCheck_Socket = cbCheck_Socket.Checked;
                Socket_Operation.IsCheck_IP = cbCheck_IP.Checked;
                Socket_Operation.IsCheck_Packet = cbCheck_Packet.Checked;
                Socket_Operation.Check_Size_From = this.txtCheck_Size_From.Text.Trim();
                Socket_Operation.Check_Size_To = this.txtCheck_Size_To.Text.Trim();
                Socket_Operation.Check_Socket_txt = this.txtCheck_Socket.Text.Trim();
                Socket_Operation.Check_IP_txt = this.txtCheck_IP.Text.Trim();
                Socket_Operation.Check_Packet_txt = this.txtCheck_Packet.Text.Trim();

                if (cbReset_CNT.Checked)
                {
                    this.Select_Index = -1;

                    this.rtbHEX.Clear();
                    this.rtbDEC.Clear();
                    this.rtbBIN.Clear();
                    this.rtbUNICODE.Clear();
                    this.rtbASCII.Clear();
                    this.rtbUTF8.Clear();
                    this.rtbGB2312.Clear();
                    this.dgvSocketList.Rows.Clear();

                    Socket_Operation.CheckCNT = 0;
                    Socket_Cache.SocketQueue.ResetSocketQueue();
                }

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("设置拦截参数完成!", "Setting interception parameters completed!");
                Socket_Operation.DoLog(sLanguage_UI);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }          
        }
        #endregion        

        #region//设置拦截选项
        private void cbInterecept_Send_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_Send = this.cbInterecept_Send.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-发送, ", "Block-Send, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Interecept_Send.ToString());
        }

        private void cbInterecept_SendTo_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_SendTo = this.cbInterecept_SendTo.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-发送到, ", "Block-SendTo, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Interecept_SendTo.ToString());
        }

        private void cbInterecept_Recv_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_Recv = this.cbInterecept_Recv.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-接收, ", "Block-Recv, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Interecept_Recv.ToString());
        }

        private void cbInterecept_RecvFrom_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_RecvFrom = this.cbInterecept_RecvFrom.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-接收自, ", "Block-RecvFrom, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Interecept_RecvFrom.ToString());
        }

        private void cbDisplay_Send_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_Send = this.cbDisplay_Send.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("显示-发送, ", "Show-Send, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Display_Send.ToString());
        }

        private void cbDisplay_SendTo_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_SendTo = this.cbDisplay_SendTo.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("显示-发送到, ", "Show-SendTo, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Display_SendTo.ToString());
        }

        private void cbDisplay_Recv_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_Recv = this.cbDisplay_Recv.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("显示-接收, ", "Show-Recv, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Display_Recv.ToString());
        }

        private void cbDisplay_RecvFrom_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_RecvFrom = this.cbDisplay_RecvFrom.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("显示-接收自, ", "Show-RecvFrom, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Cache.Display_RecvFrom.ToString());
        }

        private void cbCheck_Size_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.IsCheck_Size = this.cbCheck_Size.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("过滤-封包长度, ", "Filter-SocketSize, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Operation.IsCheck_Size.ToString());
        }

        private void cbCheck_Socket_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.IsCheck_Socket = this.cbCheck_Socket.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("过滤-套接字, ", "Filter-Socket, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Operation.IsCheck_Socket.ToString());
        }

        private void cbCheck_IP_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.IsCheck_IP = this.cbCheck_IP.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("过滤-IP地址, ", "Filter-IP Address, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Operation.IsCheck_IP.ToString());
        }

        private void cbCheck_Packet_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.IsCheck_Packet = this.cbCheck_Packet.Checked;

            sLanguage_UI = MultiLanguage.GetDefaultLanguage("过滤-封包内容, ", "Filter-Socket Packet, ");
            Socket_Operation.DoLog(sLanguage_UI + Socket_Operation.IsCheck_Packet.ToString());
        }        
        #endregion

        #region//开始拦截
        private void bStartHook_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetSocketParam();

                this.bStartHook.Enabled = false;
                this.bStopHook.Enabled = true;

                ws.StartHook();

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("开始拦截！", "Start Intercepting!");
                Socket_Operation.DoLog(sLanguage_UI);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//结束拦截
        private void bStopHook_Click(object sender, EventArgs e)
        {
            try
            {
                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;

                ws.StopHook();

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("结束拦截！", "Stop Interception!");
                Socket_Operation.DoLog(sLanguage_UI);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                this.tlInterecept_CNT.Text = Socket_Cache.SocketQueue.Interecept_CNT.ToString();
                this.tlCheck_CNT.Text = Socket_Operation.CheckCNT.ToString();

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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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

                    case "tsmiShowHook":

                        #region//显示拦截日志

                        Socket_Operation.bDoLog_Hook = true;

                        #endregion

                        break;

                    case "tsmiHideHook":

                        #region//隐藏拦截日志

                        Socket_Operation.bDoLog_Hook = false;

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                            Socket_Operation.DoLog(ex.Message);
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
                                string sMessage = MultiLanguage.GetDefaultLanguage("确定删除选中的滤镜吗？", "Are you sure to delete the selected filter?");
                                DialogResult dr = Socket_Operation.ShowSelectMessageBox(sMessage);

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
                            Socket_Operation.DoLog(ex.Message);
                        }
                        
                        #endregion

                        break;

                    case "tsmiClearAll":

                        #region//清除所有滤镜

                        if (dgvFilterList.Rows.Count > 0)
                        {
                            string sMessage = MultiLanguage.GetDefaultLanguage("确定删除所有数据吗？", "Are you sure to delete all data?");
                            DialogResult dr = Socket_Operation.ShowSelectMessageBox(sMessage);

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
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #endregion

        #region//搜索按钮
        private void bSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int iFrom = 0;
                string sSearch = this.txtSearch.Text.Trim();

                if (!string.IsNullOrEmpty(sSearch))
                {
                    if (this.rbSearchFrom.Checked)
                    {
                        iFrom = Select_Index + 1;
                    }

                    int iIndex = Socket_Operation.SearchSocketListByHex(iFrom, sSearch);

                    if (iIndex >= 0)
                    {
                        this.dgvSocketList.Rows[iIndex].Selected = true;
                        this.dgvSocketList.CurrentCell = dgvSocketList.Rows[iIndex].Cells[0];
                    }
                    else
                    {
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("没有搜索到数据！", "No data found in search!");
                        Socket_Operation.ShowMessageBox(sLanguage_UI);
                    }
                }
                else
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("请输入搜索内容！", "Please enter search content!");
                    Socket_Operation.ShowMessageBox(sLanguage_UI);
                }

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("搜索完成!", "Search completed!");
                Socket_Operation.DoLog(sLanguage_UI);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion        

        #region//显示数据列表（异步）
        private void bgwSocketList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Socket_Cache.SocketList.SocketToList(Max_DataLen);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
            }
        }

        private void Event_RecSocketLog(Socket_Log_Info sli)
        {
            try
            {
                dgvLogList.Invoke(new MethodInvoker(delegate
                {
                    Socket_Cache.LogList.lstRecLog.Add(sli);
                }));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion        

        #region//显示数据方式（异步）
        private void dgSocketInfo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvSocketList.SelectedRows.Count == 1)
                {
                    Select_Index = dgvSocketList.SelectedRows[0].Index;

                    this.pbLoading.Visible = true;
                    bgwSocketInfo.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }

        private void tcPacketInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.pbLoading.Visible = true;
                bgwSocketInfo.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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

                        switch (tcPacketInfo.SelectedIndex)
                        {
                            case 0:
                                this.rtbHEX.Invoke((MethodInvoker)delegate { this.rtbHEX.Text = Socket_Operation.Byte_To_Hex(bSelected); });
                                break;
                            case 1:
                                this.rtbDEC.Invoke((MethodInvoker)delegate { this.rtbDEC.Text = Socket_Operation.Byte_To_Dec(bSelected); });
                                break;
                            case 2:
                                this.rtbBIN.Invoke((MethodInvoker)delegate { this.rtbBIN.Text = Socket_Operation.Byte_To_Bin(bSelected); });
                                break;
                            case 3:
                                this.rtbUNICODE.Invoke((MethodInvoker)delegate { this.rtbUNICODE.Text = Socket_Operation.Byte_To_Unicode(bSelected); });
                                break;
                            case 4:
                                this.rtbASCII.Invoke((MethodInvoker)delegate { this.rtbASCII.Text = Socket_Operation.Byte_To_Ascii(bSelected); });
                                break;
                            case 5:
                                this.rtbUTF8.Invoke((MethodInvoker)delegate { this.rtbUTF8.Text = Socket_Operation.Byte_To_UTF8(bSelected); });
                                break;
                            case 6:
                                this.rtbGB2312.Invoke((MethodInvoker)delegate { this.rtbGB2312.Text = Socket_Operation.Byte_To_GB2312(bSelected); });
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion        
    }
}

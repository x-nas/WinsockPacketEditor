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
        private bool bWakeUp = true;
        private bool bResetCNT = false;
        private string sLanguage = "";

        #region//加载窗体
        public Socket_Form(string sLanguage)
        {
            MultiLanguage.SetDefaultLanguage(sLanguage);            

            InitializeComponent();

            MultiLanguage.LoadLanguage(this, typeof(Socket_Form));
        }

        private void DLL_Form_Load(object sender, EventArgs e)
        {
            this.Init();

            sLanguage = MultiLanguage.GetDefaultLanguage("目标进程：", "Process: ");
            string sInjectInfo = string.Format(sLanguage + " {0} [{1}]", Process.GetCurrentProcess().ProcessName, RemoteHooking.GetCurrentProcessId());
            this.tlSystemInfo.Text = sInjectInfo;

            Socket_Cache.SocketSendList.InitSendList();
            Filter_List.InitFilterList();

            dgvSocketList.AutoGenerateColumns = false;
            dgvSocketList.DataSource = Socket_Cache.SocketList.lstRecPacket;
            dgvSocketList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketList, true, null);
            Socket_Cache.SocketList.RecSocketPacket += new Socket_Cache.SocketList.SocketPacketReceived(Event_RecSocketPacket);

            dgvLogList.AutoGenerateColumns = false;
            dgvLogList.DataSource = Socket_Cache.LogList.lstRecLog;
            dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
            Socket_Cache.LogList.RecSocketLog += new Socket_Cache.LogList.SocketLogReceived(Event_RecSocketLog);

            this.dgvFilterList.DataSource = Filter_List.dtFilterList;

            this.bStartHook.Enabled = true;
            this.bStopHook.Enabled = false;
            this.tSocketInfo.Enabled = true;

            Socket_Operation.DoLog(sInjectInfo);
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

        #region//初始化
        private void Init()
        {
            bResetCNT = cbReset_CNT.Checked;

            Socket_Cache.Interecept_Recv = cbInterecept_Recv.Checked;
            Socket_Cache.Interecept_RecvFrom = cbInterecept_RecvFrom.Checked;
            Socket_Cache.Interecept_Send = cbInterecept_Send.Checked;
            Socket_Cache.Interecept_SendTo = cbInterecept_SendTo.Checked;
            Socket_Cache.Display_Recv = cbDisplay_Recv.Checked;
            Socket_Cache.Display_RecvFrom = cbDisplay_RecvFrom.Checked;
            Socket_Cache.Display_Send = cbDisplay_Send.Checked;
            Socket_Cache.Display_SendTo = cbDisplay_SendTo.Checked;

            Socket_Operation.Check_Size = cbCheck_Size.Checked;
            Socket_Operation.Check_Socket = cbCheck_Socket.Checked;
            Socket_Operation.Check_IP = cbCheck_IP.Checked;
            Socket_Operation.Check_Packet = cbCheck_Packet.Checked;
            Socket_Operation.Check_Size_From = this.txtCheck_Size_From.Text.Trim();
            Socket_Operation.Check_Size_To = this.txtCheck_Size_To.Text.Trim();
            Socket_Operation.Check_Socket_txt = this.txtCheck_Socket.Text.Trim();
            Socket_Operation.Check_IP_txt = this.txtCheck_IP.Text.Trim();
            Socket_Operation.Check_Packet_txt = this.txtCheck_Packet.Text.Trim();

            if (bResetCNT)
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
                Socket_Cache.SocketQueue.ResetQueue();
                Socket_Cache.LogQueue.ResetQueue();
            }            
        }
        #endregion        

        #region//选择框
        private void cbInterecept_Send_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_Send = this.cbInterecept_Send.Checked;
        }

        private void cbInterecept_SendTo_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_SendTo = this.cbInterecept_SendTo.Checked;
        }

        private void cbInterecept_Recv_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_Recv = this.cbInterecept_Recv.Checked;
        }

        private void cbInterecept_RecvFrom_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Interecept_RecvFrom = this.cbInterecept_RecvFrom.Checked;
        }

        private void cbDisplay_Send_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_Send = this.cbDisplay_Send.Checked;
        }

        private void cbDisplay_SendTo_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_SendTo = this.cbDisplay_SendTo.Checked;
        }

        private void cbDisplay_Recv_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_Recv = this.cbDisplay_Recv.Checked;
        }

        private void cbDisplay_RecvFrom_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.Display_RecvFrom = this.cbDisplay_RecvFrom.Checked;
        }

        private void cbCheck_Size_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.Check_Size = this.cbCheck_Size.Checked;
        }

        private void cbCheck_Socket_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.Check_Socket = this.cbCheck_Socket.Checked;
        }

        private void cbCheck_IP_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.Check_IP = this.cbCheck_IP.Checked;
        }

        private void cbCheck_Packet_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Operation.Check_Packet = this.cbCheck_Packet.Checked;
        }

        private void cbReset_CNT_CheckedChanged(object sender, EventArgs e)
        {
            this.bResetCNT = this.cbReset_CNT.Checked;
        }
        #endregion

        #region//开始拦截
        private void bStartHook_Click(object sender, EventArgs e)
        {
            this.Init();

            this.bStartHook.Enabled = false;
            this.bStopHook.Enabled = true;

            ws.StartHook();

            if (bWakeUp)
            {
                RemoteHooking.WakeUpProcess();
                this.bWakeUp = false;
            }
        }
        #endregion

        #region//结束拦截
        private void bStopHook_Click(object sender, EventArgs e)
        {
            this.bStartHook.Enabled = true;
            this.bStopHook.Enabled = false;

            ws.StopHook();            
        }
        #endregion

        #region//计时器
        private void tSocketInfo_Tick(object sender, EventArgs e)
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
        #endregion
        
        #region//右键菜单
        private void cmsSocketInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;

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
                            Socket_Cache.SocketSendList.SendList_Add_BySocketListIndex(Select_Index);

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
                            int iSocketLen = Socket_Cache.SocketList.lstRecPacket[Select_Index].ResLen;

                            if (iSocketLen > 500)
                            {
                                sLanguage = MultiLanguage.GetDefaultLanguage("选择的封包较大，仅显示前500个数据！", "The selected package is too large, only the first 500 bit of data are displayed!");
                                MessageBox.Show(sLanguage, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            Socket_Send_Form ssForm = new Socket_Send_Form();
                            ssForm.Select_Index = Select_Index;
                            ssForm.Show();
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

                        Socket_Operation.SaveSocketListToExcel();                                                                  

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            } 
        }

        private void cmsLogList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;

            try
            {
                switch (sItemText)
                {
                    case "tslToExcel":

                        #region//导出到Excel

                        Socket_Operation.SaveLogListToExcel();

                        #endregion

                        break;

                    case "tslClearList":

                        #region//清空此列表

                        Socket_Cache.LogQueue.ResetQueue();
                        this.dgvLogList.Rows.Clear();

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

        #region//搜索按钮
        private void bSearch_Click(object sender, EventArgs e)
        {
            int iFrom = 0;
            string sSearch = this.txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(sSearch))
            {
                if (this.rbSearchAll.Checked)
                {
                    iFrom = 0;
                }
                else if (this.rbSearchFrom.Checked)
                {
                    iFrom = Select_Index + 1;
                }
                else
                { 
                    //
                }

                int iIndex = Socket_Operation.SearchSocketListByHex(iFrom, sSearch);

                if (iIndex >= 0)
                {
                    this.dgvSocketList.Rows[iIndex].Selected = true;
                    this.dgvSocketList.CurrentCell = dgvSocketList.Rows[iIndex].Cells[0];
                }
                else
                {
                    sLanguage = MultiLanguage.GetDefaultLanguage("没有搜索到数据！", "No data found in search!");
                    Socket_Operation.ShowMessageBox(sLanguage);                    
                }
            }
            else
            {
                sLanguage = MultiLanguage.GetDefaultLanguage("请输入搜索内容！", "Please enter search content!");
                Socket_Operation.ShowMessageBox(sLanguage);                
            }
        }
        #endregion

        #region//异步显示列表数据
        private void bgwSocketList_DoWork(object sender, DoWorkEventArgs e)
        {
            Socket_Cache.SocketList.SocketToList(Max_DataLen);
        }

        private void bgwLogList_DoWork(object sender, DoWorkEventArgs e)
        {
            Socket_Cache.LogList.LogToList();
        }

        private void Event_RecSocketPacket(Socket_Packet_Info si)
        {
            dgvSocketList.Invoke(new MethodInvoker(delegate
            {
                Socket_Cache.SocketList.lstRecPacket.Add(si);
            }));
        }

        private void Event_RecSocketLog(Socket_Log sl)
        {
            dgvLogList.Invoke(new MethodInvoker(delegate
            {
                Socket_Cache.LogList.lstRecLog.Add(sl);
            }));
        }
        #endregion        

        #region//数据显示方式
        private void dgSocketInfo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSocketList.SelectedRows.Count == 1)
            {
                Select_Index = dgvSocketList.SelectedRows[0].Index;
                ShowPacketInfo();
            }
        }

        private void tcPacketInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowPacketInfo();
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

        #region//滤镜列表
        private void dgvFilterList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Filter_Form fFilterForm = new Filter_Form();

            int iFilterIndex = int.Parse(this.dgvFilterList.CurrentRow.Cells["cFilterIndex"].Value.ToString().Trim());
            fFilterForm.Filter_Index = iFilterIndex;
            fFilterForm.Show();
        }

        private void dgvFilterList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                bool bCheck = bool.Parse(this.dgvFilterList.Rows[e.RowIndex].Cells["cCheck"].EditedFormattedValue.ToString());

                Filter_List.dtFilterList.Rows[e.RowIndex]["ISCheck"] = bCheck;
            }
        }
        #endregion      
    }
}

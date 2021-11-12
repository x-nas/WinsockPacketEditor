using System;
using System.Data;
using System.Windows.Forms;
using EasyHook;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class DLL_Form : Form
    {
        private WinSockHook ws = new WinSockHook();        
        private SocketOperation so = new SocketOperation();
        public event SocketEvent.SocketPacketReceived RecSocketPacket;
        private BindingList<SocketInfo> lstRecPacket = new BindingList<SocketInfo>();        

        private int Select_Index = -1;      
        private int FilterCNT = 0;
        private int iShowDataLen = 50;
        
        bool bDebug = true;
        bool bWakeUp = true;        
                
        public DLL_Form()
        {
            InitializeComponent();            
        }        

        //加载窗体
        private void DLL_Form_Load(object sender, EventArgs e)
        {
            this.Init();

            this.bStartHook.Enabled = true;
            this.bStopHook.Enabled = false;

            this.Text = "封包拦截器（无需代理）";
            this.tlSystemInfo.Text = string.Format("已注入目标进程 {0} [{1}]", Process.GetCurrentProcess().ProcessName, RemoteHooking.GetCurrentProcessId());           

            //初始化发送列表
            SocketSend.dtSocketBatchSend.Columns.Add("序号", typeof(int));
            SocketSend.dtSocketBatchSend.Columns.Add("备注", typeof(string));
            SocketSend.dtSocketBatchSend.Columns.Add("套接字", typeof(int));
            SocketSend.dtSocketBatchSend.Columns.Add("目的地址", typeof(string));
            SocketSend.dtSocketBatchSend.Columns.Add("长度", typeof(int));
            SocketSend.dtSocketBatchSend.Columns.Add("数据", typeof(string));
            SocketSend.dtSocketBatchSend.Columns.Add("字节", typeof(byte[]));           

            dgSocketInfo.AutoGenerateColumns = false;
            dgSocketInfo.DataSource = lstRecPacket;
            dgSocketInfo.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgSocketInfo, true, null);
            RecSocketPacket += new SocketEvent.SocketPacketReceived(Event_RecSocketPacket);
            
            this.tSocketInfo.Enabled = true;
        }

        //窗体关闭
        private void DLL_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                LocalHook.Release();
            }
            catch (Exception ex)
            {
                ShowDebug(ex.Message);
            }
        }

        //初始化信息
        private void Init()
        {
            ws.Interecept_Recv = cbInterecept_Recv.Checked;
            ws.Interecept_RecvFrom = cbInterecept_RecvFrom.Checked;
            ws.Interecept_Send = cbInterecept_Send.Checked;
            ws.Interecept_SendTo = cbInterecept_SendTo.Checked;
            ws.Display_Recv = cbDisplay_Recv.Checked;
            ws.Display_RecvFrom = cbDisplay_RecvFrom.Checked;
            ws.Display_Send = cbDisplay_Send.Checked;
            ws.Display_SendTo = cbDisplay_SendTo.Checked;
            ws.Reset_CNT = cbReset_CNT.Checked;

            so.Filter_Size = cbFilter_Size.Checked;
            so.Filter_Socket = cbFilter_Socket.Checked;
            so.Filter_IP = cbFilter_IP.Checked;
            so.Filter_Packet = cbFilter_Packet.Checked;
            so.Filter_Size_From = this.txtFilter_Size_From.Text.Trim();
            so.Filter_Size_To = this.txtFilter_Size_To.Text.Trim();
            so.Filter_Socket_txt = this.txtFilter_Socket.Text.Trim();
            so.Filter_IP_txt = this.txtFilter_IP.Text.Trim();
            so.Filter_Packet_txt = this.txtFilter_Packet.Text.Trim();

            if (cbReset_CNT.Checked)
            {               
                this.FilterCNT = 0;

                this.dgSocketInfo.Rows.Clear();

                this.rtbHEX.Clear();
                this.rtbDEC.Clear();
                this.rtbBIN.Clear();
                this.rtbUNICODE.Clear();
                this.rtbUTF8.Clear();
                this.rtbGB2312.Clear();
                this.rtbDEBUG.Clear();
            }            
        }

        //显示拦截的封包
        private void ShowSocketInfo()
        {
            try
            {
                if (ws._SocketQueue.Count > 0)
                {
                    SocketPacket sa = ws._SocketQueue.Dequeue();

                    bool bFilter = so.Filter(sa);

                    if (bFilter)
                    {
                        int iIndex = dgSocketInfo.Rows.Count + 1;
                        string sType = sa.Type;
                        int iSocket = sa.Socket;
                        int iLen = sa.Length;
                        byte[] bBuffer = sa.Buffer;

                        string sData = "";

                        if (iLen > iShowDataLen)
                        {
                            byte[] bTemp = new byte[iShowDataLen];

                            for (int j = 0; j < iShowDataLen; j++)
                            {
                                bTemp[j] = bBuffer[j];
                            }

                            sData = so.Byte_To_Hex(bTemp) + " ...";
                        }
                        else
                        {
                            sData = so.Byte_To_Hex(bBuffer);
                        }

                        SocketPacket.sockaddr sAddr = sa.Addr;

                        string sIP_From = "", sIP_To = "";

                        if (sType.Equals("R"))
                        {
                            sType = "接收";
                            sIP_From = so.GetSocketIP(iSocket, "T");
                            sIP_To = so.GetSocketIP(iSocket, "F");
                        }
                        else if (sType.Equals("S"))
                        {
                            sType = "发送";
                            sIP_From = so.GetSocketIP(iSocket, "F");
                            sIP_To = so.GetSocketIP(iSocket, "T");
                        }
                        else if (sType.Equals("ST"))
                        {
                            sType = "发送到";
                            sIP_From = so.GetSocketIP(iSocket, "F");
                            sIP_To = so.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                        }
                        else if (sType.Equals("RF"))
                        {
                            sType = "接收自";
                            sIP_From = so.GetSocketIP(sAddr.sin_addr, sAddr.sin_port);
                            sIP_To = so.GetSocketIP(iSocket, "F");
                        }
                        else
                        {
                            //
                        }

                        SocketInfo si = new SocketInfo(iIndex, sType, iSocket, sIP_From, sIP_To, iLen, sData, bBuffer);

                        if (RecSocketPacket != null)
                        {
                            RecSocketPacket(si);
                        }
                    }
                    else
                    {
                        this.FilterCNT++;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowDebug(ex.Message);
            }
        }

        //显示选中的封包信息
        private void ShowPacketInfo()
        {
            try
            {
                if (Select_Index > -1)
                {
                    byte[] bSelected = (byte[])lstRecPacket[Select_Index].Buffer;

                    switch (tcPacketInfo.SelectedIndex)
                    {
                        case 0:
                            this.rtbHEX.Invoke((MethodInvoker)delegate { this.rtbHEX.Text = this.so.Byte_To_Hex(bSelected); });
                            break;
                        case 1:
                            this.rtbDEC.Invoke((MethodInvoker)delegate { this.rtbDEC.Text = this.so.Byte_To_Dec(bSelected); });
                            break;
                        case 2:
                            this.rtbBIN.Invoke((MethodInvoker)delegate { this.rtbBIN.Text = this.so.Byte_To_Bin(bSelected); });
                            break;
                        case 3:
                            this.rtbUNICODE.Invoke((MethodInvoker)delegate { this.rtbUNICODE.Text = this.so.Byte_To_Unicode(bSelected); });
                            break;
                        case 4:
                            this.rtbUTF8.Invoke((MethodInvoker)delegate { this.rtbUTF8.Text = this.so.Byte_To_UTF8(bSelected); });
                            break;
                        case 5:
                            this.rtbGB2312.Invoke((MethodInvoker)delegate { this.rtbGB2312.Text = this.so.Byte_To_GB2312(bSelected); });
                            break;
                    }
                }                              
            }
            catch (Exception ex)
            {
                ShowDebug("ShowPacketInfo - " + ex.Message);
            }
        }                

        //计时器
        private void tSocketInfo_Tick(object sender, EventArgs e)
        {
            this.tlQueue_CNT.Text = ws._SocketQueue.Count.ToString();
            this.tlALL_CNT.Text = (ws.Recv_CNT + ws.Send_CNT).ToString();
            this.tlRecv_CNT.Text = ws.Recv_CNT.ToString();
            this.tlSend_CNT.Text = ws.Send_CNT.ToString();                        
            this.tlInterecept_CNT.Text = ws.Interecept_CNT.ToString();
            this.tlFilter_CNT.Text = FilterCNT.ToString();            

            if (!bgwSocketInfo.IsBusy)
            {
                if (ws._SocketQueue.Count > 0)
                {
                    bgwSocketInfo.RunWorkerAsync();
                }                
            }
        }

        //开始拦截
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

        //结束拦截
        private void bStopHook_Click(object sender, EventArgs e)
        {            
            this.bStartHook.Enabled = true;
            this.bStopHook.Enabled = false;

            ws.StopHook();
        }

        //选中某一行
        private void dgSocketInfo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgSocketInfo.SelectedRows.Count == 1)
            {
                Select_Index = dgSocketInfo.SelectedRows[0].Index;
                ShowPacketInfo();
            }
        }       

        //右键菜单
        private void cmsSocketInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Text;

            if (sItemText.Equals("查看发送列表"))
            {
                if (!SocketSend.bHasBatchSendForm)
                {
                    SocketBatchSend_Form sbsForm = new SocketBatchSend_Form();
                    sbsForm.Show();
                }
            }
            else
            {
                if (Select_Index > -1)
                {
                    try
                    {
                        int iIndex = lstRecPacket[Select_Index].Index;
                        int iSocket = lstRecPacket[Select_Index].Socket;
                        int iLen = lstRecPacket[Select_Index].Length;
                        string sIP_To = lstRecPacket[Select_Index].To;
                        string sData = lstRecPacket[Select_Index].Data;
                        byte[] bSelected = lstRecPacket[Select_Index].Buffer;

                        if (sItemText.Equals("发送"))
                        {
                            if (Select_Index > -1 && dgSocketInfo.SelectedRows.Count == 1 && lstRecPacket.Count > 0)
                            {
                                SocketSend_Form ssForm = new SocketSend_Form();
                                ssForm.Send_Index = iIndex.ToString();
                                ssForm.Send_Socket = iSocket.ToString();
                                ssForm.Send_Len = iLen.ToString();
                                ssForm.Send_IPTo = sIP_To;
                                ssForm.Send_Byte = bSelected;
                                ssForm.Show();
                            }
                        }
                        else if (sItemText.Equals("添加到发送列表"))
                        {
                            DataRow dr = SocketSend.dtSocketBatchSend.NewRow();
                            dr[0] = iIndex;
                            dr[1] = "";
                            dr[2] = iSocket;
                            dr[3] = sIP_To;
                            dr[4] = iLen;
                            dr[5] = sData;
                            dr[6] = bSelected;
                            SocketSend.dtSocketBatchSend.Rows.Add(dr);

                            if (!SocketSend.bHasBatchSendForm)
                            {
                                SocketBatchSend_Form sbsForm = new SocketBatchSend_Form();
                                sbsForm.Show();
                            }
                        }
                        else if (sItemText.Equals("使用此套接字"))
                        {
                            SocketSend.iUseSocket = iSocket;
                        }
                        else if (sItemText.Equals("保存此列表数据"))
                        {
                            int iSuccess = 0, iFail = 0;

                            if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    FileStream fs = new FileStream(sfdSocketInfo.FileName, FileMode.Create);
                                    StreamWriter sw = new StreamWriter(fs);

                                    for (int i = 0; i < this.lstRecPacket.Count; i++)
                                    {
                                        try
                                        {
                                            //序号
                                            string sIndex = this.lstRecPacket[i].Index.ToString();
                                            //类别
                                            string sType = this.lstRecPacket[i].Type.ToString();
                                            //套接字
                                            string sSocket = this.lstRecPacket[i].Socket.ToString();
                                            //源地址
                                            string sIP_From_Save = this.lstRecPacket[i].From.ToString();
                                            //目的地址
                                            string sIP_To_Save = this.lstRecPacket[i].To.ToString();
                                            //长度
                                            string sLen = this.lstRecPacket[i].Length.ToString();
                                            //数据
                                            string sData_Save = this.lstRecPacket[i].Data.ToString();

                                            string sSave = sIndex + "|" + sType + "|" + sSocket + "|" + sIP_From_Save + "|" + sIP_To_Save + "|" + sLen + "|" + sData_Save;

                                            sw.WriteLine(sSave);

                                            iSuccess++;
                                        }
                                        catch
                                        {
                                            iFail++;
                                        }
                                    }

                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();

                                    MessageBox.Show("保存完毕，成功【" + iSuccess + "】失败【" + iFail + "】！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("保存失败！错误：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowDebug(ex.Message);
                    }
                }
            }                        
        }

        //搜索
        private void bSearch_Click(object sender, EventArgs e)
        {
            string sSearch = this.txtSearch.Text.Trim();

            if (!sSearch.Equals(""))
            {
                for (int i = 0; i < dgSocketInfo.Rows.Count; i++)
                {
                    string sData = dgSocketInfo.Rows[i].Cells[6].Value.ToString();

                    if (sData.IndexOf(sSearch) >= 0)
                    {
                        this.dgSocketInfo.Rows[i].Selected = true;
                        this.dgSocketInfo.CurrentCell = dgSocketInfo.Rows[i].Cells[0];

                        return;
                    }
                }
            }
        }

        //调试信息
        private void ShowDebug(string sLog)
        {
            if (bDebug)
            {
                this.rtbDEBUG.Invoke((MethodInvoker)delegate { this.rtbDEBUG.AppendText(sLog + "\n");});                
            }
        }               

        //异步显示封包数据
        private void Event_RecSocketPacket(SocketInfo si)
        {
            dgSocketInfo.Invoke(new MethodInvoker(delegate
            {
                lstRecPacket.Add(si);
            }));
        }

        //多线程
        private void bgwSocketInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowSocketInfo();
        }

        //数据显示方式
        private void tcPacketInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowPacketInfo();
        }
    }
}

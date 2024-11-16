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

        private PerformanceCounter pcCPU;
        private PerformanceCounter pcMEM;

        #region//加载窗体

        public Socket_Form(string sLanguage)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(sLanguage);
                InitializeComponent();

                this.InitPerformanceCounter();
                this.InitSocketForm();
                this.InitSocketDGV();
                this.InitHexBox();
                this.LoadSystemParameter();

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

                SetSystemParameter();
                Socket_Operation.SaveSystemConfig();
                Socket_Operation.SaveFilterList(string.Empty, -1);
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
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Assembly.GetExecutingAssembly().GetName().Version.ToString());

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

                tt.SetToolTip(cbWorkingMode_Speed, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_22));
                tt.SetToolTip(rbFilterSet_Priority, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_63));
                tt.SetToolTip(rbFilterSet_Sequence, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_64));
                tt.SetToolTip(bSearch, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_25));
                tt.SetToolTip(bSearchNext, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_26));

                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;
                this.cmsIcon_StartHook.Enabled = true;
                this.cmsIcon_StopHook.Enabled = false;
                this.tSocketInfo.Enabled = true;
                this.tSocketList.Enabled = true;

                this.cbbExtractionFrom.SelectedIndex = 0;
                this.cbbExtractionTo.SelectedIndex = 0;

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sProcessName);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitPerformanceCounter()
        {
            if (!bgwPerformanceCounter.IsBusy)
            {
                bgwPerformanceCounter.RunWorkerAsync();
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
                dgvSocketList.AutoGenerateColumns = false;
                dgvSocketList.DataSource = Socket_Cache.SocketList.lstRecPacket;
                dgvSocketList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketList, true, null);
                Socket_Cache.SocketList.RecSocketPacket += new Socket_Cache.SocketList.SocketPacketReceived(Event_RecSocketPacket);

                dgvFilterList.AutoGenerateColumns = false;
                dgvFilterList.DataSource = Socket_Cache.FilterList.lstFilter;
                dgvFilterList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterList, true, null);

                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstRecLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
                Socket_Cache.LogList.RecSocketLog += new Socket_Cache.LogList.SocketLogReceived(Event_RecSocketLog);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }            
        }

        #endregion

        #region//获取CPU和内存占用率

        private void bgwPerformanceCounter_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                pcCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                pcMEM = new PerformanceCounter("Memory", "Available MBytes");

                pcCPU.NextValue();
                pcMEM.NextValue();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private string GetMonitorInfo()
        {
            string sReturn = string.Empty;

            try
            {
                if (!bgwPerformanceCounter.IsBusy)
                {
                    float fCPU = pcCPU.NextValue();
                    float fMEM = pcMEM.NextValue() / 160;

                    if (this.cbMonitorSet_Mem.Checked)
                    {
                        int iMEM_Monitor = ((int)this.nudMonitorSet_Mem.Value);

                        if (fMEM < iMEM_Monitor)
                        {
                            this.tsslMonitorInfo.ForeColor = Color.Red;
                        }
                        else
                        {
                            this.tsslMonitorInfo.ForeColor = Color.FromName("ControlText");
                        }
                    }
                    else
                    {
                        this.tsslMonitorInfo.ForeColor = Color.FromName("ControlText");
                    }

                    sReturn = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_80), fCPU.ToString("F1")) + "  " + string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_81), fMEM.ToString("F1"));
                }
                else
                {
                    sReturn = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_88);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//设置系统参数

        private void LoadSystemParameter()
        {
            try
            {
                if (Socket_Operation.LoadSystemConfig())
                {
                    cbHookSend.Checked = Socket_Cache.HookSend;
                    cbHookSendTo.Checked = Socket_Cache.HookSendTo;
                    cbHookRecv.Checked = Socket_Cache.HookRecv;
                    cbHookRecvFrom.Checked = Socket_Cache.HookRecvFrom;
                    cbHookWSASend.Checked = Socket_Cache.HookWSASend;
                    cbHookWSASendTo.Checked = Socket_Cache.HookWSASendTo;
                    cbHookWSARecv.Checked = Socket_Cache.HookWSARecv;
                    cbHookWSARecvFrom.Checked = Socket_Cache.HookWSARecvFrom;

                    if (Socket_Cache.CheckNotShow)
                    {
                        rbFilter_NotShow.Checked = true;
                    }
                    else
                    {
                        rbFilter_Show.Checked = true;
                    }

                    cbCheckSocket.Checked = Socket_Cache.CheckSocket;
                    cbCheckIP.Checked = Socket_Cache.CheckIP;
                    cbCheckPort.Checked = Socket_Cache.CheckPort;
                    cbCheckHead.Checked = Socket_Cache.CheckHead;
                    cbCheckData.Checked = Socket_Cache.CheckData;
                    cbCheckSize.Checked = Socket_Cache.CheckSize;

                    this.txtCheckSocket.Text = Socket_Cache.CheckSocket_Value;
                    this.txtCheckIP.Text = Socket_Cache.CheckIP_Value;
                    this.txtCheckPort.Text = Socket_Cache.CheckPort_Value;
                    this.txtCheckHead.Text = Socket_Cache.CheckHead_Value;
                    this.txtCheckData.Text = Socket_Cache.CheckData_Value;
                    this.nudCheckSizeFrom.Value = Socket_Cache.CheckSizeFrom_Value;
                    this.nudCheckSizeTo.Value = Socket_Cache.CheckSizeTo_Value;

                    this.cbSocketList_AutoRoll.Checked = Socket_Cache.SocketList.AutoRoll;
                    this.cbSocketList_AutoClear.Checked = Socket_Cache.SocketList.AutoClear;
                    this.nudSocketList_AutoClearValue.Value = Socket_Cache.SocketList.AutoClear_Value;
                    this.SocketList_AutoClearChange();

                    this.cbLogList_AutoRoll.Checked = Socket_Cache.FilterList.AutoRoll;
                    this.cbLogList_AutoClear.Checked = Socket_Cache.FilterList.AutoClear;
                    this.nudLogList_AutoClearValue.Value = Socket_Cache.FilterList.AutoClear_Value;
                    this.LogList_AutoClearChange();

                    this.cbWorkingMode_Speed.Checked = Socket_Cache.SpeedMode;
                    this.cbMonitorSet_Mem.Checked = Socket_Cache.MonitorMEM;
                    this.nudMonitorSet_Mem.Value = Socket_Cache.MonitorMEM_Value;
                    this.MonitorMem_Change();

                    switch (Socket_Cache.FilterList.FilterList_Execute)
                    {
                        case Socket_Cache.FilterList.Execute.Priority:
                            this.rbFilterSet_Priority.Checked = true;
                            break;

                        case Socket_Cache.FilterList.Execute.Sequence:
                            this.rbFilterSet_Sequence.Checked = true;
                            break;
                    }

                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
                }
                else
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_36));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void SetSystemParameter()
        {
            try
            {  
                Socket_Cache.HookSend = cbHookSend.Checked;
                Socket_Cache.HookSendTo = cbHookSendTo.Checked;
                Socket_Cache.HookRecv = cbHookRecv.Checked;
                Socket_Cache.HookRecvFrom = cbHookRecvFrom.Checked;
                Socket_Cache.HookWSASend = cbHookWSASend.Checked;
                Socket_Cache.HookWSASendTo = cbHookWSASendTo.Checked;
                Socket_Cache.HookWSARecv = cbHookWSARecv.Checked;
                Socket_Cache.HookWSARecvFrom = cbHookWSARecvFrom.Checked;
                
                Socket_Cache.CheckNotShow = rbFilter_NotShow.Checked;
                Socket_Cache.CheckSocket = cbCheckSocket.Checked;
                Socket_Cache.CheckIP = cbCheckIP.Checked;
                Socket_Cache.CheckPort = cbCheckPort.Checked;
                Socket_Cache.CheckHead = cbCheckHead.Checked;
                Socket_Cache.CheckData = cbCheckData.Checked;
                Socket_Cache.CheckSize = cbCheckSize.Checked;

                Socket_Cache.CheckSocket_Value = this.txtCheckSocket.Text.Trim();
                Socket_Cache.CheckIP_Value = this.txtCheckIP.Text.Trim();
                Socket_Cache.CheckPort_Value = this.txtCheckPort.Text.Trim();
                Socket_Cache.CheckHead_Value = this.txtCheckHead.Text.Trim();
                Socket_Cache.CheckData_Value = this.txtCheckData.Text.Trim();
                Socket_Cache.CheckSizeFrom_Value = this.nudCheckSizeFrom.Value;
                Socket_Cache.CheckSizeTo_Value = this.nudCheckSizeTo.Value;

                Socket_Cache.SocketList.AutoRoll = this.cbSocketList_AutoRoll.Checked;
                Socket_Cache.SocketList.AutoClear = this.cbSocketList_AutoClear.Checked;
                Socket_Cache.SocketList.AutoClear_Value = this.nudSocketList_AutoClearValue.Value;

                Socket_Cache.FilterList.AutoRoll = this.cbLogList_AutoRoll.Checked;
                Socket_Cache.FilterList.AutoClear = this.cbLogList_AutoClear.Checked;
                Socket_Cache.FilterList.AutoClear_Value = this.nudLogList_AutoClearValue.Value;

                Socket_Cache.SpeedMode = this.cbWorkingMode_Speed.Checked;
                Socket_Cache.MonitorMEM = this.cbMonitorSet_Mem.Checked;
                Socket_Cache.MonitorMEM_Value = this.nudMonitorSet_Mem.Value;

                if (this.rbFilterSet_Priority.Checked)
                {
                    Socket_Cache.FilterList.FilterList_Execute = Socket_Cache.FilterList.Execute.Priority;
                }
                else
                {
                    Socket_Cache.FilterList.FilterList_Execute = Socket_Cache.FilterList.Execute.Sequence;
                }

            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        #endregion        

        #region//检测过滤参数输入

        private void txtCheckSocket_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCheckPacket_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCheckHead_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCheckIP_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCheckPort_KeyPress(object sender, KeyPressEventArgs e)
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

        #region//列表设置

        private void cbSocketList_AutoClear_CheckedChanged(object sender, EventArgs e)
        {
            this.SocketList_AutoClearChange();
        }

        private void SocketList_AutoClearChange()
        {
            try
            {
                if (this.cbSocketList_AutoClear.Checked)
                {
                    this.nudSocketList_AutoClearValue.Enabled = true;
                }
                else
                {
                    this.nudSocketList_AutoClearValue.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cbLogList_AutoClear_CheckedChanged(object sender, EventArgs e)
        {
            this.LogList_AutoClearChange();
        }

        private void LogList_AutoClearChange()
        {
            try
            {
                if (this.cbLogList_AutoClear.Checked)
                {
                    this.nudLogList_AutoClearValue.Enabled = true;
                }
                else
                {
                    this.nudLogList_AutoClearValue.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//系统设置

        private void cbMonitorSet_Mem_CheckedChanged(object sender, EventArgs e)
        {
            this.MonitorMem_Change();
        }

        private void MonitorMem_Change()
        {
            try
            {
                if (this.cbMonitorSet_Mem.Checked)
                {
                    this.nudMonitorSet_Mem.Enabled = true;
                }
                else
                {
                    this.nudMonitorSet_Mem.Enabled = false;
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
            this.CleanUp_SocketInfo();
            this.CleanUp_SocketList();
            this.CleanUp_HexBox();
            this.CleanUp_LogList();
        }

        private void CleanUp_SocketInfo()
        {
            try
            {
                Socket_Cache.TotalPackets = 0;
                Socket_Cache.Total_SendBytes = 0;
                Socket_Cache.Total_RecvBytes = 0;
                Socket_Cache.Filter.FilterExecute_CNT = 0;              
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
                Socket_Cache.SocketQueue.ResetSocketQueue();
                Socket_Cache.SocketList.lstRecPacket.Clear();

                this.Select_Index = -1;
                this.dgvSocketList.Rows.Clear();

                Socket_Cache.SocketQueue.FilterSocketList_CNT = 0;
                Socket_Cache.SocketQueue.Send_CNT = 0;
                Socket_Cache.SocketQueue.Recv_CNT = 0;
                Socket_Cache.SocketQueue.SendTo_CNT = 0;
                Socket_Cache.SocketQueue.RecvFrom_CNT = 0;
                Socket_Cache.SocketQueue.WSASend_CNT = 0;
                Socket_Cache.SocketQueue.WSARecv_CNT = 0;
                Socket_Cache.SocketQueue.WSASendTo_CNT = 0;
                Socket_Cache.SocketQueue.WSARecvFrom_CNT = 0;
            }
            catch(Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_LogList()
        {
            try
            {
                Socket_Cache.LogQueue.ResetLogQueue();
                Socket_Cache.LogList.lstRecLog.Clear();                
                this.dgvLogList.Rows.Clear();             
            }
            catch (Exception ex)
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

        private void AutoCleanUp_SocketList()
        {
            try
            {
                if (this.cbSocketList_AutoClear.Checked)
                {
                    decimal dClearCount = this.nudSocketList_AutoClearValue.Value;

                    if (dClearCount > 0)
                    {
                        if (this.dgvSocketList.Rows.Count > dClearCount)
                        {
                            this.CleanUp_SocketList();
                            this.CleanUp_HexBox();
                        }                      
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void AutoCleanUp_LogList()
        {
            try
            {
                if (this.cbLogList_AutoClear.Checked)
                {
                    decimal dClearCount = this.nudLogList_AutoClearValue.Value;

                    if (dClearCount > 0)
                    {
                        if (this.dgvLogList.Rows.Count > dClearCount)
                        {
                            this.CleanUp_LogList();
                        }
                    }
                }
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
                this.SetSystemParameter();

                this.tcSocketInfo_FilterSet.Enabled = false;
                this.tcSocketInfo_HookSet.Enabled = false;
                this.tcSocketInfo_SystemSet.Enabled = false;

                this.bStartHook.Enabled = false;
                this.bStopHook.Enabled = true;

                this.cmsIcon_StartHook.Enabled = false;
                this.cmsIcon_StopHook.Enabled = true;

                ws.StartHook();

                if (this.cbWorkingMode_Speed.Checked)
                {
                    this.CleanUp_MainForm();
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_41));
                }

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_39));
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
                this.tcSocketInfo_FilterSet.Enabled = true;
                this.tcSocketInfo_HookSet.Enabled = true;
                this.tcSocketInfo_SystemSet.Enabled = true;

                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;

                this.cmsIcon_StartHook.Enabled = true;
                this.cmsIcon_StopHook.Enabled = false;

                ws.StopHook();

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_40));
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
            if (!bgwSocketInfo.IsBusy)
            {
                bgwSocketInfo.RunWorkerAsync();
            }
        }

        private void tSocketList_Tick(object sender, EventArgs e)
        {
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

        #endregion

        #region//显示封包信息（异步）

        private void bgwSocketInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), Socket_Operation.GetDisplayBytes(Socket_Cache.Total_SendBytes), Socket_Operation.GetDisplayBytes(Socket_Cache.Total_RecvBytes));
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
                this.tlTotal_CNT.Text = Socket_Cache.TotalPackets.ToString();
                this.tlFilterExecute_CNT.Text = Socket_Cache.Filter.FilterExecute_CNT.ToString();
                this.tsslTotalBytes.Text = e.Result.ToString();
                this.tlQueue_CNT.Text = Socket_Cache.SocketQueue.qSocket_PacketInfo.Count.ToString();
                this.tlFilterSocketList_CNT.Text = Socket_Cache.SocketQueue.FilterSocketList_CNT.ToString();
                this.tlSend_CNT.Text = Socket_Cache.SocketQueue.Send_CNT.ToString();
                this.tlRecv_CNT.Text = Socket_Cache.SocketQueue.Recv_CNT.ToString();
                this.tlSendTo_CNT.Text = Socket_Cache.SocketQueue.SendTo_CNT.ToString();
                this.tlRecvFrom_CNT.Text = Socket_Cache.SocketQueue.RecvFrom_CNT.ToString();
                this.tlWSASend_CNT.Text = Socket_Cache.SocketQueue.WSASend_CNT.ToString();
                this.tlWSARecv_CNT.Text = Socket_Cache.SocketQueue.WSARecv_CNT.ToString();
                this.tlWSASendTo_CNT.Text = Socket_Cache.SocketQueue.WSASendTo_CNT.ToString();
                this.tlWSARecvFrom_CNT.Text = Socket_Cache.SocketQueue.WSARecvFrom_CNT.ToString();
                this.tsslMonitorInfo.Text = this.GetMonitorInfo();
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
                if (this.cbSocketList_AutoRoll.Checked)
                {
                    if (dgvSocketList.Rows.Count > 0 && dgvSocketList.Height > dgvSocketList.RowTemplate.Height)
                    {
                        dgvSocketList.FirstDisplayedScrollingRowIndex = dgvSocketList.RowCount - 1;
                    }
                }

                this.AutoCleanUp_SocketList();                
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
                    Socket_Cache.SocketPacket.PacketType ptType = (Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value;
                    e.Value = Socket_Operation.GetImg_ByPacketType(ptType);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketType"].Index)
                {
                    Socket_Cache.SocketPacket.PacketType ptType = (Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value;
                    e.Value = Socket_Operation.GetName_ByPacketType(ptType);
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
            Socket_Cache.LogList.LogToList();
        }

        private void bgwLogList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.cbLogList_AutoRoll.Checked)
                {
                    if (dgvLogList.Rows.Count > 0 && dgvLogList.Height > dgvLogList.RowTemplate.Height)
                    {
                        dgvLogList.FirstDisplayedScrollingRowIndex = dgvLogList.RowCount - 1;
                    }
                }

                this.AutoCleanUp_LogList();
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
                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();

                                byte[] bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, Clipboard.GetText());
                                Socket_Cache.FilterList.AddToFilterList_BySocketListIndex(Select_Index, bBuffer);
                            }
                            else
                            {
                                Socket_Cache.FilterList.AddToFilterList_BySocketListIndex(Select_Index, null);
                            }
                        }

                        break;

                    case "cmsHexBox_CopyHex":

                        this.hbPacketData.CopyHex();

                        break;

                    case "cmsHexBox_CopyText":

                        this.hbPacketData.Copy();

                        break;

                    case "cmsHexBox_Comparison_A":

                        this.hbPacketData.CopyHex();
                        this.rtbComparison_A.Text = Clipboard.GetText();

                        break;

                    case "cmsHexBox_Comparison_B":

                        this.hbPacketData.CopyHex();
                        this.rtbComparison_B.Text = Clipboard.GetText();

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

        #region//滤镜菜单

        private void cmsFilterList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsFilterList.Close();

            try
            {
                if (dgvFilterList.Rows.Count > 0)
                {
                    int iIndex = 0;
                    int iFIndex = this.dgvFilterList.CurrentRow.Index;

                    if (iFIndex > -1)
                    {
                        Socket_Cache.Filter.FilterMove filterMove = Socket_Cache.Filter.FilterMove.Up;

                        switch (sItemText)
                        {
                            case "cmsFilterList_MoveTop":

                                filterMove = Socket_Cache.Filter.FilterMove.Top;
                                iIndex = Socket_Operation.MoveFilter_ByFilterIndex(iFIndex, filterMove);

                                break;

                            case "cmsFilterList_MoveUp":

                                filterMove = Socket_Cache.Filter.FilterMove.Up;
                                iIndex = Socket_Operation.MoveFilter_ByFilterIndex(iFIndex, filterMove);

                                break;

                            case "cmsFilterList_MoveDown":

                                filterMove = Socket_Cache.Filter.FilterMove.Down;
                                iIndex = Socket_Operation.MoveFilter_ByFilterIndex(iFIndex, filterMove);

                                break;

                            case "cmsFilterList_MoveBottom":

                                filterMove = Socket_Cache.Filter.FilterMove.Bottom;
                                iIndex = Socket_Operation.MoveFilter_ByFilterIndex(iFIndex, filterMove);

                                break;

                            case "cmsFilterList_Copy":

                                iIndex = Socket_Operation.CopyFilter_ByFilterIndex(iFIndex);

                                break;

                            case "cmsFilterList_Export":

                                string sFName = this.dgvFilterList.CurrentRow.Cells["cFName"].Value.ToString();
                                Socket_Operation.SaveFilterList_Dialog(sFName, iFIndex);
                                iIndex = iFIndex;

                                break;

                            case "cmsFilterList_Delete":

                                int iFNum = (int)this.dgvFilterList.CurrentRow.Cells["cFNum"].Value;

                                if (iFNum > 0)
                                {
                                    Socket_Operation.DeleteFilter_ByFilterNum_Dialog(iFNum);
                                }

                                break;
                        }

                        if (iIndex > -1 && iIndex < dgvFilterList.RowCount)
                        {
                            this.dgvFilterList.ClearSelection();
                            this.dgvFilterList.Rows[iIndex].Selected = true;
                            this.dgvFilterList.CurrentCell = this.dgvFilterList.Rows[iIndex].Cells[0];
                        }                       
                    }
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

                        this.CleanUp_LogList();

                        break;                   
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
                Socket_Operation.SaveFilterList_Dialog(string.Empty, -1);
            }
        }

        private void tsFilterList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.FilterList.AddFilter_New();
            Socket_Operation.SaveFilterList(string.Empty, -1);
        }

        private void tsFilterList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvFilterList.Rows.Count > 0)
            {
                Socket_Operation.CleanUpFilterList_Dialog();
            }
        }

        private void tsFilterList_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Socket_FilterInfo sfi in Socket_Cache.FilterList.lstFilter)
                {
                    int FNum = sfi.FNum;

                    Socket_Cache.FilterList.SetIsCheck_ByFilterNum(FNum, true);

                    this.dgvFilterList.Refresh();
                    this.dgvSocketList.Focus();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsFilterList_SelectNo_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Socket_FilterInfo sfi in Socket_Cache.FilterList.lstFilter)
                {
                    int FNum = sfi.FNum;

                    Socket_Cache.FilterList.SetIsCheck_ByFilterNum(FNum, false);

                    this.dgvFilterList.Refresh();
                    this.dgvSocketList.Focus();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

        #region//数据提取

        private void bExtraction_Clear_Click(object sender, EventArgs e)
        {
            this.rtbExtractionFrom.Clear();
            this.rtbExtractionTo.Clear();
        }

        private void bExtraction_Click(object sender, EventArgs e)
        {
            try
            {
                string sReturn = string.Empty;

                foreach (string line in this.rtbExtractionFrom.Lines)
                {
                    string sHex = line.Substring(10, 48);

                    sReturn += sHex;
                }

                sReturn = sReturn.Trim().ToUpper();

                this.rtbExtractionTo.Text = sReturn;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        
    }
}

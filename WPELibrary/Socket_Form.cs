﻿using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using WPELibrary.Lib;
using EasyHook;
using Be.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Text;
using WPELibrary.Lib.NativeMethods;
using System.Threading.Tasks;

namespace WPELibrary
{
    public partial class Socket_Form : Form
    {
        private int Select_Index = -1;
        private int Search_Index = -1;
        private bool bWakeUp = true;
        private readonly ToolTip tt = new ToolTip();
        private readonly WinSockHook ws = new WinSockHook();

        #region//加载窗体

        public Socket_Form(string sLanguage)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(sLanguage);
                InitializeComponent();                

                this.InitSocketForm();
                this.InitSocketDGV();                
                this.InitHexBox_XOR();
                
                this.LoadConfigs_Parameter();
                Socket_Operation.LoadSystemList();
                Socket_Operation.RegisterHotKey();
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

                this.SaveConfigs_Parameter();
                Socket_Operation.SaveSystemList();
                Socket_Operation.UnregisterHotKey();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == User32.WM_HOTKEY)
                {
                    int HOTKEY_ID = m.WParam.ToInt32();

                    switch (HOTKEY_ID)
                    {
                        case 9001:
                            Socket_Cache.Robot.DoRobot_ByIndex(0);
                            break;

                        case 9002:
                            Socket_Cache.Robot.DoRobot_ByIndex(1);
                            break;

                        case 9003:
                            Socket_Cache.Robot.DoRobot_ByIndex(2);
                            break;

                        case 9004:
                            Socket_Cache.Robot.DoRobot_ByIndex(3);
                            break;

                        case 9005:
                            Socket_Cache.Robot.DoRobot_ByIndex(4);
                            break;

                        case 9006:
                            Socket_Cache.Robot.DoRobot_ByIndex(5);
                            break;

                        case 9007:
                            Socket_Cache.Robot.DoRobot_ByIndex(6);
                            break;

                        case 9008:
                            Socket_Cache.Robot.DoRobot_ByIndex(7);
                            break;

                        case 9009:
                            Socket_Cache.Robot.DoRobot_ByIndex(8);
                            break;

                        case 9010:
                            Socket_Cache.Robot.DoRobot_ByIndex(9);
                            break;

                        case 9011:
                            Socket_Cache.Robot.DoRobot_ByIndex(10);
                            break;

                        case 9012:
                            Socket_Cache.Robot.DoRobot_ByIndex(11);
                            break;
                    }                    
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            base.WndProc(ref m);            
        }        

        #endregion

        #region//初始化      

        private void InitSocketForm()
        {
            try
            {
                this.Text = Socket_Cache.WPE + " - " + Socket_Operation.AssemblyVersion;

                tt.SetToolTip(cbWorkingMode_Speed, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_22));
                tt.SetToolTip(rbFilterSet_Priority, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_63));
                tt.SetToolTip(rbFilterSet_Sequence, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_64));
                tt.SetToolTip(bSearch, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_25));
                tt.SetToolTip(bSearchNext, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_26));

                Socket_Cache.MainHandle = this.Handle;
                string sProcessName = Socket_Operation.GetProcessName();
                this.tsslProcessName.Text = sProcessName;
                this.niWPE.Text = Socket_Cache.WPE + "\r\n" + sProcessName;                
                
                this.tsslProcessInfo.Text = Socket_Operation.GetProcessInfo();             
                this.tsslWinSock.Text = Socket_Operation.GetWinSockSupportInfo();

                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;
                this.cmsIcon_StartHook.Enabled = true;
                this.cmsIcon_StopHook.Enabled = false;
                this.tSocketInfo.Enabled = true;
                this.tSocketList.Enabled = true;

                this.cbbExtraction.SelectedIndex = 0;                

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sProcessName);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void InitHexBox_XOR()
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

                dgvRobotList.AutoGenerateColumns = false;
                dgvRobotList.DataSource = Socket_Cache.RobotList.lstRobot;
                dgvRobotList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvRobotList, true, null);

                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstSocketLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
                Socket_Cache.LogList.RecSocketLog += new Socket_Cache.LogList.SocketLogReceived(Event_RecSocketLog);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }            
        }

        #endregion        

        #region//加载系统参数

        private void LoadConfigs_Parameter()
        {
            try
            {
                Socket_Operation.LoadConfigs_SocketPacket();

                cbHookSend.Checked = Socket_Cache.SocketPacket.HookSend;
                cbHookSendTo.Checked = Socket_Cache.SocketPacket.HookSendTo;
                cbHookRecv.Checked = Socket_Cache.SocketPacket.HookRecv;
                cbHookRecvFrom.Checked = Socket_Cache.SocketPacket.HookRecvFrom;
                cbHookWSASend.Checked = Socket_Cache.SocketPacket.HookWSASend;
                cbHookWSASendTo.Checked = Socket_Cache.SocketPacket.HookWSASendTo;
                cbHookWSARecv.Checked = Socket_Cache.SocketPacket.HookWSARecv;
                cbHookWSARecvFrom.Checked = Socket_Cache.SocketPacket.HookWSARecvFrom;

                if (Socket_Cache.SocketPacket.CheckNotShow)
                {
                    rbFilter_NotShow.Checked = true;
                }
                else
                {
                    rbFilter_Show.Checked = true;
                }

                cbCheckSocket.Checked = Socket_Cache.SocketPacket.CheckSocket;
                cbCheckIP.Checked = Socket_Cache.SocketPacket.CheckIP;
                cbCheckPort.Checked = Socket_Cache.SocketPacket.CheckPort;
                cbCheckHead.Checked = Socket_Cache.SocketPacket.CheckHead;
                cbCheckData.Checked = Socket_Cache.SocketPacket.CheckData;
                cbCheckSize.Checked = Socket_Cache.SocketPacket.CheckSize;

                this.txtCheckSocket.Text = Socket_Cache.SocketPacket.CheckSocket_Value;
                this.txtCheckLength.Text = Socket_Cache.SocketPacket.CheckLength_Value;
                this.txtCheckIP.Text = Socket_Cache.SocketPacket.CheckIP_Value;
                this.txtCheckPort.Text = Socket_Cache.SocketPacket.CheckPort_Value;
                this.txtCheckHead.Text = Socket_Cache.SocketPacket.CheckHead_Value;
                this.txtCheckData.Text = Socket_Cache.SocketPacket.CheckData_Value;             

                this.cbSocketList_AutoRoll.Checked = Socket_Cache.SocketList.AutoRoll;
                this.cbSocketList_AutoClear.Checked = Socket_Cache.SocketList.AutoClear;
                this.nudSocketList_AutoClearValue.Value = Socket_Cache.SocketList.AutoClear_Value;
                this.SocketList_AutoClearChange();

                this.cbLogList_AutoRoll.Checked = Socket_Cache.LogList.AutoRoll;
                this.cbLogList_AutoClear.Checked = Socket_Cache.LogList.AutoClear;
                this.nudLogList_AutoClearValue.Value = Socket_Cache.LogList.AutoClear_Value;
                this.LogList_AutoClearChange();                

                this.cbWorkingMode_Speed.Checked = Socket_Cache.SocketPacket.SpeedMode;

                switch (Socket_Cache.FilterList.FilterList_Execute)
                {
                    case Socket_Cache.FilterList.Execute.Priority:
                        this.rbFilterSet_Priority.Checked = true;
                        break;

                    case Socket_Cache.FilterList.Execute.Sequence:
                        this.rbFilterSet_Sequence.Checked = true;
                        break;
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存系统参数

        private void SaveConfigs_Parameter()
        {
            try
            {  
                Socket_Cache.SocketPacket.HookSend = cbHookSend.Checked;
                Socket_Cache.SocketPacket.HookSendTo = cbHookSendTo.Checked;
                Socket_Cache.SocketPacket.HookRecv = cbHookRecv.Checked;
                Socket_Cache.SocketPacket.HookRecvFrom = cbHookRecvFrom.Checked;
                Socket_Cache.SocketPacket.HookWSASend = cbHookWSASend.Checked;
                Socket_Cache.SocketPacket.HookWSASendTo = cbHookWSASendTo.Checked;
                Socket_Cache.SocketPacket.HookWSARecv = cbHookWSARecv.Checked;
                Socket_Cache.SocketPacket.HookWSARecvFrom = cbHookWSARecvFrom.Checked;
                
                Socket_Cache.SocketPacket.CheckNotShow = rbFilter_NotShow.Checked;
                Socket_Cache.SocketPacket.CheckSocket = cbCheckSocket.Checked;
                Socket_Cache.SocketPacket.CheckIP = cbCheckIP.Checked;
                Socket_Cache.SocketPacket.CheckPort = cbCheckPort.Checked;
                Socket_Cache.SocketPacket.CheckHead = cbCheckHead.Checked;
                Socket_Cache.SocketPacket.CheckData = cbCheckData.Checked;
                Socket_Cache.SocketPacket.CheckSize = cbCheckSize.Checked;

                Socket_Cache.SocketPacket.CheckSocket_Value = this.txtCheckSocket.Text.Trim();
                Socket_Cache.SocketPacket.CheckLength_Value = this.txtCheckLength.Text.Trim();
                Socket_Cache.SocketPacket.CheckIP_Value = this.txtCheckIP.Text.Trim();
                Socket_Cache.SocketPacket.CheckPort_Value = this.txtCheckPort.Text.Trim();
                Socket_Cache.SocketPacket.CheckHead_Value = this.txtCheckHead.Text.Trim();
                Socket_Cache.SocketPacket.CheckData_Value = this.txtCheckData.Text.Trim();            

                Socket_Cache.SocketList.AutoRoll = this.cbSocketList_AutoRoll.Checked;
                Socket_Cache.SocketList.AutoClear = this.cbSocketList_AutoClear.Checked;
                Socket_Cache.SocketList.AutoClear_Value = this.nudSocketList_AutoClearValue.Value;

                Socket_Cache.LogList.AutoRoll = this.cbLogList_AutoRoll.Checked;
                Socket_Cache.LogList.AutoClear = this.cbLogList_AutoClear.Checked;
                Socket_Cache.LogList.AutoClear_Value = this.nudLogList_AutoClearValue.Value;                

                Socket_Cache.SocketPacket.SpeedMode = this.cbWorkingMode_Speed.Checked;            

                if (this.rbFilterSet_Priority.Checked)
                {
                    Socket_Cache.FilterList.FilterList_Execute = Socket_Cache.FilterList.Execute.Priority;
                }
                else
                {
                    Socket_Cache.FilterList.FilterList_Execute = Socket_Cache.FilterList.Execute.Sequence;
                }

                Socket_Operation.SaveConfigs_SocketPacket();
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

        private void txtCheckLength_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMostCheckedChanged();
        }

        private void TopMostCheckedChanged()
        {
            try
            {
                if (this.cbTopMost.Checked)
                {
                    this.TopMost = true;
                }
                else
                {
                    this.TopMost = false;
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
                Socket_Cache.SocketPacket.TotalPackets = 0;
                Socket_Cache.SocketPacket.Total_SendBytes = 0;
                Socket_Cache.SocketPacket.Total_RecvBytes = 0;
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
                Socket_Cache.LogQueue.ResetLogQueue(Socket_Cache.LogType.Socket);
                Socket_Cache.LogList.ResetLogList(Socket_Cache.LogType.Socket);
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
                this.tcSocketInfo_FilterSet.Enabled = false;
                this.tcSocketInfo_HookSet.Enabled = false;
                this.tcSocketInfo_SystemSet.Enabled = false;

                this.bStartHook.Enabled = false;
                this.bStopHook.Enabled = true;

                this.cmsIcon_StartHook.Enabled = false;
                this.cmsIcon_StopHook.Enabled = true;

                this.SaveConfigs_Parameter();
                Socket_Cache.FilterList.InitFilterList_ProgressionCount();

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
            Task.Run(() =>
            {
                try
                {
                    this.tlTotal_CNT.Text = Socket_Cache.SocketPacket.TotalPackets.ToString();
                    this.tlFilterExecute_CNT.Text = Socket_Cache.Filter.FilterExecute_CNT.ToString();
                    this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketPacket.Total_SendBytes), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketPacket.Total_RecvBytes));
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
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });
        }

        private void tSocketList_Tick(object sender, EventArgs e)
        {
            Task.Run(async () => 
            {
                try
                {
                    if (Socket_Cache.SocketQueue.qSocket_PacketInfo.Count > 0)
                    {
                        await Socket_Cache.SocketList.SocketToList(Socket_Cache.SocketPacket.PacketData_MaxLen);

                        if (!IsDisposed)
                        {
                            dgvSocketList.Invoke(new MethodInvoker(delegate
                            {
                                if (this.cbSocketList_AutoRoll.Checked)
                                {
                                    if (dgvSocketList.Rows.Count > 0 && dgvSocketList.Height > dgvSocketList.RowTemplate.Height)
                                    {
                                        dgvSocketList.FirstDisplayedScrollingRowIndex = dgvSocketList.RowCount - 1;
                                    }
                                }

                                this.AutoCleanUp_SocketList();
                            }));
                        }
                    }

                    if (Socket_Cache.LogQueue.qSocket_Log.Count > 0)
                    {
                        await Socket_Cache.LogList.LogToList(Socket_Cache.LogType.Socket);

                        if (!IsDisposed)
                        {
                            dgvLogList.Invoke(new MethodInvoker(delegate
                            {
                                if (this.cbLogList_AutoRoll.Checked)
                                {
                                    if (dgvLogList.Rows.Count > 0 && dgvLogList.Height > dgvLogList.RowTemplate.Height)
                                    {
                                        dgvLogList.FirstDisplayedScrollingRowIndex = dgvLogList.RowCount - 1;
                                    }
                                }

                                this.AutoCleanUp_LogList();
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });            
        }

        #endregion        

        #region//显示封包列表（异步）

        private void Event_RecSocketPacket(Socket_PacketInfo spi)
        {
            try
            {
                if (!IsDisposed)
                {
                    this.dgvSocketList.Invoke(new MethodInvoker(delegate
                    {
                        Socket_Cache.SocketList.lstRecPacket.Add(spi);
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
                    Socket_Cache.SocketPacket.PacketType ptType = (Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value;
                    e.Value = Socket_Cache.SocketPacket.GetImg_ByPacketType(ptType);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketType"].Index)
                {
                    Socket_Cache.SocketPacket.PacketType ptType = (Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value;
                    e.Value = Socket_Cache.SocketPacket.GetName_ByPacketType(ptType);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cData"].Index)
                {
                    Socket_Cache.Filter.FilterAction faAction = Socket_Cache.SocketList.lstRecPacket[e.RowIndex].FilterAction;

                    if (faAction == Socket_Cache.Filter.FilterAction.Intercept)
                    {
                        this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkRed;
                    }
                    else if (faAction == Socket_Cache.Filter.FilterAction.Replace)
                    {
                        this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Goldenrod;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示日志列表（异步）

        private void Event_RecSocketLog(Socket_LogInfo sli)
        {
            try
            {
                if (!IsDisposed)
                {
                    dgvLogList.Invoke(new MethodInvoker(delegate
                    {
                        Socket_Cache.LogList.lstSocketLog.Add(sli);
                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvLogList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvLogList.Columns["cLogID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
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

            if (Socket_Cache.SocketList.DoSearch)
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
            Task.Run(() =>
            {
                try
                {
                    if (Socket_Cache.SocketList.FindOptions.IsValid)
                    {
                        if (!IsDisposed)
                        {
                            hbPacketData.Invoke(new MethodInvoker(delegate
                            {
                                long res = this.hbPacketData.Find(Socket_Cache.SocketList.FindOptions);

                                if (res == -1)
                                {
                                    Search_Index += 1;
                                    this.SearchSocketListNext();
                                }
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });            
        }

        private async void SearchSocketListNext()
        {
            try
            {
                if (dgvSocketList.Rows.Count > 0)
                {
                    if (Socket_Cache.SocketList.FindOptions.IsValid)
                    {
                        string sSearch_Text = string.Empty;
                        string sSearch_Type = string.Empty;

                        FindType fType = Socket_Cache.SocketList.FindOptions.Type;

                        Socket_Cache.SocketPacket.EncodingFormat efFormat = new Socket_Cache.SocketPacket.EncodingFormat();

                        switch (fType)
                        {
                            case FindType.Text:
                                efFormat = Socket_Cache.SocketPacket.EncodingFormat.UTF7;
                                sSearch_Text = Socket_Cache.SocketList.FindOptions.Text;
                                break;

                            case FindType.Hex:
                                efFormat = Socket_Cache.SocketPacket.EncodingFormat.Hex;
                                byte[] bSearch_Hex = Socket_Cache.SocketList.FindOptions.Hex;
                                sSearch_Text = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, bSearch_Hex);
                                break;
                        }

                        if (rbFromHead.Checked)
                        {
                            Search_Index = 0;
                            this.rbFromIndex.Checked = true;
                            this.hbPacketData.SelectionStart = 0;
                        }

                        int iIndex = await Socket_Cache.SocketList.FindSocketList(efFormat, Search_Index, sSearch_Text, Socket_Cache.SocketList.FindOptions.MatchCase);

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
                if (Select_Index > -1)
                {
                    switch (sItemText)
                    {
                        case "cmsHexBox_Send":

                            Socket_Operation.ShowSendForm(Select_Index);

                            break;

                        case "cmsHexBox_SendList":

                            Socket_Cache.SendList.AddToSendList_BytIndex(Select_Index);
                            Socket_Operation.ShowSendListForm();

                            break;

                        case "cmsHexBox_FilterList":

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();

                                byte[] bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, Clipboard.GetText());
                                Socket_Cache.Filter.AddFilter_BySocketListIndex(Select_Index, bBuffer);
                            }
                            else
                            {
                                Socket_Cache.Filter.AddFilter_BySocketListIndex(Select_Index, null);
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

        private async void cmsSocketList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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

                    case "cmsSocketList_SendList":

                        for (int i = 0; i < dgvSocketList.Rows.Count; i++)
                        {
                            if (dgvSocketList.Rows[i].Selected)
                            {
                                Socket_Cache.SendList.AddToSendList_BytIndex(i);
                            }
                        }

                        Socket_Operation.ShowSendListForm();

                        break;

                    case "cmsSocketList_FilterList":

                        if (Select_Index > -1)
                        {
                            Socket_Cache.Filter.AddFilter_BySocketListIndex(Select_Index, null);
                        }

                        break;

                    case "cmsSocketList_ShowModified":

                        if (Select_Index > -1)
                        {
                            Socket_Operation.ShowSocketCompareForm(Select_Index);
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
                            Socket_Cache.SocketList.SaveSocketListToExcel();
                        }

                        break;

                    case "cmsSocketList_Comparison_A":

                        if (Select_Index > -1)
                        {
                            string sPacketHex = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer);
                            this.rtbComparison_A.Text = sPacketHex;
                        }

                        break;

                    case "cmsSocketList_Comparison_B":

                        if (Select_Index > -1)
                        {
                            string sPacketHex = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer);
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

        #region//滤镜列表菜单

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
                        switch (sItemText)
                        {
                            case "cmsFilterList_Top":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Top, iFIndex);
                                break;

                            case "cmsFilterList_Up":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Up, iFIndex);
                                break;

                            case "cmsFilterList_Down":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Down, iFIndex);
                                break;

                            case "cmsFilterList_Bottom":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Bottom, iFIndex);
                                break;

                            case "cmsFilterList_Copy":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Copy, iFIndex);
                                break;

                            case "cmsFilterList_Export":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Export, iFIndex);
                                break;

                            case "cmsFilterList_Delete":
                                iIndex = Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.ListAction.Delete, iFIndex);
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

        #region//机器人列表菜单

        private void cmsRobotList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsRobotList.Close();

            try
            {
                if (dgvRobotList.Rows.Count > 0)
                {
                    int iIndex = 0;
                    int iRIndex = this.dgvRobotList.CurrentRow.Index;

                    if (iRIndex > -1)
                    {
                        switch (sItemText)
                        {
                            case "cmsRobotList_Top":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Top, iRIndex);
                                break;

                            case "cmsRobotList_Up":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Up, iRIndex);
                                break;

                            case "cmsRobotList_Down":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Down, iRIndex);
                                break;

                            case "cmsRobotList_Bottom":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Bottom, iRIndex);
                                break;

                            case "cmsRobotList_Copy":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Copy, iRIndex);
                                break;

                            case "cmsRobotList_Export":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Export, iRIndex);
                                break;

                            case "cmsRobotList_Delete":
                                iIndex = Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.ListAction.Delete, iRIndex);
                                break;
                        }

                        if (iIndex > -1 && iIndex < dgvRobotList.RowCount)
                        {
                            this.dgvRobotList.ClearSelection();
                            this.dgvRobotList.Rows[iIndex].Selected = true;
                            this.dgvRobotList.CurrentCell = this.dgvRobotList.Rows[iIndex].Cells[0];
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
                            Socket_Cache.LogList.SaveLogListToExcel();
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
                    int FIndex = e.RowIndex;
                    bool bCheck = !bool.Parse(dgvFilterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    dgvFilterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bCheck;

                    Socket_Cache.Filter.SetIsCheck_ByFilterIndex(FIndex, bCheck);
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
                    int FIndex = e.RowIndex;

                    if (FIndex > -1)
                    {
                        Socket_Operation.ShowFilterForm_Dialog(FIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//滤镜列表按钮

        private void tsFilterList_Load_Click(object sender, EventArgs e)
        {
            Socket_Cache.FilterList.LoadFilterList_Dialog();
        }

        private void tsFilterList_Save_Click(object sender, EventArgs e)
        {
            if (dgvFilterList.Rows.Count > 0)
            {
                Socket_Cache.FilterList.SaveFilterList_Dialog(string.Empty, -1);
            }
        }

        private void tsFilterList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.Filter.AddFilter_New();            
        }

        private void tsFilterList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvFilterList.Rows.Count > 0)
            {
                Socket_Cache.FilterList.CleanUpFilterList_Dialog();
            }
        }

        private void tsFilterList_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < Socket_Cache.FilterList.lstFilter.Count; i++) 
                {
                    Socket_Cache.Filter.SetIsCheck_ByFilterIndex(i, true);
                }

                this.dgvFilterList.Refresh();
                this.dgvSocketList.Focus();
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
                for (int i = 0; i < Socket_Cache.FilterList.lstFilter.Count; i++)
                {
                    Socket_Cache.Filter.SetIsCheck_ByFilterIndex(i, false);
                }

                this.dgvFilterList.Refresh();
                this.dgvSocketList.Focus();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//机器人列表操作

        private void dgvRobotList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvRobotList.Rows.Count > 0)
                {
                    int RIndex = e.RowIndex;

                    if (RIndex > -1)
                    {
                        Socket_Operation.ShowRobotForm_Dialog(RIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//机器人列表按钮

        private void tsRobotList_Load_Click(object sender, EventArgs e)
        {
            Socket_Cache.RobotList.LoadRobotList_Dialog();
        }

        private void tsRobotList_Save_Click(object sender, EventArgs e)
        {
            if (dgvRobotList.Rows.Count > 0)
            {
                Socket_Cache.RobotList.SaveRobotList_Dialog(string.Empty, -1);
            }
        }

        private void tsRobotList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.Robot.AddRobot_New();
        }

        private void tsRobotList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvRobotList.Rows.Count > 0)
            {
                Socket_Cache.RobotList.CleanUpRobotList_Dialog();
            }
        }

        #endregion

        #region//文本对比（异步）

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
            Task.Run(async () =>
            {
                try
                {
                    this.rtbComparison_Result.Clear();
                    this.rtbComparison_Result.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_155);
                    this.rtbComparison_Result.Rtf = await Socket_Operation.CompareData(this.Font, this.rtbComparison_A.Text, this.rtbComparison_B.Text);
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });                        
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

        private async void bPacketInfo_Encoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sEncodingText = this.rtbPacketInfo_Encoding.Text;

                string sBytes = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Bytes, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sEncodingText));
                string sANSI_GBK = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.GBK, sEncodingText));

                string sUTF7 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF7, sEncodingText));
                string sANSI_UTF7 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF7, sEncodingText));

                string sUTF8 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF8, sEncodingText));
                string sANSI_UTF8 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF8, sEncodingText));

                string sUTF16 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF16, sEncodingText));
                string sANSI_UTF16 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF16, sEncodingText));

                string sUTF32 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF32, sEncodingText));
                string sANSI_UTF32 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.UTF32, sEncodingText));

                string sUnicode = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Unicode, sEncodingText));
                string sANSI_Unicode = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Unicode, sEncodingText));

                string sBase64 = Socket_Operation.Base64_Encoding(sEncodingText);
                string sANSI_Base64 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sBase64));

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

        private async void bPacketInfo_Decoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sDecodingText = this.rtbPacketInfo_Encoding.Text;

                string sBytes = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Bytes, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_GBK = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.GBK, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF7 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF7, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF7 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF7, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF8 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF8 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF16 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF16, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF16 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF16, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUTF32 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF32, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF32 = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF32, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sUnicode = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Unicode, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Default, sDecodingText));
                string sANSI_Unicode = await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Unicode, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText));

                string sBase64 = Socket_Operation.Base64_Decoding(sDecodingText);
                string sANSI_Base64 = Socket_Operation.Base64_Decoding(await Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Default, Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, sDecodingText)));

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
            this.InitHexBox_XOR();
            this.txtXOR.Clear();
        }

        #endregion

        #region//数据提取        

        private void bExtraction_Click(object sender, EventArgs e)
        {
            try
            {
                this.rtbExtraction.Clear();

                int iSelectIndex = this.cbbExtraction.SelectedIndex;

                if (iSelectIndex == 0)
                {
                    this.ofdExtraction.Filter = "Charles 会话文件（*.chlsx）|*.chlsx";
                }
                else if (iSelectIndex == 1)
                {
                    this.ofdExtraction.Filter = "FILT 过滤器文件（*.filt）|*.filt";
                }

                ofdExtraction.ShowDialog();

                string FilePath = ofdExtraction.FileName;

                if (!string.IsNullOrEmpty(FilePath))
                {
                    if (File.Exists(FilePath))
                    {
                        switch (iSelectIndex)
                        {
                            case 0:

                                #region//Charles XML 会话文件

                                try
                                {
                                    XDocument xdoc_Charles = new XDocument();
                                    xdoc_Charles = XDocument.Load(FilePath);

                                    XElement xeRoot_Charles = xdoc_Charles.Descendants("response").FirstOrDefault();

                                    if (xeRoot_Charles != null)
                                    {
                                        if (xeRoot_Charles.Element("body") != null)
                                        {
                                            string sBody = xeRoot_Charles.Element("body").Value;

                                            byte[] bBody = Convert.FromBase64String(sBody);
                                            this.rtbExtraction.Text = BitConverter.ToString(bBody).Replace("-", " ");
                                        }
                                    }
                                }
                                catch
                                {
                                    //                            
                                }

                                #endregion

                                break;

                            case 1:

                                #region//FILT 过滤器文件

                                string[] lines = File.ReadAllLines(FilePath, Encoding.Default);

                                XDocument xdoc_Filt = new XDocument
                                {
                                    Declaration = new XDeclaration("1.0", "utf-8", "yes")
                                };

                                XElement xeRoot_Filt = new XElement("FilterList");
                                xdoc_Filt.Add(xeRoot_Filt);

                                foreach (string line in lines)
                                {
                                    if (line.IndexOf("￥") >= 0)
                                    {
                                        string[] slFilter = line.Split('￥');

                                        if (slFilter.Length == 35)
                                        {
                                            string s0 = slFilter[0].ToString();//是否指定长度 bool （真，假）
                                            string s1 = slFilter[1].ToString();//指定长度 int
                                            string s2 = slFilter[2].ToString();//是否指定套接字 bool （真，假）
                                            string s3 = slFilter[3].ToString();//套接字 int
                                            string s4 = slFilter[4].ToString();//是否指定包头 bool （真，假）
                                            string s5 = slFilter[5].ToString();//包头 string (十六进制不带空格)
                                            string s6 = slFilter[6].ToString();//未知 bool （真，假）
                                            string s7 = slFilter[7].ToString();//未知 int 0
                                            string s8 = slFilter[8].ToString();//未知 int 0
                                            string s9 = slFilter[9].ToString();//是否替换 bool （真，假）
                                            string s10 = slFilter[10].ToString();//是否拦截 bool （真，假）
                                            string s11 = slFilter[11].ToString();//是否不可视 bool （真，假）
                                            string s12 = slFilter[12].ToString();//步长 int
                                            string s13 = slFilter[13].ToString();//过滤器名称 string
                                            string s14 = slFilter[14].ToString();//发送 bool （1，0）
                                            string s15 = slFilter[15].ToString();//接收 bool （1，0）
                                            string s16 = slFilter[16].ToString();//发送到 bool （1，0）
                                            string s17 = slFilter[17].ToString();//接收自 bool （1，0）
                                            string s18 = slFilter[18].ToString();//WSA发送 bool （1，0）
                                            string s19 = slFilter[19].ToString();//WSA接收 bool （1，0）
                                            string s20 = slFilter[20].ToString();//WSA发送到 bool （1，0）
                                            string s21 = slFilter[21].ToString();//未知 -1
                                            string s22 = slFilter[22].ToString();//普通模式 bool （真，假）
                                            string s23 = slFilter[23].ToString();//高级模式 bool （真，假）
                                            string s24 = slFilter[24].ToString();//数据包开头 bool （真，假）
                                            string s25 = slFilter[25].ToString();//自发式连锁位 bool （真，假）
                                            string s26 = slFilter[26].ToString();//普通-搜索 string （列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s27 = slFilter[27].ToString();//普通-修改 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s28 = slFilter[28].ToString();//高级-搜索 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s29 = slFilter[29].ToString();//高级-修改 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s30 = slFilter[30].ToString();//递进 bool （真，假）
                                            string s31 = slFilter[31].ToString();//普通-修改-递进 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s32 = slFilter[32].ToString();//高级-修改-递进 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s33 = slFilter[33].ToString();//未知 1

                                            string sIsEnable = bool.FalseString;
                                            string sFID = Guid.NewGuid().ToString();
                                            string sFName = s13;
                                            string sIsExecute = bool.FalseString;
                                            string sRID = Guid.Empty.ToString();
                                            string sFAppointHeader = Socket_Operation.GetBoolFromChineseString(s4).ToString();
                                            string sFHeaderContent = s5;
                                            string sFAppointSocket = Socket_Operation.GetBoolFromChineseString(s2).ToString();
                                            string sFSocketContent = s3;
                                            string sFAppointLength = Socket_Operation.GetBoolFromChineseString(s0).ToString();
                                            string sFLengthContent = s1;

                                            Socket_Cache.Filter.FilterMode FMode = new Socket_Cache.Filter.FilterMode();
                                            if (Socket_Operation.GetBoolFromChineseString(s22) == true)
                                            {
                                                FMode = Socket_Cache.Filter.FilterMode.Normal;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s23) == true)
                                            {
                                                FMode = Socket_Cache.Filter.FilterMode.Advanced;
                                            }
                                            string sFMode = ((int)FMode).ToString();

                                            Socket_Cache.Filter.FilterAction FAction = new Socket_Cache.Filter.FilterAction();
                                            if (Socket_Operation.GetBoolFromChineseString(s9) == true)
                                            {
                                                FAction = Socket_Cache.Filter.FilterAction.Replace;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s10) == true)
                                            {
                                                FAction = Socket_Cache.Filter.FilterAction.Intercept;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s11) == true)
                                            {
                                                FAction = Socket_Cache.Filter.FilterAction.NoModify_NoDisplay;
                                            }
                                            else
                                            {
                                                FAction = Socket_Cache.Filter.FilterAction.NoModify_Display;
                                            }
                                            string sFAction = ((int)FAction).ToString();

                                            bool bSend = Convert.ToBoolean(int.Parse(s14));
                                            bool bRecv = Convert.ToBoolean(int.Parse(s15));
                                            bool bSendTo = Convert.ToBoolean(int.Parse(s16));
                                            bool bRecvFrom = Convert.ToBoolean(int.Parse(s17));
                                            bool bWSASend = Convert.ToBoolean(int.Parse(s18));
                                            bool bWSARecv = Convert.ToBoolean(int.Parse(s19));
                                            bool bWSASendTo = Convert.ToBoolean(int.Parse(s20));
                                            bool bWSARecvFrom = false;

                                            Socket_Cache.Filter.FilterFunction filterFunction = new Socket_Cache.Filter.FilterFunction(bSend, bSendTo, bRecv, bRecvFrom, bWSASend, bWSASendTo, bWSARecv, bWSARecvFrom);
                                            string sFFunction = Socket_Cache.Filter.GetFilterFunctionString(filterFunction);

                                            Socket_Cache.Filter.FilterStartFrom FStartFrom = new Socket_Cache.Filter.FilterStartFrom();
                                            if (Socket_Operation.GetBoolFromChineseString(s24) == true)
                                            {
                                                FStartFrom = Socket_Cache.Filter.FilterStartFrom.Head;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s25) == true)
                                            {
                                                FStartFrom = Socket_Cache.Filter.FilterStartFrom.Position;
                                            }
                                            string sFStartFrom = ((int)FStartFrom).ToString();

                                            string sFProgressionStep = s12;
                                            string sFProgressionPosition = string.Empty;

                                            string sFSearch = string.Empty;
                                            string sFModify = string.Empty;
                                            if (FMode == Socket_Cache.Filter.FilterMode.Normal)
                                            {
                                                sFProgressionPosition = Socket_Operation.ConvertFILTString(s31, false);
                                                sFSearch = Socket_Operation.ConvertFILTString(s26, false);
                                                sFModify = Socket_Operation.ConvertFILTString(s27, false);
                                            }
                                            else if (FMode == Socket_Cache.Filter.FilterMode.Advanced)
                                            {
                                                sFProgressionPosition = Socket_Operation.ConvertFILTString(s32, false);
                                                sFSearch = Socket_Operation.ConvertFILTString(s28, false);

                                                if (FStartFrom == Socket_Cache.Filter.FilterStartFrom.Position)
                                                {
                                                    sFModify = Socket_Operation.ConvertFILTString(s29, true);
                                                }
                                                else
                                                {
                                                    sFModify = Socket_Operation.ConvertFILTString(s29, false);
                                                }
                                            }

                                            XElement xeFilter =
                                                new XElement("Filter",
                                                new XElement("IsEnable", sIsEnable),
                                                new XElement("ID", sFID),
                                                new XElement("Name", sFName),
                                                new XElement("AppointHeader", sFAppointHeader),
                                                new XElement("HeaderContent", sFHeaderContent),
                                                new XElement("AppointSocket", sFAppointSocket),
                                                new XElement("SocketContent", sFSocketContent),
                                                new XElement("AppointLength", sFAppointLength),
                                                new XElement("LengthContent", sFLengthContent),
                                                new XElement("Mode", sFMode),
                                                new XElement("Action", sFAction),
                                                new XElement("IsExecute", sIsExecute),
                                                new XElement("RobotID", sRID),
                                                new XElement("Function", sFFunction),
                                                new XElement("StartFrom", sFStartFrom),
                                                new XElement("ProgressionStep", sFProgressionStep),
                                                new XElement("ProgressionPosition", sFProgressionPosition),
                                                new XElement("Search", sFSearch),
                                                new XElement("Modify", sFModify)
                                                );

                                            xeRoot_Filt.Add(xeFilter);
                                        }

                                    }
                                }

                                this.rtbExtraction.Text = xdoc_Filt.Declaration.ToString() + "\r\n" + xdoc_Filt.ToString();

                                #endregion

                                break;
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cmsExtraction_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsExtraction.Close();

            try
            {
                switch (sItemText)
                {
                    case "cmsExtraction_Export":

                        string sFileContent = this.rtbExtraction.Text.Trim();

                        if (!string.IsNullOrEmpty(sFileContent))
                        {
                            int iSelectIndex = this.cbbExtraction.SelectedIndex;

                            switch (iSelectIndex)
                            {
                                case 0:

                                    this.sfdExtraction.Filter = "TXT（*.txt）|*.txt";

                                    break;

                                case 1:

                                    this.sfdExtraction.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";

                                    break;
                            }

                            if (this.sfdExtraction.ShowDialog() == DialogResult.OK)
                            {
                                string sFilePath = this.sfdExtraction.FileName;

                                if (!string.IsNullOrEmpty(sFilePath))
                                {
                                    File.WriteAllText(sFilePath, sFileContent);
                                }
                            }
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
    }
}

﻿using Be.Windows.Forms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using WPELibrary.Lib;
using WPELibrary.Lib.NativeMethods;
using WPELibrary.TextComparison;

namespace WPELibrary
{
    public partial class Socket_Form : Form
    {
        private readonly Socket_Cache.System.SystemMode RunMode = Socket_Cache.System.SystemMode.Process;
        private bool bWakeUp = true;
        private readonly ToolTip tt = new ToolTip();
        private readonly WinSockHook ws = new WinSockHook();

        #region//加载窗体

        public Socket_Form()
        {
            try
            {
                Socket_Cache.System.LoadSystemConfig_FromDB();
                MultiLanguage.SetDefaultLanguage(Socket_Cache.System.DefaultLanguage);

                InitializeComponent();

                Socket_Cache.System.InvokeAction = action =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(action);
                    }
                    else
                    {
                        action();
                    }
                };

                this.InitSocketDGV();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//窗体事件

        private void Socket_Form_Load(object sender, EventArgs e)
        {
            this.InitSocketForm();
            this.InitHexBox_XOR();
            this.LoadConfigs_Parameter();
            this.InitHotKeys();

            Socket_Operation.StartRemoteMGT();
            Socket_Cache.System.LoadSystemList_FromDB();
            Socket_Cache.ProxyAccount.LoadProxyAccountList_FromDB();
            Socket_Cache.ProxyMapping.LoadProxyMapLocal_FromDB();
            Socket_Cache.ProxyMapping.LoadProxyMapRemote_FromDB();
        }

        private void Socket_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveConfigs_Parameter();
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

        public void ExitMainForm()
        {
            try
            {
                ws.ExitHook();
                this.niWPE.Visible = false;

                Socket_Operation.StopRemoteMGT(this.RunMode);
                Socket_Cache.System.SaveSystemList_ToDB();
                Socket_Cache.System.SaveRunConfig_ToDB(this.RunMode);
                Socket_Cache.ProxyAccount.SaveProxyAccountList_ToDB(this.RunMode);
                Socket_Cache.ProxyMapping.SaveProxyMapLocal_ToDB(this.RunMode);
                Socket_Cache.ProxyMapping.SaveProxyMapRemote_ToDB(this.RunMode);
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

                    if (this.tcAutomation.SelectedIndex == 1)
                    {
                        Socket_Cache.Send.DoSend_ByHotKey(HOTKEY_ID);
                    }
                    else if (this.tcAutomation.SelectedIndex == 2)
                    {
                        Socket_Cache.Robot.DoRobot_ByHotKey(HOTKEY_ID);
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
                this.Text = Socket_Cache.System.WPE + " - " + Socket_Operation.AssemblyVersion;

                tt.SetToolTip(cbWorkingMode_Speed, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_22));
                tt.SetToolTip(rbFilterSet_Priority, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_63));
                tt.SetToolTip(rbFilterSet_Sequence, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_64));
                tt.SetToolTip(bSearch, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_25));
                tt.SetToolTip(bSearchNext, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_26));

                Socket_Cache.System.MainHandle = this.Handle;
                string sProcessName = Socket_Operation.GetProcessName();
                this.tsslProcessName.Text = sProcessName;
                this.niWPE.Text = Socket_Cache.System.WPE + "\r\n" + sProcessName;

                this.tSocketInfo.Enabled = true;
                this.tSocketList.Enabled = true;

                this.tsslProcessInfo.Text = Socket_Operation.GetProcessInfo();
                this.tsslWinSock.Text = Socket_Operation.GetWinSockSupportInfo();

                this.bStartHook.Enabled = true;
                this.bStopHook.Enabled = false;
                this.cmsIcon_StartHook.Enabled = true;
                this.cmsIcon_StopHook.Enabled = false;
                this.cbbExtraction.SelectedIndex = 0;

                this.InitFilterActionColor();
                Socket_Operation.InitCPUAndMemoryCounter();

                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketPacket.Total_SendBytes), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketPacket.Total_RecvBytes));

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

        private void InitHotKeys()
        {
            this.txtHotKey1.RegisterHotkeyFromText(9001);
            this.txtHotKey2.RegisterHotkeyFromText(9002);
            this.txtHotKey3.RegisterHotkeyFromText(9003);
            this.txtHotKey4.RegisterHotkeyFromText(9004);
            this.txtHotKey5.RegisterHotkeyFromText(9005);
            this.txtHotKey6.RegisterHotkeyFromText(9006);
            this.txtHotKey7.RegisterHotkeyFromText(9007);
            this.txtHotKey8.RegisterHotkeyFromText(9008);
            this.txtHotKey9.RegisterHotkeyFromText(9009);
            this.txtHotKey10.RegisterHotkeyFromText(9010);
            this.txtHotKey11.RegisterHotkeyFromText(9011);
            this.txtHotKey12.RegisterHotkeyFromText(9012);
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

                dgvFilterList.AutoGenerateColumns = false;
                dgvFilterList.DataSource = Socket_Cache.FilterList.lstFilter;
                dgvFilterList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterList, true, null);

                dgvSendList.AutoGenerateColumns = false;
                dgvSendList.DataSource = Socket_Cache.SendList.lstSend;
                dgvSendList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSendList, true, null);

                dgvRobotList.AutoGenerateColumns = false;
                dgvRobotList.DataSource = Socket_Cache.RobotList.lstRobot;
                dgvRobotList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvRobotList, true, null);

                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstSocketLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//加载本界面的运行配置

        private void LoadConfigs_Parameter()
        {
            try
            {
                Socket_Cache.System.LoadRunConfig_FromDB();

                this.cbHookWS1_Send.Checked = Socket_Cache.SocketPacket.HookWS1_Send;
                this.cbHookWS1_SendTo.Checked = Socket_Cache.SocketPacket.HookWS1_SendTo;
                this.cbHookWS1_Recv.Checked = Socket_Cache.SocketPacket.HookWS1_Recv;
                this.cbHookWS1_RecvFrom.Checked = Socket_Cache.SocketPacket.HookWS1_RecvFrom;
                this.cbHookWS2_Send.Checked = Socket_Cache.SocketPacket.HookWS2_Send;
                this.cbHookWS2_SendTo.Checked = Socket_Cache.SocketPacket.HookWS2_SendTo;
                this.cbHookWS2_Recv.Checked = Socket_Cache.SocketPacket.HookWS2_Recv;
                this.cbHookWS2_RecvFrom.Checked = Socket_Cache.SocketPacket.HookWS2_RecvFrom;
                this.cbHookWSA_Send.Checked = Socket_Cache.SocketPacket.HookWSA_Send;
                this.cbHookWSA_SendTo.Checked = Socket_Cache.SocketPacket.HookWSA_SendTo;
                this.cbHookWSA_Recv.Checked = Socket_Cache.SocketPacket.HookWSA_Recv;
                this.cbHookWSA_RecvFrom.Checked = Socket_Cache.SocketPacket.HookWSA_RecvFrom;

                this.txtHotKey1.Text = Socket_Cache.SocketPacket.HotKey1;
                this.txtHotKey2.Text = Socket_Cache.SocketPacket.HotKey2;
                this.txtHotKey3.Text = Socket_Cache.SocketPacket.HotKey3;
                this.txtHotKey4.Text = Socket_Cache.SocketPacket.HotKey4;
                this.txtHotKey5.Text = Socket_Cache.SocketPacket.HotKey5;
                this.txtHotKey6.Text = Socket_Cache.SocketPacket.HotKey6;
                this.txtHotKey7.Text = Socket_Cache.SocketPacket.HotKey7;
                this.txtHotKey8.Text = Socket_Cache.SocketPacket.HotKey8;
                this.txtHotKey9.Text = Socket_Cache.SocketPacket.HotKey9;
                this.txtHotKey10.Text = Socket_Cache.SocketPacket.HotKey10;
                this.txtHotKey11.Text = Socket_Cache.SocketPacket.HotKey11;
                this.txtHotKey12.Text = Socket_Cache.SocketPacket.HotKey12;

                if (Socket_Cache.SocketPacket.CheckNotShow)
                {
                    this.rbFilter_NotShow.Checked = true;
                }
                else
                {
                    this.rbFilter_Show.Checked = true;
                }

                this.cbCheckSocket.Checked = Socket_Cache.SocketPacket.CheckSocket;
                this.cbCheckIP.Checked = Socket_Cache.SocketPacket.CheckIP;
                this.cbCheckPort.Checked = Socket_Cache.SocketPacket.CheckPort;
                this.cbCheckHead.Checked = Socket_Cache.SocketPacket.CheckHead;
                this.cbCheckData.Checked = Socket_Cache.SocketPacket.CheckData;
                this.cbCheckSize.Checked = Socket_Cache.SocketPacket.CheckSize;

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

                this.cbLogList_AutoRoll.Checked = Socket_Cache.LogList.Socket_AutoRoll;
                this.cbLogList_AutoClear.Checked = Socket_Cache.LogList.Socket_AutoClear;
                this.nudLogList_AutoClearValue.Value = Socket_Cache.LogList.Socket_AutoClear_Value;
                this.LogList_AutoClearChange();

                this.cbWorkingMode_Speed.Checked = Socket_Cache.SocketPacket.SpeedMode;

                switch (Socket_Cache.System.ListExecute)
                {
                    case Socket_Cache.System.Execute.Together:
                        this.rbListExecute_Together.Checked = true;
                        break;

                    case Socket_Cache.System.Execute.Sequence:
                        this.rbListExecute_Sequence.Checked = true;
                        break;
                }

                switch (Socket_Cache.Filter.FilterExecute)
                {
                    case Socket_Cache.Filter.Execute.Priority:
                        this.rbFilterSet_Priority.Checked = true;
                        break;

                    case Socket_Cache.Filter.Execute.Sequence:
                        this.rbFilterSet_Sequence.Checked = true;
                        break;
                }

                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存本页面运行配置

        private void SaveConfigs_Parameter()
        {
            try
            {
                Socket_Cache.SocketPacket.HookWS1_Send = cbHookWS1_Send.Checked;
                Socket_Cache.SocketPacket.HookWS1_SendTo = cbHookWS1_SendTo.Checked;
                Socket_Cache.SocketPacket.HookWS1_Recv = cbHookWS1_Recv.Checked;
                Socket_Cache.SocketPacket.HookWS1_RecvFrom = cbHookWS1_RecvFrom.Checked;
                Socket_Cache.SocketPacket.HookWS2_Send = cbHookWS2_Send.Checked;
                Socket_Cache.SocketPacket.HookWS2_SendTo = cbHookWS2_SendTo.Checked;
                Socket_Cache.SocketPacket.HookWS2_Recv = cbHookWS2_Recv.Checked;
                Socket_Cache.SocketPacket.HookWS2_RecvFrom = cbHookWS2_RecvFrom.Checked;
                Socket_Cache.SocketPacket.HookWSA_Send = cbHookWSA_Send.Checked;
                Socket_Cache.SocketPacket.HookWSA_SendTo = cbHookWSA_SendTo.Checked;
                Socket_Cache.SocketPacket.HookWSA_Recv = cbHookWSA_Recv.Checked;
                Socket_Cache.SocketPacket.HookWSA_RecvFrom = cbHookWSA_RecvFrom.Checked;

                Socket_Cache.SocketPacket.HotKey1 = this.txtHotKey1.Text.Trim();
                Socket_Cache.SocketPacket.HotKey2 = this.txtHotKey2.Text.Trim();
                Socket_Cache.SocketPacket.HotKey3 = this.txtHotKey3.Text.Trim();
                Socket_Cache.SocketPacket.HotKey4 = this.txtHotKey4.Text.Trim();
                Socket_Cache.SocketPacket.HotKey5 = this.txtHotKey5.Text.Trim();
                Socket_Cache.SocketPacket.HotKey6 = this.txtHotKey6.Text.Trim();
                Socket_Cache.SocketPacket.HotKey7 = this.txtHotKey7.Text.Trim();
                Socket_Cache.SocketPacket.HotKey8 = this.txtHotKey8.Text.Trim();
                Socket_Cache.SocketPacket.HotKey9 = this.txtHotKey9.Text.Trim();
                Socket_Cache.SocketPacket.HotKey10 = this.txtHotKey10.Text.Trim();
                Socket_Cache.SocketPacket.HotKey11 = this.txtHotKey11.Text.Trim();
                Socket_Cache.SocketPacket.HotKey12 = this.txtHotKey12.Text.Trim();

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

                Socket_Cache.LogList.Socket_AutoRoll = this.cbLogList_AutoRoll.Checked;
                Socket_Cache.LogList.Socket_AutoClear = this.cbLogList_AutoClear.Checked;
                Socket_Cache.LogList.Socket_AutoClear_Value = this.nudLogList_AutoClearValue.Value;

                Socket_Cache.SocketPacket.SpeedMode = this.cbWorkingMode_Speed.Checked;

                if (this.rbListExecute_Together.Checked)
                {
                    Socket_Cache.System.ListExecute = Socket_Cache.System.Execute.Together;
                }
                else
                {
                    Socket_Cache.System.ListExecute = Socket_Cache.System.Execute.Sequence;
                }

                if (this.rbFilterSet_Priority.Checked)
                {
                    Socket_Cache.Filter.FilterExecute = Socket_Cache.Filter.Execute.Priority;
                }
                else
                {
                    Socket_Cache.Filter.FilterExecute = Socket_Cache.Filter.Execute.Sequence;
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

        private void AutoScrollDataGridView(DataGridView dgv, bool autoScroll)
        {
            if (autoScroll && !dgv.IsDisposed)
            {
                if (dgv.InvokeRequired)
                {
                    dgv.Invoke(new Action(() =>
                    {
                        if (dgv.Rows.Count > 0 && dgv.Height > dgv.RowTemplate.Height)
                        {
                            dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
                        }
                    }));
                }
                else
                {
                    if (dgv.Rows.Count > 0 && dgv.Height > dgv.RowTemplate.Height)
                    {
                        dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
                    }
                }
            }
        }

        #endregion        

        #region//快捷键

        private void bHotKey1_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey1.RegisterHotkeyFromText(9001))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey1.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey2_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey2.RegisterHotkeyFromText(9002))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey2.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey3_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey3.RegisterHotkeyFromText(9003))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey3.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey4_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey4.RegisterHotkeyFromText(9004))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey4.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey5_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey5.RegisterHotkeyFromText(9005))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey5.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey6_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey6.RegisterHotkeyFromText(9006))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey6.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey7_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey7.RegisterHotkeyFromText(9007))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey7.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey8_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey8.RegisterHotkeyFromText(9008))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey8.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey9_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey9.RegisterHotkeyFromText(9009))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey9.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey10_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey10.RegisterHotkeyFromText(9010))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey10.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey11_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey11.RegisterHotkeyFromText(9011))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey11.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
            }
        }

        private void bHotKey12_Click(object sender, EventArgs e)
        {
            if (this.txtHotKey12.RegisterHotkeyFromText(9012))
            {
                string Msg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_133), this.txtHotKey12.Text.Trim());
                Socket_Operation.ShowMessageBox(Msg);
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
            this.TopMost = this.cbTopMost.Checked;
        }

        private void cbWorkingMode_Speed_CheckedChanged(object sender, EventArgs e)
        {
            Socket_Cache.SocketPacket.SpeedMode = this.cbWorkingMode_Speed.Checked;
        }

        private void rbListExecute_Together_CheckedChanged(object sender, EventArgs e)
        {
            this.ListExecute_Changed();
        }

        private void ListExecute_Changed()
        {
            if (this.rbListExecute_Together.Checked)
            {
                Socket_Cache.System.ListExecute = Socket_Cache.System.Execute.Together;
            }
            else
            {
                Socket_Cache.System.ListExecute = Socket_Cache.System.Execute.Sequence;
            }
        }

        private void InitFilterActionColor()
        {
            try
            {
                this.lFAColor_Replace.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Replace;
                this.lFAColor_Replace.BackColor = Socket_Cache.Filter.FilterActionBackColor_Replace;

                this.lFAColor_Intercept.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Intercept;
                this.lFAColor_Intercept.BackColor = Socket_Cache.Filter.FilterActionBackColor_Intercept;

                this.lFAColor_Change.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Change;
                this.lFAColor_Change.BackColor = Socket_Cache.Filter.FilterActionBackColor_Change;

                this.lFAColor_Other.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Other;
                this.lFAColor_Other.BackColor = Socket_Cache.Filter.FilterActionBackColor_Other;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//系统备份

        private void bBackUp_Export_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = Socket_Operation.AssemblyVersion;
                bool SystemConfig = this.cbBackUp_SystemConfig.Checked;
                bool ProxySet = this.cbBackUp_ProxySet.Checked;
                bool ProxyAccount = this.cbBackUp_ProxyAccount.Checked;
                bool ProxyMapping = this.cbBackUp_ProxyMapping.Checked;
                bool InjectionSet = this.cbBackUp_InjectionSet.Checked;
                bool FilterList = this.cbBackUp_FilterList.Checked;
                bool SendList = this.cbBackUp_SendList.Checked;
                bool RobotList = this.cbBackUp_RobotList.Checked;

                Socket_Cache.System.ExportSystemBackUp_Dialog(
                    FileName,
                    SystemConfig,
                    ProxySet,
                    ProxyAccount,
                    ProxyMapping,
                    InjectionSet,
                    FilterList,
                    SendList,
                    RobotList);
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
                Socket_Cache.SocketList.spiSelect = null;
                this.dgvSocketList.Rows.Clear();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_LogList()
        {
            try
            {
                Socket_Cache.LogQueue.ResetLogQueue(Socket_Cache.System.LogType.Socket);
                Socket_Cache.LogList.ResetLogList(Socket_Cache.System.LogType.Socket);
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

        private void AutoCleanUp_SocketList()
        {
            try
            {
                if (this.cbSocketList_AutoClear.Checked && !this.dgvSocketList.IsDisposed)
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
                if (this.cbLogList_AutoClear.Checked && !this.dgvLogList.IsDisposed)
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
                this.gbSystemSet_FilterSet.Enabled = false;

                this.bStartHook.Enabled = false;
                this.bStopHook.Enabled = true;

                this.cmsIcon_StartHook.Enabled = false;
                this.cmsIcon_StopHook.Enabled = true;

                this.SaveConfigs_Parameter();
                Socket_Cache.FilterList.InitFilterList_Count();

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
                this.gbSystemSet_FilterSet.Enabled = true;

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
            try
            {
                this.tlTotal_CNT.Text = Socket_Cache.SocketPacket.TotalPackets.ToString();
                this.tlFilterExecute_CNT.Text = Socket_Cache.Filter.FilterExecute_CNT.ToString();
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

                Socket_Cache.SocketPacket.SocketBytesInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketPacket.Total_SendBytes), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketPacket.Total_RecvBytes));
                this.tsslTotalBytes.Text = Socket_Cache.SocketPacket.SocketBytesInfo;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async void tSocketList_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Socket_Cache.SocketQueue.qSocket_PacketInfo.Count > 0)
                {
                    await Socket_Cache.SocketList.SocketToList();
                    this.AutoScrollDataGridView(dgvSocketList, cbSocketList_AutoRoll.Checked);
                    this.AutoCleanUp_SocketList();
                }

                if (Socket_Cache.LogQueue.qSocket_Log.Count > 0)
                {
                    Socket_Cache.LogList.LogToList(Socket_Cache.System.LogType.Socket);
                    this.AutoScrollDataGridView(dgvLogList, cbLogList_AutoRoll.Checked);
                    this.AutoCleanUp_LogList();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(nameof(tSocketList_Tick), ex.Message);
            }
        }

        #endregion

        #region//显示封包列表（异步）

        private void dgvSocketList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvSocketList.Columns["cTypeImg"].Index)
                {
                    e.Value = Socket_Cache.SocketPacket.GetImg_ByPacketType((Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketType"].Index)
                {
                    e.Value = Socket_Cache.SocketPacket.GetName_ByPacketType((Socket_Cache.SocketPacket.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cData"].Index)
                {
                    switch (Socket_Cache.SocketList.lstRecPacket[e.RowIndex].FilterAction)
                    {
                        case Socket_Cache.Filter.FilterAction.Replace:
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Replace;
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Socket_Cache.Filter.FilterActionBackColor_Replace;
                            break;

                        case Socket_Cache.Filter.FilterAction.Intercept:
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Intercept;
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Socket_Cache.Filter.FilterActionBackColor_Intercept;
                            break;

                        case Socket_Cache.Filter.FilterAction.Change:
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Socket_Cache.Filter.FilterActionForeColor_Change;
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Socket_Cache.Filter.FilterActionBackColor_Change;
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

        #region//显示日志列表（异步）

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

        #region//显示滤镜列表（异步）        

        private void dgvFilterList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvFilterList.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    int FIndex = e.RowIndex;
                    bool bCheck = !bool.Parse(dgvFilterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    dgvFilterList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bCheck;
                    Socket_Cache.FilterList.lstFilter[FIndex].IsEnable = bCheck;
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
                int iSelectIndex = this.dgvFilterList.SelectedRows[0].Index;

                if (iSelectIndex >= 0 && iSelectIndex < Socket_Cache.FilterList.lstFilter.Count)
                {
                    Socket_Operation.ShowFilterForm_Dialog(Socket_Cache.FilterList.lstFilter[iSelectIndex]);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示发送列表（异步）        

        private void dgvSendList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSendList.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    int SIndex = e.RowIndex;
                    bool bCheck = !bool.Parse(dgvSendList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    dgvSendList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bCheck;

                    Socket_Cache.Send.SetIsCheck_BySendIndex(SIndex, bCheck);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSendList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSendList.Rows.Count > 0)
                {
                    int SIndex = e.RowIndex;

                    if (SIndex > -1 && SIndex < Socket_Cache.SendList.lstSend.Count)
                    {
                        Socket_Operation.ShowSendListForm_Dialog(Socket_Cache.SendList.lstSend[SIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示机器人列表（异步）

        private void dgvRobotList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvRobotList.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    int RIndex = e.RowIndex;
                    bool bCheck = !bool.Parse(dgvRobotList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    dgvRobotList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bCheck;
                    Socket_Cache.RobotList.lstRobot[RIndex].IsEnable = bCheck;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvRobotList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iSelectIndex = this.dgvRobotList.SelectedRows[0].Index;

                if (iSelectIndex >= 0 && iSelectIndex < Socket_Cache.RobotList.lstRobot.Count)
                {
                    Socket_Operation.ShowRobotForm_Dialog(Socket_Cache.RobotList.lstRobot[iSelectIndex]);
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
            Socket_Operation.ShowFindForm();

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
            try
            {
                if (Socket_Cache.SocketList.FindOptions.IsValid)
                {
                    long res = this.hbPacketData.Find(Socket_Cache.SocketList.FindOptions);

                    if (res == -1)
                    {
                        Socket_Cache.SocketList.Search_Index += 1;
                        this.SearchSocketListNext();
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
            if (!this.bgwSearch.IsBusy)
            {
                this.bgwSearch.RunWorkerAsync();
            }
        }

        private void bgwSearch_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (dgvSocketList.Rows.Count > 0)
                {
                    if (Socket_Cache.SocketList.FindOptions.IsValid)
                    {
                        byte[] bSearchContent = null;
                        FindType fType = Socket_Cache.SocketList.FindOptions.Type;
                        Socket_Cache.SocketPacket.EncodingFormat efFormat = new Socket_Cache.SocketPacket.EncodingFormat();

                        switch (fType)
                        {
                            case FindType.Text:
                                efFormat = Socket_Cache.SocketPacket.EncodingFormat.UTF7;
                                bSearchContent = Socket_Operation.StringToBytes(efFormat, Socket_Cache.SocketList.FindOptions.Text);
                                break;

                            case FindType.Hex:
                                efFormat = Socket_Cache.SocketPacket.EncodingFormat.Hex;
                                bSearchContent = Socket_Cache.SocketList.FindOptions.Hex;
                                break;
                        }

                        if (rbFromHead.Checked)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                this.dgvSocketList.ClearSelection();
                                this.rbFromIndex.Checked = true;
                                this.hbPacketData.SelectionStart = 0;
                                Socket_Cache.SocketList.Search_Index = 0;
                            }));
                        }

                        e.Result = Socket_Cache.SocketList.SearchForSocketList(Socket_Cache.SocketList.Search_Index, bSearchContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearch_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled && e.Result != null)
                {
                    if (int.TryParse(e.Result.ToString(), out int iSearchResultIndex))
                    {
                        if (iSearchResultIndex >= 0)
                        {
                            this.dgvSocketList.Rows[iSearchResultIndex].Selected = true;
                            this.dgvSocketList.CurrentCell = this.dgvSocketList.Rows[iSearchResultIndex].Cells[0];
                            this.dgvSocketList.FirstDisplayedScrollingRowIndex = iSearchResultIndex;

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

        #endregion        

        #region//显示封包数据        

        private void dgvSocketInfo_SelectionChanged(object sender, EventArgs e)
        {
            this.ShowSelectSocketData();
        }

        private void ShowSelectSocketData()
        {
            try
            {
                if (this.dgvSocketList.SelectedRows.Count > 0)
                {
                    int iSelectIndex = this.dgvSocketList.SelectedRows[0].Index;

                    if (iSelectIndex >= 0 && iSelectIndex < Socket_Cache.SocketList.lstRecPacket.Count)
                    {
                        Socket_Cache.SocketList.Search_Index = iSelectIndex;
                        Socket_Cache.SocketList.spiSelect = Socket_Cache.SocketList.lstRecPacket[iSelectIndex];

                        DynamicByteProvider dbp = new DynamicByteProvider(Socket_Cache.SocketList.spiSelect.PacketBuffer);
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

        #region//显示封包统计（异步）

        private void bPacketStatistics_Click(object sender, EventArgs e)
        {
            if (!this.bgwPacketStatistics.IsBusy)
            {
                int selectedIndex = cbbPacketStatistics.SelectedIndex;
                this.bgwPacketStatistics.RunWorkerAsync(selectedIndex);
            }
        }

        private void bgwPacketStatistics_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int selectedIndex = (int)e.Argument;

                switch (selectedIndex)
                {
                    case 0:
                        e.Result = Socket_Cache.SocketList.StatisticalSocketList_ByPacketLen();
                        break;

                    case 1:
                        e.Result = Socket_Cache.SocketList.StatisticalSocketList_ByPacketSocket();
                        break;

                    case 2:
                        e.Result = Socket_Cache.SocketList.StatisticalFilterList_ByExecutionCount();
                        break;

                    default:
                        e.Result = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwPacketStatistics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.dgvPacketStatistics.DataSource = e.Result;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包编辑器菜单

        private void cmsHexBox_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Socket_Operation.InitSendListComboBox(this.cmsHexBox_tscbSendList);
        }

        private void cmsHexBox_tscbSendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Socket_Cache.SocketList.spiSelect != null)
                {
                    if (this.cmsHexBox_tscbSendList.SelectedItem != null)
                    {
                        Socket_Cache.SendList.SendListItem item = (Socket_Cache.SendList.SendListItem)this.cmsHexBox_tscbSendList.SelectedItem;
                        Guid SID = item.SID;
                        BindingList<Socket_PacketInfo> SCollection = Socket_Cache.Send.GetSendCollection_ByGuid(SID);

                        if (SCollection != null)
                        {
                            int iSocket = Socket_Cache.SocketList.spiSelect.PacketSocket;
                            Socket_Cache.SocketPacket.PacketType ptType = Socket_Cache.SocketList.spiSelect.PacketType;
                            string sIPFrom = Socket_Cache.SocketList.spiSelect.PacketFrom;
                            string sIPTo = Socket_Cache.SocketList.spiSelect.PacketTo;

                            byte[] bBuffer = null;

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
                                bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, Clipboard.GetText());
                            }
                            else
                            {
                                bBuffer = Socket_Cache.SocketList.spiSelect.PacketBuffer;
                            }

                            Socket_Cache.Send.AddSendCollection(SCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
                        }

                        this.cmsHexBox.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cmsHexBox_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsHexBox.Close();

            try
            {
                if (Socket_Cache.SocketList.spiSelect != null)
                {
                    switch (sItemText)
                    {
                        case "cmsHexBox_Send":

                            Socket_Operation.ShowSendForm(Socket_Cache.SocketList.spiSelect);

                            break;

                        case "cmsHexBox_FilterList":

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();

                                byte[] bBuffer = Socket_Operation.StringToBytes(Socket_Cache.SocketPacket.EncodingFormat.Hex, Clipboard.GetText());
                                Socket_Cache.Filter.AddFilter_ByPacketInfo(Socket_Cache.SocketList.spiSelect, bBuffer);
                            }
                            else
                            {
                                Socket_Cache.Filter.AddFilter_ByPacketInfo(Socket_Cache.SocketList.spiSelect, null);
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

        private void cmsSocketList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Socket_Operation.InitSendListComboBox(this.tscbSendList);
        }

        private void tscbSendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.tscbSendList.SelectedItem != null)
                {
                    Socket_Cache.SendList.SendListItem item = (Socket_Cache.SendList.SendListItem)this.tscbSendList.SelectedItem;
                    Guid SID = item.SID;

                    List<Socket_PacketInfo> spiList = Socket_Operation.GetSelectedSocket(this.dgvSocketList);

                    if (spiList.Count > 0)
                    {
                        Socket_Cache.Send.AddSendCollection_ByPacketInfo(SID, spiList);
                    }

                    this.cmsSocketList.Close();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cmsSocketList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsSocketList.Close();

            try
            {
                if (Socket_Cache.SocketList.spiSelect != null)
                {
                    switch (sItemText)
                    {
                        case "cmsSocketList_Send":

                            Socket_Operation.ShowSendForm(Socket_Cache.SocketList.spiSelect);

                            break;

                        case "cmsSocketList_FilterList":

                            Socket_Cache.Filter.AddFilter_ByPacketInfo(Socket_Cache.SocketList.spiSelect, null);

                            break;

                        case "cmsSocketList_SystemSocket":

                            Socket_Cache.System.SystemSocket = Socket_Cache.SocketList.spiSelect.PacketSocket;

                            break;

                        case "cmsSocketList_ShowModified":

                            Socket_Operation.ShowSocketCompareForm(Socket_Cache.SocketList.spiSelect);

                            break;

                        case "cmsSocketList_ToExcel":

                            if (dgvSocketList.Rows.Count > 0)
                            {
                                Socket_Cache.SocketList.SaveSocketList_Dialog();
                            }

                            break;

                        case "cmsSocketList_Comparison_A":

                            this.rtbComparison_A.Text = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Cache.SocketList.spiSelect.PacketBuffer);

                            break;

                        case "cmsSocketList_Comparison_B":

                            this.rtbComparison_B.Text = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, Socket_Cache.SocketList.spiSelect.PacketBuffer);

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

        #region//滤镜列表菜单

        private void cmsFilterList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsFilterList.Close();

            try
            {
                if (dgvFilterList.Rows.Count > 0)
                {
                    List<Socket_FilterInfo> sfiList = Socket_Operation.GetSelectedFilter(this.dgvFilterList);

                    if (sfiList.Count > 0)
                    {
                        switch (sItemText)
                        {
                            case "cmsFilterList_Top":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Top, sfiList);
                                break;

                            case "cmsFilterList_Up":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Up, sfiList);
                                break;

                            case "cmsFilterList_Down":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Down, sfiList);
                                break;

                            case "cmsFilterList_Bottom":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Bottom, sfiList);
                                break;

                            case "cmsFilterList_Copy":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Copy, sfiList);
                                break;

                            case "cmsFilterList_Export":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Export, sfiList);
                                break;

                            case "cmsFilterList_Delete":
                                Socket_Cache.FilterList.UpdateFilterList_ByListAction(Socket_Cache.System.ListAction.Delete, sfiList);
                                break;
                        }

                        this.dgvFilterList.ClearSelection();

                        foreach (Socket_FilterInfo sfi in sfiList)
                        {
                            int iIndex = Socket_Cache.FilterList.lstFilter.IndexOf(sfi);

                            if (iIndex > -1 && iIndex < dgvFilterList.RowCount)
                            {
                                this.dgvFilterList.Rows[iIndex].Selected = true;
                                dgvFilterList.FirstDisplayedScrollingRowIndex = iIndex;
                            }
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

        #region//发送列表菜单

        private void cmsSendList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsSendList.Close();

            try
            {
                if (dgvSendList.Rows.Count > 0)
                {
                    List<Socket_SendInfo> ssiList = Socket_Operation.GetSelectedSend(this.dgvSendList);

                    if (ssiList.Count > 0)
                    {
                        switch (sItemText)
                        {
                            case "cmsSendList_Top":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Top, ssiList);
                                break;

                            case "cmsSendList_Up":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Up, ssiList);
                                break;

                            case "cmsSendList_Down":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Down, ssiList);
                                break;

                            case "cmsSendList_Bottom":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Bottom, ssiList);
                                break;

                            case "cmsSendList_Copy":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Copy, ssiList);
                                break;

                            case "cmsSendList_Export":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Export, ssiList);
                                break;

                            case "cmsSendList_Delete":
                                Socket_Cache.SendList.UpdateSendList_ByListAction(Socket_Cache.System.ListAction.Delete, ssiList);
                                break;
                        }

                        this.dgvSendList.ClearSelection();

                        foreach (Socket_SendInfo ssi in ssiList)
                        {
                            int iIndex = Socket_Cache.SendList.lstSend.IndexOf(ssi);

                            if (iIndex > -1 && iIndex < dgvSendList.RowCount)
                            {
                                this.dgvSendList.Rows[iIndex].Selected = true;
                                dgvSendList.FirstDisplayedScrollingRowIndex = iIndex;
                            }
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
                    List<Socket_RobotInfo> sriList = Socket_Operation.GetSelectedRobot(this.dgvRobotList);

                    if (sriList.Count > 0)
                    {
                        switch (sItemText)
                        {
                            case "cmsRobotList_Top":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Top, sriList);
                                break;

                            case "cmsRobotList_Up":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Up, sriList);
                                break;

                            case "cmsRobotList_Down":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Down, sriList);
                                break;

                            case "cmsRobotList_Bottom":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Bottom, sriList);
                                break;

                            case "cmsRobotList_Copy":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Copy, sriList);
                                break;

                            case "cmsRobotList_Export":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Export, sriList);
                                break;

                            case "cmsRobotList_Delete":
                                Socket_Cache.RobotList.UpdateRobotList_ByListAction(Socket_Cache.System.ListAction.Delete, sriList);
                                break;
                        }

                        this.dgvRobotList.ClearSelection();

                        foreach (Socket_RobotInfo sri in sriList)
                        {
                            int iIndex = Socket_Cache.RobotList.lstRobot.IndexOf(sri);

                            if (iIndex > -1 && iIndex < dgvRobotList.RowCount)
                            {
                                this.dgvRobotList.Rows[iIndex].Selected = true;
                                this.dgvRobotList.FirstDisplayedScrollingRowIndex = iIndex;
                            }
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

        #region//滤镜列表按钮

        private void tsFilterList_Load_Click(object sender, EventArgs e)
        {
            Socket_Cache.FilterList.LoadFilterList_Dialog();
        }

        private void tsFilterList_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    List<Socket_FilterInfo> sfiList = new List<Socket_FilterInfo>();
                    sfiList.AddRange(Socket_Cache.FilterList.lstFilter);

                    Socket_Cache.FilterList.SaveFilterList_Dialog(string.Empty, sfiList);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsFilterList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.Filter.AddFilter_New();

            this.dgvFilterList.ClearSelection();
            this.dgvFilterList.CurrentCell = this.dgvFilterList.Rows[this.dgvFilterList.Rows.Count - 1].Cells[0];
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
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    List<Socket_FilterInfo> sfiList = Socket_Operation.GetSelectedFilter(this.dgvFilterList);

                    foreach (Socket_FilterInfo sfi in sfiList)
                    {
                        sfi.IsEnable = true;
                    }

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
                if (Socket_Cache.FilterList.lstFilter.Count > 0)
                {
                    List<Socket_FilterInfo> sfiList = Socket_Operation.GetSelectedFilter(this.dgvFilterList);

                    foreach (Socket_FilterInfo sfi in sfiList)
                    {
                        sfi.IsEnable = false;
                    }

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

        #region//发送列表按钮

        private void tsSendList_Load_Click(object sender, EventArgs e)
        {
            Socket_Cache.SendList.LoadSendList_Dialog();
        }

        private void tsSendList_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (Socket_Cache.SendList.lstSend.Count > 0)
                {
                    List<Socket_SendInfo> ssiList = new List<Socket_SendInfo>();
                    ssiList.AddRange(Socket_Cache.SendList.lstSend);

                    Socket_Cache.SendList.SaveSendList_Dialog(string.Empty, ssiList);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsSendList_Start_Click(object sender, EventArgs e)
        {
            if (dgvSendList.Rows.Count > 0)
            {
                if (!this.bgwSendList.IsBusy)
                {
                    this.tsSendList_Start.Enabled = false;
                    this.tsSendList_Stop.Enabled = true;
                    Socket_Cache.SendList.lstExecute.Clear();

                    this.bgwSendList.RunWorkerAsync();
                }
            }
        }

        private void tsSendList_Stop_Click(object sender, EventArgs e)
        {
            this.bgwSendList.CancelAsync();            
        }

        private void tsSendList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.Send.AddSend_New();

            this.dgvSendList.ClearSelection();
            this.dgvSendList.CurrentCell = this.dgvSendList.Rows[this.dgvSendList.Rows.Count - 1].Cells[0];
        }

        private void tsSendList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvSendList.Rows.Count > 0)
            {
                Socket_Cache.SendList.CleanUpSendList_Dialog();
            }
        }

        #endregion

        #region//执行发送列表（异步）

        private void bgwSendList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (Socket_SendInfo ssi in Socket_Cache.SendList.lstSend)
                {
                    if (ssi.IsEnable)
                    {
                        Socket_Send ss = Socket_Cache.Send.DoSend(ssi.SID);
                        if (ss != null)
                        {
                            if (Socket_Cache.System.ListExecute == Socket_Cache.System.Execute.Together)
                            { 
                                Socket_Cache.SendList.lstExecute.Add(ss);
                            }
                            else
                            {
                                while (ss.Worker.IsBusy)
                                {
                                    if (this.bgwSendList.CancellationPending)
                                    {
                                        ss.StopSend();

                                        e.Cancel = true;
                                        return;
                                    }

                                    Thread.Sleep(100);
                                }
                            }  
                        }
                    }
                }

                while (Socket_Cache.SendList.lstExecute.Count > 0)
                {
                    foreach (Socket_Send ss in Socket_Cache.SendList.lstExecute.ToList())
                    {
                        if (this.bgwSendList.CancellationPending)
                        {
                            ss.StopSend();                            
                        }

                        if (!ss.Worker.IsBusy)
                        {
                            Socket_Cache.SendList.lstExecute.Remove(ss);
                        }
                    }

                    Thread.Sleep(100);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSendList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tsSendList_Start.Enabled = true;
                this.tsSendList_Stop.Enabled = false;
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
            try
            {
                if (Socket_Cache.RobotList.lstRobot.Count > 0)
                {
                    List<Socket_RobotInfo> sriList = new List<Socket_RobotInfo>();
                    sriList.AddRange(Socket_Cache.RobotList.lstRobot);

                    Socket_Cache.RobotList.SaveRobotList_Dialog(string.Empty, sriList);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsRobotList_Start_Click(object sender, EventArgs e)
        {
            if (dgvRobotList.Rows.Count > 0)
            {
                if (!this.bgwRobotList.IsBusy)
                {
                    this.tsRobotList_Start.Enabled = false;
                    this.tsRobotList_Stop.Enabled = true;
                    Socket_Cache.RobotList.lstExecute.Clear();

                    this.bgwRobotList.RunWorkerAsync();
                }
            }
        }

        private void tsRobotList_Stop_Click(object sender, EventArgs e)
        {
            this.bgwRobotList.CancelAsync();
        }

        private void tsRobotList_Add_Click(object sender, EventArgs e)
        {
            Socket_Cache.Robot.AddRobot_New();

            this.dgvRobotList.ClearSelection();
            this.dgvRobotList.CurrentCell = this.dgvRobotList.Rows[this.dgvRobotList.Rows.Count - 1].Cells[0];
        }

        private void tsRobotList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvRobotList.Rows.Count > 0)
            {
                Socket_Cache.RobotList.CleanUpRobotList_Dialog();
            }
        }

        #endregion

        #region//执行机器人列表（异步）

        private void bgwRobotList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (Socket_RobotInfo sri in Socket_Cache.RobotList.lstRobot)
                {
                    if (sri.IsEnable)
                    {
                        Socket_Robot sr = Socket_Cache.Robot.DoRobot(sri.RID, null);
                        if (sr != null)
                        {
                            if (Socket_Cache.System.ListExecute == Socket_Cache.System.Execute.Together)
                            {
                                Socket_Cache.RobotList.lstExecute.Add(sr);
                            }
                            else
                            {
                                while (sr.Worker.IsBusy)
                                {
                                    if (this.bgwRobotList.CancellationPending)
                                    {
                                        sr.StopRobot();

                                        e.Cancel = true;
                                        return;
                                    }

                                    Thread.Sleep(100);
                                }
                            }                                
                        }
                    }
                }

                while (Socket_Cache.RobotList.lstExecute.Count > 0)
                {
                    foreach (Socket_Robot sr in Socket_Cache.RobotList.lstExecute.ToList())
                    {
                        if (this.bgwRobotList.CancellationPending)
                        {
                            sr.StopRobot();
                        }

                        if (!sr.Worker.IsBusy)
                        {
                            Socket_Cache.RobotList.lstExecute.Remove(sr);
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwRobotList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tsRobotList_Start.Enabled = true;
                this.tsRobotList_Stop.Enabled = false;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//文本对比

        private void bTextCompare_Click(object sender, EventArgs e)
        {
            string TextA = this.rtbComparison_A.Text.Trim();
            string TextB = this.rtbComparison_B.Text.Trim();

            if (!Socket_Cache.System.IsShow_TextCompare)
            {
                TextCompareForm tcForm = new TextCompareForm(TextA, TextB);
                tcForm.Show();
            }            
        }

        private void bTextDuplicate_Click(object sender, EventArgs e)
        {
            string TextA = this.rtbComparison_A.Text.Trim();
            string TextB = this.rtbComparison_B.Text.Trim();

            if (!Socket_Cache.System.IsShow_TextDuplicate)
            {
                TextDuplicateForm tdForm = new TextDuplicateForm(TextA, TextB);
                tdForm.Show();
            }
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
                if (dbpXOR_From == null)
                {
                    return;
                }

                byte[] blXOR_From = dbpXOR_From.Bytes.ToArray();

                string sXOR_Value = this.txtXOR.Text.Trim();
                string[] slXOR_Value = sXOR_Value.Split(' ');

                if (blXOR_From.Length == 0 || string.IsNullOrEmpty(sXOR_Value) || slXOR_Value.Length == 0)
                {
                    return;
                }

                byte[] blXOR_To = new byte[blXOR_From.Length];
                int j = 0;

                foreach (byte bXOR_From in blXOR_From)
                {
                    if (j == slXOR_Value.Length)
                    {
                        j = 0;
                    }

                    if (!Byte.TryParse(slXOR_Value[j], System.Globalization.NumberStyles.HexNumber, null, out byte bXOR_Value))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_21));
                        return;
                    }

                    blXOR_To[j] = (byte)(bXOR_From ^ bXOR_Value);
                    j++;
                }

                DynamicByteProvider dbpXOR_To = new DynamicByteProvider(blXOR_To);
                this.hbXOR_To.ByteProvider = dbpXOR_To;
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

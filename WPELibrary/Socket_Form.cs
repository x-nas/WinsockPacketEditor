using Be.Windows.Forms;
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
using WPE.Lib;
using WPE.Lib.NativeMethods;
using WPE.Lib.Forms;

namespace WPELibrary
{
    public partial class Socket_Form : Form
    {
        private readonly Operate.SystemConfig.SystemMode RunMode = Operate.SystemConfig.SystemMode.Process;
        private bool bWakeUp = true;
        private readonly ToolTip tt = new ToolTip();
        private readonly Hook ws = new Hook();

        #region//加载窗体

        public Socket_Form()
        {
            try
            {
                Operate.SystemConfig.LoadSystemConfig_FromDB();

                InitializeComponent();

                Operate.SystemConfig.InvokeAction = action =>
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            Operate.SystemConfig.LoadSystemList_FromDB();
            Operate.ProxyConfig.ProxyAccount.LoadProxyAccountList_FromDB();
            Operate.ProxyConfig.ProxyMapping.LoadProxyMapLocal_FromDB();
            Operate.ProxyConfig.ProxyMapping.LoadProxyMapRemote_FromDB();
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        public void ExitMainForm()
        {
            try
            {
                ws.ExitHook();
                this.niWPE.Visible = false;

                Socket_Operation.StopRemoteMGT(this.RunMode);
                Operate.SystemConfig.SaveSystemList_ToDB();
                Operate.SystemConfig.SaveRunConfig_ToDB(this.RunMode);
                Operate.ProxyConfig.ProxyAccount.SaveProxyAccountList_ToDB(this.RunMode);
                Operate.ProxyConfig.ProxyMapping.SaveProxyMapLocal_ToDB(this.RunMode);
                Operate.ProxyConfig.ProxyMapping.SaveProxyMapRemote_ToDB(this.RunMode);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.SendConfig.Send.DoSend_ByHotKey(HOTKEY_ID);
                    }
                    else if (this.tcAutomation.SelectedIndex == 2)
                    {
                        Operate.RobotConfig.Robot.DoRobot_ByHotKey(HOTKEY_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            base.WndProc(ref m);
        }

        #endregion

        #region//初始化

        private void InitSocketForm()
        {
            try
            {
                this.Text = "WPE x64 - " + Operate.SystemConfig.AssemblyVersion;

                tt.SetToolTip(cbWorkingMode_Speed, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_22));
                tt.SetToolTip(rbFilterSet_Priority, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_63));
                tt.SetToolTip(rbFilterSet_Sequence, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_64));
                tt.SetToolTip(bSearch, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_25));
                tt.SetToolTip(bSearchNext, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_26));

                Operate.SystemConfig.MainHandle = this.Handle;
                string sProcessName = Socket_Operation.GetProcessName();
                this.tsslProcessName.Text = sProcessName;
                this.niWPE.Text = "WPE x64\r\n" + sProcessName;

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

                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), Socket_Operation.GetDisplayBytes(Operate.PacketConfig.Packet.Total_SendBytes), Socket_Operation.GetDisplayBytes(Operate.PacketConfig.Packet.Total_RecvBytes));

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sProcessName);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                dgvSocketList.DataSource = Operate.PacketConfig.List.lstRecPacket;
                dgvSocketList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketList, true, null);

                dgvFilterList.AutoGenerateColumns = false;
                dgvFilterList.DataSource = Operate.FilterConfig.List.lstFilter;
                dgvFilterList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterList, true, null);

                dgvSendList.AutoGenerateColumns = false;
                dgvSendList.DataSource = Operate.SendConfig.SendList.lstSend;
                dgvSendList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSendList, true, null);

                dgvRobotList.AutoGenerateColumns = false;
                dgvRobotList.DataSource = Operate.RobotConfig.RobotList.lstRobot;
                dgvRobotList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvRobotList, true, null);

                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Operate.LogConfig.lstLogInfo;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//加载本界面的运行配置

        private void LoadConfigs_Parameter()
        {
            try
            {
                Operate.SystemConfig.LoadRunConfig_FromDB();

                this.cbHookWS1_Send.Checked = Operate.PacketConfig.Packet.HookWS1_Send;
                this.cbHookWS1_SendTo.Checked = Operate.PacketConfig.Packet.HookWS1_SendTo;
                this.cbHookWS1_Recv.Checked = Operate.PacketConfig.Packet.HookWS1_Recv;
                this.cbHookWS1_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWS1_RecvFrom;
                this.cbHookWS2_Send.Checked = Operate.PacketConfig.Packet.HookWS2_Send;
                this.cbHookWS2_SendTo.Checked = Operate.PacketConfig.Packet.HookWS2_SendTo;
                this.cbHookWS2_Recv.Checked = Operate.PacketConfig.Packet.HookWS2_Recv;
                this.cbHookWS2_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWS2_RecvFrom;
                this.cbHookWSA_Send.Checked = Operate.PacketConfig.Packet.HookWSA_Send;
                this.cbHookWSA_SendTo.Checked = Operate.PacketConfig.Packet.HookWSA_SendTo;
                this.cbHookWSA_Recv.Checked = Operate.PacketConfig.Packet.HookWSA_Recv;
                this.cbHookWSA_RecvFrom.Checked = Operate.PacketConfig.Packet.HookWSA_RecvFrom;

                this.txtHotKey1.Text = Operate.PacketConfig.Packet.HotKey1;
                this.txtHotKey2.Text = Operate.PacketConfig.Packet.HotKey2;
                this.txtHotKey3.Text = Operate.PacketConfig.Packet.HotKey3;
                this.txtHotKey4.Text = Operate.PacketConfig.Packet.HotKey4;
                this.txtHotKey5.Text = Operate.PacketConfig.Packet.HotKey5;
                this.txtHotKey6.Text = Operate.PacketConfig.Packet.HotKey6;
                this.txtHotKey7.Text = Operate.PacketConfig.Packet.HotKey7;
                this.txtHotKey8.Text = Operate.PacketConfig.Packet.HotKey8;
                this.txtHotKey9.Text = Operate.PacketConfig.Packet.HotKey9;
                this.txtHotKey10.Text = Operate.PacketConfig.Packet.HotKey10;
                this.txtHotKey11.Text = Operate.PacketConfig.Packet.HotKey11;
                this.txtHotKey12.Text = Operate.PacketConfig.Packet.HotKey12;

                if (Operate.PacketConfig.Packet.CheckNotShow)
                {
                    this.rbFilter_NotShow.Checked = true;
                }
                else
                {
                    this.rbFilter_Show.Checked = true;
                }

                this.cbCheckSocket.Checked = Operate.PacketConfig.Packet.CheckSocket;
                this.cbCheckIP.Checked = Operate.PacketConfig.Packet.CheckIP;
                this.cbCheckPort.Checked = Operate.PacketConfig.Packet.CheckPort;
                this.cbCheckHead.Checked = Operate.PacketConfig.Packet.CheckHead;
                this.cbCheckData.Checked = Operate.PacketConfig.Packet.CheckData;
                this.cbCheckSize.Checked = Operate.PacketConfig.Packet.CheckLen;

                this.txtCheckSocket.Text = Operate.PacketConfig.Packet.CheckSocket_Value;
                this.txtCheckLength.Text = Operate.PacketConfig.Packet.CheckLength_Value;
                this.txtCheckIP.Text = Operate.PacketConfig.Packet.CheckIP_Value;
                this.txtCheckPort.Text = Operate.PacketConfig.Packet.CheckPort_Value;
                this.txtCheckHead.Text = Operate.PacketConfig.Packet.CheckHead_Value;
                this.txtCheckData.Text = Operate.PacketConfig.Packet.CheckData_Value;

                this.cbSocketList_AutoRoll.Checked = Operate.PacketConfig.List.AutoRoll;
                this.cbSocketList_AutoClear.Checked = Operate.PacketConfig.List.AutoClear;
                this.nudSocketList_AutoClearValue.Value = Operate.PacketConfig.List.AutoClear_Value;
                this.SocketList_AutoClearChange();

                this.cbLogList_AutoRoll.Checked = Operate.LogConfig.AutoRoll;
                this.cbLogList_AutoClear.Checked = Operate.LogConfig.AutoClear;
                this.nudLogList_AutoClearValue.Value = Operate.LogConfig.AutoClear_Value;
                this.LogList_AutoClearChange();

                this.cbWorkingMode_Speed.Checked = Operate.PacketConfig.Packet.SpeedMode;

                switch (Operate.SystemConfig.ListExecute)
                {
                    case Operate.SystemConfig.Execute.Together:
                        this.rbListExecute_Together.Checked = true;
                        break;

                    case Operate.SystemConfig.Execute.Sequence:
                        this.rbListExecute_Sequence.Checked = true;
                        break;
                }

                switch (Operate.FilterConfig.Filter.FilterExecute)
                {
                    case Operate.FilterConfig.Filter.Execute.Priority:
                        this.rbFilterSet_Priority.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.Execute.Sequence:
                        this.rbFilterSet_Sequence.Checked = true;
                        break;
                }

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存本页面运行配置

        private void SaveConfigs_Parameter()
        {
            try
            {
                Operate.PacketConfig.Packet.HookWS1_Send = cbHookWS1_Send.Checked;
                Operate.PacketConfig.Packet.HookWS1_SendTo = cbHookWS1_SendTo.Checked;
                Operate.PacketConfig.Packet.HookWS1_Recv = cbHookWS1_Recv.Checked;
                Operate.PacketConfig.Packet.HookWS1_RecvFrom = cbHookWS1_RecvFrom.Checked;
                Operate.PacketConfig.Packet.HookWS2_Send = cbHookWS2_Send.Checked;
                Operate.PacketConfig.Packet.HookWS2_SendTo = cbHookWS2_SendTo.Checked;
                Operate.PacketConfig.Packet.HookWS2_Recv = cbHookWS2_Recv.Checked;
                Operate.PacketConfig.Packet.HookWS2_RecvFrom = cbHookWS2_RecvFrom.Checked;
                Operate.PacketConfig.Packet.HookWSA_Send = cbHookWSA_Send.Checked;
                Operate.PacketConfig.Packet.HookWSA_SendTo = cbHookWSA_SendTo.Checked;
                Operate.PacketConfig.Packet.HookWSA_Recv = cbHookWSA_Recv.Checked;
                Operate.PacketConfig.Packet.HookWSA_RecvFrom = cbHookWSA_RecvFrom.Checked;

                Operate.PacketConfig.Packet.HotKey1 = this.txtHotKey1.Text.Trim();
                Operate.PacketConfig.Packet.HotKey2 = this.txtHotKey2.Text.Trim();
                Operate.PacketConfig.Packet.HotKey3 = this.txtHotKey3.Text.Trim();
                Operate.PacketConfig.Packet.HotKey4 = this.txtHotKey4.Text.Trim();
                Operate.PacketConfig.Packet.HotKey5 = this.txtHotKey5.Text.Trim();
                Operate.PacketConfig.Packet.HotKey6 = this.txtHotKey6.Text.Trim();
                Operate.PacketConfig.Packet.HotKey7 = this.txtHotKey7.Text.Trim();
                Operate.PacketConfig.Packet.HotKey8 = this.txtHotKey8.Text.Trim();
                Operate.PacketConfig.Packet.HotKey9 = this.txtHotKey9.Text.Trim();
                Operate.PacketConfig.Packet.HotKey10 = this.txtHotKey10.Text.Trim();
                Operate.PacketConfig.Packet.HotKey11 = this.txtHotKey11.Text.Trim();
                Operate.PacketConfig.Packet.HotKey12 = this.txtHotKey12.Text.Trim();

                Operate.PacketConfig.Packet.CheckNotShow = rbFilter_NotShow.Checked;
                Operate.PacketConfig.Packet.CheckSocket = cbCheckSocket.Checked;
                Operate.PacketConfig.Packet.CheckIP = cbCheckIP.Checked;
                Operate.PacketConfig.Packet.CheckPort = cbCheckPort.Checked;
                Operate.PacketConfig.Packet.CheckHead = cbCheckHead.Checked;
                Operate.PacketConfig.Packet.CheckData = cbCheckData.Checked;
                Operate.PacketConfig.Packet.CheckLen = cbCheckSize.Checked;

                Operate.PacketConfig.Packet.CheckSocket_Value = this.txtCheckSocket.Text.Trim();
                Operate.PacketConfig.Packet.CheckLength_Value = this.txtCheckLength.Text.Trim();
                Operate.PacketConfig.Packet.CheckIP_Value = this.txtCheckIP.Text.Trim();
                Operate.PacketConfig.Packet.CheckPort_Value = this.txtCheckPort.Text.Trim();
                Operate.PacketConfig.Packet.CheckHead_Value = this.txtCheckHead.Text.Trim();
                Operate.PacketConfig.Packet.CheckData_Value = this.txtCheckData.Text.Trim();

                Operate.PacketConfig.List.AutoRoll = this.cbSocketList_AutoRoll.Checked;
                Operate.PacketConfig.List.AutoClear = this.cbSocketList_AutoClear.Checked;
                Operate.PacketConfig.List.AutoClear_Value = this.nudSocketList_AutoClearValue.Value;

                Operate.LogConfig.AutoRoll = this.cbLogList_AutoRoll.Checked;
                Operate.LogConfig.AutoClear = this.cbLogList_AutoClear.Checked;
                Operate.LogConfig.AutoClear_Value = this.nudLogList_AutoClearValue.Value;

                Operate.PacketConfig.Packet.SpeedMode = this.cbWorkingMode_Speed.Checked;

                if (this.rbListExecute_Together.Checked)
                {
                    Operate.SystemConfig.ListExecute = Operate.SystemConfig.Execute.Together;
                }
                else
                {
                    Operate.SystemConfig.ListExecute = Operate.SystemConfig.Execute.Sequence;
                }

                if (this.rbFilterSet_Priority.Checked)
                {
                    Operate.FilterConfig.Filter.FilterExecute = Operate.FilterConfig.Filter.Execute.Priority;
                }
                else
                {
                    Operate.FilterConfig.Filter.FilterExecute = Operate.FilterConfig.Filter.Execute.Sequence;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            Operate.PacketConfig.Packet.SpeedMode = this.cbWorkingMode_Speed.Checked;
        }

        private void rbListExecute_Together_CheckedChanged(object sender, EventArgs e)
        {
            this.ListExecute_Changed();
        }

        private void ListExecute_Changed()
        {
            if (this.rbListExecute_Together.Checked)
            {
                Operate.SystemConfig.ListExecute = Operate.SystemConfig.Execute.Together;
            }
            else
            {
                Operate.SystemConfig.ListExecute = Operate.SystemConfig.Execute.Sequence;
            }
        }

        private void InitFilterActionColor()
        {
            try
            {
                this.lFAColor_Replace.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Replace;
                this.lFAColor_Replace.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Replace;

                this.lFAColor_Intercept.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Intercept;
                this.lFAColor_Intercept.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Intercept;

                this.lFAColor_Change.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Change;
                this.lFAColor_Change.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Change;

                this.lFAColor_Other.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Other;
                this.lFAColor_Other.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Other;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//系统备份

        private void bBackUp_Export_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = Operate.SystemConfig.AssemblyVersion;
                bool SystemConfig = this.cbBackUp_SystemConfig.Checked;
                bool ProxySet = this.cbBackUp_ProxySet.Checked;
                bool ProxyAccount = this.cbBackUp_ProxyAccount.Checked;
                bool ProxyMapping = this.cbBackUp_ProxyMapping.Checked;
                bool InjectionSet = this.cbBackUp_InjectionSet.Checked;
                bool FilterList = this.cbBackUp_FilterList.Checked;
                bool SendList = this.cbBackUp_SendList.Checked;
                bool RobotList = this.cbBackUp_RobotList.Checked;

                Operate.SystemConfig.ExportSystemBackUp_Dialog(
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.PacketConfig.Packet.TotalPackets = 0;
                Operate.PacketConfig.Packet.Total_SendBytes = 0;
                Operate.PacketConfig.Packet.Total_RecvBytes = 0;
                Operate.FilterConfig.Filter.FilterExecute_CNT = 0;

                Operate.PacketConfig.Queue.FilterSocketList_CNT = 0;
                Operate.PacketConfig.Queue.Send_CNT = 0;
                Operate.PacketConfig.Queue.Recv_CNT = 0;
                Operate.PacketConfig.Queue.SendTo_CNT = 0;
                Operate.PacketConfig.Queue.RecvFrom_CNT = 0;
                Operate.PacketConfig.Queue.WSASend_CNT = 0;
                Operate.PacketConfig.Queue.WSARecv_CNT = 0;
                Operate.PacketConfig.Queue.WSASendTo_CNT = 0;
                Operate.PacketConfig.Queue.WSARecvFrom_CNT = 0;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_SocketList()
        {
            try
            {
                Operate.PacketConfig.Queue.ClearPacketQueue();
                Operate.PacketConfig.List.lstRecPacket.Clear();
                Operate.PacketConfig.List.spiSelect = null;
                this.dgvSocketList.Rows.Clear();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_LogList()
        {
            try
            {
                Operate.LogConfig.ClearLogQueue();
                Operate.LogConfig.ClearLogList();
                this.dgvLogList.Rows.Clear();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.FilterConfig.List.InitFilterList_Count();

                ws.StartHook();

                if (this.cbWorkingMode_Speed.Checked)
                {
                    this.CleanUp_MainForm();
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_41));
                }

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_39));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_40));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//计时器

        private void tSocketInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tlTotal_CNT.Text = Operate.PacketConfig.Packet.TotalPackets.ToString();
                this.tlFilterExecute_CNT.Text = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
                this.tlQueue_CNT.Text = Operate.PacketConfig.Queue.cqPacketInfo.Count.ToString();
                this.tlFilterSocketList_CNT.Text = Operate.PacketConfig.Queue.FilterSocketList_CNT.ToString();
                this.tlSend_CNT.Text = Operate.PacketConfig.Queue.Send_CNT.ToString();
                this.tlRecv_CNT.Text = Operate.PacketConfig.Queue.Recv_CNT.ToString();
                this.tlSendTo_CNT.Text = Operate.PacketConfig.Queue.SendTo_CNT.ToString();
                this.tlRecvFrom_CNT.Text = Operate.PacketConfig.Queue.RecvFrom_CNT.ToString();
                this.tlWSASend_CNT.Text = Operate.PacketConfig.Queue.WSASend_CNT.ToString();
                this.tlWSARecv_CNT.Text = Operate.PacketConfig.Queue.WSARecv_CNT.ToString();
                this.tlWSASendTo_CNT.Text = Operate.PacketConfig.Queue.WSASendTo_CNT.ToString();
                this.tlWSARecvFrom_CNT.Text = Operate.PacketConfig.Queue.WSARecvFrom_CNT.ToString();

                Operate.PacketConfig.Packet.SocketBytesInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_31), Socket_Operation.GetDisplayBytes(Operate.PacketConfig.Packet.Total_SendBytes), Socket_Operation.GetDisplayBytes(Operate.PacketConfig.Packet.Total_RecvBytes));
                this.tsslTotalBytes.Text = Operate.PacketConfig.Packet.SocketBytesInfo;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async void tSocketList_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Operate.PacketConfig.Queue.cqPacketInfo.Count > 0)
                {
                    await Operate.PacketConfig.List.SocketToList();
                    this.AutoScrollDataGridView(dgvSocketList, cbSocketList_AutoRoll.Checked);
                    this.AutoCleanUp_SocketList();
                }

                if (Operate.LogConfig.cqLogInfo.Count > 0)
                {
                    Operate.LogConfig.LogToList();
                    this.AutoScrollDataGridView(dgvLogList, cbLogList_AutoRoll.Checked);
                    this.AutoCleanUp_LogList();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(tSocketList_Tick), ex.Message);
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
                    e.Value = Operate.PacketConfig.Packet.GetImg_ByPacketType((Operate.PacketConfig.Packet.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketType"].Index)
                {
                    e.Value = Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)dgvSocketList.Rows[e.RowIndex].Cells["cPacketType"].Value);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cPacketID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSocketList.Columns["cData"].Index)
                {
                    switch (Operate.PacketConfig.List.lstRecPacket[e.RowIndex].FilterAction)
                    {
                        case Operate.FilterConfig.Filter.FilterAction.Replace:
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Replace;
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Replace;
                            break;

                        case Operate.FilterConfig.Filter.FilterAction.Intercept:
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Intercept;
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Intercept;
                            break;

                        case Operate.FilterConfig.Filter.FilterAction.Change:
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Change;
                            this.dgvSocketList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Change;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.FilterConfig.List.lstFilter[FIndex].IsEnable = bCheck;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvFilterList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iSelectIndex = this.dgvFilterList.SelectedRows[0].Index;

                if (iSelectIndex >= 0 && iSelectIndex < Operate.FilterConfig.List.lstFilter.Count)
                {
                    Socket_Operation.ShowFilterForm_Dialog(Operate.FilterConfig.List.lstFilter[iSelectIndex]);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    Operate.SendConfig.Send.SetIsCheck_BySendIndex(SIndex, bCheck);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSendList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSendList.Rows.Count > 0)
                {
                    int SIndex = e.RowIndex;

                    if (SIndex > -1 && SIndex < Operate.SendConfig.SendList.lstSend.Count)
                    {
                        Socket_Operation.ShowSendListForm_Dialog(Operate.SendConfig.SendList.lstSend[SIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.RobotConfig.RobotList.lstRobot[RIndex].IsEnable = bCheck;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvRobotList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iSelectIndex = this.dgvRobotList.SelectedRows[0].Index;

                if (iSelectIndex >= 0 && iSelectIndex < Operate.RobotConfig.RobotList.lstRobot.Count)
                {
                    Socket_Operation.ShowRobotForm_Dialog(Operate.RobotConfig.RobotList.lstRobot[iSelectIndex]);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//搜索封包内容（异步）

        private void bSearch_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowFindForm();

            if (Operate.PacketConfig.List.DoSearch)
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
                if (Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    long res = this.hbPacketData.Find(Operate.PacketConfig.List.FindOptions);

                    if (res == -1)
                    {
                        Operate.PacketConfig.List.Search_Index += 1;
                        this.SearchSocketListNext();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    if (Operate.PacketConfig.List.FindOptions.IsValid)
                    {
                        byte[] bSearchContent = null;
                        FindType fType = Operate.PacketConfig.List.FindOptions.Type;
                        Operate.PacketConfig.Packet.EncodingFormat efFormat = new Operate.PacketConfig.Packet.EncodingFormat();

                        switch (fType)
                        {
                            case FindType.Text:
                                efFormat = Operate.PacketConfig.Packet.EncodingFormat.UTF7;
                                bSearchContent = Socket_Operation.StringToBytes(efFormat, Operate.PacketConfig.List.FindOptions.Text);
                                break;

                            case FindType.Hex:
                                efFormat = Operate.PacketConfig.Packet.EncodingFormat.Hex;
                                bSearchContent = Operate.PacketConfig.List.FindOptions.Hex;
                                break;
                        }

                        if (rbFromHead.Checked)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                this.dgvSocketList.ClearSelection();
                                this.rbFromIndex.Checked = true;
                                this.hbPacketData.SelectionStart = 0;
                                Operate.PacketConfig.List.Search_Index = 0;
                            }));
                        }

                        e.Result = Operate.PacketConfig.List.SearchForSocketList(Operate.PacketConfig.List.Search_Index, bSearchContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    if (iSelectIndex >= 0 && iSelectIndex < Operate.PacketConfig.List.lstRecPacket.Count)
                    {
                        Operate.PacketConfig.List.Search_Index = iSelectIndex;
                        Operate.PacketConfig.List.spiSelect = Operate.PacketConfig.List.lstRecPacket[iSelectIndex];

                        DynamicByteProvider dbp = new DynamicByteProvider(Operate.PacketConfig.List.spiSelect.PacketBuffer);
                        hbPacketData.ByteProvider = dbp;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        e.Result = Operate.PacketConfig.List.StatisticalSocketList_ByPacketLen();
                        break;

                    case 1:
                        e.Result = Operate.PacketConfig.List.StatisticalSocketList_ByPacketSocket();
                        break;

                    case 2:
                        e.Result = Operate.PacketConfig.List.StatisticalFilterList_ByExecutionCount();
                        break;

                    default:
                        e.Result = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                if (Operate.PacketConfig.List.spiSelect != null)
                {
                    if (this.cmsHexBox_tscbSendList.SelectedItem != null)
                    {
                        Operate.SendConfig.SendList.SendListItem item = (Operate.SendConfig.SendList.SendListItem)this.cmsHexBox_tscbSendList.SelectedItem;
                        Guid SID = item.SID;
                        BindingList<PacketInfo> SCollection = Operate.SendConfig.Send.GetSendCollection_ByGuid(SID);

                        if (SCollection != null)
                        {
                            int iSocket = Operate.PacketConfig.List.spiSelect.PacketSocket;
                            Operate.PacketConfig.Packet.PacketType ptType = Operate.PacketConfig.List.spiSelect.PacketType;
                            string sIPFrom = Operate.PacketConfig.List.spiSelect.PacketFrom;
                            string sIPTo = Operate.PacketConfig.List.spiSelect.PacketTo;

                            byte[] bBuffer = null;

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
                                bBuffer = Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                            }
                            else
                            {
                                bBuffer = Operate.PacketConfig.List.spiSelect.PacketBuffer;
                            }

                            Operate.SendConfig.Send.AddSendCollection(SCollection, iSocket, ptType, sIPFrom, sIPTo, bBuffer);
                        }

                        this.cmsHexBox.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cmsHexBox_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsHexBox.Close();

            try
            {
                if (Operate.PacketConfig.List.spiSelect != null)
                {
                    switch (sItemText)
                    {
                        case "cmsHexBox_Send":

                            Socket_Operation.ShowSendForm(Operate.PacketConfig.List.spiSelect);

                            break;

                        case "cmsHexBox_FilterList":

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();

                                byte[] bBuffer = Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.spiSelect, bBuffer);
                            }
                            else
                            {
                                Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.spiSelect, null);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.SendConfig.SendList.SendListItem item = (Operate.SendConfig.SendList.SendListItem)this.tscbSendList.SelectedItem;
                    Guid SID = item.SID;

                    List<PacketInfo> spiList = Socket_Operation.GetSelectedSocket(this.dgvSocketList);

                    if (spiList.Count > 0)
                    {
                        Operate.SendConfig.Send.AddSendCollection_ByPacketInfo(SID, spiList);
                    }

                    this.cmsSocketList.Close();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cmsSocketList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsSocketList.Close();

            try
            {
                if (Operate.PacketConfig.List.spiSelect != null)
                {
                    switch (sItemText)
                    {
                        case "cmsSocketList_Send":

                            Socket_Operation.ShowSendForm(Operate.PacketConfig.List.spiSelect);

                            break;

                        case "cmsSocketList_FilterList":

                            Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.spiSelect, null);

                            break;

                        case "cmsSocketList_SystemSocket":

                            Operate.SystemConfig.SystemSocket = Operate.PacketConfig.List.spiSelect.PacketSocket;

                            break;

                        case "cmsSocketList_ShowModified":

                            Socket_Operation.ShowSocketCompareForm(Operate.PacketConfig.List.spiSelect);

                            break;

                        case "cmsSocketList_ToExcel":

                            if (dgvSocketList.Rows.Count > 0)
                            {
                                Operate.PacketConfig.List.SaveSocketList_Dialog();
                            }

                            break;

                        case "cmsSocketList_Comparison_A":

                            this.rtbComparison_A.Text = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.PacketConfig.List.spiSelect.PacketBuffer);

                            break;

                        case "cmsSocketList_Comparison_B":

                            this.rtbComparison_B.Text = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.PacketConfig.List.spiSelect.PacketBuffer);

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Top, sfiList);
                                break;

                            case "cmsFilterList_Up":
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Up, sfiList);
                                break;

                            case "cmsFilterList_Down":
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Down, sfiList);
                                break;

                            case "cmsFilterList_Bottom":
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Bottom, sfiList);
                                break;

                            case "cmsFilterList_Copy":
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Copy, sfiList);
                                break;

                            case "cmsFilterList_Export":
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Export, sfiList);
                                break;

                            case "cmsFilterList_Delete":
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(Operate.SystemConfig.ListAction.Delete, sfiList);
                                break;
                        }

                        this.dgvFilterList.ClearSelection();

                        foreach (Socket_FilterInfo sfi in sfiList)
                        {
                            int iIndex = Operate.FilterConfig.List.lstFilter.IndexOf(sfi);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Top, ssiList);
                                break;

                            case "cmsSendList_Up":
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Up, ssiList);
                                break;

                            case "cmsSendList_Down":
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Down, ssiList);
                                break;

                            case "cmsSendList_Bottom":
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Bottom, ssiList);
                                break;

                            case "cmsSendList_Copy":
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Copy, ssiList);
                                break;

                            case "cmsSendList_Export":
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Export, ssiList);
                                break;

                            case "cmsSendList_Delete":
                                Operate.SendConfig.SendList.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Delete, ssiList);
                                break;
                        }

                        this.dgvSendList.ClearSelection();

                        foreach (Socket_SendInfo ssi in ssiList)
                        {
                            int iIndex = Operate.SendConfig.SendList.lstSend.IndexOf(ssi);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Top, sriList);
                                break;

                            case "cmsRobotList_Up":
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Up, sriList);
                                break;

                            case "cmsRobotList_Down":
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Down, sriList);
                                break;

                            case "cmsRobotList_Bottom":
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Bottom, sriList);
                                break;

                            case "cmsRobotList_Copy":
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Copy, sriList);
                                break;

                            case "cmsRobotList_Export":
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Export, sriList);
                                break;

                            case "cmsRobotList_Delete":
                                Operate.RobotConfig.RobotList.UpdateRobotList_ByListAction(Operate.SystemConfig.ListAction.Delete, sriList);
                                break;
                        }

                        this.dgvRobotList.ClearSelection();

                        foreach (Socket_RobotInfo sri in sriList)
                        {
                            int iIndex = Operate.RobotConfig.RobotList.lstRobot.IndexOf(sri);

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            Operate.LogConfig.SaveLogListToExcel();
                        }

                        break;

                    case "cmsLogList_CleanUp":

                        this.CleanUp_LogList();

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//滤镜列表按钮

        private void tsFilterList_Load_Click(object sender, EventArgs e)
        {
            Operate.FilterConfig.List.LoadFilterList_Dialog();
        }

        private void tsFilterList_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate.FilterConfig.List.lstFilter.Count > 0)
                {
                    List<Socket_FilterInfo> sfiList = new List<Socket_FilterInfo>();
                    sfiList.AddRange(Operate.FilterConfig.List.lstFilter);

                    Operate.FilterConfig.List.SaveFilterList_Dialog(string.Empty, sfiList);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsFilterList_Add_Click(object sender, EventArgs e)
        {
            Operate.FilterConfig.Filter.AddFilter_New();

            this.dgvFilterList.ClearSelection();
            this.dgvFilterList.CurrentCell = this.dgvFilterList.Rows[this.dgvFilterList.Rows.Count - 1].Cells[0];
        }

        private void tsFilterList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvFilterList.Rows.Count > 0)
            {
                Operate.FilterConfig.List.CleanUpFilterList_Dialog();
            }
        }

        private void tsFilterList_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate.FilterConfig.List.lstFilter.Count > 0)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tsFilterList_SelectNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate.FilterConfig.List.lstFilter.Count > 0)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//发送列表按钮

        private void tsSendList_Load_Click(object sender, EventArgs e)
        {
            Operate.SendConfig.SendList.LoadSendList_Dialog();
        }

        private void tsSendList_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate.SendConfig.SendList.lstSend.Count > 0)
                {
                    List<Socket_SendInfo> ssiList = new List<Socket_SendInfo>();
                    ssiList.AddRange(Operate.SendConfig.SendList.lstSend);

                    Operate.SendConfig.SendList.SaveSendList_Dialog(string.Empty, ssiList);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.SendConfig.SendList.lstExecute.Clear();

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
            Operate.SendConfig.Send.AddSend_New();

            this.dgvSendList.ClearSelection();
            this.dgvSendList.CurrentCell = this.dgvSendList.Rows[this.dgvSendList.Rows.Count - 1].Cells[0];
        }

        private void tsSendList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvSendList.Rows.Count > 0)
            {
                Operate.SendConfig.SendList.CleanUpSendList_Dialog();
            }
        }

        #endregion

        #region//执行发送列表（异步）

        private void bgwSendList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (Socket_SendInfo ssi in Operate.SendConfig.SendList.lstSend)
                {
                    if (ssi.IsEnable)
                    {
                        Socket_Send ss = Operate.SendConfig.Send.DoSend(ssi.SID);
                        if (ss != null)
                        {
                            if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                            { 
                                Operate.SendConfig.SendList.lstExecute.Add(ss);
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

                while (Operate.SendConfig.SendList.lstExecute.Count > 0)
                {
                    foreach (Socket_Send ss in Operate.SendConfig.SendList.lstExecute.ToList())
                    {
                        if (this.bgwSendList.CancellationPending)
                        {
                            ss.StopSend();                            
                        }

                        if (!ss.Worker.IsBusy)
                        {
                            Operate.SendConfig.SendList.lstExecute.Remove(ss);
                        }
                    }

                    Thread.Sleep(100);
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//机器人列表按钮

        private void tsRobotList_Load_Click(object sender, EventArgs e)
        {
            Operate.RobotConfig.RobotList.LoadRobotList_Dialog();
        }

        private void tsRobotList_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate.RobotConfig.RobotList.lstRobot.Count > 0)
                {
                    List<Socket_RobotInfo> sriList = new List<Socket_RobotInfo>();
                    sriList.AddRange(Operate.RobotConfig.RobotList.lstRobot);

                    Operate.RobotConfig.RobotList.SaveRobotList_Dialog(string.Empty, sriList);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Operate.RobotConfig.RobotList.lstExecute.Clear();

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
            Operate.RobotConfig.Robot.AddRobot_New();

            this.dgvRobotList.ClearSelection();
            this.dgvRobotList.CurrentCell = this.dgvRobotList.Rows[this.dgvRobotList.Rows.Count - 1].Cells[0];
        }

        private void tsRobotList_CleanUp_Click(object sender, EventArgs e)
        {
            if (dgvRobotList.Rows.Count > 0)
            {
                Operate.RobotConfig.RobotList.CleanUpRobotList_Dialog();
            }
        }

        #endregion

        #region//执行机器人列表（异步）

        private void bgwRobotList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (Socket_RobotInfo sri in Operate.RobotConfig.RobotList.lstRobot)
                {
                    if (sri.IsEnable)
                    {
                        Socket_Robot sr = Operate.RobotConfig.Robot.DoRobot(sri.RID, null);
                        if (sr != null)
                        {
                            if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                            {
                                Operate.RobotConfig.RobotList.lstExecute.Add(sr);
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

                while (Operate.RobotConfig.RobotList.lstExecute.Count > 0)
                {
                    foreach (Socket_Robot sr in Operate.RobotConfig.RobotList.lstExecute.ToList())
                    {
                        if (this.bgwRobotList.CancellationPending)
                        {
                            sr.StopRobot();
                        }

                        if (!sr.Worker.IsBusy)
                        {
                            Operate.RobotConfig.RobotList.lstExecute.Remove(sr);
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//文本对比

        private void bTextCompare_Click(object sender, EventArgs e)
        {
            string TextA = this.rtbComparison_A.Text.Trim();
            string TextB = this.rtbComparison_B.Text.Trim();

            if (!Operate.SystemConfig.IsShow_TextCompare)
            {
                TextCompareForm tcForm = new TextCompareForm(TextA, TextB);
                tcForm.Show();
            }            
        }

        private void bTextDuplicate_Click(object sender, EventArgs e)
        {
            string TextA = this.rtbComparison_A.Text.Trim();
            string TextB = this.rtbComparison_B.Text.Trim();

            if (!Operate.SystemConfig.IsShow_TextDuplicate)
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

                string sBytes = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Bytes, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sEncodingText));
                string sANSI_GBK = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.GBK, sEncodingText));

                string sUTF7 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF7, sEncodingText));
                string sANSI_UTF7 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF7, sEncodingText));

                string sUTF8 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sEncodingText));
                string sANSI_UTF8 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sEncodingText));

                string sUTF16 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF16, sEncodingText));
                string sANSI_UTF16 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF16, sEncodingText));

                string sUTF32 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF32, sEncodingText));
                string sANSI_UTF32 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF32, sEncodingText));

                string sUnicode = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Unicode, sEncodingText));
                string sANSI_Unicode = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Unicode, sEncodingText));

                string sBase64 = Socket_Operation.Base64_Encoding(sEncodingText);
                string sANSI_Base64 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sBase64));

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bPacketInfo_Decoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sDecodingText = this.rtbPacketInfo_Encoding.Text;

                string sBytes = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Bytes, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                string sANSI_GBK = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.GBK, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                string sUTF7 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF7, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF7 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF7, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                string sUTF8 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF8 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                string sUTF16 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF16, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF16 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF16, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                string sUTF32 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF32, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                string sANSI_UTF32 = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF32, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                string sUnicode = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Unicode, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                string sANSI_Unicode = Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Unicode, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                string sBase64 = Socket_Operation.Base64_Decoding(sDecodingText);
                string sANSI_Base64 = Socket_Operation.Base64_Decoding(Socket_Operation.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Socket_Operation.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText)));

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                                            Operate.FilterConfig.Filter.FilterMode FMode = new Operate.FilterConfig.Filter.FilterMode();
                                            if (Socket_Operation.GetBoolFromChineseString(s22) == true)
                                            {
                                                FMode = Operate.FilterConfig.Filter.FilterMode.Normal;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s23) == true)
                                            {
                                                FMode = Operate.FilterConfig.Filter.FilterMode.Advanced;
                                            }
                                            string sFMode = ((int)FMode).ToString();

                                            Operate.FilterConfig.Filter.FilterAction FAction = new Operate.FilterConfig.Filter.FilterAction();
                                            if (Socket_Operation.GetBoolFromChineseString(s9) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.Replace;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s10) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.Intercept;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s11) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay;
                                            }
                                            else
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.NoModify_Display;
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

                                            Operate.FilterConfig.Filter.FilterFunction filterFunction = new Operate.FilterConfig.Filter.FilterFunction(bSend, bSendTo, bRecv, bRecvFrom, bWSASend, bWSASendTo, bWSARecv, bWSARecvFrom);
                                            string sFFunction = Operate.FilterConfig.Filter.GetFilterFunctionString(filterFunction);

                                            Operate.FilterConfig.Filter.FilterStartFrom FStartFrom = new Operate.FilterConfig.Filter.FilterStartFrom();
                                            if (Socket_Operation.GetBoolFromChineseString(s24) == true)
                                            {
                                                FStartFrom = Operate.FilterConfig.Filter.FilterStartFrom.Head;
                                            }
                                            else if (Socket_Operation.GetBoolFromChineseString(s25) == true)
                                            {
                                                FStartFrom = Operate.FilterConfig.Filter.FilterStartFrom.Position;
                                            }
                                            string sFStartFrom = ((int)FStartFrom).ToString();

                                            string sFProgressionStep = s12;
                                            string sFProgressionPosition = string.Empty;

                                            string sFSearch = string.Empty;
                                            string sFModify = string.Empty;
                                            if (FMode == Operate.FilterConfig.Filter.FilterMode.Normal)
                                            {
                                                sFProgressionPosition = Socket_Operation.ConvertFILTString(s31, false);
                                                sFSearch = Socket_Operation.ConvertFILTString(s26, false);
                                                sFModify = Socket_Operation.ConvertFILTString(s27, false);
                                            }
                                            else if (FMode == Operate.FilterConfig.Filter.FilterMode.Advanced)
                                            {
                                                sFProgressionPosition = Socket_Operation.ConvertFILTString(s32, false);
                                                sFSearch = Socket_Operation.ConvertFILTString(s28, false);

                                                if (FStartFrom == Operate.FilterConfig.Filter.FilterStartFrom.Position)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        #endregion        
    }
}

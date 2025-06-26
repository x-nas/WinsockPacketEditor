﻿using Be.Windows.Forms;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WPE.Lib;
using WPE.InjectMode;

namespace WinsockPacketEditor
{
    public partial class SocketProxy_Form : Form
    {
        private readonly Operate.SystemConfig.SystemMode RunMode = Operate.SystemConfig.SystemMode.Proxy;
        private readonly InjectModeForm socketForm;
        private static Socket SocketServer;

        #region//窗体事件

        public SocketProxy_Form(InjectModeForm socketForm)
        {
            try
            {
                Operate.SystemConfig.LoadSystemConfig_FromDB();

                InitializeComponent();

                this.socketForm = socketForm;
                this.InitSocketDGV();

                this.InitForm();
                this.LoadConfigs_Parameter();                                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void SocketProxy_Form_Load(object sender, EventArgs e)
        {
            this.InitProxyIPAppoint();            
            this.ProxyIP_Appoint_Changed();
            this.EnableAuth_Changed();
            this.LogList_AutoClear_Changed();
            this.Enable_MapLocal_Changed();
            this.Enable_MapRemote_Changed();
            this.Enable_ExternalProxyChanged();
            this.ExternalProxy_AppointPort_Changed();
            this.ExternalProxy_EnableAuth_Changed();            
        }

        private void SocketProxy_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ExitMainForm();
        }

        private void ExitMainForm()
        {
            try
            {
                if (this.socketForm != null)
                {
                    //this.socketForm.ExitMainForm();
                }

                if (this.cbEnable_SystemProxy.Checked)
                {
                    Socket_Operation.StopSystemProxy();
                }                

                this.SaveConfigs_Parameter();

                Socket_Operation.StopRemoteMGT(this.RunMode);
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

        #endregion

        #region//初始化数据表

        private void InitSocketDGV()
        {
            try
            {
                tvProxyData.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyData, true, null);
          
                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Operate.LogConfig.lstLogInfo;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);                

                tvProxyData.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyData, true, null);
                Operate.ProxyConfig.SocketProxyList.RecProxyData += new Operate.ProxyConfig.SocketProxyList.ProxyDataReceived(Event_RecProxyData);

                tvProxyInfo.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyInfo, true, null);
                Operate.ProxyConfig.SocketProxyList.RecProxyTCP += new Operate.ProxyConfig.SocketProxyList.ProxyTCPReceived(Event_RecProxyInfo);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//初始化界面

        private void InitForm()
        {
            try
            {
                this.Text = " - " + Operate.SystemConfig.AssemblyVersion;

                this.tSocketProxy.Enabled = true;
                this.tShowProxyState.Enabled = true;
                this.cbbAuthType.SelectedIndex = 0;                

                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Operate.ProxyConfig.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Operate.ProxyConfig.SocketProxy.Total_Response));
                this.tsslProxySpeed.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_172), "0.00", "0.00");
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitProxyIPAppoint()
        {
            try
            {
                IPAddress[] ipAddresses = Operate.SystemConfig.GetLocalIPAddress();

                this.cbbProxyIP_Appoint.Items.Clear();

                foreach (IPAddress ipAddress in ipAddresses)
                {
                    this.cbbProxyIP_Appoint.Items.Add(ipAddress.ToString());
                }

                if (this.cbbProxyIP_Appoint.Items.Count > 0)
                {
                    this.cbbProxyIP_Appoint.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//指定代理IP地址

        private void cbProxyIP_Auto_CheckedChanged(object sender, EventArgs e)
        {
            this.ProxyIP_Appoint_Changed();
        }

        private void ProxyIP_Appoint_Changed()
        {
            this.cbbProxyIP_Appoint.Enabled = !this.cbProxyIP_Auto.Checked;
        }

        #endregion

        #region//启用身份认证

        private void cbEnable_Auth_CheckedChanged(object sender, EventArgs e)
        {
            this.EnableAuth_Changed();
        }

        private void EnableAuth_Changed()
        {
            this.cbbAuthType.Enabled = 
                this.bAccount.Enabled = 
                this.bAuthInfo.Enabled =
                this.cbEnable_Auth.Checked;
        }

        private void bAccount_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyAccountListForm();
        }

        private void bAuthInfo_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyAuthForm();
        }

        #endregion

        #region//加载本页面的运行配置

        private void LoadConfigs_Parameter()
        {
            try
            {
                Operate.SystemConfig.LoadRunConfig_FromDB();

                this.cbProxyIP_Auto.Checked = Operate.ProxyConfig.SocketProxy.ProxyIP_Auto;            
                this.cbEnable_SOCKS5.Checked = Operate.ProxyConfig.SocketProxy.Enable_SOCKS5;
                this.nudProxyPort.Value = Operate.ProxyConfig.SocketProxy.ProxyPort;
                this.cbEnable_Auth.Checked = Operate.ProxyConfig.SocketProxy.Enable_Auth;                

                this.cbNoRecordData.Checked = Operate.ProxyConfig.SocketProxy.NoRecord;
                this.cbDeleteClosed.Checked = Operate.ProxyConfig.SocketProxy.DelClosed;

                this.cbLogList_AutoRoll.Checked = Operate.LogConfig.AutoRoll;
                this.cbLogList_AutoClear.Checked = Operate.LogConfig.AutoClear;
                this.nudLogList_AutoClearValue.Value = Operate.LogConfig.AutoClear_Value;

                this.cbEnable_MapLocal.Checked = Operate.ProxyConfig.ProxyMapping.Enable_MapLocal;
                this.cbEnable_MapRemote.Checked = Operate.ProxyConfig.ProxyMapping.Enable_MapRemote;

                this.cbEnable_ExternalProxy.Checked = Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy;
                this.txtExternalProxy_IP.Text = Operate.ProxyConfig.SocketProxy.ExternalProxy_IP;
                this.txtExternalProxy_Port.Text = Operate.ProxyConfig.SocketProxy.ExternalProxy_Port.ToString();
                this.cbExternalProxy_AppointPort.Checked = Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_AppointPort;
                this.txtExternalProxy_AppointPort.Text = Operate.ProxyConfig.SocketProxy.ExternalProxy_AppointPort;
                this.cbExternalProxy_EnableAuth.Checked = Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_Auth;
                this.txtExternalProxy_UserName.Text = Operate.ProxyConfig.SocketProxy.ExternalProxy_UserName;
                this.txtExternalProxy_PassWord.Text = Operate.ProxyConfig.SocketProxy.ExternalProxy_PassWord;

                this.cbSpeedMode.Checked = Operate.ProxyConfig.SocketProxy.SpeedMode;

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存本页面的运行配置

        private void SaveConfigs_Parameter()
        {
            try
            {
                Operate.ProxyConfig.SocketProxy.ProxyIP_Auto = this.cbProxyIP_Auto.Checked;             
                Operate.ProxyConfig.SocketProxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Operate.ProxyConfig.SocketProxy.ProxyPort = ((ushort)this.nudProxyPort.Value);
                Operate.ProxyConfig.SocketProxy.Enable_Auth = this.cbEnable_Auth.Checked;                

                Operate.ProxyConfig.SocketProxy.NoRecord = this.cbNoRecordData.Checked;
                Operate.ProxyConfig.SocketProxy.DelClosed = this.cbDeleteClosed.Checked;

                Operate.LogConfig.AutoRoll = this.cbLogList_AutoRoll.Checked;
                Operate.LogConfig.AutoClear = this.cbLogList_AutoClear.Checked;
                Operate.LogConfig.AutoClear_Value = this.nudLogList_AutoClearValue.Value;

                Operate.ProxyConfig.ProxyMapping.Enable_MapLocal = this.cbEnable_MapLocal.Checked;
                Operate.ProxyConfig.ProxyMapping.Enable_MapRemote = this.cbEnable_MapRemote.Checked;

                Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy = this.cbEnable_ExternalProxy.Checked;
                Operate.ProxyConfig.SocketProxy.ExternalProxy_IP = this.txtExternalProxy_IP.Text.Trim();
                Operate.ProxyConfig.SocketProxy.ExternalProxy_Port = ushort.Parse(this.txtExternalProxy_Port.Text.Trim());
                Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_AppointPort = this.cbExternalProxy_AppointPort.Checked;
                Operate.ProxyConfig.SocketProxy.ExternalProxy_AppointPort = this.txtExternalProxy_AppointPort.Text.Trim();
                Operate.ProxyConfig.SocketProxy.Enable_ExternalProxy_Auth = this.cbExternalProxy_EnableAuth.Checked;
                Operate.ProxyConfig.SocketProxy.ExternalProxy_UserName = this.txtExternalProxy_UserName.Text.Trim();
                Operate.ProxyConfig.SocketProxy.ExternalProxy_PassWord = this.txtExternalProxy_PassWord.Text.Trim();

                Operate.ProxyConfig.SocketProxy.SpeedMode = this.cbSpeedMode.Checked;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//系统代理

        private void cbEnable_SystemProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.SystemProxy_CheckedChanged();
        }

        private void SystemProxy_CheckedChanged()
        {
            try
            {
                if (this.cbEnable_SystemProxy.Checked)
                {
                    Socket_Operation.StartSystemProxy();
                }
                else
                {
                    Socket_Operation.StopSystemProxy();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//列表设置

        private void cbLogList_AutoClear_CheckedChanged(object sender, EventArgs e)
        {
            this.LogList_AutoClear_Changed();
        }

        private void LogList_AutoClear_Changed()
        {
            this.nudLogList_AutoClearValue.Enabled = this.cbLogList_AutoClear.Checked;           
        }

        #endregion

        #region//代理映射

        private void cbEnable_MapLocal_CheckedChanged(object sender, EventArgs e)
        {
            this.Enable_MapLocal_Changed();
        }

        private void cbEnable_MapRemote_CheckedChanged(object sender, EventArgs e)
        {
            this.Enable_MapRemote_Changed();
        }

        private void Enable_MapLocal_Changed()
        {
            this.bProxyMapping_Local.Enabled = this.cbEnable_MapLocal.Checked;
            Operate.ProxyConfig.ProxyMapping.Enable_MapLocal = this.cbEnable_MapLocal.Checked;
        }

        private void Enable_MapRemote_Changed()
        {
            this.bProxyMapping_Remote.Enabled = this.cbEnable_MapRemote.Checked;
            Operate.ProxyConfig.ProxyMapping.Enable_MapRemote = this.cbEnable_MapRemote.Checked;
        }

        private void bProxyMapping_Local_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyMapLocalListForm();
        }

        private void bProxyMapping_Remote_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyMapRemoteListForm();
        }

        #endregion

        #region//外部代理

        private void cbEnable_ExternalProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.Enable_ExternalProxyChanged();
        }

        private void Enable_ExternalProxyChanged()
        { 
            this.txtExternalProxy_IP.Enabled =
                this.txtExternalProxy_Port.Enabled =
                this.bExternalProxy_Detection.Enabled =
                this.tlpExternalProxy_Appoint.Enabled =
                this.cbEnable_ExternalProxy.Checked;

            this.ExternalProxy_AppointPort_Changed();
        }

        private void cbExternalProxy_AppointPort_CheckedChanged(object sender, EventArgs e)
        {
            this.ExternalProxy_AppointPort_Changed();
        }

        private void ExternalProxy_AppointPort_Changed()
        {
            this.txtExternalProxy_AppointPort.Enabled = this.cbExternalProxy_AppointPort.Checked;
        }

        private void cbExternalProxy_EnableAuth_CheckedChanged(object sender, EventArgs e)
        {
            this.ExternalProxy_EnableAuth_Changed();
        }

        private void ExternalProxy_EnableAuth_Changed()
        { 
            this.txtExternalProxy_UserName.Enabled = this.txtExternalProxy_PassWord.Enabled = this.cbExternalProxy_EnableAuth.Checked;
        }

        private async void bExternalProxy_Detection_Click(object sender, EventArgs e)
        {
            if (this.CheckProxySet())
            {
                this.SaveConfigs_Parameter();

                this.bExternalProxy_Detection.Enabled = false;
                bool Result = await Socket_Operation.DetectionExternalProxy();

                if (Result)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_211));
                }

                this.bExternalProxy_Detection.Enabled = true;
            }
        }

        #endregion

        #region//系统设置

        private void cbSpeedMode_CheckedChanged(object sender, EventArgs e)
        {
            Operate.ProxyConfig.SocketProxy.SpeedMode = this.cbSpeedMode.Checked;
        }

        #endregion

        #region//开始代理

        private void bStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckProxySet())
                {
                    Operate.ProxyConfig.SocketProxy.IsListening = true;

                    this.InitProxyStart();
                    this.UpdateUIState(starting: true);

                    if (SocketServer == null)
                    {
                        InitializeServerSocket();
                    }

                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_142));
                }                                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void UpdateUIState(bool starting)
        {
            this.bStart.Enabled = !starting;
            this.bStop.Enabled = starting;

            this.cbEnable_Auth.Enabled = !starting;            

            this.tpProxySet.Enabled = !starting;
            this.tpExternalProxy.Enabled = !starting;            
        }

        private void InitializeServerSocket()
        {
            try
            {
                SocketServer?.Close();
                SocketServer?.Dispose();

                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Operate.ProxyConfig.SocketProxy.ProxyPort);
                SocketServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    LingerState = new LingerOption(false, 0),
                    ExclusiveAddressUse = false
                };

                SocketServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                SocketServer.Bind(ep);
                SocketServer.Listen(backlog: 1000);

                AcceptClients();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }        

        private void InitProxyStart()
        {
            try
            {
                if (this.cbProxyIP_Auto.Checked)
                {
                    Operate.ProxyConfig.SocketProxy.ProxyTCP_IP = IPAddress.Any;
                    Operate.ProxyConfig.SocketProxy.ProxyUDP_IP = IPAddress.Parse(this.cbbProxyIP_Appoint.Items[0].ToString());
                }
                else
                {
                    Operate.ProxyConfig.SocketProxy.ProxyTCP_IP = IPAddress.Parse(this.cbbProxyIP_Appoint.SelectedItem.ToString());
                    Operate.ProxyConfig.SocketProxy.ProxyUDP_IP = IPAddress.Parse(this.cbbProxyIP_Appoint.SelectedItem.ToString());
                }

                Operate.ProxyConfig.SocketProxy.ProxyTotal_CNT = 0;
                Operate.ProxyConfig.SocketProxy.ProxyTCP_CNT = 0;
                Operate.ProxyConfig.SocketProxy.ProxyUDP_CNT = 0;

                this.SaveConfigs_Parameter();

                string sProxyIP = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_137), Operate.ProxyConfig.SocketProxy.ProxyTCP_IP, Operate.ProxyConfig.SocketProxy.ProxyUDP_IP);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sProxyIP);

                if (this.cbEnable_Auth.Checked)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_169));
                }

                if (this.cbEnable_ExternalProxy.Checked)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_170));
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//接受客户端链接

        private void AcceptClients()
        {
            try
            {
                if (Operate.ProxyConfig.SocketProxy.IsListening && SocketServer != null)
                {
                    var acceptArgs = new SocketAsyncEventArgs();
                    acceptArgs.Completed += AcceptCompleted;

                    if (!SocketServer.AcceptAsync(acceptArgs))
                    {
                        AcceptCompleted(null, acceptArgs);
                    }                    
                }
            }
            catch (ObjectDisposedException)
            {
                // Socket已关闭，正常退出
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                Task.Delay(5000).ContinueWith(_ => AcceptClients());
            }
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && Operate.ProxyConfig.SocketProxy.IsListening && e.AcceptSocket != null)
                {
                    Operate.ProxyConfig.SocketProxy.HandleClient(e.AcceptSocket);

                    e.AcceptSocket = null;

                    if (Operate.ProxyConfig.SocketProxy.IsListening)
                    {
                        if (!SocketServer.AcceptAsync(e))
                        {
                            AcceptCompleted(null, e);
                        }
                    }
                    else
                    {
                        e.Dispose();
                    }
                }
                else
                {
                    e.Dispose();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                e.Dispose();

                if (Operate.ProxyConfig.SocketProxy.IsListening)
                {
                    Task.Delay(1000).ContinueWith(_ => AcceptClients());
                }
            }
        }

        //private void AcceptCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        if (Operate.ProxyConfig.SocketProxy.IsListening)
        //        {
        //            Socket clientSocket = SocketServer.EndAccept(ar);

        //            if (clientSocket != null)
        //            {
        //                Operate.ProxyConfig.SocketProxy.HandleClient(clientSocket);
        //            }

        //            AcceptClients();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
        //    }
        //}

        #endregion

        #region//设置有效性检测

        private bool CheckProxySet()
        {
            bool bReturn = true;

            try
            {
                //启用SOCKS5代理
                if (!this.cbEnable_SOCKS5.Checked)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_141));
                    return false;
                }

                //启用外部代理
                if (this.cbEnable_ExternalProxy.Checked)
                {
                    string ExternalProxyIP = this.txtExternalProxy_IP.Text.Trim();
                    string ExternalProxyPort = this.txtExternalProxy_Port.Text.Trim();                    

                    if (string.IsNullOrEmpty(ExternalProxyIP) || string.IsNullOrEmpty(ExternalProxyPort))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_202));
                        return false;
                    }

                    Operate.ProxyConfig.SocketProxy.AddressType atExternalProxy = Socket_Operation.GetAddressType_ByString(ExternalProxyIP);
                    if (atExternalProxy != Operate.ProxyConfig.SocketProxy.AddressType.IPv4 && atExternalProxy != Operate.ProxyConfig.SocketProxy.AddressType.Domain)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_203));
                        return false;
                    }

                    if (ushort.TryParse(ExternalProxyPort, out ushort uPort))
                    {
                        if (uPort < 0)
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_202));
                            return false;
                        }
                    }
                    else
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_202));
                        return false;
                    }                    

                    //指定端口
                    if (this.cbExternalProxy_AppointPort.Checked)
                    {
                        string ExternalProxy_AppointPort = this.txtExternalProxy_AppointPort.Text.Trim();

                        if (string.IsNullOrEmpty(ExternalProxy_AppointPort))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_202));
                            return false;
                        }
                    }

                    //启用外部代理认证
                    if (this.cbExternalProxy_EnableAuth.Checked)
                    {
                        string UserName = this.txtExternalProxy_UserName.Text.Trim();
                        string PassWord = this.txtExternalProxy_PassWord.Text.Trim();

                        if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord)) 
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_204));
                            return false;                        
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//停止代理

        private void bStop_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.ProxyConfig.SocketProxy.IsListening = false;

                if (SocketServer != null)
                {
                    try
                    {
                        SocketServer.Close();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                    finally
                    {
                        SocketServer = null;
                    }
                }

                this.UpdateUIState(starting: false);

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_143));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//计时器

        private void tSocketProxy_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Operate.ProxyConfig.SocketProxyQueue.qSocket_ProxyTCP.Count > 0)
                {
                    Operate.ProxyConfig.SocketProxyList.ProxyTCP_ToList();
                }

                if (Operate.ProxyConfig.SocketProxyQueue.qSocket_ProxyUDP.Count > 0)
                {
                    Operate.ProxyConfig.SocketProxyList.ProxyUDP_ToList();
                }

                if (Operate.ProxyConfig.SocketProxyQueue.qSocket_ProxyData.Count > 0)
                {
                    Operate.ProxyConfig.SocketProxyList.ProxyData_ToList();
                }

                if (Operate.LogConfig.cqLogInfo.Count > 0)
                {
                    Operate.LogConfig.LogToList();

                    if (this.cbLogList_AutoRoll.Checked && !this.dgvLogList.IsDisposed)
                    {
                        if (dgvLogList.Rows.Count > 0 && dgvLogList.Height > dgvLogList.RowTemplate.Height)
                        {
                            dgvLogList.FirstDisplayedScrollingRowIndex = dgvLogList.RowCount - 1;
                        }
                    }

                    this.AutoCleanUp_LogList();
                }                                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tShowProxyState_Tick(object sender, EventArgs e)
        {
            this.ShowProxyInfo();
            this.ShowProxyChart();            
        }

        private async void tUpdateProxyState_Tick(object sender, EventArgs e)
        {
            await this.UpdateClientLinks();
            await this.UpdateAccountLinksAndDevices();            
            await Operate.ProxyConfig.SocketProxy.UpdateProxyUDP();
            await Operate.ProxyConfig.ProxyAccount.UpdateOnlineStatus();
        }

        #endregion

        #region//清空数据

        private void bCleanUp_Click(object sender, EventArgs e)
        {
            try
            {
                this.CleanUp_ProxyData();
                this.CleanUp_ProxyInfo();
                this.CleanUp_LogList();
                this.CleanUp_HexBox();
                this.CleanUp_ShowProxyInfo();
                Operate.ProxyConfig.ProxyAccount.ClearProxyAuthList();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ShowProxyInfo()
        {
            Operate.ProxyConfig.SocketProxy.ProxyTotal_CNT = 0;
            Operate.ProxyConfig.SocketProxy.ProxyTCP_CNT = 0;
            Operate.ProxyConfig.SocketProxy.ProxyUDP_CNT = 0;
            Operate.ProxyConfig.SocketProxy.Total_Request = 0;
            Operate.ProxyConfig.SocketProxy.Total_Response = 0;
        }

        private void CleanUp_ProxyData()
        {
            Operate.ProxyConfig.SocketProxyQueue.ResetProxy_DataQueue();
            Operate.ProxyConfig.SocketProxyList.ResetProxy_DataList();
            this.tvProxyData.Nodes.Clear();
        }

        private void CleanUp_ProxyInfo()
        {
            Operate.ProxyConfig.SocketProxyQueue.ResetProxy_TCPQueue();
            Operate.ProxyConfig.SocketProxyList.ResetProxy_TCPList();
            this.tvProxyInfo.Nodes.Clear();
        }

        private void CleanUp_LogList()
        {
            Operate.LogConfig.ClearLogQueue();
            Operate.LogConfig.ClearLogList();
            this.dgvLogList.Rows.Clear();
        }

        private void CleanUp_HexBox()
        {
            try
            {
                if (hbData.ByteProvider != null)
                {
                    IDisposable byteProvider = hbData.ByteProvider as IDisposable;

                    if (byteProvider != null)
                    {
                        byteProvider.Dispose();
                    }

                    hbData.ByteProvider = null;
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理列表（异步）

        private async void Event_RecProxyData(Socket_ProxyData spd)
        {
            try
            {
                if (!tvProxyData.IsDisposed)
                {
                    int RootImgIndex = -1;
                    int RequestImgIndex = -1;
                    int ResponseImgIndex = -1;
                    int DataImgIndex = -1;
                    TreeNode RootNode = null;

                    switch (spd.DomainType)
                    {
                        case Operate.ProxyConfig.SocketProxy.DomainType.Socket:
                            RootImgIndex = 0;
                            RequestImgIndex = 2;
                            ResponseImgIndex = 3;
                            DataImgIndex = 1;
                            break;

                        case Operate.ProxyConfig.SocketProxy.DomainType.Http:
                            RootImgIndex = 7;
                            RequestImgIndex = 2;
                            ResponseImgIndex = 3;
                            DataImgIndex = 8;
                            break;

                        case Operate.ProxyConfig.SocketProxy.DomainType.Https:
                            RootImgIndex = 7;
                            RequestImgIndex = 2;
                            ResponseImgIndex = 3;
                            DataImgIndex = 8;
                            break;
                    }

                    await Task.Run(() =>
                    {
                        RootNode = Socket_Operation.FindNodeSync(this.tvProxyData.Nodes, spd.Domain);
                        if (RootNode == null)
                        {
                            RootNode = Socket_Operation.AddTreeNode(this.tvProxyData, this.tvProxyData.Nodes, spd.Domain, RootImgIndex, null);
                            Socket_Operation.AddTreeNode(this.tvProxyData, RootNode.Nodes, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_138), RequestImgIndex, null);
                            Socket_Operation.AddTreeNode(this.tvProxyData, RootNode.Nodes, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_139), ResponseImgIndex, null);
                        }

                        if (!this.cbNoRecordData.Checked)
                        {
                            TreeNode DataNode = new TreeNode();
                            switch (spd.DataType)
                            {
                                case Operate.ProxyConfig.SocketProxy.DataType.Request:
                                    DataNode = RootNode.Nodes[0];
                                    break;

                                case Operate.ProxyConfig.SocketProxy.DataType.Response:
                                    DataNode = RootNode.Nodes[1];
                                    break;
                            }

                            string sDataNodeName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_140), spd.Buffer.Length);
                            Socket_Operation.AddTreeNode(this.tvProxyData, DataNode.Nodes, sDataNodeName, DataImgIndex, spd.Buffer);
                        }
                    });

                    if (tvProxyData.InvokeRequired)
                    {
                        tvProxyData.Invoke(new Action(() =>
                        {
                            tvProxyData.Refresh();
                        }));
                    }
                    else
                    {
                        tvProxyData.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Event_RecProxyData), ex.Message);
            }
        }        

        #endregion

        #region//显示客户端列表（异步）

        private async void Event_RecProxyInfo(Socket_ProxyTCP spt)
        {
            try
            {
                if (!tvProxyInfo.IsDisposed && spt != null)
                {
                    if (spt.CommandType == Operate.ProxyConfig.SocketProxy.CommandType.Connect)
                    {
                        int iRootImgIndex = -1;
                        int iChildImgIndex = 5;

                        string ClientIP = Operate.ProxyConfig.SocketProxy.GetClientIPAddress(spt);
                        string ClientUserName = Operate.ProxyConfig.ProxyAccount.GetUserName_ByAccountID(spt.AID);
                        string sRootName = Socket_Operation.GetClientListName(ClientIP, ClientUserName);

                        if (!string.IsNullOrEmpty(sRootName))
                        {
                            string sChildName = spt.Client.Address;

                            await Task.Run(() =>
                            {
                                TreeNode RootNode = Socket_Operation.FindNodeSync(this.tvProxyInfo.Nodes, sRootName);
                                if (RootNode == null)
                                {
                                    RootNode = Socket_Operation.AddTreeNode(this.tvProxyInfo, this.tvProxyInfo.Nodes, sRootName, iRootImgIndex, null);
                                }

                                if (RootNode != null)
                                {
                                    TreeNode ChildNode = Socket_Operation.FindNodeSync(RootNode.Nodes, sChildName);
                                    if (ChildNode == null)
                                    {
                                        ChildNode = Socket_Operation.AddTreeNode(this.tvProxyInfo, RootNode.Nodes, sChildName, iChildImgIndex, null);
                                    }
                                }                                
                            });

                            if (tvProxyInfo.InvokeRequired)
                            {
                                tvProxyInfo.Invoke(new Action(() =>
                                {
                                    tvProxyInfo.Refresh();
                                }));
                            }
                            else
                            {
                                tvProxyInfo.Refresh();
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Event_RecProxyInfo), ex.Message);
            }
        }

        #endregion

        #region//更新客户端链接（异步）

        private async Task UpdateClientLinks()
        {
            await Task.Run(() =>
            {
                try
                {
                    ConcurrentBag<Socket_ProxyTCP> sptRemove = new ConcurrentBag<Socket_ProxyTCP>();

                    var snapshot = Operate.ProxyConfig.SocketProxyList.lstProxyTCP.ToArray();
                    foreach (Socket_ProxyTCP spt in snapshot)
                    {
                        if (spt.Client.Socket == null)
                        {
                            string ClientIP = Operate.ProxyConfig.SocketProxy.GetClientIPAddress(spt);
                            string ClientUserName = Operate.ProxyConfig.ProxyAccount.GetUserName_ByAccountID(spt.AID);

                            if (string.IsNullOrEmpty(ClientUserName))
                            {
                                TreeNode ClientNode = Socket_Operation.FindNodeSync(this.tvProxyInfo.Nodes, spt.Client.Address);
                                if (ClientNode != null)
                                {
                                    if (!tvProxyInfo.IsDisposed)
                                    {
                                        tvProxyInfo.Invoke(new MethodInvoker(delegate
                                        {
                                            ClientNode.Remove();
                                        }));
                                    }
                                }

                                sptRemove.Add(spt);
                            }
                            else
                            {
                                string sRootName = Socket_Operation.GetClientListName(ClientIP, ClientUserName);

                                TreeNode RootNode = Socket_Operation.FindNodeSync(this.tvProxyInfo.Nodes, sRootName);
                                if (RootNode != null)
                                {
                                    TreeNode ClientNode = Socket_Operation.FindNodeSync(RootNode.Nodes, spt.Client.Address);

                                    if (!tvProxyInfo.IsDisposed)
                                    {
                                        tvProxyInfo.Invoke(new MethodInvoker(delegate
                                        {
                                            if (ClientNode != null)
                                            {
                                                ClientNode.Remove();
                                                sptRemove.Add(spt);
                                            }

                                            if (RootNode.Nodes.Count == 0)
                                            {
                                                Operate.ProxyConfig.ProxyAccount.DeleteProxyAuthInfo_ByAIDAndIP(spt.AID, ClientIP);

                                                if (this.cbDeleteClosed.Checked)
                                                {
                                                    RootNode.Remove();
                                                }

                                                if (spt.AID != null && spt.AID != Guid.Empty)
                                                {
                                                    Operate.ProxyConfig.ProxyAccount.SetOnline_ByAccountID(spt.AID, false);
                                                }
                                            }
                                        }));
                                    }
                                }
                            }
                        }                        
                    }

                    foreach (Socket_ProxyTCP spt in sptRemove)
                    {
                        Operate.ProxyConfig.SocketProxyList.ClearTCP(spt);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(UpdateClientLinks), ex.Message);
                }
            });
        }

        #endregion

        #region//更新账号链接数和设备数（异步）

        private async Task UpdateAccountLinksAndDevices()
        {
            await Task.Run(() =>
            {
                try
                {
                    foreach (Proxy_AuthInfo pai in Operate.ProxyConfig.ProxyAccount.lstProxyAuth.ToList())
                    {
                        string ClientIP = pai.IPAddress.ToString();
                        pai.LinksNumber = Operate.ProxyConfig.ProxyAccount.GetLinksNumber_ByAccountID(pai.AID, ClientIP, this.tvProxyInfo.Nodes);
                        pai.DevicesNumber = Operate.ProxyConfig.ProxyAccount.GetDevicesNumber_ByAccountID(pai.AID);
                    }
                }
                catch (Exception ex)
                {
                    Operate.DoLog(nameof(UpdateAccountLinksAndDevices), ex.Message);
                }
            });
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

        #region//显示选中的代理数据

        private void tvSocketProxy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                byte[] bBuffer = e.Node.Tag as byte[];

                if (bBuffer != null)
                {
                    this.hbData.ByteProvider = new DynamicByteProvider(bBuffer);
                }
                else
                {
                    this.hbData.ByteProvider = null;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理信息

        private void ShowProxyInfo()
        {
            try
            {
                ulong ProxyTCP_CNT = Operate.ProxyConfig.SocketProxy.ProxyTCP_CNT;
                ulong ProxyUDP_CNT = Operate.ProxyConfig.SocketProxy.ProxyUDP_CNT;
                ulong ProxyTotal_CNT = ProxyTCP_CNT + ProxyUDP_CNT;
                int ProxyAccountOnLine = Socket_Operation.GetOnLineProxyAccountCount(Operate.ProxyConfig.ProxyAccount.lstProxyAccount);
                Operate.ProxyConfig.SocketProxy.ProxyOnLineInfo = ProxyAccountOnLine.ToString() + "/" + Operate.ProxyConfig.ProxyAccount.lstProxyAccount.Count.ToString();
                Operate.ProxyConfig.SocketProxy.ProxyBytesInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Operate.ProxyConfig.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Operate.ProxyConfig.SocketProxy.Total_Response));
                
                this.tlProxyTotal_CNT.Text = ProxyTotal_CNT.ToString();
                this.tlProxyTCP_CNT.Text = ProxyTCP_CNT.ToString();
                this.tlProxyUDP_CNT.Text = ProxyUDP_CNT.ToString();
                this.tlProxyCache_CNT.Text = Operate.ProxyConfig.SocketProxyQueue.qSocket_ProxyData.Count.ToString();
                this.tlProxyLinks_CNT.Text = Operate.ProxyConfig.SocketProxyList.lstProxyTCP.Count.ToString();
                this.tlProxyAccount_CNT.Text = Operate.ProxyConfig.SocketProxy.ProxyOnLineInfo;
                this.tsslTotalBytes.Text = Operate.ProxyConfig.SocketProxy.ProxyBytesInfo;
                this.tsslProxySpeed.Text = Operate.ProxyConfig.SocketProxy.ProxySpeedInfo;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理性能图表

        private void ShowProxyChart()
        {
            try
            {
                double dProxySpeed_Uplink = Operate.ProxyConfig.SocketProxy.ProxySpeed_Uplink;
                Operate.ProxyConfig.SocketProxy.ProxySpeed_Uplink = 0;
                double dProxySpeed_Downlink = Operate.ProxyConfig.SocketProxy.ProxySpeed_Downlink;                
                Operate.ProxyConfig.SocketProxy.ProxySpeed_Downlink = 0;                
           
                this.Invoke((MethodInvoker)delegate
                {
                    Series sProxy_Uplink = cProxyChart.Series[0];
                    if (sProxy_Uplink.Points.Count >= Operate.ProxyConfig.SocketProxy.MaxChartPoint)
                    {
                        sProxy_Uplink.Points.RemoveAt(0);
                    }

                    double dChartProxySpeed_Uplink = dProxySpeed_Uplink / Operate.ProxyConfig.SocketProxy.MaxNetworkSpeed;
                    if (dChartProxySpeed_Uplink > 10)
                    {
                        dChartProxySpeed_Uplink = 10;
                    }
                    sProxy_Uplink.Points.AddY(dChartProxySpeed_Uplink);

                    Series sProxy_Downlink = cProxyChart.Series[1];
                    if (sProxy_Downlink.Points.Count >= Operate.ProxyConfig.SocketProxy.MaxChartPoint)
                    {
                        sProxy_Downlink.Points.RemoveAt(0);
                    }

                    double dChartProxySpeed_Downlink = dProxySpeed_Downlink / Operate.ProxyConfig.SocketProxy.MaxNetworkSpeed;
                    if (dChartProxySpeed_Downlink > 10)
                    {
                        dChartProxySpeed_Downlink = 10;
                    }
                    sProxy_Downlink.Points.AddY(dChartProxySpeed_Downlink);

                    Operate.ProxyConfig.SocketProxy.ProxySpeedInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_172), (dProxySpeed_Uplink / 1024).ToString("0.00"), (dProxySpeed_Downlink / 1024).ToString("0.00"));
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包编辑器右键菜单

        private void cmsHexBox_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            this.cmsHexBox.Close();

            try
            {
                switch (sItemText)
                {                    
                    case "cmsHexBox_CopyHex":

                        this.hbData.CopyHex();

                        break;

                    case "cmsHexBox_Copy":

                        this.hbData.Copy();

                        break;              

                    case "cmsHexBox_SelectAll":

                        this.hbData.SelectAll();

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

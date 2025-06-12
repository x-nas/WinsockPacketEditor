using Be.Windows.Forms;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WPELibrary;
using WPELibrary.Lib;

namespace WinsockPacketEditor
{
    public partial class SocketProxy_Form : Form
    {
        private readonly Socket_Cache.System.SystemMode RunMode = Socket_Cache.System.SystemMode.Proxy;
        private readonly Socket_Form socketForm;
        private static Socket SocketServer;

        #region//窗体事件

        public SocketProxy_Form(Socket_Form socketForm)
        {
            try
            {
                Socket_Cache.System.LoadSystemConfig_FromDB();
                MultiLanguage.SetDefaultLanguage(Socket_Cache.System.DefaultLanguage);

                InitializeComponent();

                this.socketForm = socketForm;
                this.InitSocketDGV();

                this.InitForm();
                this.LoadConfigs_Parameter();                                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    this.socketForm.ExitMainForm();
                }

                if (this.cbEnable_SystemProxy.Checked)
                {
                    Socket_Operation.StopSystemProxy();
                }                

                this.SaveConfigs_Parameter();

                Socket_Operation.StopRemoteMGT(this.RunMode);
                Socket_Cache.System.SaveRunConfig_ToDB(this.RunMode);
                Socket_Cache.ProxyAccount.SaveProxyAccountList_ToDB(this.RunMode);
                Socket_Cache.ProxyMapping.SaveProxyMapLocal_ToDB(this.RunMode);
                Socket_Cache.ProxyMapping.SaveProxyMapRemote_ToDB(this.RunMode);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化数据表

        private void InitSocketDGV()
        {
            try
            {
                tvProxyData.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyData, true, null);

                dgvAuth.AutoGenerateColumns = false;
                dgvAuth.DataSource = Socket_Cache.SocketProxy.lstProxyAuth;
                dgvAuth.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvAuth, true, null);
                Socket_Cache.SocketProxy.RecProxyAuth += new Socket_Cache.SocketProxy.ProxyAuthReceived(Event_RecProxyAuth);

                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstProxyLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);                

                tvProxyData.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyData, true, null);
                Socket_Cache.SocketProxyList.RecProxyData += new Socket_Cache.SocketProxyList.ProxyDataReceived(Event_RecProxyData);

                tvProxyInfo.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyInfo, true, null);
                Socket_Cache.SocketProxyList.RecProxyTCP += new Socket_Cache.SocketProxyList.ProxyTCPReceived(Event_RecProxyInfo);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//初始化界面

        private void InitForm()
        {
            try
            {
                this.Text = Socket_Cache.System.WPE + " - " + Socket_Operation.AssemblyVersion;

                this.tSocketProxy.Enabled = true;
                this.tShowProxyState.Enabled = true;
                this.cbbAuthType.SelectedIndex = 0;                

                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Response));
                this.tsslProxySpeed.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_172), "0.00", "0.00");
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitProxyIPAppoint()
        {
            try
            {
                IPAddress[] ipAddresses = Socket_Operation.GetLocalIPAddress();

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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            this.cbbAuthType.Enabled = this.bAccount.Enabled = this.cbEnable_Auth.Checked;
        }

        private void bAccount_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyAccountListForm();
        }

        #endregion

        #region//加载本页面的运行配置

        private void LoadConfigs_Parameter()
        {
            try
            {
                Socket_Cache.System.LoadRunConfig_FromDB();

                this.cbProxyIP_Auto.Checked = Socket_Cache.SocketProxy.ProxyIP_Auto;            
                this.cbEnable_SOCKS5.Checked = Socket_Cache.SocketProxy.Enable_SOCKS5;
                this.nudProxyPort.Value = Socket_Cache.SocketProxy.ProxyPort;
                this.cbEnable_Auth.Checked = Socket_Cache.SocketProxy.Enable_Auth;                

                this.cbNoRecordData.Checked = Socket_Cache.SocketProxy.NoRecord;
                this.cbDeleteClosed.Checked = Socket_Cache.SocketProxy.DelClosed;

                this.cbLogList_AutoRoll.Checked = Socket_Cache.LogList.Proxy_AutoRoll;
                this.cbLogList_AutoClear.Checked = Socket_Cache.LogList.Proxy_AutoClear;
                this.nudLogList_AutoClearValue.Value = Socket_Cache.LogList.Proxy_AutoClear_Value;

                this.cbEnable_MapLocal.Checked = Socket_Cache.ProxyMapping.Enable_MapLocal;
                this.cbEnable_MapRemote.Checked = Socket_Cache.ProxyMapping.Enable_MapRemote;

                this.cbEnable_ExternalProxy.Checked = Socket_Cache.SocketProxy.Enable_ExternalProxy;
                this.txtExternalProxy_IP.Text = Socket_Cache.SocketProxy.ExternalProxy_IP;
                this.txtExternalProxy_Port.Text = Socket_Cache.SocketProxy.ExternalProxy_Port.ToString();
                this.cbExternalProxy_AppointPort.Checked = Socket_Cache.SocketProxy.Enable_ExternalProxy_AppointPort;
                this.txtExternalProxy_AppointPort.Text = Socket_Cache.SocketProxy.ExternalProxy_AppointPort;
                this.cbExternalProxy_EnableAuth.Checked = Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth;
                this.txtExternalProxy_UserName.Text = Socket_Cache.SocketProxy.ExternalProxy_UserName;
                this.txtExternalProxy_PassWord.Text = Socket_Cache.SocketProxy.ExternalProxy_PassWord;

                this.cbSpeedMode.Checked = Socket_Cache.SocketProxy.SpeedMode;

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_35));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存本页面的运行配置

        private void SaveConfigs_Parameter()
        {
            try
            {
                Socket_Cache.SocketProxy.ProxyIP_Auto = this.cbProxyIP_Auto.Checked;             
                Socket_Cache.SocketProxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Socket_Cache.SocketProxy.ProxyPort = ((ushort)this.nudProxyPort.Value);
                Socket_Cache.SocketProxy.Enable_Auth = this.cbEnable_Auth.Checked;                

                Socket_Cache.SocketProxy.NoRecord = this.cbNoRecordData.Checked;
                Socket_Cache.SocketProxy.DelClosed = this.cbDeleteClosed.Checked;

                Socket_Cache.LogList.Proxy_AutoRoll = this.cbLogList_AutoRoll.Checked;
                Socket_Cache.LogList.Proxy_AutoClear = this.cbLogList_AutoClear.Checked;
                Socket_Cache.LogList.Proxy_AutoClear_Value = this.nudLogList_AutoClearValue.Value;

                Socket_Cache.ProxyMapping.Enable_MapLocal = this.cbEnable_MapLocal.Checked;
                Socket_Cache.ProxyMapping.Enable_MapRemote = this.cbEnable_MapRemote.Checked;

                Socket_Cache.SocketProxy.Enable_ExternalProxy = this.cbEnable_ExternalProxy.Checked;
                Socket_Cache.SocketProxy.ExternalProxy_IP = this.txtExternalProxy_IP.Text.Trim();
                Socket_Cache.SocketProxy.ExternalProxy_Port = ushort.Parse(this.txtExternalProxy_Port.Text.Trim());
                Socket_Cache.SocketProxy.Enable_ExternalProxy_AppointPort = this.cbExternalProxy_AppointPort.Checked;
                Socket_Cache.SocketProxy.ExternalProxy_AppointPort = this.txtExternalProxy_AppointPort.Text.Trim();
                Socket_Cache.SocketProxy.Enable_ExternalProxy_Auth = this.cbExternalProxy_EnableAuth.Checked;
                Socket_Cache.SocketProxy.ExternalProxy_UserName = this.txtExternalProxy_UserName.Text.Trim();
                Socket_Cache.SocketProxy.ExternalProxy_PassWord = this.txtExternalProxy_PassWord.Text.Trim();

                Socket_Cache.SocketProxy.SpeedMode = this.cbSpeedMode.Checked;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            Socket_Cache.ProxyMapping.Enable_MapLocal = this.cbEnable_MapLocal.Checked;
        }

        private void Enable_MapRemote_Changed()
        {
            this.bProxyMapping_Remote.Enabled = this.cbEnable_MapRemote.Checked;
            Socket_Cache.ProxyMapping.Enable_MapRemote = this.cbEnable_MapRemote.Checked;
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
            Socket_Cache.SocketProxy.SpeedMode = this.cbSpeedMode.Checked;
        }

        #endregion

        #region//开始代理

        private void bStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckProxySet())
                {
                    Socket_Cache.SocketProxy.IsListening = true;

                    this.InitProxyStart();
                    this.UpdateUIState(starting: true);

                    if (SocketServer == null)
                    {
                        InitializeServerSocket();
                    }

                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_142));
                }                                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Socket_Cache.SocketProxy.ProxyPort);
                SocketServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    LingerState = new LingerOption(false, 0)
                };

                SocketServer.Bind(ep);
                SocketServer.Listen(backlog: 1000);

                AcceptClients();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }        

        private void InitProxyStart()
        {
            try
            {
                if (this.cbProxyIP_Auto.Checked)
                {
                    Socket_Cache.SocketProxy.ProxyTCP_IP = IPAddress.Any;
                    Socket_Cache.SocketProxy.ProxyUDP_IP = IPAddress.Parse(this.cbbProxyIP_Appoint.Items[0].ToString());
                }
                else
                {
                    Socket_Cache.SocketProxy.ProxyTCP_IP = IPAddress.Parse(this.cbbProxyIP_Appoint.SelectedItem.ToString());
                    Socket_Cache.SocketProxy.ProxyUDP_IP = IPAddress.Parse(this.cbbProxyIP_Appoint.SelectedItem.ToString());
                }

                Socket_Cache.SocketProxy.ProxyTotal_CNT = 0;
                Socket_Cache.SocketProxy.ProxyTCP_CNT = 0;
                Socket_Cache.SocketProxy.ProxyUDP_CNT = 0;

                this.SaveConfigs_Parameter();

                string sProxyIP = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_137), Socket_Cache.SocketProxy.ProxyTCP_IP, Socket_Cache.SocketProxy.ProxyUDP_IP);
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sProxyIP);

                if (this.cbEnable_Auth.Checked)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_169));
                }

                if (this.cbEnable_ExternalProxy.Checked)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_170));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//接受客户端链接

        private void AcceptClients()
        {
            try
            {
                if (Socket_Cache.SocketProxy.IsListening)
                {
                    SocketServer.BeginAccept(new AsyncCallback(AcceptCallback), null);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (Socket_Cache.SocketProxy.IsListening)
                {
                    Socket clientSocket = SocketServer.EndAccept(ar);

                    if (clientSocket != null)
                    {
                        Socket_Cache.SocketProxy.HandleClient(clientSocket);
                    }

                    AcceptClients();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

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

                    Socket_Cache.SocketProxy.AddressType atExternalProxy = Socket_Operation.GetAddressType_ByString(ExternalProxyIP);
                    if (atExternalProxy != Socket_Cache.SocketProxy.AddressType.IPv4 && atExternalProxy != Socket_Cache.SocketProxy.AddressType.Domain)
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//停止代理

        private void bStop_Click(object sender, EventArgs e)
        {
            try
            {
                Socket_Cache.SocketProxy.IsListening = false;

                if (SocketServer != null)
                {
                    try
                    {
                        SocketServer.Close();
                    }
                    catch (Exception ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                    finally
                    {
                        SocketServer = null;
                    }
                }

                this.UpdateUIState(starting: false);

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_143));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//计时器

        private void tSocketProxy_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyTCP.Count > 0)
                {
                    Socket_Cache.SocketProxyList.ProxyTCP_ToList();
                }

                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyUDP.Count > 0)
                {
                    Socket_Cache.SocketProxyList.ProxyUDP_ToList();
                }

                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count > 0)
                {
                    Socket_Cache.SocketProxyList.ProxyData_ToList();
                }

                if (Socket_Cache.LogQueue.qProxy_Log.Count > 0)
                {
                    Socket_Cache.LogList.LogToList(Socket_Cache.System.LogType.Proxy);

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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            this.dgvAuth.Refresh();
            await Socket_Cache.SocketProxy.UpdateProxyUDP();
            await Socket_Cache.ProxyAccount.UpdateOnlineStatus();
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
                Socket_Cache.SocketProxy.ClearProxyAuthList();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ShowProxyInfo()
        {
            Socket_Cache.SocketProxy.ProxyTotal_CNT = 0;
            Socket_Cache.SocketProxy.ProxyTCP_CNT = 0;
            Socket_Cache.SocketProxy.ProxyUDP_CNT = 0;
            Socket_Cache.SocketProxy.Total_Request = 0;
            Socket_Cache.SocketProxy.Total_Response = 0;
        }

        private void CleanUp_ProxyData()
        {
            Socket_Cache.SocketProxyQueue.ResetProxy_DataQueue();
            Socket_Cache.SocketProxyList.ResetProxy_DataList();
            this.tvProxyData.Nodes.Clear();
        }

        private void CleanUp_ProxyInfo()
        {
            Socket_Cache.SocketProxyQueue.ResetProxy_TCPQueue();
            Socket_Cache.SocketProxyList.ResetProxy_TCPList();
            this.tvProxyInfo.Nodes.Clear();
        }

        private void CleanUp_LogList()
        {
            Socket_Cache.LogQueue.ResetLogQueue(Socket_Cache.System.LogType.Proxy);
            Socket_Cache.LogList.ResetLogList(Socket_Cache.System.LogType.Proxy);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        case Socket_Cache.SocketProxy.DomainType.Socket:
                            RootImgIndex = 0;
                            RequestImgIndex = 2;
                            ResponseImgIndex = 3;
                            DataImgIndex = 1;
                            break;

                        case Socket_Cache.SocketProxy.DomainType.Http:
                            RootImgIndex = 7;
                            RequestImgIndex = 2;
                            ResponseImgIndex = 3;
                            DataImgIndex = 8;
                            break;

                        case Socket_Cache.SocketProxy.DomainType.Https:
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
                                case Socket_Cache.SocketProxy.DataType.Request:
                                    DataNode = RootNode.Nodes[0];
                                    break;

                                case Socket_Cache.SocketProxy.DataType.Response:
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    if (spt.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                    {
                        int iRootImgIndex = -1;
                        int iChildImgIndex = 5;

                        string ClientIP = Socket_Cache.SocketProxy.GetClientIPAddress(spt);
                        string ClientUserName = Socket_Cache.ProxyAccount.GetUserName_ByAccountID(spt.AID);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                    foreach (Socket_ProxyTCP spt in Socket_Cache.SocketProxyList.lstProxyTCP.ToList())
                    {
                        if (spt.Client.Socket == null)
                        {
                            string ClientIP = Socket_Cache.SocketProxy.GetClientIPAddress(spt);
                            string ClientUserName = Socket_Cache.ProxyAccount.GetUserName_ByAccountID(spt.AID);

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
                                                Socket_Cache.SocketProxy.DeleteProxyAuthInfo_ByIPAndAID(ClientIP, spt.AID);

                                                if (this.cbDeleteClosed.Checked)
                                                {
                                                    RootNode.Remove();
                                                }

                                                if (spt.AID != null && spt.AID != Guid.Empty)
                                                {
                                                    Socket_Cache.ProxyAccount.SetOnline_ByAccountID(spt.AID, false);
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
                        Socket_Cache.SocketProxyList.ClearTCP(spt);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    foreach (Proxy_AuthInfo pai in Socket_Cache.SocketProxy.lstProxyAuth.ToList())
                    {
                        string ClientIP = pai.IPAddress.ToString();
                        pai.LinksNumber = Socket_Cache.ProxyAccount.GetLinksNumber_ByAccountID(pai.AID, ClientIP, this.tvProxyInfo.Nodes);
                        pai.DevicesNumber = Socket_Cache.ProxyAccount.GetDevicesNumber_ByAccountID(pai.AID);
                    }
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });
        }

        #endregion

        #region//显示认证列表（异步）        

        private void Event_RecProxyAuth(Proxy_AuthInfo pai)
        {
            try
            {
                if (!dgvAuth.IsDisposed)
                {
                    dgvAuth.Invoke(new MethodInvoker(delegate
                    {
                        Proxy_AuthInfo paiItem = Socket_Cache.SocketProxy.lstProxyAuth.FirstOrDefault(item => item.IPAddress == pai.IPAddress && item.AID == pai.AID);

                        if (paiItem != null)
                        {
                            paiItem.AuthResult = pai.AuthResult;
                            paiItem.AuthTime = pai.AuthTime;
                        }
                        else
                        {
                            Socket_Cache.SocketProxy.lstProxyAuth.Add(pai);
                        }

                        this.dgvAuth.Refresh();
                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void dgvAuth_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvAuth.Columns["cAuthID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvAuth.Columns["cUserName"].Index)
                {
                    Guid AID = Guid.Parse(dgvAuth.Rows[e.RowIndex].Cells["cAID"].Value.ToString());
                    e.Value = Socket_Cache.ProxyAccount.GetUserName_ByAccountID(AID);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvAuth.Columns["cAuthResult"].Index)
                {
                    e.Value = Socket_Cache.ProxyAccount.GetImg_ByAuthResult(Convert.ToBoolean(e.Value));
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理信息

        private void ShowProxyInfo()
        {
            try
            {
                ulong ProxyTCP_CNT = Socket_Cache.SocketProxy.ProxyTCP_CNT;
                ulong ProxyUDP_CNT = Socket_Cache.SocketProxy.ProxyUDP_CNT;
                ulong ProxyTotal_CNT = ProxyTCP_CNT + ProxyUDP_CNT;
                int ProxyAccountOnLine = Socket_Operation.GetOnLineProxyAccountCount(Socket_Cache.ProxyAccount.lstProxyAccount);
                Socket_Cache.SocketProxy.ProxyOnLineInfo = ProxyAccountOnLine.ToString() + "/" + Socket_Cache.ProxyAccount.lstProxyAccount.Count.ToString();
                Socket_Cache.SocketProxy.ProxyBytesInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Response));
                
                this.tlProxyTotal_CNT.Text = ProxyTotal_CNT.ToString();
                this.tlProxyTCP_CNT.Text = ProxyTCP_CNT.ToString();
                this.tlProxyUDP_CNT.Text = ProxyUDP_CNT.ToString();
                this.tlProxyCache_CNT.Text = Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count.ToString();
                this.tlProxyLinks_CNT.Text = Socket_Cache.SocketProxyList.lstProxyTCP.Count.ToString();
                this.tlProxyAccount_CNT.Text = Socket_Cache.SocketProxy.ProxyOnLineInfo;
                this.tsslTotalBytes.Text = Socket_Cache.SocketProxy.ProxyBytesInfo;
                this.tsslProxySpeed.Text = Socket_Cache.SocketProxy.ProxySpeedInfo;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理性能图表

        private void ShowProxyChart()
        {
            try
            {
                double dProxySpeed_Uplink = Socket_Cache.SocketProxy.ProxySpeed_Uplink;
                Socket_Cache.SocketProxy.ProxySpeed_Uplink = 0;
                double dProxySpeed_Downlink = Socket_Cache.SocketProxy.ProxySpeed_Downlink;                
                Socket_Cache.SocketProxy.ProxySpeed_Downlink = 0;                
           
                this.Invoke((MethodInvoker)delegate
                {
                    Series sProxy_Uplink = cProxyChart.Series[0];
                    if (sProxy_Uplink.Points.Count >= Socket_Cache.SocketProxy.MaxChartPoint)
                    {
                        sProxy_Uplink.Points.RemoveAt(0);
                    }

                    double dChartProxySpeed_Uplink = dProxySpeed_Uplink / Socket_Cache.SocketProxy.MaxNetworkSpeed;
                    if (dChartProxySpeed_Uplink > 10)
                    {
                        dChartProxySpeed_Uplink = 10;
                    }
                    sProxy_Uplink.Points.AddY(dChartProxySpeed_Uplink);

                    Series sProxy_Downlink = cProxyChart.Series[1];
                    if (sProxy_Downlink.Points.Count >= Socket_Cache.SocketProxy.MaxChartPoint)
                    {
                        sProxy_Downlink.Points.RemoveAt(0);
                    }

                    double dChartProxySpeed_Downlink = dProxySpeed_Downlink / Socket_Cache.SocketProxy.MaxNetworkSpeed;
                    if (dChartProxySpeed_Downlink > 10)
                    {
                        dChartProxySpeed_Downlink = 10;
                    }
                    sProxy_Downlink.Points.AddY(dChartProxySpeed_Downlink);

                    Socket_Cache.SocketProxy.ProxySpeedInfo = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_172), (dProxySpeed_Uplink / 1024).ToString("0.00"), (dProxySpeed_Downlink / 1024).ToString("0.00"));
                });
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        #endregion

        
    }
}

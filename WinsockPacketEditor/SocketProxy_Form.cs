using System;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;
using Be.Windows.Forms;
using System.Threading.Tasks;
using WPELibrary;

namespace WinsockPacketEditor
{
    public partial class SocketProxy_Form : Form
    {
        private Socket_Form socketForm;
        private static Socket SocketServer;

        #region//窗体加载

        public SocketProxy_Form(Socket_Form socketForm)
        {
            try
            {
                InitializeComponent();
                this.socketForm = socketForm;
                this.InitSocketDGV();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//窗体事件

        private void SocketProxy_Form_Load(object sender, EventArgs e)
        {
            this.InitForm();
            this.LoadConfigs_Parameter();
        }

        private void SocketProxy_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ExitMainForm();
        }

        private void ExitMainForm()
        {
            try
            {
                this.SaveConfigs_Parameter();

                if (this.cbEnable_SystemProxy.Checked)
                { 
                    Socket_Operation.StopSystemProxy();
                }

                if (this.socketForm != null)
                {
                    this.socketForm.ExitMainForm();
                }
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
                dgvLogList.AutoGenerateColumns = false;
                dgvLogList.DataSource = Socket_Cache.LogList.lstProxyLog;
                dgvLogList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvLogList, true, null);
                Socket_Cache.LogList.RecProxyLog += new Socket_Cache.LogList.ProxyLogReceived(Event_RecProxyLog);

                tvProxyData.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyData, true, null);
                Socket_Cache.SocketProxyList.RecProxyData += new Socket_Cache.SocketProxyList.ProxyDataReceived(Event_RecProxyData);

                tvProxyInfo.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvProxyInfo, true, null);
                Socket_Cache.SocketProxyList.RecProxyInfo += new Socket_Cache.SocketProxyList.ProxyInfoReceived(Event_RecProxyInfo);
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
                this.Text = Socket_Cache.WPE + " - " + Socket_Operation.AssemblyVersion;

                this.tSocketProxy.Enabled = true;
                this.tCheckProxyState.Enabled = true;

                this.InitProxyIPAppoint();

                this.ProxyIP_Appoint_Changed();
                this.EnableAuth_Changed();
                this.LogList_AutoClear_Changed();
                this.Enable_EXTHttp_Changed();
                this.Enable_EXTHttps_Changed();                
                
                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Response));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async void InitProxyIPAppoint()
        {
            try
            {
                IPAddress[] ipAddresses = await Socket_Operation.GetLocalIPAddress();

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
            try
            {
                this.cbbProxyIP_Appoint.Enabled = !this.cbProxyIP_Auto.Checked;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//启用身份认证

        private void cbEnable_Auth_CheckedChanged(object sender, EventArgs e)
        {
            this.EnableAuth_Changed();
        }

        private void EnableAuth_Changed()
        {
            try
            {
                this.txtAuth_UserName.Enabled = this.txtAuth_PassWord.Enabled = this.cbEnable_Auth.Checked;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//加载系统参数

        private void LoadConfigs_Parameter()
        {
            try
            {
                Socket_Operation.LoadConfigs_SocketProxy();

                this.cbProxyIP_Auto.Checked = Socket_Cache.SocketProxy.ProxyIP_Auto;            
                this.cbEnable_SOCKS5.Checked = Socket_Cache.SocketProxy.Enable_SOCKS5;
                this.nudProxyPort.Value = Socket_Cache.SocketProxy.ProxyPort;
                this.cbEnable_Auth.Checked = Socket_Cache.SocketProxy.Enable_Auth;
                this.txtAuth_UserName.Text = Socket_Cache.SocketProxy.Auth_UserName;
                this.txtAuth_PassWord.Text = Socket_Cache.SocketProxy.Auth_PassWord;

                this.cbNoRecordData.Checked = Socket_Cache.SocketProxyList.NoRecord;
                this.cbDeleteClosed.Checked = Socket_Cache.SocketProxyList.DelClosed;

                this.cbLogList_AutoRoll.Checked = Socket_Cache.LogList.Proxy_AutoRoll;
                this.cbLogList_AutoClear.Checked = Socket_Cache.LogList.Proxy_AutoClear;
                this.nudLogList_AutoClearValue.Value = Socket_Cache.LogList.Proxy_AutoClear_Value;

                this.cbEnable_EXTHttp.Checked = Socket_Cache.SocketProxy.Enable_EXTHttp;
                this.txtEXTHttpIP.Text = Socket_Cache.SocketProxy.EXTHttpIP;
                this.txtEXTHttpPort.Text = Socket_Cache.SocketProxy.EXTHttpPort.ToString();
                this.cbEnable_EXTHttps.Checked = Socket_Cache.SocketProxy.Enable_EXTHttps;
                this.txtEXTHttpsIP.Text = Socket_Cache.SocketProxy.EXTHttpsIP;
                this.txtEXTHttpsPort.Text = Socket_Cache.SocketProxy.EXTHttpsPort.ToString();
                this.txtAppointHttpPort.Text = Socket_Cache.SocketProxy.AppointHttpPort;
                this.txtAppointHttpsPort.Text = Socket_Cache.SocketProxy.AppointHttpsPort;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存系统参数

        private void SaveConfigs_Parameter()
        {
            try
            {
                Socket_Cache.SocketProxy.ProxyIP_Auto = this.cbProxyIP_Auto.Checked;             
                Socket_Cache.SocketProxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Socket_Cache.SocketProxy.ProxyPort = ((ushort)this.nudProxyPort.Value);
                Socket_Cache.SocketProxy.Enable_Auth = this.cbEnable_Auth.Checked;
                Socket_Cache.SocketProxy.Auth_UserName = this.txtAuth_UserName.Text.Trim();
                Socket_Cache.SocketProxy.Auth_PassWord = this.txtAuth_PassWord.Text.Trim();

                Socket_Cache.SocketProxyList.NoRecord = this.cbNoRecordData.Checked;
                Socket_Cache.SocketProxyList.DelClosed = this.cbDeleteClosed.Checked;

                Socket_Cache.LogList.Proxy_AutoRoll = this.cbLogList_AutoRoll.Checked;
                Socket_Cache.LogList.Proxy_AutoClear = this.cbLogList_AutoClear.Checked;
                Socket_Cache.LogList.Proxy_AutoClear_Value = this.nudLogList_AutoClearValue.Value;

                Socket_Cache.SocketProxy.Enable_EXTHttp = this.cbEnable_EXTHttp.Checked;
                Socket_Cache.SocketProxy.EXTHttpIP = this.txtEXTHttpIP.Text.Trim();
                Socket_Cache.SocketProxy.EXTHttpPort = ushort.Parse(this.txtEXTHttpPort.Text.Trim());
                Socket_Cache.SocketProxy.Enable_EXTHttps = this.cbEnable_EXTHttps.Checked;
                Socket_Cache.SocketProxy.EXTHttpsIP = this.txtEXTHttpsIP.Text.Trim();
                Socket_Cache.SocketProxy.EXTHttpsPort = ushort.Parse(this.txtEXTHttpsPort.Text.Trim());
                Socket_Cache.SocketProxy.AppointHttpPort = this.txtAppointHttpPort.Text.Trim();
                Socket_Cache.SocketProxy.AppointHttpsPort = this.txtAppointHttpsPort.Text.Trim();

                Socket_Operation.SaveConfigs_SocketProxy();
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

        #region//外部代理设置

        private void cbEnableHttpProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.Enable_EXTHttp_Changed();
        }

        private void Enable_EXTHttp_Changed()
        {
            try
            {
                this.txtEXTHttpIP.Enabled = this.txtEXTHttpPort.Enabled = this.txtAppointHttpPort.Enabled = this.cbEnable_EXTHttp.Checked;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cbEnableHttpsProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.Enable_EXTHttps_Changed();
        }

        private void Enable_EXTHttps_Changed()
        {
            try
            {
                this.txtEXTHttpsIP.Enabled = this.txtEXTHttpsPort.Enabled = this.txtAppointHttpsPort.Enabled = this.cbEnable_EXTHttps.Checked;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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

                    this.bStart.Enabled = false;
                    this.bStop.Enabled = true;
                    this.tpProxySet.Enabled = false;
                    this.tpAuthSet.Enabled = false;
                    this.tpExternalProxy.Enabled = false;

                    if (SocketServer == null)
                    {
                        IPEndPoint ep = new IPEndPoint(IPAddress.Any, Socket_Cache.SocketProxy.ProxyPort);
                        SocketServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        SocketServer.Bind(ep);
                        SocketServer.Listen(int.MaxValue);
                        AcceptClients();                        
                    }

                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_142));
                }
                else
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_141));
                }
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

                if (this.cbEnable_EXTHttp.Checked)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_170));
                }

                if (this.cbEnable_EXTHttps.Checked)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_171));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

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
                if (!this.cbEnable_SOCKS5.Checked)
                {
                    return false;
                }

                if (this.cbEnable_Auth.Checked)
                {
                    string sAuth_UserName = this.txtAuth_UserName.Text.Trim();
                    string sAuth_PassWord = this.txtAuth_PassWord.Text.Trim();

                    if (string.IsNullOrEmpty(sAuth_UserName) || string.IsNullOrEmpty(sAuth_PassWord))
                    {
                        return false;
                    }
                }

                if (this.cbEnable_EXTHttp.Checked)
                {
                    string HttpIP = this.txtEXTHttpIP.Text.Trim();
                    ushort HttpPort = ushort.Parse(this.txtEXTHttpPort.Text.Trim());
                    string AppointHttpPort = this.txtAppointHttpPort.Text.Trim();

                    if (string.IsNullOrEmpty(HttpIP) || HttpPort < 1)
                    {
                        return false;
                    }

                    if (!Socket_Operation.IsIPv4(HttpIP))
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(AppointHttpPort))
                    {
                        return false;
                    }                    
                }

                if (this.cbEnable_EXTHttps.Checked)
                {
                    string HttpsIP = this.txtEXTHttpsIP.Text.Trim();
                    ushort HttpsPort = ushort.Parse(this.txtEXTHttpsPort.Text.Trim());
                    string AppointHttpsPort = this.txtAppointHttpsPort.Text.Trim();

                    if (string.IsNullOrEmpty(HttpsIP) || HttpsPort < 1)
                    {
                        return false;
                    }

                    if (!Socket_Operation.IsIPv4(HttpsIP))
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(AppointHttpsPort))
                    {
                        return false;
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

                this.bStart.Enabled = true;
                this.bStop.Enabled = false;
                this.tpProxySet.Enabled = true;
                this.tpAuthSet.Enabled = true;
                this.tpExternalProxy.Enabled = true;

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_143));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//计时器

        private async void tSocketProxy_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyInfo.Count > 0)
                {
                    await Socket_Cache.SocketProxyList.ProxyInfoToList();
                }

                if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count > 0)
                {
                    await Socket_Cache.SocketProxyList.ProxyDataToList();
                }

                if (Socket_Cache.LogQueue.qProxy_Log.Count > 0)
                {
                    await Socket_Cache.LogList.LogToList(Socket_Cache.LogType.Proxy);

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

        private async void tCheckProxyState_Tick(object sender, EventArgs e)
        {
            await this.CheckProxyState();
            this.ShowProxyInfo();
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
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ShowProxyInfo()
        {
            try
            {
                Socket_Cache.SocketProxy.ProxyTotal_CNT = 0;
                Socket_Cache.SocketProxy.ProxyTCP_CNT = 0;
                Socket_Cache.SocketProxy.ProxyUDP_CNT = 0;
                Socket_Cache.SocketProxy.Total_Request = 0;
                Socket_Cache.SocketProxy.Total_Response = 0;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ProxyData()
        {
            try
            {
                Socket_Cache.SocketProxyQueue.ResetProxyDataQueue();
                Socket_Cache.SocketProxyList.ResetProxyDataList();
                this.tvProxyData.Nodes.Clear();                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ProxyInfo()
        {
            try
            {
                Socket_Cache.SocketProxyQueue.ResetProxyInfoQueue();
                Socket_Cache.SocketProxyList.ResetProxyInfoList();
                this.tvProxyInfo.Nodes.Clear();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_LogList()
        {
            try
            {
                Socket_Cache.LogQueue.ResetLogQueue(Socket_Cache.LogType.Proxy);
                Socket_Cache.LogList.ResetLogList(Socket_Cache.LogType.Proxy);
                this.dgvLogList.Rows.Clear();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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

        #region//清理 TCP 和 UDP 端口（异步）

        private async Task CheckProxyState()
        {
            try
            {
                DateTime dtNow = DateTime.Now;

                for (int i = 0; i < Socket_Cache.SocketProxyList.lstProxyInfo.Count; i++)
                {
                    //清理已关闭的客户端链接（TCP）
                    if (Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientSocket == null)
                    {
                        TreeNode ClientNode = await Socket_Operation.FindNodeAsync(this.tvProxyInfo, Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientAddress);

                        if (ClientNode != null)
                        {
                            if (!IsDisposed)
                            {
                                tvProxyInfo.BeginInvoke(new MethodInvoker(delegate
                                {
                                    if (this.cbDeleteClosed.Checked)
                                    {
                                        ClientNode.Remove();
                                    }
                                    else
                                    {
                                        ClientNode.ImageIndex = 6;
                                        ClientNode.SelectedImageIndex = 6;
                                    }                                    
                                }));
                            }
                        }
                    }

                    //清理 UDP 端口
                    if (Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientUDP != null && Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientUDP_Time != null)
                    {
                        TimeSpan timeSpan = dtNow - Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientUDP_Time;
                        if (timeSpan.TotalSeconds > Socket_Cache.SocketProxy.UDPCloseTime)
                        {
                            Socket_Cache.SocketProxyList.lstProxyInfo[i].CloseUDPClient();
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

                    TreeNode RootNode = await Socket_Operation.FindNodeAsync(this.tvProxyData, spd.Domain);
                    if (RootNode == null)
                    {
                        RootNode = await Socket_Operation.AddTreeNode(this.tvProxyData, this.tvProxyData.Nodes, spd.Domain, RootImgIndex, null);
                        await Socket_Operation.AddTreeNode(this.tvProxyData, RootNode.Nodes, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_138), RequestImgIndex, null);
                        await Socket_Operation.AddTreeNode(this.tvProxyData, RootNode.Nodes, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_139), ResponseImgIndex, null);
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
                        await Socket_Operation.AddTreeNode(this.tvProxyData, DataNode.Nodes, sDataNodeName, DataImgIndex, spd.Buffer);
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

        private async void Event_RecProxyInfo(Socket_ProxyInfo spi)
        {
            try
            {
                if (!tvProxyInfo.IsDisposed)
                {
                    if (spi != null)
                    {
                        if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                        {
                            int iRootImgIndex = -1;
                            int iChildImgIndex = 5;
                            string sRootName = Socket_Cache.SocketProxy.GetClientName(spi);                            

                            if (!string.IsNullOrEmpty(sRootName))
                            {
                                string sChildName = spi.ClientAddress;

                                TreeNode RootNode = await Socket_Operation.FindNodeAsync(this.tvProxyInfo, sRootName);
                                if (RootNode == null)
                                {
                                    RootNode = await Socket_Operation.AddTreeNode(this.tvProxyInfo, this.tvProxyInfo.Nodes, sRootName, iRootImgIndex, null);
                                }

                                TreeNode ChildNode = await Socket_Operation.FindNodeAsync(this.tvProxyInfo, sChildName);
                                if (ChildNode == null)
                                {
                                    ChildNode = await Socket_Operation.AddTreeNode(this.tvProxyInfo, RootNode.Nodes, sChildName, iChildImgIndex, null);
                                }
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

        #region//显示日志列表（异步）        

        private void Event_RecProxyLog(Socket_LogInfo sli)
        {
            try
            {
                if (!dgvLogList.IsDisposed)
                {
                    dgvLogList.Invoke(new MethodInvoker(delegate
                    {
                        Socket_Cache.LogList.lstProxyLog.Add(sli);
                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理信息（异步）

        private void ShowProxyInfo()
        {
            try
            {
                ulong ProxyTCP_CNT = Socket_Cache.SocketProxy.ProxyTCP_CNT;
                ulong ProxyUDP_CNT = Socket_Cache.SocketProxy.ProxyUDP_CNT;
                ulong ProxyTotal_CNT = ProxyTCP_CNT + ProxyUDP_CNT;

                this.tlProxyTotal_CNT.Text = ProxyTotal_CNT.ToString();
                this.tlProxyTCP_CNT.Text = ProxyTCP_CNT.ToString();
                this.tlProxyUDP_CNT.Text = ProxyUDP_CNT.ToString();
                this.tlProxyCache_CNT.Text = Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count.ToString();             
                this.tlProxyLinks_CNT.Text = Socket_Cache.SocketProxyList.lstProxyInfo.Count.ToString();
                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Response));
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
                byte[] bBuffer = null;
                if (e.Node.Tag != null)
                {
                    bBuffer = e.Node.Tag as byte[];                  
                }

                if (bBuffer != null)
                {
                    DynamicByteProvider dbp = new DynamicByteProvider(bBuffer);
                    this.hbData.ByteProvider = dbp;
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
    }
}

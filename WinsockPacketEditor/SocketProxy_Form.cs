using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;
using Be.Windows.Forms;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    public partial class SocketProxy_Form : Form
    {
        private static Socket SocketServer;

        #region//窗体加载

        public SocketProxy_Form()
        {
            try
            {
                InitializeComponent();

                this.InitForm();
                this.InitSocketDGV();
                this.LoadConfigs_Parameter();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//窗体事件

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

                string sServerInfo = string.Empty;
                IPAddress[] ipAddresses = Socket_Operation.GetLocalIPAddress();

                this.Auth_CheckedChanged();
                this.LogList_AutoClearChange();

                foreach (var address in ipAddresses)
                {
                    sServerInfo += address.ToString() + ", ";
                }

                sServerInfo = sServerInfo.Trim().TrimEnd(',');

                this.tsslServerInfo.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_137), sServerInfo);
                this.tsslTotalBytes.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Response));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cbProxySet_Auth_CheckedChanged(object sender, EventArgs e)
        {
            this.Auth_CheckedChanged();
        }

        private void Auth_CheckedChanged()
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

                this.cbEnable_SOCKS5.Checked = Socket_Cache.SocketProxy.Enable_SOCKS5;
                this.nudProxyPort.Value = Socket_Cache.SocketProxy.ProxyPort;
                this.cbEnable_Auth.Checked = Socket_Cache.SocketProxy.Enable_Auth;
                this.txtAuth_UserName.Text = Socket_Cache.SocketProxy.Auth_UserName;
                this.txtAuth_PassWord.Text = Socket_Cache.SocketProxy.Auth_PassWord;

                this.cbLogList_AutoRoll.Checked = Socket_Cache.LogList.Proxy_AutoRoll;
                this.cbLogList_AutoClear.Checked = Socket_Cache.LogList.Proxy_AutoClear;
                this.nudLogList_AutoClearValue.Value = Socket_Cache.LogList.Proxy_AutoClear_Value;
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
                Socket_Cache.SocketProxy.Enable_SOCKS5 = this.cbEnable_SOCKS5.Checked;
                Socket_Cache.SocketProxy.ProxyPort = ((ushort)this.nudProxyPort.Value);
                Socket_Cache.SocketProxy.Enable_Auth = this.cbEnable_Auth.Checked;
                Socket_Cache.SocketProxy.Auth_UserName = this.txtAuth_UserName.Text.Trim();
                Socket_Cache.SocketProxy.Auth_PassWord = this.txtAuth_PassWord.Text.Trim();

                Socket_Cache.LogList.Proxy_AutoRoll = this.cbLogList_AutoRoll.Checked;
                Socket_Cache.LogList.Proxy_AutoClear = this.cbLogList_AutoClear.Checked;
                Socket_Cache.LogList.Proxy_AutoClear_Value = this.nudLogList_AutoClearValue.Value;

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

        #region//开始代理

        private void bStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckProxySet())
                {
                    Socket_Cache.SocketProxy.IsListening = true;
                    Socket_Cache.SocketProxy.ProxyIP = IPAddress.Any;

                    this.bStart.Enabled = false;
                    this.bStop.Enabled = true;
                    this.tpProxySet.Enabled = false;

                    this.SaveConfigs_Parameter();
                    this.StartListen();                    
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

        private async void StartListen()
        {
            try
            {
                if (SocketServer == null)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, Socket_Cache.SocketProxy.ProxyPort);
                    SocketServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    SocketServer.Bind(ep);
                    SocketServer.Listen(int.MaxValue);
                    await AcceptClients();                    
                }

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_142));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private Task AcceptClients()
        {
            return Task.Run(() =>
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
            });            
        }

        private async void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (Socket_Cache.SocketProxy.IsListening)
                {
                    Socket clientSocket = SocketServer.EndAccept(ar);

                    if (clientSocket != null)
                    {
                        await Socket_Cache.SocketProxy.HandleClient(clientSocket);
                    }

                    await AcceptClients();
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

                this.StopListen();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void StopListen()
        {
            try
            {
                //SocketServer.Close();
                //UDPListener.Close();

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
                if (!bgwProxyInfo.IsBusy)
                {
                    if (Socket_Cache.SocketProxyQueue.qSocket_ProxyInfo.Count > 0)
                    {
                        bgwProxyInfo.RunWorkerAsync();
                    }
                }

                if (!bgwProxyData.IsBusy)
                {
                    if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count > 0)
                    {
                        bgwProxyData.RunWorkerAsync();
                    }
                }

                if (!bgwLogList.IsBusy)
                {
                    if (Socket_Cache.LogQueue.qProxy_Log.Count > 0)
                    {
                        bgwLogList.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void tCheckProxyState_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.bgwCheckProxyState.IsBusy)
                {
                    this.bgwCheckProxyState.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
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

        #region//显示代理列表（异步）

        private void bgwProxyData_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_43), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Request), Socket_Operation.GetDisplayBytes(Socket_Cache.SocketProxy.Total_Response));
            Socket_Cache.SocketProxyList.ProxyDataToList();
        }

        private void bgwProxyData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.tsslTotalBytes.Text = e.Result.ToString();
        }

        private Task<TreeNode> AddProxyData(TreeNodeCollection Nodes, string NodeName, int ImgIndex, byte[] bData)
        {
            TreeNode tnReturn = null;

            try
            {
                if (!IsDisposed)
                {
                    tvProxyData.Invoke(new MethodInvoker(delegate
                    {
                        tnReturn = Nodes.Add(NodeName);

                        if (ImgIndex > -1)
                        {
                            tnReturn.ImageIndex = ImgIndex;
                            tnReturn.SelectedImageIndex = ImgIndex;
                        }

                        if (bData != null && bData.Length > 0)
                        {
                            tnReturn.Tag = bData;
                        }

                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return Task.FromResult(tnReturn);
        }

        private async void Event_RecProxyData(Socket_ProxyData spd)
        {
            try
            {
                if (!IsDisposed)
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
                        RootNode = this.AddProxyData(this.tvProxyData.Nodes, spd.Domain, RootImgIndex, null).Result;
                        await this.AddProxyData(RootNode.Nodes, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_138), RequestImgIndex, null);
                        await this.AddProxyData(RootNode.Nodes, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_139), ResponseImgIndex, null);
                    }

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
                    await this.AddProxyData(DataNode.Nodes, sDataNodeName, DataImgIndex, spd.Buffer);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示客户端列表（异步）        

        private void bgwProxyInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            Socket_Cache.SocketProxyList.ProxyInfoToList();
        }

        private Task<TreeNode> AddProxyInfo(TreeNodeCollection Nodes, string NodeName, int ImgIndex)
        {
            TreeNode tnReturn = null;

            try
            {
                if (!IsDisposed)
                {
                    tvProxyInfo.Invoke(new MethodInvoker(delegate
                    {
                        tnReturn = Nodes.Add(NodeName);

                        if (ImgIndex > -1)
                        {
                            tnReturn.ImageIndex = ImgIndex;
                            tnReturn.SelectedImageIndex = ImgIndex;
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return Task.FromResult(tnReturn);
        }

        private async void Event_RecProxyInfo(Socket_ProxyInfo spi)
        {
            try
            {
                if (spi != null)
                {
                    if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                    {
                        int iRootImgIndex = -1;
                        int iChildImgIndex = 5;
                        string sRootName = Socket_Cache.SocketProxy.GetClientName(spi).Result;
                        string sChildName = spi.ClientAddress;

                        if (!string.IsNullOrEmpty(sRootName))
                        {
                            TreeNode RootNode = await Socket_Operation.FindNodeAsync(this.tvProxyInfo, sRootName);
                            if (RootNode == null)
                            {
                                RootNode = this.AddProxyInfo(this.tvProxyInfo.Nodes, sRootName, iRootImgIndex).Result;
                            }

                            TreeNode ChildNode = await Socket_Operation.FindNodeAsync(this.tvProxyInfo, sChildName);
                            if (ChildNode == null)
                            {
                                ChildNode = this.AddProxyInfo(RootNode.Nodes, sChildName, iChildImgIndex).Result;
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

        private void bgwLogList_DoWork(object sender, DoWorkEventArgs e)
        {
            Socket_Cache.LogList.LogToList(Socket_Cache.LogType.Proxy);
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
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Event_RecProxyLog(Socket_LogInfo sli)
        {
            try
            {
                if (!IsDisposed)
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

        #region//显示代理数据（异步）

        private async void tvSocketProxy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                byte[] bBuffer = null;

                if (e.Node.Tag != null)
                {
                    bBuffer = e.Node.Tag as byte[];                  
                }

                await this.ShowSelectProxyData(bBuffer);                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private Task ShowSelectProxyData(byte[] bBuffer)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!IsDisposed)
                    {
                        hbData.Invoke(new MethodInvoker(delegate
                        {
                            if (bBuffer != null)
                            {
                                DynamicByteProvider dbp = new DynamicByteProvider(bBuffer);
                                this.hbData.ByteProvider = dbp;
                            }
                            else
                            {
                                this.hbData.ByteProvider = null;
                            }
                        }));
                    }                    
                }
                catch (Exception ex)
                {
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            });            
        }

        #endregion

        #region//清理 TCP 和 UDP 端口（异步）

        private void bgwCheckProxyState_DoWork(object sender, DoWorkEventArgs e)
        {
            this.CheckProxyState();
        }

        private async void CheckProxyState()
        {
            try
            {
                DateTime dtNow = DateTime.Now;

                for (int i = 0; i < Socket_Cache.SocketProxyList.lstProxyInfo.Count; i++)
                {
                    //清理 TCP 端口
                    if (Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientSocket == null)
                    {
                        TreeNode ClientNode = await Socket_Operation.FindNodeAsync(this.tvProxyInfo, Socket_Cache.SocketProxyList.lstProxyInfo[i].ClientAddress);

                        if (ClientNode != null)
                        {
                            if (!IsDisposed)
                            {
                                tvProxyInfo.BeginInvoke(new MethodInvoker(delegate
                                {
                                    ClientNode.ImageIndex = 6;
                                    ClientNode.SelectedImageIndex = 6;
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
    }
}

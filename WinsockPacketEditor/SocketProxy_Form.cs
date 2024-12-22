using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;
using Be.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SocketProxy_Form : Form
    {
        private Socket SocketServer;

        #region//窗体加载

        public SocketProxy_Form()
        {
            InitializeComponent();

            this.InitForm();
            this.InitSocketDGV();
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

                tvClientList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tvClientList, true, null);
                Socket_Cache.SocketProxyList.RecSocketProxy += new Socket_Cache.SocketProxyList.SocketProxyReceived(Event_RecSocketProxy);
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
                string sServerInfo = string.Empty;
                var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName()).Where(address => address.AddressFamily == AddressFamily.InterNetwork).ToArray();

                this.Auth_CheckedChanged();

                foreach (var address in ipAddresses)
                {
                    sServerInfo += address.ToString() + ", ";
                }

                sServerInfo = sServerInfo.Trim().TrimEnd(',');

                this.tsslServerInfo.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_137), sServerInfo);
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
                this.txtAuth_UserName.Enabled = this.txtAuth_PassWord.Enabled = this.cbProxySet_Auth.Checked;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion                

        #region//显示代理列表（异步）

        private void bgwProxyList_DoWork(object sender, DoWorkEventArgs e)
        {
            Socket_Cache.SocketProxyList.ProxyDataToList();
        }       

        private void Event_RecProxyData(Socket_ProxyData spd)
        {
            try
            {
                if (!IsDisposed)
                {
                    tvProxyData.BeginInvoke(new MethodInvoker(delegate
                    {
                        TreeNode RootNode = Socket_Operation.FindNodeByName(this.tvProxyData.Nodes, spd.Domain);

                        if (RootNode == null)
                        {
                            RootNode = this.tvProxyData.Nodes.Add(spd.Domain);
                            RootNode.ImageIndex = 0;
                            RootNode.SelectedImageIndex = 0;

                            TreeNode RequestNode = RootNode.Nodes.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_138));
                            RequestNode.ImageIndex = 2;
                            RequestNode.SelectedImageIndex = 2;

                            TreeNode ResponseNode = RootNode.Nodes.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_139));
                            ResponseNode.ImageIndex = 3;
                            ResponseNode.SelectedImageIndex = 3;
                        }

                        RootNode.ForeColor = Color.Black;

                        TreeNode ChildNode = new TreeNode();
                        switch (spd.DataType)
                        {
                            case Socket_Cache.SocketProxy.DataType.Request:
                                ChildNode = RootNode.Nodes[0];
                                break;

                            case Socket_Cache.SocketProxy.DataType.Response:
                                ChildNode = RootNode.Nodes[1];
                                break;
                        }
                        string sNodeName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_140), spd.Buffer.Length);
                        TreeNode DataNode = ChildNode.Nodes.Add(sNodeName);
                        DataNode.Tag = spd.Buffer;
                        DataNode.ImageIndex = 1;
                        DataNode.SelectedImageIndex = 1;

                        foreach (TreeNode node in this.tvProxyData.Nodes)
                        {
                            if (node == RootNode)
                            {
                                node.BackColor = Color.LightYellow;
                            }
                            else
                            {
                                node.BackColor = Color.White;
                            }
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Event_RecSocketProxy(Socket_ProxyInfo spi)
        {
            try
            {
                if (!IsDisposed)
                {
                    tvClientList.BeginInvoke(new MethodInvoker(delegate
                    {
                        string sRootName = ((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Address.ToString();
                        string sChildName = spi.ClientAddress;

                        TreeNode RootNode = Socket_Operation.FindNodeByName(this.tvClientList.Nodes, sRootName);                        

                        if (RootNode == null)
                        {
                            RootNode = this.tvClientList.Nodes.Add(sRootName);                        
                        }

                        TreeNode ChildNode = Socket_Operation.FindNodeByName(RootNode.Nodes, sChildName);

                        if (ChildNode == null)
                        {
                            ChildNode = RootNode.Nodes.Add(sChildName);                            
                        }                    

                        ChildNode.ImageIndex = 5;
                        ChildNode.SelectedImageIndex = 5;

                        foreach (TreeNode node in this.tvClientList.Nodes) 
                        {
                            if (node == RootNode)
                            {
                                node.BackColor = Color.LightYellow;
                            }
                            else
                            {
                                node.BackColor = Color.White;
                            }
                        }
                    }));
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

        #region//计时器

        private void tSocketProxy_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!bgwLogList.IsBusy)
                {
                    if (Socket_Cache.LogQueue.qProxy_Log.Count > 0)
                    {
                        bgwLogList.RunWorkerAsync();
                    }
                }

                if (!bgwProxyList.IsBusy)
                {
                    if (Socket_Cache.SocketProxyQueue.qSocket_ProxyData.Count > 0)
                    {
                        bgwProxyList.RunWorkerAsync();
                    }
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示选中的包数据

        private void tvSocketProxy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Tag != null)
                {
                    byte[] bBuffer = e.Node.Tag as byte[];

                    if (bBuffer != null)
                    {
                        DynamicByteProvider dbp = new DynamicByteProvider(bBuffer);
                        hbData.ByteProvider = dbp;
                    }
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
                this.CleanUp_ProxyList();
                this.CleanUp_ClientList();
                this.CleanUp_LogList();
                this.CleanUp_HexBox();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ProxyList()
        {
            try
            {
                this.tvProxyData.Nodes.Clear();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_ClientList()
        {
            try
            {
                this.tvClientList.Nodes.Clear();
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

        #region//开始代理

        private void bStart_Click(object sender, EventArgs e)
        {
            if (this.CheckProxySet())
            {
                this.bStart.Enabled = false;
                this.bStop.Enabled = true;
                this.tpProxySet.Enabled = false;

                Socket_Cache.SocketProxy.Port = ((ushort)this.nudProxySet_Port.Value);

                this.StartListen();
            }
            else
            {
                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_141));
            }
        }

        private void StartListen()
        {
            try
            {
                Socket_Cache.SocketProxy.bIsListening = true;

                if (SocketServer == null)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, Socket_Cache.SocketProxy.Port);
                    SocketServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    SocketServer.Bind(ep);
                    SocketServer.Listen(int.MaxValue);

                    AcceptClients();
                }

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_142));
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
                if (!this.cbSOCKS5.Checked)
                {
                    return false;
                }

                if (this.cbProxySet_Auth.Checked)
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
            this.bStart.Enabled = true;
            this.bStop.Enabled = false;
            this.tpProxySet.Enabled = true;

            this.StopListen();
        }

        private void StopListen()
        {
            try
            {
                Socket_Cache.SocketProxy.bIsListening = false;            

                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_143));
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//接受客户端连接

        private void AcceptClients()
        {
            try
            {
                if (Socket_Cache.SocketProxy.bIsListening)
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
                if (SocketServer != null)
                {
                    Socket clientSocket = SocketServer.EndAccept(ar);

                    if (clientSocket != null) 
                    {
                        HandleClient(clientSocket);
                    }
                    
                    AcceptClients();
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            try
            {
                Socket_ProxyInfo spi = new Socket_ProxyInfo
                {
                    ClientSocket = clientSocket,                    
                    ClientBuffer = new byte[clientSocket.ReceiveBufferSize],
                    TargetBuffer = new byte[clientSocket.ReceiveBufferSize],
                    ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Handshake
                };                

                spi.ClientSocket.BeginReceive(spi.ClientBuffer, 0, spi.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spi);                             
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//接收客户端请求

        private void ReadCallback(IAsyncResult ar)
        {
            Socket_ProxyInfo spi = (Socket_ProxyInfo)ar.AsyncState;

            try
            {
                if (Socket_Cache.SocketProxy.bIsListening && spi.ClientSocket != null)
                {
                    int bytesRead = spi.ClientSocket.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        spi.ClientData = new byte[bytesRead];
                        Buffer.BlockCopy(spi.ClientBuffer, 0, spi.ClientData, 0, bytesRead);

                        switch (spi.ProxyStep)
                        {
                            case Socket_Cache.SocketProxy.ProxyStep.Handshake:
                                this.Handshake(spi);
                                break;

                            case Socket_Cache.SocketProxy.ProxyStep.AuthUserName:
                                this.AuthUserName(spi);
                                break;

                            case Socket_Cache.SocketProxy.ProxyStep.Command:
                                this.Command(spi);
                                break;

                            case Socket_Cache.SocketProxy.ProxyStep.ForwardData:
                                this.ForwardData(spi);
                                break;
                        }

                        spi.ClientSocket.BeginReceive(spi.ClientBuffer, 0, spi.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spi);
                    }
                    else
                    {
                        this.UpdateClientSocket_Closed(spi);
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_144), spi.ClientAddress);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                this.UpdateClientSocket_Closed(spi);
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.ClientAddress + " " + ex.Message);
            }
        }

        #endregion

        #region//握手过程

        private void Handshake(Socket_ProxyInfo spi)
        {
            try
            {
                byte[] bClientData = spi.ClientData;

                byte bVERSION = bClientData[0];
                spi.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bVERSION;

                int iMETHODS_COUNT = bClientData[1];
                byte[] bMETHODS = new byte[iMETHODS_COUNT];                

                string sAuth = string.Empty;
                for (int i = 0; i < iMETHODS_COUNT; i++)
                {
                    bMETHODS[i] = bClientData[2 + i];

                    sAuth += (Socket_Cache.SocketProxy.AuthType)bMETHODS[i] + ", ";
                }

                sAuth = sAuth.Trim().TrimEnd(',');

                string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_145), spi.ProxyType, sAuth);
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);                

                byte[] bAuthRequest;
                if (this.cbProxySet_Auth.Checked)
                {
                    bAuthRequest = new byte[] { ((byte)Socket_Cache.SocketProxy.ProxyType.Socket5), ((byte)Socket_Cache.SocketProxy.AuthType.PassWord) };
                    spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.AuthUserName;

                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_146));
                }
                else
                {
                    bAuthRequest = new byte[] { ((byte)Socket_Cache.SocketProxy.ProxyType.Socket5), ((byte)Socket_Cache.SocketProxy.AuthType.None) };
                    spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;

                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_147));
                }

                spi.ClientSocket.Send(bAuthRequest);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//验证账号密码

        private void AuthUserName(Socket_ProxyInfo spi)
        {
            try
            {
                byte VERSION = spi.ClientData[0];

                if (VERSION == 0x01)
                { 
                    int USERNAME_LENGTH = spi.ClientData[1];
                    byte[] USERNAME = new byte[USERNAME_LENGTH];
                    Buffer.BlockCopy(spi.ClientData, 2, USERNAME, 0, USERNAME_LENGTH);

                    int PASSWORD_LENGTH = spi.ClientData[2 + USERNAME_LENGTH];
                    byte[] PASSWORD = new byte[PASSWORD_LENGTH];
                    Buffer.BlockCopy(spi.ClientData, 3 + USERNAME_LENGTH, PASSWORD, 0, PASSWORD_LENGTH);

                    string sUserName = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, USERNAME);
                    string sPassWord = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, PASSWORD);

                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_148), sUserName, sPassWord);
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);

                    string sAuth_UserName = this.txtAuth_UserName.Text.Trim();
                    string sAuth_PassWord = this.txtAuth_PassWord.Text.Trim();

                    bool bAuthOK = true;
                    if (!string.IsNullOrEmpty(sAuth_UserName) && !string.IsNullOrEmpty(sAuth_PassWord))
                    {
                        if (!sAuth_UserName.Equals(sUserName) || !sAuth_PassWord.Equals(sPassWord))
                        {
                            bAuthOK = false;
                        }                      
                    }                 

                    byte[] bAuth;
                    if (bAuthOK)
                    {
                        bAuth = new byte[] { 0x01, 0x00 };
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_149));
                    }
                    else
                    {
                        bAuth = new byte[] { 0x01, 0x01 };
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_150));
                    }

                    spi.ClientSocket.Send(bAuth);
                    spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行命令

        private void Command(Socket_ProxyInfo spi)
        {
            string sDomain = string.Empty;
            IPAddress ip = IPAddress.Any;
            ushort port = 0;

            try
            {
                Socket_Cache.SocketProxy.ProxyType ProxyType = (Socket_Cache.SocketProxy.ProxyType)spi.ClientData[0];
                Socket_Cache.SocketProxy.CommandType CommandType = (Socket_Cache.SocketProxy.CommandType)spi.ClientData[1];
                Socket_Cache.SocketProxy.AddressType AddressType = (Socket_Cache.SocketProxy.AddressType)spi.ClientData[3];

                byte[] bADDRESS = new byte[spi.ClientData.Length - 4];
                Buffer.BlockCopy(spi.ClientData, 4, bADDRESS, 0, bADDRESS.Length);

                switch (AddressType)
                {
                    case Socket_Cache.SocketProxy.AddressType.IPV4:

                        byte[] bIPV4 = new byte[4];
                        Buffer.BlockCopy(bADDRESS, 0, bIPV4, 0, bIPV4.Length);
                        ip = new IPAddress(bIPV4);

                        byte[] bIPV4_Port = new byte[2];
                        Buffer.BlockCopy(bADDRESS, 4, bIPV4_Port, 0, bIPV4_Port.Length);
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bIPV4_Port);                        

                        sDomain = ip.ToString();

                        break;

                    case Socket_Cache.SocketProxy.AddressType.Domain:

                        byte Length = bADDRESS[0];
                        byte[] bDomain = new byte[Length];
                        Buffer.BlockCopy(bADDRESS, 1, bDomain, 0, bDomain.Length);
                        sDomain = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, bDomain);

                        Socket_Cache.SocketProxy.AddressType atType = Socket_Operation.GetAddressType_ByString(sDomain);

                        switch (atType)
                        {
                            case Socket_Cache.SocketProxy.AddressType.IPV4:
                                ip = IPAddress.Parse(sDomain);
                                break;

                            case Socket_Cache.SocketProxy.AddressType.IPV6:
                                ip = IPAddress.Parse(sDomain);
                                break;

                            case Socket_Cache.SocketProxy.AddressType.Domain:
                                ip = Dns.GetHostEntry(sDomain).AddressList[0];
                                break;
                        }

                        byte[] bDomain_Port = new byte[2];
                        Buffer.BlockCopy(bADDRESS, 1 + Length, bDomain_Port, 0, bDomain_Port.Length);
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bDomain_Port);                      

                        break;

                    case Socket_Cache.SocketProxy.AddressType.IPV6:

                        byte[] bIPV6 = new byte[16];
                        Buffer.BlockCopy(bADDRESS, 0, bIPV6, 0, bIPV6.Length);
                        ip = new IPAddress(bIPV6);

                        byte[] bIPV6_Port = new byte[2];
                        Buffer.BlockCopy(bADDRESS, 16, bIPV6_Port, 0, bIPV6_Port.Length);
                        port = Socket_Operation.ByteArrayToInt16BigEndian(bIPV6_Port);

                        sDomain = ip.ToString();

                        break;
                }

                spi.IPAddress = ip;
                spi.Port = port;           

                byte[] serverPort = BitConverter.GetBytes(Socket_Cache.SocketProxy.Port);

                if (CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                {
                    IPEndPoint targetEP = new IPEndPoint(ip, port);
                    spi.TargetSocket = new Socket(targetEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        spi.TargetSocket.Connect(targetEP);
                        spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                        spi.ClientSocket.Send(new byte[] { ((byte)spi.ProxyType), 0x00, 0x00, (byte)Socket_Cache.SocketProxy.AddressType.IPV4, 0, 0, 0, 0, serverPort[1], serverPort[0] });

                        sDomain = string.Format("{0}: {1}", sDomain, port);
                        spi.ClientAddress = string.Format("{0} [{1}]", sDomain, ((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Port.ToString());
                        spi.TargetAddress = string.Format("socket://{0}", sDomain);

                        Socket_Cache.SocketProxyList.SocketProxyToList(spi);
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_151), spi.TargetAddress);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                    catch (SocketException ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }
                else
                {                    
                    spi.ClientSocket.Send(new byte[] { ((byte)spi.ProxyType), 0x07, 0x00, (byte)Socket_Cache.SocketProxy.AddressType.IPV4, 0, 0, 0, 0, serverPort[1], serverPort[0] });
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_152));
                }

                spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
            }
            catch (Exception ex)
            {                
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name,  sDomain + ": " + port +" - " + ex.Message);
            }
        }

        #endregion

        #region//请求数据

        private void ForwardData(Socket_ProxyInfo spi)
        {
            try
            {
                if (spi.TargetSocket != null)
                {
                    spi.TargetSocket.Send(spi.ClientData, SocketFlags.None);
                    Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, Socket_Cache.SocketProxy.DataType.Request);

                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_153), spi.ClientSocket.RemoteEndPoint.ToString(), spi.ClientData.Length);
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//响应数据

        private void ResponseCallback(IAsyncResult ar)
        {
            Socket_ProxyInfo spi = (Socket_ProxyInfo)ar.AsyncState;

            try
            {
                if (spi.TargetSocket != null)
                {
                    int bytesRead = spi.TargetSocket.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        spi.TargetData = new byte[bytesRead];
                        Buffer.BlockCopy(spi.TargetBuffer, 0, spi.TargetData, 0, bytesRead);

                        if (spi.ClientSocket != null)
                        {
                            spi.ClientSocket.Send(spi.TargetData, SocketFlags.None);
                            Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, Socket_Cache.SocketProxy.DataType.Response);

                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_154), spi.TargetSocket.RemoteEndPoint.ToString(), spi.TargetData.Length);
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                        }

                        spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                    }
                    else
                    {
                        this.UpdateTargetSocket_Closed(spi);

                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_144), spi.TargetAddress);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                this.UpdateTargetSocket_Closed(spi);                
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.TargetAddress + " " + ex.Message);
            }
        }

        #endregion

        #region//更新已关闭的客户端链接

        public void UpdateClientSocket_Closed(Socket_ProxyInfo spi)
        {
            try
            {  
                TreeNode ClientNode = Socket_Operation.FindNodeByName(this.tvClientList.Nodes, spi.ClientAddress);

                if (ClientNode != null) 
                {
                    if (!IsDisposed)
                    {
                        tvClientList.BeginInvoke(new MethodInvoker(delegate
                        {
                            ClientNode.ImageIndex = 6;
                            ClientNode.StateImageIndex = 6;
                        }));
                    }                    
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//更新已关闭的远端链接

        public void UpdateTargetSocket_Closed(Socket_ProxyInfo spi)
        {
            try
            {  
                TreeNode ClientNode = Socket_Operation.FindNodeByName(this.tvProxyData.Nodes, spi.TargetAddress);

                if (ClientNode != null)
                {
                    if (!IsDisposed)
                    {
                        tvProxyData.BeginInvoke(new MethodInvoker(delegate
                        {
                            ClientNode.ForeColor = Color.Maroon;
                        }));
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

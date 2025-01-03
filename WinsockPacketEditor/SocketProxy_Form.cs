﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;
using Be.Windows.Forms;
using System.Collections.Generic;

namespace WinsockPacketEditor
{
    public partial class SocketProxy_Form : Form
    {
        private static Socket SocketServer;
        private static UdpClient UDPListener;

        private static Dictionary<IPEndPoint, IPEndPoint> UDPMapping = new Dictionary<IPEndPoint, IPEndPoint>();
        private static Dictionary<IPEndPoint, string> UDPMapping_DNS = new Dictionary<IPEndPoint, string>();

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
                    tvProxyData.Invoke(new MethodInvoker(delegate
                    {                        
                        TreeNode RootNode = Socket_Operation.FindNode_ByName(this.tvProxyData.Nodes, spd.Domain);
                        if (RootNode == null)
                        {
                            RootNode = this.tvProxyData.Nodes.Add(spd.Domain);

                            int RootImgIndex = -1;
                            switch (spd.DomainType)
                            {
                                case Socket_Cache.SocketProxy.DomainType.Socket:
                                    RootImgIndex = 0;                              
                                    break;

                                case Socket_Cache.SocketProxy.DomainType.Http:
                                    RootImgIndex = 7;                                
                                    break;

                                case Socket_Cache.SocketProxy.DomainType.Https:
                                    RootImgIndex = 7;                                  
                                    break;
                            }

                            RootNode.ImageIndex = RootImgIndex;
                            RootNode.SelectedImageIndex = RootImgIndex;

                            TreeNode RequestNode = RootNode.Nodes.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_138));
                            RequestNode.ImageIndex = 2;
                            RequestNode.SelectedImageIndex = 2;

                            TreeNode ResponseNode = RootNode.Nodes.Add(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_139));
                            ResponseNode.ImageIndex = 3;
                            ResponseNode.SelectedImageIndex = 3;
                        }                        

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

                        Socket_Operation.UpdateNodeColor(RootNode);

                        string sDataNodeName = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_140), spd.Buffer.Length);
                        TreeNode DataNode = ChildNode.Nodes.Add(sDataNodeName);
                        DataNode.Tag = spd.Buffer;

                        int DataImgIndex = -1;
                        switch (spd.DomainType)
                        {
                            case Socket_Cache.SocketProxy.DomainType.Socket:                             
                                DataImgIndex = 1;
                                break;

                            case Socket_Cache.SocketProxy.DomainType.Http:                             
                                DataImgIndex = 8;
                                break;

                            case Socket_Cache.SocketProxy.DomainType.Https:                             
                                DataImgIndex = 8;
                                break;
                        }

                        DataNode.ImageIndex = DataImgIndex;
                        DataNode.SelectedImageIndex = DataImgIndex;                        
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
                    tvClientList.Invoke(new MethodInvoker(delegate
                    {
                        string sRootName = ((IPEndPoint)spi.ClientSocket.RemoteEndPoint).Address.ToString();
                        string sChildName = spi.ClientAddress;

                        TreeNode RootNode = Socket_Operation.FindNode_ByName(this.tvClientList.Nodes, sRootName);
                        if (RootNode == null)
                        {
                            RootNode = this.tvClientList.Nodes.Add(sRootName);                        
                        }

                        Socket_Operation.UpdateNodeColor(RootNode);

                        TreeNode ChildNode = Socket_Operation.FindNode_ByName(RootNode.Nodes, sChildName);
                        if (ChildNode == null)
                        {
                            ChildNode = RootNode.Nodes.Add(sChildName);                            
                        }                    

                        ChildNode.ImageIndex = 5;
                        ChildNode.SelectedImageIndex = 5;                        
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
                Socket_Cache.SocketProxyQueue.ResetProxyDataQueue();
                Socket_Cache.SocketProxyList.ResetProxyDataList();
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
            try
            {
                if (this.CheckProxySet())
                {
                    Socket_Cache.SocketProxy.IsListening = true;
                    Socket_Cache.SocketProxy.ProxyIP = Socket_Operation.GetLocalIPAddress()[0];

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

        private void StartListen()
        {
            try
            {
                if (SocketServer == null)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, Socket_Cache.SocketProxy.ProxyPort);
                    SocketServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    SocketServer.Bind(ep);
                    SocketServer.Listen(int.MaxValue);
                    AcceptClients();                    
                }

                if (UDPListener == null)
                {
                    UDPListener = new UdpClient(Socket_Cache.SocketProxy.ProxyPort);
                    UDPListener.BeginReceive(new AsyncCallback(AcceptUdpCallback), null);
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

        #region//接受客户端连接

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
                    ClientData = new byte[0],
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
                if (Socket_Cache.SocketProxy.IsListening && spi.ClientSocket != null)
                {
                    int bytesRead = spi.ClientSocket.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        byte[] bRead = new byte[bytesRead];
                        Buffer.BlockCopy(spi.ClientBuffer, 0, bRead, 0, bytesRead);

                        spi.ClientData = spi.ClientData.Concat(bRead).ToArray();

                        byte[] bData = new byte[spi.ClientData.Length];
                        Buffer.BlockCopy(spi.ClientData, 0, bData, 0, spi.ClientData.Length);

                        if (Socket_Operation.CheckDataIsMatchProxyStep(bData, spi.ProxyStep))
                        {
                            switch (spi.ProxyStep)
                            {
                                case Socket_Cache.SocketProxy.ProxyStep.Handshake:
                                    this.Handshake(spi, bData);
                                    break;

                                case Socket_Cache.SocketProxy.ProxyStep.AuthUserName:
                                    this.AuthUserName(spi, bData);
                                    break;

                                case Socket_Cache.SocketProxy.ProxyStep.Command:
                                    this.Command(spi, bData);
                                    break;

                                case Socket_Cache.SocketProxy.ProxyStep.ForwardData:
                                    this.ForwardData(spi, bData);
                                    break;
                            }

                            spi.ClientData = new byte[0];
                        }                        

                        spi.ClientSocket.BeginReceive(spi.ClientBuffer, 0, spi.ClientBuffer.Length, 0, new AsyncCallback(ReadCallback), spi);
                    }
                    else
                    {                        
                        this.UpdateClientSocket_Closed(spi);
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_144), spi.ClientSocket.RemoteEndPoint);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }                  
                }
            }
            catch (Exception ex)
            {
                this.UpdateClientSocket_Closed(spi);
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.ClientSocket.RemoteEndPoint + " - " + ex.Message);
            }
        }

        #endregion

        #region//握手过程

        private void Handshake(Socket_ProxyInfo spi, byte[] bData)
        {
            try
            {          
                spi.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];

                if (spi.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                {
                    bool bSupportAuthType = false;
                    Socket_Cache.SocketProxy.AuthType atServer = new Socket_Cache.SocketProxy.AuthType();
                    Socket_Cache.SocketProxy.AuthType atClient = new Socket_Cache.SocketProxy.AuthType();

                    if (Socket_Cache.SocketProxy.Enable_Auth)
                    {
                        atServer = Socket_Cache.SocketProxy.AuthType.UserName;
                    }
                    else
                    {
                        atServer = Socket_Cache.SocketProxy.AuthType.None;
                    }

                    int iMETHODS_COUNT = bData[1];
                    byte[] bMETHODS = new byte[iMETHODS_COUNT];                    

                    for (int i = 0; i < iMETHODS_COUNT; i++)
                    {
                        bMETHODS[i] = bData[2 + i];
                        atClient = (Socket_Cache.SocketProxy.AuthType)bMETHODS[i];

                        if (atServer == atClient)
                        {
                            bSupportAuthType = true;
                        }
                    }

                    if (bSupportAuthType)
                    {
                        byte[] bAuth = new byte[] { ((byte)Socket_Cache.SocketProxy.ProxyType.Socket5), ((byte)atServer) };
                        spi.ClientSocket.Send(bAuth);

                        if (atServer == Socket_Cache.SocketProxy.AuthType.UserName)
                        {
                            spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.AuthUserName;

                            if (bData.Length > iMETHODS_COUNT + 2)
                            {
                                byte[] bAuthDate = new byte[bData.Length - (iMETHODS_COUNT + 2)];
                                Buffer.BlockCopy(bData, iMETHODS_COUNT + 2, bAuthDate, 0, bAuthDate.Length);

                                if (Socket_Operation.CheckDataIsMatchProxyStep(bAuthDate, Socket_Cache.SocketProxy.ProxyStep.AuthUserName))
                                {
                                    this.AuthUserName(spi, bAuthDate);
                                }
                            }
                        }
                        else
                        {
                            spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.Command;
                        }
                    }
                    else
                    {
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_146), spi.ClientSocket.RemoteEndPoint, atServer);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
                else
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_145), spi.ProxyType);
                    Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//验证账号密码

        private void AuthUserName(Socket_ProxyInfo spi, byte[] bData)
        {
            try
            {
                byte VERSION = bData[0];

                if (VERSION == 0x01)
                { 
                    int USERNAME_LENGTH = bData[1];
                    byte[] USERNAME = new byte[USERNAME_LENGTH];
                    Buffer.BlockCopy(bData, 2, USERNAME, 0, USERNAME_LENGTH);

                    int PASSWORD_LENGTH = bData[2 + USERNAME_LENGTH];
                    byte[] PASSWORD = new byte[PASSWORD_LENGTH];
                    Buffer.BlockCopy(bData, 3 + USERNAME_LENGTH, PASSWORD, 0, PASSWORD_LENGTH);

                    string sUserName = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, USERNAME);
                    string sPassWord = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.UTF8, PASSWORD);                    

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
                    }
                    else
                    {
                        bAuth = new byte[] { 0x01, 0x01 };

                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_147), spi.ClientSocket.RemoteEndPoint);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
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

        private void Command(Socket_ProxyInfo spi, byte[] bData)
        {
            try
            {
                spi.ProxyType = (Socket_Cache.SocketProxy.ProxyType)bData[0];
                spi.CommandType = (Socket_Cache.SocketProxy.CommandType)bData[1];
                spi.AddressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                if (spi.ProxyType == Socket_Cache.SocketProxy.ProxyType.Socket5)
                {
                    byte[] bADDRESS = new byte[bData.Length - 4];
                    Buffer.BlockCopy(bData, 4, bADDRESS, 0, bADDRESS.Length);

                    IPEndPoint targetEP = Socket_Operation.GetIPEndPoint_ByAddressType(spi.AddressType, bADDRESS);
                    string sIPString = Socket_Operation.GetIP_ByAddressType(spi.AddressType, bADDRESS);
                    ushort uPort = ((ushort)targetEP.Port);

                    spi.DomainType = Socket_Operation.GetDomainType_ByPort(uPort);
                    spi.ClientAddress = Socket_Operation.GetClientAddress(sIPString, uPort, spi);
                    spi.TargetAddress = Socket_Operation.GetTargetAddress(sIPString, uPort, spi);

                    try
                    {
                        Socket_Cache.SocketProxy.CommandResponse commandResponse = new Socket_Cache.SocketProxy.CommandResponse();

                        if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                        {
                            try
                            {
                                spi.TargetSocket = new Socket(targetEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                spi.TargetSocket.Connect(targetEP);
                                spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);

                                commandResponse = Socket_Cache.SocketProxy.CommandResponse.Success;
                            }
                            catch (SocketException)
                            {
                                commandResponse = Socket_Cache.SocketProxy.CommandResponse.Fault;
                            }                            
                        }
                        else if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.UDP)
                        {
                            commandResponse = Socket_Cache.SocketProxy.CommandResponse.Success;
                        }
                        else
                        {
                            commandResponse = Socket_Cache.SocketProxy.CommandResponse.Unsupport;

                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_152), spi.ClientSocket.RemoteEndPoint, spi.CommandType);
                            Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                        }

                        byte[] bServerIP = Socket_Cache.SocketProxy.ProxyIP.GetAddressBytes();
                        byte[] bServerPort = BitConverter.GetBytes(Socket_Cache.SocketProxy.ProxyPort);

                        spi.ClientSocket.Send(new byte[] { ((byte)spi.ProxyType), (byte)commandResponse, 0x00, (byte)Socket_Cache.SocketProxy.AddressType.IPV4, bServerIP[0], bServerIP[1], bServerIP[2], bServerIP[3], bServerPort[1], bServerPort[0] });

                        if (commandResponse == Socket_Cache.SocketProxy.CommandResponse.Success)
                        {
                            spi.ProxyStep = Socket_Cache.SocketProxy.ProxyStep.ForwardData;
                            Socket_Cache.SocketProxyList.SocketProxyToList(spi);
                        }                        
                    }
                    catch (SocketException ex)
                    {
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.TargetAddress + " - " + ex.Message);
                    }                                      
                }
            }
            catch (Exception ex)
            {                
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//请求数据（TCP）       

        private void ForwardData(Socket_ProxyInfo spi, byte[] bData)
        {
            try
            {
                if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                {
                    spi.TargetSocket.Send(bData, SocketFlags.None);
                }               

                Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, bData, Socket_Cache.SocketProxy.DataType.Request);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.ClientAddress + " - " + ex.Message);
            }
        }

        #endregion

        #region//响应数据（TCP）

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
                            if (spi.CommandType == Socket_Cache.SocketProxy.CommandType.Connect)
                            {
                                spi.ClientSocket.Send(spi.TargetData, SocketFlags.None);                                
                            }

                            Socket_Cache.SocketProxyQueue.ProxyDataToQueue(spi, spi.TargetData, Socket_Cache.SocketProxy.DataType.Response);
                        }

                        spi.TargetSocket.BeginReceive(spi.TargetBuffer, 0, spi.TargetBuffer.Length, SocketFlags.None, new AsyncCallback(ResponseCallback), spi);
                    }
                    else
                    {
                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_144), spi.TargetAddress);
                        Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, spi.TargetAddress + " - " + ex.Message);
            }
        }

        #endregion

        #region//数据转发（UDP）

        private void AcceptUdpCallback(IAsyncResult ar)
        {
            try
            {
                string sDNS_Head = string.Empty;
                ushort TargetPort = 0;
                IPEndPoint remoteEndPoint = null;
                byte[] bData = UDPListener.EndReceive(ar, ref remoteEndPoint);

                if (bData.Length > 0)
                {
                    IPEndPoint targetResponseEndPoint = null;
                    TargetPort = ((ushort)remoteEndPoint.Port);

                    if (TargetPort == 53)
                    {
                        sDNS_Head = Convert.ToInt32(bData[0]).ToString() + "." + Convert.ToInt32(bData[1]).ToString();
                        targetResponseEndPoint = UDPMapping_DNS.FirstOrDefault(kvp => kvp.Value.Equals(sDNS_Head)).Key;
                        UDPMapping_DNS.Remove(targetResponseEndPoint);
                    }
                    else
                    {
                        targetResponseEndPoint = UDPMapping.FirstOrDefault(kvp => kvp.Value.Equals(remoteEndPoint)).Key;
                    }

                    if (targetResponseEndPoint != null)
                    {
                        byte[] bIP = targetResponseEndPoint.Address.GetAddressBytes();
                        byte[] bPort = BitConverter.GetBytes(targetResponseEndPoint.Port);
                        byte[] bResponseData = new byte[] { 0x00, 0x00, 0x00, (byte)Socket_Cache.SocketProxy.AddressType.IPV4, bIP[0], bIP[1], bIP[2], bIP[3], bPort[1], bPort[0] };
                        bResponseData = bResponseData.Concat(bData).ToArray();

                        UDPListener.Send(bResponseData, bResponseData.Length, targetResponseEndPoint);
                    }
                    else
                    {
                        if (bData[0].Equals(0) && bData[1].Equals(0) && bData[2].Equals(0))
                        {
                            Socket_Cache.SocketProxy.AddressType addressType = (Socket_Cache.SocketProxy.AddressType)bData[3];

                            if (addressType == Socket_Cache.SocketProxy.AddressType.IPV4 || addressType == Socket_Cache.SocketProxy.AddressType.IPV6 || addressType == Socket_Cache.SocketProxy.AddressType.Domain)
                            {
                                byte[] bADDRESS = new byte[bData.Length - 4];
                                Buffer.BlockCopy(bData, 4, bADDRESS, 0, bADDRESS.Length);
                                IPEndPoint targetEndPoint = Socket_Operation.GetIPEndPoint_ByAddressType(addressType, bADDRESS);
                                
                                TargetPort = ((ushort)targetEndPoint.Port);
                                byte[] bUDP_Data = Socket_Operation.GetUDPData_ByAddressType(addressType, bData);

                                if (bUDP_Data != null)
                                {
                                    if (TargetPort == 53)
                                    {
                                        sDNS_Head = Convert.ToInt32(bUDP_Data[0]).ToString() + "." + Convert.ToInt32(bUDP_Data[1]).ToString();
                                        if (!UDPMapping_DNS.ContainsKey(remoteEndPoint))
                                        {
                                            UDPMapping_DNS.Add(remoteEndPoint, sDNS_Head);
                                        }
                                    }
                                    else
                                    {
                                        if (!UDPMapping.ContainsKey(remoteEndPoint))
                                        {
                                            UDPMapping.Add(remoteEndPoint, targetEndPoint);
                                        }
                                    }

                                    UDPListener.Send(bUDP_Data, bUDP_Data.Length, targetEndPoint);
                                }
                            }
                        }
                    }

                    UDPListener.BeginReceive(new AsyncCallback(AcceptUdpCallback), null);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//更新已关闭的客户端链接

        public void UpdateClientSocket_Closed(Socket_ProxyInfo spi)
        {
            try
            {  
                TreeNode ClientNode = Socket_Operation.FindNode_ByName(this.tvClientList.Nodes, spi.ClientAddress);

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

    }
}

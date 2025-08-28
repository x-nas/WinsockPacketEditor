using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProxyModeForm : Window, InterfaceInfo.IProxyMode
    {        
        private bool setcolor = false;
        private bool SearchFromHead = true;
        private static Socket ProxyServer;        
        private AntdUI.FormFloatButton FloatButton = null;
        private FilterList cFilterList = null;
        private SendList cSendList = null;
        private RobotList cRobotList = null;
        private LogList cLogList = null;
        private StatisticalData cStatisticalData = null;
        private ComparisonText cComparisonText = null;
        private XORCalculation cXORCalculation = null;
        private Transcoding cTranscoding = null;
        private ExtractionData cExtractionData = null;
        private AccountList cAccountList = null;
        private ClientList cClientList = null;

        #region//窗体事件

        public ProxyModeForm()
        {
            InitializeComponent();            
        }

        private void ProxyModeForm_Load(object sender, EventArgs e)
        {
            this.pageHeader.Loading = true;
            AntdUI.Spin.open(this, AntdUI.Localization.Get("Loading", "正在加载..."), config =>
            {
                Operate.SystemConfig.StartRemoteMGT();
                Operate.SystemConfig.InitCPUAndMemoryCounter();
                Operate.SystemConfig.LoadInjectMode_FromDB();
                Operate.SystemConfig.LoadProxyMode_FromDB();
                Operate.SystemConfig.LoadSystemList_FromDB();
                Operate.ProxyConfig.Account.LoadProxyAccountList_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapLocal_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapRemote_FromDB();
                
                this.InitProxyServerIP();
                this.InitGlobal();
                this.InitFloatButton();
                this.InitTable_ProxyList();

            }, () =>
            {
                this.pageHeader.Loading = false;                
            });

            Operate.SystemConfig.MainHandle = this.Handle;

            this.Dark_Changed();
            this.InitForm();
            this.InitControls();
            this.InitHotKeys();

            this.hbProxyData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.tabProxyMode.TabMenuVisible = false;            
            this.mProxyMode.SelectIndex(0, true);
        }

        private void ProxyModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Operate.ProxyConfig.Proxy.Enable_SystemProxy)
            {
                Operate.ProxyConfig.Proxy.Enable_SystemProxy = false;
                Operate.ProxyConfig.Proxy.DisableSystemProxy(this);
            }

            Operate.SystemConfig.StopRemoteMGT();
            Operate.SystemConfig.SaveSystemConfig_ToDB();
            Operate.SystemConfig.SaveInjectMode_ToDB();
            Operate.SystemConfig.SaveProxyMode_ToDB();
            Operate.SystemConfig.SaveSystemList_ToDB();
            Operate.ProxyConfig.Account.SaveAccountList_ToDB();
            Operate.ProxyConfig.Mapping.SaveMapLocal_ToDB();
            Operate.ProxyConfig.Mapping.SaveMapRemote_ToDB();            
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            try
            {
                if (m.Msg == User32.WM_HOTKEY)
                {
                    int HOTKEY_ID = m.WParam.ToInt32();

                    if (this.tabProxyMode.SelectedIndex == 3)
                    {
                        Operate.SendConfig.Send.DoSend_ByHotKey(HOTKEY_ID);
                    }
                    else if (this.tabProxyMode.SelectedIndex == 4)
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

        private void InitControls()
        {
            cAccountList = new AccountList(this);
            cAccountList.Dock = DockStyle.Fill;
            this.tpAccountList.Controls.Add(cAccountList);

            cClientList = new ClientList();
            cClientList.Dock = DockStyle.Fill;
            this.tpClientList.Controls.Add(cClientList);

            cFilterList = new FilterList(this);
            cFilterList.Dock = DockStyle.Fill;
            this.tpFilterList.Controls.Add(cFilterList);

            cSendList = new SendList(this);
            cSendList.Dock = DockStyle.Fill;
            this.tpSendList.Controls.Add(cSendList);

            cRobotList = new RobotList(this);
            cRobotList.Dock = DockStyle.Fill;
            this.tpRobotList.Controls.Add(cRobotList);

            cLogList = new LogList(this);
            cLogList.Dock = DockStyle.Fill;
            this.tpSystemLog.Controls.Add(cLogList);

            cStatisticalData = new StatisticalData();
            cStatisticalData.Dock = DockStyle.Fill;
            this.tpStatistical.Controls.Add(cStatisticalData);

            cComparisonText = new ComparisonText();
            cComparisonText.Dock = DockStyle.Fill;
            this.tpComparison.Controls.Add(cComparisonText);

            cXORCalculation = new XORCalculation(this);
            cXORCalculation.Dock = DockStyle.Fill;
            this.tpXOR.Controls.Add(cXORCalculation);

            cTranscoding = new Transcoding();
            cTranscoding.Dock = DockStyle.Fill;
            this.tpTranscoding.Controls.Add(cTranscoding);

            cExtractionData = new ExtractionData(this);
            cExtractionData.Dock = DockStyle.Fill;
            this.tpExtraction.Controls.Add(cExtractionData);
        }

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;

            this.mProxyMode.Collapsed = false;
            this.MenuCollapseChange();            

            for (int i = 0; i < this.mProxyMode.Items.Count; i++)
            {
                this.mProxyMode.Items[i].BadgeBack = this.colorTheme.Value;
            }            
        }

        private void InitGlobal()
        {
            var globals = new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("中文","zh-CN"),
                new AntdUI.SelectItem("English","en-US")
            };

            btn_global.Items.AddRange(globals);

            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                btn_global.SelectedValue = globals[1].Tag;
            }
            else
            {
                btn_global.SelectedValue = globals[0].Tag;
            }
        }

        public void InitFloatButton()
        {
            Operate.SystemConfig.InitFloatButton(this, this.FloatButton);
        }

        private void InitProxyServerIP()
        {
            if (Operate.ProxyConfig.Proxy.ProxyServerIP == null)
            {
                Operate.ProxyConfig.Proxy.ProxyServerIP = Operate.SystemConfig.GetLocalIPAddress();
            }
        }

        private void InitHotKeys()
        {
            Operate.SystemConfig.RegisterHotkey_FromText(9001, Operate.SystemConfig.HotKey1);
            Operate.SystemConfig.RegisterHotkey_FromText(9002, Operate.SystemConfig.HotKey2);
            Operate.SystemConfig.RegisterHotkey_FromText(9003, Operate.SystemConfig.HotKey3);
            Operate.SystemConfig.RegisterHotkey_FromText(9004, Operate.SystemConfig.HotKey4);
            Operate.SystemConfig.RegisterHotkey_FromText(9005, Operate.SystemConfig.HotKey5);
            Operate.SystemConfig.RegisterHotkey_FromText(9006, Operate.SystemConfig.HotKey6);
            Operate.SystemConfig.RegisterHotkey_FromText(9007, Operate.SystemConfig.HotKey7);
            Operate.SystemConfig.RegisterHotkey_FromText(9008, Operate.SystemConfig.HotKey8);
            Operate.SystemConfig.RegisterHotkey_FromText(9009, Operate.SystemConfig.HotKey9);
            Operate.SystemConfig.RegisterHotkey_FromText(9010, Operate.SystemConfig.HotKey10);
            Operate.SystemConfig.RegisterHotkey_FromText(9011, Operate.SystemConfig.HotKey11);
            Operate.SystemConfig.RegisterHotkey_FromText(9012, Operate.SystemConfig.HotKey12);
        }

        public void RefreshFilterList()
        {
            this.cFilterList?.RefreshFilterList();
        }

        public void RefreshProxyData()
        {
            if (Operate.ProxyConfig.List.piSelect != null)
            {
                DynamicByteProvider dbp = new DynamicByteProvider(Operate.ProxyConfig.List.piSelect.PacketBuffer);
                hbProxyData.ByteProvider = dbp;
            }
        }

        public void RefreshAccountList()
        {
            this.cAccountList?.RefreshAccountList();
        }

        public void RefreshSendList()
        {
            this.cSendList?.RefreshSendList();
        }

        public void RefreshRobotList()
        {
            this.cRobotList?.RefreshRobotList();
        }

        #endregion

        #region//初始化表格

        private void InitTable_ProxyList()
        {
            tProxyList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column(string.Empty, string.Empty, AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new AntdUI.CellImage(Operate.PacketConfig.Packet.GetImg_ByPacketType(pi.PacketType));
                        }

                        return value;
                    },
                }.SetFixed(),
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.ProxyList.Column.ID"),
                new AntdUI.Column("ProxyTime", "时间戳", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss:fffffff");
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),                
                new AntdUI.Column("PacketType", "类别", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)value);
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketSocket", "套接字", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ClientAddr", "客户端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(pi.ClientLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ClientLocation", "所属地").SetWidth("100").SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ServerDomain", "服务端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(pi.ServerLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ServerLocation", "所属地").SetWidth("100").SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketData", "数据").SetLocalizationTitleID("Table.ProxyList.Column."),
            };

            this.tProxyList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tProxyList.DataSource = Operate.ProxyConfig.List.lstProxyInfo;
        }

        public void SetColumnVisible_ProxyList()
        {
            try
            {
                this.tProxyList.Columns[1].Visible = Operate.ProxyConfig.List.IsShow_ID;
                this.tProxyList.Columns[2].Visible = Operate.ProxyConfig.List.IsShow_ProxyTime;
                this.tProxyList.Columns[3].Visible = Operate.ProxyConfig.List.IsShow_PacketType;
                this.tProxyList.Columns[4].Visible = Operate.ProxyConfig.List.IsShow_PacketSocket;
                this.tProxyList.Columns[5].Visible = Operate.ProxyConfig.List.IsShow_ClientAddr;
                this.tProxyList.Columns[6].Visible = Operate.ProxyConfig.List.IsShow_ClientLocation;
                this.tProxyList.Columns[7].Visible = Operate.ProxyConfig.List.IsShow_ServerAddr;
                this.tProxyList.Columns[8].Visible = Operate.ProxyConfig.List.IsShow_ServerLocation;
                this.tProxyList.Columns[9].Visible = Operate.ProxyConfig.List.IsShow_PacketLen;
                this.tProxyList.Columns[10].Visible = Operate.ProxyConfig.List.IsShow_PacketData;

            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private Table.CellStyleInfo tProxyList_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            try
            {
                int index = e.RowIndex - 1;
                if (index > -1 && index < Operate.ProxyConfig.List.lstProxyInfo.Count)
                {
                    ProxyInfo pi = Operate.ProxyConfig.List.lstProxyInfo[index];
                    if (pi != null)
                    {
                        switch (pi.FilterAction)
                        {
                            case Operate.FilterConfig.Filter.FilterAction.Replace:

                                return new AntdUI.Table.CellStyleInfo
                                {
                                    ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Replace,
                                    BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Replace,
                                };

                            case Operate.FilterConfig.Filter.FilterAction.Intercept:

                                return new AntdUI.Table.CellStyleInfo
                                {
                                    ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Intercept,
                                    BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Intercept,
                                };

                            case Operate.FilterConfig.Filter.FilterAction.Change:

                                return new AntdUI.Table.CellStyleInfo
                                {
                                    ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Change,
                                    BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Change,
                                };

                            default:
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return null;
        }        

        #endregion

        #region//更换主题颜色

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            setcolor = true;
            AntdUI.Style.SetPrimary(e.Value);

            for (int i = 0; i < this.mProxyMode.Items.Count; i++)
            {
                this.mProxyMode.Items[i].BadgeBack = e.Value;
            }

            Refresh();
        }

        #endregion

        #region//更换主题模式

        private void btn_mode_Click(object sender, EventArgs e)
        {
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;

            this.Dark_Changed();
            OnSizeChanged(e);
        }

        private void Dark_Changed()
        {
            if (setcolor)
            {
                var color = AntdUI.Style.Db.Primary;
                AntdUI.Style.SetPrimary(color);
            }

            Dark = AntdUI.Config.IsDark;
            btn_mode.Toggle = Dark;

            if (Dark)
            {
                BackColor = Operate.SystemConfig.Color_30;
                ForeColor = Color.White;

                this.tabProxyMode.BackColor = Operate.SystemConfig.Color_35;

                this.tProxyList.BackColor = Operate.SystemConfig.Color_40;
                this.tProxyList.ColumnBack = Operate.SystemConfig.Color_40;
                this.tProxyList.ColumnFore = Color.Silver;
                this.tProxyList.ForeColor = Color.Lime;

                this.pPacketData.Back = Operate.SystemConfig.Color_40;
                this.hbProxyData.BackColor = Operate.SystemConfig.Color_40;
                this.hbProxyData.ForeColor = Color.Silver;
            }
            else
            {
                BackColor = Operate.SystemConfig.Color_250;
                ForeColor = Color.Black;

                this.tabProxyMode.BackColor = Color.White;

                this.tProxyList.BackColor = Color.White;
                this.tProxyList.ColumnBack = Color.White;
                this.tProxyList.ColumnFore = Color.Black;
                this.tProxyList.ForeColor = Color.Green;

                this.pPacketData.Back = Color.White;
                this.hbProxyData.BackColor = Color.White;
                this.hbProxyData.ForeColor = Color.Black;
            }

            this.cAccountList?.Dark_Changed();
            this.cClientList?.Dark_Changed();
            this.cFilterList?.Dark_Changed();
            this.cSendList?.Dark_Changed();
            this.cRobotList?.Dark_Changed();
            this.cStatisticalData?.Dark_Changed();
            this.cComparisonText?.Dark_Changed();
            this.cXORCalculation?.Dark_Changed();
            this.cTranscoding?.Dark_Changed();
            this.cExtractionData?.Dark_Changed();
            this.cLogList?.Dark_Changed();
        }

        #endregion

        #region//切换语言

        private void btn_global_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is string lang)
            {
                btn_global.Loading = true;

                if (lang.StartsWith("en"))
                {
                    AntdUI.Localization.Provider = new Localizer();
                }
                else
                {
                    AntdUI.Localization.Provider = null;
                }

                AntdUI.Localization.SetLanguage(lang);
                this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");
                this.cComparisonText.SetTextInfo();
                this.cExtractionData.SetExtractionInfo();

                Refresh();
                btn_global.Loading = false;
            }
        }

        #endregion

        #region//系统设置

        private void btn_setting_Click(object sender, EventArgs e)
        {
            var setting = new SystemSetting();
            if (AntdUI.Modal.open(this, AntdUI.Localization.Get("Setting", "设置"), setting) == DialogResult.OK)
            {
                AntdUI.Config.Animation = setting.Animation;
                AntdUI.Config.ShadowEnabled = setting.ShadowEnabled;
                AntdUI.Config.ShowInWindow = setting.ShowInWindow;
                AntdUI.Config.ScrollBarHide = setting.ScrollBarHide;
                AntdUI.Config.TextRenderingHighQuality = setting.TextRenderingHighQuality;
                if (AntdUI.Config.TextRenderingHighQuality == setting.TextRenderingHighQuality)
                {
                    return;
                }

                Refresh();
            }
        }

        #endregion

        #region//清空数据

        private void CleanUp_ProxyListInfo()
        {
            Operate.ProxyConfig.Proxy.ProxyTotal_CNT = 0;
            Operate.ProxyConfig.Proxy.TCP_Req_CNT = 0;
            Operate.ProxyConfig.Proxy.TCP_Resp_CNT = 0;
            Operate.ProxyConfig.Proxy.UDP_Req_CNT = 0;
            Operate.ProxyConfig.Proxy.UDP_Resp_CNT = 0;
            Operate.FilterConfig.Filter.FilterExecute_CNT = 0;
            Operate.FilterConfig.Filter.FilterReplace_CNT = 0;
            Operate.FilterConfig.Filter.FilterChange_CNT = 0;
            Operate.FilterConfig.Filter.FilterIntercept_CNT = 0;
            Operate.FilterConfig.Filter.FilterDisplay_CNT = 0;
            Operate.FilterConfig.Filter.FilterNoDisplay_CNT = 0;
            Operate.ProxyConfig.Proxy.FilterProxy_CNT = 0;
            Operate.ProxyConfig.Proxy.Total_Request = 0;
            Operate.ProxyConfig.Proxy.Total_Response = 0;
        }

        private void CleanUp_ProxyList()
        {
            try
            {
                Operate.ProxyConfig.Queue.ResetProxyInfoQueue();
                Operate.ProxyConfig.List.lstProxyInfo.Clear();                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_HexBox()
        {
            if (hbProxyData.InvokeRequired)
            {
                hbProxyData.Invoke(new Action(CleanUp_HexBox));
                return;
            }

            if (hbProxyData.ByteProvider != null)
            {
                IDisposable byteProvider = hbProxyData.ByteProvider as IDisposable;

                if (byteProvider != null)
                {
                    byteProvider.Dispose();
                }

                hbProxyData.ByteProvider = null;
            }
        }        

        #endregion

        #region//计时器

        private void timerProxyList_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Operate.ProxyConfig.Queue.qProxyTCP.Count > 0)
                {
                    Operate.ProxyConfig.List.ProxyTCP_ToList();                    
                }

                if (Operate.ProxyConfig.Queue.qProxyInfo.Count > 0)
                {
                    Operate.ProxyConfig.List.ProxyInfo_ToList();
                }

                if (Operate.LogConfig.Queue.cqLogInfo.Count > 0)
                {
                    Operate.LogConfig.List.LogToList();
                }

                if (Operate.LogConfig.Queue.cqProxyLogInfo.Count > 0)
                {
                    Operate.LogConfig.List.ProxyLogToList();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void timerProxyListInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.bgwProxyList.IsBusy)
                {
                    this.bgwProxyList.RunWorkerAsync();
                }

                this.cClientList?.ShowClientList();
                this.cAccountList?.SaveAccountList();

                this.mProxyMode.Items[0].Badge = Operate.ProxyConfig.List.lstProxyInfo.Count.ToString();
                this.mProxyMode.Items[1].Badge = this.cClientList.GetClientNumber().ToString();
                this.mProxyMode.Items[2].Badge = this.cAccountList.lstAccount.Count.ToString();
                this.mProxyMode.Items[3].Badge = Operate.FilterConfig.List.lstFilterInfo.Count.ToString();
                this.mProxyMode.Items[4].Badge = Operate.SendConfig.List.lstSendInfo.Count.ToString();
                this.mProxyMode.Items[5].Badge = Operate.RobotConfig.List.lstRobotInfo.Count.ToString();
                this.mProxyMode.Items[11].Badge = Operate.LogConfig.List.lstLogInfo.Count.ToString();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//显示选中的封包数据

        private void tProxyList_SelectIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = tProxyList.SelectedIndex - 1;
                if (selectedIndex >= 0 && selectedIndex < Operate.ProxyConfig.List.lstProxyInfo.Count)
                {
                    Operate.ProxyConfig.List.Search_Index = selectedIndex;
                    Operate.ProxyConfig.List.piSelect = Operate.ProxyConfig.List.lstProxyInfo[selectedIndex];

                    DynamicByteProvider dbp = new DynamicByteProvider(Operate.ProxyConfig.List.piSelect.PacketBuffer);
                    hbProxyData.ByteProvider = dbp;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理列表（异步）

        private void bgwProxyList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Operate.ProxyConfig.List.AutoRoll)
                {
                    tProxyList.ScrollBar.ValueY = tProxyList.ScrollBar.MaxY;
                }

                if (Operate.ProxyConfig.List.AutoClear)
                {
                    if (Operate.ProxyConfig.List.lstProxyInfo.Count > Operate.ProxyConfig.List.AutoClear_Value)
                    {
                        this.CleanUp_ProxyList();
                        this.CleanUp_HexBox();
                    }
                }

                if (Operate.LogConfig.List.AutoRoll)
                {
                    this.cLogList.ScrollToBottom();
                }

                if (Operate.LogConfig.List.AutoClear)
                {
                    if (Operate.LogConfig.List.lstLogInfo.Count > Operate.LogConfig.List.AutoClear_Value)
                    {
                        this.cLogList.CleanUp_LogList();
                    }

                    if (Operate.LogConfig.List.lstProxyLogInfo.Count > Operate.LogConfig.List.AutoClear_Value)
                    {
                        this.cClientList.CleanUp_ProxyLogList();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwProxyList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tProxyList.Refresh();
                this.cClientList.RefreshClientList();
                this.cAccountList.RefreshAccountList();
                this.cLogList.RefreshLogList();

                long ProxyTotal_CNT =
                    Operate.ProxyConfig.Proxy.TCP_Req_CNT +
                    Operate.ProxyConfig.Proxy.TCP_Resp_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Req_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Resp_CNT;

                this.lProxyTotal_CNT.Text = ProxyTotal_CNT.ToString();
                this.lTCP_Req_CNT.Text = Operate.ProxyConfig.Proxy.TCP_Req_CNT.ToString();
                this.lTCP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.TCP_Resp_CNT.ToString();
                this.lUDP_Req_CNT.Text = Operate.ProxyConfig.Proxy.UDP_Req_CNT.ToString();
                this.lUDP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.UDP_Resp_CNT.ToString();
                this.lFilterExecute_CNT.Text = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
                this.lProxyQueue_CNT.Text = Operate.ProxyConfig.Queue.qProxyInfo.Count.ToString();
                this.lFilterProxy_CNT.Text = Operate.ProxyConfig.Proxy.FilterProxy_CNT.ToString();
                this.lProxyTCP_CNT.Text = Operate.ProxyConfig.List.lstProxyTCP.Count.ToString();
                this.lProxyUDP_CNT.Text = Operate.ProxyConfig.List.cdProxyUDP.Count.ToString();                

                Operate.ProxyConfig.Proxy.ProxyOnLineInfo = string.Format(
                        "{0}/{1}",
                        Operate.ProxyConfig.Account.GetOnLineProxyAccountCount(Operate.ProxyConfig.Account.lstAccountInfo),
                        Operate.ProxyConfig.Account.lstAccountInfo.Count);
                this.lProxyAccount_CNT.Text = Operate.ProxyConfig.Proxy.ProxyOnLineInfo;

                Operate.ProxyConfig.Proxy.ProxyBytesInfo = string.Format(
                    AntdUI.Localization.Get("ProxyModeForm.ProxyBytesInfo", "请求: {0}  响应: {1}"),
                    Operate.SystemConfig.GetDisplayBytes(Operate.ProxyConfig.Proxy.Total_Request),
                    Operate.SystemConfig.GetDisplayBytes(Operate.ProxyConfig.Proxy.Total_Response));
                this.lTotalBytes.Text = Operate.ProxyConfig.Proxy.ProxyBytesInfo;

                decimal dUplink = Operate.ProxyConfig.Proxy.ProxySpeed_Uplink / 1024;
                Operate.ProxyConfig.Proxy.ProxySpeed_Uplink = 0;
                decimal dDownlink = Operate.ProxyConfig.Proxy.ProxySpeed_Downlink / 1024;
                Operate.ProxyConfig.Proxy.ProxySpeed_Downlink = 0;

                Operate.ProxyConfig.Proxy.ProxySpeedInfo = string.Format(
                    AntdUI.Localization.Get("ProxyModeForm.ProxySpeedInfo", "上行: {0} KB/s  下行: {1} KB/s"),
                    dUplink.ToString("0.00"),
                    dDownlink.ToString("0.00"));
                this.lProxySpeed.Text = Operate.ProxyConfig.Proxy.ProxySpeedInfo;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion        

        #region//主菜单

        private void bMenuCollapse_Click(object sender, EventArgs e)
        {
            this.mProxyMode.Collapsed = !this.mProxyMode.Collapsed;
            this.MenuCollapseChange();
        }

        private void MenuCollapseChange()
        {
            if (this.mProxyMode.Collapsed)
            {
                this.mProxyMode.Width = this.tlpMenu.Width = this.mProxyMode.CollapseWidth;
            }
            else
            {
                this.mProxyMode.Width = this.tlpMenu.Width = this.mProxyMode.CollapsedWidth;
            }
        }

        private void mProxyMode_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;

            switch (miSelect.ID)
            {
                case "miProxyList":
                    this.tabProxyMode.SelectTab("tpProxyList");
                    break;

                case "miClientList":
                    this.tabProxyMode.SelectTab("tpClientList");
                    break;

                case "miAccountList":
                    this.tabProxyMode.SelectTab("tpAccountList");
                    break;

                case "miFilterList":
                    this.tabProxyMode.SelectTab("tpFilterList");
                    break;

                case "miSendList":
                    this.tabProxyMode.SelectTab("tpSendList");
                    break;

                case "miRobotList":
                    this.tabProxyMode.SelectTab("tpRobotList");
                    break;

                case "miStatistical":
                    this.tabProxyMode.SelectTab("tpStatistical");                    
                    break;

                case "miComparison":
                    this.tabProxyMode.SelectTab("tpComparison");
                    break;

                case "miXOR":
                    this.tabProxyMode.SelectTab("tpXOR");
                    break;

                case "miTranscoding":
                    this.tabProxyMode.SelectTab("tpTranscoding");
                    break;

                case "miExtraction":
                    this.tabProxyMode.SelectTab("tpExtraction");
                    break;

                case "miSystemLog":
                    this.tabProxyMode.SelectTab("tpSystemLog");
                    break;
            }
        }

        #endregion

        #region//代理列表 - 菜单

        private void bProxyStart_Click(object sender, EventArgs e)
        {
            this.bProxyStart.Enabled = false;
            this.bProxyStop.Enabled = true;

            this.Start_Proxy();
        }

        private void bProxyStop_Click(object sender, EventArgs e)
        {
            this.bProxyStart.Enabled = true;
            this.bProxyStop.Enabled = false;

            this.Stop_Proxy();
        }

        private void bProxyList_Clear_Click(object sender, EventArgs e)
        {            
            this.CleanUp_ProxyList();
            this.CleanUp_ProxyListInfo();
            this.CleanUp_HexBox();
            this.cLogList.CleanUp_LogList();

            AntdUI.Message.open(new AntdUI.Message.Config(this, "已清空数据", TType.Warn)
            {
                LocalizationText = "InjectModeForm.Clear"
            });
        }

        private void mProxyList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mProxyList.USelect();

            switch (miSelect.ID)
            {
                case "miProxyListSearch":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SearchPacketForm(this)
                    {
                        Size = new Size(1000, 100),
                    })
                    {
                        Align = AntdUI.TAlignMini.Top,
                        Mask = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ProxySettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miFilterSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new FilterSettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miHookSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HookSettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miListSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ListSettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miMapSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new MapSettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miExternalProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new EXTProxySettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miHotKeySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HotKeyForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miBackUpSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new BackUpSettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miSystemSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SystemSettingsForm(this))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;
            }
        }

        #endregion

        #region//代理列表 - 右键菜单

        private void tProxyList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.List.lstProxyInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tProxyList, item =>
                {
                    List<ProxyInfo> piList = new List<ProxyInfo>();

                    foreach (int SelectIndex in this.tProxyList.SelectedIndexs)
                    {
                        piList.Add(Operate.ProxyConfig.List.lstProxyInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Edit":

                            if (piList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketEditForm(this, null, piList[0]))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (piList.Count > 0)
                            {
                                bool bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(piList[0], null);
                                if (bOK)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到滤镜列表成功", TType.Success)
                                    {
                                        LocalizationText = "ToFilterList.Success"
                                    });
                                }
                                else
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到滤镜列表失败", TType.Error)
                                    {
                                        LocalizationText = "ToFilterList.Error"
                                    });
                                }
                            }

                            break;

                        case "SYSSocket":

                            if (piList.Count > 0)
                            {
                                Operate.SystemConfig.SystemSocket = piList[0].PacketSocket;

                                AntdUI.Message.open(new AntdUI.Message.Config(this, "设置系统套接字完成", TType.Success)
                                {
                                    LocalizationText = "SSocket.Success"
                                });
                            }

                            break;

                        case "PacketModification":

                            if (piList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketModificationForm(this, piList[0]))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.ProxyConfig.List.SaveProxyList_Dialog(this, this.tProxyList, Operate.PacketConfig.Packet.InjectProcess, piList);

                            break;

                        case "ToTextA":

                            if (piList.Count > 0)
                            {
                                this.cComparisonText.SetTextA(Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, piList[0].PacketBuffer));
                          
                                AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本A", TType.Success)
                                {
                                    LocalizationText = "ToTextA"
                                });
                            }

                            break;

                        case "ToTextB":

                            if (piList.Count > 0)
                            {
                                this.cComparisonText.SetTextB(Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, piList[0].PacketBuffer));

                                AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本B", TType.Success)
                                {
                                    LocalizationText = "ToTextB"
                                });
                            }

                            break;

                        case "DeSelect":

                            this.tProxyList.SelectedIndex = -1;

                            break;

                        default:

                            if (piList.Count > 0)
                            {
                                if (Guid.TryParse(item.ID, out Guid SID))
                                {
                                    SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                    if (si != null && piList.Count > 0)
                                    {
                                        if (Operate.SendConfig.Send.AddSendCollection_ByProxyInfo(SID, piList))
                                        {
                                            string sText = string.Format(AntdUI.Localization.Get("ToSendList.Success", "已添加到: {0}"), item.Text);
                                            AntdUI.Message.open(new AntdUI.Message.Config(this, sText, TType.Success));
                                        }
                                        else
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到发送列表出错", TType.Error)
                                            {
                                                LocalizationText = "ToSendList.Error"
                                            });
                                        }
                                    }
                                }
                            }

                            break;
                    }
                }, Operate.PacketConfig.List.GetCMS_PacketList());
            }
        }

        #endregion

        #region//代理数据 - 右键菜单

        private void hbProxyData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DynamicByteProvider dbp = hbProxyData.ByteProvider as DynamicByteProvider;
                if (dbp == null || dbp.Bytes.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbProxyData, (item) =>
                {
                    switch (item.ID)
                    {
                        case "Edit":

                            if (Operate.PacketConfig.List.piSelect != null)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketEditForm(this, null, Operate.ProxyConfig.List.piSelect))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (Operate.ProxyConfig.List.piSelect != null)
                            {
                                bool bOK = false;
                                if (this.hbProxyData.CanCopy())
                                {
                                    this.hbProxyData.CopyHex();
                                    byte[] bBufferCopy = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(Operate.ProxyConfig.List.piSelect, bBufferCopy);
                                }
                                else
                                {
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(Operate.ProxyConfig.List.piSelect, dbp.Bytes.ToArray());
                                }

                                if (bOK)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到滤镜列表", TType.Success)
                                    {
                                        LocalizationText = "ToFilterList.Success"
                                    });
                                }
                                else
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到滤镜列表出错", TType.Error)
                                    {
                                        LocalizationText = "ToFilterList.Error"
                                    });
                                }
                            }

                            break;

                        case "Copy_Text":

                            this.hbProxyData.Copy();

                            break;

                        case "Copy_Hex":

                            this.hbProxyData.CopyHex();

                            break;

                        case "ToTextA":

                            string StringA = string.Empty;
                            if (this.hbProxyData.CanCopy())
                            {
                                this.hbProxyData.CopyHex();
                                StringA = Clipboard.GetText();
                            }
                            else
                            {
                                StringA = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            this.cComparisonText.SetTextA(StringA);

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本A", TType.Success)
                            {
                                LocalizationText = "ToTextA"
                            });

                            break;

                        case "ToTextB":

                            string StringB = string.Empty;
                            if (this.hbProxyData.CanCopy())
                            {
                                this.hbProxyData.CopyHex();
                                StringB = Clipboard.GetText();
                            }
                            else
                            {
                                StringB = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            this.cComparisonText.SetTextB(StringB);

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本B", TType.Success)
                            {
                                LocalizationText = "ToTextB"
                            });

                            break;

                        case "SelectAll":

                            this.hbProxyData.SelectAll();

                            break;

                        default:

                            if (Operate.ProxyConfig.List.piSelect == null)
                            {
                                return;
                            }

                            if (Guid.TryParse(item.ID, out Guid SID))
                            {
                                SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                if (si != null)
                                {
                                    byte[] bBuffer = null;
                                    if (this.hbProxyData.CanCopy())
                                    {
                                        this.hbProxyData.CopyHex();
                                        bBuffer = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    }
                                    else
                                    {
                                        bBuffer = dbp.Bytes.ToArray();
                                    }

                                    List<ProxyInfo> piList = new List<ProxyInfo>
                                    {
                                        new ProxyInfo
                                        {
                                            PacketSocket = Operate.ProxyConfig.List.piSelect.PacketSocket,
                                            PacketType = Operate.ProxyConfig.List.piSelect.PacketType,
                                            ClientAddr = Operate.ProxyConfig.List.piSelect.ClientAddr,
                                            ServerAddr = Operate.ProxyConfig.List.piSelect.ServerAddr,
                                            PacketBuffer = bBuffer,
                                            PacketLen = bBuffer.Length,
                                            PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bBuffer, Operate.PacketConfig.Packet.PacketData_MaxLen),
                                        }
                                    };

                                    if (Operate.SendConfig.Send.AddSendCollection_ByProxyInfo(SID, piList))
                                    {
                                        string sText = string.Format(AntdUI.Localization.Get("ToSendList.Success", "已添加到: {0}"), item.Text);
                                        AntdUI.Message.open(new AntdUI.Message.Config(this, sText, TType.Success));
                                    }
                                    else
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到发送列表出错", TType.Error)
                                        {
                                            LocalizationText = "ToSendList.Error"
                                        });
                                    }
                                }
                            }

                            break;
                    }
                }, Operate.PacketConfig.Packet.GetCMS_PacketData(this.hbProxyData)));
            }
        }

        #endregion

        #region//开始代理

        private void Start_Proxy()
        {
            try
            {
                Operate.ProxyConfig.Proxy.IsListening = true;             

                if (Operate.ProxyConfig.Proxy.ProxyIP_Auto)
                {
                    Operate.ProxyConfig.Proxy.ProxyTCP_IP = IPAddress.Any;
                    Operate.ProxyConfig.Proxy.ProxyUDP_IP = Operate.ProxyConfig.Proxy.ProxyServerIP[0];
                }
                else
                {
                    if (IPAddress.TryParse(Operate.ProxyConfig.Proxy.ProxyIP, out IPAddress proxyIP))
                    {
                        Operate.ProxyConfig.Proxy.ProxyTCP_IP = proxyIP;
                        Operate.ProxyConfig.Proxy.ProxyUDP_IP = proxyIP;
                    }
                    else
                    {
                        Operate.ProxyConfig.Proxy.ProxyTCP_IP = IPAddress.Any;
                        Operate.ProxyConfig.Proxy.ProxyUDP_IP = Operate.ProxyConfig.Proxy.ProxyServerIP[0];
                    }
                }

                string sProxyIP = string.Format(AntdUI.Localization.Get("ProxyModeForm.ProxyServerIP", "代理服务器IP地址: TCP [{0}] UDP [{1}]"), Operate.ProxyConfig.Proxy.ProxyTCP_IP, Operate.ProxyConfig.Proxy.ProxyUDP_IP);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sProxyIP);

                if (Operate.ProxyConfig.Proxy.Enable_Auth)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("ProxyModeForm.ProxyServer.Auth", "已启用代理服务身份认证"));
                }

                if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("ProxyModeForm.ProxyServer.EXTProxy", "已启用外部 SOCKS5 代理"));
                }

                if (ProxyServer == null)
                {
                    InitializeServerSocket();
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this, "开始 SOCKS5 代理", TType.Success)
                {
                    LocalizationText = "ProxyModeForm.StartProxy"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitializeServerSocket()
        {
            try
            {
                ProxyServer?.Close();
                ProxyServer?.Dispose();

                IPEndPoint ep = new IPEndPoint(Operate.ProxyConfig.Proxy.ProxyTCP_IP, Operate.ProxyConfig.Proxy.ProxyPort);
                ProxyServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    LingerState = new LingerOption(false, 0),
                    ExclusiveAddressUse = false
                };

                ProxyServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                ProxyServer.Bind(ep);
                ProxyServer.Listen(backlog: 1000);

                AcceptClients();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void AcceptClients()
        {
            try
            {
                if (Operate.ProxyConfig.Proxy.IsListening && ProxyServer != null)
                {
                    var acceptArgs = new SocketAsyncEventArgs();
                    acceptArgs.Completed += AcceptCompleted;

                    if (!ProxyServer.AcceptAsync(acceptArgs))
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

        private async void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && Operate.ProxyConfig.Proxy.IsListening && e.AcceptSocket != null)
                {
                    await Operate.ProxyConfig.Proxy.HandleClient(e.AcceptSocket);

                    e.AcceptSocket = null;

                    if (Operate.ProxyConfig.Proxy.IsListening)
                    {
                        if (!ProxyServer.AcceptAsync(e))
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
            }
        }

        #endregion

        #region//停止代理

        private void Stop_Proxy()
        {
            try
            {
                Operate.ProxyConfig.Proxy.IsListening = false;

                if (ProxyServer != null)
                {
                    try
                    {
                        ProxyServer.Close();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                    finally
                    {
                        ProxyServer = null;
                    }
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this, "停止 SOCKS5 代理", TType.Error)
                {
                    LocalizationText = "ProxyModeForm.StopProxy"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//查找封包（异步）

        public void SearchProxyList(bool FromHead)
        {
            if (!this.bgwSearchProxyList.IsBusy)
            {
                this.SearchFromHead = FromHead;
                this.bgwSearchProxyList.RunWorkerAsync();
            }
        }

        private void HexBox_FindNext()
        {
            try
            {
                if (Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    long res = this.hbProxyData.Find(Operate.PacketConfig.List.FindOptions);

                    if (res == -1)
                    {
                        Operate.ProxyConfig.List.Search_Index += 1;
                        this.SearchProxyList(this.SearchFromHead);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearchProxyList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Operate.ProxyConfig.List.lstProxyInfo.Count > 0)
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
                                bSearchContent = Operate.SystemConfig.StringToBytes(efFormat, Operate.PacketConfig.List.FindOptions.Text);
                                break;

                            case FindType.Hex:
                                efFormat = Operate.PacketConfig.Packet.EncodingFormat.Hex;
                                bSearchContent = Operate.PacketConfig.List.FindOptions.Hex;
                                break;
                        }

                        if (this.SearchFromHead)
                        {
                            Operate.ProxyConfig.List.Search_Index = 0;
                        }

                        e.Result = Operate.ProxyConfig.List.SearchForProxyList(Operate.ProxyConfig.List.Search_Index, bSearchContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearchProxyList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled && e.Result != null)
                {
                    if (int.TryParse(e.Result.ToString(), out int iSearchResultIndex))
                    {
                        if (iSearchResultIndex >= 0)
                        {
                            this.tProxyList.SelectedIndex = iSearchResultIndex + 1;
                            this.tProxyList.ScrollLine(iSearchResultIndex + 1, true);
                            this.HexBox_FindNext();
                        }
                        else
                        {
                            string NoMatch = AntdUI.Localization.Get("SearchPacketForm.NoMatch", "没有匹配的封包");
                            AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("SearchPacketForm", "查找封包"), "\r\n" + NoMatch + "\r\n\r\n")
                            {
                                Icon = TType.Info,
                                Keyboard = false,
                                MaskClosable = false,
                                CancelText = null,
                            });
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
    }
}

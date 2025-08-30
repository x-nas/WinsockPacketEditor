using AntdUI;
using Be.Windows.Forms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class InjectModeForm : Window, InterfaceInfo.IInjectMode
    {
        private bool bWakeUp = true;
        private bool setcolor = false;
        private bool SearchFromHead = true;
        private readonly WinSockHook ws = new WinSockHook();
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

        #region//窗体事件

        public InjectModeForm()
        {            
            InitializeComponent();
        }

        private void InjectModeForm_Load(object sender, EventArgs e)
        {
            this.pageHeader.Loading = true;
            AntdUI.Spin.open(this, AntdUI.Localization.Get("Loading", "正在加载..."), config =>
            {
                Operate.SystemConfig.LoadInjectMode_FromDB();
                Operate.SystemConfig.LoadProxyMode_FromDB();
                Operate.SystemConfig.LoadSystemList_FromDB();
                Operate.ProxyConfig.Account.LoadProxyAccountList_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapLocal_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapRemote_FromDB();                

                this.InitGlobal();
                this.InitFloatButton();                
                this.InitTable_PacketList();

            }, () =>
            {
                this.pageHeader.Loading = false;
            });

            Operate.SystemConfig.MainHandle = this.Handle;

            this.Dark_Changed();
            this.InitForm();
            this.InitControls();
            this.InitHotKeys();

            this.hbPacketData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.tabInjectMode.TabMenuVisible = false;
            this.mInjectMode.SelectIndex(0, true);            
        }

        private void InjectModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ws.ExitHook();

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

                    if (this.tabInjectMode.SelectedIndex == 2)
                    {
                        Operate.SendConfig.Send.DoSend_ByHotKey(HOTKEY_ID);
                    }
                    else if (this.tabInjectMode.SelectedIndex == 3)
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
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("InjectModeForm", "注入模式");
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;
            this.lProcessName.Text = Operate.ProcessConfig.GetInjectProcessName();
            this.lModuleName.Text = Operate.ProcessConfig.GetInjectModuleName();
            this.lWinsockInfo.Text = Operate.ProcessConfig.GetInjectWinsockInfo();            
            this.lSpeedInfo.Text = Operate.PacketConfig.Packet.GetPacketSpeedInfo();

            this.mInjectMode.Collapsed = false;
            this.MenuCollapseChange();            

            for (int i = 0; i < this.mInjectMode.Items.Count; i++)
            {
                this.mInjectMode.Items[i].BadgeBack = this.colorTheme.Value;
            }

            Operate.DoLog(MethodBase.GetCurrentMethod().Name, this.lProcessName.Text);
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
            this.cFilterList.RefreshFilterList();
        }

        public void RefreshSendList()
        {
            this.cSendList.RefreshSendList();
        }

        public void RefreshRobotList()
        {
            this.cSendList.Refresh();
        }

        public void RefreshPacketData()
        {
            if (Operate.PacketConfig.List.piSelect != null)
            {
                DynamicByteProvider dbp = new DynamicByteProvider(Operate.PacketConfig.List.piSelect.PacketBuffer);
                hbPacketData.ByteProvider = dbp;
            }            
        }

        #endregion

        #region//初始化数据表

        private void InitTable_PacketList()
        {
            tPacketList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column(string.Empty, string.Empty, AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is PacketInfo pi)
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
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column.ID"),
                new AntdUI.Column("PacketTime", "时间戳", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss:fffffff");
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketType", "类别", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)value);
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketSocket", "套接字", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketFrom", "本机地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is PacketInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(pi.FromLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("FromLocation", "所属地").SetWidth("100").SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketTo", "远端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is PacketInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(pi.ToLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("ToLocation", "所属地").SetWidth("100").SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketData", "数据").SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tPacketList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tPacketList.DataSource = Operate.PacketConfig.List.lstPacketInfo;
        }

        public void SetColumnVisible_ProxyList()
        {
            try
            {
                this.tPacketList.Columns[1].Visible = Operate.PacketConfig.List.IsShow_ID;
                this.tPacketList.Columns[2].Visible = Operate.PacketConfig.List.IsShow_ProxyTime;
                this.tPacketList.Columns[3].Visible = Operate.PacketConfig.List.IsShow_PacketType;
                this.tPacketList.Columns[4].Visible = Operate.PacketConfig.List.IsShow_PacketSocket;
                this.tPacketList.Columns[5].Visible = Operate.PacketConfig.List.IsShow_ClientAddr;
                this.tPacketList.Columns[6].Visible = Operate.PacketConfig.List.IsShow_ClientLocation;
                this.tPacketList.Columns[7].Visible = Operate.PacketConfig.List.IsShow_ServerAddr;
                this.tPacketList.Columns[8].Visible = Operate.PacketConfig.List.IsShow_ServerLocation;
                this.tPacketList.Columns[9].Visible = Operate.PacketConfig.List.IsShow_PacketLen;
                this.tPacketList.Columns[10].Visible = Operate.PacketConfig.List.IsShow_PacketData;

            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private Table.CellStyleInfo tPacketList_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            try
            {
                int index = e.RowIndex - 1;
                if (index > -1 && index < Operate.PacketConfig.List.lstPacketInfo.Count)
                {
                    PacketInfo pi = Operate.PacketConfig.List.lstPacketInfo[index];
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

            for (int i = 0; i < this.mInjectMode.Items.Count; i ++)
            {
                this.mInjectMode.Items[i].BadgeBack = e.Value;
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

                this.tabInjectMode.BackColor = Operate.SystemConfig.Color_35;
                
                this.tPacketList.BackColor = Operate.SystemConfig.Color_40;
                this.tPacketList.ColumnBack = Operate.SystemConfig.Color_40;
                this.tPacketList.ColumnFore = Color.Silver;
                this.tPacketList.ForeColor = Color.Lime;

                this.pPacketData.Back = Operate.SystemConfig.Color_40;
                this.hbPacketData.BackColor = Operate.SystemConfig.Color_40;
                this.hbPacketData.ForeColor = Color.Silver;
            }
            else
            {
                BackColor = Operate.SystemConfig.Color_250;
                ForeColor = Color.Black;

                this.tabInjectMode.BackColor = Color.White;

                this.tPacketList.BackColor = Color.White;
                this.tPacketList.ColumnBack = Color.White;
                this.tPacketList.ColumnFore = Color.Black;
                this.tPacketList.ForeColor = Color.Green;

                this.pPacketData.Back = Color.White;
                this.hbPacketData.BackColor = Color.White;
                this.hbPacketData.ForeColor = Color.Black;
            }

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
                this.Text = "WPE x64 - " + AntdUI.Localization.Get("InjectModeForm", "注入模式");
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

        #region//主菜单

        private void bMenuCollapse_Click(object sender, EventArgs e)
        {
            this.mInjectMode.Collapsed = !this.mInjectMode.Collapsed;
            this.MenuCollapseChange();
        }

        private void MenuCollapseChange()
        {
            if (this.mInjectMode.Collapsed)
            {
                this.mInjectMode.Width = this.tlpMenu.Width = this.mInjectMode.CollapseWidth;
            }
            else
            {
                this.mInjectMode.Width = this.tlpMenu.Width = this.mInjectMode.CollapsedWidth;
            }
        }

        private void mInjectMode_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;

            switch (miSelect.ID)
            {
                case "miPacketList":
                    this.tabInjectMode.SelectTab("tpPacketList");
                    break;

                case "miFilterList":
                    this.tabInjectMode.SelectTab("tpFilterList");
                    break;

                case "miSendList":
                    this.tabInjectMode.SelectTab("tpSendList");
                    break;

                case "miRobotList":
                    this.tabInjectMode.SelectTab("tpRobotList");
                    break;

                case "miStatistical":
                    this.tabInjectMode.SelectTab("tpStatistical");
                    break;

                case "miComparison":
                    this.tabInjectMode.SelectTab("tpComparison");
                    break;

                case "miXOR":
                    this.tabInjectMode.SelectTab("tpXOR");
                    break;

                case "miTranscoding":
                    this.tabInjectMode.SelectTab("tpTranscoding");
                    break;

                case "miExtraction":
                    this.tabInjectMode.SelectTab("tpExtraction");
                    break;

                case "miSystemLog":
                    this.tabInjectMode.SelectTab("tpSystemLog");
                    break;
            }
        }

        #endregion

        #region//封包列表 - 菜单

        private void bHookStart_Click(object sender, EventArgs e)
        {
            this.bHookStart.Enabled = false;
            this.bHookStop.Enabled = true;

            this.Start_Hook();
        }

        private void bHookStop_Click(object sender, EventArgs e)
        {
            this.bHookStart.Enabled = true;
            this.bHookStop.Enabled = false;

            this.Stop_Hook();
        }

        private void bPacketList_Clear_Click(object sender, EventArgs e)
        {
            this.CleanUp_PacketListInfo();
            this.CleanUp_PacketList();
            this.CleanUp_HexBox();
            this.cLogList.CleanUp_LogList();

            AntdUI.Message.open(new AntdUI.Message.Config(this, "已清空数据", TType.Warn)
            {
                LocalizationText = "ClearedData"
            });
        }

        private void mPacketList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mPacketList.SelectIndex(-1);

            switch (miSelect.ID)
            {
                case "miPacketListSearch":

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

        #region//封包列表 - 右键菜单        

        private void tPacketList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.PacketConfig.List.lstPacketInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tPacketList, item =>
                {
                    List<PacketInfo> piList = new List<PacketInfo>();

                    foreach (int SelectIndex in this.tPacketList.SelectedIndexs)
                    {
                        piList.Add(Operate.PacketConfig.List.lstPacketInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Edit":

                            if (piList.Count > 0)
                            {
                                var PacketEdit = new PacketEdit(this, piList[0]);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (piList.Count > 0)
                            {
                                bool bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(piList[0], null);
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
                                var PacketModification = new PacketModification(this, piList[0]);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("PacketModificationForm", "封包数据对比"), PacketModification)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.PacketConfig.List.SavePacketList_Dialog(this, this.tPacketList, Operate.PacketConfig.Packet.InjectProcess, piList);

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

                            this.tPacketList.SelectedIndex = -1;

                            break;

                        default:

                            if (piList.Count > 0)
                            {
                                if (Guid.TryParse(item.ID, out Guid SID))
                                {
                                    SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                    if (si != null && piList.Count > 0)
                                    {
                                        if (Operate.SendConfig.Send.AddSendCollection_ByPacketInfo(SID, piList))
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到 " + item.Text, TType.Success)
                                            {
                                                LocalizationText = "ToSendList.Success"
                                            });
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

        #region//封包数据 - 右键菜单

        private void hbPacketData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;
                if (dbp == null || dbp.Bytes.Count == 0)
                { 
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbPacketData, (item) =>
                {
                    switch (item.ID)
                    {
                        case "Edit":

                            if (Operate.PacketConfig.List.piSelect != null)
                            {
                                var PacketEdit = new PacketEdit(this, Operate.PacketConfig.List.piSelect);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (Operate.PacketConfig.List.piSelect != null)
                            {
                                bool bOK = false;
                                if (this.hbPacketData.CanCopy())
                                {
                                    this.hbPacketData.CopyHex();
                                    byte[] bBufferCopy = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.piSelect, bBufferCopy);
                                }
                                else
                                {
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.piSelect, dbp.Bytes.ToArray());
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

                            this.hbPacketData.Copy();

                            break;

                        case "Copy_Hex":

                            this.hbPacketData.CopyHex();

                            break;

                        case "ToTextA":

                            string StringA = string.Empty;
                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
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
                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
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

                            this.hbPacketData.SelectAll();

                            break;

                        default:

                            if (Operate.PacketConfig.List.piSelect == null)
                            {
                                return;
                            }

                            if (Guid.TryParse(item.ID, out Guid SID))
                            {
                                SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                if (si != null)
                                {
                                    byte[] bBuffer = null;
                                    if (this.hbPacketData.CanCopy())
                                    {
                                        this.hbPacketData.CopyHex();
                                        bBuffer = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    }
                                    else
                                    {
                                        bBuffer = dbp.Bytes.ToArray();
                                    }

                                    List<PacketInfo> piList = new List<PacketInfo>
                                    {
                                        new PacketInfo
                                        {
                                            PacketSocket = Operate.PacketConfig.List.piSelect.PacketSocket,
                                            PacketType = Operate.PacketConfig.List.piSelect.PacketType,
                                            PacketFrom = Operate.PacketConfig.List.piSelect.PacketFrom,
                                            PacketTo = Operate.PacketConfig.List.piSelect.PacketTo,
                                            PacketBuffer = bBuffer,
                                            PacketLen = bBuffer.Length,
                                            PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bBuffer, Operate.PacketConfig.Packet.PacketData_MaxLen),
                                        }
                                    };

                                    if (Operate.SendConfig.Send.AddSendCollection_ByPacketInfo(SID, piList))
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到 " + item.Text, TType.Success)
                                        {
                                            LocalizationText = "ToSendList.Success"
                                        });
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
                }, Operate.PacketConfig.Packet.GetCMS_PacketData(this.hbPacketData)));
            }
        }

        #endregion                       

        #region//开始拦截

        private void Start_Hook()
        {
            try
            {                
                ws.StartHook();

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this, "开始拦截", TType.Success)
                {
                    LocalizationText = "InjectModeForm.StartHook"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//停止拦截        

        private void Stop_Hook()
        {
            try
            {
                ws.StopHook();      

                AntdUI.Message.open(new AntdUI.Message.Config(this, "停止拦截", TType.Error)
                {
                    LocalizationText = "InjectModeForm.StopHook"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//清理数据

        private void CleanUp_PacketListInfo()
        {
            try
            {
                Operate.PacketConfig.Packet.TotalPackets = 0;
                Operate.PacketConfig.Packet.Total_SendBytes = 0;
                Operate.PacketConfig.Packet.Total_RecvBytes = 0;
                Operate.FilterConfig.Filter.FilterExecute_CNT = 0;
                Operate.FilterConfig.Filter.FilterReplace_CNT = 0;
                Operate.FilterConfig.Filter.FilterChange_CNT = 0;
                Operate.FilterConfig.Filter.FilterIntercept_CNT = 0;
                Operate.FilterConfig.Filter.FilterDisplay_CNT = 0;
                Operate.FilterConfig.Filter.FilterNoDisplay_CNT = 0;
                Operate.PacketConfig.Packet.FilterPacket_CNT = 0;
                Operate.PacketConfig.Packet.Send_CNT = 0;
                Operate.PacketConfig.Packet.Recv_CNT = 0;
                Operate.PacketConfig.Packet.SendTo_CNT = 0;
                Operate.PacketConfig.Packet.RecvFrom_CNT = 0;
                Operate.PacketConfig.Packet.WSASend_CNT = 0;
                Operate.PacketConfig.Packet.WSARecv_CNT = 0;
                Operate.PacketConfig.Packet.WSASendTo_CNT = 0;
                Operate.PacketConfig.Packet.WSARecvFrom_CNT = 0;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_PacketList()
        {
            try
            {
                Operate.PacketConfig.Queue.ClearPacketQueue();
                Operate.PacketConfig.List.lstPacketInfo.Clear();
                Operate.PacketConfig.List.piSelect = null;                
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

        #endregion

        #region//计时器

        private async void timerPacketList_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerPacketList.Stop();

                await Task.Run(() =>
                {
                    if (Operate.PacketConfig.Queue.cqPacketInfo.Count > 0)
                    {
                        Operate.PacketConfig.List.PacketToList();
                    }

                    if (Operate.LogConfig.Queue.cqLogInfo.Count > 0)
                    {
                        Operate.LogConfig.List.LogToList();
                    }

                    if (Operate.LogConfig.Queue.cqFilterLogInfo.Count > 0)
                    {
                        Operate.LogConfig.List.FilterLogToList();
                    }
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                this.timerPacketList.Start();
            }
        }

        private async void timerPacketListInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerPacketListInfo.Stop();

                await Task.Run(() =>
                {
                    this.RefreshPacketList();
                    this.cLogList?.RefreshLogList();

                    this.ShowInjectInfo();                   
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                this.timerPacketListInfo.Start();
            }      
        }

        #endregion

        #region//显示选中的封包数据

        private void tPacketList_SelectIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = tPacketList.SelectedIndex - 1;
                if (selectedIndex >= 0 && selectedIndex < Operate.PacketConfig.List.lstPacketInfo.Count)
                {
                    Operate.PacketConfig.List.Search_Index = selectedIndex;
                    Operate.PacketConfig.List.piSelect = Operate.PacketConfig.List.lstPacketInfo[selectedIndex];

                    DynamicByteProvider dbp = new DynamicByteProvider(Operate.PacketConfig.List.piSelect.PacketBuffer);
                    hbPacketData.ByteProvider = dbp;                    
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示封包列表

        private void RefreshPacketList()
        {
            try
            {
                if (tPacketList.InvokeRequired)
                {
                    tPacketList.BeginInvoke(new Action(() => this.tPacketList.Refresh()));
                }
                else
                {
                    this.tPacketList.Refresh();
                }

                if (Operate.PacketConfig.List.AutoRoll)
                {
                    tPacketList.ScrollBar.ValueY = tPacketList.ScrollBar.MaxY;
                }

                if (Operate.PacketConfig.List.AutoClear)
                {
                    if (Operate.PacketConfig.List.lstPacketInfo.Count > Operate.PacketConfig.List.AutoClear_Value)
                    {
                        this.CleanUp_PacketList();
                        this.CleanUp_HexBox();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示注入信息

        private void ShowInjectInfo()
        {
            this.lTotal_CNT.Text = Operate.PacketConfig.Packet.TotalPackets.ToString();
            this.lFilterExecute_CNT.Text = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
            this.lQueue_CNT.Text = Operate.PacketConfig.Queue.cqPacketInfo.Count.ToString();
            this.lFilterPacket_CNT.Text = Operate.PacketConfig.Packet.FilterPacket_CNT.ToString();
            this.lSend_CNT.Text = Operate.PacketConfig.Packet.Send_CNT.ToString();
            this.lRecv_CNT.Text = Operate.PacketConfig.Packet.Recv_CNT.ToString();
            this.lSendTo_CNT.Text = Operate.PacketConfig.Packet.SendTo_CNT.ToString();
            this.lRecvFrom_CNT.Text = Operate.PacketConfig.Packet.RecvFrom_CNT.ToString();
            this.lWSASend_CNT.Text = Operate.PacketConfig.Packet.WSASend_CNT.ToString();
            this.lWSARecv_CNT.Text = Operate.PacketConfig.Packet.WSARecv_CNT.ToString();
            this.lWSASendTo_CNT.Text = Operate.PacketConfig.Packet.WSASendTo_CNT.ToString();
            this.lWSARecvFrom_CNT.Text = Operate.PacketConfig.Packet.WSARecvFrom_CNT.ToString();
            this.lSpeedInfo.Text = Operate.PacketConfig.Packet.GetPacketSpeedInfo();
            this.mInjectMode.Items[0].Badge = Operate.PacketConfig.List.lstPacketInfo.Count.ToString();
            this.mInjectMode.Items[1].Badge = Operate.FilterConfig.List.lstFilterInfo.Count.ToString();
            this.mInjectMode.Items[2].Badge = Operate.SendConfig.List.lstSendInfo.Count.ToString();
            this.mInjectMode.Items[3].Badge = Operate.RobotConfig.List.lstRobotInfo.Count.ToString();
            this.mInjectMode.Items[9].Badge = Operate.LogConfig.List.lstLogInfo.Count.ToString();
        }

        #endregion

        #region//查找封包（异步）

        public void SearchPacketList(bool FromHead)
        {
            if (!this.bgwSearchPacketList.IsBusy)
            {
                this.SearchFromHead = FromHead;
                this.bgwSearchPacketList.RunWorkerAsync();
            }
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
                        this.SearchPacketList(this.SearchFromHead);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearchPacketList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (Operate.PacketConfig.List.lstPacketInfo.Count > 0)
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
                            Operate.PacketConfig.List.Search_Index = 0;
                        }

                        e.Result = Operate.PacketConfig.List.SearchForPacketList(Operate.PacketConfig.List.Search_Index, bSearchContent);                        
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearchPacketList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled && e.Result != null)
                {
                    if (int.TryParse(e.Result.ToString(), out int iSearchResultIndex))
                    {
                        if (iSearchResultIndex >= 0)
                        {
                            this.tPacketList.SelectedIndex = iSearchResultIndex + 1;
                            this.tPacketList.ScrollLine(iSearchResultIndex + 1, true);
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

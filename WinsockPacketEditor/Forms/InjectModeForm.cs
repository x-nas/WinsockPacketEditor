using AntdUI;
using Be.Windows.Forms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinsockPacketEditor
{
    public partial class InjectModeForm : Window, InterfaceInfo.IInjectMode
    {
        private bool bWakeUp = true;
        private bool setcolor = false;
        private bool SearchFromHead = true;
        private string TextA = string.Empty;
        private string TextB = string.Empty;       
        private readonly WinSockHook ws = new WinSockHook();
        private AntdUI.FormFloatButton FloatButton = null;
        private FilterList cFilterList = null;
        private SendList cSendList = null;
        private RobotList cRobotList = null;

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
                this.InitTable_LogList();
                this.InitTable_StatisticalFilter();

            }, () =>
            {
                this.pageHeader.Loading = false;
            });

            Operate.SystemConfig.MainHandle = this.Handle;

            this.Dark_Changed();
            this.InitForm();
            this.InitControls();
            this.InitComparison();
            this.InitExtraction();
            this.InitHotKeys();

            this.hbXOR_From.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbPacketData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.hbXOR_From.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.hbXOR_To.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
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
        }

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("InjectModeForm", "注入模式");
            this.pageHeader.Text = "Winsock Packet Editor";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;
            this.lProcessName.Text = Operate.ProcessConfig.GetInjectProcessName();
            this.lModuleName.Text = Operate.ProcessConfig.GetInjectModuleName();
            this.lWinsockInfo.Text = Operate.ProcessConfig.GetInjectWinsockInfo();            
            this.lSpeedInfo.Text = Operate.PacketConfig.Packet.GetPacketSpeedInfo();

            this.mInjectMode.Collapsed = true;
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
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column."),
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

        private void InitTable_LogList()
        {
            tSystemLog.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("LogTime", "时间戳")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss:fffffff");
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("FuncName", "模块", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("LogContent", "日志内容").SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tSystemLog.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSystemLog.DataSource = Operate.LogConfig.List.lstLogInfo;
        }

        private void InitTable_StatisticalFilter()
        {
            tStatisticalFilter.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.StatisticalFilter.Column."),
                new AntdUI.Column("FName", "滤镜名称").SetLocalizationTitleID("Table.StatisticalFilter.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            if(fi.IsEnable)
                            {
                                if(fi.ExecutionCount > 0)
                                {
                                    return new AntdUI.CellBadge(AntdUI.TState.Processing, "处理中");
                                }
                                else
                                {
                                    return new AntdUI.CellBadge(AntdUI.TState.Success, "启用");
                                }
                            }
                            else
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Error, "停止");
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.StatisticalFilter.Column."),
                new AntdUI.Column("FAction", "动作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        switch((Operate.FilterConfig.Filter.FilterAction)value)
                        {
                            case Operate.FilterConfig.Filter.FilterAction.Replace:
                                return AntdUI.Localization.Get("FilterAction.Replace", "替换");

                            case Operate.FilterConfig.Filter.FilterAction.Change:
                                return AntdUI.Localization.Get("FilterAction.Change", "换包");

                            case Operate.FilterConfig.Filter.FilterAction.Intercept:
                                return AntdUI.Localization.Get("FilterAction.Intercept", "拦截");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                                return AntdUI.Localization.Get("FilterAction.NoModify_NoDisplay", "不显示");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                                return AntdUI.Localization.Get("FilterAction.NoModify_Display", "只显示");

                            default:
                                return value;
                        }
                    },
                }.SetLocalizationTitleID("Table.StatisticalFilter.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.StatisticalFilter.Column."),
            };

            this.tStatisticalFilter.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tStatisticalFilter.Binding(Operate.FilterConfig.List.lstFilterInfo);
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
                BackColor = Color.FromArgb(30, 30, 30);
                ForeColor = Color.White;

                this.tPacketList.ColumnFore = Color.Silver;
                this.tPacketList.ForeColor = Color.LimeGreen;

                this.hbPacketData.BackColor =
                    this.hbXOR_From.BackColor = 
                    this.hbXOR_To.BackColor =                    
                    Color.FromArgb(30, 30, 30);

                this.hbPacketData.ForeColor = 
                    this.hbXOR_From.ForeColor = 
                    this.hbXOR_To.ForeColor =                   
                    Color.Silver;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;

                this.tPacketList.ColumnFore = Color.Black;
                this.tPacketList.ForeColor = Color.Green;

                this.hbPacketData.BackColor =
                    this.hbXOR_From.BackColor =
                    this.hbXOR_To.BackColor =                
                    Color.White;

                this.hbPacketData.ForeColor =
                    this.hbXOR_From.ForeColor =
                    this.hbXOR_To.ForeColor =                
                    Color.Black;
            }

            this.Statistical_DarkChanged();
        }

        private void Statistical_DarkChanged()
        {
            if (Dark)
            {
                this.progressExecute.Back = Color.FromArgb(48, 58, 66);
                this.progressReplace.Back = Color.FromArgb(39, 41, 83);
                this.progressReplace.ForeColor = Color.White;
                this.progressChange.Back = Color.FromArgb(47, 46, 80);
                this.progressChange.ForeColor = Color.White;
                this.progressIntercept.Back = Color.FromArgb(57, 47, 78);
                this.progressIntercept.ForeColor = Color.White;
                this.progressDisplay.Back = Color.FromArgb(67, 46, 76);
                this.progressDisplay.ForeColor = Color.White;
                this.progressNoDisplay.Back = Color.FromArgb(80, 47, 79);
                this.progressNoDisplay.ForeColor = Color.White;
            }
            else
            {
                this.progressExecute.Back = null;
                this.progressReplace.Back = null;
                this.progressReplace.ForeColor = null;
                this.progressChange.Back = null;
                this.progressChange.ForeColor = null;
                this.progressIntercept.Back = null;
                this.progressIntercept.ForeColor = null;
                this.progressDisplay.Back = null;
                this.progressDisplay.ForeColor = null;
                this.progressNoDisplay.Back = null;
                this.progressNoDisplay.ForeColor = null;
            }
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
                this.bMenuCollapse.IconSvg = "MenuUnfoldOutlined";
            }
            else
            {
                this.mInjectMode.Width = this.tlpMenu.Width = this.mInjectMode.CollapsedWidth;
                this.bMenuCollapse.IconSvg = "MenuFoldOutlined";
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
            this.CleanUp_LogList();

            AntdUI.Message.open(new AntdUI.Message.Config(this, "已清空数据", TType.Warn)
            {
                LocalizationText = "InjectModeForm.Clear"
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

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new FilterSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miHookSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HookSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miListSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ListSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miHotKeySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HotKeyForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miBackUpSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new BackUpSettingsForm())
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
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketEditForm(this, piList[0], null))
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
                                    LocalizationText = "SYSSocket.Success"
                                });
                            }                            

                            break;

                        case "PacketModification":

                            if (piList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketModificationForm(this, piList[0], null))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.PacketConfig.List.SavePacketList_Dialog(this, this.tPacketList, Operate.PacketConfig.Packet.InjectProcess, piList);

                            break;

                        case "ToTextA":

                            if (piList.Count > 0)
                            {
                                this.TextA = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, piList[0].PacketBuffer);
                                this.txtComparison_A.Text = this.TextA;

                                AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本A", TType.Success)
                                {
                                    LocalizationText = "System.ToTextA"
                                });
                            }

                            break;

                        case "ToTextB":

                            if (piList.Count > 0)
                            {
                                this.TextB = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, piList[0].PacketBuffer);
                                this.txtComparison_B.Text = this.TextB;

                                AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本B", TType.Success)
                                {
                                    LocalizationText = "System.ToTextB"
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
                                                LocalizationText = "cmsPacketList_ToSendList.Success"
                                            });
                                        }
                                        else
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到发送列表出错", TType.Error)
                                            {
                                                LocalizationText = "cmsPacketList_ToSendList.Error"
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
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketEditForm(this, Operate.PacketConfig.List.piSelect, null))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
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

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
                                this.TextA = Clipboard.GetText();
                            }
                            else
                            {
                                this.TextA = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            this.txtComparison_A.Text = this.TextA;

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本A", TType.Success)
                            {
                                LocalizationText = "System.ToTextA"
                            });

                            break;

                        case "ToTextB":

                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
                                this.TextB = Clipboard.GetText();
                            }
                            else
                            {
                                this.TextB = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            this.txtComparison_B.Text = this.TextB;

                            AntdUI.Message.open(new AntdUI.Message.Config(this, "已添加到文本B", TType.Success)
                            {
                                LocalizationText = "System.ToTextB"
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
                                            LocalizationText = "cmsPacketList_ToSendList.Success"
                                        });
                                    }
                                    else
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(this, "添加到发送列表出错", TType.Error)
                                        {
                                            LocalizationText = "cmsPacketList_ToSendList.Error"
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

        #region//日志列表 - 右键菜单

        private void tSystemLog_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.LogConfig.List.lstLogInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tSystemLog, item =>
                {
                    List<LogInfo> liList = new List<LogInfo>();

                    foreach (int SelectIndex in this.tSystemLog.SelectedIndexs)
                    {
                        liList.Add(Operate.LogConfig.List.lstLogInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Copy":

                            if (liList.Count > 0)
                            {
                                string LogString = string.Empty;
                                foreach (LogInfo li in liList)
                                {
                                    LogString += li.LogTime.ToString() + ": " + li.FuncName + " - " + li.LogContent + "\r\n";
                                }

                                Clipboard.SetText(LogString);

                                AntdUI.Message.open(new AntdUI.Message.Config(this, "已复制到剪贴板", TType.Success)
                                {
                                    LocalizationText = "System.CopyLog"
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.LogConfig.List.SaveLogList_Dialog(this, this.tSystemLog, Operate.PacketConfig.Packet.InjectProcess, liList);

                            break;                

                        case "ClearUp":

                            AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("InjectModeForm.miLogList", "日志列表"), "\r\n确定删除所有数据吗\r\n\r\n")
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,
                                OnOk = config =>
                                {
                                    this.CleanUp_LogList();

                                    return true;
                                }
                            });                            

                            break;

                        case "DeSelect":

                            this.tSystemLog.SelectedIndex = -1;

                            break;
                    }
                }, Operate.LogConfig.List.GetCMS_LogList());
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

        private void CleanUp_LogList()
        {
            try
            {
                Operate.LogConfig.Queue.ClearLogQueue();
                Operate.LogConfig.List.ClearLogList();                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//计时器

        private void timerPacketList_Tick(object sender, EventArgs e)
        {
            if (Operate.PacketConfig.Queue.cqPacketInfo.Count > 0)
            {
                Operate.PacketConfig.List.PacketToList();
            }

            if (Operate.LogConfig.Queue.cqLogInfo.Count > 0)
            {
                Operate.LogConfig.List.LogToList();
            }
        }        

        private void timerPacketListInfo_Tick(object sender, EventArgs e)
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

            if (!this.bgwPacketList.IsBusy)
            { 
                this.bgwPacketList.RunWorkerAsync();
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

        #region//显示封包列表（异步）

        private void bgwPacketList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
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

                if (Operate.LogConfig.List.AutoRoll)
                {
                    tSystemLog.ScrollBar.ValueY = tSystemLog.ScrollBar.MaxY;
                }

                if (Operate.LogConfig.List.AutoClear)
                {
                    if (Operate.LogConfig.List.lstLogInfo.Count > Operate.LogConfig.List.AutoClear_Value)
                    {
                        this.CleanUp_LogList();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwPacketList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.tPacketList.Refresh();
            this.tSystemLog.Refresh();
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

        #region//统计数据

        private void bStatistical_Filter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.bgwStatistical.IsBusy)
                {
                    this.bStatistical_Filter.Loading = true;
                    this.bgwStatistical.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwStatistical_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                long ProxyTotal_CNT =
                    Operate.ProxyConfig.Proxy.TCP_Req_CNT +
                    Operate.ProxyConfig.Proxy.TCP_Resp_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Req_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Resp_CNT;

                long FilterExec = Operate.FilterConfig.Filter.FilterExecute_CNT;
                long FilterReplace = Operate.FilterConfig.Filter.FilterReplace_CNT;
                long FilterChange = Operate.FilterConfig.Filter.FilterChange_CNT;
                long FilterIntercept = Operate.FilterConfig.Filter.FilterIntercept_CNT;
                long FilterDisplay = Operate.FilterConfig.Filter.FilterDisplay_CNT;
                long FilterNoDisplay = Operate.FilterConfig.Filter.FilterNoDisplay_CNT;

                decimal dExecute = 0;
                if (ProxyTotal_CNT > 0)
                {
                    dExecute = (decimal)FilterExec / ProxyTotal_CNT;
                    dExecute = Math.Round(dExecute, 2);
                }

                this.progressExecute.Value = (float)dExecute;
                this.progressExecute.Text = (dExecute * 100).ToString() + "%";

                decimal dReplace = 0, dChange = 0, dIntercept = 0, dDisplay = 0, dNoDisplay = 0;
                if (FilterExec > 0)
                {
                    dReplace = (decimal)FilterReplace / FilterExec;
                    dReplace = Math.Round(dReplace, 2);
                    dChange = (decimal)FilterChange / FilterExec;
                    dChange = Math.Round(dChange, 2);
                    dIntercept = (decimal)FilterIntercept / FilterExec;
                    dIntercept = Math.Round(dIntercept, 2);
                    dDisplay = (decimal)FilterDisplay / FilterExec;
                    dDisplay = Math.Round(dDisplay, 2);
                    dNoDisplay = (decimal)FilterNoDisplay / FilterExec;
                    dNoDisplay = Math.Round(dNoDisplay, 2);
                }

                this.progressReplace.Value = (float)dReplace;
                this.progressReplace.Text = (dReplace * 100).ToString() + "%";
                this.progressChange.Value = (float)dChange;
                this.progressChange.Text = (dChange * 100).ToString() + "%";
                this.progressIntercept.Value = (float)dIntercept;
                this.progressIntercept.Text = (dIntercept * 100).ToString() + "%";
                this.progressDisplay.Value = (float)dDisplay;
                this.progressDisplay.Text = (dDisplay * 100).ToString() + "%";
                this.progressNoDisplay.Value = (float)dNoDisplay;
                this.progressNoDisplay.Text = (dNoDisplay * 100).ToString() + "%";
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwStatistical_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.bStatistical_Filter.Loading = false;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//文本对比

        #region//初始化

        private void InitComparison()
        {
            this.ddlComparisonType.Items.Clear();

            this.ddlComparisonType.Items.AddRange(new AntdUI.SelectItem[]
            {
                    new AntdUI.SelectItem("文本比较")
                    {
                        LocalizationText = "",
                    },
                    new AntdUI.SelectItem("文本查重")
                    {
                        LocalizationText = "",
                    },
            });

            this.ddlComparisonType.SelectedIndex = 0;
            this.ComparisonType_Changed();

            this.Comparison_A_Changed();
            this.Comparison_B_Changed();
        }

        private void txtComparison_A_TextChanged(object sender, EventArgs e)
        {
            this.Comparison_A_Changed();
        }

        private void Comparison_A_Changed()
        {
            string StringA = this.txtComparison_A.Text.Trim();
            if (string.IsNullOrEmpty(StringA))
            {
                this.txtComparison_A.Status = TType.Error;
            }
            else
            {
                this.txtComparison_A.Status = TType.Success;
            }

            this.lComparison_A.Text = string.Format(AntdUI.Localization.Get("System.TextA", "文本 A  ( 长度 {0} )"), StringA.Length);
        }

        private void txtComparison_B_TextChanged(object sender, EventArgs e)
        {
            this.Comparison_B_Changed();
        }

        private void Comparison_B_Changed()
        {
            string StringB = this.txtComparison_B.Text.Trim();
            if (string.IsNullOrEmpty(StringB))
            {
                this.txtComparison_B.Status = TType.Error;
            }
            else
            {
                this.txtComparison_B.Status = TType.Success;
            }

            this.lComparison_B.Text = string.Format(AntdUI.Localization.Get("System.TextB", "文本 B  ( 长度 {0} )"), StringB.Length);
        }

        private void ddlComparisonType_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            this.ComparisonType_Changed();
        }

        private void ComparisonType_Changed()
        {
            if (this.ddlComparisonType.SelectedIndex == 0)
            {
                this.nudComparison_DuplicateNum.Enabled = false;
            }
            else if (this.ddlComparisonType.SelectedIndex == 1)
            {
                this.nudComparison_DuplicateNum.Enabled = true;
            }
        }

        #endregion

        #region//分析文本

        private void bComparison_Click(object sender, EventArgs e)
        {
            try
            {
                this.bComparison.Loading = true;
                this.txtComparison_Result.Spin(AntdUI.Localization.Get("Loading", "正在加载..."), config =>
                {
                    this.txtComparison_Result.Clear();

                    if (this.ddlComparisonType.SelectedIndex == 0)
                    {
                        string StringA = this.txtComparison_A.Text.Trim();
                        string StringB = this.txtComparison_B.Text.Trim();

                        if (!string.IsNullOrEmpty(StringA) || !string.IsNullOrEmpty(StringB))
                        {
                            string rtfString = Operate.SystemConfig.CompareData(this.Font, StringA, StringB);
                            var styles = Operate.SystemConfig.ConvertRtfToTextStyles(rtfString);

                            using (var rtb = new RichTextBox())
                            {
                                rtb.Rtf = rtfString;
                                this.txtComparison_Result.Text = rtb.Text;
                            }

                            foreach (var style in styles)
                            {
                                if (style.Fore == Color.Red || style.Fore == Color.Green)
                                {
                                    this.txtComparison_Result.SetStyle(style.Start, style.Length, this.Font, style.Fore, null);
                                }
                                else
                                {
                                    this.txtComparison_Result.SetStyle(style.Start, style.Length, this.Font, null, null);
                                }                                
                            }
                        }
                    }
                    else if (this.ddlComparisonType.SelectedIndex == 1)
                    {
                        this.TextA = this.txtComparison_A.Text.Trim();
                        this.TextB = this.txtComparison_B.Text.Trim();
                        int minBytes = (int)nudComparison_DuplicateNum.Value;
                        var results = Operate.SystemConfig.ComparePackets(this.TextA, this.TextB, minBytes);

                        this.txtComparison_A.Text = Operate.SystemConfig.FormatHex(results.TextA);
                        this.txtComparison_B.Text = Operate.SystemConfig.FormatHex(results.TextB);
                    }
                }, () =>
                {
                    this.bComparison.Loading = false;
                });                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }        

        #endregion

        #region//还原

        private void bComparison_Reset_Click(object sender, EventArgs e)
        {
            this.txtComparison_A.Text = this.TextA;
            this.txtComparison_B.Text = this.TextB;
        }

        #endregion

        #region//交换

        private void bComparison_Change_Click(object sender, EventArgs e)
        {
            string sTextA = this.txtComparison_A.Text.Trim();
            string sTextB = this.txtComparison_B.Text.Trim();

            this.txtComparison_A.Text = sTextB;
            this.txtComparison_B.Text = sTextA;
        }

        #endregion

        #region//清空

        private void bComparison_Clean_Click(object sender, EventArgs e)
        {
            this.txtComparison_A.Clear();
            this.txtComparison_B.Clear();
            this.txtComparison_Result.Clear();
        }


        #endregion

        #endregion

        #region//异或计算

        private void txtXOR_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtXOR.Text.Trim()))
            {
                this.txtXOR.Status = TType.Error;
            }
            else
            {
                this.txtXOR.Status = TType.Success;
            }
        }

        private void bXOR_Click(object sender, EventArgs e)
        {
            try
            {
                DynamicByteProvider dbpXOR_From = this.hbXOR_From.ByteProvider as DynamicByteProvider;
                if (dbpXOR_From == null)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "异或值为空", TType.Error)
                    {
                        LocalizationText = "XOR.Empty"
                    });

                    return;
                }                

                byte[] blXOR_From = dbpXOR_From.Bytes.ToArray();
                if (blXOR_From.Length == 0)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "异或值为空", TType.Error)
                    {
                        LocalizationText = "XOR.Empty"
                    });

                    return;
                }
                
                if (string.IsNullOrEmpty(this.txtXOR.Text.Trim()))
                {
                    this.txtXOR.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this, "异或值为空", TType.Error)
                    {
                        LocalizationText = "XOR.Empty"
                    });

                    return;
                }

                if (!Operate.SystemConfig.IsHexString(this.txtXOR.Text.Trim()))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "异或值不是十六进制", TType.Error)
                    {
                        LocalizationText = "XOR.Error"
                    });

                    return;
                }

                string[] slXOR_Value = this.txtXOR.Text.Trim().Split(' ');
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
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "异或值不是十六进制", TType.Error)
                        {
                            LocalizationText = "XOR.Error"
                        });

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

        private void bXOR_ClearUp_Click(object sender, EventArgs e)
        {
            this.hbXOR_From.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.txtXOR.Clear();
        }

        private void hbXOR_From_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbXOR_From, (item) =>
                {
                    DynamicByteProvider dbp = hbXOR_From.ByteProvider as DynamicByteProvider;

                    switch (item.ID)
                    {
                        case "Cut":

                            this.hbXOR_From.Cut();

                            break;

                        case "Copy":

                            this.hbXOR_From.Copy();

                            break;

                        case "Paste":

                            this.hbXOR_From.Paste();

                            break;

                        case "SelectAll":

                            this.hbXOR_From.SelectAll();

                            break;
                    }
                }, Operate.SystemConfig.GetCMS_XOR(this.hbXOR_From)));
            }
        }

        private void hbXOR_To_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbXOR_To, (item) =>
                {
                    DynamicByteProvider dbp = hbXOR_To.ByteProvider as DynamicByteProvider;

                    switch (item.ID)
                    {
                        case "Cut":

                            this.hbXOR_To.Cut();

                            break;

                        case "Copy":

                            this.hbXOR_To.Copy();

                            break;

                        case "Paste":

                            this.hbXOR_To.Paste();

                            break;

                        case "SelectAll":

                            this.hbXOR_To.SelectAll();

                            break;
                    }
                }, Operate.SystemConfig.GetCMS_XOR(this.hbXOR_To)));
            }
        }


        #endregion

        #region//编码转换

        private void bEncoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sEncodingText = this.txtTranscoding.Text.Trim();

                this.txtBytes.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Bytes, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sEncodingText));
                this.txtANSIGBK.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.GBK, sEncodingText));

                this.txtUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF7, sEncodingText));
                this.txtANSIUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF7, sEncodingText));

                this.txtUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sEncodingText));
                this.txtANSIUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF8, sEncodingText));

                this.txtUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF16, sEncodingText));
                this.txtANSIUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF16, sEncodingText));

                this.txtUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF32, sEncodingText));
                this.txtANSIUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.UTF32, sEncodingText));

                this.txtUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Unicode, sEncodingText));
                this.txtANSIUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Unicode, sEncodingText));

                string sBase64 = Operate.SystemConfig.Base64_Encoding(sEncodingText);
                this.txtbase64.Text = sBase64;
                this.txtANSIbase64.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sBase64));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bDecoding_Click(object sender, EventArgs e)
        {
            try
            {
                string sDecodingText = this.txtTranscoding.Text;

                this.txtBytes.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Bytes, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIGBK.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.GBK, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF7, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF7.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF7, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF8.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF8, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF16, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF16.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF16, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF32, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUTF32.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UTF32, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Unicode, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Default, sDecodingText));
                this.txtANSIUnicode.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Unicode, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText));

                this.txtbase64.Text = Operate.SystemConfig.Base64_Decoding(sDecodingText);
                this.txtANSIbase64.Text = Operate.SystemConfig.Base64_Decoding(Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Default, Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, sDecodingText)));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtTranscoding_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTranscoding.Text.Trim()))
            {
                this.txtTranscoding.Status = TType.Error;
            }
            else
            {
                this.txtTranscoding.Status = TType.Success;
            }
        }

        #endregion

        #region//数据提取

        private void InitExtraction()
        {
            this.ddlExtraction.Items.Clear();
            this.ddlExtraction.Items.AddRange(new AntdUI.SelectItem[]
            {
                    new AntdUI.SelectItem("[ Charles XML 会话文件（.chlsx）] 提取 [ 十六进制数据 ]")
                    {
                        LocalizationText = "",
                    },
                    new AntdUI.SelectItem("[ FILT过滤器文件（.filt）] 提取 [ WPE64 滤镜文件（.sp）]")
                    {
                        LocalizationText = "",
                    },
            });

            this.ddlExtraction.SelectedIndex = 0;
            this.Extraction_Changed();
            this.udExtraction.UseAdmin();
        }

        private void ddlExtraction_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            this.Extraction_Changed();
        }

        private void Extraction_Changed()
        {
            if (this.ddlExtraction.SelectedIndex == 0)
            {
                this.udExtraction.Filter = AntdUI.Localization.Get("System.Charles", "Charles 会话文件") + "（*.chlsx）|*.chlsx";
            }
            else if (this.ddlExtraction.SelectedIndex == 1)
            {
                this.udExtraction.Filter = AntdUI.Localization.Get("System.FILT", "FILT 过滤器文件") + "（*.filt）|*.filt";
            }
        }

        private void udExtraction_DragChanged(object sender, StringsEventArgs e)
        {
            try
            {
                string FilePath = e.Value[0];

                if (!string.IsNullOrEmpty(FilePath))
                {
                    if (File.Exists(FilePath))
                    {
                        switch (this.ddlExtraction.SelectedIndex)
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
                                            this.txtExtraction.Text = BitConverter.ToString(bBody).Replace("-", " ");
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
                                            string sFAppointHeader = Operate.SystemConfig.GetBoolFromChineseString(s4).ToString();
                                            string sFHeaderContent = s5;
                                            string sFAppointSocket = Operate.SystemConfig.GetBoolFromChineseString(s2).ToString();
                                            string sFSocketContent = s3;
                                            string sFAppointLength = Operate.SystemConfig.GetBoolFromChineseString(s0).ToString();
                                            string sFLengthContent = s1;

                                            Operate.FilterConfig.Filter.FilterMode FMode = new Operate.FilterConfig.Filter.FilterMode();
                                            if (Operate.SystemConfig.GetBoolFromChineseString(s22) == true)
                                            {
                                                FMode = Operate.FilterConfig.Filter.FilterMode.Normal;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s23) == true)
                                            {
                                                FMode = Operate.FilterConfig.Filter.FilterMode.Advanced;
                                            }
                                            string sFMode = ((int)FMode).ToString();

                                            Operate.FilterConfig.Filter.FilterAction FAction = new Operate.FilterConfig.Filter.FilterAction();
                                            if (Operate.SystemConfig.GetBoolFromChineseString(s9) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.Replace;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s10) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.Intercept;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s11) == true)
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

                                            Operate.FilterConfig.Filter.FilterFunction filterFunction = 
                                                new Operate.FilterConfig.Filter.FilterFunction(
                                                    bSend, 
                                                    bSendTo, 
                                                    bRecv, 
                                                    bRecvFrom, 
                                                    bWSASend, 
                                                    bWSASendTo, 
                                                    bWSARecv, 
                                                    bWSARecvFrom,
                                                    false,
                                                    false,
                                                    false,
                                                    false);
                                            string sFFunction = Operate.FilterConfig.Filter.GetFilterFunctionString(filterFunction);

                                            Operate.FilterConfig.Filter.FilterStartFrom FStartFrom = new Operate.FilterConfig.Filter.FilterStartFrom();
                                            if (Operate.SystemConfig.GetBoolFromChineseString(s24) == true)
                                            {
                                                FStartFrom = Operate.FilterConfig.Filter.FilterStartFrom.Head;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s25) == true)
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
                                                sFProgressionPosition = Operate.SystemConfig.ConvertFILTString(s31, false);
                                                sFSearch = Operate.SystemConfig.ConvertFILTString(s26, false);
                                                sFModify = Operate.SystemConfig.ConvertFILTString(s27, false);
                                            }
                                            else if (FMode == Operate.FilterConfig.Filter.FilterMode.Advanced)
                                            {
                                                sFProgressionPosition = Operate.SystemConfig.ConvertFILTString(s32, false);
                                                sFSearch = Operate.SystemConfig.ConvertFILTString(s28, false);

                                                if (FStartFrom == Operate.FilterConfig.Filter.FilterStartFrom.Position)
                                                {
                                                    sFModify = Operate.SystemConfig.ConvertFILTString(s29, true);
                                                }
                                                else
                                                {
                                                    sFModify = Operate.SystemConfig.ConvertFILTString(s29, false);
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

                                this.txtExtraction.Text = xdoc_Filt.Declaration.ToString() + "\r\n" + xdoc_Filt.ToString();

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

        private void bExtraction_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileContent = this.txtExtraction.Text.Trim();
                if (string.IsNullOrEmpty(sFileContent))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "提取数据为空", TType.Error)
                    {
                        LocalizationText = "System.Extraction.Empty"
                    });

                    return;
                }

                SaveFileDialog sfdExtraction = new SaveFileDialog();

                switch (this.ddlExtraction.SelectedIndex)
                {
                    case 0:

                        sfdExtraction.Filter = "TXT（*.txt）|*.txt";

                        break;

                    case 1:

                        sfdExtraction.Filter = AntdUI.Localization.Get("FilterListFile", "滤镜列表文件") + "（*.fp）|*.fp";

                        break;
                }

                if (sfdExtraction.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = sfdExtraction.FileName;
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        File.WriteAllText(FilePath, sFileContent);

                        string Title = AntdUI.Localization.Get("System.Extraction.Success", "数据提取成功");
                        AntdUI.Notification.success(this, Title, FilePath, AntdUI.TAlignFrom.TR);
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
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

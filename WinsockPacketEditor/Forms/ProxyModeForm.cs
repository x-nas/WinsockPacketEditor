using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinsockPacketEditor
{
    public partial class ProxyModeForm : Window, InterfaceInfo.IProxyMode
    {
        private string TextA = string.Empty;
        private string TextB = string.Empty;
        private bool setcolor = false;
        private bool SearchFromHead = true;
        private static Socket ProxyServer;
        private BindingList<AccountInfo> lstAccount;
        private AntdUI.FormFloatButton FloatButton = null;

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
                this.InitTable_AccountList();
                this.InitTable_AuthList();
                this.InitTable_FilterList();
                this.InitTable_SendList();
                this.InitTable_RobotList();
                this.InitTable_LogList();

            }, () =>
            {
                this.pageHeader.Loading = false;                
            });

            Operate.SystemConfig.MainHandle = this.Handle;

            this.Dark_Changed();
            this.InitForm();
            this.InitComparison();
            this.InitExtraction();
            this.InitHotKeys();

            this.hbXOR_From.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbXOR_To.ByteProvider = new DynamicByteProvider(new byte[0]);
            this.hbProxyData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.hbXOR_From.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.hbXOR_To.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.tabProxyMode.TabMenuVisible = false;
            this.mProxyMode.SelectIndex(0, true);
        }

        private void ProxyModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Operate.ProxyConfig.Proxy.Enable_SystemProxy)
            {
                Operate.ProxyConfig.Proxy.Enable_SystemProxy = false;
                Operate.ProxyConfig.Proxy.StopSystemProxy(this);
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

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");
            this.pageHeader.Text = "Winsock Packet Editor";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;

            this.mProxyMode.Collapsed = true;
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
            this.tFilterList.Refresh();
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
            Operate.ProxyConfig.Account.NeedSave = true;
            this.tAccountList.Binding(GetPageData(this.pAccountList.Current, this.pAccountList.PageSize));
        }

        public void RefreshSendList()
        {
            this.tSendList.Refresh();
        }

        public void RefreshRobotList()
        {
            this.tRobotList.Refresh();
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
                }.SetFixed().SetLocalizationTitleID("Table.ProxyList.Column."),
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
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketSocket", "套接字", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("ClientAddr", "客户端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetFlagSVG(pi.ClientLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ClientLocation", "所属地").SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("ServerAddr", "服务端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetFlagSVG(pi.ServerLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ServerLocation", "所属地").SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketData", "数据").SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tProxyList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tProxyList.DataSource = Operate.ProxyConfig.List.lstProxyInfo;
        }

        private void InitTable_AccountList()
        {
            tAccountList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsCheck").SetFixed(),
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((this.pAccountList.Current - 1) * this.pAccountList.PageSize + rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("UserName", "用户名").SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("IsOnLine", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.IsOnLine)
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Success, "在线");
                            }
                            else
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Error, "离线");
                            }
                        }

                        return value;
                    },
                }.SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),                
                new AntdUI.Column("LimitLinks", "链接数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.IsLimitLinks)
                            {
                                return new AntdUI.CellTag(ai.LimitLinks.ToString(), AntdUI.TTypeMini.Info);
                            }
                            else
                            {
                                return new AntdUI.CellTag(AntdUI.Localization.Get("Unlimited", "无限制"), AntdUI.TTypeMini.Success);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("LimitDevices", "设备数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.IsLimitDevices)
                            {
                                return new AntdUI.CellTag(ai.LimitDevices.ToString(), AntdUI.TTypeMini.Info);
                            }
                            else
                            {
                                return new AntdUI.CellTag(AntdUI.Localization.Get("Unlimited", "无限制"), AntdUI.TTypeMini.Success);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("ExpiryTime", "过期时间").SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),                
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bLocation", null, AntdUI.TTypeMini.Warn).SetIcon("EnvironmentOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.AccountList.Column."),
            };

            this.tAccountList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));            
            this.pAccountList.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };
            this.tAccountList.Binding(GetPageData(this.pAccountList.Current, this.pAccountList.PageSize));
        }

        private void InitTable_AuthList()
        {
            tAuthList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("AuthTime", "认证时间").SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AID", "账号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.ProxyConfig.Account.GetUserName_ByAccountID((Guid)value);
                    },
                }.SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AuthIP", "IP地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AuthInfo ai)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetFlagSVG(ai.IPLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.AuthList.Column."),                
                new AntdUI.Column("LinksNumber", "链接数", AntdUI.ColumnAlign.Center).SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("DevicesNumber", "设备数", AntdUI.ColumnAlign.Center).SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AuthResult", "认证结果", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if((bool)value)
                        {
                            return new CellTag("通过", TTypeMini.Success);
                        }
                        else
                        {
                            return new CellTag("失败", TTypeMini.Error);
                        }
                    },
                }.SetLocalizationTitleID("Table.AuthList.Column."),
            };

            this.tAuthList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tAuthList.DataSource = Operate.ProxyConfig.Account.cdAuthInfo.Values;
        }

        private void InitTable_FilterList()
        {
            tFilterList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "80",
                    Call = (value, record, i_row, i_col) =>
                    {
                        System.Threading.Thread.Sleep(500);
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("FName", "滤镜名称").SetLocalizationTitleID("Table.FilterList.Column."),
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
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FAction", "动作", AntdUI.ColumnAlign.Center)
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
                                return AntdUI.Localization.Get("FilterAction.NoModify_NoDisplay", "不修改不显示");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                                return AntdUI.Localization.Get("FilterAction.NoModify_Display", "不修改只显示");

                            default:
                                return value;
                        }
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("Appoint", "指定类型")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            List<CellTag> ctList = new List<CellTag>();

                            if(fi.AppointHeader)
                            {
                                ctList.Add(new AntdUI.CellTag("包头", AntdUI.TTypeMini.Success));
                            }

                            if(fi.AppointSocket)
                            {
                                ctList.Add(new AntdUI.CellTag("套接字", AntdUI.TTypeMini.Warn));
                            }

                            if(fi.AppointPort)
                            {
                                ctList.Add(new AntdUI.CellTag("端口", AntdUI.TTypeMini.Default));
                            }

                            if(fi.AppointLength)
                            {
                                ctList.Add(new AntdUI.CellTag("长度", AntdUI.TTypeMini.Primary));
                            }

                            if(ctList.Count > 0)
                            {
                                AntdUI.CellTag[] cellTags = new AntdUI.CellTag[ctList.Count];
                                for(int i = 0; i < ctList.Count; i++)
                                {
                                    cellTags[i] = ctList[i];
                                }

                                return cellTags;
                            }
                            else
                            {
                                return null;
                            }
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("Progression", "递进")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            List<CellTag> ctList = new List<CellTag>();

                            if(!string.IsNullOrEmpty(fi.ProgressionPosition))
                            {
                                ctList.Add(new AntdUI.CellTag("启用", AntdUI.TTypeMini.Error));
                            }

                            if(fi.IsProgressionContinuous)
                            {
                                ctList.Add(new AntdUI.CellTag("连续", AntdUI.TTypeMini.Success));
                            }

                            if(fi.IsProgressionCarry)
                            {
                                ctList.Add(new AntdUI.CellTag("进位", AntdUI.TTypeMini.Warn));
                            }

                            if(ctList.Count > 0)
                            {
                                AntdUI.CellTag[] cellTags = new AntdUI.CellTag[ctList.Count];
                                for(int i = 0; i < ctList.Count; i++)
                                {
                                    cellTags[i] = ctList[i];
                                }

                                return cellTags;
                            }
                            else
                            {
                                return null;
                            }
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };

            this.tFilterList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tFilterList.Binding(Operate.FilterConfig.List.lstFilterInfo);
        }

        private void InitTable_SendList()
        {
            tSendList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "80",
                    Call = (value, record, i_row, i_col) =>
                    {
                        System.Threading.Thread.Sleep(500);
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SName", "发送名称").SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is SendInfo si)
                        {
                            AntdUI.CellBadge cellBadge = null;

                            if(si.IsEnable)
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Success, "启用");
                                if(si.ExecutionCount > 0)
                                {
                                    cellBadge = new AntdUI.CellBadge(AntdUI.TState.Processing, "处理中");
                                }
                            }
                            else
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Error, "停止");
                            }

                            return cellBadge;
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("ExecutionSuccess", "成功次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.Green,
                        };
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("ExecutionFail", "失败次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.Red,
                        };
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SSystemSocket", "套接字", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if((bool)value)
                        {
                            return new CellTag(Operate.SystemConfig.SystemSocket.ToString(), TTypeMini.Error);
                        }
                        else
                        {
                            return new CellTag(AntdUI.Localization.Get("System.SystemSocket", "自定义"), TTypeMini.Success);
                        }
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SLoopCNT", "循环")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is SendInfo si)
                        {
                            return new CellTag[]
                            {
                                new CellTag(si.SLoopCNT.ToString() + AntdUI.Localization.Get("System.LoopCNT", " 次"), TTypeMini.Success),
                                new CellTag(AntdUI.Localization.Get("System.LoopINT", "间隔 ") + si.SLoopINT.ToString() + AntdUI.Localization.Get("System.Millisecond", " 毫秒"), TTypeMini.Warn)
                            };
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new Column("SNotes", "备注")
                {
                    LineBreak = true,
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.SendList.Column."),
            };

            this.tSendList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSendList.Binding(Operate.SendConfig.List.lstSendInfo);
        }

        private void InitTable_RobotList()
        {
            tRobotList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "80",
                    Call = (value, record, i_row, i_col) =>
                    {
                        System.Threading.Thread.Sleep(500);
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("RName", "机器人名称").SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is RobotInfo ri)
                        {
                            AntdUI.CellBadge cellBadge = null;

                            if(ri.IsEnable)
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Success, "启用");
                                if(ri.ExecutionCount > 0)
                                {
                                    cellBadge = new AntdUI.CellBadge(AntdUI.TState.Processing, "处理中");
                                }
                            }
                            else
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Error, "停止");
                            }

                            return cellBadge;
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.RobotList.Column."),
            };

            this.tRobotList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tRobotList.Binding(Operate.RobotConfig.List.lstRobotInfo);
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

        private void InitCalendar_ExpiryTime()
        {
            try
            {
                Dictionary<string, int> TimeCounts = new Dictionary<string, int>();

                foreach (AccountInfo ai in Operate.ProxyConfig.Account.lstAccountInfo)
                {
                    string ExpiryTime = ai.ExpiryTime.ToString("yyyy-MM-dd");

                    if (TimeCounts.ContainsKey(ExpiryTime))
                    {
                        TimeCounts[ExpiryTime]++;
                    }
                    else
                    {
                        TimeCounts.Add(ExpiryTime, 1);
                    }
                }

                dtpExpiryTime.BadgeAction = dates =>
                {
                    List<AntdUI.DateBadge> dbList = new List<AntdUI.DateBadge>();
                    foreach (var kvp in TimeCounts)
                    {
                        dbList.Add(new AntdUI.DateBadge(kvp.Key, kvp.Value));
                    }

                    return dbList;
                };
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private BindingList<AccountInfo> GetPageData(int current, int pageSize)
        {
            var list = new BindingList<AccountInfo>();

            try
            {
                if (this.lstAccount == null)
                {
                    this.lstAccount = Operate.ProxyConfig.Account.lstAccountInfo;
                }

                this.pAccountList.Total = this.lstAccount.Count;                
                int start = Math.Abs(current - 1) * pageSize;

                for (int i = start; i < this.lstAccount.Count && i < start + pageSize; i++)
                {
                    list.Add(this.lstAccount[i]);
                }

                this.InitCalendar_ExpiryTime();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return list;
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

        private void pAccountList_ValueChanged(object sender, PagePageEventArgs e)
        {
            this.tAccountList.Binding(GetPageData(e.Current, e.PageSize));
        }

        private string pAccountList_ShowTotalChanged(object sender, PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}条 {e.PageTotal}页";
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
                BackColor = Color.FromArgb(30, 30, 30);
                ForeColor = Color.White;

                this.tProxyList.ColumnFore = Color.Silver;
                this.tProxyList.ForeColor = Color.LimeGreen;

                this.hbProxyData.BackColor =
                    this.hbXOR_From.BackColor =
                    this.hbXOR_To.BackColor = 
                    Color.FromArgb(30, 30, 30);

                this.hbProxyData.ForeColor =
                    this.hbXOR_From.ForeColor =
                    this.hbXOR_To.ForeColor = 
                    Color.Silver;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;

                this.tProxyList.ColumnFore = Color.Black;
                this.tProxyList.ForeColor = Color.Green;

                this.hbProxyData.BackColor =
                    this.hbXOR_From.BackColor =
                    this.hbXOR_To.BackColor = 
                    Color.White;

                this.hbProxyData.ForeColor =
                    this.hbXOR_From.ForeColor =
                    this.hbXOR_To.ForeColor = 
                    Color.Black;
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
                this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");

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

                if (!this.bgwClientList.IsBusy)
                {
                    this.treeClientList.PauseLayout = true;
                    this.bgwClientList.RunWorkerAsync();
                }

                if (Operate.ProxyConfig.Account.NeedSave && !this.bgwAccountList.IsBusy)
                {
                    Operate.ProxyConfig.Account.NeedSave = false;
                    this.bgwAccountList.RunWorkerAsync();
                }

                this.mProxyMode.Items[0].Badge = Operate.ProxyConfig.List.lstProxyInfo.Count.ToString();
                this.mProxyMode.Items[1].Badge = this.treeClientList.Items.Count().ToString();
                this.mProxyMode.Items[2].Badge = this.lstAccount.Count.ToString();
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

        private void bgwProxyList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tAuthList.DataSource = Operate.ProxyConfig.Account.cdAuthInfo.Values;
                this.tProxyList.Refresh();
                this.tSystemLog.Refresh();                

                ulong ProxyTotal_CNT =
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
                this.lAuthCount_Value.Text = Operate.ProxyConfig.Account.cdAuthInfo.Count.ToString();
                this.lLinksCount_Value.Text = Operate.ProxyConfig.Account.GetLinksCount_FromAuthList().ToString();
                this.lDevicesCount_Value.Text = Operate.ProxyConfig.Account.GetDevicesCount_FromAuthList().ToString();

                Operate.ProxyConfig.Proxy.ProxyOnLineInfo = string.Format(
                        "{0}/{1}",
                        Operate.ProxyConfig.Account.GetOnLineProxyAccountCount(Operate.ProxyConfig.Account.lstAccountInfo),
                        Operate.ProxyConfig.Account.lstAccountInfo.Count);
                this.lProxyAccount_CNT.Text = Operate.ProxyConfig.Proxy.ProxyOnLineInfo;

                Operate.ProxyConfig.Proxy.ProxyBytesInfo = string.Format(
                    AntdUI.Localization.Get("ProxyBytesInfo", "请求: {0}  响应: {1}"),
                    Operate.SystemConfig.GetDisplayBytes(Operate.ProxyConfig.Proxy.Total_Request),
                    Operate.SystemConfig.GetDisplayBytes(Operate.ProxyConfig.Proxy.Total_Response));
                this.lTotalBytes.Text = Operate.ProxyConfig.Proxy.ProxyBytesInfo;

                decimal dUplink = Operate.ProxyConfig.Proxy.ProxySpeed_Uplink / 1024;
                Operate.ProxyConfig.Proxy.ProxySpeed_Uplink = 0;
                decimal dDownlink = Operate.ProxyConfig.Proxy.ProxySpeed_Downlink / 1024;
                Operate.ProxyConfig.Proxy.ProxySpeed_Downlink = 0;

                Operate.ProxyConfig.Proxy.ProxySpeedInfo = string.Format(
                    AntdUI.Localization.Get("ProxySpeedInfo", "上行: {0} KB/s  下行: {1} KB/s"),
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

        #region//显示客户端列表（异步）

        private void bgwClientList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var peList = Operate.ProxyConfig.List.lstProxyTCP.ToList();
                if (peList == null || peList.Count == 0)
                {
                    return;
                }

                foreach (ProxyTCP pe in peList)
                {
                    if (pe == null)
                    {
                        continue;
                    }

                    if (pe.TCP_Client == null)
                    {
                        Operate.ProxyConfig.List.lstProxyTCP.Remove(pe);
                        pe?.Dispose();
                        continue;
                    }

                    #region//更新客户端链接

                    if (pe.CommandType != Operate.ProxyConfig.Proxy.CommandType.Bind)
                    {
                        string ClientIP = Operate.ProxyConfig.Proxy.GetClientIPAddress(pe);
                        string ClientUserName = Operate.ProxyConfig.Account.GetUserName_ByAccountID(pe.AID);
                        string sRootName = Operate.ProxyConfig.Proxy.GetClientListName(ClientIP, ClientUserName);

                        if (string.IsNullOrEmpty(sRootName))
                        {
                            return;
                        }

                        AntdUI.TreeItem tiRoot = Operate.SystemConfig.FindNodeByName(this.treeClientList, sRootName);
                        if (tiRoot == null)
                        {
                            tiRoot = new TreeItem(sRootName)
                            {
                                IconSvg = "DesktopOutlined",
                            };
                            this.treeClientList.Items.Add(tiRoot);
                        }

                        string sChildName = pe.TCP_Client.Address;
                        if (string.IsNullOrEmpty(sChildName))
                        {
                            return;
                        }

                        AntdUI.TreeItem tiChild = Operate.SystemConfig.FindNodeByName(this.treeClientList, sChildName);
                        if (tiChild == null)
                        {
                            tiChild = new TreeItem(sChildName);
                            switch (pe.DomainType)
                            {
                                case Operate.ProxyConfig.Proxy.DomainType.Http:
                                    tiChild.IconSvg = "IeOutlined";
                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.Https:
                                    tiChild.IconSvg = "LockOutlined";
                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.Socket:
                                    tiChild.IconSvg = "ApiOutlined";
                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.External:

                                    break;
                            }
                            tiRoot.Sub.Add(tiChild);
                        }
                    }

                    #endregion

                    if (pe.TCP_Client.Socket == null)
                    {
                        #region//移除关闭的客户端链接                    

                        string ClientIP = Operate.ProxyConfig.Proxy.GetClientIPAddress(pe);
                        string ClientUserName = Operate.ProxyConfig.Account.GetUserName_ByAccountID(pe.AID);

                        if (string.IsNullOrEmpty(ClientUserName))
                        {
                            TreeItem tiChild = Operate.SystemConfig.FindNodeByName(this.treeClientList, pe.TCP_Client.Address);
                            if (tiChild != null)
                            {
                                this.treeClientList.Items.Remove(tiChild);
                            }
                            Operate.ProxyConfig.List.lstProxyTCP.Remove(pe);
                            pe?.Dispose();
                        }
                        else
                        {
                            string sRootName = Operate.ProxyConfig.Proxy.GetClientListName(ClientIP, ClientUserName);
                            if (string.IsNullOrEmpty(sRootName))
                            {
                                return;
                            }

                            TreeItem tiRoot = Operate.SystemConfig.FindNodeByName(this.treeClientList, sRootName);
                            if (tiRoot == null)
                            {
                                return;
                            }

                            TreeItem tiChild = Operate.SystemConfig.FindNodeByName(tiRoot.Sub, pe.TCP_Client.Address);
                            if (tiChild != null)
                            {
                                tiRoot.Sub.Remove(tiChild);                                
                            }                            

                            if (tiRoot.Sub.Count == 0)
                            {
                                this.treeClientList.Items.Remove(tiRoot);
                                Operate.ProxyConfig.Account.DeleteProxyAuthInfo_ByAIDAndIP(pe.AID, ClientIP);
                                  
                                if (pe.AID != null && pe.AID != Guid.Empty)
                                {
                                    Operate.ProxyConfig.Account.SetOnline_ByAccountID(pe.AID, false);
                                }
                            }

                            Operate.ProxyConfig.List.lstProxyTCP.Remove(pe);
                            pe?.Dispose();
                        }

                        #endregion
                    }
                }                

                foreach (AuthInfo ai in Operate.ProxyConfig.Account.cdAuthInfo.Values)
                {
                    string clientIP = ai.AuthIP.ToString();
                    ai.LinksNumber = Operate.ProxyConfig.Account.GetLinksNumber_ByAccountID(ai.AID, clientIP, this.treeClientList);
                    ai.DevicesNumber = Operate.ProxyConfig.Account.GetDevicesNumber_ByAccountID(ai.AID);

                    var key = (ai.AID, ai.AuthIP);
                    Operate.ProxyConfig.Account.cdAuthInfo.AddOrUpdate(
                        key,
                        ai,
                        (_, existing) => ai
                    );
                }

                Operate.ProxyConfig.Proxy.CheckUDPTimeOut();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwClientList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.treeClientList.PauseLayout = false;
                this.treeClientList.Refresh();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存账号列表（异步）

        private void bgwAccountList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Operate.ProxyConfig.Account.SaveAccountList_ToDB();
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
                this.bMenuCollapse.IconSvg = "MenuUnfoldOutlined";
            }
            else
            {
                this.mProxyMode.Width = this.tlpMenu.Width = this.mProxyMode.CollapsedWidth;
                this.bMenuCollapse.IconSvg = "MenuFoldOutlined";
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
            this.CleanUp_LogList();

            AntdUI.Message.open(new AntdUI.Message.Config(this, "已清空数据", TType.Warn)
            {
                LocalizationText = "InjectModeForm.Clear"
            });
        }

        private void mProxyList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mProxyList.SelectIndex(-1);

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

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ProxySettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
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

                case "miMapSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new MapSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miExternalProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ExternalProxySettingsForm())
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
                                    LocalizationText = "SYSSocket.Success"
                                });
                            }

                            break;

                        case "PacketModification":

                            if (piList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketModificationForm(this, null, piList[0]))
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

                            if (this.hbProxyData.CanCopy())
                            {
                                this.hbProxyData.CopyHex();
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

                            if (this.hbProxyData.CanCopy())
                            {
                                this.hbProxyData.CopyHex();
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
                }, Operate.PacketConfig.Packet.GetCMS_PacketData(this.hbProxyData)));
            }
        }

        #endregion

        #region//账号列表 - 菜单

        private void mAccountList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mAccountList.SelectIndex(-1);            

            switch (miSelect.ID)
            {
                case "miAdd":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new AccountEditForm(this, null))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miImport":

                    Operate.ProxyConfig.Account.LoadAccountList_Dialog(this);

                    break;

                case "miExport":

                    if (Operate.ProxyConfig.Account.lstAccountInfo.Count > 0)
                    {
                        Operate.ProxyConfig.Account.SaveAccount_Dialog(this, string.Empty, null);
                    }

                    break;

                case "miClear":

                    if (Operate.ProxyConfig.Account.lstAccountInfo.Count > 0)
                    {
                        Operate.ProxyConfig.Account.DeleteAccount_Dialog(this, null);
                    }                    

                    break;
            }            
        }

        private void tAccountList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is AccountInfo ai)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new AccountEditForm(this, ai))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bLocation":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new LocationForm(this, ai))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bDelete":

                        List<AccountInfo> aiList = new List<AccountInfo>
                        {
                            ai
                        };

                        Operate.ProxyConfig.Account.DeleteAccount_Dialog(this, aiList);

                        break;
                }
            }
        }

        #endregion

        #region//账号列表 - 右键菜单

        private void tAccountList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.Account.lstAccountInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tAccountList, (item) =>
                {
                    List<AccountInfo> aiList = new List<AccountInfo>();
                    foreach (AccountInfo ai in Operate.ProxyConfig.Account.lstAccountInfo)
                    {
                        if (ai.IsCheck)
                        {
                            aiList.Add(ai);
                        }
                    }

                    switch (item.ID)
                    {
                        case "ExpiryTime":

                            if (aiList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ExpiryTimeForm(this, aiList))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "LimitLinks":

                            if (aiList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new LimitLinksForm(this, aiList))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "LimitDevices":

                            if (aiList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new LimitDevicesForm(this, aiList))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "Export":

                            if (aiList.Count > 0)
                            {
                                Operate.ProxyConfig.Account.SaveAccount_Dialog(this, string.Empty, aiList);
                            }

                            break;

                        case "Delete":

                            if (aiList.Count > 0)
                            {
                                Operate.ProxyConfig.Account.DeleteAccount_Dialog(this, aiList);
                            }

                            break;                     
                    }

                    this.tAccountList.SelectedIndex = -1;
                }, Operate.ProxyConfig.Account.GetCMS_AccountList()));
            }
        }

        #endregion

        #region//账号列表 - 搜索

        private void bSearchExpiryTime_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dtStart = this.dtpExpiryTime.Value[0];
                DateTime dtEnd = this.dtpExpiryTime.Value[1];

                this.lstAccount = Operate.ProxyConfig.Account.GetProxyAccount_ByExpireTime(dtStart, dtEnd);
                this.RefreshAccountList();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void txtSearchUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && sender is Input input)
            {
                e.Handled = true;

                string UserName = this.txtSearchUserName.Text.Trim();

                if (string.IsNullOrEmpty(UserName))
                {
                    this.lstAccount = Operate.ProxyConfig.Account.lstAccountInfo;
                }
                else
                { 
                    this.lstAccount = Operate.ProxyConfig.Account.GetAccount_ByUserName(UserName);
                }

                this.RefreshAccountList();
            }
        }




        #endregion

        #region//滤镜列表 - 菜单

        private void bFilterList_Reset_Click(object sender, EventArgs e)
        {
            Operate.FilterConfig.List.InitFilterList_Count();
        }

        private void mFilterList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mFilterList.SelectIndex(-1);

            switch (miSelect.ID)
            {
                case "miAdd":

                    Operate.FilterConfig.Filter.AddFilter_New();
                    this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;

                    break;

                case "miImport":

                    Operate.FilterConfig.List.LoadFilterList_Dialog(this);

                    break;

                case "miExport":

                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        Operate.FilterConfig.List.SaveFilterList_Dialog(this, string.Empty, null);
                    }

                    break;

                case "miClear":

                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        Operate.FilterConfig.List.CleanUpFilterList_Dialog(this);
                    }

                    break;
            }
        }

        private void tFilterList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is FilterInfo fi)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new FilterEditForm(this, fi))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bDelete":

                        List<FilterInfo> fiList = new List<FilterInfo>();
                        fiList.Add(fi);
                        Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, fiList);

                        break;
                }
            }
        }

        #endregion

        #region//滤镜列表 - 右键菜单

        private void tFilterList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.FilterConfig.List.lstFilterInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tFilterList, (item) =>
                {
                    List<FilterInfo> fiList = new List<FilterInfo>();

                    foreach (int SelectIndex in this.tFilterList.SelectedIndexs)
                    {
                        fiList.Add(Operate.FilterConfig.List.lstFilterInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Top, fiList);
                            }

                            break;

                        case "Up":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Up, fiList);
                            }

                            break;

                        case "Down":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Down, fiList);
                            }

                            break;

                        case "Bottom":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Bottom, fiList);
                            }

                            break;

                        case "Copy":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Copy, fiList);
                                this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                            }

                            break;

                        case "Export":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Export, fiList);
                            }

                            break;

                        case "Delete":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, fiList);
                            }

                            break;
                    }

                    this.tFilterList.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
            }
        }

        #endregion

        #region//发送列表 - 菜单

        private void bSendList_Start_Click(object sender, EventArgs e)
        {
            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
            {
                if (!this.bgwSendList.IsBusy)
                {
                    this.bSendList_Start.Loading = true;
                    this.bSendList_Stop.Enabled = true;
                    this.tSendList.Enabled = false;

                    Operate.SendConfig.List.lstSendExecute.Clear();

                    this.bgwSendList.RunWorkerAsync();
                }
            }
        }

        private void bSendList_Stop_Click(object sender, EventArgs e)
        {
            this.bgwSendList.CancelAsync();
        }

        private void mSendList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mSendList.SelectIndex(-1);

            switch (miSelect.ID)
            {
                case "miAdd":

                    Operate.SendConfig.Send.AddSend_New();
                    this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;

                    break;

                case "miImport":

                    Operate.SendConfig.List.LoadSendList_Dialog(this);

                    break;

                case "miExport":

                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        Operate.SendConfig.List.SaveSendList_Dialog(this, string.Empty, null);
                    }

                    break;

                case "miClear":

                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        Operate.SendConfig.List.CleanUpSendList_Dialog(this);
                    }

                    break;
            }
        }

        private void tSendList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is SendInfo si)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SendEditForm(this, si))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bDelete":

                        List<SendInfo> siList = new List<SendInfo>
                        {
                            si
                        };

                        Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, siList);

                        break;
                }
            }
        }

        #endregion

        #region//发送列表 - 右键菜单

        private void tSendList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.SendConfig.List.lstSendInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tSendList, (item) =>
                {
                    List<SendInfo> siList = new List<SendInfo>();

                    foreach (int SelectIndex in this.tSendList.SelectedIndexs)
                    {
                        siList.Add(Operate.SendConfig.List.lstSendInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Top, siList);
                            }

                            break;

                        case "Up":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Up, siList);
                            }

                            break;

                        case "Down":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Down, siList);
                            }

                            break;

                        case "Bottom":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Bottom, siList);
                            }

                            break;

                        case "Copy":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Copy, siList);
                                this.tSendList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                            }

                            break;

                        case "Export":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Export, siList);
                            }

                            break;

                        case "Delete":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, siList);
                            }

                            break;
                    }

                    this.tSendList.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
            }
        }

        #endregion

        #region//机器人列表 - 菜单

        private void bRobotList_Start_Click(object sender, EventArgs e)
        {
            if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
            {
                if (!this.bgwRobotList.IsBusy)
                {
                    this.bRobotList_Start.Loading = true;
                    this.bRobotList_Stop.Enabled = true;
                    this.tRobotList.Enabled = false;

                    Operate.RobotConfig.List.lstRobotExecute.Clear();

                    this.bgwRobotList.RunWorkerAsync();
                }
            }
        }

        private void bRobotList_Stop_Click(object sender, EventArgs e)
        {
            this.bgwRobotList.CancelAsync();
        }

        private void mRobotList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mRobotList.SelectIndex(-1);

            switch (miSelect.ID)
            {
                case "miAdd":

                    Operate.RobotConfig.Robot.AddRobot_New();
                    this.tRobotList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;

                    break;

                case "miImport":

                    Operate.RobotConfig.List.LoadRobotList_Dialog(this);

                    break;

                case "miExport":

                    if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                    {
                        Operate.RobotConfig.List.SaveRobotList_Dialog(this, string.Empty, null);
                    }

                    break;

                case "miClear":

                    if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                    {
                        Operate.RobotConfig.List.CleanUpRobotList_Dialog(this);
                    }

                    break;
            }
        }

        private void tRobotList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is RobotInfo ri)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new RobotEditForm(this, ri))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bDelete":

                        List<RobotInfo> riList = new List<RobotInfo>
                        {
                            ri,
                        };

                        Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, riList);

                        break;
                }
            }
        }

        #endregion

        #region//机器人列表 - 右键菜单

        private void tRobotList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.RobotConfig.List.lstRobotInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tRobotList, (item) =>
                {
                    List<RobotInfo> riList = new List<RobotInfo>();

                    foreach (int SelectIndex in this.tRobotList.SelectedIndexs)
                    {
                        riList.Add(Operate.RobotConfig.List.lstRobotInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Top, riList);
                            }

                            break;

                        case "Up":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Up, riList);
                            }

                            break;

                        case "Down":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Down, riList);
                            }

                            break;

                        case "Bottom":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Bottom, riList);
                            }

                            break;

                        case "Copy":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Copy, riList);
                                this.tRobotList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                            }

                            break;

                        case "Export":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Export, riList);
                            }

                            break;

                        case "Delete":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, riList);
                            }

                            break;
                    }

                    this.tRobotList.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
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

        #region//开始代理

        private void Start_Proxy()
        {
            try
            {
                Operate.ProxyConfig.Proxy.IsListening = true;

                if (Operate.ProxyConfig.Proxy.ProxyTCP_IP == null || Operate.ProxyConfig.Proxy.ProxyUDP_IP == null)
                {
                    Operate.ProxyConfig.Proxy.ProxyTCP_IP = IPAddress.Any;
                    Operate.ProxyConfig.Proxy.ProxyUDP_IP = Operate.ProxyConfig.Proxy.ProxyServerIP[0];
                }

                string sProxyIP = string.Format(AntdUI.Localization.Get("ProxyServerIP", "代理服务器IP地址: TCP [{0}] UDP [{1}]"), Operate.ProxyConfig.Proxy.ProxyTCP_IP, Operate.ProxyConfig.Proxy.ProxyUDP_IP);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sProxyIP);

                if (Operate.ProxyConfig.Proxy.Enable_Auth)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("ProxyServer.Auth", "已启用代理服务身份认证"));
                }

                if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("ProxyServer.ExternalProxy", "已启用外部 SOCKS5 代理"));
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

        #region//执行发送列表（异步）

        private void bgwSendList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                for (int index = 0; index < Operate.SendConfig.List.lstSendInfo.Count; index++)
                {
                    SendInfo si = Operate.SendConfig.List.lstSendInfo[index];
                    if (si.IsEnable)
                    {
                        SendExecute se = Operate.SendConfig.Send.DoSend(si.SID);
                        if (se != null)
                        {
                            if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                            {
                                Operate.SendConfig.List.lstSendExecute.Add(se);
                            }
                            else
                            {
                                while (se.Worker.IsBusy)
                                {
                                    if (this.bgwSendList.CancellationPending)
                                    {
                                        se.StopSend();

                                        e.Cancel = true;
                                        return;
                                    }

                                    Thread.Sleep(10);
                                }
                            }
                        }
                    }
                }

                while (Operate.SendConfig.List.lstSendExecute.Count > 0)
                {
                    foreach (SendExecute se in Operate.SendConfig.List.lstSendExecute.ToList())
                    {
                        if (this.bgwSendList.CancellationPending)
                        {
                            se.StopSend();
                        }

                        if (!se.Worker.IsBusy)
                        {
                            Operate.SendConfig.List.lstSendExecute.Remove(se);
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

        private void bgwSendList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.bSendList_Start.Loading = false;
            this.bSendList_Stop.Enabled = false;
            this.tSendList.Enabled = true;
        }

        #endregion

        #region//执行机器人列表（异步）

        private void bgwRobotList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
                {
                    if (ri.IsEnable)
                    {
                        RobotExecute re = Operate.RobotConfig.Robot.DoRobot(ri.RID, null);
                        if (re != null)
                        {
                            if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                            {
                                Operate.RobotConfig.List.lstRobotExecute.Add(re);
                            }
                            else
                            {
                                while (re.Worker.IsBusy)
                                {
                                    if (this.bgwRobotList.CancellationPending)
                                    {
                                        re.StopRobot();

                                        e.Cancel = true;
                                        return;
                                    }

                                    Thread.Sleep(100);
                                }
                            }
                        }
                    }
                }

                while (Operate.RobotConfig.List.lstRobotExecute.Count > 0)
                {
                    foreach (RobotExecute re in Operate.RobotConfig.List.lstRobotExecute.ToList())
                    {
                        if (this.bgwRobotList.CancellationPending)
                        {
                            re.StopRobot();
                        }

                        if (!re.Worker.IsBusy)
                        {
                            Operate.RobotConfig.List.lstRobotExecute.Remove(re);
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
            this.bRobotList_Start.Loading = false;
            this.bRobotList_Stop.Enabled = false;
            this.tRobotList.Enabled = true;
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

using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPE.Lib;
using WPE.Lib.Controls;

namespace WPE.ProxyMode
{
    public partial class ProxyModeForm : Window, Operate.IProxyMode
    {
        private bool StartProxy = true;
        private bool setcolor = false;
        private static Socket ProxyServer;
        private BindingList<AccountInfo> lstAccount;
        private AntdUI.FormFloatButton FloatButton = null;
        private readonly Operate.SystemConfig.SystemMode RunMode = Operate.SystemConfig.SystemMode.Proxy;

        #region//窗体事件

        public ProxyModeForm()
        {
            InitializeComponent();
        }

        private void ProxyModeForm_Load(object sender, EventArgs e)
        {
            Operate.SystemConfig.MainHandle = this.Handle;
            Operate.SystemConfig.InvokeAction = action =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(action);
                }
                else
                {
                    action();
                }
            };            

            this.pageHeader.Loading = true;
            AntdUI.Spin.open(this, AntdUI.Localization.Get("Loading", "正在加载..."), config =>
            {                
                //Operate.SystemConfig.InitCPUAndMemoryCounter();                
                Operate.SystemConfig.LoadProxyMode_FromDB();
                //Operate.SystemConfig.LoadSystemList_FromDB();
                Operate.ProxyConfig.Account.LoadProxyAccountList_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapLocal_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapRemote_FromDB();
                //Operate.SystemConfig.StartRemoteMGT();

                this.InitProxyServerIP();
                this.InitFloatButton();
                this.InitTable_ProxyList();
                this.InitTable_AccountList();
                this.InitTable_AuthList();
                this.InitTable_LogList();

            }, () =>
            {
                this.pageHeader.Loading = false;                
            });

            this.Dark_Changed();
            this.InitForm();                        

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

            Operate.SystemConfig.SaveSystemConfig_ToDB();
            Operate.SystemConfig.SaveProxyMode_ToDB();
            //Operate.SystemConfig.SaveSystemList_ToDB();            
            Operate.ProxyConfig.Account.SaveAccountList_ToDB(this.RunMode);
            Operate.ProxyConfig.Mapping.SaveMapLocal_ToDB(this.RunMode);
            Operate.ProxyConfig.Mapping.SaveMapRemote_ToDB(this.RunMode);
            Operate.SystemConfig.StopRemoteMGT(this.RunMode);
        }

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");
            this.pageHeader.Text = "Winsock Packet Editor";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;

            this.mProxyMode.Collapsed = true;
            this.MenuCollapseChange();

            btn_global.Items.AddRange(
                new AntdUI.ISelectItem[]
                {
                    new AntdUI.SelectItem("中文", "zh-CN"),
                    new AntdUI.SelectItem("English", "en-US")
                });

            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                btn_global.SelectedValue = btn_global.Items[1];
            }
            else
            {
                btn_global.SelectedValue = btn_global.Items[0];
            }

            for (int i = 0; i < this.mProxyMode.Items.Count; i++)
            {
                this.mProxyMode.Items[i].BadgeBack = this.colorTheme.Value;
            }            
        }

        public void InitFloatButton()
        {
            if (Operate.SystemConfig.IsShow_FloatButton)
            {
                if (FloatButton == null)
                {
                    FloatButton = AntdUI.FloatButton.open(new AntdUI.FloatButton.Config(this,
                        new AntdUI.FloatButton.ConfigBtn[]
                        {
                            new AntdUI.FloatButton.ConfigBtn("GitHub", "QuestionOutlined", true)
                    {
                        Tooltip = "问题反馈",
                        Type= AntdUI.TTypeMini.Success
                    },
                            new AntdUI.FloatButton.ConfigBtn("WebSite", "HomeOutlined", true)
                    {
                        Tooltip = "访问官网",
                        Type= AntdUI.TTypeMini.Default
                    }
                        }, btn =>
                        {
                            btn.Loading = true;

                            AntdUI.ITask.Run(() =>
                            {
                                switch (btn.Name)
                                {
                                    case "GitHub":
                                        Process.Start(Operate.SystemConfig.WPE64_Issuse);
                                        break;

                                    case "WebSite":
                                        Process.Start(Operate.SystemConfig.WPE64_URL);
                                        break;
                                }

                                btn.Loading = false;
                            });
                        }));
                }
                else
                {
                    FloatButton.Show();
                }
            }
            else
            {
                if (FloatButton != null)
                {
                    FloatButton.Close();
                    FloatButton = null;
                }
            }
        }

        private void InitProxyServerIP()
        {
            if (Operate.ProxyConfig.Proxy.ProxyServerIP == null)
            {
                Operate.ProxyConfig.Proxy.ProxyServerIP = Operate.SystemConfig.GetLocalIPAddress();
            }
        }

        public void RefreshAccountList()
        {
            this.tAccountList.Binding(GetPageData(this.pAccountList.Current, this.pAccountList.PageSize));
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
                            return new AntdUI.CellImage(Operate.ProxyConfig.Proxy.GetImg_ByDataType(pi.DataType));
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
                new AntdUI.Column("ProtocolType", "类别", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return value.ToString().ToUpper();
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("ClientIP", "客户端IP")
                {
                    Render = (value, record, rowindex)=>
                    {
                        IPEndPoint clientIP = value as IPEndPoint;
                        return clientIP.Address.ToString() + ":" + clientIP.Port.ToString();
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ServerIP", "服务端IP")
                {
                    Render = (value, record, rowindex)=>
                    {
                        IPEndPoint serverIP = value as IPEndPoint;
                        return serverIP.Address.ToString() + ":" + serverIP.Port.ToString();
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
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
                new AntdUI.Column("LoginTime", "登录时间")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.LoginTime == DateTime.MinValue)
                            {
                                return null;
                            }
                            else
                            {
                                return ai.LoginTime;
                            }
                        }

                        return value;
                    },
                }.SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("LoginIP", "登录IP").SetLocalizationTitleID("Table.AccountList.Column."),                
                new AntdUI.Column("IPLocation", "IP所属地").SetLocalizationTitleID("Table.AccountList.Column."),                
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
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AuthTime", "认证时间").SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AuthIP", "IP地址").SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AID", "账号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.ProxyConfig.Account.GetUserName_ByAccountID((Guid)value);
                    },
                }.SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("LinksNumber", "链接数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("DevicesNumber", "设备数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.AuthList.Column."),
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
            this.tAuthList.Binding(Operate.ProxyConfig.Account.lstAuthInfo);
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

        private BindingList<AccountInfo> GetPageData(int current, int pageSize)
        {
            if (this.lstAccount == null)
            {
                this.lstAccount = Operate.ProxyConfig.Account.lstAccountInfo;
            }

            this.pAccountList.Total = this.lstAccount.Count;

            var list = new BindingList<AccountInfo>();
            int start = Math.Abs(current - 1) * pageSize;

            for (int i = start; i < this.lstAccount.Count && i < start + pageSize; i++)
            {
                list.Add(this.lstAccount[i]);
            }

            this.InitCalendar_ExpiryTime();

            return list;
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

                this.hbProxyData.BackColor = Color.FromArgb(30, 30, 30);
                this.hbProxyData.ForeColor = Color.Silver;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;

                this.tProxyList.ColumnFore = Color.Black;
                this.tProxyList.ForeColor = Color.Green;

                this.hbProxyData.BackColor = Color.White;
                this.hbProxyData.ForeColor = Color.Black;
            }
        }        

        #endregion

        #region//切换语言

        private void btn_global_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.SelectItem value)
            {
                if (btn_global.Tag == value)
                {
                    return;
                }

                btn_global.Tag = value;
                btn_global.Loading = true;

                string lang = value.Tag.ToString();
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
            this.CleanUp_ProxyList();
            this.CleanUp_HexBox();
            this.CleanUp_LogList();
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
            if (Operate.ProxyConfig.Queue.qProxyExecute.Count > 0)
            {
                Operate.ProxyConfig.List.ProxyExecute_ToList();
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

        private void timerProxyListInfo_Tick(object sender, EventArgs e)
        {
            if (!this.bgwProxyList.IsBusy)
            {
                this.bgwProxyList.RunWorkerAsync();
            }

            if (!this.bgwClientList.IsBusy)
            {
                this.bgwClientList.RunWorkerAsync();
            }

            this.mProxyMode.Items[0].Badge = Operate.ProxyConfig.List.lstProxyInfo.Count.ToString();
            this.mProxyMode.Items[1].Badge = this.treeClientList.Items.Count().ToString();
            this.mProxyMode.Items[2].Badge = this.lstAccount.Count.ToString();
            this.mProxyMode.Items[4].Badge = Operate.LogConfig.List.lstLogInfo.Count.ToString();
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
            this.tProxyList.Refresh();
            this.tSystemLog.Refresh();

            this.lProxyTotal_CNT.Text = (Operate.ProxyConfig.Proxy.ProxyTCP_CNT + Operate.ProxyConfig.Proxy.ProxyUDP_CNT).ToString();
            this.lProxyTCP_CNT.Text = Operate.ProxyConfig.Proxy.ProxyTCP_CNT.ToString();
            this.lProxyUDP_CNT.Text = Operate.ProxyConfig.Proxy.ProxyUDP_CNT.ToString();
            this.lProxyQueue_CNT.Text = Operate.ProxyConfig.Queue.qProxyInfo.Count.ToString();
            this.lProxyLinks_CNT.Text = Operate.ProxyConfig.List.lstProxyExecute.Count.ToString();

            this.lAuthCount_Value.Text = Operate.ProxyConfig.Account.lstAuthInfo.Count.ToString();
            this.lLinksCount_Value.Text = Operate.ProxyConfig.Account.GetLinksCount_FromAuthList().ToString();
            this.lDevicesCount_Value.Text = Operate.ProxyConfig.Account.GetDevicesCount_FromAuthList().ToString();

            Operate.ProxyConfig.Proxy.ProxyOnLineInfo = string.Format(
                    "{0}/{1}",
                    Socket_Operation.GetOnLineProxyAccountCount(Operate.ProxyConfig.Account.lstAccountInfo),
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

        #endregion

        #region//显示客户端列表（异步）

        private void bgwClientList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (ProxyExecute pe in Operate.ProxyConfig.List.lstProxyExecute.ToList())
                {
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

                            Operate.ProxyConfig.List.lstProxyExecute.Remove(pe);
                        }
                        else
                        {
                            string sRootName = Operate.ProxyConfig.Proxy.GetClientListName(ClientIP, ClientUserName);

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
                            Operate.ProxyConfig.List.lstProxyExecute.Remove(pe);

                            if (tiRoot.Sub.Count == 0)
                            {
                                Operate.ProxyConfig.Account.DeleteProxyAuthInfo_ByAIDAndIP(pe.AID, ClientIP);

                                if (Operate.ProxyConfig.Proxy.DelClosed)
                                {
                                    this.treeClientList.Items.Remove(tiRoot);
                                }

                                if (pe.AID != null && pe.AID != Guid.Empty)
                                {
                                    Operate.ProxyConfig.Account.SetOnline_ByAccountID(pe.AID, false);
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
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
                    }
                }
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
                foreach (AuthInfo ai in Operate.ProxyConfig.Account.lstAuthInfo.ToList())
                {
                    string ClientIP = ai.AuthIP.ToString();
                    ai.LinksNumber = Operate.ProxyConfig.Account.GetLinksNumber_ByAccountID(ai.AID, ClientIP, this.treeClientList);
                    ai.DevicesNumber = Operate.ProxyConfig.Account.GetDevicesNumber_ByAccountID(ai.AID);
                }
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

                case "miStatistical":
                    this.tabProxyMode.SelectTab("tpStatistical");                    
                    break;

                case "miSystemLog":
                    this.tabProxyMode.SelectTab("tpSystemLog");
                    break;
            }
        }

        #endregion

        #region//代理数据 - 菜单

        private void sProxyList_SelectIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            switch (this.sProxyList.SelectIndex)
            {
                //代理设置
                case 0:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ProxySettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;                

                //列表设置
                case 1:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ListSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //映射设置
                case 2:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new MapSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //外部代理
                case 3:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ExternalProxySettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //系统设置
                case 4:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SystemSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //清空数据
                case 5:

                    this.CleanUp_ProxyListInfo();

                    break;

                //代理
                case 6:

                    if (this.StartProxy)
                    {
                        this.sProxyList.Items[6].IconSvg = "PauseCircleFilled";
                        this.sProxyList.Items[6].Text = AntdUI.Localization.Get("ProxyModeForm.StopProxy", "停止代理");
                        this.StartProxy = false;

                        this.Start_Proxy();
                    }
                    else
                    {
                        this.sProxyList.Items[6].IconSvg = "PlayCircleFilled";
                        this.sProxyList.Items[6].Text = AntdUI.Localization.Get("ProxyModeForm.StartProxy", "开始代理");
                        this.StartProxy = true;

                        this.Stop_Proxy();
                    }

                    break;
            }

            this.sProxyList.SelectIndex = -1;
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

                Operate.ProxyConfig.Proxy.ProxyTotal_CNT = 0;
                Operate.ProxyConfig.Proxy.ProxyTCP_CNT = 0;
                Operate.ProxyConfig.Proxy.ProxyUDP_CNT = 0;

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

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && Operate.ProxyConfig.Proxy.IsListening && e.AcceptSocket != null)
                {
                    Operate.ProxyConfig.Proxy.HandleClient(e.AcceptSocket);

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

                if (Operate.ProxyConfig.Proxy.IsListening)
                {
                    Task.Delay(1000).ContinueWith(_ => AcceptClients());
                }
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

                AntdUI.Message.open(new AntdUI.Message.Config(this, "停止 SOCKS5 代理", TType.Warn)
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
    }
}

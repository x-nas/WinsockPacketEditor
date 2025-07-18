using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;
using WPE.Lib.Controls;

namespace WPE.ProxyMode
{
    public partial class ProxyModeForm : Window, Operate.IProxyMode
    {
        private bool setcolor = false;
        private BindingList<AccountInfo> lstAccount;
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
                Operate.SystemConfig.InitCPUAndMemoryCounter();                
                Operate.SystemConfig.LoadInjectMode_FromDB();
                Operate.SystemConfig.LoadSystemList_FromDB();
                Operate.ProxyConfig.Account.LoadProxyAccountList_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapLocal_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapRemote_FromDB();
                Operate.SystemConfig.StartRemoteMGT();

                this.InitTable_AccountList();

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
            Operate.SystemConfig.StopRemoteMGT(this.RunMode);
            Operate.SystemConfig.SaveSystemConfig_ToDB();
            //Operate.SystemConfig.SaveSystemList_ToDB();
            //Operate.SystemConfig.SaveInjectMode_ToDB();
            Operate.ProxyConfig.Account.SaveProxyAccountList_ToDB(this.RunMode);
            //Operate.ProxyConfig.Mapping.SaveProxyMapLocal_ToDB(this.RunMode);
            //Operate.ProxyConfig.Mapping.SaveProxyMapRemote_ToDB(this.RunMode);
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

            Operate.DoLog(MethodBase.GetCurrentMethod().Name, this.lProcessName.Text);
        }

        public void RefreshAccountList()
        {
            this.tAccountList.Binding(GetPageData(this.pAccountList.Current, this.pAccountList.PageSize));
        }

        #endregion

        #region//初始化表格

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
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
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
                    //AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HotKeyForm())
                    //{
                    //    Align = AntdUI.TAlignMini.Right,
                    //    Mask = true,
                    //    MaskClosable = false,
                    //    DisplayDelay = 0,
                    //});
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

                    break;

                //代理
                case 6:

                    //if (this.StartHook)
                    //{
                    //    this.sPacketList.Items[8].IconSvg = "StopOutlined";
                    //    this.sPacketList.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StopHook", "停止拦截");
                    //    this.StartHook = false;

                    //    this.Start_Hook();
                    //}
                    //else
                    //{
                    //    this.sPacketList.Items[8].IconSvg = "PlayCircleFilled";
                    //    this.sPacketList.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StartHook", "开始拦截");
                    //    this.StartHook = true;

                    //    this.Stop_Hook();
                    //}

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
    }
}

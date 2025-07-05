using AntdUI;
using Be.Windows.Forms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WPE.Lib;
using WPE.Lib.Controls;

namespace WPE.InjectMode
{
    public partial class InjectModeForm : Window
    {
        private bool StartHook = true;
        private bool bWakeUp = true;
        private bool setcolor = false;
        private bool SearchFromHead = true;
        private readonly Hook ws = new Hook();

        #region//窗体事件

        public InjectModeForm()
        {            
            InitializeComponent();
        }

        private void InjectModeForm_Load(object sender, EventArgs e)
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
            Operate.SystemConfig.LoadSystemConfig_FromDB();
            Operate.SystemConfig.LoadInjectMode_FromDB();
            Operate.SystemConfig.LoadSystemList_FromDB();

            this.InitForm();
            this.Dark_Changed();

            this.InitTable_PacketList();
            this.InitTable_FilterList();
            this.InitTable_SendList();
            this.InitTable_LogList();

            this.splitterPacketList.SplitterWidth = 10;        
            this.tabInjectMode.TabMenuVisible = false;            
            this.mInjectMode.SelectIndex(0, true);            
        }

        private void InjectModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemList_ToDB();
            Operate.SystemConfig.SaveInjectMode_ToDB();
            Operate.SystemConfig.SaveSystemList_ToDB();
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

            for (int i = 0; i < this.mInjectMode.Items.Count; i++)
            {
                this.mInjectMode.Items[i].BadgeBack = this.colorTheme.Value;
            }

            Operate.DoLog(MethodBase.GetCurrentMethod().Name, this.lProcessName.Text);
        }

        public void RefreshFilterList()
        {
            this.tFilterList.Refresh();
        }

        public void RefreshSendList()
        {
            this.tSendList.Refresh();
        }

        #endregion

        #region//初始化数据表

        private void InitTable_PacketList()
        {
            tPacketList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("PacketImg", string.Empty, AntdUI.ColumnAlign.Center).SetFixed(),
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
                new AntdUI.Column("PacketFrom", "本机地址").SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketTo", "远端地址").SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketData", "数据").SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tPacketList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tPacketList.DataSource = Operate.PacketConfig.List.lstPacketInfo;
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
                            AntdUI.CellBadge cellBadge = null;

                            if(fi.IsEnable)
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Success, "启用");

                                if(fi.ExecutionCount > 0)
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
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.FilterList.Column."),
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
                new AntdUI.Column("CellLinks", "操作").SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
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
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.SendList.Column."),
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
                new AntdUI.Column("CellLinks", "操作").SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.SendList.Column."),
            };

            this.tSendList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSendList.Binding(Operate.SendConfig.List.lstSendInfo);
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
                this.hbPacketData.BackColor = Color.FromArgb(30, 30, 30);
                this.hbPacketData.ForeColor = Color.Silver;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;

                this.tPacketList.ColumnFore = Color.Black;
                this.tPacketList.ForeColor = Color.Green;
                this.hbPacketData.BackColor = Color.White;
                this.hbPacketData.ForeColor = Color.Black;
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

        private void sPacketList_SelectIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            switch (this.sPacketList.SelectIndex)
            {
                //过滤设置
                case 0:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new FilterSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //拦截设置
                case 1:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HookSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //列表设置
                case 2:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ListSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //快捷键设置
                case 3:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HotKeyForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //备份设置
                case 4:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new BackUpSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //系统设置
                case 5:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SystemSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //查找封包
                case 6:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SearchPacketForm(this) { Size = new Size(300, 300) })
                    {                        
                        Align = AntdUI.TAlignMini.Right,
                        Mask = false,                        
                        DisplayDelay = 0,
                    });
                    break;

                //清空数据
                case 7:

                    this.CleanUp_PacketListInfo();
                    this.CleanUp_PacketList();
                    this.CleanUp_HexBox();
                    this.CleanUp_LogList();

                    break;

                //拦截
                case 8:

                    if (this.StartHook)
                    {
                        this.sPacketList.Items[8].IconSvg = "PauseCircleFilled";
                        this.sPacketList.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StopHook", "停止拦截");
                        this.StartHook = false;

                        this.Start_Hook();
                    }
                    else
                    {
                        this.sPacketList.Items[8].IconSvg = "PlayCircleFilled";
                        this.sPacketList.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StartHook", "开始拦截");
                        this.StartHook = true;

                        this.Stop_Hook();
                    }

                    break;
            }

            this.sPacketList.SelectIndex = -1;
        }

        #endregion

        #region//封包列表 - 右键菜单

        private void InitCMS_PacketList()
        {
            AntdUI.IContextMenuStripItem[] menulist = { };

            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
            {
                AntdUI.IContextMenuStripItem[] menulist_SendList = new AntdUI.IContextMenuStripItem[Operate.SendConfig.List.lstSendInfo.Count];

                for (int i = 0; i < menulist_SendList.Length; i++)
                {
                    menulist_SendList[i] = new AntdUI.ContextMenuStripItem(Operate.SendConfig.List.lstSendInfo[i].SName)
                    {
                        ID = Operate.SendConfig.List.lstSendInfo[i].SID.ToString().ToUpper(),
                    };
                }
                
                menulist = new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("添加到发送列表")
                {
                    ID = "cmsPacketList_ToSendList",
                    IconSvg = "ProfileOutlined",
                    LocalizationText = "InjectModeForm.cmsPacketList.ToSendList",
                    Sub = menulist_SendList,
                },
                };
            }
            else
            {
                menulist = new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("添加到发送列表")
                    {
                        Enabled = false,
                        ID = "cmsPacketList_ToSendList",
                        IconSvg = "ProfileOutlined",
                        LocalizationText = "InjectModeForm.cmsPacketList.ToSendList",
                    },
                };
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
                    case "cmsPacketList_ToSendList":

                        if (piList.Count > 0)
                        {
                            //
                        }

                        break;

                    default:

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

                        break;
                }
            }, menulist);
        }

        private void tPacketList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.PacketConfig.List.lstPacketInfo.Count == 0)
                {
                    return;
                }

                this.InitCMS_PacketList();
            }
        }

        #endregion

        #region//滤镜列表 - 菜单

        private void sFilterList_SelectIndexChanged(object sender, IntEventArgs e)
        {
            switch (this.sFilterList.SelectIndex)
            {
                //导入
                case 0:
                    Operate.FilterConfig.List.LoadFilterList_Dialog(this);
                    break;

                //导出
                case 1:                    
                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        Operate.FilterConfig.List.SaveFilterList_Dialog(this, string.Empty, Operate.FilterConfig.List.lstFilterInfo.ToList());
                    }
                    break;

                //新增
                case 2:                    
                    Operate.FilterConfig.Filter.AddFilter_New();
                    this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                    break;

                //清空
                case 3:                    
                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        Operate.FilterConfig.List.CleanUpFilterList_Dialog(this);
                    }
                    break;
            }

            this.sFilterList.SelectIndex = -1;
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
                        case "cmsFilterList_Top":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Top, fiList);
                            }

                            break;

                        case "cmsFilterList_Up":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Up, fiList);
                            }

                            break;

                        case "cmsFilterList_Down":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Down, fiList);
                            }

                            break;

                        case "cmsFilterList_Bottom":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Bottom, fiList);
                            }

                            break;

                        case "cmsFilterList_Copy":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Copy, fiList);
                                this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                            }

                            break;

                        case "cmsFilterList_Export":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Export, fiList);
                            }

                            break;

                        case "cmsFilterList_Delete":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, fiList);
                            }

                            break;
                    }

                    this.tFilterList.SelectedIndex = -1;
                },
                new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("置顶", "Ctrl+向上键")
                {
                    ID = "cmsFilterList_Top",
                    IconSvg = "VerticalAlignTopOutlined",
                    LocalizationText = "InjectModeForm.cmsFilterList.Top",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("向上移动", "Alt+向上键")
                {
                    ID = "cmsFilterList_Up",
                    IconSvg = "ArrowUpOutlined",
                },
                    new AntdUI.ContextMenuStripItem("向下移动", "Alt+向下键")
                {
                    ID = "cmsFilterList_Down",
                    IconSvg = "ArrowDownOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("置底", "Ctrl+向下键")
                {
                    ID = "cmsFilterList_Bottom",
                    IconSvg = "VerticalAlignBottomOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("复制")
                {
                    ID = "cmsFilterList_Copy",
                    IconSvg = "CopyOutlined",
                },
                    new AntdUI.ContextMenuStripItem("导出到文件")
                {
                    ID = "cmsFilterList_Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                    new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "cmsFilterList_Delete",
                    IconSvg = "DeleteOutlined",
                },
                }));
            }
        }

        #endregion

        #region//发送列表 - 菜单

        private void sSendList_SelectIndexChanged(object sender, IntEventArgs e)
        {
            switch (this.sSendList.SelectIndex)
            {
                //导入
                case 0:
                    Operate.SendConfig.List.LoadSendList_Dialog(this);
                    break;

                //导出
                case 1:
                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        Operate.SendConfig.List.SaveSendList_Dialog(this, string.Empty, Operate.SendConfig.List.lstSendInfo.ToList());
                    }
                    break;

                //执行发送
                case 2:

                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        if (!this.bgwSendList.IsBusy)
                        {
                            this.sSendList.Items[2].Enabled = false;
                            this.sSendList.Items[3].Enabled = true;
                            
                            Operate.SendConfig.List.lstSendExecute.Clear();

                            this.bgwSendList.RunWorkerAsync();
                        }
                    }

                    break;

                //停止
                case 3:

                    this.bgwSendList.CancelAsync();

                    break;

                //新增
                case 4:
                    Operate.SendConfig.Send.AddSend_New();
                    this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;
                    break;

                //清空
                case 5:
                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        Operate.SendConfig.List.CleanUpSendList_Dialog(this);
                    }
                    break;
            }

            this.sSendList.SelectIndex = -1;
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
                        case "cmsSendList_Top":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Top, siList);
                            }

                            break;

                        case "cmsSendList_Up":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Up, siList);
                            }

                            break;

                        case "cmsSendList_Down":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Down, siList);
                            }

                            break;

                        case "cmsSendList_Bottom":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Bottom, siList);
                            }

                            break;

                        case "cmsSendList_Copy":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Copy, siList);
                                this.tSendList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                            }

                            break;

                        case "cmsSendList_Export":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Export, siList);
                            }

                            break;

                        case "cmsSendList_Delete":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this, Operate.SystemConfig.ListAction.Delete, siList);
                            }

                            break;
                    }

                    this.tSendList.SelectedIndex = -1;
                },
                new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("置顶", "Ctrl+向上键")
                {
                    ID = "cmsSendList_Top",
                    IconSvg = "VerticalAlignTopOutlined",
                    LocalizationText = "InjectModeForm.cmsFilterList.Top",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("向上移动", "Alt+向上键")
                {
                    ID = "cmsSendList_Up",
                    IconSvg = "ArrowUpOutlined",
                },
                    new AntdUI.ContextMenuStripItem("向下移动", "Alt+向下键")
                {
                    ID = "cmsSendList_Down",
                    IconSvg = "ArrowDownOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("置底", "Ctrl+向下键")
                {
                    ID = "cmsSendList_Bottom",
                    IconSvg = "VerticalAlignBottomOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("复制")
                {
                    ID = "cmsSendList_Copy",
                    IconSvg = "CopyOutlined",
                },
                    new AntdUI.ContextMenuStripItem("导出到文件")
                {
                    ID = "cmsSendList_Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                    new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "cmsSendList_Delete",
                    IconSvg = "DeleteOutlined",
                },
                }));
            }
        }

        #endregion

        #region//开始拦截

        private void Start_Hook()
        {
            try
            {
                Operate.FilterConfig.List.InitFilterList_Count();
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

                AntdUI.Message.open(new AntdUI.Message.Config(this, "停止拦截", TType.Warn)
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
                Operate.PacketConfig.Queue.FilterPacketList_CNT = 0;
                Operate.PacketConfig.Queue.Send_CNT = 0;
                Operate.PacketConfig.Queue.Recv_CNT = 0;
                Operate.PacketConfig.Queue.SendTo_CNT = 0;
                Operate.PacketConfig.Queue.RecvFrom_CNT = 0;
                Operate.PacketConfig.Queue.WSASend_CNT = 0;
                Operate.PacketConfig.Queue.WSARecv_CNT = 0;
                Operate.PacketConfig.Queue.WSASendTo_CNT = 0;
                Operate.PacketConfig.Queue.WSARecvFrom_CNT = 0;
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
            this.lFilterPacketList_CNT.Text = Operate.PacketConfig.Queue.FilterPacketList_CNT.ToString();
            this.lSend_CNT.Text = Operate.PacketConfig.Queue.Send_CNT.ToString();
            this.lRecv_CNT.Text = Operate.PacketConfig.Queue.Recv_CNT.ToString();
            this.lSendTo_CNT.Text = Operate.PacketConfig.Queue.SendTo_CNT.ToString();
            this.lRecvFrom_CNT.Text = Operate.PacketConfig.Queue.RecvFrom_CNT.ToString();
            this.lWSASend_CNT.Text = Operate.PacketConfig.Queue.WSASend_CNT.ToString();
            this.lWSARecv_CNT.Text = Operate.PacketConfig.Queue.WSARecv_CNT.ToString();
            this.lWSASendTo_CNT.Text = Operate.PacketConfig.Queue.WSASendTo_CNT.ToString();
            this.lWSARecvFrom_CNT.Text = Operate.PacketConfig.Queue.WSARecvFrom_CNT.ToString();
            this.lSpeedInfo.Text = Operate.PacketConfig.Packet.GetPacketSpeedInfo();
            this.mInjectMode.Items[0].Badge = Operate.PacketConfig.List.lstPacketInfo.Count.ToString();
            this.mInjectMode.Items[1].Badge = Operate.FilterConfig.List.lstFilterInfo.Count.ToString();
            this.mInjectMode.Items[2].Badge = Operate.SendConfig.List.lstSendInfo.Count.ToString();
            this.mInjectMode.Items[3].Badge = Operate.RobotConfig.RobotList.lstRobot.Count.ToString();
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
            this.tPacketList.Refresh();
            this.tSystemLog.Refresh();

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
                                bSearchContent = Socket_Operation.StringToBytes(efFormat, Operate.PacketConfig.List.FindOptions.Text);
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

                        e.Result = Operate.PacketConfig.List.SearchForSocketList(Operate.PacketConfig.List.Search_Index, bSearchContent);                        
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

        #region//执行发送列表（异步）

        private void bgwSendList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                foreach (SendInfo si in Operate.SendConfig.List.lstSendInfo)
                {
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

                                    Thread.Sleep(100);
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
            this.sSendList.Items[2].Enabled = true;
            this.sSendList.Items[3].Enabled = false;
        }

        #endregion
    }
}

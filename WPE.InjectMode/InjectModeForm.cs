using AntdUI;
using Be.Windows.Forms;
using EasyHook;
using System;
using System.Drawing;
using System.Reflection;
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

            this.InitForm();
            this.Dark_Changed();
            this.InitTable_PacketList();
            this.InitTable_LogList();

            this.splitter.SplitterWidth = 10;
            this.tabInjectMode.TabMenuVisible = false;
            this.mInjectMode.SelectIndex(0, true);            
        }

        private void InjectModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemList_ToDB();
            Operate.SystemConfig.SaveInjectMode_ToDB();
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

            Operate.DoLog(MethodBase.GetCurrentMethod().Name, this.lProcessName.Text);
        }

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
            this.tPacketList.DataSource = Operate.PacketConfig.List.lstRecPacket;
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
                if (index > -1 && index < Operate.PacketConfig.List.lstRecPacket.Count)
                {
                    PacketInfo pi = Operate.PacketConfig.List.lstRecPacket[index];
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
                this.tPacketList.ForeColor = Color.FromArgb(0, 128, 0);
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

        #region//封包列表 - 顶部菜单

        private void segmented_SelectIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            switch (this.segmented.SelectIndex)
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
                        this.segmented.Items[8].IconSvg = "PauseCircleFilled";
                        this.segmented.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StopHook", "停止拦截");
                        this.StartHook = false;

                        this.Start_Hook();
                    }
                    else
                    {
                        this.segmented.Items[8].IconSvg = "PlayCircleFilled";
                        this.segmented.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StartHook", "开始拦截");
                        this.StartHook = true;

                        this.Stop_Hook();
                    }

                    break;
            }

            this.segmented.SelectIndex = -1;
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
                Operate.PacketConfig.List.lstRecPacket.Clear();
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
            try
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
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            this.mInjectMode.Items[0].Badge = Operate.PacketConfig.List.lstRecPacket.Count.ToString();
            this.mInjectMode.Items[1].Badge = Operate.FilterConfig.List.lstFilter.Count.ToString();
            this.mInjectMode.Items[2].Badge = Operate.SendConfig.SendList.lstSend.Count.ToString();
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
                if (selectedIndex >= 0 && selectedIndex < Operate.PacketConfig.List.lstRecPacket.Count)
                {
                    Operate.PacketConfig.List.Search_Index = selectedIndex;
                    Operate.PacketConfig.List.piSelect = Operate.PacketConfig.List.lstRecPacket[selectedIndex];

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
                int PacketListCNT = tPacketList.ToDataTable().Rows.Count - 1;
                if (Operate.PacketConfig.List.AutoRoll && PacketListCNT > 0)
                {
                    tPacketList.ScrollLine(PacketListCNT, true);
                }

                if (Operate.PacketConfig.List.AutoClear)
                {
                    if (PacketListCNT > Operate.PacketConfig.List.AutoClear_Value)
                    {
                        this.CleanUp_PacketList();
                        this.CleanUp_HexBox();
                    }
                }

                int SystemLogCNT = tSystemLog.ToDataTable().Rows.Count - 1;
                if (Operate.LogConfig.List.AutoRoll && SystemLogCNT > 0)
                {
                    tSystemLog.ScrollLine(SystemLogCNT, true);
                }

                if (Operate.LogConfig.List.AutoClear)
                {
                    if (SystemLogCNT > Operate.LogConfig.List.AutoClear_Value)
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
                if (Operate.PacketConfig.List.lstRecPacket.Count > 0)
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
    }
}

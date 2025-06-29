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

            this.tabInjectMode.TabMenuVisible = false;
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
            this.tPacketList.Binding(Operate.PacketConfig.List.lstRecPacket);
        }

        private Table.CellStyleInfo tPacketList_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            try
            {
                switch (Operate.PacketConfig.List.lstRecPacket[e.RowIndex - 1].FilterAction)
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
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }            
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
                this.tPacketList.ForeColor = Color.Black;
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
                    this.tabInjectMode.SelectTab(0);
                    break;

                case "miStatistical":
                    this.tabInjectMode.SelectTab(1);
                    break;

                case "miComparison":
                    this.tabInjectMode.SelectTab(2);
                    break;

                case "miXOR":
                    this.tabInjectMode.SelectTab(3);
                    break;

                case "miTranscoding":
                    this.tabInjectMode.SelectTab(4);
                    break;

                case "miExtraction":
                    this.tabInjectMode.SelectTab(5);
                    break;

                case "miSystemLog":
                    this.tabInjectMode.SelectTab(6);
                    break;
            }
        }

        #endregion

        #region//分段菜单

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

                //搜索封包
                case 6:

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
                this.timerPacketList.Start();
                this.timerPacketListInfo.Start();

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

                this.timerPacketList.Stop();
                this.timerPacketListInfo.Stop();

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
                Operate.LogConfig.ClearLogQueue();
                Operate.LogConfig.ClearLogList();
                //this.dgvLogList.Rows.Clear();
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

                if (Operate.PacketConfig.List.AutoRoll && Operate.PacketConfig.List.lstRecPacket.Count > 0)
                {
                    tPacketList.ScrollLine(Operate.PacketConfig.List.lstRecPacket.Count, true);
                }

                if (Operate.PacketConfig.List.AutoClear)
                {
                    if (Operate.PacketConfig.List.lstRecPacket.Count > Operate.PacketConfig.List.AutoClear_Value)
                    {
                        this.CleanUp_PacketList();
                        this.CleanUp_HexBox();
                    }
                }                
            }

            //if (Operate.LogConfig.cqLogInfo.Count > 0)
            //{
            //    Operate.LogConfig.LogToList();
            //    this.AutoScrollDataGridView(dgvLogList, cbLogList_AutoRoll.Checked);
            //    this.AutoCleanUp_LogList();
            //}            
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
        }


        #endregion

        #region//显示选中的封包数据

        private void tPacketList_CellClick(object sender, TableClickEventArgs e)
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
    }
}

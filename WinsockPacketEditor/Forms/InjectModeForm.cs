using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class InjectModeForm : Window, InterfaceInfo.IInjectMode
    {        
        private bool setcolor = false;
        private PacketList cPacketList = null;
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
            Theme().Dark(Operate.SystemConfig.Color_30).Light(Operate.SystemConfig.Color_250);
            Operate.SystemConfig.SelectMode = Operate.SystemConfig.SystemMode.Inject;
        }

        private void InjectModeForm_Load(object sender, EventArgs e)
        {
            this.pageHeader.Loading = true;
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
            
            Operate.SystemConfig.InitHotKeys(this.Handle);

            AntdUI.Spin.open(this, AntdUI.Localization.Get("Loading", "正在加载..."), config =>
            {
                Operate.SystemConfig.StartRemoteMGT(this);
                Operate.SystemConfig.InitCPUAndMemoryCounter();
                Operate.SystemConfig.InitListExecute();
                Operate.SystemConfig.LoadInjectMode_FromDB();
                Operate.SystemConfig.LoadProxyMode_FromDB();
                Operate.SystemConfig.LoadSystemList_FromDB();                

                this.InitGlobal();
                this.InitFloatButton();
                this.InitControls();

            }, () =>
            {
                this.pageHeader.Loading = false;
            });
            
            this.InitForm();            
            this.Dark_Changed();

            this.timerAutoSave.Interval = Operate.SystemConfig.AutoSaveINT;
            this.timerAutoSave.Enabled = true;
            this.tabInjectMode.TabMenuVisible = false;
            this.mInjectMode.SelectIndex(0, true);
            this.colorTheme.Value = Operate.SystemConfig.SystemColor;
        }

        private void InjectModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.StopRemoteMGT(this);
            Operate.SystemConfig.SaveSystemConfig_ToDB();
            Operate.SystemConfig.SaveInjectMode_ToDB();
            Operate.SystemConfig.SaveProxyMode_ToDB();
            Operate.SystemConfig.SaveSystemList_ToDB();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == User32.WM_HOTKEY)
            {
                int hotKeyId = m.WParam.ToInt32();

                BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        await Operate.SystemConfig.DoHotKey(hotKeyId);
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(nameof(WndProc), ex.Message);
                    }
                }));

                return;
            }

            base.WndProc(ref m);
        }

        private void InitControls()
        {
            //PacketList
            if (this.tpPacketList.InvokeRequired)
            {
                this.tpPacketList.Invoke(new Action(() =>
                {
                    cPacketList = new PacketList(this);
                    cPacketList.Dock = DockStyle.Fill;
                    this.tpPacketList.Controls.Add(cPacketList);
                }));
            }
            else
            {
                cPacketList = new PacketList(this);
                cPacketList.Dock = DockStyle.Fill;
                this.tpPacketList.Controls.Add(cPacketList);
            }

            //FilterList
            if (this.tpFilterList.InvokeRequired)
            {
                this.tpFilterList.Invoke(new Action(() =>
                {
                    cFilterList = new FilterList(this);
                    cFilterList.Dock = DockStyle.Fill;
                    this.tpFilterList.Controls.Add(cFilterList);
                }));
            }
            else
            {
                cFilterList = new FilterList(this);
                cFilterList.Dock = DockStyle.Fill;
                this.tpFilterList.Controls.Add(cFilterList);
            }

            //SendList
            if (this.tpSendList.InvokeRequired)
            {
                this.tpSendList.Invoke(new Action(() =>
                {
                    cSendList = new SendList(this);
                    cSendList.Dock = DockStyle.Fill;
                    this.tpSendList.Controls.Add(cSendList);
                }));
            }
            else
            {
                cSendList = new SendList(this);
                cSendList.Dock = DockStyle.Fill;
                this.tpSendList.Controls.Add(cSendList);
            }

            //RobotList
            if (this.tpRobotList.InvokeRequired)
            {
                this.tpRobotList.Invoke(new Action(() =>
                {
                    cRobotList = new RobotList(this);
                    cRobotList.Dock = DockStyle.Fill;
                    this.tpRobotList.Controls.Add(cRobotList);
                }));
            }
            else
            {
                cRobotList = new RobotList(this);
                cRobotList.Dock = DockStyle.Fill;
                this.tpRobotList.Controls.Add(cRobotList);
            }

            //LogList
            if (this.tpSystemLog.InvokeRequired)
            {
                this.tpSystemLog.Invoke(new Action(() =>
                {
                    cLogList = new LogList(this);
                    cLogList.Dock = DockStyle.Fill;
                    this.tpSystemLog.Controls.Add(cLogList);
                }));
            }
            else
            {
                cLogList = new LogList(this);
                cLogList.Dock = DockStyle.Fill;
                this.tpSystemLog.Controls.Add(cLogList);
            }

            //StatisticalData
            if (this.tpStatistical.InvokeRequired)
            {
                this.tpStatistical.Invoke(new Action(() =>
                {
                    cStatisticalData = new StatisticalData();
                    cStatisticalData.Dock = DockStyle.Fill;
                    this.tpStatistical.Controls.Add(cStatisticalData);
                }));
            }
            else
            {
                cStatisticalData = new StatisticalData();
                cStatisticalData.Dock = DockStyle.Fill;
                this.tpStatistical.Controls.Add(cStatisticalData);
            }

            //ComparisonText
            if (this.tpComparison.InvokeRequired)
            {
                this.tpComparison.Invoke(new Action(() =>
                {
                    cComparisonText = new ComparisonText();
                    cComparisonText.Dock = DockStyle.Fill;
                    this.tpComparison.Controls.Add(cComparisonText);
                }));
            }
            else
            {
                cComparisonText = new ComparisonText();
                cComparisonText.Dock = DockStyle.Fill;
                this.tpComparison.Controls.Add(cComparisonText);
            }

            //XORCalculation
            if (this.tpXOR.InvokeRequired)
            {
                this.tpXOR.Invoke(new Action(() =>
                {
                    cXORCalculation = new XORCalculation(this);
                    cXORCalculation.Dock = DockStyle.Fill;
                    this.tpXOR.Controls.Add(cXORCalculation);
                }));
            }
            else
            {
                cXORCalculation = new XORCalculation(this);
                cXORCalculation.Dock = DockStyle.Fill;
                this.tpXOR.Controls.Add(cXORCalculation);
            }

            //Transcoding
            if (this.tpTranscoding.InvokeRequired)
            {
                this.tpTranscoding.Invoke(new Action(() =>
                {
                    cTranscoding = new Transcoding();
                    cTranscoding.Dock = DockStyle.Fill;
                    this.tpTranscoding.Controls.Add(cTranscoding);
                }));
            }
            else
            {
                cTranscoding = new Transcoding();
                cTranscoding.Dock = DockStyle.Fill;
                this.tpTranscoding.Controls.Add(cTranscoding);
            }

            //ExtractionData
            if (this.tpExtraction.InvokeRequired)
            {
                this.tpExtraction.Invoke(new Action(() =>
                {
                    cExtractionData = new ExtractionData(this);
                    cExtractionData.Dock = DockStyle.Fill;
                    this.tpExtraction.Controls.Add(cExtractionData);
                }));
            }
            else
            {
                cExtractionData = new ExtractionData(this);
                cExtractionData.Dock = DockStyle.Fill;
                this.tpExtraction.Controls.Add(cExtractionData);
            }
        }

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("InjectModeForm", "注入模式");
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;            

            this.mInjectMode.Collapsed = false;
            this.MenuCollapseChange();            

            for (int i = 0; i < this.mInjectMode.Items.Count; i++)
            {
                this.mInjectMode.Items[i].BadgeBack = this.colorTheme.Value;
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
            Operate.SystemConfig.InitFloatButton(this);  
        }        

        #endregion

        #region//接口实现

        public void SetColumnVisible_PacketList()
        {
            this.cPacketList?.SetColumnVisible_PacketList();
        }

        public void SearchPacketList(bool FromHead)
        {
            this.cPacketList?.SearchPacketList(FromHead);
        }

        public void RefreshPacketData()
        {
            this.cPacketList?.RefreshPacketData();
        }

        public void RefreshFilterList()
        {
            this.cFilterList?.RefreshFilterList();
        }

        public void RefreshSendList()
        {
            this.cSendList?.RefreshSendList();
        }

        public void RefreshRobotList()
        {
            this.cRobotList?.RefreshRobotList();
        }

        public void CleanUp_LogList()
        {
            this.cLogList?.CleanUp_LogList();
        }

        public void SetTextA(string TextA)
        {
            this.cComparisonText?.SetTextA(TextA);
        }

        public void SetTextB(string TextB)
        {
            this.cComparisonText?.SetTextB(TextB);
        }

        #endregion

        #region//更换主题颜色

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            setcolor = true;
            Operate.SystemConfig.SystemColor = e.Value;

            AntdUI.Style.SetPrimary(Operate.SystemConfig.SystemColor);

            for (int i = 0; i < this.mInjectMode.Items.Count; i++)
            {
                this.mInjectMode.Items[i].BadgeBack = Operate.SystemConfig.SystemColor;
            }

            Refresh();
        }

        #endregion

        #region//更换主题模式

        private void btn_mode_Click(object sender, EventArgs e)
        {
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;

            this.Dark_Changed();
            Refresh();
        }

        private void Dark_Changed()
        {
            btn_mode.Toggle = AntdUI.Config.IsDark;

            if (setcolor)
            {
                var color = AntdUI.Style.Db.Primary;
                AntdUI.Style.SetPrimary(color);
            }

            this.cPacketList?.Dark_Changed();
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
                this.cPacketList.SetColumnName_PacketList();
                this.cComparisonText.SetTextInfo();
                this.cExtractionData.SetExtractionInfo();

                Refresh();
                btn_global.Loading = false;
            }            
        }

        #endregion

        #region//显示设置

        private void btn_setting_Click(object sender, EventArgs e)
        {
            var setting = new DisplaySetting();
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

        #region//计时器 - 数据列表

        private void timerPacketList_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerPacketList.Stop();

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
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(timerPacketList_Tick), ex.Message);
            }
            finally
            {
                this.timerPacketList.Start();
            }
        }

        #endregion

        #region//计时器 - 列表信息

        private void timerPacketListInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerPacketListInfo.Stop();

                this.mInjectMode.Items[0].Badge = Operate.PacketConfig.List.lstPacketInfo.Count.ToString();
                this.mInjectMode.Items[1].Badge = Operate.FilterConfig.List.lstFilterInfo.Count.ToString();
                this.mInjectMode.Items[2].Badge = Operate.SendConfig.List.lstSendInfo.Count.ToString();
                this.mInjectMode.Items[3].Badge = Operate.RobotConfig.List.lstRobotInfo.Count.ToString();
                this.mInjectMode.Items[9].Badge = Operate.LogConfig.List.lstLogInfo.Count.ToString();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(timerPacketListInfo_Tick), ex.Message);
            }
            finally
            {
                this.timerPacketListInfo.Start();
            }      
        }

        #endregion

        #region//计时器 - 自动保存

        private void timerAutoSave_Tick(object sender, EventArgs e)
        {
            if (!this.bgwAutoSave.IsBusy)
            {
                this.bgwAutoSave.RunWorkerAsync();
            }
        }

        #endregion        

        #region//自动保存（异步）

        private void bgwAutoSave_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Operate.SystemConfig.SaveSystemConfig_ToDB();
                Operate.SystemConfig.SaveInjectMode_ToDB();
                Operate.SystemConfig.SaveProxyMode_ToDB();
                Operate.SystemConfig.SaveSystemList_ToDB();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bgwAutoSave_DoWork), ex.Message);
            }
        }

        #endregion        
    }
}

using AntdUI;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class InjectModeForm : Window, InterfaceInfo.IInjectMode
    {        
        private bool setcolor = false;
        private AntdUI.FormFloatButton FloatButton = null;
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

            }, () =>
            {
                this.pageHeader.Loading = false;
            });

            Operate.SystemConfig.MainHandle = this.Handle;
            
            this.InitForm();
            this.InitControls();
            this.InitHotKeys();
            this.Dark_Changed();

            this.tabInjectMode.TabMenuVisible = false;
            this.mInjectMode.SelectIndex(0, true);            
        }

        private void InjectModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                    Operate.SystemConfig.DoHotKey(HOTKEY_ID);
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
            cPacketList = new PacketList(this);
            cPacketList.Dock = DockStyle.Fill;
            this.tpPacketList.Controls.Add(cPacketList);

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
            this.cSendList?.Refresh();
        }

        public void RefreshPacketData()
        {
            this.cPacketList?.RefreshPacketData();
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
            }
            else
            {
                BackColor = Operate.SystemConfig.Color_250;
                ForeColor = Color.Black;

                this.tabInjectMode.BackColor = Color.White;
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
                    this.cPacketList.RefreshPacketList();
                    this.cLogList?.RefreshLogList();

                    this.ShowMenuInfo();                   
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

        #region//显示菜单信息

        private void ShowMenuInfo()
        {
            this.mInjectMode.Items[0].Badge = Operate.PacketConfig.List.lstPacketInfo.Count.ToString();
            this.mInjectMode.Items[1].Badge = Operate.FilterConfig.List.lstFilterInfo.Count.ToString();
            this.mInjectMode.Items[2].Badge = Operate.SendConfig.List.lstSendInfo.Count.ToString();
            this.mInjectMode.Items[3].Badge = Operate.RobotConfig.List.lstRobotInfo.Count.ToString();
            this.mInjectMode.Items[9].Badge = Operate.LogConfig.List.lstLogInfo.Count.ToString();
        }

        #endregion
    }
}

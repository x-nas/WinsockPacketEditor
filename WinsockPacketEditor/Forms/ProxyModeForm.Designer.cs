namespace WinsockPacketEditor
{
    partial class ProxyModeForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            AntdUI.MenuItem menuItem37 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem38 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem39 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem40 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem41 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem42 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem43 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem44 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem45 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem46 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem47 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem48 = new AntdUI.MenuItem();
            AntdUI.Tabs.StyleLine styleLine4 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxyModeForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.colorTheme = new AntdUI.ColorPicker();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.btn_setting = new AntdUI.Button();
            this.tlpMenu = new WinsockPacketEditor.TableLayoutPanelEx();
            this.mProxyMode = new AntdUI.Menu();
            this.bMenuCollapse = new AntdUI.Button();
            this.tabProxyMode = new AntdUI.Tabs();
            this.tpProxyList = new AntdUI.TabPage();
            this.tpClientList = new AntdUI.TabPage();
            this.tpAccountList = new AntdUI.TabPage();
            this.tpFilterList = new AntdUI.TabPage();
            this.tpSendList = new AntdUI.TabPage();
            this.tpRobotList = new AntdUI.TabPage();
            this.tpStatistical = new AntdUI.TabPage();
            this.tpComparison = new AntdUI.TabPage();
            this.tpXOR = new AntdUI.TabPage();
            this.tpTranscoding = new AntdUI.TabPage();
            this.tpExtraction = new AntdUI.TabPage();
            this.tpSystemLog = new AntdUI.TabPage();
            this.timerProxyList = new System.Windows.Forms.Timer(this.components);
            this.timerProxyListInfo = new System.Windows.Forms.Timer(this.components);
            this.timerAutoSave = new System.Windows.Forms.Timer(this.components);
            this.bgwAutoSave = new System.ComponentModel.BackgroundWorker();
            this.pageHeader.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.tabProxyMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader
            // 
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.Controls.Add(this.colorTheme);
            this.pageHeader.Controls.Add(this.btn_mode);
            this.pageHeader.Controls.Add(this.btn_global);
            this.pageHeader.Controls.Add(this.btn_setting);
            this.pageHeader.DividerMargin = 3;
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.FullBox = true;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1450, 60);
            this.pageHeader.SubText = "2.0.0.0";
            this.pageHeader.TabIndex = 7;
            this.pageHeader.Text = "WPE x64";
            // 
            // colorTheme
            // 
            this.colorTheme.Dock = System.Windows.Forms.DockStyle.Right;
            this.colorTheme.Location = new System.Drawing.Point(1068, 0);
            this.colorTheme.Name = "colorTheme";
            this.colorTheme.Padding = new System.Windows.Forms.Padding(5);
            this.colorTheme.ShowClose = true;
            this.colorTheme.ShowReset = true;
            this.colorTheme.Size = new System.Drawing.Size(40, 40);
            this.colorTheme.TabIndex = 13;
            this.colorTheme.Value = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.colorTheme.ValueChanged += new AntdUI.ColorEventHandler(this.colorTheme_ValueChanged);
            // 
            // btn_mode
            // 
            this.btn_mode.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_mode.Ghost = true;
            this.btn_mode.IconSvg = "SunOutlined";
            this.btn_mode.Location = new System.Drawing.Point(1108, 0);
            this.btn_mode.Name = "btn_mode";
            this.btn_mode.Radius = 0;
            this.btn_mode.Size = new System.Drawing.Size(50, 40);
            this.btn_mode.TabIndex = 12;
            this.btn_mode.ToggleIconSvg = "MoonOutlined";
            this.btn_mode.WaveSize = 0;
            this.btn_mode.Click += new System.EventHandler(this.btn_mode_Click);
            // 
            // btn_global
            // 
            this.btn_global.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_global.DropDownRadius = 6;
            this.btn_global.Ghost = true;
            this.btn_global.IconSvg = "GlobalOutlined";
            this.btn_global.Location = new System.Drawing.Point(1158, 0);
            this.btn_global.Name = "btn_global";
            this.btn_global.Placement = AntdUI.TAlignFrom.BR;
            this.btn_global.Radius = 0;
            this.btn_global.Size = new System.Drawing.Size(50, 40);
            this.btn_global.TabIndex = 11;
            this.btn_global.WaveSize = 0;
            this.btn_global.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.btn_global_SelectedValueChanged);
            // 
            // btn_setting
            // 
            this.btn_setting.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_setting.Ghost = true;
            this.btn_setting.IconSvg = "SettingOutlined";
            this.btn_setting.Location = new System.Drawing.Point(1208, 0);
            this.btn_setting.Name = "btn_setting";
            this.btn_setting.Radius = 0;
            this.btn_setting.Size = new System.Drawing.Size(50, 40);
            this.btn_setting.TabIndex = 10;
            this.btn_setting.WaveSize = 0;
            this.btn_setting.Click += new System.EventHandler(this.btn_setting_Click);
            // 
            // tlpMenu
            // 
            this.tlpMenu.ColumnCount = 1;
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Controls.Add(this.mProxyMode, 0, 1);
            this.tlpMenu.Controls.Add(this.bMenuCollapse, 0, 0);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.tlpMenu.Location = new System.Drawing.Point(0, 60);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 2;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Size = new System.Drawing.Size(170, 742);
            this.tlpMenu.TabIndex = 8;
            // 
            // mProxyMode
            // 
            this.mProxyMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.mProxyMode.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mProxyMode.IconRatio = 1F;
            menuItem37.Badge = "";
            menuItem37.IconSvg = "ProjectFilled";
            menuItem37.ID = "miProxyList";
            menuItem37.LocalizationText = "ProxyModeForm.{id}";
            menuItem37.Select = true;
            menuItem37.Text = "代理数据";
            menuItem38.IconSvg = "DesktopOutlined";
            menuItem38.ID = "miClientList";
            menuItem38.LocalizationText = "ProxyModeForm.{id}";
            menuItem38.Text = "客户端列表";
            menuItem39.IconSvg = "TeamOutlined";
            menuItem39.ID = "miAccountList";
            menuItem39.LocalizationText = "ProxyModeForm.{id}";
            menuItem39.Text = "账号列表";
            menuItem40.IconSvg = "FilterOutlined";
            menuItem40.ID = "miFilterList";
            menuItem40.LocalizationText = "ProxyModeForm.{id}";
            menuItem40.Text = "滤镜列表";
            menuItem41.IconSvg = "SendOutlined";
            menuItem41.ID = "miSendList";
            menuItem41.LocalizationText = "ProxyModeForm.{id}";
            menuItem41.Text = "发送列表";
            menuItem42.IconSvg = "RobotOutlined";
            menuItem42.ID = "miRobotList";
            menuItem42.LocalizationText = "ProxyModeForm.{id}";
            menuItem42.Text = "机器人列表";
            menuItem43.IconSvg = "PieChartOutlined";
            menuItem43.ID = "miStatistical";
            menuItem43.LocalizationText = "ProxyModeForm.{id}";
            menuItem43.Text = "统计数据";
            menuItem44.IconSvg = "DiffOutlined";
            menuItem44.ID = "miComparison";
            menuItem44.LocalizationText = "ProxyModeForm.{id}";
            menuItem44.Text = "文本对比";
            menuItem45.IconSvg = "BuildOutlined";
            menuItem45.ID = "miXOR";
            menuItem45.LocalizationText = "ProxyModeForm.{id}";
            menuItem45.Text = "异或计算";
            menuItem46.IconSvg = "InteractionOutlined";
            menuItem46.ID = "miTranscoding";
            menuItem46.LocalizationText = "ProxyModeForm.{id}";
            menuItem46.Text = "编码转换";
            menuItem47.IconSvg = "DeliveredProcedureOutlined";
            menuItem47.ID = "miExtraction";
            menuItem47.LocalizationText = "ProxyModeForm.{id}";
            menuItem47.Text = "数据提取";
            menuItem48.Badge = "";
            menuItem48.IconSvg = "ExceptionOutlined";
            menuItem48.ID = "miSystemLog";
            menuItem48.LocalizationText = "ProxyModeForm.{id}";
            menuItem48.Text = "系统日志";
            this.mProxyMode.Items.Add(menuItem37);
            this.mProxyMode.Items.Add(menuItem38);
            this.mProxyMode.Items.Add(menuItem39);
            this.mProxyMode.Items.Add(menuItem40);
            this.mProxyMode.Items.Add(menuItem41);
            this.mProxyMode.Items.Add(menuItem42);
            this.mProxyMode.Items.Add(menuItem43);
            this.mProxyMode.Items.Add(menuItem44);
            this.mProxyMode.Items.Add(menuItem45);
            this.mProxyMode.Items.Add(menuItem46);
            this.mProxyMode.Items.Add(menuItem47);
            this.mProxyMode.Items.Add(menuItem48);
            this.mProxyMode.Location = new System.Drawing.Point(3, 49);
            this.mProxyMode.Name = "mProxyMode";
            this.mProxyMode.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.mProxyMode.Size = new System.Drawing.Size(164, 690);
            this.mProxyMode.TabIndex = 5;
            this.mProxyMode.SelectChanged += new AntdUI.SelectEventHandler(this.mProxyMode_SelectChanged);
            // 
            // bMenuCollapse
            // 
            this.bMenuCollapse.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bMenuCollapse.Dock = System.Windows.Forms.DockStyle.Left;
            this.bMenuCollapse.Ghost = true;
            this.bMenuCollapse.IconSvg = "MenuOutlined";
            this.bMenuCollapse.Location = new System.Drawing.Point(1, 1);
            this.bMenuCollapse.Margin = new System.Windows.Forms.Padding(1);
            this.bMenuCollapse.Name = "bMenuCollapse";
            this.bMenuCollapse.Size = new System.Drawing.Size(44, 44);
            this.bMenuCollapse.TabIndex = 6;
            this.bMenuCollapse.WaveSize = 0;
            this.bMenuCollapse.Click += new System.EventHandler(this.bMenuCollapse_Click);
            // 
            // tabProxyMode
            // 
            this.tabProxyMode.Controls.Add(this.tpProxyList);
            this.tabProxyMode.Controls.Add(this.tpClientList);
            this.tabProxyMode.Controls.Add(this.tpAccountList);
            this.tabProxyMode.Controls.Add(this.tpFilterList);
            this.tabProxyMode.Controls.Add(this.tpSendList);
            this.tabProxyMode.Controls.Add(this.tpRobotList);
            this.tabProxyMode.Controls.Add(this.tpStatistical);
            this.tabProxyMode.Controls.Add(this.tpComparison);
            this.tabProxyMode.Controls.Add(this.tpXOR);
            this.tabProxyMode.Controls.Add(this.tpTranscoding);
            this.tabProxyMode.Controls.Add(this.tpExtraction);
            this.tabProxyMode.Controls.Add(this.tpSystemLog);
            this.tabProxyMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProxyMode.Location = new System.Drawing.Point(170, 60);
            this.tabProxyMode.Name = "tabProxyMode";
            this.tabProxyMode.Pages.Add(this.tpProxyList);
            this.tabProxyMode.Pages.Add(this.tpClientList);
            this.tabProxyMode.Pages.Add(this.tpAccountList);
            this.tabProxyMode.Pages.Add(this.tpFilterList);
            this.tabProxyMode.Pages.Add(this.tpSendList);
            this.tabProxyMode.Pages.Add(this.tpRobotList);
            this.tabProxyMode.Pages.Add(this.tpStatistical);
            this.tabProxyMode.Pages.Add(this.tpComparison);
            this.tabProxyMode.Pages.Add(this.tpXOR);
            this.tabProxyMode.Pages.Add(this.tpTranscoding);
            this.tabProxyMode.Pages.Add(this.tpExtraction);
            this.tabProxyMode.Pages.Add(this.tpSystemLog);
            this.tabProxyMode.Size = new System.Drawing.Size(1280, 742);
            this.tabProxyMode.Style = styleLine4;
            this.tabProxyMode.TabIndex = 11;
            this.tabProxyMode.Text = "tabs1";
            // 
            // tpProxyList
            // 
            this.tpProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxyList.Location = new System.Drawing.Point(0, 36);
            this.tpProxyList.Name = "tpProxyList";
            this.tpProxyList.Size = new System.Drawing.Size(1280, 706);
            this.tpProxyList.TabIndex = 0;
            this.tpProxyList.Text = "代理数据";
            // 
            // tpClientList
            // 
            this.tpClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpClientList.Location = new System.Drawing.Point(0, 36);
            this.tpClientList.Name = "tpClientList";
            this.tpClientList.Size = new System.Drawing.Size(1280, 706);
            this.tpClientList.TabIndex = 8;
            this.tpClientList.Text = "客户端列表";
            // 
            // tpAccountList
            // 
            this.tpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpAccountList.Location = new System.Drawing.Point(0, 36);
            this.tpAccountList.Name = "tpAccountList";
            this.tpAccountList.Size = new System.Drawing.Size(1280, 706);
            this.tpAccountList.TabIndex = 7;
            this.tpAccountList.Text = "账号列表";
            // 
            // tpFilterList
            // 
            this.tpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterList.Location = new System.Drawing.Point(0, 36);
            this.tpFilterList.Name = "tpFilterList";
            this.tpFilterList.Size = new System.Drawing.Size(1280, 706);
            this.tpFilterList.TabIndex = 9;
            this.tpFilterList.Text = "滤镜列表";
            // 
            // tpSendList
            // 
            this.tpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSendList.Location = new System.Drawing.Point(0, 36);
            this.tpSendList.Name = "tpSendList";
            this.tpSendList.Size = new System.Drawing.Size(1280, 706);
            this.tpSendList.TabIndex = 10;
            this.tpSendList.Text = "发送列表";
            // 
            // tpRobotList
            // 
            this.tpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRobotList.Location = new System.Drawing.Point(0, 36);
            this.tpRobotList.Name = "tpRobotList";
            this.tpRobotList.Size = new System.Drawing.Size(1280, 706);
            this.tpRobotList.TabIndex = 11;
            this.tpRobotList.Text = "机器人列表";
            // 
            // tpStatistical
            // 
            this.tpStatistical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpStatistical.Location = new System.Drawing.Point(0, 36);
            this.tpStatistical.Name = "tpStatistical";
            this.tpStatistical.Size = new System.Drawing.Size(1280, 706);
            this.tpStatistical.TabIndex = 1;
            this.tpStatistical.Text = "统计数据";
            // 
            // tpComparison
            // 
            this.tpComparison.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpComparison.Location = new System.Drawing.Point(0, 36);
            this.tpComparison.Name = "tpComparison";
            this.tpComparison.Size = new System.Drawing.Size(1280, 706);
            this.tpComparison.TabIndex = 12;
            this.tpComparison.Text = "文本对比";
            // 
            // tpXOR
            // 
            this.tpXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpXOR.Location = new System.Drawing.Point(0, 36);
            this.tpXOR.Name = "tpXOR";
            this.tpXOR.Size = new System.Drawing.Size(1280, 706);
            this.tpXOR.TabIndex = 13;
            this.tpXOR.Text = "异或计算";
            // 
            // tpTranscoding
            // 
            this.tpTranscoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTranscoding.Location = new System.Drawing.Point(0, 36);
            this.tpTranscoding.Name = "tpTranscoding";
            this.tpTranscoding.Size = new System.Drawing.Size(1280, 706);
            this.tpTranscoding.TabIndex = 14;
            this.tpTranscoding.Text = "编码转换";
            // 
            // tpExtraction
            // 
            this.tpExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpExtraction.Location = new System.Drawing.Point(0, 36);
            this.tpExtraction.Name = "tpExtraction";
            this.tpExtraction.Size = new System.Drawing.Size(1280, 706);
            this.tpExtraction.TabIndex = 15;
            this.tpExtraction.Text = "数据提取";
            // 
            // tpSystemLog
            // 
            this.tpSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSystemLog.Location = new System.Drawing.Point(0, 36);
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.Size = new System.Drawing.Size(1280, 706);
            this.tpSystemLog.TabIndex = 6;
            this.tpSystemLog.Text = "系统日志";
            // 
            // timerProxyList
            // 
            this.timerProxyList.Enabled = true;
            this.timerProxyList.Interval = 10;
            this.timerProxyList.Tick += new System.EventHandler(this.timerProxyList_Tick);
            // 
            // timerProxyListInfo
            // 
            this.timerProxyListInfo.Enabled = true;
            this.timerProxyListInfo.Interval = 1000;
            this.timerProxyListInfo.Tick += new System.EventHandler(this.timerProxyListInfo_Tick);
            // 
            // timerAutoSave
            // 
            this.timerAutoSave.Interval = 1000;
            this.timerAutoSave.Tick += new System.EventHandler(this.timerAutoSave_Tick);
            // 
            // bgwAutoSave
            // 
            this.bgwAutoSave.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwAutoSave_DoWork);
            // 
            // ProxyModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1450, 802);
            this.Controls.Add(this.tabProxyMode);
            this.Controls.Add(this.tlpMenu);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(660, 400);
            this.Name = "ProxyModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WPE x64";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProxyModeForm_FormClosing);
            this.Load += new System.EventHandler(this.ProxyModeForm_Load);
            this.pageHeader.ResumeLayout(false);
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
            this.tabProxyMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Button btn_setting;
        private TableLayoutPanelEx tlpMenu;
        private AntdUI.Menu mProxyMode;
        private AntdUI.Button bMenuCollapse;
        private AntdUI.Tabs tabProxyMode;
        private AntdUI.TabPage tpProxyList;
        private AntdUI.TabPage tpAccountList;
        private AntdUI.TabPage tpStatistical;
        private AntdUI.TabPage tpSystemLog;
        private AntdUI.TabPage tpClientList;
        private System.Windows.Forms.Timer timerProxyList;
        private System.Windows.Forms.Timer timerProxyListInfo;
        private AntdUI.TabPage tpFilterList;
        private AntdUI.TabPage tpSendList;
        private AntdUI.TabPage tpRobotList;
        private AntdUI.TabPage tpComparison;
        private AntdUI.TabPage tpXOR;
        private AntdUI.TabPage tpTranscoding;
        private AntdUI.TabPage tpExtraction;
        private System.Windows.Forms.Timer timerAutoSave;
        private System.ComponentModel.BackgroundWorker bgwAutoSave;
    }
}


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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem9 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem10 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem11 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem12 = new AntdUI.MenuItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxyModeForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.colorTheme = new AntdUI.ColorPicker();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.btn_setting = new AntdUI.Button();
            this.tabProxyMode = new AntdUI.Tabs();
            this.tpProxyList = new AntdUI.TabPage();
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
            this.tpAccountList = new AntdUI.TabPage();
            this.tlpMenu = new WinsockPacketEditor.TableLayoutPanelEx();
            this.mProxyMode = new AntdUI.Menu();
            this.bMenuCollapse = new AntdUI.Button();
            this.tpClientList = new AntdUI.TabPage();
            this.tpFilterList = new AntdUI.TabPage();
            this.tpSendList = new AntdUI.TabPage();
            this.tpRobotList = new AntdUI.TabPage();
            this.pageHeader.SuspendLayout();
            this.tabProxyMode.SuspendLayout();
            this.tlpMenu.SuspendLayout();
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
            this.tabProxyMode.Cursor = System.Windows.Forms.Cursors.Hand;
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
            this.tabProxyMode.Style = styleLine1;
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
            // tpAccountList
            // 
            this.tpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpAccountList.Location = new System.Drawing.Point(0, 36);
            this.tpAccountList.Name = "tpAccountList";
            this.tpAccountList.Size = new System.Drawing.Size(1280, 706);
            this.tpAccountList.TabIndex = 16;
            this.tpAccountList.Text = "账号列表";
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
            menuItem1.Badge = "";
            menuItem1.IconSvg = "ProjectFilled";
            menuItem1.ID = "miProxyList";
            menuItem1.LocalizationText = "ProxyModeForm.{id}";
            menuItem1.Select = true;
            menuItem1.Text = "代理数据";
            menuItem2.IconSvg = "DesktopOutlined";
            menuItem2.ID = "miClientList";
            menuItem2.LocalizationText = "ProxyModeForm.{id}";
            menuItem2.Text = "客户端列表";
            menuItem3.IconSvg = "TeamOutlined";
            menuItem3.ID = "miAccountList";
            menuItem3.LocalizationText = "ProxyModeForm.{id}";
            menuItem3.Text = "账号列表";
            menuItem4.IconSvg = "FilterOutlined";
            menuItem4.ID = "miFilterList";
            menuItem4.LocalizationText = "ProxyModeForm.{id}";
            menuItem4.Text = "滤镜列表";
            menuItem5.IconSvg = "SendOutlined";
            menuItem5.ID = "miSendList";
            menuItem5.LocalizationText = "ProxyModeForm.{id}";
            menuItem5.Text = "发送列表";
            menuItem6.IconSvg = "RobotOutlined";
            menuItem6.ID = "miRobotList";
            menuItem6.LocalizationText = "ProxyModeForm.{id}";
            menuItem6.Text = "机器人列表";
            menuItem7.IconSvg = "PieChartOutlined";
            menuItem7.ID = "miStatistical";
            menuItem7.LocalizationText = "ProxyModeForm.{id}";
            menuItem7.Text = "统计数据";
            menuItem8.IconSvg = "DiffOutlined";
            menuItem8.ID = "miComparison";
            menuItem8.LocalizationText = "ProxyModeForm.{id}";
            menuItem8.Text = "文本对比";
            menuItem9.IconSvg = "BuildOutlined";
            menuItem9.ID = "miXOR";
            menuItem9.LocalizationText = "ProxyModeForm.{id}";
            menuItem9.Text = "异或计算";
            menuItem10.IconSvg = "InteractionOutlined";
            menuItem10.ID = "miTranscoding";
            menuItem10.LocalizationText = "ProxyModeForm.{id}";
            menuItem10.Text = "编码转换";
            menuItem11.IconSvg = "DeliveredProcedureOutlined";
            menuItem11.ID = "miExtraction";
            menuItem11.LocalizationText = "ProxyModeForm.{id}";
            menuItem11.Text = "数据提取";
            menuItem12.Badge = "";
            menuItem12.IconSvg = "ExceptionOutlined";
            menuItem12.ID = "miSystemLog";
            menuItem12.LocalizationText = "ProxyModeForm.{id}";
            menuItem12.Text = "系统日志";
            this.mProxyMode.Items.Add(menuItem1);
            this.mProxyMode.Items.Add(menuItem2);
            this.mProxyMode.Items.Add(menuItem3);
            this.mProxyMode.Items.Add(menuItem4);
            this.mProxyMode.Items.Add(menuItem5);
            this.mProxyMode.Items.Add(menuItem6);
            this.mProxyMode.Items.Add(menuItem7);
            this.mProxyMode.Items.Add(menuItem8);
            this.mProxyMode.Items.Add(menuItem9);
            this.mProxyMode.Items.Add(menuItem10);
            this.mProxyMode.Items.Add(menuItem11);
            this.mProxyMode.Items.Add(menuItem12);
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
            // tpClientList
            // 
            this.tpClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpClientList.Location = new System.Drawing.Point(0, 36);
            this.tpClientList.Name = "tpClientList";
            this.tpClientList.Size = new System.Drawing.Size(1280, 706);
            this.tpClientList.TabIndex = 17;
            this.tpClientList.Text = "客户端列表";
            // 
            // tpFilterList
            // 
            this.tpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterList.Location = new System.Drawing.Point(0, 36);
            this.tpFilterList.Name = "tpFilterList";
            this.tpFilterList.Size = new System.Drawing.Size(1280, 706);
            this.tpFilterList.TabIndex = 18;
            this.tpFilterList.Text = "滤镜列表";
            // 
            // tpSendList
            // 
            this.tpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSendList.Location = new System.Drawing.Point(0, 36);
            this.tpSendList.Name = "tpSendList";
            this.tpSendList.Size = new System.Drawing.Size(1280, 706);
            this.tpSendList.TabIndex = 19;
            this.tpSendList.Text = "发送列表";
            // 
            // tpRobotList
            // 
            this.tpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRobotList.Location = new System.Drawing.Point(0, 36);
            this.tpRobotList.Name = "tpRobotList";
            this.tpRobotList.Size = new System.Drawing.Size(1280, 706);
            this.tpRobotList.TabIndex = 20;
            this.tpRobotList.Text = "机器人列表";
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
            this.tabProxyMode.ResumeLayout(false);
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
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
        private AntdUI.TabPage tpStatistical;
        private AntdUI.TabPage tpSystemLog;
        private System.Windows.Forms.Timer timerProxyList;
        private System.Windows.Forms.Timer timerProxyListInfo;
        private AntdUI.TabPage tpComparison;
        private AntdUI.TabPage tpXOR;
        private AntdUI.TabPage tpTranscoding;
        private AntdUI.TabPage tpExtraction;
        private System.Windows.Forms.Timer timerAutoSave;
        private System.ComponentModel.BackgroundWorker bgwAutoSave;
        private AntdUI.TabPage tpAccountList;
        private AntdUI.TabPage tpClientList;
        private AntdUI.TabPage tpFilterList;
        private AntdUI.TabPage tpSendList;
        private AntdUI.TabPage tpRobotList;
    }
}


namespace WinsockPacketEditor
{
    partial class InjectModeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InjectModeForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.colorTheme = new AntdUI.ColorPicker();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.btn_setting = new AntdUI.Button();
            this.tlpMenu = new WinsockPacketEditor.TableLayoutPanelEx();
            this.mInjectMode = new AntdUI.Menu();
            this.bMenuCollapse = new AntdUI.Button();
            this.tabInjectMode = new AntdUI.Tabs();
            this.tpPacketList = new AntdUI.TabPage();
            this.tpStatistical = new AntdUI.TabPage();
            this.tpComparison = new AntdUI.TabPage();
            this.tpXOR = new AntdUI.TabPage();
            this.tpTranscoding = new AntdUI.TabPage();
            this.tpExtraction = new AntdUI.TabPage();
            this.tpSystemLog = new AntdUI.TabPage();
            this.timerPacketList = new System.Windows.Forms.Timer(this.components);
            this.timerPacketListInfo = new System.Windows.Forms.Timer(this.components);
            this.timerAutoSave = new System.Windows.Forms.Timer(this.components);
            this.bgwAutoSave = new System.ComponentModel.BackgroundWorker();
            this.tpFilterList = new AntdUI.TabPage();
            this.tpSendList = new AntdUI.TabPage();
            this.tpRobotList = new AntdUI.TabPage();
            this.pageHeader.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.tabInjectMode.SuspendLayout();
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
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1450, 60);
            this.pageHeader.SubText = "2.0.0.0";
            this.pageHeader.TabIndex = 6;
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
            this.tlpMenu.Controls.Add(this.mInjectMode, 0, 1);
            this.tlpMenu.Controls.Add(this.bMenuCollapse, 0, 0);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.tlpMenu.Location = new System.Drawing.Point(0, 60);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 2;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Size = new System.Drawing.Size(170, 742);
            this.tlpMenu.TabIndex = 7;
            // 
            // mInjectMode
            // 
            this.mInjectMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.mInjectMode.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mInjectMode.IconRatio = 1F;
            menuItem1.Badge = "";
            menuItem1.IconSvg = "ProjectFilled";
            menuItem1.ID = "miPacketList";
            menuItem1.LocalizationText = "InjectModeForm.{id}";
            menuItem1.Select = true;
            menuItem1.Text = "封包数据";
            menuItem2.Badge = "";
            menuItem2.IconSvg = "FilterOutlined";
            menuItem2.ID = "miFilterList";
            menuItem2.LocalizationText = "InjectModeForm.{id}";
            menuItem2.Text = "滤镜列表";
            menuItem3.Badge = "";
            menuItem3.IconSvg = "SendOutlined";
            menuItem3.ID = "miSendList";
            menuItem3.LocalizationText = "InjectModeForm.{id}";
            menuItem3.Text = "发送列表";
            menuItem4.Badge = "";
            menuItem4.IconSvg = "RobotOutlined";
            menuItem4.ID = "miRobotList";
            menuItem4.LocalizationText = "InjectModeForm.{id}";
            menuItem4.Text = "机器人列表";
            menuItem5.IconSvg = "PieChartOutlined";
            menuItem5.ID = "miStatistical";
            menuItem5.LocalizationText = "InjectModeForm.{id}";
            menuItem5.Text = "统计数据";
            menuItem6.IconSvg = "DiffOutlined";
            menuItem6.ID = "miComparison";
            menuItem6.LocalizationText = "InjectModeForm.{id}";
            menuItem6.Text = "文本对比";
            menuItem7.IconSvg = "BuildOutlined";
            menuItem7.ID = "miXOR";
            menuItem7.LocalizationText = "InjectModeForm.{id}";
            menuItem7.Text = "异或计算";
            menuItem8.IconSvg = "InteractionOutlined";
            menuItem8.ID = "miTranscoding";
            menuItem8.LocalizationText = "InjectModeForm.{id}";
            menuItem8.Text = "编码转换";
            menuItem9.IconSvg = "DeliveredProcedureOutlined";
            menuItem9.ID = "miExtraction";
            menuItem9.LocalizationText = "InjectModeForm.{id}";
            menuItem9.Text = "数据提取";
            menuItem10.Badge = "";
            menuItem10.IconSvg = "ExceptionOutlined";
            menuItem10.ID = "miSystemLog";
            menuItem10.LocalizationText = "InjectModeForm.{id}";
            menuItem10.Text = "系统日志";
            this.mInjectMode.Items.Add(menuItem1);
            this.mInjectMode.Items.Add(menuItem2);
            this.mInjectMode.Items.Add(menuItem3);
            this.mInjectMode.Items.Add(menuItem4);
            this.mInjectMode.Items.Add(menuItem5);
            this.mInjectMode.Items.Add(menuItem6);
            this.mInjectMode.Items.Add(menuItem7);
            this.mInjectMode.Items.Add(menuItem8);
            this.mInjectMode.Items.Add(menuItem9);
            this.mInjectMode.Items.Add(menuItem10);
            this.mInjectMode.Location = new System.Drawing.Point(3, 49);
            this.mInjectMode.Name = "mInjectMode";
            this.mInjectMode.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.mInjectMode.Size = new System.Drawing.Size(164, 690);
            this.mInjectMode.TabIndex = 5;
            this.mInjectMode.SelectChanged += new AntdUI.SelectEventHandler(this.mInjectMode_SelectChanged);
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
            // tabInjectMode
            // 
            this.tabInjectMode.Controls.Add(this.tpPacketList);
            this.tabInjectMode.Controls.Add(this.tpFilterList);
            this.tabInjectMode.Controls.Add(this.tpSendList);
            this.tabInjectMode.Controls.Add(this.tpRobotList);
            this.tabInjectMode.Controls.Add(this.tpStatistical);
            this.tabInjectMode.Controls.Add(this.tpComparison);
            this.tabInjectMode.Controls.Add(this.tpXOR);
            this.tabInjectMode.Controls.Add(this.tpTranscoding);
            this.tabInjectMode.Controls.Add(this.tpExtraction);
            this.tabInjectMode.Controls.Add(this.tpSystemLog);
            this.tabInjectMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInjectMode.Location = new System.Drawing.Point(170, 60);
            this.tabInjectMode.Name = "tabInjectMode";
            this.tabInjectMode.Pages.Add(this.tpPacketList);
            this.tabInjectMode.Pages.Add(this.tpFilterList);
            this.tabInjectMode.Pages.Add(this.tpSendList);
            this.tabInjectMode.Pages.Add(this.tpRobotList);
            this.tabInjectMode.Pages.Add(this.tpStatistical);
            this.tabInjectMode.Pages.Add(this.tpComparison);
            this.tabInjectMode.Pages.Add(this.tpXOR);
            this.tabInjectMode.Pages.Add(this.tpTranscoding);
            this.tabInjectMode.Pages.Add(this.tpExtraction);
            this.tabInjectMode.Pages.Add(this.tpSystemLog);
            this.tabInjectMode.Size = new System.Drawing.Size(1280, 742);
            this.tabInjectMode.Style = styleLine1;
            this.tabInjectMode.TabIndex = 10;
            this.tabInjectMode.Text = "tabs1";
            // 
            // tpPacketList
            // 
            this.tpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpPacketList.Location = new System.Drawing.Point(0, 36);
            this.tpPacketList.Name = "tpPacketList";
            this.tpPacketList.Size = new System.Drawing.Size(1280, 706);
            this.tpPacketList.TabIndex = 0;
            this.tpPacketList.Text = "封包列表";
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
            this.tpComparison.TabIndex = 2;
            this.tpComparison.Text = "文本对比";
            // 
            // tpXOR
            // 
            this.tpXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpXOR.Location = new System.Drawing.Point(0, 36);
            this.tpXOR.Name = "tpXOR";
            this.tpXOR.Size = new System.Drawing.Size(1280, 706);
            this.tpXOR.TabIndex = 3;
            this.tpXOR.Text = "异或计算";
            // 
            // tpTranscoding
            // 
            this.tpTranscoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTranscoding.Location = new System.Drawing.Point(0, 36);
            this.tpTranscoding.Name = "tpTranscoding";
            this.tpTranscoding.Size = new System.Drawing.Size(1280, 706);
            this.tpTranscoding.TabIndex = 4;
            this.tpTranscoding.Text = "编码转换";
            // 
            // tpExtraction
            // 
            this.tpExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpExtraction.Location = new System.Drawing.Point(0, 36);
            this.tpExtraction.Name = "tpExtraction";
            this.tpExtraction.Size = new System.Drawing.Size(1280, 706);
            this.tpExtraction.TabIndex = 5;
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
            // timerPacketList
            // 
            this.timerPacketList.Enabled = true;
            this.timerPacketList.Interval = 10;
            this.timerPacketList.Tick += new System.EventHandler(this.timerPacketList_Tick);
            // 
            // timerPacketListInfo
            // 
            this.timerPacketListInfo.Enabled = true;
            this.timerPacketListInfo.Interval = 1000;
            this.timerPacketListInfo.Tick += new System.EventHandler(this.timerPacketListInfo_Tick);
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
            // tpFilterList
            // 
            this.tpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterList.Location = new System.Drawing.Point(0, 36);
            this.tpFilterList.Name = "tpFilterList";
            this.tpFilterList.Size = new System.Drawing.Size(1280, 706);
            this.tpFilterList.TabIndex = 7;
            this.tpFilterList.Text = "滤镜列表";
            // 
            // tpSendList
            // 
            this.tpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSendList.Location = new System.Drawing.Point(0, 36);
            this.tpSendList.Name = "tpSendList";
            this.tpSendList.Size = new System.Drawing.Size(1280, 706);
            this.tpSendList.TabIndex = 8;
            this.tpSendList.Text = "发送列表";
            // 
            // tpRobotList
            // 
            this.tpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRobotList.Location = new System.Drawing.Point(0, 36);
            this.tpRobotList.Name = "tpRobotList";
            this.tpRobotList.Size = new System.Drawing.Size(1280, 706);
            this.tpRobotList.TabIndex = 9;
            this.tpRobotList.Text = "机器人列表";
            // 
            // InjectModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1450, 802);
            this.Controls.Add(this.tabInjectMode);
            this.Controls.Add(this.tlpMenu);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MinimumSize = new System.Drawing.Size(660, 400);
            this.Name = "InjectModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WPE x64";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InjectModeForm_FormClosing);
            this.Load += new System.EventHandler(this.InjectModeForm_Load);
            this.pageHeader.ResumeLayout(false);
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
            this.tabInjectMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.PageHeader pageHeader;
        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Button btn_setting;
        private TableLayoutPanelEx tlpMenu;
        private AntdUI.Menu mInjectMode;
        private AntdUI.Button bMenuCollapse;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Tabs tabInjectMode;
        private AntdUI.TabPage tpPacketList;
        private AntdUI.TabPage tpStatistical;
        private AntdUI.TabPage tpComparison;
        private AntdUI.TabPage tpXOR;
        private AntdUI.TabPage tpTranscoding;
        private AntdUI.TabPage tpExtraction;
        private AntdUI.TabPage tpSystemLog;
        private System.Windows.Forms.Timer timerPacketList;
        private System.Windows.Forms.Timer timerPacketListInfo;
        private System.Windows.Forms.Timer timerAutoSave;
        private System.ComponentModel.BackgroundWorker bgwAutoSave;
        private AntdUI.TabPage tpFilterList;
        private AntdUI.TabPage tpSendList;
        private AntdUI.TabPage tpRobotList;
    }
}
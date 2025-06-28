namespace WPE.InjectMode
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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            AntdUI.SegmentedItem segmentedItem1 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem2 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem3 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem4 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem5 = new AntdUI.SegmentedItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InjectModeForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.colorTheme = new AntdUI.ColorPicker();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.btn_setting = new AntdUI.Button();
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.mInjectMode = new AntdUI.Menu();
            this.bMenuCollapse = new AntdUI.Button();
            this.tabInjectMode = new AntdUI.Tabs();
            this.tpPacketList = new AntdUI.TabPage();
            this.tpStatistical = new AntdUI.TabPage();
            this.tpComparison = new AntdUI.TabPage();
            this.tpXOR = new AntdUI.TabPage();
            this.tpTranscoding = new AntdUI.TabPage();
            this.tpExtraction = new AntdUI.TabPage();
            this.tpLog = new AntdUI.TabPage();
            this.tlpPacketList = new System.Windows.Forms.TableLayoutPanel();
            this.segmented = new AntdUI.Segmented();
            this.pageHeader.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.tabInjectMode.SuspendLayout();
            this.tpPacketList.SuspendLayout();
            this.tlpPacketList.SuspendLayout();
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
            this.pageHeader.DividerShow = true;
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1300, 40);
            this.pageHeader.SubText = "2.0.0.0";
            this.pageHeader.TabIndex = 6;
            this.pageHeader.Text = "WPE x64";
            // 
            // colorTheme
            // 
            this.colorTheme.Dock = System.Windows.Forms.DockStyle.Right;
            this.colorTheme.Location = new System.Drawing.Point(966, 0);
            this.colorTheme.Name = "colorTheme";
            this.colorTheme.Padding = new System.Windows.Forms.Padding(5);
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
            this.btn_mode.Location = new System.Drawing.Point(1006, 0);
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
            this.btn_global.Location = new System.Drawing.Point(1056, 0);
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
            this.btn_setting.Location = new System.Drawing.Point(1106, 0);
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
            this.tlpMenu.Location = new System.Drawing.Point(0, 40);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 2;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Size = new System.Drawing.Size(170, 680);
            this.tlpMenu.TabIndex = 7;
            // 
            // mInjectMode
            // 
            this.mInjectMode.Dock = System.Windows.Forms.DockStyle.Left;
            menuItem1.IconSvg = "ProjectFilled";
            menuItem1.ID = "miPacketList";
            menuItem1.LocalizationText = "InjectModeForm.{id}";
            menuItem1.Select = true;
            menuItem1.Text = "封包列表";
            menuItem2.IconSvg = "AreaChartOutlined";
            menuItem2.ID = "miStatistical";
            menuItem2.LocalizationText = "InjectModeForm.{id}";
            menuItem2.Text = "统计数据";
            menuItem3.IconSvg = "SnippetsFilled";
            menuItem3.ID = "miComparison";
            menuItem3.LocalizationText = "InjectModeForm.{id}";
            menuItem3.Text = "文本对比";
            menuItem4.IconSvg = "CalculatorFilled";
            menuItem4.ID = "miXOR";
            menuItem4.LocalizationText = "InjectModeForm.{id}";
            menuItem4.Text = "异或计算";
            menuItem5.IconSvg = "BuildFilled";
            menuItem5.ID = "miTranscoding";
            menuItem5.LocalizationText = "InjectModeForm.{id}";
            menuItem5.Text = "编码转换";
            menuItem6.IconSvg = "SaveFilled";
            menuItem6.ID = "miExtraction";
            menuItem6.LocalizationText = "InjectModeForm.{id}";
            menuItem6.Text = "数据提取";
            menuItem7.IconSvg = "ContainerFilled";
            menuItem7.ID = "miSystemLog";
            menuItem7.LocalizationText = "InjectModeForm.{id}";
            menuItem7.Text = "系统日志";
            this.mInjectMode.Items.Add(menuItem1);
            this.mInjectMode.Items.Add(menuItem2);
            this.mInjectMode.Items.Add(menuItem3);
            this.mInjectMode.Items.Add(menuItem4);
            this.mInjectMode.Items.Add(menuItem5);
            this.mInjectMode.Items.Add(menuItem6);
            this.mInjectMode.Items.Add(menuItem7);
            this.mInjectMode.Location = new System.Drawing.Point(3, 51);
            this.mInjectMode.Name = "mInjectMode";
            this.mInjectMode.Size = new System.Drawing.Size(164, 626);
            this.mInjectMode.TabIndex = 5;
            this.mInjectMode.SelectChanged += new AntdUI.SelectEventHandler(this.mInjectMode_SelectChanged);
            // 
            // bMenuCollapse
            // 
            this.bMenuCollapse.BorderWidth = 1F;
            this.bMenuCollapse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bMenuCollapse.Ghost = true;
            this.bMenuCollapse.IconRatio = 1F;
            this.bMenuCollapse.IconSvg = "MenuFoldOutlined";
            this.bMenuCollapse.Location = new System.Drawing.Point(1, 1);
            this.bMenuCollapse.Margin = new System.Windows.Forms.Padding(1);
            this.bMenuCollapse.Name = "bMenuCollapse";
            this.bMenuCollapse.Size = new System.Drawing.Size(168, 46);
            this.bMenuCollapse.TabIndex = 6;
            this.bMenuCollapse.Click += new System.EventHandler(this.bMenuCollapse_Click);
            // 
            // tabInjectMode
            // 
            this.tabInjectMode.Controls.Add(this.tpPacketList);
            this.tabInjectMode.Controls.Add(this.tpStatistical);
            this.tabInjectMode.Controls.Add(this.tpComparison);
            this.tabInjectMode.Controls.Add(this.tpXOR);
            this.tabInjectMode.Controls.Add(this.tpTranscoding);
            this.tabInjectMode.Controls.Add(this.tpExtraction);
            this.tabInjectMode.Controls.Add(this.tpLog);
            this.tabInjectMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInjectMode.Location = new System.Drawing.Point(170, 40);
            this.tabInjectMode.Name = "tabInjectMode";
            this.tabInjectMode.Pages.Add(this.tpPacketList);
            this.tabInjectMode.Pages.Add(this.tpStatistical);
            this.tabInjectMode.Pages.Add(this.tpComparison);
            this.tabInjectMode.Pages.Add(this.tpXOR);
            this.tabInjectMode.Pages.Add(this.tpTranscoding);
            this.tabInjectMode.Pages.Add(this.tpExtraction);
            this.tabInjectMode.Pages.Add(this.tpLog);
            this.tabInjectMode.Size = new System.Drawing.Size(1130, 680);
            this.tabInjectMode.Style = styleLine1;
            this.tabInjectMode.TabIndex = 10;
            this.tabInjectMode.Text = "tabs1";
            // 
            // tpPacketList
            // 
            this.tpPacketList.Controls.Add(this.tlpPacketList);
            this.tpPacketList.Location = new System.Drawing.Point(3, 33);
            this.tpPacketList.Name = "tpPacketList";
            this.tpPacketList.Size = new System.Drawing.Size(1124, 644);
            this.tpPacketList.TabIndex = 0;
            this.tpPacketList.Text = "封包列表";
            // 
            // tpStatistical
            // 
            this.tpStatistical.Location = new System.Drawing.Point(0, 0);
            this.tpStatistical.Name = "tpStatistical";
            this.tpStatistical.Size = new System.Drawing.Size(0, 0);
            this.tpStatistical.TabIndex = 1;
            this.tpStatistical.Text = "统计数据";
            // 
            // tpComparison
            // 
            this.tpComparison.Location = new System.Drawing.Point(0, 0);
            this.tpComparison.Name = "tpComparison";
            this.tpComparison.Size = new System.Drawing.Size(0, 0);
            this.tpComparison.TabIndex = 2;
            this.tpComparison.Text = "文本对比";
            // 
            // tpXOR
            // 
            this.tpXOR.Location = new System.Drawing.Point(0, 0);
            this.tpXOR.Name = "tpXOR";
            this.tpXOR.Size = new System.Drawing.Size(0, 0);
            this.tpXOR.TabIndex = 3;
            this.tpXOR.Text = "异或计算";
            // 
            // tpTranscoding
            // 
            this.tpTranscoding.Location = new System.Drawing.Point(0, 0);
            this.tpTranscoding.Name = "tpTranscoding";
            this.tpTranscoding.Size = new System.Drawing.Size(0, 0);
            this.tpTranscoding.TabIndex = 4;
            this.tpTranscoding.Text = "编码转换";
            // 
            // tpExtraction
            // 
            this.tpExtraction.Location = new System.Drawing.Point(0, 0);
            this.tpExtraction.Name = "tpExtraction";
            this.tpExtraction.Size = new System.Drawing.Size(0, 0);
            this.tpExtraction.TabIndex = 5;
            this.tpExtraction.Text = "数据提取";
            // 
            // tpLog
            // 
            this.tpLog.Location = new System.Drawing.Point(0, 0);
            this.tpLog.Name = "tpLog";
            this.tpLog.Size = new System.Drawing.Size(0, 0);
            this.tpLog.TabIndex = 6;
            this.tpLog.Text = "系统日志";
            // 
            // tlpPacketList
            // 
            this.tlpPacketList.ColumnCount = 1;
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Controls.Add(this.segmented, 0, 0);
            this.tlpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList.Name = "tlpPacketList";
            this.tlpPacketList.RowCount = 2;
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Size = new System.Drawing.Size(1124, 644);
            this.tlpPacketList.TabIndex = 10;
            // 
            // segmented
            // 
            this.segmented.Dock = System.Windows.Forms.DockStyle.Top;
            this.segmented.IconAlign = AntdUI.TAlignMini.Left;
            segmentedItem1.Badge = null;
            segmentedItem1.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem1.BadgeBack = null;
            segmentedItem1.BadgeMode = false;
            segmentedItem1.BadgeOffsetX = 0;
            segmentedItem1.BadgeOffsetY = 0;
            segmentedItem1.BadgeSize = 0.6F;
            segmentedItem1.BadgeSvg = null;
            segmentedItem1.IconSvg = "FilterOutlined";
            segmentedItem1.Text = "过滤设置";
            segmentedItem2.Badge = null;
            segmentedItem2.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem2.BadgeBack = null;
            segmentedItem2.BadgeMode = false;
            segmentedItem2.BadgeOffsetX = 0;
            segmentedItem2.BadgeOffsetY = 0;
            segmentedItem2.BadgeSize = 0.6F;
            segmentedItem2.BadgeSvg = null;
            segmentedItem2.IconSvg = "AimOutlined";
            segmentedItem2.Text = "拦截设置";
            segmentedItem3.Badge = null;
            segmentedItem3.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem3.BadgeBack = null;
            segmentedItem3.BadgeMode = false;
            segmentedItem3.BadgeOffsetX = 0;
            segmentedItem3.BadgeOffsetY = 0;
            segmentedItem3.BadgeSize = 0.6F;
            segmentedItem3.BadgeSvg = null;
            segmentedItem3.IconSvg = "UnorderedListOutlined";
            segmentedItem3.Text = "列表设置";
            segmentedItem4.Badge = null;
            segmentedItem4.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem4.BadgeBack = null;
            segmentedItem4.BadgeMode = false;
            segmentedItem4.BadgeOffsetX = 0;
            segmentedItem4.BadgeOffsetY = 0;
            segmentedItem4.BadgeSize = 0.6F;
            segmentedItem4.BadgeSvg = null;
            segmentedItem4.IconSvg = "GoldOutlined";
            segmentedItem4.Text = "快捷键";
            segmentedItem5.Badge = null;
            segmentedItem5.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem5.BadgeBack = null;
            segmentedItem5.BadgeMode = false;
            segmentedItem5.BadgeOffsetX = 0;
            segmentedItem5.BadgeOffsetY = 0;
            segmentedItem5.BadgeSize = 0.6F;
            segmentedItem5.BadgeSvg = null;
            segmentedItem5.IconSvg = "SettingOutlined";
            segmentedItem5.Text = "系统设置";
            this.segmented.Items.Add(segmentedItem1);
            this.segmented.Items.Add(segmentedItem2);
            this.segmented.Items.Add(segmentedItem3);
            this.segmented.Items.Add(segmentedItem4);
            this.segmented.Items.Add(segmentedItem5);
            this.segmented.Location = new System.Drawing.Point(3, 3);
            this.segmented.Name = "segmented";
            this.segmented.Size = new System.Drawing.Size(1118, 45);
            this.segmented.TabIndex = 0;
            this.segmented.SelectIndexChanged += new AntdUI.IntEventHandler(this.segmented_SelectIndexChanged);
            // 
            // InjectModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1300, 720);
            this.Controls.Add(this.tabInjectMode);
            this.Controls.Add(this.tlpMenu);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
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
            this.tabInjectMode.ResumeLayout(false);
            this.tpPacketList.ResumeLayout(false);
            this.tlpPacketList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.PageHeader pageHeader;
        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Button btn_setting;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
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
        private AntdUI.TabPage tpLog;
        private System.Windows.Forms.TableLayoutPanel tlpPacketList;
        private AntdUI.Segmented segmented;
    }
}
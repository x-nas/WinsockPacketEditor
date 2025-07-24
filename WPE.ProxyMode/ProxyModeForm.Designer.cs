namespace WPE.ProxyMode
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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            AntdUI.SegmentedItem segmentedItem1 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem2 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem3 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem4 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem5 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem6 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem7 = new AntdUI.SegmentedItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem9 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem10 = new AntdUI.MenuItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxyModeForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.colorTheme = new AntdUI.ColorPicker();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.btn_setting = new AntdUI.Button();
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.mProxyMode = new AntdUI.Menu();
            this.bMenuCollapse = new AntdUI.Button();
            this.tabProxyMode = new AntdUI.Tabs();
            this.tpProxyList = new AntdUI.TabPage();
            this.tlpProxyList = new System.Windows.Forms.TableLayoutPanel();
            this.splitterProxyList = new AntdUI.Splitter();
            this.tProxyList = new AntdUI.Table();
            this.pPacketData = new AntdUI.Panel();
            this.hbProxyData = new Be.Windows.Forms.HexBox();
            this.tlpPacketListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lProxyLinks_CNT = new AntdUI.Label();
            this.lProxyLinks = new AntdUI.Label();
            this.label15 = new AntdUI.Label();
            this.lProxyAccount_CNT = new AntdUI.Label();
            this.lProxyAccount = new AntdUI.Label();
            this.label12 = new AntdUI.Label();
            this.lProxyQueue_CNT = new AntdUI.Label();
            this.lProxyQueue = new AntdUI.Label();
            this.lProxyUDP_CNT = new AntdUI.Label();
            this.lProxyTCP_CNT = new AntdUI.Label();
            this.lProxyTotal_CNT = new AntdUI.Label();
            this.label2 = new AntdUI.Label();
            this.label3 = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.lProxyUDP = new AntdUI.Label();
            this.lProxyTCP = new AntdUI.Label();
            this.lProxyTotal = new AntdUI.Label();
            this.sProxyList = new AntdUI.Segmented();
            this.tlpProxyInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lTotalBytes = new AntdUI.Label();
            this.lSplit1 = new AntdUI.Label();
            this.lProxySpeed = new AntdUI.Label();
            this.tpClientList = new AntdUI.TabPage();
            this.splitterClientList = new AntdUI.Splitter();
            this.treeClientList = new AntdUI.Tree();
            this.tlpAuthInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tAuthList = new AntdUI.Table();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lDevicesCount_Value = new AntdUI.Label();
            this.lLinksCount_Value = new AntdUI.Label();
            this.lAuthCount_Value = new AntdUI.Label();
            this.label18 = new AntdUI.Label();
            this.label19 = new AntdUI.Label();
            this.lDevicesCount = new AntdUI.Label();
            this.lLinksCount = new AntdUI.Label();
            this.lAuthCount = new AntdUI.Label();
            this.tpAccountList = new AntdUI.TabPage();
            this.tlpAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.tAccountList = new AntdUI.Table();
            this.tlpAccountListButton = new System.Windows.Forms.TableLayoutPanel();
            this.mAccountList = new AntdUI.Menu();
            this.txtSearchUserName = new AntdUI.Input();
            this.dtpExpiryTime = new AntdUI.DatePickerRange();
            this.bSearchExpiryTime = new AntdUI.Button();
            this.pAccountList = new AntdUI.Pagination();
            this.tpStatistical = new AntdUI.TabPage();
            this.tpSystemLog = new AntdUI.TabPage();
            this.tSystemLog = new AntdUI.Table();
            this.timerProxyList = new System.Windows.Forms.Timer(this.components);
            this.bgwProxyList = new System.ComponentModel.BackgroundWorker();
            this.timerProxyListInfo = new System.Windows.Forms.Timer(this.components);
            this.bgwClientList = new System.ComponentModel.BackgroundWorker();
            this.pageHeader.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.tabProxyMode.SuspendLayout();
            this.tpProxyList.SuspendLayout();
            this.tlpProxyList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterProxyList)).BeginInit();
            this.splitterProxyList.Panel1.SuspendLayout();
            this.splitterProxyList.Panel2.SuspendLayout();
            this.splitterProxyList.SuspendLayout();
            this.pPacketData.SuspendLayout();
            this.tlpPacketListInfo.SuspendLayout();
            this.tlpProxyInfo.SuspendLayout();
            this.tpClientList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).BeginInit();
            this.splitterClientList.Panel1.SuspendLayout();
            this.splitterClientList.Panel2.SuspendLayout();
            this.splitterClientList.SuspendLayout();
            this.tlpAuthInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tpAccountList.SuspendLayout();
            this.tlpAccountList.SuspendLayout();
            this.tlpAccountListButton.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
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
            this.pageHeader.FullBox = true;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1300, 40);
            this.pageHeader.SubText = "2.0.0.0";
            this.pageHeader.TabIndex = 7;
            this.pageHeader.Text = "WPE x64";
            // 
            // colorTheme
            // 
            this.colorTheme.Dock = System.Windows.Forms.DockStyle.Right;
            this.colorTheme.Location = new System.Drawing.Point(918, 0);
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
            this.btn_mode.Location = new System.Drawing.Point(958, 0);
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
            this.btn_global.Location = new System.Drawing.Point(1008, 0);
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
            this.btn_setting.Location = new System.Drawing.Point(1058, 0);
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
            this.tlpMenu.Location = new System.Drawing.Point(0, 40);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 2;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Size = new System.Drawing.Size(170, 760);
            this.tlpMenu.TabIndex = 8;
            // 
            // mProxyMode
            // 
            this.mProxyMode.Dock = System.Windows.Forms.DockStyle.Left;
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
            menuItem4.IconSvg = "PieChartOutlined";
            menuItem4.ID = "miStatistical";
            menuItem4.LocalizationText = "ProxyModeForm.{id}";
            menuItem4.Text = "统计数据";
            menuItem5.Badge = "";
            menuItem5.IconSvg = "ExceptionOutlined";
            menuItem5.ID = "miSystemLog";
            menuItem5.LocalizationText = "ProxyModeForm.{id}";
            menuItem5.Text = "系统日志";
            this.mProxyMode.Items.Add(menuItem1);
            this.mProxyMode.Items.Add(menuItem2);
            this.mProxyMode.Items.Add(menuItem3);
            this.mProxyMode.Items.Add(menuItem4);
            this.mProxyMode.Items.Add(menuItem5);
            this.mProxyMode.Location = new System.Drawing.Point(3, 51);
            this.mProxyMode.Name = "mProxyMode";
            this.mProxyMode.Size = new System.Drawing.Size(164, 706);
            this.mProxyMode.TabIndex = 5;
            this.mProxyMode.SelectChanged += new AntdUI.SelectEventHandler(this.mProxyMode_SelectChanged);
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
            // tabProxyMode
            // 
            this.tabProxyMode.Controls.Add(this.tpProxyList);
            this.tabProxyMode.Controls.Add(this.tpClientList);
            this.tabProxyMode.Controls.Add(this.tpAccountList);
            this.tabProxyMode.Controls.Add(this.tpStatistical);
            this.tabProxyMode.Controls.Add(this.tpSystemLog);
            this.tabProxyMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProxyMode.Location = new System.Drawing.Point(170, 40);
            this.tabProxyMode.Name = "tabProxyMode";
            this.tabProxyMode.Pages.Add(this.tpProxyList);
            this.tabProxyMode.Pages.Add(this.tpClientList);
            this.tabProxyMode.Pages.Add(this.tpAccountList);
            this.tabProxyMode.Pages.Add(this.tpStatistical);
            this.tabProxyMode.Pages.Add(this.tpSystemLog);
            this.tabProxyMode.SelectedIndex = 1;
            this.tabProxyMode.Size = new System.Drawing.Size(1130, 760);
            this.tabProxyMode.Style = styleLine1;
            this.tabProxyMode.TabIndex = 11;
            this.tabProxyMode.Text = "tabs1";
            // 
            // tpProxyList
            // 
            this.tpProxyList.Controls.Add(this.tlpProxyList);
            this.tpProxyList.Location = new System.Drawing.Point(-1124, -724);
            this.tpProxyList.Name = "tpProxyList";
            this.tpProxyList.Size = new System.Drawing.Size(1124, 724);
            this.tpProxyList.TabIndex = 0;
            this.tpProxyList.Text = "代理数据";
            // 
            // tlpProxyList
            // 
            this.tlpProxyList.ColumnCount = 1;
            this.tlpProxyList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList.Controls.Add(this.splitterProxyList, 0, 2);
            this.tlpProxyList.Controls.Add(this.tlpPacketListInfo, 0, 1);
            this.tlpProxyList.Controls.Add(this.sProxyList, 0, 0);
            this.tlpProxyList.Controls.Add(this.tlpProxyInfo, 0, 3);
            this.tlpProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyList.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyList.Name = "tlpProxyList";
            this.tlpProxyList.RowCount = 4;
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList.Size = new System.Drawing.Size(1124, 724);
            this.tlpProxyList.TabIndex = 10;
            // 
            // splitterProxyList
            // 
            this.splitterProxyList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterProxyList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterProxyList.Location = new System.Drawing.Point(3, 68);
            this.splitterProxyList.Name = "splitterProxyList";
            this.splitterProxyList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterProxyList.Panel1
            // 
            this.splitterProxyList.Panel1.Controls.Add(this.tProxyList);
            this.splitterProxyList.Panel1MinSize = 0;
            // 
            // splitterProxyList.Panel2
            // 
            this.splitterProxyList.Panel2.Controls.Add(this.pPacketData);
            this.splitterProxyList.Panel2MinSize = 0;
            this.splitterProxyList.Size = new System.Drawing.Size(1118, 623);
            this.splitterProxyList.SplitterDistance = 450;
            this.splitterProxyList.SplitterSize = 80;
            this.splitterProxyList.SplitterWidth = 10;
            this.splitterProxyList.TabIndex = 6;
            // 
            // tProxyList
            // 
            this.tProxyList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProxyList.Bordered = true;
            this.tProxyList.CellImpactHeight = false;
            this.tProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProxyList.EmptyHeader = true;
            this.tProxyList.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tProxyList.Gap = 8;
            this.tProxyList.GapCell = 0;
            this.tProxyList.Location = new System.Drawing.Point(0, 0);
            this.tProxyList.MultipleRows = true;
            this.tProxyList.Name = "tProxyList";
            this.tProxyList.Size = new System.Drawing.Size(1118, 450);
            this.tProxyList.TabIndex = 0;
            this.tProxyList.SelectIndexChanged += new System.EventHandler(this.tProxyList_SelectIndexChanged);
            // 
            // pPacketData
            // 
            this.pPacketData.BorderWidth = 1F;
            this.pPacketData.Controls.Add(this.hbProxyData);
            this.pPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData.Location = new System.Drawing.Point(0, 0);
            this.pPacketData.Name = "pPacketData";
            this.pPacketData.Padding = new System.Windows.Forms.Padding(6);
            this.pPacketData.Radius = 0;
            this.pPacketData.Size = new System.Drawing.Size(1118, 163);
            this.pPacketData.TabIndex = 0;
            // 
            // hbProxyData
            // 
            this.hbProxyData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbProxyData.ColumnInfoVisible = true;
            this.hbProxyData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbProxyData.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbProxyData.LineInfoVisible = true;
            this.hbProxyData.Location = new System.Drawing.Point(7, 7);
            this.hbProxyData.Name = "hbProxyData";
            this.hbProxyData.ReadOnly = true;
            this.hbProxyData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbProxyData.Size = new System.Drawing.Size(1104, 149);
            this.hbProxyData.StringViewVisible = true;
            this.hbProxyData.TabIndex = 1;
            this.hbProxyData.VScrollBarVisible = true;
            // 
            // tlpPacketListInfo
            // 
            this.tlpPacketListInfo.ColumnCount = 18;
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketListInfo.Controls.Add(this.lProxyLinks_CNT, 16, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyLinks, 15, 1);
            this.tlpPacketListInfo.Controls.Add(this.label15, 14, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyAccount_CNT, 13, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyAccount, 12, 1);
            this.tlpPacketListInfo.Controls.Add(this.label12, 11, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyQueue_CNT, 10, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyQueue, 9, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyUDP_CNT, 7, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTCP_CNT, 4, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTotal_CNT, 1, 1);
            this.tlpPacketListInfo.Controls.Add(this.label2, 8, 1);
            this.tlpPacketListInfo.Controls.Add(this.label3, 5, 1);
            this.tlpPacketListInfo.Controls.Add(this.label4, 2, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyUDP, 6, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTCP, 3, 1);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTotal, 0, 1);
            this.tlpPacketListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpPacketListInfo.Location = new System.Drawing.Point(0, 35);
            this.tlpPacketListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketListInfo.Name = "tlpPacketListInfo";
            this.tlpPacketListInfo.RowCount = 3;
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketListInfo.Size = new System.Drawing.Size(1124, 30);
            this.tlpPacketListInfo.TabIndex = 5;
            // 
            // lProxyLinks_CNT
            // 
            this.lProxyLinks_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyLinks_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyLinks_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyLinks_CNT.Location = new System.Drawing.Point(418, 3);
            this.lProxyLinks_CNT.Name = "lProxyLinks_CNT";
            this.lProxyLinks_CNT.Size = new System.Drawing.Size(9, 24);
            this.lProxyLinks_CNT.TabIndex = 22;
            this.lProxyLinks_CNT.Text = "0";
            // 
            // lProxyLinks
            // 
            this.lProxyLinks.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyLinks.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyLinks.Location = new System.Drawing.Point(368, 3);
            this.lProxyLinks.Name = "lProxyLinks";
            this.lProxyLinks.Size = new System.Drawing.Size(44, 24);
            this.lProxyLinks.TabIndex = 21;
            this.lProxyLinks.Text = "链接数:";
            // 
            // label15
            // 
            this.label15.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Silver;
            this.label15.Location = new System.Drawing.Point(357, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(5, 24);
            this.label15.TabIndex = 20;
            this.label15.Text = "|";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyAccount_CNT
            // 
            this.lProxyAccount_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyAccount_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyAccount_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyAccount_CNT.Location = new System.Drawing.Point(328, 3);
            this.lProxyAccount_CNT.Name = "lProxyAccount_CNT";
            this.lProxyAccount_CNT.Size = new System.Drawing.Size(23, 24);
            this.lProxyAccount_CNT.TabIndex = 19;
            this.lProxyAccount_CNT.Text = "0/0";
            // 
            // lProxyAccount
            // 
            this.lProxyAccount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyAccount.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyAccount.Location = new System.Drawing.Point(292, 3);
            this.lProxyAccount.Name = "lProxyAccount";
            this.lProxyAccount.Size = new System.Drawing.Size(30, 24);
            this.lProxyAccount.TabIndex = 18;
            this.lProxyAccount.Text = "在线:";
            // 
            // label12
            // 
            this.label12.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Silver;
            this.label12.Location = new System.Drawing.Point(281, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(5, 24);
            this.label12.TabIndex = 17;
            this.label12.Text = "|";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyQueue_CNT
            // 
            this.lProxyQueue_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyQueue_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyQueue_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyQueue_CNT.Location = new System.Drawing.Point(266, 3);
            this.lProxyQueue_CNT.Name = "lProxyQueue_CNT";
            this.lProxyQueue_CNT.Size = new System.Drawing.Size(9, 24);
            this.lProxyQueue_CNT.TabIndex = 16;
            this.lProxyQueue_CNT.Text = "0";
            // 
            // lProxyQueue
            // 
            this.lProxyQueue.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyQueue.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyQueue.Location = new System.Drawing.Point(216, 3);
            this.lProxyQueue.Name = "lProxyQueue";
            this.lProxyQueue.Size = new System.Drawing.Size(44, 24);
            this.lProxyQueue.TabIndex = 15;
            this.lProxyQueue.Text = "缓存区:";
            // 
            // lProxyUDP_CNT
            // 
            this.lProxyUDP_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyUDP_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyUDP_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyUDP_CNT.Location = new System.Drawing.Point(190, 3);
            this.lProxyUDP_CNT.Name = "lProxyUDP_CNT";
            this.lProxyUDP_CNT.Size = new System.Drawing.Size(9, 24);
            this.lProxyUDP_CNT.TabIndex = 14;
            this.lProxyUDP_CNT.Text = "0";
            // 
            // lProxyTCP_CNT
            // 
            this.lProxyTCP_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTCP_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTCP_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTCP_CNT.Location = new System.Drawing.Point(126, 3);
            this.lProxyTCP_CNT.Name = "lProxyTCP_CNT";
            this.lProxyTCP_CNT.Size = new System.Drawing.Size(9, 24);
            this.lProxyTCP_CNT.TabIndex = 13;
            this.lProxyTCP_CNT.Text = "0";
            // 
            // lProxyTotal_CNT
            // 
            this.lProxyTotal_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTotal_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTotal_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTotal_CNT.Location = new System.Drawing.Point(66, 3);
            this.lProxyTotal_CNT.Name = "lProxyTotal_CNT";
            this.lProxyTotal_CNT.Size = new System.Drawing.Size(9, 24);
            this.lProxyTotal_CNT.TabIndex = 12;
            this.lProxyTotal_CNT.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(205, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(5, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "|";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(141, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(5, 24);
            this.label3.TabIndex = 9;
            this.label3.Text = "|";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(81, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(5, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "|";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyUDP
            // 
            this.lProxyUDP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyUDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyUDP.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyUDP.Location = new System.Drawing.Point(152, 3);
            this.lProxyUDP.Name = "lProxyUDP";
            this.lProxyUDP.Size = new System.Drawing.Size(32, 24);
            this.lProxyUDP.TabIndex = 7;
            this.lProxyUDP.Text = "UDP:";
            // 
            // lProxyTCP
            // 
            this.lProxyTCP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTCP.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTCP.Location = new System.Drawing.Point(92, 3);
            this.lProxyTCP.Name = "lProxyTCP";
            this.lProxyTCP.Size = new System.Drawing.Size(28, 24);
            this.lProxyTCP.TabIndex = 6;
            this.lProxyTCP.Text = "TCP:";
            // 
            // lProxyTotal
            // 
            this.lProxyTotal.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTotal.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTotal.Location = new System.Drawing.Point(3, 3);
            this.lProxyTotal.Name = "lProxyTotal";
            this.lProxyTotal.Size = new System.Drawing.Size(57, 24);
            this.lProxyTotal.TabIndex = 5;
            this.lProxyTotal.Text = "代理总数:";
            // 
            // sProxyList
            // 
            this.sProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sProxyList.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sProxyList.Full = true;
            this.sProxyList.IconAlign = AntdUI.TAlignMini.Left;
            this.sProxyList.IconRatio = 1F;
            segmentedItem1.Badge = null;
            segmentedItem1.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem1.BadgeBack = null;
            segmentedItem1.BadgeMode = false;
            segmentedItem1.BadgeOffsetX = 0;
            segmentedItem1.BadgeOffsetY = 0;
            segmentedItem1.BadgeSize = 0.6F;
            segmentedItem1.BadgeSvg = null;
            segmentedItem1.IconSvg = "ShareAltOutlined";
            segmentedItem1.Text = "代理设置";
            segmentedItem2.Badge = null;
            segmentedItem2.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem2.BadgeBack = null;
            segmentedItem2.BadgeMode = false;
            segmentedItem2.BadgeOffsetX = 0;
            segmentedItem2.BadgeOffsetY = 0;
            segmentedItem2.BadgeSize = 0.6F;
            segmentedItem2.BadgeSvg = null;
            segmentedItem2.IconSvg = "UnorderedListOutlined";
            segmentedItem2.Text = "列表设置";
            segmentedItem3.Badge = null;
            segmentedItem3.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem3.BadgeBack = null;
            segmentedItem3.BadgeMode = false;
            segmentedItem3.BadgeOffsetX = 0;
            segmentedItem3.BadgeOffsetY = 0;
            segmentedItem3.BadgeSize = 0.6F;
            segmentedItem3.BadgeSvg = null;
            segmentedItem3.IconSvg = "BlockOutlined";
            segmentedItem3.Text = "映射设置";
            segmentedItem4.Badge = null;
            segmentedItem4.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem4.BadgeBack = null;
            segmentedItem4.BadgeMode = false;
            segmentedItem4.BadgeOffsetX = 0;
            segmentedItem4.BadgeOffsetY = 0;
            segmentedItem4.BadgeSize = 0.6F;
            segmentedItem4.BadgeSvg = null;
            segmentedItem4.IconSvg = "CloudUploadOutlined";
            segmentedItem4.Text = "外部代理";
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
            segmentedItem6.Badge = null;
            segmentedItem6.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem6.BadgeBack = null;
            segmentedItem6.BadgeMode = false;
            segmentedItem6.BadgeOffsetX = 0;
            segmentedItem6.BadgeOffsetY = 0;
            segmentedItem6.BadgeSize = 0.6F;
            segmentedItem6.BadgeSvg = null;
            segmentedItem6.IconSvg = "DeleteOutlined";
            segmentedItem6.Text = "清空数据";
            segmentedItem7.Badge = null;
            segmentedItem7.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem7.BadgeBack = null;
            segmentedItem7.BadgeMode = false;
            segmentedItem7.BadgeOffsetX = 0;
            segmentedItem7.BadgeOffsetY = 0;
            segmentedItem7.BadgeSize = 0.6F;
            segmentedItem7.BadgeSvg = null;
            segmentedItem7.IconSvg = "PlayCircleFilled";
            segmentedItem7.ID = "siStartHook";
            segmentedItem7.Text = "开始代理";
            this.sProxyList.Items.Add(segmentedItem1);
            this.sProxyList.Items.Add(segmentedItem2);
            this.sProxyList.Items.Add(segmentedItem3);
            this.sProxyList.Items.Add(segmentedItem4);
            this.sProxyList.Items.Add(segmentedItem5);
            this.sProxyList.Items.Add(segmentedItem6);
            this.sProxyList.Items.Add(segmentedItem7);
            this.sProxyList.Location = new System.Drawing.Point(0, 0);
            this.sProxyList.Margin = new System.Windows.Forms.Padding(0);
            this.sProxyList.Name = "sProxyList";
            this.sProxyList.Radius = 0;
            this.sProxyList.Size = new System.Drawing.Size(1124, 35);
            this.sProxyList.TabIndex = 3;
            this.sProxyList.SelectIndexChanged += new AntdUI.IntEventHandler(this.sProxyList_SelectIndexChanged);
            // 
            // tlpProxyInfo
            // 
            this.tlpProxyInfo.ColumnCount = 3;
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxyInfo.Controls.Add(this.lTotalBytes, 2, 0);
            this.tlpProxyInfo.Controls.Add(this.lSplit1, 1, 0);
            this.tlpProxyInfo.Controls.Add(this.lProxySpeed, 0, 0);
            this.tlpProxyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpProxyInfo.Location = new System.Drawing.Point(0, 694);
            this.tlpProxyInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyInfo.Name = "tlpProxyInfo";
            this.tlpProxyInfo.RowCount = 1;
            this.tlpProxyInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyInfo.Size = new System.Drawing.Size(1124, 30);
            this.tlpProxyInfo.TabIndex = 4;
            // 
            // lTotalBytes
            // 
            this.lTotalBytes.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotalBytes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotalBytes.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotalBytes.Location = new System.Drawing.Point(96, 3);
            this.lTotalBytes.Name = "lTotalBytes";
            this.lTotalBytes.Size = new System.Drawing.Size(67, 24);
            this.lTotalBytes.TabIndex = 11;
            this.lTotalBytes.Text = "TotalBytes";
            // 
            // lSplit1
            // 
            this.lSplit1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit1.ForeColor = System.Drawing.Color.Silver;
            this.lSplit1.Location = new System.Drawing.Point(85, 3);
            this.lSplit1.Name = "lSplit1";
            this.lSplit1.Size = new System.Drawing.Size(5, 24);
            this.lSplit1.TabIndex = 8;
            this.lSplit1.Text = "|";
            this.lSplit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxySpeed
            // 
            this.lProxySpeed.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxySpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxySpeed.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxySpeed.Location = new System.Drawing.Point(3, 3);
            this.lProxySpeed.Name = "lProxySpeed";
            this.lProxySpeed.Size = new System.Drawing.Size(76, 24);
            this.lProxySpeed.TabIndex = 5;
            this.lProxySpeed.Text = "ProxySpeed";
            // 
            // tpClientList
            // 
            this.tpClientList.Controls.Add(this.splitterClientList);
            this.tpClientList.Location = new System.Drawing.Point(3, 33);
            this.tpClientList.Name = "tpClientList";
            this.tpClientList.Size = new System.Drawing.Size(1124, 724);
            this.tpClientList.TabIndex = 8;
            this.tpClientList.Text = "客户端列表";
            // 
            // splitterClientList
            // 
            this.splitterClientList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterClientList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterClientList.Location = new System.Drawing.Point(0, 0);
            this.splitterClientList.Name = "splitterClientList";
            // 
            // splitterClientList.Panel1
            // 
            this.splitterClientList.Panel1.Controls.Add(this.treeClientList);
            this.splitterClientList.Panel1MinSize = 0;
            // 
            // splitterClientList.Panel2
            // 
            this.splitterClientList.Panel2.Controls.Add(this.tlpAuthInfo);
            this.splitterClientList.Panel2MinSize = 0;
            this.splitterClientList.Size = new System.Drawing.Size(1124, 724);
            this.splitterClientList.SplitterDistance = 374;
            this.splitterClientList.SplitterSize = 80;
            this.splitterClientList.SplitterWidth = 10;
            this.splitterClientList.TabIndex = 0;
            // 
            // treeClientList
            // 
            this.treeClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeClientList.Location = new System.Drawing.Point(0, 0);
            this.treeClientList.Name = "treeClientList";
            this.treeClientList.Size = new System.Drawing.Size(374, 724);
            this.treeClientList.TabIndex = 0;
            // 
            // tlpAuthInfo
            // 
            this.tlpAuthInfo.ColumnCount = 1;
            this.tlpAuthInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAuthInfo.Controls.Add(this.tAuthList, 0, 1);
            this.tlpAuthInfo.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tlpAuthInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAuthInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpAuthInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAuthInfo.Name = "tlpAuthInfo";
            this.tlpAuthInfo.RowCount = 2;
            this.tlpAuthInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAuthInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAuthInfo.Size = new System.Drawing.Size(740, 724);
            this.tlpAuthInfo.TabIndex = 0;
            // 
            // tAuthList
            // 
            this.tAuthList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAuthList.CellImpactHeight = false;
            this.tAuthList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAuthList.GapCell = 6;
            this.tAuthList.Location = new System.Drawing.Point(3, 33);
            this.tAuthList.Name = "tAuthList";
            this.tAuthList.Size = new System.Drawing.Size(734, 688);
            this.tAuthList.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lDevicesCount_Value, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.lLinksCount_Value, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.lAuthCount_Value, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label18, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.label19, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lDevicesCount, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.lLinksCount, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lAuthCount, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(740, 30);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lDevicesCount_Value
            // 
            this.lDevicesCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDevicesCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDevicesCount_Value.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDevicesCount_Value.Location = new System.Drawing.Point(244, 3);
            this.lDevicesCount_Value.Name = "lDevicesCount_Value";
            this.lDevicesCount_Value.Size = new System.Drawing.Size(9, 24);
            this.lDevicesCount_Value.TabIndex = 14;
            this.lDevicesCount_Value.Text = "0";
            // 
            // lLinksCount_Value
            // 
            this.lLinksCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lLinksCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lLinksCount_Value.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lLinksCount_Value.Location = new System.Drawing.Point(155, 3);
            this.lLinksCount_Value.Name = "lLinksCount_Value";
            this.lLinksCount_Value.Size = new System.Drawing.Size(9, 24);
            this.lLinksCount_Value.TabIndex = 13;
            this.lLinksCount_Value.Text = "0";
            // 
            // lAuthCount_Value
            // 
            this.lAuthCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAuthCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAuthCount_Value.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAuthCount_Value.Location = new System.Drawing.Point(66, 3);
            this.lAuthCount_Value.Name = "lAuthCount_Value";
            this.lAuthCount_Value.Size = new System.Drawing.Size(9, 24);
            this.lAuthCount_Value.TabIndex = 12;
            this.lAuthCount_Value.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.ForeColor = System.Drawing.Color.Silver;
            this.label18.Location = new System.Drawing.Point(170, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(5, 24);
            this.label18.TabIndex = 9;
            this.label18.Text = "|";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.ForeColor = System.Drawing.Color.Silver;
            this.label19.Location = new System.Drawing.Point(81, 3);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(5, 24);
            this.label19.TabIndex = 8;
            this.label19.Text = "|";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lDevicesCount
            // 
            this.lDevicesCount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDevicesCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDevicesCount.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDevicesCount.Location = new System.Drawing.Point(181, 3);
            this.lDevicesCount.Name = "lDevicesCount";
            this.lDevicesCount.Size = new System.Drawing.Size(57, 24);
            this.lDevicesCount.TabIndex = 7;
            this.lDevicesCount.Text = "设备总数:";
            // 
            // lLinksCount
            // 
            this.lLinksCount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lLinksCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lLinksCount.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lLinksCount.Location = new System.Drawing.Point(92, 3);
            this.lLinksCount.Name = "lLinksCount";
            this.lLinksCount.Size = new System.Drawing.Size(57, 24);
            this.lLinksCount.TabIndex = 6;
            this.lLinksCount.Text = "链接总数:";
            // 
            // lAuthCount
            // 
            this.lAuthCount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAuthCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAuthCount.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAuthCount.Location = new System.Drawing.Point(3, 3);
            this.lAuthCount.Name = "lAuthCount";
            this.lAuthCount.Size = new System.Drawing.Size(57, 24);
            this.lAuthCount.TabIndex = 5;
            this.lAuthCount.Text = "记录总数:";
            // 
            // tpAccountList
            // 
            this.tpAccountList.Controls.Add(this.tlpAccountList);
            this.tpAccountList.Location = new System.Drawing.Point(-1124, -724);
            this.tpAccountList.Name = "tpAccountList";
            this.tpAccountList.Size = new System.Drawing.Size(1124, 724);
            this.tpAccountList.TabIndex = 7;
            this.tpAccountList.Text = "账号列表";
            // 
            // tlpAccountList
            // 
            this.tlpAccountList.ColumnCount = 1;
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.Controls.Add(this.tAccountList, 0, 1);
            this.tlpAccountList.Controls.Add(this.tlpAccountListButton, 0, 0);
            this.tlpAccountList.Controls.Add(this.pAccountList, 0, 2);
            this.tlpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountList.Location = new System.Drawing.Point(0, 0);
            this.tlpAccountList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountList.Name = "tlpAccountList";
            this.tlpAccountList.RowCount = 3;
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountList.Size = new System.Drawing.Size(1124, 724);
            this.tlpAccountList.TabIndex = 1;
            // 
            // tAccountList
            // 
            this.tAccountList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAccountList.CellImpactHeight = false;
            this.tAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAccountList.GapCell = 6;
            this.tAccountList.Location = new System.Drawing.Point(3, 53);
            this.tAccountList.Name = "tAccountList";
            this.tAccountList.Size = new System.Drawing.Size(1118, 622);
            this.tAccountList.TabIndex = 1;
            this.tAccountList.CellClick += new AntdUI.Table.ClickEventHandler(this.tAccountList_CellClick);
            this.tAccountList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tAccountList_CellButtonClick);
            // 
            // tlpAccountListButton
            // 
            this.tlpAccountListButton.ColumnCount = 5;
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAccountListButton.Controls.Add(this.mAccountList, 4, 1);
            this.tlpAccountListButton.Controls.Add(this.txtSearchUserName, 3, 1);
            this.tlpAccountListButton.Controls.Add(this.dtpExpiryTime, 0, 1);
            this.tlpAccountListButton.Controls.Add(this.bSearchExpiryTime, 1, 1);
            this.tlpAccountListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpAccountListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountListButton.Name = "tlpAccountListButton";
            this.tlpAccountListButton.RowCount = 2;
            this.tlpAccountListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountListButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountListButton.Size = new System.Drawing.Size(1124, 50);
            this.tlpAccountListButton.TabIndex = 2;
            // 
            // mAccountList
            // 
            this.mAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mAccountList.Gap = 0;
            this.mAccountList.IconRatio = 1F;
            menuItem6.IconSvg = "PlusOutlined";
            menuItem7.IconSvg = "UserAddOutlined";
            menuItem7.ID = "miAdd";
            menuItem7.Text = "新增账号";
            menuItem8.IconSvg = "FolderOpenOutlined";
            menuItem8.ID = "miImport";
            menuItem8.Text = "导入账号列表";
            menuItem9.IconSvg = "DeliveredProcedureOutlined";
            menuItem9.ID = "miExport";
            menuItem9.Text = "导出所有账号";
            menuItem10.IconSvg = "DeleteOutlined";
            menuItem10.ID = "miClear";
            menuItem10.Text = "清空所有账号";
            menuItem6.Sub.Add(menuItem7);
            menuItem6.Sub.Add(menuItem8);
            menuItem6.Sub.Add(menuItem9);
            menuItem6.Sub.Add(menuItem10);
            this.mAccountList.Items.Add(menuItem6);
            this.mAccountList.Location = new System.Drawing.Point(1061, 2);
            this.mAccountList.Mode = AntdUI.TMenuMode.Horizontal;
            this.mAccountList.Name = "mAccountList";
            this.mAccountList.Size = new System.Drawing.Size(60, 45);
            this.mAccountList.TabIndex = 3;
            this.mAccountList.Trigger = AntdUI.Trigger.Click;
            this.mAccountList.SelectChanged += new AntdUI.SelectEventHandler(this.mAccountList_SelectChanged);
            // 
            // txtSearchUserName
            // 
            this.txtSearchUserName.AllowClear = true;
            this.txtSearchUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchUserName.Location = new System.Drawing.Point(811, 2);
            this.txtSearchUserName.Name = "txtSearchUserName";
            this.txtSearchUserName.PlaceholderText = "请输入用户名查询";
            this.txtSearchUserName.PrefixSvg = "SearchOutlined";
            this.txtSearchUserName.Size = new System.Drawing.Size(244, 45);
            this.txtSearchUserName.TabIndex = 4;
            this.txtSearchUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearchUserName_KeyPress);
            // 
            // dtpExpiryTime
            // 
            this.dtpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpExpiryTime.Format = "yyyy-MM-dd HH:mm:ss";
            this.dtpExpiryTime.LocalizationPlaceholderEnd = "DatePicker.PlaceholderE";
            this.dtpExpiryTime.LocalizationPlaceholderStart = "DatePicker.PlaceholderS";
            this.dtpExpiryTime.Location = new System.Drawing.Point(3, 2);
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            this.dtpExpiryTime.PlaceholderEnd = "过期结束时间";
            this.dtpExpiryTime.PlaceholderStart = "过期开始时间";
            this.dtpExpiryTime.Size = new System.Drawing.Size(494, 45);
            this.dtpExpiryTime.TabIndex = 5;
            this.dtpExpiryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bSearchExpiryTime
            // 
            this.bSearchExpiryTime.BorderWidth = 1F;
            this.bSearchExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearchExpiryTime.Location = new System.Drawing.Point(503, 2);
            this.bSearchExpiryTime.Name = "bSearchExpiryTime";
            this.bSearchExpiryTime.Size = new System.Drawing.Size(94, 45);
            this.bSearchExpiryTime.TabIndex = 7;
            this.bSearchExpiryTime.Text = "查询";
            this.bSearchExpiryTime.Click += new System.EventHandler(this.bSearchExpiryTime_Click);
            // 
            // pAccountList
            // 
            this.pAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAccountList.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pAccountList.Location = new System.Drawing.Point(3, 681);
            this.pAccountList.Name = "pAccountList";
            this.pAccountList.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pAccountList.ShowSizeChanger = true;
            this.pAccountList.Size = new System.Drawing.Size(1118, 40);
            this.pAccountList.TabIndex = 3;
            this.pAccountList.ValueChanged += new AntdUI.PageValueEventHandler(this.pAccountList_ValueChanged);
            this.pAccountList.ShowTotalChanged += new AntdUI.PageValueRtEventHandler(this.pAccountList_ShowTotalChanged);
            // 
            // tpStatistical
            // 
            this.tpStatistical.Location = new System.Drawing.Point(-1124, -724);
            this.tpStatistical.Name = "tpStatistical";
            this.tpStatistical.Size = new System.Drawing.Size(1124, 724);
            this.tpStatistical.TabIndex = 1;
            this.tpStatistical.Text = "统计数据";
            // 
            // tpSystemLog
            // 
            this.tpSystemLog.Controls.Add(this.tSystemLog);
            this.tpSystemLog.Location = new System.Drawing.Point(-1124, -724);
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.Size = new System.Drawing.Size(1124, 724);
            this.tpSystemLog.TabIndex = 6;
            this.tpSystemLog.Text = "系统日志";
            // 
            // tSystemLog
            // 
            this.tSystemLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSystemLog.CellImpactHeight = false;
            this.tSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSystemLog.EmptyHeader = true;
            this.tSystemLog.Gap = 8;
            this.tSystemLog.GapCell = 0;
            this.tSystemLog.Location = new System.Drawing.Point(0, 0);
            this.tSystemLog.MultipleRows = true;
            this.tSystemLog.Name = "tSystemLog";
            this.tSystemLog.Size = new System.Drawing.Size(1124, 724);
            this.tSystemLog.TabIndex = 2;
            this.tSystemLog.Text = "table1";
            this.tSystemLog.CellClick += new AntdUI.Table.ClickEventHandler(this.tSystemLog_CellClick);
            // 
            // timerProxyList
            // 
            this.timerProxyList.Enabled = true;
            this.timerProxyList.Interval = 10;
            this.timerProxyList.Tick += new System.EventHandler(this.timerProxyList_Tick);
            // 
            // bgwProxyList
            // 
            this.bgwProxyList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwProxyList_DoWork);
            this.bgwProxyList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwProxyList_RunWorkerCompleted);
            // 
            // timerProxyListInfo
            // 
            this.timerProxyListInfo.Enabled = true;
            this.timerProxyListInfo.Interval = 1000;
            this.timerProxyListInfo.Tick += new System.EventHandler(this.timerProxyListInfo_Tick);
            // 
            // bgwClientList
            // 
            this.bgwClientList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwClientList_DoWork);
            this.bgwClientList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwClientList_RunWorkerCompleted);
            // 
            // ProxyModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1300, 800);
            this.Controls.Add(this.tabProxyMode);
            this.Controls.Add(this.tlpMenu);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
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
            this.tabProxyMode.ResumeLayout(false);
            this.tpProxyList.ResumeLayout(false);
            this.tlpProxyList.ResumeLayout(false);
            this.splitterProxyList.Panel1.ResumeLayout(false);
            this.splitterProxyList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterProxyList)).EndInit();
            this.splitterProxyList.ResumeLayout(false);
            this.pPacketData.ResumeLayout(false);
            this.tlpPacketListInfo.ResumeLayout(false);
            this.tlpPacketListInfo.PerformLayout();
            this.tlpProxyInfo.ResumeLayout(false);
            this.tlpProxyInfo.PerformLayout();
            this.tpClientList.ResumeLayout(false);
            this.splitterClientList.Panel1.ResumeLayout(false);
            this.splitterClientList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).EndInit();
            this.splitterClientList.ResumeLayout(false);
            this.tlpAuthInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tpAccountList.ResumeLayout(false);
            this.tlpAccountList.ResumeLayout(false);
            this.tlpAccountListButton.ResumeLayout(false);
            this.tpSystemLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Button btn_setting;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private AntdUI.Menu mProxyMode;
        private AntdUI.Button bMenuCollapse;
        private AntdUI.Tabs tabProxyMode;
        private AntdUI.TabPage tpProxyList;
        private System.Windows.Forms.TableLayoutPanel tlpProxyList;
        private System.Windows.Forms.TableLayoutPanel tlpPacketListInfo;
        private AntdUI.Label lProxyLinks_CNT;
        private AntdUI.Label lProxyLinks;
        private AntdUI.Label label15;
        private AntdUI.Label lProxyAccount_CNT;
        private AntdUI.Label lProxyAccount;
        private AntdUI.Label label12;
        private AntdUI.Label lProxyQueue_CNT;
        private AntdUI.Label lProxyQueue;
        private AntdUI.Label lProxyUDP_CNT;
        private AntdUI.Label lProxyTCP_CNT;
        private AntdUI.Label lProxyTotal_CNT;
        private AntdUI.Label label2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label lProxyUDP;
        private AntdUI.Label lProxyTCP;
        private AntdUI.Label lProxyTotal;
        private AntdUI.Segmented sProxyList;
        private System.Windows.Forms.TableLayoutPanel tlpProxyInfo;
        private AntdUI.Label lTotalBytes;
        private AntdUI.Label lSplit1;
        private AntdUI.Label lProxySpeed;
        private AntdUI.TabPage tpAccountList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountList;
        private AntdUI.Table tAccountList;
        private AntdUI.TabPage tpStatistical;
        private AntdUI.TabPage tpSystemLog;
        private AntdUI.Table tSystemLog;
        private AntdUI.TabPage tpClientList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountListButton;
        private AntdUI.Menu mAccountList;
        private AntdUI.Pagination pAccountList;
        private AntdUI.Input txtSearchUserName;
        private AntdUI.DatePickerRange dtpExpiryTime;
        private AntdUI.Button bSearchExpiryTime;
        private System.Windows.Forms.Timer timerProxyList;
        private System.ComponentModel.BackgroundWorker bgwProxyList;
        private System.Windows.Forms.Timer timerProxyListInfo;
        private AntdUI.Splitter splitterProxyList;
        private AntdUI.Table tProxyList;
        private AntdUI.Panel pPacketData;
        private Be.Windows.Forms.HexBox hbProxyData;
        private AntdUI.Splitter splitterClientList;
        private AntdUI.Tree treeClientList;
        private System.Windows.Forms.TableLayoutPanel tlpAuthInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Label lDevicesCount_Value;
        private AntdUI.Label lLinksCount_Value;
        private AntdUI.Label lAuthCount_Value;
        private AntdUI.Label label18;
        private AntdUI.Label label19;
        private AntdUI.Label lDevicesCount;
        private AntdUI.Label lLinksCount;
        private AntdUI.Label lAuthCount;
        private AntdUI.Table tAuthList;
        private System.ComponentModel.BackgroundWorker bgwClientList;
    }
}


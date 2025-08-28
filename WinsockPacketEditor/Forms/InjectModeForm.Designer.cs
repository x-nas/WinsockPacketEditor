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
            AntdUI.MenuItem menuItem11 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem12 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem13 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem14 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem15 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem16 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem17 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem18 = new AntdUI.MenuItem();
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
            this.tlpPacketList = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.bPacketList_Clear = new AntdUI.Button();
            this.bHookStop = new AntdUI.Button();
            this.mPacketList = new AntdUI.Menu();
            this.bHookStart = new AntdUI.Button();
            this.splitterPacketList = new AntdUI.Splitter();
            this.tlpPacketList2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lWSARecvFrom_CNT = new AntdUI.Label();
            this.lWSARecvFrom = new AntdUI.Label();
            this.label33 = new AntdUI.Label();
            this.lWSASendTo_CNT = new AntdUI.Label();
            this.lWSASendTo = new AntdUI.Label();
            this.label30 = new AntdUI.Label();
            this.lWSARecv_CNT = new AntdUI.Label();
            this.lWSARecv = new AntdUI.Label();
            this.label27 = new AntdUI.Label();
            this.lWSASend_CNT = new AntdUI.Label();
            this.lWSASend = new AntdUI.Label();
            this.label24 = new AntdUI.Label();
            this.lRecvFrom_CNT = new AntdUI.Label();
            this.lRecvFrom = new AntdUI.Label();
            this.label21 = new AntdUI.Label();
            this.lSendTo_CNT = new AntdUI.Label();
            this.lSendTo = new AntdUI.Label();
            this.label18 = new AntdUI.Label();
            this.lRecv_CNT = new AntdUI.Label();
            this.lRecv = new AntdUI.Label();
            this.label15 = new AntdUI.Label();
            this.lSend_CNT = new AntdUI.Label();
            this.lSend = new AntdUI.Label();
            this.label12 = new AntdUI.Label();
            this.lFilterPacket_CNT = new AntdUI.Label();
            this.lFilterPacket = new AntdUI.Label();
            this.lQueue_CNT = new AntdUI.Label();
            this.lFilterExecute_CNT = new AntdUI.Label();
            this.lTotal_CNT = new AntdUI.Label();
            this.label2 = new AntdUI.Label();
            this.label3 = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.lQueue = new AntdUI.Label();
            this.lFilterExecute = new AntdUI.Label();
            this.lTotal = new AntdUI.Label();
            this.tPacketList = new AntdUI.Table();
            this.pPacketData = new AntdUI.Panel();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.tlpProcessInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lSpeedInfo = new AntdUI.Label();
            this.lSplit3 = new AntdUI.Label();
            this.lSplit2 = new AntdUI.Label();
            this.lSplit1 = new AntdUI.Label();
            this.lWinsockInfo = new AntdUI.Label();
            this.lModuleName = new AntdUI.Label();
            this.lProcessName = new AntdUI.Label();
            this.tpFilterList = new AntdUI.TabPage();
            this.tpSendList = new AntdUI.TabPage();
            this.tpRobotList = new AntdUI.TabPage();
            this.tpStatistical = new AntdUI.TabPage();
            this.tpComparison = new AntdUI.TabPage();
            this.tpXOR = new AntdUI.TabPage();
            this.tpTranscoding = new AntdUI.TabPage();
            this.tpExtraction = new AntdUI.TabPage();
            this.tpSystemLog = new AntdUI.TabPage();
            this.timerPacketList = new System.Windows.Forms.Timer(this.components);
            this.timerPacketListInfo = new System.Windows.Forms.Timer(this.components);
            this.bgwSearchPacketList = new System.ComponentModel.BackgroundWorker();
            this.bgwPacketList = new System.ComponentModel.BackgroundWorker();
            this.pageHeader.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.tabInjectMode.SuspendLayout();
            this.tpPacketList.SuspendLayout();
            this.tlpPacketList.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterPacketList)).BeginInit();
            this.splitterPacketList.Panel1.SuspendLayout();
            this.splitterPacketList.Panel2.SuspendLayout();
            this.splitterPacketList.SuspendLayout();
            this.tlpPacketList2.SuspendLayout();
            this.tlpPacketListInfo.SuspendLayout();
            this.pPacketData.SuspendLayout();
            this.tlpProcessInfo.SuspendLayout();
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
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1450, 40);
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
            this.tlpMenu.Location = new System.Drawing.Point(0, 40);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 2;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Size = new System.Drawing.Size(170, 762);
            this.tlpMenu.TabIndex = 7;
            // 
            // mInjectMode
            // 
            this.mInjectMode.Dock = System.Windows.Forms.DockStyle.Left;
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
            this.mInjectMode.Location = new System.Drawing.Point(3, 47);
            this.mInjectMode.Name = "mInjectMode";
            this.mInjectMode.Size = new System.Drawing.Size(164, 712);
            this.mInjectMode.TabIndex = 5;
            this.mInjectMode.SelectChanged += new AntdUI.SelectEventHandler(this.mInjectMode_SelectChanged);
            // 
            // bMenuCollapse
            // 
            this.bMenuCollapse.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bMenuCollapse.Dock = System.Windows.Forms.DockStyle.Left;
            this.bMenuCollapse.Ghost = true;
            this.bMenuCollapse.IconRatio = 1F;
            this.bMenuCollapse.IconSvg = "MenuFoldOutlined";
            this.bMenuCollapse.Location = new System.Drawing.Point(1, 1);
            this.bMenuCollapse.Margin = new System.Windows.Forms.Padding(1);
            this.bMenuCollapse.Name = "bMenuCollapse";
            this.bMenuCollapse.Size = new System.Drawing.Size(42, 42);
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
            this.tabInjectMode.Location = new System.Drawing.Point(170, 40);
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
            this.tabInjectMode.Size = new System.Drawing.Size(1280, 762);
            this.tabInjectMode.Style = styleLine1;
            this.tabInjectMode.TabIndex = 10;
            this.tabInjectMode.Text = "tabs1";
            // 
            // tpPacketList
            // 
            this.tpPacketList.Controls.Add(this.tlpPacketList);
            this.tpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpPacketList.Location = new System.Drawing.Point(3, 33);
            this.tpPacketList.Name = "tpPacketList";
            this.tpPacketList.Size = new System.Drawing.Size(1274, 726);
            this.tpPacketList.TabIndex = 0;
            this.tpPacketList.Text = "封包列表";
            // 
            // tlpPacketList
            // 
            this.tlpPacketList.ColumnCount = 1;
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tlpPacketList.Controls.Add(this.splitterPacketList, 0, 1);
            this.tlpPacketList.Controls.Add(this.tlpProcessInfo, 0, 2);
            this.tlpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList.Name = "tlpPacketList";
            this.tlpPacketList.RowCount = 3;
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketList.Size = new System.Drawing.Size(1274, 726);
            this.tlpPacketList.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.bPacketList_Clear, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.bHookStop, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.mPacketList, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.bHookStart, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1274, 50);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // bPacketList_Clear
            // 
            this.bPacketList_Clear.BorderWidth = 1F;
            this.bPacketList_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bPacketList_Clear.IconSvg = "DeleteOutlined";
            this.bPacketList_Clear.LocalizationText = "Clear";
            this.bPacketList_Clear.Location = new System.Drawing.Point(203, 3);
            this.bPacketList_Clear.Name = "bPacketList_Clear";
            this.bPacketList_Clear.Size = new System.Drawing.Size(94, 44);
            this.bPacketList_Clear.TabIndex = 9;
            this.bPacketList_Clear.Text = "清空";
            this.bPacketList_Clear.Type = AntdUI.TTypeMini.Warn;
            this.bPacketList_Clear.Click += new System.EventHandler(this.bPacketList_Clear_Click);
            // 
            // bHookStop
            // 
            this.bHookStop.BorderWidth = 1F;
            this.bHookStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bHookStop.Enabled = false;
            this.bHookStop.IconSvg = "PauseCircleOutlined";
            this.bHookStop.LocalizationText = "Stop";
            this.bHookStop.Location = new System.Drawing.Point(103, 3);
            this.bHookStop.Name = "bHookStop";
            this.bHookStop.Size = new System.Drawing.Size(94, 44);
            this.bHookStop.TabIndex = 8;
            this.bHookStop.Text = "停止";
            this.bHookStop.Type = AntdUI.TTypeMini.Error;
            this.bHookStop.Click += new System.EventHandler(this.bHookStop_Click);
            // 
            // mPacketList
            // 
            this.mPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPacketList.Gap = 5;
            this.mPacketList.IconRatio = 1F;
            menuItem11.IconSvg = "SearchOutlined";
            menuItem11.ID = "miPacketListSearch";
            menuItem12.IconSvg = "PlusOutlined";
            menuItem13.IconSvg = "FilterOutlined";
            menuItem13.ID = "miFilterSettings";
            menuItem13.LocalizationText = "InjectModeForm.{id}";
            menuItem13.Text = "过滤设置";
            menuItem14.IconSvg = "AimOutlined";
            menuItem14.ID = "miHookSettings";
            menuItem14.LocalizationText = "InjectModeForm.{id}";
            menuItem14.Text = "拦截设置";
            menuItem15.IconSvg = "OrderedListOutlined";
            menuItem15.ID = "miListSettings";
            menuItem15.LocalizationText = "InjectModeForm.{id}";
            menuItem15.Text = "列表设置";
            menuItem16.IconSvg = "GoldOutlined";
            menuItem16.ID = "miHotKeySettings";
            menuItem16.LocalizationText = "InjectModeForm.{id}";
            menuItem16.Text = "快捷键设置";
            menuItem17.IconSvg = "DeliveredProcedureOutlined";
            menuItem17.ID = "miBackUpSettings";
            menuItem17.LocalizationText = "InjectModeForm.{id}";
            menuItem17.Text = "备份设置";
            menuItem18.IconSvg = "SettingOutlined";
            menuItem18.ID = "miSystemSettings";
            menuItem18.LocalizationText = "InjectModeForm.{id}";
            menuItem18.Text = "系统设置";
            menuItem12.Sub.Add(menuItem13);
            menuItem12.Sub.Add(menuItem14);
            menuItem12.Sub.Add(menuItem15);
            menuItem12.Sub.Add(menuItem16);
            menuItem12.Sub.Add(menuItem17);
            menuItem12.Sub.Add(menuItem18);
            this.mPacketList.Items.Add(menuItem11);
            this.mPacketList.Items.Add(menuItem12);
            this.mPacketList.Location = new System.Drawing.Point(1171, 3);
            this.mPacketList.Mode = AntdUI.TMenuMode.Horizontal;
            this.mPacketList.Name = "mPacketList";
            this.mPacketList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mPacketList.Size = new System.Drawing.Size(100, 44);
            this.mPacketList.TabIndex = 6;
            this.mPacketList.Trigger = AntdUI.Trigger.Click;
            this.mPacketList.SelectChanged += new AntdUI.SelectEventHandler(this.mPacketList_SelectChanged);
            // 
            // bHookStart
            // 
            this.bHookStart.BorderWidth = 1F;
            this.bHookStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bHookStart.IconSvg = "PlayCircleOutlined";
            this.bHookStart.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bHookStart.LoadingWaveCount = 6;
            this.bHookStart.LoadingWaveSize = 6;
            this.bHookStart.LoadingWaveValue = 0.6F;
            this.bHookStart.LoadingWaveVertical = true;
            this.bHookStart.LocalizationText = "Start";
            this.bHookStart.Location = new System.Drawing.Point(3, 3);
            this.bHookStart.Name = "bHookStart";
            this.bHookStart.Size = new System.Drawing.Size(94, 44);
            this.bHookStart.TabIndex = 7;
            this.bHookStart.Text = "开始";
            this.bHookStart.Type = AntdUI.TTypeMini.Info;
            this.bHookStart.Click += new System.EventHandler(this.bHookStart_Click);
            // 
            // splitterPacketList
            // 
            this.splitterPacketList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterPacketList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterPacketList.Location = new System.Drawing.Point(3, 53);
            this.splitterPacketList.Name = "splitterPacketList";
            this.splitterPacketList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterPacketList.Panel1
            // 
            this.splitterPacketList.Panel1.Controls.Add(this.tlpPacketList2);
            this.splitterPacketList.Panel1MinSize = 0;
            // 
            // splitterPacketList.Panel2
            // 
            this.splitterPacketList.Panel2.Controls.Add(this.pPacketData);
            this.splitterPacketList.Panel2MinSize = 0;
            this.splitterPacketList.Size = new System.Drawing.Size(1268, 640);
            this.splitterPacketList.SplitterDistance = 450;
            this.splitterPacketList.SplitterSize = 80;
            this.splitterPacketList.SplitterWidth = 10;
            this.splitterPacketList.TabIndex = 2;
            // 
            // tlpPacketList2
            // 
            this.tlpPacketList2.ColumnCount = 1;
            this.tlpPacketList2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList2.Controls.Add(this.tlpPacketListInfo, 0, 0);
            this.tlpPacketList2.Controls.Add(this.tPacketList, 0, 1);
            this.tlpPacketList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList2.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketList2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList2.Name = "tlpPacketList2";
            this.tlpPacketList2.RowCount = 2;
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList2.Size = new System.Drawing.Size(1268, 450);
            this.tlpPacketList2.TabIndex = 0;
            // 
            // tlpPacketListInfo
            // 
            this.tlpPacketListInfo.ColumnCount = 36;
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
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketListInfo.Controls.Add(this.lWSARecvFrom_CNT, 34, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSARecvFrom, 33, 1);
            this.tlpPacketListInfo.Controls.Add(this.label33, 32, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSASendTo_CNT, 31, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSASendTo, 30, 1);
            this.tlpPacketListInfo.Controls.Add(this.label30, 29, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSARecv_CNT, 28, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSARecv, 27, 1);
            this.tlpPacketListInfo.Controls.Add(this.label27, 26, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSASend_CNT, 25, 1);
            this.tlpPacketListInfo.Controls.Add(this.lWSASend, 24, 1);
            this.tlpPacketListInfo.Controls.Add(this.label24, 23, 1);
            this.tlpPacketListInfo.Controls.Add(this.lRecvFrom_CNT, 22, 1);
            this.tlpPacketListInfo.Controls.Add(this.lRecvFrom, 21, 1);
            this.tlpPacketListInfo.Controls.Add(this.label21, 20, 1);
            this.tlpPacketListInfo.Controls.Add(this.lSendTo_CNT, 19, 1);
            this.tlpPacketListInfo.Controls.Add(this.lSendTo, 18, 1);
            this.tlpPacketListInfo.Controls.Add(this.label18, 17, 1);
            this.tlpPacketListInfo.Controls.Add(this.lRecv_CNT, 16, 1);
            this.tlpPacketListInfo.Controls.Add(this.lRecv, 15, 1);
            this.tlpPacketListInfo.Controls.Add(this.label15, 14, 1);
            this.tlpPacketListInfo.Controls.Add(this.lSend_CNT, 13, 1);
            this.tlpPacketListInfo.Controls.Add(this.lSend, 12, 1);
            this.tlpPacketListInfo.Controls.Add(this.label12, 11, 1);
            this.tlpPacketListInfo.Controls.Add(this.lFilterPacket_CNT, 10, 1);
            this.tlpPacketListInfo.Controls.Add(this.lFilterPacket, 9, 1);
            this.tlpPacketListInfo.Controls.Add(this.lQueue_CNT, 7, 1);
            this.tlpPacketListInfo.Controls.Add(this.lFilterExecute_CNT, 4, 1);
            this.tlpPacketListInfo.Controls.Add(this.lTotal_CNT, 1, 1);
            this.tlpPacketListInfo.Controls.Add(this.label2, 8, 1);
            this.tlpPacketListInfo.Controls.Add(this.label3, 5, 1);
            this.tlpPacketListInfo.Controls.Add(this.label4, 2, 1);
            this.tlpPacketListInfo.Controls.Add(this.lQueue, 6, 1);
            this.tlpPacketListInfo.Controls.Add(this.lFilterExecute, 3, 1);
            this.tlpPacketListInfo.Controls.Add(this.lTotal, 0, 1);
            this.tlpPacketListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpPacketListInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketListInfo.Name = "tlpPacketListInfo";
            this.tlpPacketListInfo.RowCount = 3;
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketListInfo.Size = new System.Drawing.Size(1268, 30);
            this.tlpPacketListInfo.TabIndex = 6;
            // 
            // lWSARecvFrom_CNT
            // 
            this.lWSARecvFrom_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecvFrom_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecvFrom_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecvFrom_CNT.Location = new System.Drawing.Point(967, 3);
            this.lWSARecvFrom_CNT.Name = "lWSARecvFrom_CNT";
            this.lWSARecvFrom_CNT.Size = new System.Drawing.Size(9, 24);
            this.lWSARecvFrom_CNT.TabIndex = 40;
            this.lWSARecvFrom_CNT.Text = "0";
            // 
            // lWSARecvFrom
            // 
            this.lWSARecvFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecvFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecvFrom.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecvFrom.LocalizationText = "InjectModeForm.WSARecvFrom";
            this.lWSARecvFrom.Location = new System.Drawing.Point(887, 3);
            this.lWSARecvFrom.Name = "lWSARecvFrom";
            this.lWSARecvFrom.Size = new System.Drawing.Size(74, 24);
            this.lWSARecvFrom.TabIndex = 39;
            this.lWSARecvFrom.Text = "WSA接收自:";
            // 
            // label33
            // 
            this.label33.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label33.ForeColor = System.Drawing.Color.Silver;
            this.label33.Location = new System.Drawing.Point(876, 3);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(5, 24);
            this.label33.TabIndex = 38;
            this.label33.Text = "|";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWSASendTo_CNT
            // 
            this.lWSASendTo_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASendTo_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASendTo_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASendTo_CNT.Location = new System.Drawing.Point(861, 3);
            this.lWSASendTo_CNT.Name = "lWSASendTo_CNT";
            this.lWSASendTo_CNT.Size = new System.Drawing.Size(9, 24);
            this.lWSASendTo_CNT.TabIndex = 37;
            this.lWSASendTo_CNT.Text = "0";
            // 
            // lWSASendTo
            // 
            this.lWSASendTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASendTo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASendTo.LocalizationText = "InjectModeForm.WSASendTo";
            this.lWSASendTo.Location = new System.Drawing.Point(781, 3);
            this.lWSASendTo.Name = "lWSASendTo";
            this.lWSASendTo.Size = new System.Drawing.Size(74, 24);
            this.lWSASendTo.TabIndex = 36;
            this.lWSASendTo.Text = "WSA发送到:";
            // 
            // label30
            // 
            this.label30.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label30.ForeColor = System.Drawing.Color.Silver;
            this.label30.Location = new System.Drawing.Point(770, 3);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(5, 24);
            this.label30.TabIndex = 35;
            this.label30.Text = "|";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWSARecv_CNT
            // 
            this.lWSARecv_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecv_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecv_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecv_CNT.Location = new System.Drawing.Point(755, 3);
            this.lWSARecv_CNT.Name = "lWSARecv_CNT";
            this.lWSARecv_CNT.Size = new System.Drawing.Size(9, 24);
            this.lWSARecv_CNT.TabIndex = 34;
            this.lWSARecv_CNT.Text = "0";
            // 
            // lWSARecv
            // 
            this.lWSARecv.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecv.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecv.LocalizationText = "InjectModeForm.WSARecv";
            this.lWSARecv.Location = new System.Drawing.Point(688, 3);
            this.lWSARecv.Name = "lWSARecv";
            this.lWSARecv.Size = new System.Drawing.Size(61, 24);
            this.lWSARecv.TabIndex = 33;
            this.lWSARecv.Text = "WSA接收:";
            // 
            // label27
            // 
            this.label27.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.ForeColor = System.Drawing.Color.Silver;
            this.label27.Location = new System.Drawing.Point(677, 3);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(5, 24);
            this.label27.TabIndex = 32;
            this.label27.Text = "|";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWSASend_CNT
            // 
            this.lWSASend_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASend_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASend_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASend_CNT.Location = new System.Drawing.Point(662, 3);
            this.lWSASend_CNT.Name = "lWSASend_CNT";
            this.lWSASend_CNT.Size = new System.Drawing.Size(9, 24);
            this.lWSASend_CNT.TabIndex = 31;
            this.lWSASend_CNT.Text = "0";
            // 
            // lWSASend
            // 
            this.lWSASend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASend.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASend.LocalizationText = "InjectModeForm.WSASend";
            this.lWSASend.Location = new System.Drawing.Point(595, 3);
            this.lWSASend.Name = "lWSASend";
            this.lWSASend.Size = new System.Drawing.Size(61, 24);
            this.lWSASend.TabIndex = 30;
            this.lWSASend.Text = "WSA发送:";
            // 
            // label24
            // 
            this.label24.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.ForeColor = System.Drawing.Color.Silver;
            this.label24.Location = new System.Drawing.Point(584, 3);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(5, 24);
            this.label24.TabIndex = 29;
            this.label24.Text = "|";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRecvFrom_CNT
            // 
            this.lRecvFrom_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecvFrom_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecvFrom_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecvFrom_CNT.Location = new System.Drawing.Point(569, 3);
            this.lRecvFrom_CNT.Name = "lRecvFrom_CNT";
            this.lRecvFrom_CNT.Size = new System.Drawing.Size(9, 24);
            this.lRecvFrom_CNT.TabIndex = 28;
            this.lRecvFrom_CNT.Text = "0";
            // 
            // lRecvFrom
            // 
            this.lRecvFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecvFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecvFrom.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecvFrom.LocalizationText = "InjectModeForm.RecvFrom";
            this.lRecvFrom.Location = new System.Drawing.Point(519, 3);
            this.lRecvFrom.Name = "lRecvFrom";
            this.lRecvFrom.Size = new System.Drawing.Size(44, 24);
            this.lRecvFrom.TabIndex = 27;
            this.lRecvFrom.Text = "接收自:";
            // 
            // label21
            // 
            this.label21.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.ForeColor = System.Drawing.Color.Silver;
            this.label21.Location = new System.Drawing.Point(508, 3);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(5, 24);
            this.label21.TabIndex = 26;
            this.label21.Text = "|";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSendTo_CNT
            // 
            this.lSendTo_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSendTo_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSendTo_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSendTo_CNT.Location = new System.Drawing.Point(493, 3);
            this.lSendTo_CNT.Name = "lSendTo_CNT";
            this.lSendTo_CNT.Size = new System.Drawing.Size(9, 24);
            this.lSendTo_CNT.TabIndex = 25;
            this.lSendTo_CNT.Text = "0";
            // 
            // lSendTo
            // 
            this.lSendTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSendTo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSendTo.LocalizationText = "InjectModeForm.SendTo";
            this.lSendTo.Location = new System.Drawing.Point(443, 3);
            this.lSendTo.Name = "lSendTo";
            this.lSendTo.Size = new System.Drawing.Size(44, 24);
            this.lSendTo.TabIndex = 24;
            this.lSendTo.Text = "发送到:";
            // 
            // label18
            // 
            this.label18.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.ForeColor = System.Drawing.Color.Silver;
            this.label18.Location = new System.Drawing.Point(432, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(5, 24);
            this.label18.TabIndex = 23;
            this.label18.Text = "|";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRecv_CNT
            // 
            this.lRecv_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecv_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecv_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecv_CNT.Location = new System.Drawing.Point(417, 3);
            this.lRecv_CNT.Name = "lRecv_CNT";
            this.lRecv_CNT.Size = new System.Drawing.Size(9, 24);
            this.lRecv_CNT.TabIndex = 22;
            this.lRecv_CNT.Text = "0";
            // 
            // lRecv
            // 
            this.lRecv.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecv.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecv.LocalizationText = "InjectModeForm.Recv";
            this.lRecv.Location = new System.Drawing.Point(381, 3);
            this.lRecv.Name = "lRecv";
            this.lRecv.Size = new System.Drawing.Size(30, 24);
            this.lRecv.TabIndex = 21;
            this.lRecv.Text = "接收:";
            // 
            // label15
            // 
            this.label15.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Silver;
            this.label15.Location = new System.Drawing.Point(370, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(5, 24);
            this.label15.TabIndex = 20;
            this.label15.Text = "|";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSend_CNT
            // 
            this.lSend_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_CNT.Location = new System.Drawing.Point(355, 3);
            this.lSend_CNT.Name = "lSend_CNT";
            this.lSend_CNT.Size = new System.Drawing.Size(9, 24);
            this.lSend_CNT.TabIndex = 19;
            this.lSend_CNT.Text = "0";
            // 
            // lSend
            // 
            this.lSend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend.LocalizationText = "InjectModeForm.Send";
            this.lSend.Location = new System.Drawing.Point(319, 3);
            this.lSend.Name = "lSend";
            this.lSend.Size = new System.Drawing.Size(30, 24);
            this.lSend.TabIndex = 18;
            this.lSend.Text = "发送:";
            // 
            // label12
            // 
            this.label12.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Silver;
            this.label12.Location = new System.Drawing.Point(308, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(5, 24);
            this.label12.TabIndex = 17;
            this.label12.Text = "|";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFilterPacket_CNT
            // 
            this.lFilterPacket_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterPacket_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterPacket_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterPacket_CNT.Location = new System.Drawing.Point(293, 3);
            this.lFilterPacket_CNT.Name = "lFilterPacket_CNT";
            this.lFilterPacket_CNT.Size = new System.Drawing.Size(9, 24);
            this.lFilterPacket_CNT.TabIndex = 16;
            this.lFilterPacket_CNT.Text = "0";
            // 
            // lFilterPacket
            // 
            this.lFilterPacket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterPacket.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterPacket.LocalizationText = "InjectModeForm.Filter";
            this.lFilterPacket.Location = new System.Drawing.Point(257, 3);
            this.lFilterPacket.Name = "lFilterPacket";
            this.lFilterPacket.Size = new System.Drawing.Size(30, 24);
            this.lFilterPacket.TabIndex = 15;
            this.lFilterPacket.Text = "过滤:";
            // 
            // lQueue_CNT
            // 
            this.lQueue_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lQueue_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lQueue_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lQueue_CNT.Location = new System.Drawing.Point(231, 3);
            this.lQueue_CNT.Name = "lQueue_CNT";
            this.lQueue_CNT.Size = new System.Drawing.Size(9, 24);
            this.lQueue_CNT.TabIndex = 14;
            this.lQueue_CNT.Text = "0";
            // 
            // lFilterExecute_CNT
            // 
            this.lFilterExecute_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterExecute_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterExecute_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterExecute_CNT.Location = new System.Drawing.Point(155, 3);
            this.lFilterExecute_CNT.Name = "lFilterExecute_CNT";
            this.lFilterExecute_CNT.Size = new System.Drawing.Size(9, 24);
            this.lFilterExecute_CNT.TabIndex = 13;
            this.lFilterExecute_CNT.Text = "0";
            // 
            // lTotal_CNT
            // 
            this.lTotal_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_CNT.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_CNT.Location = new System.Drawing.Point(66, 3);
            this.lTotal_CNT.Name = "lTotal_CNT";
            this.lTotal_CNT.Size = new System.Drawing.Size(9, 24);
            this.lTotal_CNT.TabIndex = 12;
            this.lTotal_CNT.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(246, 3);
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
            this.label3.Location = new System.Drawing.Point(170, 3);
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
            // lQueue
            // 
            this.lQueue.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lQueue.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lQueue.LocalizationText = "InjectModeForm.Buffer";
            this.lQueue.Location = new System.Drawing.Point(181, 3);
            this.lQueue.Name = "lQueue";
            this.lQueue.Size = new System.Drawing.Size(44, 24);
            this.lQueue.TabIndex = 7;
            this.lQueue.Text = "缓存区:";
            // 
            // lFilterExecute
            // 
            this.lFilterExecute.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterExecute.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterExecute.LocalizationText = "InjectModeForm.ExecuteFilter";
            this.lFilterExecute.Location = new System.Drawing.Point(92, 3);
            this.lFilterExecute.Name = "lFilterExecute";
            this.lFilterExecute.Size = new System.Drawing.Size(57, 24);
            this.lFilterExecute.TabIndex = 6;
            this.lFilterExecute.Text = "滤镜执行:";
            // 
            // lTotal
            // 
            this.lTotal.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal.LocalizationText = "InjectModeForm.TotalPackets";
            this.lTotal.Location = new System.Drawing.Point(3, 3);
            this.lTotal.Name = "lTotal";
            this.lTotal.Size = new System.Drawing.Size(57, 24);
            this.lTotal.TabIndex = 5;
            this.lTotal.Text = "封包总数:";
            // 
            // tPacketList
            // 
            this.tPacketList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tPacketList.Bordered = true;
            this.tPacketList.CellImpactHeight = false;
            this.tPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPacketList.EmptyHeader = true;
            this.tPacketList.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tPacketList.Gap = 8;
            this.tPacketList.GapCell = 0;
            this.tPacketList.Gaps = new System.Drawing.Size(8, 8);
            this.tPacketList.Location = new System.Drawing.Point(0, 30);
            this.tPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.tPacketList.MultipleRows = true;
            this.tPacketList.Name = "tPacketList";
            this.tPacketList.Size = new System.Drawing.Size(1268, 420);
            this.tPacketList.TabIndex = 1;
            this.tPacketList.CellClick += new AntdUI.Table.ClickEventHandler(this.tPacketList_CellClick);
            this.tPacketList.SetRowStyle += new AntdUI.Table.SetRowStyleEventHandler(this.tPacketList_SetRowStyle);
            this.tPacketList.SelectIndexChanged += new System.EventHandler(this.tPacketList_SelectIndexChanged);
            // 
            // pPacketData
            // 
            this.pPacketData.BorderWidth = 1F;
            this.pPacketData.Controls.Add(this.hbPacketData);
            this.pPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData.Location = new System.Drawing.Point(0, 0);
            this.pPacketData.Name = "pPacketData";
            this.pPacketData.Padding = new System.Windows.Forms.Padding(6);
            this.pPacketData.Radius = 0;
            this.pPacketData.Size = new System.Drawing.Size(1268, 180);
            this.pPacketData.TabIndex = 0;
            // 
            // hbPacketData
            // 
            this.hbPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketData.ColumnInfoVisible = true;
            this.hbPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketData.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Location = new System.Drawing.Point(7, 7);
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ReadOnly = true;
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.Size = new System.Drawing.Size(1254, 166);
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.TabIndex = 1;
            this.hbPacketData.VScrollBarVisible = true;
            this.hbPacketData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbPacketData_MouseDown);
            // 
            // tlpProcessInfo
            // 
            this.tlpProcessInfo.ColumnCount = 7;
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProcessInfo.Controls.Add(this.lSpeedInfo, 6, 0);
            this.tlpProcessInfo.Controls.Add(this.lSplit3, 5, 0);
            this.tlpProcessInfo.Controls.Add(this.lSplit2, 3, 0);
            this.tlpProcessInfo.Controls.Add(this.lSplit1, 1, 0);
            this.tlpProcessInfo.Controls.Add(this.lWinsockInfo, 4, 0);
            this.tlpProcessInfo.Controls.Add(this.lModuleName, 2, 0);
            this.tlpProcessInfo.Controls.Add(this.lProcessName, 0, 0);
            this.tlpProcessInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProcessInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpProcessInfo.Location = new System.Drawing.Point(0, 696);
            this.tlpProcessInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProcessInfo.Name = "tlpProcessInfo";
            this.tlpProcessInfo.RowCount = 1;
            this.tlpProcessInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessInfo.Size = new System.Drawing.Size(1274, 30);
            this.tlpProcessInfo.TabIndex = 4;
            // 
            // lSpeedInfo
            // 
            this.lSpeedInfo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSpeedInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSpeedInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSpeedInfo.Location = new System.Drawing.Point(309, 3);
            this.lSpeedInfo.Name = "lSpeedInfo";
            this.lSpeedInfo.Size = new System.Drawing.Size(66, 24);
            this.lSpeedInfo.TabIndex = 11;
            this.lSpeedInfo.Text = "SpeedInfo";
            // 
            // lSplit3
            // 
            this.lSplit3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit3.ForeColor = System.Drawing.Color.Silver;
            this.lSplit3.Location = new System.Drawing.Point(298, 3);
            this.lSplit3.Name = "lSplit3";
            this.lSplit3.Size = new System.Drawing.Size(5, 24);
            this.lSplit3.TabIndex = 10;
            this.lSplit3.Text = "|";
            this.lSplit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSplit2
            // 
            this.lSplit2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit2.ForeColor = System.Drawing.Color.Silver;
            this.lSplit2.Location = new System.Drawing.Point(201, 3);
            this.lSplit2.Name = "lSplit2";
            this.lSplit2.Size = new System.Drawing.Size(5, 24);
            this.lSplit2.TabIndex = 9;
            this.lSplit2.Text = "|";
            this.lSplit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSplit1
            // 
            this.lSplit1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit1.ForeColor = System.Drawing.Color.Silver;
            this.lSplit1.Location = new System.Drawing.Point(96, 3);
            this.lSplit1.Name = "lSplit1";
            this.lSplit1.Size = new System.Drawing.Size(5, 24);
            this.lSplit1.TabIndex = 8;
            this.lSplit1.Text = "|";
            this.lSplit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWinsockInfo
            // 
            this.lWinsockInfo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWinsockInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWinsockInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWinsockInfo.Location = new System.Drawing.Point(212, 3);
            this.lWinsockInfo.Name = "lWinsockInfo";
            this.lWinsockInfo.Size = new System.Drawing.Size(80, 24);
            this.lWinsockInfo.TabIndex = 7;
            this.lWinsockInfo.Text = "WinsockInfo";
            // 
            // lModuleName
            // 
            this.lModuleName.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lModuleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lModuleName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lModuleName.Location = new System.Drawing.Point(107, 3);
            this.lModuleName.Name = "lModuleName";
            this.lModuleName.Size = new System.Drawing.Size(88, 24);
            this.lModuleName.TabIndex = 6;
            this.lModuleName.Text = "ModuleName";
            // 
            // lProcessName
            // 
            this.lProcessName.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProcessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProcessName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProcessName.Location = new System.Drawing.Point(3, 3);
            this.lProcessName.Name = "lProcessName";
            this.lProcessName.Size = new System.Drawing.Size(87, 24);
            this.lProcessName.TabIndex = 5;
            this.lProcessName.Text = "ProcessName";
            // 
            // tpFilterList
            // 
            this.tpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterList.Location = new System.Drawing.Point(3, 33);
            this.tpFilterList.Name = "tpFilterList";
            this.tpFilterList.Size = new System.Drawing.Size(1274, 726);
            this.tpFilterList.TabIndex = 7;
            this.tpFilterList.Text = "滤镜列表";
            // 
            // tpSendList
            // 
            this.tpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSendList.Location = new System.Drawing.Point(3, 33);
            this.tpSendList.Name = "tpSendList";
            this.tpSendList.Size = new System.Drawing.Size(1274, 726);
            this.tpSendList.TabIndex = 8;
            this.tpSendList.Text = "发送列表";
            // 
            // tpRobotList
            // 
            this.tpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRobotList.Location = new System.Drawing.Point(3, 33);
            this.tpRobotList.Name = "tpRobotList";
            this.tpRobotList.Size = new System.Drawing.Size(1274, 726);
            this.tpRobotList.TabIndex = 9;
            this.tpRobotList.Text = "机器人列表";
            // 
            // tpStatistical
            // 
            this.tpStatistical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpStatistical.Location = new System.Drawing.Point(3, 33);
            this.tpStatistical.Name = "tpStatistical";
            this.tpStatistical.Size = new System.Drawing.Size(1274, 726);
            this.tpStatistical.TabIndex = 1;
            this.tpStatistical.Text = "统计数据";
            // 
            // tpComparison
            // 
            this.tpComparison.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpComparison.Location = new System.Drawing.Point(3, 33);
            this.tpComparison.Name = "tpComparison";
            this.tpComparison.Size = new System.Drawing.Size(1274, 726);
            this.tpComparison.TabIndex = 2;
            this.tpComparison.Text = "文本对比";
            // 
            // tpXOR
            // 
            this.tpXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpXOR.Location = new System.Drawing.Point(3, 33);
            this.tpXOR.Name = "tpXOR";
            this.tpXOR.Size = new System.Drawing.Size(1274, 726);
            this.tpXOR.TabIndex = 3;
            this.tpXOR.Text = "异或计算";
            // 
            // tpTranscoding
            // 
            this.tpTranscoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTranscoding.Location = new System.Drawing.Point(3, 33);
            this.tpTranscoding.Name = "tpTranscoding";
            this.tpTranscoding.Size = new System.Drawing.Size(1274, 726);
            this.tpTranscoding.TabIndex = 4;
            this.tpTranscoding.Text = "编码转换";
            // 
            // tpExtraction
            // 
            this.tpExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpExtraction.Location = new System.Drawing.Point(3, 33);
            this.tpExtraction.Name = "tpExtraction";
            this.tpExtraction.Size = new System.Drawing.Size(1274, 726);
            this.tpExtraction.TabIndex = 5;
            this.tpExtraction.Text = "数据提取";
            // 
            // tpSystemLog
            // 
            this.tpSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSystemLog.Location = new System.Drawing.Point(3, 33);
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.Size = new System.Drawing.Size(1274, 726);
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
            // bgwSearchPacketList
            // 
            this.bgwSearchPacketList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSearchPacketList_DoWork);
            this.bgwSearchPacketList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSearchPacketList_RunWorkerCompleted);
            // 
            // bgwPacketList
            // 
            this.bgwPacketList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwPacketList_DoWork);
            this.bgwPacketList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwPacketList_RunWorkerCompleted);
            // 
            // InjectModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1450, 802);
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
            this.tlpMenu.PerformLayout();
            this.tabInjectMode.ResumeLayout(false);
            this.tpPacketList.ResumeLayout(false);
            this.tlpPacketList.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.splitterPacketList.Panel1.ResumeLayout(false);
            this.splitterPacketList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterPacketList)).EndInit();
            this.splitterPacketList.ResumeLayout(false);
            this.tlpPacketList2.ResumeLayout(false);
            this.tlpPacketListInfo.ResumeLayout(false);
            this.tlpPacketListInfo.PerformLayout();
            this.pPacketData.ResumeLayout(false);
            this.tlpProcessInfo.ResumeLayout(false);
            this.tlpProcessInfo.PerformLayout();
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
        private AntdUI.TabPage tpSystemLog;
        private System.Windows.Forms.TableLayoutPanel tlpPacketList;
        private AntdUI.TabPage tpFilterList;
        private AntdUI.TabPage tpSendList;
        private AntdUI.TabPage tpRobotList;
        private AntdUI.Splitter splitterPacketList;
        private System.Windows.Forms.TableLayoutPanel tlpProcessInfo;
        private AntdUI.Label lWinsockInfo;
        private AntdUI.Label lModuleName;
        private AntdUI.Label lProcessName;
        private AntdUI.Label lSplit2;
        private AntdUI.Label lSplit1;
        private AntdUI.Label lSpeedInfo;
        private AntdUI.Label lSplit3;
        private System.Windows.Forms.Timer timerPacketList;
        private System.Windows.Forms.Timer timerPacketListInfo;
        private System.ComponentModel.BackgroundWorker bgwSearchPacketList;
        private System.ComponentModel.BackgroundWorker bgwPacketList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Button bHookStop;
        private AntdUI.Menu mPacketList;
        private AntdUI.Button bHookStart;
        private AntdUI.Button bPacketList_Clear;
        private System.Windows.Forms.TableLayoutPanel tlpPacketList2;
        private System.Windows.Forms.TableLayoutPanel tlpPacketListInfo;
        private AntdUI.Label lWSARecvFrom_CNT;
        private AntdUI.Label lWSARecvFrom;
        private AntdUI.Label label33;
        private AntdUI.Label lWSASendTo_CNT;
        private AntdUI.Label lWSASendTo;
        private AntdUI.Label label30;
        private AntdUI.Label lWSARecv_CNT;
        private AntdUI.Label lWSARecv;
        private AntdUI.Label label27;
        private AntdUI.Label lWSASend_CNT;
        private AntdUI.Label lWSASend;
        private AntdUI.Label label24;
        private AntdUI.Label lRecvFrom_CNT;
        private AntdUI.Label lRecvFrom;
        private AntdUI.Label label21;
        private AntdUI.Label lSendTo_CNT;
        private AntdUI.Label lSendTo;
        private AntdUI.Label label18;
        private AntdUI.Label lRecv_CNT;
        private AntdUI.Label lRecv;
        private AntdUI.Label label15;
        private AntdUI.Label lSend_CNT;
        private AntdUI.Label lSend;
        private AntdUI.Label label12;
        private AntdUI.Label lFilterPacket_CNT;
        private AntdUI.Label lFilterPacket;
        private AntdUI.Label lQueue_CNT;
        private AntdUI.Label lFilterExecute_CNT;
        private AntdUI.Label lTotal_CNT;
        private AntdUI.Label label2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label lQueue;
        private AntdUI.Label lFilterExecute;
        private AntdUI.Label lTotal;
        private AntdUI.Table tPacketList;
        private AntdUI.Panel pPacketData;
        private Be.Windows.Forms.HexBox hbPacketData;
    }
}
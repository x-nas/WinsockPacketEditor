namespace WinsockPacketEditor
{
    partial class ProxyList
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpProxyList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpProxyList_Button = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtPacketList_AutoClear = new AntdUI.InputNumber();
            this.cbPacketList_AutoClear = new AntdUI.Checkbox();
            this.cbPacketList_AutoRoll = new AntdUI.Checkbox();
            this.bProxyList_Clear = new AntdUI.Button();
            this.bProxyStop = new AntdUI.Button();
            this.bProxyStart = new AntdUI.Button();
            this.ddMenu = new AntdUI.Dropdown();
            this.bSearchPacket = new AntdUI.Button();
            this.splitterProxyList = new AntdUI.Splitter();
            this.tlpProxyList2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpPacketListInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lSplit16 = new AntdUI.Label();
            this.lFilterProxy_CNT = new AntdUI.Label();
            this.lFilterProxy = new AntdUI.Label();
            this.lSplit14 = new AntdUI.Label();
            this.lProxyUDP_CNT = new AntdUI.Label();
            this.lProxyUDP = new AntdUI.Label();
            this.lUDP_Resp_CNT = new AntdUI.Label();
            this.lUDP_Resp = new AntdUI.Label();
            this.lSplit5 = new AntdUI.Label();
            this.lUDP_Req_CNT = new AntdUI.Label();
            this.lUDP_Req = new AntdUI.Label();
            this.lSplit4 = new AntdUI.Label();
            this.lsplit2 = new AntdUI.Label();
            this.lTCP_Resp_CNT = new AntdUI.Label();
            this.lTCP_Resp = new AntdUI.Label();
            this.lSplit18 = new AntdUI.Label();
            this.lTCP_Req_CNT = new AntdUI.Label();
            this.lTCP_Req = new AntdUI.Label();
            this.lsplit7 = new AntdUI.Label();
            this.lFilterExecute_CNT = new AntdUI.Label();
            this.lFilterExecute = new AntdUI.Label();
            this.lProxyTCP_CNT = new AntdUI.Label();
            this.lProxyTCP = new AntdUI.Label();
            this.lsplit15 = new AntdUI.Label();
            this.lProxyAccount_CNT = new AntdUI.Label();
            this.lProxyAccount = new AntdUI.Label();
            this.lsplit8 = new AntdUI.Label();
            this.lProxyQueue_CNT = new AntdUI.Label();
            this.lProxyQueue = new AntdUI.Label();
            this.lProxyTotal_CNT = new AntdUI.Label();
            this.lsplit6 = new AntdUI.Label();
            this.lProxyTotal = new AntdUI.Label();
            this.tProxyList = new AntdUI.Table();
            this.pPacketData = new AntdUI.Panel();
            this.splitterQuickList = new AntdUI.Splitter();
            this.hbProxyData = new Be.Windows.Forms.HexBox();
            this.tlpProxyInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lTotalBytes = new AntdUI.Label();
            this.lSplit1 = new AntdUI.Label();
            this.lProxySpeed = new AntdUI.Label();
            this.bgwSearchProxyList = new System.ComponentModel.BackgroundWorker();
            this.tlpProxyList.SuspendLayout();
            this.tlpProxyList_Button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterProxyList)).BeginInit();
            this.splitterProxyList.Panel1.SuspendLayout();
            this.splitterProxyList.Panel2.SuspendLayout();
            this.splitterProxyList.SuspendLayout();
            this.tlpProxyList2.SuspendLayout();
            this.tlpPacketListInfo.SuspendLayout();
            this.pPacketData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterQuickList)).BeginInit();
            this.splitterQuickList.Panel2.SuspendLayout();
            this.splitterQuickList.SuspendLayout();
            this.tlpProxyInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyList
            // 
            this.tlpProxyList.ColumnCount = 1;
            this.tlpProxyList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList.Controls.Add(this.tlpProxyList_Button, 0, 0);
            this.tlpProxyList.Controls.Add(this.splitterProxyList, 0, 1);
            this.tlpProxyList.Controls.Add(this.tlpProxyInfo, 0, 2);
            this.tlpProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyList.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyList.Name = "tlpProxyList";
            this.tlpProxyList.RowCount = 3;
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpProxyList.Size = new System.Drawing.Size(1100, 700);
            this.tlpProxyList.TabIndex = 11;
            // 
            // tlpProxyList_Button
            // 
            this.tlpProxyList_Button.ColumnCount = 9;
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpProxyList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpProxyList_Button.Controls.Add(this.txtPacketList_AutoClear, 4, 0);
            this.tlpProxyList_Button.Controls.Add(this.cbPacketList_AutoClear, 3, 0);
            this.tlpProxyList_Button.Controls.Add(this.cbPacketList_AutoRoll, 5, 0);
            this.tlpProxyList_Button.Controls.Add(this.bProxyList_Clear, 2, 0);
            this.tlpProxyList_Button.Controls.Add(this.bProxyStop, 1, 0);
            this.tlpProxyList_Button.Controls.Add(this.bProxyStart, 0, 0);
            this.tlpProxyList_Button.Controls.Add(this.ddMenu, 8, 0);
            this.tlpProxyList_Button.Controls.Add(this.bSearchPacket, 7, 0);
            this.tlpProxyList_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyList_Button.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyList_Button.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyList_Button.Name = "tlpProxyList_Button";
            this.tlpProxyList_Button.RowCount = 1;
            this.tlpProxyList_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList_Button.Size = new System.Drawing.Size(1100, 40);
            this.tlpProxyList_Button.TabIndex = 7;
            // 
            // txtPacketList_AutoClear
            // 
            this.txtPacketList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketList_AutoClear.Location = new System.Drawing.Point(287, 2);
            this.txtPacketList_AutoClear.Margin = new System.Windows.Forms.Padding(2);
            this.txtPacketList_AutoClear.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.txtPacketList_AutoClear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPacketList_AutoClear.Name = "txtPacketList_AutoClear";
            this.txtPacketList_AutoClear.SelectionStart = 1;
            this.txtPacketList_AutoClear.Size = new System.Drawing.Size(80, 36);
            this.txtPacketList_AutoClear.TabIndex = 17;
            this.txtPacketList_AutoClear.Text = "5000";
            this.txtPacketList_AutoClear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPacketList_AutoClear.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtPacketList_AutoClear.ValueChanged += new AntdUI.DecimalEventHandler(this.txtPacketList_AutoClear_ValueChanged);
            // 
            // cbPacketList_AutoClear
            // 
            this.cbPacketList_AutoClear.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbPacketList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPacketList_AutoClear.LocalizationText = "ListSettingsForm.AutoClear";
            this.cbPacketList_AutoClear.Location = new System.Drawing.Point(203, 2);
            this.cbPacketList_AutoClear.Margin = new System.Windows.Forms.Padding(2);
            this.cbPacketList_AutoClear.Name = "cbPacketList_AutoClear";
            this.cbPacketList_AutoClear.Size = new System.Drawing.Size(80, 36);
            this.cbPacketList_AutoClear.TabIndex = 16;
            this.cbPacketList_AutoClear.Text = "自动清理";
            this.cbPacketList_AutoClear.CheckedChanged += new AntdUI.BoolEventHandler(this.cbPacketList_AutoClear_CheckedChanged);
            // 
            // cbPacketList_AutoRoll
            // 
            this.cbPacketList_AutoRoll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbPacketList_AutoRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPacketList_AutoRoll.LocalizationText = "ListSettingsForm.AutoRoll";
            this.cbPacketList_AutoRoll.Location = new System.Drawing.Point(371, 2);
            this.cbPacketList_AutoRoll.Margin = new System.Windows.Forms.Padding(2);
            this.cbPacketList_AutoRoll.Name = "cbPacketList_AutoRoll";
            this.cbPacketList_AutoRoll.Size = new System.Drawing.Size(80, 36);
            this.cbPacketList_AutoRoll.TabIndex = 15;
            this.cbPacketList_AutoRoll.Text = "自动滚动";
            this.cbPacketList_AutoRoll.CheckedChanged += new AntdUI.BoolEventHandler(this.cbPacketList_AutoRoll_CheckedChanged);
            // 
            // bProxyList_Clear
            // 
            this.bProxyList_Clear.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bProxyList_Clear.BorderWidth = 1F;
            this.bProxyList_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bProxyList_Clear.IconSvg = "DeleteOutlined";
            this.bProxyList_Clear.LocalizationText = "Clear";
            this.bProxyList_Clear.Location = new System.Drawing.Point(136, 2);
            this.bProxyList_Clear.Margin = new System.Windows.Forms.Padding(2);
            this.bProxyList_Clear.Name = "bProxyList_Clear";
            this.bProxyList_Clear.Size = new System.Drawing.Size(63, 36);
            this.bProxyList_Clear.TabIndex = 9;
            this.bProxyList_Clear.Text = "清空";
            this.bProxyList_Clear.Type = AntdUI.TTypeMini.Warn;
            this.bProxyList_Clear.Click += new System.EventHandler(this.bProxyList_Clear_Click);
            // 
            // bProxyStop
            // 
            this.bProxyStop.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bProxyStop.BorderWidth = 1F;
            this.bProxyStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bProxyStop.Enabled = false;
            this.bProxyStop.IconSvg = "PauseCircleOutlined";
            this.bProxyStop.LocalizationText = "Stop";
            this.bProxyStop.Location = new System.Drawing.Point(69, 2);
            this.bProxyStop.Margin = new System.Windows.Forms.Padding(2);
            this.bProxyStop.Name = "bProxyStop";
            this.bProxyStop.Size = new System.Drawing.Size(63, 36);
            this.bProxyStop.TabIndex = 8;
            this.bProxyStop.Text = "停止";
            this.bProxyStop.Type = AntdUI.TTypeMini.Error;
            this.bProxyStop.Click += new System.EventHandler(this.bProxyStop_Click);
            // 
            // bProxyStart
            // 
            this.bProxyStart.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bProxyStart.BorderWidth = 1F;
            this.bProxyStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bProxyStart.IconSvg = "PlayCircleOutlined";
            this.bProxyStart.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bProxyStart.LoadingWaveCount = 6;
            this.bProxyStart.LoadingWaveSize = 6;
            this.bProxyStart.LoadingWaveValue = 0.6F;
            this.bProxyStart.LoadingWaveVertical = true;
            this.bProxyStart.LocalizationText = "Start";
            this.bProxyStart.Location = new System.Drawing.Point(2, 2);
            this.bProxyStart.Margin = new System.Windows.Forms.Padding(2);
            this.bProxyStart.Name = "bProxyStart";
            this.bProxyStart.Size = new System.Drawing.Size(63, 36);
            this.bProxyStart.TabIndex = 7;
            this.bProxyStart.Text = "开始";
            this.bProxyStart.Type = AntdUI.TTypeMini.Info;
            this.bProxyStart.Click += new System.EventHandler(this.bProxyStart_Click);
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(1066, 2);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Size = new System.Drawing.Size(32, 36);
            this.ddMenu.TabIndex = 10;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // bSearchPacket
            // 
            this.bSearchPacket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSearchPacket.Ghost = true;
            this.bSearchPacket.IconRatio = 1F;
            this.bSearchPacket.IconSvg = "SearchOutlined";
            this.bSearchPacket.Location = new System.Drawing.Point(1030, 2);
            this.bSearchPacket.Margin = new System.Windows.Forms.Padding(2);
            this.bSearchPacket.Name = "bSearchPacket";
            this.bSearchPacket.Size = new System.Drawing.Size(32, 36);
            this.bSearchPacket.TabIndex = 11;
            this.bSearchPacket.WaveSize = 0;
            this.bSearchPacket.Click += new System.EventHandler(this.bSearchPacket_Click);
            // 
            // splitterProxyList
            // 
            this.splitterProxyList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterProxyList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterProxyList.Location = new System.Drawing.Point(2, 42);
            this.splitterProxyList.Margin = new System.Windows.Forms.Padding(2);
            this.splitterProxyList.Name = "splitterProxyList";
            this.splitterProxyList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterProxyList.Panel1
            // 
            this.splitterProxyList.Panel1.Controls.Add(this.tlpProxyList2);
            this.splitterProxyList.Panel1MinSize = 0;
            // 
            // splitterProxyList.Panel2
            // 
            this.splitterProxyList.Panel2.Controls.Add(this.pPacketData);
            this.splitterProxyList.Panel2MinSize = 0;
            this.splitterProxyList.Size = new System.Drawing.Size(1096, 632);
            this.splitterProxyList.SplitterDistance = 450;
            this.splitterProxyList.SplitterSize = 80;
            this.splitterProxyList.SplitterWidth = 10;
            this.splitterProxyList.TabIndex = 6;
            // 
            // tlpProxyList2
            // 
            this.tlpProxyList2.ColumnCount = 1;
            this.tlpProxyList2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList2.Controls.Add(this.tlpPacketListInfo, 0, 0);
            this.tlpProxyList2.Controls.Add(this.tProxyList, 0, 1);
            this.tlpProxyList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyList2.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyList2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyList2.Name = "tlpProxyList2";
            this.tlpProxyList2.RowCount = 2;
            this.tlpProxyList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList2.Size = new System.Drawing.Size(1096, 450);
            this.tlpProxyList2.TabIndex = 0;
            // 
            // tlpPacketListInfo
            // 
            this.tlpPacketListInfo.ColumnCount = 33;
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
            this.tlpPacketListInfo.Controls.Add(this.lSplit16, 11, 0);
            this.tlpPacketListInfo.Controls.Add(this.lFilterProxy_CNT, 10, 0);
            this.tlpPacketListInfo.Controls.Add(this.lFilterProxy, 9, 0);
            this.tlpPacketListInfo.Controls.Add(this.lSplit14, 20, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyUDP_CNT, 19, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyUDP, 18, 0);
            this.tlpPacketListInfo.Controls.Add(this.lUDP_Resp_CNT, 31, 0);
            this.tlpPacketListInfo.Controls.Add(this.lUDP_Resp, 30, 0);
            this.tlpPacketListInfo.Controls.Add(this.lSplit5, 29, 0);
            this.tlpPacketListInfo.Controls.Add(this.lUDP_Req_CNT, 28, 0);
            this.tlpPacketListInfo.Controls.Add(this.lUDP_Req, 27, 0);
            this.tlpPacketListInfo.Controls.Add(this.lSplit4, 26, 0);
            this.tlpPacketListInfo.Controls.Add(this.lsplit2, 17, 0);
            this.tlpPacketListInfo.Controls.Add(this.lTCP_Resp_CNT, 25, 0);
            this.tlpPacketListInfo.Controls.Add(this.lTCP_Resp, 24, 0);
            this.tlpPacketListInfo.Controls.Add(this.lSplit18, 23, 0);
            this.tlpPacketListInfo.Controls.Add(this.lTCP_Req_CNT, 22, 0);
            this.tlpPacketListInfo.Controls.Add(this.lTCP_Req, 21, 0);
            this.tlpPacketListInfo.Controls.Add(this.lsplit7, 5, 0);
            this.tlpPacketListInfo.Controls.Add(this.lFilterExecute_CNT, 4, 0);
            this.tlpPacketListInfo.Controls.Add(this.lFilterExecute, 3, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTCP_CNT, 16, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTCP, 15, 0);
            this.tlpPacketListInfo.Controls.Add(this.lsplit15, 14, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyAccount_CNT, 13, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyAccount, 12, 0);
            this.tlpPacketListInfo.Controls.Add(this.lsplit8, 8, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyQueue_CNT, 7, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyQueue, 6, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTotal_CNT, 1, 0);
            this.tlpPacketListInfo.Controls.Add(this.lsplit6, 2, 0);
            this.tlpPacketListInfo.Controls.Add(this.lProxyTotal, 0, 0);
            this.tlpPacketListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpPacketListInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketListInfo.Name = "tlpPacketListInfo";
            this.tlpPacketListInfo.RowCount = 2;
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketListInfo.Size = new System.Drawing.Size(1096, 24);
            this.tlpPacketListInfo.TabIndex = 6;
            // 
            // lSplit16
            // 
            this.lSplit16.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit16.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit16.ForeColor = System.Drawing.Color.Silver;
            this.lSplit16.Location = new System.Drawing.Point(277, 2);
            this.lSplit16.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit16.Name = "lSplit16";
            this.lSplit16.Size = new System.Drawing.Size(5, 20);
            this.lSplit16.TabIndex = 47;
            this.lSplit16.Text = "|";
            this.lSplit16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFilterProxy_CNT
            // 
            this.lFilterProxy_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterProxy_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterProxy_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterProxy_CNT.Location = new System.Drawing.Point(265, 2);
            this.lFilterProxy_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterProxy_CNT.Name = "lFilterProxy_CNT";
            this.lFilterProxy_CNT.Size = new System.Drawing.Size(8, 20);
            this.lFilterProxy_CNT.TabIndex = 46;
            this.lFilterProxy_CNT.Text = "0";
            // 
            // lFilterProxy
            // 
            this.lFilterProxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterProxy.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterProxy.LocalizationText = "ProxyModeForm.Filter";
            this.lFilterProxy.Location = new System.Drawing.Point(230, 2);
            this.lFilterProxy.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterProxy.Name = "lFilterProxy";
            this.lFilterProxy.Size = new System.Drawing.Size(31, 20);
            this.lFilterProxy.TabIndex = 45;
            this.lFilterProxy.Text = "过滤 :";
            // 
            // lSplit14
            // 
            this.lSplit14.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit14.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit14.ForeColor = System.Drawing.Color.Silver;
            this.lSplit14.Location = new System.Drawing.Point(549, 2);
            this.lSplit14.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit14.Name = "lSplit14";
            this.lSplit14.Size = new System.Drawing.Size(5, 20);
            this.lSplit14.TabIndex = 44;
            this.lSplit14.Text = "|";
            this.lSplit14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyUDP_CNT
            // 
            this.lProxyUDP_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyUDP_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyUDP_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyUDP_CNT.Location = new System.Drawing.Point(537, 2);
            this.lProxyUDP_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyUDP_CNT.Name = "lProxyUDP_CNT";
            this.lProxyUDP_CNT.Size = new System.Drawing.Size(8, 20);
            this.lProxyUDP_CNT.TabIndex = 43;
            this.lProxyUDP_CNT.Text = "0";
            // 
            // lProxyUDP
            // 
            this.lProxyUDP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyUDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyUDP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyUDP.LocalizationText = "ProxyModeForm.UDPLink";
            this.lProxyUDP.Location = new System.Drawing.Point(461, 2);
            this.lProxyUDP.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyUDP.Name = "lProxyUDP";
            this.lProxyUDP.Size = new System.Drawing.Size(72, 20);
            this.lProxyUDP.TabIndex = 42;
            this.lProxyUDP.Text = "UDP 连接数 :";
            // 
            // lUDP_Resp_CNT
            // 
            this.lUDP_Resp_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUDP_Resp_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUDP_Resp_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUDP_Resp_CNT.Location = new System.Drawing.Point(871, 2);
            this.lUDP_Resp_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lUDP_Resp_CNT.Name = "lUDP_Resp_CNT";
            this.lUDP_Resp_CNT.Size = new System.Drawing.Size(8, 20);
            this.lUDP_Resp_CNT.TabIndex = 41;
            this.lUDP_Resp_CNT.Text = "0";
            // 
            // lUDP_Resp
            // 
            this.lUDP_Resp.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUDP_Resp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUDP_Resp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUDP_Resp.LocalizationText = "ProxyModeForm.UDPResp";
            this.lUDP_Resp.Location = new System.Drawing.Point(807, 2);
            this.lUDP_Resp.Margin = new System.Windows.Forms.Padding(2);
            this.lUDP_Resp.Name = "lUDP_Resp";
            this.lUDP_Resp.Size = new System.Drawing.Size(60, 20);
            this.lUDP_Resp.TabIndex = 40;
            this.lUDP_Resp.Text = "UDP 响应 :";
            // 
            // lSplit5
            // 
            this.lSplit5.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit5.ForeColor = System.Drawing.Color.Silver;
            this.lSplit5.Location = new System.Drawing.Point(798, 2);
            this.lSplit5.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit5.Name = "lSplit5";
            this.lSplit5.Size = new System.Drawing.Size(5, 20);
            this.lSplit5.TabIndex = 39;
            this.lSplit5.Text = "|";
            this.lSplit5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lUDP_Req_CNT
            // 
            this.lUDP_Req_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUDP_Req_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUDP_Req_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUDP_Req_CNT.Location = new System.Drawing.Point(786, 2);
            this.lUDP_Req_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lUDP_Req_CNT.Name = "lUDP_Req_CNT";
            this.lUDP_Req_CNT.Size = new System.Drawing.Size(8, 20);
            this.lUDP_Req_CNT.TabIndex = 38;
            this.lUDP_Req_CNT.Text = "0";
            // 
            // lUDP_Req
            // 
            this.lUDP_Req.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUDP_Req.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUDP_Req.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUDP_Req.LocalizationText = "ProxyModeForm.UDPReq";
            this.lUDP_Req.Location = new System.Drawing.Point(722, 2);
            this.lUDP_Req.Margin = new System.Windows.Forms.Padding(2);
            this.lUDP_Req.Name = "lUDP_Req";
            this.lUDP_Req.Size = new System.Drawing.Size(60, 20);
            this.lUDP_Req.TabIndex = 37;
            this.lUDP_Req.Text = "UDP 请求 :";
            // 
            // lSplit4
            // 
            this.lSplit4.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit4.ForeColor = System.Drawing.Color.Silver;
            this.lSplit4.Location = new System.Drawing.Point(713, 2);
            this.lSplit4.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit4.Name = "lSplit4";
            this.lSplit4.Size = new System.Drawing.Size(5, 20);
            this.lSplit4.TabIndex = 36;
            this.lSplit4.Text = "|";
            this.lSplit4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lsplit2
            // 
            this.lsplit2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lsplit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsplit2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsplit2.ForeColor = System.Drawing.Color.Silver;
            this.lsplit2.Location = new System.Drawing.Point(452, 2);
            this.lsplit2.Margin = new System.Windows.Forms.Padding(2);
            this.lsplit2.Name = "lsplit2";
            this.lsplit2.Size = new System.Drawing.Size(5, 20);
            this.lsplit2.TabIndex = 32;
            this.lsplit2.Text = "|";
            this.lsplit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTCP_Resp_CNT
            // 
            this.lTCP_Resp_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTCP_Resp_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTCP_Resp_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTCP_Resp_CNT.Location = new System.Drawing.Point(701, 2);
            this.lTCP_Resp_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lTCP_Resp_CNT.Name = "lTCP_Resp_CNT";
            this.lTCP_Resp_CNT.Size = new System.Drawing.Size(8, 20);
            this.lTCP_Resp_CNT.TabIndex = 30;
            this.lTCP_Resp_CNT.Text = "0";
            // 
            // lTCP_Resp
            // 
            this.lTCP_Resp.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTCP_Resp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTCP_Resp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTCP_Resp.LocalizationText = "ProxyModeForm.TCPResp";
            this.lTCP_Resp.Location = new System.Drawing.Point(640, 2);
            this.lTCP_Resp.Margin = new System.Windows.Forms.Padding(2);
            this.lTCP_Resp.Name = "lTCP_Resp";
            this.lTCP_Resp.Size = new System.Drawing.Size(57, 20);
            this.lTCP_Resp.TabIndex = 29;
            this.lTCP_Resp.Text = "TCP 响应 :";
            // 
            // lSplit18
            // 
            this.lSplit18.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit18.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit18.ForeColor = System.Drawing.Color.Silver;
            this.lSplit18.Location = new System.Drawing.Point(631, 2);
            this.lSplit18.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit18.Name = "lSplit18";
            this.lSplit18.Size = new System.Drawing.Size(5, 20);
            this.lSplit18.TabIndex = 28;
            this.lSplit18.Text = "|";
            this.lSplit18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTCP_Req_CNT
            // 
            this.lTCP_Req_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTCP_Req_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTCP_Req_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTCP_Req_CNT.Location = new System.Drawing.Point(619, 2);
            this.lTCP_Req_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lTCP_Req_CNT.Name = "lTCP_Req_CNT";
            this.lTCP_Req_CNT.Size = new System.Drawing.Size(8, 20);
            this.lTCP_Req_CNT.TabIndex = 27;
            this.lTCP_Req_CNT.Text = "0";
            // 
            // lTCP_Req
            // 
            this.lTCP_Req.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTCP_Req.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTCP_Req.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTCP_Req.LocalizationText = "ProxyModeForm.TCPReq";
            this.lTCP_Req.Location = new System.Drawing.Point(558, 2);
            this.lTCP_Req.Margin = new System.Windows.Forms.Padding(2);
            this.lTCP_Req.Name = "lTCP_Req";
            this.lTCP_Req.Size = new System.Drawing.Size(57, 20);
            this.lTCP_Req.TabIndex = 26;
            this.lTCP_Req.Text = "TCP 请求 :";
            // 
            // lsplit7
            // 
            this.lsplit7.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lsplit7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsplit7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsplit7.ForeColor = System.Drawing.Color.Silver;
            this.lsplit7.Location = new System.Drawing.Point(153, 2);
            this.lsplit7.Margin = new System.Windows.Forms.Padding(2);
            this.lsplit7.Name = "lsplit7";
            this.lsplit7.Size = new System.Drawing.Size(5, 20);
            this.lsplit7.TabIndex = 25;
            this.lsplit7.Text = "|";
            this.lsplit7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFilterExecute_CNT
            // 
            this.lFilterExecute_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterExecute_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterExecute_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterExecute_CNT.Location = new System.Drawing.Point(141, 2);
            this.lFilterExecute_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterExecute_CNT.Name = "lFilterExecute_CNT";
            this.lFilterExecute_CNT.Size = new System.Drawing.Size(8, 20);
            this.lFilterExecute_CNT.TabIndex = 24;
            this.lFilterExecute_CNT.Text = "0";
            // 
            // lFilterExecute
            // 
            this.lFilterExecute.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterExecute.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterExecute.LocalizationText = "ProxyModeForm.ExecuteFilter";
            this.lFilterExecute.Location = new System.Drawing.Point(82, 2);
            this.lFilterExecute.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterExecute.Name = "lFilterExecute";
            this.lFilterExecute.Size = new System.Drawing.Size(55, 20);
            this.lFilterExecute.TabIndex = 23;
            this.lFilterExecute.Text = "滤镜执行 :";
            // 
            // lProxyTCP_CNT
            // 
            this.lProxyTCP_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTCP_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTCP_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTCP_CNT.Location = new System.Drawing.Point(440, 2);
            this.lProxyTCP_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyTCP_CNT.Name = "lProxyTCP_CNT";
            this.lProxyTCP_CNT.Size = new System.Drawing.Size(8, 20);
            this.lProxyTCP_CNT.TabIndex = 22;
            this.lProxyTCP_CNT.Text = "0";
            // 
            // lProxyTCP
            // 
            this.lProxyTCP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTCP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTCP.LocalizationText = "ProxyModeForm.TCPLink";
            this.lProxyTCP.Location = new System.Drawing.Point(367, 2);
            this.lProxyTCP.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyTCP.Name = "lProxyTCP";
            this.lProxyTCP.Size = new System.Drawing.Size(69, 20);
            this.lProxyTCP.TabIndex = 21;
            this.lProxyTCP.Text = "TCP 连接数 :";
            // 
            // lsplit15
            // 
            this.lsplit15.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lsplit15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsplit15.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsplit15.ForeColor = System.Drawing.Color.Silver;
            this.lsplit15.Location = new System.Drawing.Point(358, 2);
            this.lsplit15.Margin = new System.Windows.Forms.Padding(2);
            this.lsplit15.Name = "lsplit15";
            this.lsplit15.Size = new System.Drawing.Size(5, 20);
            this.lsplit15.TabIndex = 20;
            this.lsplit15.Text = "|";
            this.lsplit15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyAccount_CNT
            // 
            this.lProxyAccount_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyAccount_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyAccount_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyAccount_CNT.Location = new System.Drawing.Point(333, 2);
            this.lProxyAccount_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyAccount_CNT.Name = "lProxyAccount_CNT";
            this.lProxyAccount_CNT.Size = new System.Drawing.Size(21, 20);
            this.lProxyAccount_CNT.TabIndex = 19;
            this.lProxyAccount_CNT.Text = "0/0";
            // 
            // lProxyAccount
            // 
            this.lProxyAccount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyAccount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyAccount.LocalizationText = "ProxyModeForm.Account";
            this.lProxyAccount.Location = new System.Drawing.Point(286, 2);
            this.lProxyAccount.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyAccount.Name = "lProxyAccount";
            this.lProxyAccount.Size = new System.Drawing.Size(43, 20);
            this.lProxyAccount.TabIndex = 18;
            this.lProxyAccount.Text = "账号数 :";
            // 
            // lsplit8
            // 
            this.lsplit8.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lsplit8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsplit8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsplit8.ForeColor = System.Drawing.Color.Silver;
            this.lsplit8.Location = new System.Drawing.Point(221, 2);
            this.lsplit8.Margin = new System.Windows.Forms.Padding(2);
            this.lsplit8.Name = "lsplit8";
            this.lsplit8.Size = new System.Drawing.Size(5, 20);
            this.lsplit8.TabIndex = 17;
            this.lsplit8.Text = "|";
            this.lsplit8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyQueue_CNT
            // 
            this.lProxyQueue_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyQueue_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyQueue_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyQueue_CNT.Location = new System.Drawing.Point(209, 2);
            this.lProxyQueue_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyQueue_CNT.Name = "lProxyQueue_CNT";
            this.lProxyQueue_CNT.Size = new System.Drawing.Size(8, 20);
            this.lProxyQueue_CNT.TabIndex = 16;
            this.lProxyQueue_CNT.Text = "0";
            // 
            // lProxyQueue
            // 
            this.lProxyQueue.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyQueue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyQueue.LocalizationText = "ProxyModeForm.Buffer";
            this.lProxyQueue.Location = new System.Drawing.Point(162, 2);
            this.lProxyQueue.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyQueue.Name = "lProxyQueue";
            this.lProxyQueue.Size = new System.Drawing.Size(43, 20);
            this.lProxyQueue.TabIndex = 15;
            this.lProxyQueue.Text = "缓存区 :";
            // 
            // lProxyTotal_CNT
            // 
            this.lProxyTotal_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTotal_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTotal_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTotal_CNT.Location = new System.Drawing.Point(61, 2);
            this.lProxyTotal_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyTotal_CNT.Name = "lProxyTotal_CNT";
            this.lProxyTotal_CNT.Size = new System.Drawing.Size(8, 20);
            this.lProxyTotal_CNT.TabIndex = 12;
            this.lProxyTotal_CNT.Text = "0";
            // 
            // lsplit6
            // 
            this.lsplit6.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lsplit6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsplit6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsplit6.ForeColor = System.Drawing.Color.Silver;
            this.lsplit6.Location = new System.Drawing.Point(73, 2);
            this.lsplit6.Margin = new System.Windows.Forms.Padding(2);
            this.lsplit6.Name = "lsplit6";
            this.lsplit6.Size = new System.Drawing.Size(5, 20);
            this.lsplit6.TabIndex = 8;
            this.lsplit6.Text = "|";
            this.lsplit6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxyTotal
            // 
            this.lProxyTotal.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxyTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxyTotal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxyTotal.LocalizationText = "ProxyModeForm.TotalProxy";
            this.lProxyTotal.Location = new System.Drawing.Point(2, 2);
            this.lProxyTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lProxyTotal.Name = "lProxyTotal";
            this.lProxyTotal.Size = new System.Drawing.Size(55, 20);
            this.lProxyTotal.TabIndex = 5;
            this.lProxyTotal.Text = "代理总数 :";
            // 
            // tProxyList
            // 
            this.tProxyList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProxyList.Bordered = true;
            this.tProxyList.CellImpactHeight = false;
            this.tProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProxyList.EmptyHeader = true;
            this.tProxyList.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tProxyList.Gap = 8;
            this.tProxyList.GapCell = 0;
            this.tProxyList.Gaps = new System.Drawing.Size(8, 8);
            this.tProxyList.Location = new System.Drawing.Point(0, 24);
            this.tProxyList.Margin = new System.Windows.Forms.Padding(0);
            this.tProxyList.MultipleRows = true;
            this.tProxyList.Name = "tProxyList";
            this.tProxyList.Size = new System.Drawing.Size(1096, 426);
            this.tProxyList.TabIndex = 1;
            this.tProxyList.VirtualMode = true;
            this.tProxyList.CellClick += new AntdUI.Table.ClickEventHandler(this.tProxyList_CellClick);
            this.tProxyList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tProxyList_CellDoubleClick);
            this.tProxyList.SetRowStyle += new AntdUI.Table.SetRowStyleEventHandler(this.tProxyList_SetRowStyle);
            this.tProxyList.SelectIndexChanged += new System.EventHandler(this.tProxyList_SelectIndexChanged);
            // 
            // pPacketData
            // 
            this.pPacketData.BorderWidth = 1F;
            this.pPacketData.Controls.Add(this.splitterQuickList);
            this.pPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData.Location = new System.Drawing.Point(0, 0);
            this.pPacketData.Margin = new System.Windows.Forms.Padding(2);
            this.pPacketData.Name = "pPacketData";
            this.pPacketData.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pPacketData.Radius = 0;
            this.pPacketData.Size = new System.Drawing.Size(1096, 172);
            this.pPacketData.TabIndex = 0;
            // 
            // splitterQuickList
            // 
            this.splitterQuickList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterQuickList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterQuickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterQuickList.Location = new System.Drawing.Point(5, 6);
            this.splitterQuickList.Name = "splitterQuickList";
            this.splitterQuickList.Panel1MinSize = 0;
            // 
            // splitterQuickList.Panel2
            // 
            this.splitterQuickList.Panel2.Controls.Add(this.hbProxyData);
            this.splitterQuickList.Panel2MinSize = 0;
            this.splitterQuickList.Size = new System.Drawing.Size(1086, 160);
            this.splitterQuickList.SplitterDistance = 300;
            this.splitterQuickList.SplitterSize = 80;
            this.splitterQuickList.SplitterWidth = 5;
            this.splitterQuickList.TabIndex = 0;
            // 
            // hbProxyData
            // 
            this.hbProxyData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbProxyData.ColumnInfoVisible = true;
            this.hbProxyData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbProxyData.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbProxyData.LineInfoVisible = true;
            this.hbProxyData.Location = new System.Drawing.Point(0, 0);
            this.hbProxyData.Margin = new System.Windows.Forms.Padding(2);
            this.hbProxyData.Name = "hbProxyData";
            this.hbProxyData.ReadOnly = true;
            this.hbProxyData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbProxyData.Size = new System.Drawing.Size(781, 160);
            this.hbProxyData.StringViewVisible = true;
            this.hbProxyData.TabIndex = 2;
            this.hbProxyData.VScrollBarVisible = true;
            this.hbProxyData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hbProxyData_KeyDown);
            this.hbProxyData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbProxyData_MouseDown);
            // 
            // tlpProxyInfo
            // 
            this.tlpProxyInfo.ColumnCount = 3;
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpProxyInfo.Controls.Add(this.lTotalBytes, 2, 0);
            this.tlpProxyInfo.Controls.Add(this.lSplit1, 1, 0);
            this.tlpProxyInfo.Controls.Add(this.lProxySpeed, 0, 0);
            this.tlpProxyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpProxyInfo.Location = new System.Drawing.Point(0, 676);
            this.tlpProxyInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyInfo.Name = "tlpProxyInfo";
            this.tlpProxyInfo.RowCount = 1;
            this.tlpProxyInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyInfo.Size = new System.Drawing.Size(1100, 24);
            this.tlpProxyInfo.TabIndex = 4;
            // 
            // lTotalBytes
            // 
            this.lTotalBytes.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotalBytes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotalBytes.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotalBytes.Location = new System.Drawing.Point(83, 2);
            this.lTotalBytes.Margin = new System.Windows.Forms.Padding(2);
            this.lTotalBytes.Name = "lTotalBytes";
            this.lTotalBytes.Size = new System.Drawing.Size(60, 20);
            this.lTotalBytes.TabIndex = 11;
            this.lTotalBytes.Text = "TotalBytes";
            // 
            // lSplit1
            // 
            this.lSplit1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit1.ForeColor = System.Drawing.Color.Silver;
            this.lSplit1.Location = new System.Drawing.Point(74, 2);
            this.lSplit1.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit1.Name = "lSplit1";
            this.lSplit1.Size = new System.Drawing.Size(5, 20);
            this.lSplit1.TabIndex = 8;
            this.lSplit1.Text = "|";
            this.lSplit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lProxySpeed
            // 
            this.lProxySpeed.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProxySpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProxySpeed.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxySpeed.Location = new System.Drawing.Point(2, 2);
            this.lProxySpeed.Margin = new System.Windows.Forms.Padding(2);
            this.lProxySpeed.Name = "lProxySpeed";
            this.lProxySpeed.Size = new System.Drawing.Size(68, 20);
            this.lProxySpeed.TabIndex = 5;
            this.lProxySpeed.Text = "ProxySpeed";
            // 
            // bgwSearchProxyList
            // 
            this.bgwSearchProxyList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSearchProxyList_DoWork);
            this.bgwSearchProxyList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSearchProxyList_RunWorkerCompleted);
            // 
            // ProxyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpProxyList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProxyList";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.ProxyList_Load);
            this.tlpProxyList.ResumeLayout(false);
            this.tlpProxyList_Button.ResumeLayout(false);
            this.tlpProxyList_Button.PerformLayout();
            this.splitterProxyList.Panel1.ResumeLayout(false);
            this.splitterProxyList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterProxyList)).EndInit();
            this.splitterProxyList.ResumeLayout(false);
            this.tlpProxyList2.ResumeLayout(false);
            this.tlpPacketListInfo.ResumeLayout(false);
            this.tlpPacketListInfo.PerformLayout();
            this.pPacketData.ResumeLayout(false);
            this.splitterQuickList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterQuickList)).EndInit();
            this.splitterQuickList.ResumeLayout(false);
            this.tlpProxyInfo.ResumeLayout(false);
            this.tlpProxyInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpProxyList;
        private TableLayoutPanelEx tlpProxyList_Button;
        private AntdUI.Button bProxyList_Clear;
        private AntdUI.Button bProxyStop;
        private AntdUI.Button bProxyStart;
        private AntdUI.Splitter splitterProxyList;
        private TableLayoutPanelEx tlpProxyList2;
        private TableLayoutPanelEx tlpPacketListInfo;
        private AntdUI.Label lSplit16;
        private AntdUI.Label lFilterProxy_CNT;
        private AntdUI.Label lFilterProxy;
        private AntdUI.Label lSplit14;
        private AntdUI.Label lProxyUDP_CNT;
        private AntdUI.Label lProxyUDP;
        private AntdUI.Label lUDP_Resp_CNT;
        private AntdUI.Label lUDP_Resp;
        private AntdUI.Label lSplit5;
        private AntdUI.Label lUDP_Req_CNT;
        private AntdUI.Label lUDP_Req;
        private AntdUI.Label lSplit4;
        private AntdUI.Label lsplit2;
        private AntdUI.Label lTCP_Resp_CNT;
        private AntdUI.Label lTCP_Resp;
        private AntdUI.Label lSplit18;
        private AntdUI.Label lTCP_Req_CNT;
        private AntdUI.Label lTCP_Req;
        private AntdUI.Label lsplit7;
        private AntdUI.Label lFilterExecute_CNT;
        private AntdUI.Label lFilterExecute;
        private AntdUI.Label lProxyTCP_CNT;
        private AntdUI.Label lProxyTCP;
        private AntdUI.Label lsplit15;
        private AntdUI.Label lProxyAccount_CNT;
        private AntdUI.Label lProxyAccount;
        private AntdUI.Label lsplit8;
        private AntdUI.Label lProxyQueue_CNT;
        private AntdUI.Label lProxyQueue;
        private AntdUI.Label lProxyTotal_CNT;
        private AntdUI.Label lsplit6;
        private AntdUI.Label lProxyTotal;
        private AntdUI.Table tProxyList;
        private AntdUI.Panel pPacketData;
        private TableLayoutPanelEx tlpProxyInfo;
        private AntdUI.Label lTotalBytes;
        private AntdUI.Label lSplit1;
        private AntdUI.Label lProxySpeed;
        private System.ComponentModel.BackgroundWorker bgwSearchProxyList;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Button bSearchPacket;
        private AntdUI.Checkbox cbPacketList_AutoRoll;
        private AntdUI.Checkbox cbPacketList_AutoClear;
        private AntdUI.InputNumber txtPacketList_AutoClear;
        private AntdUI.Splitter splitterQuickList;
        private Be.Windows.Forms.HexBox hbProxyData;
    }
}

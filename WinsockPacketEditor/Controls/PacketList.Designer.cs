namespace WinsockPacketEditor
{
    partial class PacketList
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacketList));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpPacketList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tableLayoutPanel2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbTopMost = new AntdUI.Checkbox();
            this.txtPacketList_AutoClear = new AntdUI.InputNumber();
            this.cbPacketList_AutoClear = new AntdUI.Checkbox();
            this.cbPacketList_AutoRoll = new AntdUI.Checkbox();
            this.ddMenu = new AntdUI.Dropdown();
            this.bSearchPacket = new AntdUI.Button();
            this.bPacketList_Clear = new AntdUI.Button();
            this.bHookStop = new AntdUI.Button();
            this.bHookStart = new AntdUI.Button();
            this.tlpProcessInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lSpeedInfo = new AntdUI.Label();
            this.lSplit3 = new AntdUI.Label();
            this.lSplit2 = new AntdUI.Label();
            this.lSplit1 = new AntdUI.Label();
            this.lWinsockInfo = new AntdUI.Label();
            this.lModuleName = new AntdUI.Label();
            this.lProcessName = new AntdUI.Label();
            this.splitterPacketList = new AntdUI.Splitter();
            this.tlpPacketList2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dgvPacketList = new System.Windows.Forms.DataGridView();
            this.cTypeImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPacketTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPacketType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPacketSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFromImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.cPacketFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFromLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cToImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.cPacketTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cToLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPacketLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPacketData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpPacketListInfo = new WinsockPacketEditor.TableLayoutPanelEx();
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
            this.splitterQuickList = new AntdUI.Splitter();
            this.bgwSearchPacketList = new System.ComponentModel.BackgroundWorker();
            this.timerPacketList = new System.Windows.Forms.Timer(this.components);
            this.timerPacketListInfo = new System.Windows.Forms.Timer(this.components);
            this.tlpPacketList.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tlpProcessInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterPacketList)).BeginInit();
            this.splitterPacketList.Panel1.SuspendLayout();
            this.splitterPacketList.Panel2.SuspendLayout();
            this.splitterPacketList.SuspendLayout();
            this.tlpPacketList2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPacketList)).BeginInit();
            this.tlpPacketListInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterQuickList)).BeginInit();
            this.splitterQuickList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPacketList
            // 
            this.tlpPacketList.ColumnCount = 1;
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tlpPacketList.Controls.Add(this.tlpProcessInfo, 0, 2);
            this.tlpPacketList.Controls.Add(this.splitterPacketList, 0, 1);
            this.tlpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList.Name = "tlpPacketList";
            this.tlpPacketList.RowCount = 3;
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketList.Size = new System.Drawing.Size(1100, 700);
            this.tlpPacketList.TabIndex = 11;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 10;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.cbTopMost, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtPacketList_AutoClear, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbPacketList_AutoClear, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbPacketList_AutoRoll, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.ddMenu, 9, 0);
            this.tableLayoutPanel2.Controls.Add(this.bSearchPacket, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.bPacketList_Clear, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.bHookStop, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.bHookStart, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1100, 40);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // cbTopMost
            // 
            this.cbTopMost.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbTopMost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTopMost.LocalizationText = "TopMost";
            this.cbTopMost.Location = new System.Drawing.Point(451, 2);
            this.cbTopMost.Margin = new System.Windows.Forms.Padding(2);
            this.cbTopMost.Name = "cbTopMost";
            this.cbTopMost.Size = new System.Drawing.Size(104, 36);
            this.cbTopMost.TabIndex = 19;
            this.cbTopMost.Text = "窗口保持最前";
            this.cbTopMost.CheckedChanged += new AntdUI.BoolEventHandler(this.cbTopMost_CheckedChanged);
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
            this.txtPacketList_AutoClear.Size = new System.Drawing.Size(76, 36);
            this.txtPacketList_AutoClear.TabIndex = 16;
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
            this.cbPacketList_AutoClear.TabIndex = 15;
            this.cbPacketList_AutoClear.Text = "自动清理";
            this.cbPacketList_AutoClear.CheckedChanged += new AntdUI.BoolEventHandler(this.cbPacketList_AutoClear_CheckedChanged);
            // 
            // cbPacketList_AutoRoll
            // 
            this.cbPacketList_AutoRoll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbPacketList_AutoRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPacketList_AutoRoll.LocalizationText = "ListSettingsForm.AutoRoll";
            this.cbPacketList_AutoRoll.Location = new System.Drawing.Point(367, 2);
            this.cbPacketList_AutoRoll.Margin = new System.Windows.Forms.Padding(2);
            this.cbPacketList_AutoRoll.Name = "cbPacketList_AutoRoll";
            this.cbPacketList_AutoRoll.Size = new System.Drawing.Size(80, 36);
            this.cbPacketList_AutoRoll.TabIndex = 14;
            this.cbPacketList_AutoRoll.Text = "自动滚动";
            this.cbPacketList_AutoRoll.CheckedChanged += new AntdUI.BoolEventHandler(this.cbPacketList_AutoRoll_CheckedChanged);
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
            this.ddMenu.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu.Size = new System.Drawing.Size(32, 36);
            this.ddMenu.TabIndex = 13;
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
            this.bSearchPacket.TabIndex = 12;
            this.bSearchPacket.WaveSize = 0;
            this.bSearchPacket.Click += new System.EventHandler(this.bSearchPacket_Click);
            // 
            // bPacketList_Clear
            // 
            this.bPacketList_Clear.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bPacketList_Clear.BorderWidth = 1F;
            this.bPacketList_Clear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bPacketList_Clear.IconSvg = "DeleteOutlined";
            this.bPacketList_Clear.LocalizationText = "Clear";
            this.bPacketList_Clear.Location = new System.Drawing.Point(136, 2);
            this.bPacketList_Clear.Margin = new System.Windows.Forms.Padding(2);
            this.bPacketList_Clear.Name = "bPacketList_Clear";
            this.bPacketList_Clear.Size = new System.Drawing.Size(63, 36);
            this.bPacketList_Clear.TabIndex = 9;
            this.bPacketList_Clear.Text = "清空";
            this.bPacketList_Clear.Type = AntdUI.TTypeMini.Warn;
            this.bPacketList_Clear.Click += new System.EventHandler(this.bPacketList_Clear_Click);
            // 
            // bHookStop
            // 
            this.bHookStop.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bHookStop.BorderWidth = 1F;
            this.bHookStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bHookStop.Enabled = false;
            this.bHookStop.IconSvg = "PauseCircleOutlined";
            this.bHookStop.LocalizationText = "Stop";
            this.bHookStop.Location = new System.Drawing.Point(69, 2);
            this.bHookStop.Margin = new System.Windows.Forms.Padding(2);
            this.bHookStop.Name = "bHookStop";
            this.bHookStop.Size = new System.Drawing.Size(63, 36);
            this.bHookStop.TabIndex = 8;
            this.bHookStop.Text = "停止";
            this.bHookStop.Type = AntdUI.TTypeMini.Error;
            this.bHookStop.Click += new System.EventHandler(this.bHookStop_Click);
            // 
            // bHookStart
            // 
            this.bHookStart.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bHookStart.BorderWidth = 1F;
            this.bHookStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bHookStart.IconSvg = "PlayCircleOutlined";
            this.bHookStart.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bHookStart.LoadingWaveCount = 6;
            this.bHookStart.LoadingWaveSize = 6;
            this.bHookStart.LoadingWaveValue = 0.6F;
            this.bHookStart.LoadingWaveVertical = true;
            this.bHookStart.LocalizationText = "Start";
            this.bHookStart.Location = new System.Drawing.Point(2, 2);
            this.bHookStart.Margin = new System.Windows.Forms.Padding(2);
            this.bHookStart.Name = "bHookStart";
            this.bHookStart.Size = new System.Drawing.Size(63, 36);
            this.bHookStart.TabIndex = 7;
            this.bHookStart.Text = "开始";
            this.bHookStart.Type = AntdUI.TTypeMini.Info;
            this.bHookStart.Click += new System.EventHandler(this.bHookStart_Click);
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
            this.tlpProcessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpProcessInfo.Controls.Add(this.lSpeedInfo, 6, 0);
            this.tlpProcessInfo.Controls.Add(this.lSplit3, 5, 0);
            this.tlpProcessInfo.Controls.Add(this.lSplit2, 3, 0);
            this.tlpProcessInfo.Controls.Add(this.lSplit1, 1, 0);
            this.tlpProcessInfo.Controls.Add(this.lWinsockInfo, 4, 0);
            this.tlpProcessInfo.Controls.Add(this.lModuleName, 2, 0);
            this.tlpProcessInfo.Controls.Add(this.lProcessName, 0, 0);
            this.tlpProcessInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProcessInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpProcessInfo.Location = new System.Drawing.Point(0, 676);
            this.tlpProcessInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProcessInfo.Name = "tlpProcessInfo";
            this.tlpProcessInfo.RowCount = 1;
            this.tlpProcessInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessInfo.Size = new System.Drawing.Size(1100, 24);
            this.tlpProcessInfo.TabIndex = 4;
            // 
            // lSpeedInfo
            // 
            this.lSpeedInfo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSpeedInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSpeedInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSpeedInfo.Location = new System.Drawing.Point(270, 2);
            this.lSpeedInfo.Margin = new System.Windows.Forms.Padding(2);
            this.lSpeedInfo.Name = "lSpeedInfo";
            this.lSpeedInfo.Size = new System.Drawing.Size(59, 20);
            this.lSpeedInfo.TabIndex = 11;
            this.lSpeedInfo.Text = "SpeedInfo";
            // 
            // lSplit3
            // 
            this.lSplit3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit3.ForeColor = System.Drawing.Color.Silver;
            this.lSplit3.Location = new System.Drawing.Point(261, 2);
            this.lSplit3.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit3.Name = "lSplit3";
            this.lSplit3.Size = new System.Drawing.Size(5, 20);
            this.lSplit3.TabIndex = 10;
            this.lSplit3.Text = "|";
            this.lSplit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSplit2
            // 
            this.lSplit2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit2.ForeColor = System.Drawing.Color.Silver;
            this.lSplit2.Location = new System.Drawing.Point(176, 2);
            this.lSplit2.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit2.Name = "lSplit2";
            this.lSplit2.Size = new System.Drawing.Size(5, 20);
            this.lSplit2.TabIndex = 9;
            this.lSplit2.Text = "|";
            this.lSplit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSplit1
            // 
            this.lSplit1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit1.ForeColor = System.Drawing.Color.Silver;
            this.lSplit1.Location = new System.Drawing.Point(84, 2);
            this.lSplit1.Margin = new System.Windows.Forms.Padding(2);
            this.lSplit1.Name = "lSplit1";
            this.lSplit1.Size = new System.Drawing.Size(5, 20);
            this.lSplit1.TabIndex = 8;
            this.lSplit1.Text = "|";
            this.lSplit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWinsockInfo
            // 
            this.lWinsockInfo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWinsockInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWinsockInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWinsockInfo.Location = new System.Drawing.Point(185, 2);
            this.lWinsockInfo.Margin = new System.Windows.Forms.Padding(2);
            this.lWinsockInfo.Name = "lWinsockInfo";
            this.lWinsockInfo.Size = new System.Drawing.Size(72, 20);
            this.lWinsockInfo.TabIndex = 7;
            this.lWinsockInfo.Text = "WinsockInfo";
            // 
            // lModuleName
            // 
            this.lModuleName.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lModuleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lModuleName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lModuleName.Location = new System.Drawing.Point(93, 2);
            this.lModuleName.Margin = new System.Windows.Forms.Padding(2);
            this.lModuleName.Name = "lModuleName";
            this.lModuleName.Size = new System.Drawing.Size(79, 20);
            this.lModuleName.TabIndex = 6;
            this.lModuleName.Text = "ModuleName";
            // 
            // lProcessName
            // 
            this.lProcessName.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProcessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProcessName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProcessName.Location = new System.Drawing.Point(2, 2);
            this.lProcessName.Margin = new System.Windows.Forms.Padding(2);
            this.lProcessName.Name = "lProcessName";
            this.lProcessName.Size = new System.Drawing.Size(78, 20);
            this.lProcessName.TabIndex = 5;
            this.lProcessName.Text = "ProcessName";
            // 
            // splitterPacketList
            // 
            this.splitterPacketList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterPacketList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterPacketList.Location = new System.Drawing.Point(3, 43);
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
            this.splitterPacketList.Panel2.Controls.Add(this.splitterQuickList);
            this.splitterPacketList.Panel2MinSize = 0;
            this.splitterPacketList.Size = new System.Drawing.Size(1094, 630);
            this.splitterPacketList.SplitterDistance = 450;
            this.splitterPacketList.SplitterSize = 80;
            this.splitterPacketList.SplitterWidth = 5;
            this.splitterPacketList.TabIndex = 7;
            // 
            // tlpPacketList2
            // 
            this.tlpPacketList2.ColumnCount = 1;
            this.tlpPacketList2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList2.Controls.Add(this.dgvPacketList, 0, 1);
            this.tlpPacketList2.Controls.Add(this.tlpPacketListInfo, 0, 0);
            this.tlpPacketList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList2.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketList2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList2.Name = "tlpPacketList2";
            this.tlpPacketList2.RowCount = 2;
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList2.Size = new System.Drawing.Size(1094, 450);
            this.tlpPacketList2.TabIndex = 1;
            // 
            // dgvPacketList
            // 
            this.dgvPacketList.AllowUserToAddRows = false;
            this.dgvPacketList.AllowUserToDeleteRows = false;
            this.dgvPacketList.AllowUserToResizeColumns = false;
            this.dgvPacketList.AllowUserToResizeRows = false;
            this.dgvPacketList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPacketList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPacketList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPacketList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPacketList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPacketList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPacketList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPacketList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTypeImg,
            this.cID,
            this.cPacketTime,
            this.cPacketType,
            this.cPacketSocket,
            this.cFromImg,
            this.cPacketFrom,
            this.cFromLocation,
            this.cToImg,
            this.cPacketTo,
            this.cToLocation,
            this.cPacketLen,
            this.cPacketData});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPacketList.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPacketList.EnableHeadersVisualStyles = false;
            this.dgvPacketList.Location = new System.Drawing.Point(0, 24);
            this.dgvPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.dgvPacketList.Name = "dgvPacketList";
            this.dgvPacketList.ReadOnly = true;
            this.dgvPacketList.RowHeadersVisible = false;
            this.dgvPacketList.RowTemplate.Height = 23;
            this.dgvPacketList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPacketList.Size = new System.Drawing.Size(1094, 426);
            this.dgvPacketList.TabIndex = 8;
            this.dgvPacketList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPacketList_CellDoubleClick);
            this.dgvPacketList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPacketList_CellFormatting);
            this.dgvPacketList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvPacketList_DataError);
            this.dgvPacketList.SelectionChanged += new System.EventHandler(this.dgvPacketList_SelectionChanged);
            this.dgvPacketList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvPacketList_MouseClick);
            // 
            // cTypeImg
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cTypeImg.DefaultCellStyle = dataGridViewCellStyle2;
            this.cTypeImg.HeaderText = "";
            this.cTypeImg.Name = "cTypeImg";
            this.cTypeImg.ReadOnly = true;
            this.cTypeImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cTypeImg.Width = 7;
            // 
            // cID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.DefaultCellStyle = dataGridViewCellStyle3;
            this.cID.HeaderText = "序号";
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cID.Width = 42;
            // 
            // cPacketTime
            // 
            this.cPacketTime.DataPropertyName = "PacketTime";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketTime.DefaultCellStyle = dataGridViewCellStyle4;
            this.cPacketTime.HeaderText = "时间戳";
            this.cPacketTime.Name = "cPacketTime";
            this.cPacketTime.ReadOnly = true;
            this.cPacketTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPacketTime.Width = 54;
            // 
            // cPacketType
            // 
            this.cPacketType.DataPropertyName = "PacketType";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketType.DefaultCellStyle = dataGridViewCellStyle5;
            this.cPacketType.HeaderText = "类别";
            this.cPacketType.Name = "cPacketType";
            this.cPacketType.ReadOnly = true;
            this.cPacketType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPacketType.Width = 42;
            // 
            // cPacketSocket
            // 
            this.cPacketSocket.DataPropertyName = "PacketSocket";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketSocket.DefaultCellStyle = dataGridViewCellStyle6;
            this.cPacketSocket.HeaderText = "套接字";
            this.cPacketSocket.Name = "cPacketSocket";
            this.cPacketSocket.ReadOnly = true;
            this.cPacketSocket.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPacketSocket.Width = 54;
            // 
            // cFromImg
            // 
            this.cFromImg.HeaderText = "";
            this.cFromImg.Name = "cFromImg";
            this.cFromImg.ReadOnly = true;
            this.cFromImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cFromImg.Width = 7;
            // 
            // cPacketFrom
            // 
            this.cPacketFrom.DataPropertyName = "PacketFrom";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketFrom.DefaultCellStyle = dataGridViewCellStyle7;
            this.cPacketFrom.HeaderText = "本机地址";
            this.cPacketFrom.Name = "cPacketFrom";
            this.cPacketFrom.ReadOnly = true;
            this.cPacketFrom.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPacketFrom.Width = 66;
            // 
            // cFromLocation
            // 
            this.cFromLocation.DataPropertyName = "FromLocation";
            this.cFromLocation.HeaderText = "所属地";
            this.cFromLocation.Name = "cFromLocation";
            this.cFromLocation.ReadOnly = true;
            this.cFromLocation.Width = 73;
            // 
            // cToImg
            // 
            this.cToImg.HeaderText = "";
            this.cToImg.Name = "cToImg";
            this.cToImg.ReadOnly = true;
            this.cToImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cToImg.Width = 7;
            // 
            // cPacketTo
            // 
            this.cPacketTo.DataPropertyName = "PacketTo";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketTo.DefaultCellStyle = dataGridViewCellStyle8;
            this.cPacketTo.HeaderText = "远端地址";
            this.cPacketTo.Name = "cPacketTo";
            this.cPacketTo.ReadOnly = true;
            this.cPacketTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPacketTo.Width = 66;
            // 
            // cToLocation
            // 
            this.cToLocation.DataPropertyName = "ToLocation";
            this.cToLocation.HeaderText = "所属地";
            this.cToLocation.Name = "cToLocation";
            this.cToLocation.ReadOnly = true;
            this.cToLocation.Width = 73;
            // 
            // cPacketLen
            // 
            this.cPacketLen.DataPropertyName = "PacketLen";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketLen.DefaultCellStyle = dataGridViewCellStyle9;
            this.cPacketLen.HeaderText = "长度";
            this.cPacketLen.Name = "cPacketLen";
            this.cPacketLen.ReadOnly = true;
            this.cPacketLen.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPacketLen.Width = 42;
            // 
            // cPacketData
            // 
            this.cPacketData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cPacketData.DataPropertyName = "PacketData";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketData.DefaultCellStyle = dataGridViewCellStyle10;
            this.cPacketData.HeaderText = "数据";
            this.cPacketData.Name = "cPacketData";
            this.cPacketData.ReadOnly = true;
            this.cPacketData.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPacketData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.tlpPacketListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpPacketListInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketListInfo.Name = "tlpPacketListInfo";
            this.tlpPacketListInfo.RowCount = 3;
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpPacketListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketListInfo.Size = new System.Drawing.Size(1094, 24);
            this.tlpPacketListInfo.TabIndex = 6;
            // 
            // lWSARecvFrom_CNT
            // 
            this.lWSARecvFrom_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecvFrom_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecvFrom_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecvFrom_CNT.Location = new System.Drawing.Point(885, 2);
            this.lWSARecvFrom_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lWSARecvFrom_CNT.Name = "lWSARecvFrom_CNT";
            this.lWSARecvFrom_CNT.Size = new System.Drawing.Size(8, 20);
            this.lWSARecvFrom_CNT.TabIndex = 40;
            this.lWSARecvFrom_CNT.Text = "0";
            // 
            // lWSARecvFrom
            // 
            this.lWSARecvFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecvFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecvFrom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecvFrom.LocalizationText = "InjectModeForm.WSARecvFrom";
            this.lWSARecvFrom.Location = new System.Drawing.Point(807, 2);
            this.lWSARecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.lWSARecvFrom.Name = "lWSARecvFrom";
            this.lWSARecvFrom.Size = new System.Drawing.Size(74, 20);
            this.lWSARecvFrom.TabIndex = 39;
            this.lWSARecvFrom.Text = "WSA 接收自 :";
            // 
            // label33
            // 
            this.label33.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label33.ForeColor = System.Drawing.Color.Silver;
            this.label33.Location = new System.Drawing.Point(798, 2);
            this.label33.Margin = new System.Windows.Forms.Padding(2);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(5, 20);
            this.label33.TabIndex = 38;
            this.label33.Text = "|";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWSASendTo_CNT
            // 
            this.lWSASendTo_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASendTo_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASendTo_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASendTo_CNT.Location = new System.Drawing.Point(786, 2);
            this.lWSASendTo_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lWSASendTo_CNT.Name = "lWSASendTo_CNT";
            this.lWSASendTo_CNT.Size = new System.Drawing.Size(8, 20);
            this.lWSASendTo_CNT.TabIndex = 37;
            this.lWSASendTo_CNT.Text = "0";
            // 
            // lWSASendTo
            // 
            this.lWSASendTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASendTo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASendTo.LocalizationText = "InjectModeForm.WSASendTo";
            this.lWSASendTo.Location = new System.Drawing.Point(708, 2);
            this.lWSASendTo.Margin = new System.Windows.Forms.Padding(2);
            this.lWSASendTo.Name = "lWSASendTo";
            this.lWSASendTo.Size = new System.Drawing.Size(74, 20);
            this.lWSASendTo.TabIndex = 36;
            this.lWSASendTo.Text = "WSA 发送到 :";
            // 
            // label30
            // 
            this.label30.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label30.ForeColor = System.Drawing.Color.Silver;
            this.label30.Location = new System.Drawing.Point(699, 2);
            this.label30.Margin = new System.Windows.Forms.Padding(2);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(5, 20);
            this.label30.TabIndex = 35;
            this.label30.Text = "|";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWSARecv_CNT
            // 
            this.lWSARecv_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecv_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecv_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecv_CNT.Location = new System.Drawing.Point(687, 2);
            this.lWSARecv_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lWSARecv_CNT.Name = "lWSARecv_CNT";
            this.lWSARecv_CNT.Size = new System.Drawing.Size(8, 20);
            this.lWSARecv_CNT.TabIndex = 34;
            this.lWSARecv_CNT.Text = "0";
            // 
            // lWSARecv
            // 
            this.lWSARecv.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSARecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSARecv.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSARecv.LocalizationText = "InjectModeForm.WSARecv";
            this.lWSARecv.Location = new System.Drawing.Point(621, 2);
            this.lWSARecv.Margin = new System.Windows.Forms.Padding(2);
            this.lWSARecv.Name = "lWSARecv";
            this.lWSARecv.Size = new System.Drawing.Size(62, 20);
            this.lWSARecv.TabIndex = 33;
            this.lWSARecv.Text = "WSA 接收 :";
            // 
            // label27
            // 
            this.label27.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.ForeColor = System.Drawing.Color.Silver;
            this.label27.Location = new System.Drawing.Point(612, 2);
            this.label27.Margin = new System.Windows.Forms.Padding(2);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(5, 20);
            this.label27.TabIndex = 32;
            this.label27.Text = "|";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lWSASend_CNT
            // 
            this.lWSASend_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASend_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASend_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASend_CNT.Location = new System.Drawing.Point(600, 2);
            this.lWSASend_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lWSASend_CNT.Name = "lWSASend_CNT";
            this.lWSASend_CNT.Size = new System.Drawing.Size(8, 20);
            this.lWSASend_CNT.TabIndex = 31;
            this.lWSASend_CNT.Text = "0";
            // 
            // lWSASend
            // 
            this.lWSASend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWSASend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWSASend.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWSASend.LocalizationText = "InjectModeForm.WSASend";
            this.lWSASend.Location = new System.Drawing.Point(534, 2);
            this.lWSASend.Margin = new System.Windows.Forms.Padding(2);
            this.lWSASend.Name = "lWSASend";
            this.lWSASend.Size = new System.Drawing.Size(62, 20);
            this.lWSASend.TabIndex = 30;
            this.lWSASend.Text = "WSA 发送 :";
            // 
            // label24
            // 
            this.label24.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.ForeColor = System.Drawing.Color.Silver;
            this.label24.Location = new System.Drawing.Point(525, 2);
            this.label24.Margin = new System.Windows.Forms.Padding(2);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(5, 20);
            this.label24.TabIndex = 29;
            this.label24.Text = "|";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRecvFrom_CNT
            // 
            this.lRecvFrom_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecvFrom_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecvFrom_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecvFrom_CNT.Location = new System.Drawing.Point(513, 2);
            this.lRecvFrom_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lRecvFrom_CNT.Name = "lRecvFrom_CNT";
            this.lRecvFrom_CNT.Size = new System.Drawing.Size(8, 20);
            this.lRecvFrom_CNT.TabIndex = 28;
            this.lRecvFrom_CNT.Text = "0";
            // 
            // lRecvFrom
            // 
            this.lRecvFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecvFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecvFrom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecvFrom.LocalizationText = "InjectModeForm.RecvFrom";
            this.lRecvFrom.Location = new System.Drawing.Point(466, 2);
            this.lRecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.lRecvFrom.Name = "lRecvFrom";
            this.lRecvFrom.Size = new System.Drawing.Size(43, 20);
            this.lRecvFrom.TabIndex = 27;
            this.lRecvFrom.Text = "接收自 :";
            // 
            // label21
            // 
            this.label21.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.ForeColor = System.Drawing.Color.Silver;
            this.label21.Location = new System.Drawing.Point(457, 2);
            this.label21.Margin = new System.Windows.Forms.Padding(2);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(5, 20);
            this.label21.TabIndex = 26;
            this.label21.Text = "|";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSendTo_CNT
            // 
            this.lSendTo_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSendTo_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSendTo_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSendTo_CNT.Location = new System.Drawing.Point(445, 2);
            this.lSendTo_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lSendTo_CNT.Name = "lSendTo_CNT";
            this.lSendTo_CNT.Size = new System.Drawing.Size(8, 20);
            this.lSendTo_CNT.TabIndex = 25;
            this.lSendTo_CNT.Text = "0";
            // 
            // lSendTo
            // 
            this.lSendTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSendTo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSendTo.LocalizationText = "InjectModeForm.SendTo";
            this.lSendTo.Location = new System.Drawing.Point(398, 2);
            this.lSendTo.Margin = new System.Windows.Forms.Padding(2);
            this.lSendTo.Name = "lSendTo";
            this.lSendTo.Size = new System.Drawing.Size(43, 20);
            this.lSendTo.TabIndex = 24;
            this.lSendTo.Text = "发送到 :";
            // 
            // label18
            // 
            this.label18.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.ForeColor = System.Drawing.Color.Silver;
            this.label18.Location = new System.Drawing.Point(389, 2);
            this.label18.Margin = new System.Windows.Forms.Padding(2);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(5, 20);
            this.label18.TabIndex = 23;
            this.label18.Text = "|";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRecv_CNT
            // 
            this.lRecv_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecv_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecv_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecv_CNT.Location = new System.Drawing.Point(377, 2);
            this.lRecv_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lRecv_CNT.Name = "lRecv_CNT";
            this.lRecv_CNT.Size = new System.Drawing.Size(8, 20);
            this.lRecv_CNT.TabIndex = 22;
            this.lRecv_CNT.Text = "0";
            // 
            // lRecv
            // 
            this.lRecv.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRecv.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRecv.LocalizationText = "InjectModeForm.Recv";
            this.lRecv.Location = new System.Drawing.Point(342, 2);
            this.lRecv.Margin = new System.Windows.Forms.Padding(2);
            this.lRecv.Name = "lRecv";
            this.lRecv.Size = new System.Drawing.Size(31, 20);
            this.lRecv.TabIndex = 21;
            this.lRecv.Text = "接收 :";
            // 
            // label15
            // 
            this.label15.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Silver;
            this.label15.Location = new System.Drawing.Point(333, 2);
            this.label15.Margin = new System.Windows.Forms.Padding(2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(5, 20);
            this.label15.TabIndex = 20;
            this.label15.Text = "|";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSend_CNT
            // 
            this.lSend_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_CNT.Location = new System.Drawing.Point(321, 2);
            this.lSend_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lSend_CNT.Name = "lSend_CNT";
            this.lSend_CNT.Size = new System.Drawing.Size(8, 20);
            this.lSend_CNT.TabIndex = 19;
            this.lSend_CNT.Text = "0";
            // 
            // lSend
            // 
            this.lSend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend.LocalizationText = "InjectModeForm.Send";
            this.lSend.Location = new System.Drawing.Point(286, 2);
            this.lSend.Margin = new System.Windows.Forms.Padding(2);
            this.lSend.Name = "lSend";
            this.lSend.Size = new System.Drawing.Size(31, 20);
            this.lSend.TabIndex = 18;
            this.lSend.Text = "发送 :";
            // 
            // label12
            // 
            this.label12.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Silver;
            this.label12.Location = new System.Drawing.Point(277, 2);
            this.label12.Margin = new System.Windows.Forms.Padding(2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(5, 20);
            this.label12.TabIndex = 17;
            this.label12.Text = "|";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFilterPacket_CNT
            // 
            this.lFilterPacket_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterPacket_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterPacket_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterPacket_CNT.Location = new System.Drawing.Point(265, 2);
            this.lFilterPacket_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterPacket_CNT.Name = "lFilterPacket_CNT";
            this.lFilterPacket_CNT.Size = new System.Drawing.Size(8, 20);
            this.lFilterPacket_CNT.TabIndex = 16;
            this.lFilterPacket_CNT.Text = "0";
            // 
            // lFilterPacket
            // 
            this.lFilterPacket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterPacket.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterPacket.LocalizationText = "InjectModeForm.Filter";
            this.lFilterPacket.Location = new System.Drawing.Point(230, 2);
            this.lFilterPacket.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterPacket.Name = "lFilterPacket";
            this.lFilterPacket.Size = new System.Drawing.Size(31, 20);
            this.lFilterPacket.TabIndex = 15;
            this.lFilterPacket.Text = "过滤 :";
            // 
            // lQueue_CNT
            // 
            this.lQueue_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lQueue_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lQueue_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lQueue_CNT.Location = new System.Drawing.Point(209, 2);
            this.lQueue_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lQueue_CNT.Name = "lQueue_CNT";
            this.lQueue_CNT.Size = new System.Drawing.Size(8, 20);
            this.lQueue_CNT.TabIndex = 14;
            this.lQueue_CNT.Text = "0";
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
            this.lFilterExecute_CNT.TabIndex = 13;
            this.lFilterExecute_CNT.Text = "0";
            // 
            // lTotal_CNT
            // 
            this.lTotal_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_CNT.Location = new System.Drawing.Point(61, 2);
            this.lTotal_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lTotal_CNT.Name = "lTotal_CNT";
            this.lTotal_CNT.Size = new System.Drawing.Size(8, 20);
            this.lTotal_CNT.TabIndex = 12;
            this.lTotal_CNT.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(221, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(5, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "|";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(153, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(5, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "|";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(73, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(5, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "|";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lQueue
            // 
            this.lQueue.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lQueue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lQueue.LocalizationText = "InjectModeForm.Buffer";
            this.lQueue.Location = new System.Drawing.Point(162, 2);
            this.lQueue.Margin = new System.Windows.Forms.Padding(2);
            this.lQueue.Name = "lQueue";
            this.lQueue.Size = new System.Drawing.Size(43, 20);
            this.lQueue.TabIndex = 7;
            this.lQueue.Text = "缓存区 :";
            // 
            // lFilterExecute
            // 
            this.lFilterExecute.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFilterExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFilterExecute.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFilterExecute.LocalizationText = "InjectModeForm.ExecuteFilter";
            this.lFilterExecute.Location = new System.Drawing.Point(82, 2);
            this.lFilterExecute.Margin = new System.Windows.Forms.Padding(2);
            this.lFilterExecute.Name = "lFilterExecute";
            this.lFilterExecute.Size = new System.Drawing.Size(55, 20);
            this.lFilterExecute.TabIndex = 6;
            this.lFilterExecute.Text = "滤镜执行 :";
            // 
            // lTotal
            // 
            this.lTotal.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal.LocalizationText = "InjectModeForm.TotalPackets";
            this.lTotal.Location = new System.Drawing.Point(2, 2);
            this.lTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lTotal.Name = "lTotal";
            this.lTotal.Size = new System.Drawing.Size(55, 20);
            this.lTotal.TabIndex = 5;
            this.lTotal.Text = "封包总数 :";
            // 
            // splitterQuickList
            // 
            this.splitterQuickList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterQuickList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterQuickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterQuickList.Location = new System.Drawing.Point(0, 0);
            this.splitterQuickList.Name = "splitterQuickList";
            this.splitterQuickList.Panel1MinSize = 0;
            this.splitterQuickList.Panel2MinSize = 0;
            this.splitterQuickList.Size = new System.Drawing.Size(1094, 175);
            this.splitterQuickList.SplitterDistance = 300;
            this.splitterQuickList.SplitterSize = 80;
            this.splitterQuickList.SplitterWidth = 5;
            this.splitterQuickList.TabIndex = 2;
            // 
            // bgwSearchPacketList
            // 
            this.bgwSearchPacketList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSearchPacketList_DoWork);
            this.bgwSearchPacketList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSearchPacketList_RunWorkerCompleted);
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
            // PacketList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpPacketList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PacketList";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.PacketList_Load);
            this.tlpPacketList.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tlpProcessInfo.ResumeLayout(false);
            this.tlpProcessInfo.PerformLayout();
            this.splitterPacketList.Panel1.ResumeLayout(false);
            this.splitterPacketList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterPacketList)).EndInit();
            this.splitterPacketList.ResumeLayout(false);
            this.tlpPacketList2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPacketList)).EndInit();
            this.tlpPacketListInfo.ResumeLayout(false);
            this.tlpPacketListInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterQuickList)).EndInit();
            this.splitterQuickList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpPacketList;
        private TableLayoutPanelEx tableLayoutPanel2;
        private AntdUI.Button bPacketList_Clear;
        private AntdUI.Button bHookStop;
        private AntdUI.Button bHookStart;
        private TableLayoutPanelEx tlpProcessInfo;
        private AntdUI.Label lSpeedInfo;
        private AntdUI.Label lSplit3;
        private AntdUI.Label lSplit2;
        private AntdUI.Label lSplit1;
        private AntdUI.Label lWinsockInfo;
        private AntdUI.Label lModuleName;
        private AntdUI.Label lProcessName;
        private System.ComponentModel.BackgroundWorker bgwSearchPacketList;
        private AntdUI.Button bSearchPacket;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Checkbox cbPacketList_AutoRoll;
        private AntdUI.Checkbox cbPacketList_AutoClear;
        private AntdUI.InputNumber txtPacketList_AutoClear;
        private AntdUI.Splitter splitterPacketList;
        private TableLayoutPanelEx tlpPacketList2;
        private TableLayoutPanelEx tlpPacketListInfo;
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
        private AntdUI.Splitter splitterQuickList;
        private System.Windows.Forms.DataGridView dgvPacketList;
        private System.Windows.Forms.DataGridViewImageColumn cTypeImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketSocket;
        private System.Windows.Forms.DataGridViewImageColumn cFromImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFromLocation;
        private System.Windows.Forms.DataGridViewImageColumn cToImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cToLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketData;
        private AntdUI.Checkbox cbTopMost;
        private System.Windows.Forms.Timer timerPacketList;
        private System.Windows.Forms.Timer timerPacketListInfo;
    }
}

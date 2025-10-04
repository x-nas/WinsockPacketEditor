namespace WinsockPacketEditor
{
    partial class QuickList
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
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            this.tlpQuickList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tabQuickList = new AntdUI.Tabs();
            this.tpFilterList = new AntdUI.TabPage();
            this.tlpFilterList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tFilterList = new AntdUI.Table();
            this.tlpFilterListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bFilterList_ResetCount = new AntdUI.Button();
            this.bFilterList_DisableAll = new AntdUI.Button();
            this.bFilterList_EnableAll = new AntdUI.Button();
            this.bFilterList_Add = new AntdUI.Button();
            this.bFilterList_Delete = new AntdUI.Button();
            this.tpRobotList = new AntdUI.TabPage();
            this.tlpRobotList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tRobotList = new AntdUI.Table();
            this.tlpRobotListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bRobotList_Stop = new AntdUI.Button();
            this.bRobotList_Execute = new AntdUI.Button();
            this.bRobotList_ResetCount = new AntdUI.Button();
            this.bRobotList_DisableAll = new AntdUI.Button();
            this.bRobotList_EnableAll = new AntdUI.Button();
            this.bRobotList_Add = new AntdUI.Button();
            this.bRobotList_Delete = new AntdUI.Button();
            this.tpSendList = new AntdUI.TabPage();
            this.tlpSendList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tSendList = new AntdUI.Table();
            this.tlpSendListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSendList_Stop = new AntdUI.Button();
            this.bSendList_Execute = new AntdUI.Button();
            this.bSendList_ResetCount = new AntdUI.Button();
            this.bSendList_DisableAll = new AntdUI.Button();
            this.bSendList_EnableAll = new AntdUI.Button();
            this.bSendList_Add = new AntdUI.Button();
            this.bSendList_Delete = new AntdUI.Button();
            this.tlpQuickList.SuspendLayout();
            this.tabQuickList.SuspendLayout();
            this.tpFilterList.SuspendLayout();
            this.tlpFilterList.SuspendLayout();
            this.tlpFilterListButton.SuspendLayout();
            this.tpRobotList.SuspendLayout();
            this.tlpRobotList.SuspendLayout();
            this.tlpRobotListButton.SuspendLayout();
            this.tpSendList.SuspendLayout();
            this.tlpSendList.SuspendLayout();
            this.tlpSendListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpQuickList
            // 
            this.tlpQuickList.ColumnCount = 1;
            this.tlpQuickList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpQuickList.Controls.Add(this.tabQuickList, 0, 0);
            this.tlpQuickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpQuickList.Location = new System.Drawing.Point(0, 0);
            this.tlpQuickList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpQuickList.Name = "tlpQuickList";
            this.tlpQuickList.RowCount = 1;
            this.tlpQuickList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpQuickList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpQuickList.Size = new System.Drawing.Size(500, 500);
            this.tlpQuickList.TabIndex = 0;
            // 
            // tabQuickList
            // 
            this.tabQuickList.Controls.Add(this.tpRobotList);
            this.tabQuickList.Controls.Add(this.tpSendList);
            this.tabQuickList.Controls.Add(this.tpFilterList);
            this.tabQuickList.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabQuickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabQuickList.Gap = 10;
            this.tabQuickList.Location = new System.Drawing.Point(0, 0);
            this.tabQuickList.Margin = new System.Windows.Forms.Padding(0);
            this.tabQuickList.Name = "tabQuickList";
            this.tabQuickList.Pages.Add(this.tpFilterList);
            this.tabQuickList.Pages.Add(this.tpSendList);
            this.tabQuickList.Pages.Add(this.tpRobotList);
            this.tabQuickList.SelectedIndex = 2;
            this.tabQuickList.Size = new System.Drawing.Size(500, 500);
            this.tabQuickList.Style = styleCard1;
            this.tabQuickList.TabIndex = 0;
            this.tabQuickList.Type = AntdUI.TabType.Card;
            // 
            // tpFilterList
            // 
            this.tpFilterList.Controls.Add(this.tlpFilterList);
            this.tpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterList.LocalizationText = "Filter";
            this.tpFilterList.Location = new System.Drawing.Point(0, 26);
            this.tpFilterList.Name = "tpFilterList";
            this.tpFilterList.Size = new System.Drawing.Size(500, 474);
            this.tpFilterList.TabIndex = 0;
            this.tpFilterList.Text = "滤镜";
            // 
            // tlpFilterList
            // 
            this.tlpFilterList.ColumnCount = 1;
            this.tlpFilterList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.Controls.Add(this.tFilterList, 0, 1);
            this.tlpFilterList.Controls.Add(this.tlpFilterListButton, 0, 0);
            this.tlpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterList.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterList.Name = "tlpFilterList";
            this.tlpFilterList.RowCount = 2;
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.Size = new System.Drawing.Size(500, 474);
            this.tlpFilterList.TabIndex = 0;
            // 
            // tFilterList
            // 
            this.tFilterList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tFilterList.CellImpactHeight = false;
            this.tFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterList.Gap = 8;
            this.tFilterList.GapCell = 5;
            this.tFilterList.Gaps = new System.Drawing.Size(8, 8);
            this.tFilterList.Location = new System.Drawing.Point(0, 35);
            this.tFilterList.Margin = new System.Windows.Forms.Padding(0);
            this.tFilterList.MultipleRows = true;
            this.tFilterList.Name = "tFilterList";
            this.tFilterList.Size = new System.Drawing.Size(500, 439);
            this.tFilterList.SwitchSize = 12;
            this.tFilterList.TabIndex = 1;
            this.tFilterList.VisibleHeader = false;
            this.tFilterList.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellClick);
            this.tFilterList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellDoubleClick);
            // 
            // tlpFilterListButton
            // 
            this.tlpFilterListButton.ColumnCount = 5;
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterListButton.Controls.Add(this.bFilterList_ResetCount, 2, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_DisableAll, 1, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_EnableAll, 0, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_Add, 3, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_Delete, 4, 0);
            this.tlpFilterListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterListButton.Name = "tlpFilterListButton";
            this.tlpFilterListButton.RowCount = 1;
            this.tlpFilterListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.Size = new System.Drawing.Size(500, 35);
            this.tlpFilterListButton.TabIndex = 2;
            // 
            // bFilterList_ResetCount
            // 
            this.bFilterList_ResetCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_ResetCount.Ghost = true;
            this.bFilterList_ResetCount.IconRatio = 0.8F;
            this.bFilterList_ResetCount.IconSvg = "UndoOutlined";
            this.bFilterList_ResetCount.Location = new System.Drawing.Point(203, 3);
            this.bFilterList_ResetCount.Name = "bFilterList_ResetCount";
            this.bFilterList_ResetCount.Size = new System.Drawing.Size(94, 29);
            this.bFilterList_ResetCount.TabIndex = 4;
            this.bFilterList_ResetCount.WaveSize = 0;
            this.bFilterList_ResetCount.Click += new System.EventHandler(this.bFilterList_ResetCount_Click);
            // 
            // bFilterList_DisableAll
            // 
            this.bFilterList_DisableAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_DisableAll.Ghost = true;
            this.bFilterList_DisableAll.IconRatio = 0.8F;
            this.bFilterList_DisableAll.IconSvg = "CloseSquareOutlined";
            this.bFilterList_DisableAll.Location = new System.Drawing.Point(103, 3);
            this.bFilterList_DisableAll.Name = "bFilterList_DisableAll";
            this.bFilterList_DisableAll.Size = new System.Drawing.Size(94, 29);
            this.bFilterList_DisableAll.TabIndex = 3;
            this.bFilterList_DisableAll.WaveSize = 0;
            this.bFilterList_DisableAll.Click += new System.EventHandler(this.bFilterList_DisableAll_Click);
            // 
            // bFilterList_EnableAll
            // 
            this.bFilterList_EnableAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_EnableAll.Ghost = true;
            this.bFilterList_EnableAll.IconRatio = 0.8F;
            this.bFilterList_EnableAll.IconSvg = "CheckSquareOutlined";
            this.bFilterList_EnableAll.Location = new System.Drawing.Point(3, 3);
            this.bFilterList_EnableAll.Name = "bFilterList_EnableAll";
            this.bFilterList_EnableAll.Size = new System.Drawing.Size(94, 29);
            this.bFilterList_EnableAll.TabIndex = 2;
            this.bFilterList_EnableAll.WaveSize = 0;
            this.bFilterList_EnableAll.Click += new System.EventHandler(this.bFilterList_EnableAll_Click);
            // 
            // bFilterList_Add
            // 
            this.bFilterList_Add.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_Add.Ghost = true;
            this.bFilterList_Add.IconRatio = 0.8F;
            this.bFilterList_Add.IconSvg = "PlusOutlined";
            this.bFilterList_Add.Location = new System.Drawing.Point(303, 3);
            this.bFilterList_Add.Name = "bFilterList_Add";
            this.bFilterList_Add.Size = new System.Drawing.Size(94, 29);
            this.bFilterList_Add.TabIndex = 0;
            this.bFilterList_Add.WaveSize = 0;
            this.bFilterList_Add.Click += new System.EventHandler(this.bFilterList_Add_Click);
            // 
            // bFilterList_Delete
            // 
            this.bFilterList_Delete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_Delete.Ghost = true;
            this.bFilterList_Delete.IconRatio = 0.8F;
            this.bFilterList_Delete.IconSvg = "DeleteOutlined";
            this.bFilterList_Delete.Location = new System.Drawing.Point(403, 3);
            this.bFilterList_Delete.Name = "bFilterList_Delete";
            this.bFilterList_Delete.Size = new System.Drawing.Size(94, 29);
            this.bFilterList_Delete.TabIndex = 1;
            this.bFilterList_Delete.WaveSize = 0;
            this.bFilterList_Delete.Click += new System.EventHandler(this.bFilterList_Delete_Click);
            // 
            // tpRobotList
            // 
            this.tpRobotList.Controls.Add(this.tlpRobotList);
            this.tpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRobotList.LocalizationText = "Robot";
            this.tpRobotList.Location = new System.Drawing.Point(0, 26);
            this.tpRobotList.Name = "tpRobotList";
            this.tpRobotList.Size = new System.Drawing.Size(500, 474);
            this.tpRobotList.TabIndex = 2;
            this.tpRobotList.Text = "机器人";
            // 
            // tlpRobotList
            // 
            this.tlpRobotList.ColumnCount = 1;
            this.tlpRobotList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.Controls.Add(this.tRobotList, 0, 1);
            this.tlpRobotList.Controls.Add(this.tlpRobotListButton, 0, 0);
            this.tlpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotList.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotList.Name = "tlpRobotList";
            this.tlpRobotList.RowCount = 2;
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.Size = new System.Drawing.Size(500, 474);
            this.tlpRobotList.TabIndex = 2;
            // 
            // tRobotList
            // 
            this.tRobotList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tRobotList.CellImpactHeight = false;
            this.tRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRobotList.Gap = 8;
            this.tRobotList.GapCell = 5;
            this.tRobotList.Gaps = new System.Drawing.Size(8, 8);
            this.tRobotList.Location = new System.Drawing.Point(0, 35);
            this.tRobotList.Margin = new System.Windows.Forms.Padding(0);
            this.tRobotList.MultipleRows = true;
            this.tRobotList.Name = "tRobotList";
            this.tRobotList.Size = new System.Drawing.Size(500, 439);
            this.tRobotList.SwitchSize = 12;
            this.tRobotList.TabIndex = 1;
            this.tRobotList.VisibleHeader = false;
            this.tRobotList.CellClick += new AntdUI.Table.ClickEventHandler(this.tRobotList_CellClick);
            this.tRobotList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tRobotList_CellDoubleClick);
            // 
            // tlpRobotListButton
            // 
            this.tlpRobotListButton.ColumnCount = 7;
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Stop, 3, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Execute, 2, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_ResetCount, 4, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_DisableAll, 1, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_EnableAll, 0, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Add, 5, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Delete, 6, 0);
            this.tlpRobotListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotListButton.Name = "tlpRobotListButton";
            this.tlpRobotListButton.RowCount = 1;
            this.tlpRobotListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.Size = new System.Drawing.Size(500, 35);
            this.tlpRobotListButton.TabIndex = 2;
            // 
            // bRobotList_Stop
            // 
            this.bRobotList_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Stop.Ghost = true;
            this.bRobotList_Stop.IconRatio = 0.8F;
            this.bRobotList_Stop.IconSvg = "PauseCircleOutlined";
            this.bRobotList_Stop.Location = new System.Drawing.Point(213, 3);
            this.bRobotList_Stop.Name = "bRobotList_Stop";
            this.bRobotList_Stop.Size = new System.Drawing.Size(64, 29);
            this.bRobotList_Stop.TabIndex = 6;
            this.bRobotList_Stop.WaveSize = 0;
            this.bRobotList_Stop.Click += new System.EventHandler(this.bRobotList_Stop_Click);
            // 
            // bRobotList_Execute
            // 
            this.bRobotList_Execute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Execute.Ghost = true;
            this.bRobotList_Execute.IconRatio = 0.8F;
            this.bRobotList_Execute.IconSvg = "PlayCircleOutlined";
            this.bRobotList_Execute.Location = new System.Drawing.Point(143, 3);
            this.bRobotList_Execute.Name = "bRobotList_Execute";
            this.bRobotList_Execute.Size = new System.Drawing.Size(64, 29);
            this.bRobotList_Execute.TabIndex = 5;
            this.bRobotList_Execute.WaveSize = 0;
            this.bRobotList_Execute.Click += new System.EventHandler(this.bRobotList_Execute_Click);
            // 
            // bRobotList_ResetCount
            // 
            this.bRobotList_ResetCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_ResetCount.Ghost = true;
            this.bRobotList_ResetCount.IconRatio = 0.8F;
            this.bRobotList_ResetCount.IconSvg = "UndoOutlined";
            this.bRobotList_ResetCount.Location = new System.Drawing.Point(283, 3);
            this.bRobotList_ResetCount.Name = "bRobotList_ResetCount";
            this.bRobotList_ResetCount.Size = new System.Drawing.Size(64, 29);
            this.bRobotList_ResetCount.TabIndex = 4;
            this.bRobotList_ResetCount.WaveSize = 0;
            this.bRobotList_ResetCount.Click += new System.EventHandler(this.bRobotList_ResetCount_Click);
            // 
            // bRobotList_DisableAll
            // 
            this.bRobotList_DisableAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_DisableAll.Ghost = true;
            this.bRobotList_DisableAll.IconRatio = 0.8F;
            this.bRobotList_DisableAll.IconSvg = "CloseSquareOutlined";
            this.bRobotList_DisableAll.Location = new System.Drawing.Point(73, 3);
            this.bRobotList_DisableAll.Name = "bRobotList_DisableAll";
            this.bRobotList_DisableAll.Size = new System.Drawing.Size(64, 29);
            this.bRobotList_DisableAll.TabIndex = 3;
            this.bRobotList_DisableAll.WaveSize = 0;
            this.bRobotList_DisableAll.Click += new System.EventHandler(this.bRobotList_DisableAll_Click);
            // 
            // bRobotList_EnableAll
            // 
            this.bRobotList_EnableAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_EnableAll.Ghost = true;
            this.bRobotList_EnableAll.IconRatio = 0.8F;
            this.bRobotList_EnableAll.IconSvg = "CheckSquareOutlined";
            this.bRobotList_EnableAll.Location = new System.Drawing.Point(3, 3);
            this.bRobotList_EnableAll.Name = "bRobotList_EnableAll";
            this.bRobotList_EnableAll.Size = new System.Drawing.Size(64, 29);
            this.bRobotList_EnableAll.TabIndex = 2;
            this.bRobotList_EnableAll.WaveSize = 0;
            this.bRobotList_EnableAll.Click += new System.EventHandler(this.bRobotList_EnableAll_Click);
            // 
            // bRobotList_Add
            // 
            this.bRobotList_Add.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Add.Ghost = true;
            this.bRobotList_Add.IconRatio = 0.8F;
            this.bRobotList_Add.IconSvg = "PlusOutlined";
            this.bRobotList_Add.Location = new System.Drawing.Point(353, 3);
            this.bRobotList_Add.Name = "bRobotList_Add";
            this.bRobotList_Add.Size = new System.Drawing.Size(64, 29);
            this.bRobotList_Add.TabIndex = 0;
            this.bRobotList_Add.WaveSize = 0;
            this.bRobotList_Add.Click += new System.EventHandler(this.bRobotList_Add_Click);
            // 
            // bRobotList_Delete
            // 
            this.bRobotList_Delete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Delete.Ghost = true;
            this.bRobotList_Delete.IconRatio = 0.8F;
            this.bRobotList_Delete.IconSvg = "DeleteOutlined";
            this.bRobotList_Delete.Location = new System.Drawing.Point(423, 3);
            this.bRobotList_Delete.Name = "bRobotList_Delete";
            this.bRobotList_Delete.Size = new System.Drawing.Size(74, 29);
            this.bRobotList_Delete.TabIndex = 1;
            this.bRobotList_Delete.WaveSize = 0;
            this.bRobotList_Delete.Click += new System.EventHandler(this.bRobotList_Delete_Click);
            // 
            // tpSendList
            // 
            this.tpSendList.Controls.Add(this.tlpSendList);
            this.tpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSendList.LocalizationText = "Send";
            this.tpSendList.Location = new System.Drawing.Point(0, 26);
            this.tpSendList.Name = "tpSendList";
            this.tpSendList.Size = new System.Drawing.Size(500, 474);
            this.tpSendList.TabIndex = 1;
            this.tpSendList.Text = "发送";
            // 
            // tlpSendList
            // 
            this.tlpSendList.ColumnCount = 1;
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.Controls.Add(this.tSendList, 0, 1);
            this.tlpSendList.Controls.Add(this.tlpSendListButton, 0, 0);
            this.tlpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendList.Location = new System.Drawing.Point(0, 0);
            this.tlpSendList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendList.Name = "tlpSendList";
            this.tlpSendList.RowCount = 2;
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.Size = new System.Drawing.Size(500, 474);
            this.tlpSendList.TabIndex = 1;
            // 
            // tSendList
            // 
            this.tSendList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSendList.CellImpactHeight = false;
            this.tSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSendList.Gap = 8;
            this.tSendList.GapCell = 5;
            this.tSendList.Gaps = new System.Drawing.Size(8, 8);
            this.tSendList.Location = new System.Drawing.Point(0, 35);
            this.tSendList.Margin = new System.Windows.Forms.Padding(0);
            this.tSendList.MultipleRows = true;
            this.tSendList.Name = "tSendList";
            this.tSendList.Size = new System.Drawing.Size(500, 439);
            this.tSendList.SwitchSize = 12;
            this.tSendList.TabIndex = 1;
            this.tSendList.VisibleHeader = false;
            this.tSendList.CellClick += new AntdUI.Table.ClickEventHandler(this.tSendList_CellClick);
            this.tSendList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tSendList_CellDoubleClick);
            // 
            // tlpSendListButton
            // 
            this.tlpSendListButton.ColumnCount = 7;
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tlpSendListButton.Controls.Add(this.bSendList_Stop, 3, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_Execute, 2, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_ResetCount, 4, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_DisableAll, 1, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_EnableAll, 0, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_Add, 5, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_Delete, 6, 0);
            this.tlpSendListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpSendListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendListButton.Name = "tlpSendListButton";
            this.tlpSendListButton.RowCount = 1;
            this.tlpSendListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendListButton.Size = new System.Drawing.Size(500, 35);
            this.tlpSendListButton.TabIndex = 2;
            // 
            // bSendList_Stop
            // 
            this.bSendList_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Stop.Ghost = true;
            this.bSendList_Stop.IconRatio = 0.8F;
            this.bSendList_Stop.IconSvg = "PauseCircleOutlined";
            this.bSendList_Stop.Location = new System.Drawing.Point(213, 3);
            this.bSendList_Stop.Name = "bSendList_Stop";
            this.bSendList_Stop.Size = new System.Drawing.Size(64, 29);
            this.bSendList_Stop.TabIndex = 6;
            this.bSendList_Stop.WaveSize = 0;
            this.bSendList_Stop.Click += new System.EventHandler(this.bSendList_Stop_Click);
            // 
            // bSendList_Execute
            // 
            this.bSendList_Execute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Execute.Ghost = true;
            this.bSendList_Execute.IconRatio = 0.8F;
            this.bSendList_Execute.IconSvg = "PlayCircleOutlined";
            this.bSendList_Execute.Location = new System.Drawing.Point(143, 3);
            this.bSendList_Execute.Name = "bSendList_Execute";
            this.bSendList_Execute.Size = new System.Drawing.Size(64, 29);
            this.bSendList_Execute.TabIndex = 5;
            this.bSendList_Execute.WaveSize = 0;
            this.bSendList_Execute.Click += new System.EventHandler(this.bSendList_Execute_Click);
            // 
            // bSendList_ResetCount
            // 
            this.bSendList_ResetCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_ResetCount.Ghost = true;
            this.bSendList_ResetCount.IconRatio = 0.8F;
            this.bSendList_ResetCount.IconSvg = "UndoOutlined";
            this.bSendList_ResetCount.Location = new System.Drawing.Point(283, 3);
            this.bSendList_ResetCount.Name = "bSendList_ResetCount";
            this.bSendList_ResetCount.Size = new System.Drawing.Size(64, 29);
            this.bSendList_ResetCount.TabIndex = 4;
            this.bSendList_ResetCount.WaveSize = 0;
            this.bSendList_ResetCount.Click += new System.EventHandler(this.bSendList_ResetCount_Click);
            // 
            // bSendList_DisableAll
            // 
            this.bSendList_DisableAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_DisableAll.Ghost = true;
            this.bSendList_DisableAll.IconRatio = 0.8F;
            this.bSendList_DisableAll.IconSvg = "CloseSquareOutlined";
            this.bSendList_DisableAll.Location = new System.Drawing.Point(73, 3);
            this.bSendList_DisableAll.Name = "bSendList_DisableAll";
            this.bSendList_DisableAll.Size = new System.Drawing.Size(64, 29);
            this.bSendList_DisableAll.TabIndex = 3;
            this.bSendList_DisableAll.WaveSize = 0;
            this.bSendList_DisableAll.Click += new System.EventHandler(this.bSendList_DisableAll_Click);
            // 
            // bSendList_EnableAll
            // 
            this.bSendList_EnableAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_EnableAll.Ghost = true;
            this.bSendList_EnableAll.IconRatio = 0.8F;
            this.bSendList_EnableAll.IconSvg = "CheckSquareOutlined";
            this.bSendList_EnableAll.Location = new System.Drawing.Point(3, 3);
            this.bSendList_EnableAll.Name = "bSendList_EnableAll";
            this.bSendList_EnableAll.Size = new System.Drawing.Size(64, 29);
            this.bSendList_EnableAll.TabIndex = 2;
            this.bSendList_EnableAll.WaveSize = 0;
            this.bSendList_EnableAll.Click += new System.EventHandler(this.bSendList_EnableAll_Click);
            // 
            // bSendList_Add
            // 
            this.bSendList_Add.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Add.Ghost = true;
            this.bSendList_Add.IconRatio = 0.8F;
            this.bSendList_Add.IconSvg = "PlusOutlined";
            this.bSendList_Add.Location = new System.Drawing.Point(353, 3);
            this.bSendList_Add.Name = "bSendList_Add";
            this.bSendList_Add.Size = new System.Drawing.Size(64, 29);
            this.bSendList_Add.TabIndex = 0;
            this.bSendList_Add.WaveSize = 0;
            this.bSendList_Add.Click += new System.EventHandler(this.bSendList_Add_Click);
            // 
            // bSendList_Delete
            // 
            this.bSendList_Delete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Delete.Ghost = true;
            this.bSendList_Delete.IconRatio = 0.8F;
            this.bSendList_Delete.IconSvg = "DeleteOutlined";
            this.bSendList_Delete.Location = new System.Drawing.Point(423, 3);
            this.bSendList_Delete.Name = "bSendList_Delete";
            this.bSendList_Delete.Size = new System.Drawing.Size(74, 29);
            this.bSendList_Delete.TabIndex = 1;
            this.bSendList_Delete.WaveSize = 0;
            this.bSendList_Delete.Click += new System.EventHandler(this.bSendList_Delete_Click);
            // 
            // QuickList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpQuickList);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "QuickList";
            this.Size = new System.Drawing.Size(500, 500);
            this.Load += new System.EventHandler(this.QuickList_Load);
            this.tlpQuickList.ResumeLayout(false);
            this.tabQuickList.ResumeLayout(false);
            this.tpFilterList.ResumeLayout(false);
            this.tlpFilterList.ResumeLayout(false);
            this.tlpFilterListButton.ResumeLayout(false);
            this.tpRobotList.ResumeLayout(false);
            this.tlpRobotList.ResumeLayout(false);
            this.tlpRobotListButton.ResumeLayout(false);
            this.tpSendList.ResumeLayout(false);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendListButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpQuickList;
        private AntdUI.Tabs tabQuickList;
        private AntdUI.TabPage tpFilterList;
        private AntdUI.TabPage tpSendList;
        private AntdUI.TabPage tpRobotList;
        private TableLayoutPanelEx tlpFilterList;
        private AntdUI.Table tFilterList;
        private TableLayoutPanelEx tlpFilterListButton;
        private AntdUI.Button bFilterList_Add;
        private AntdUI.Button bFilterList_Delete;
        private AntdUI.Button bFilterList_DisableAll;
        private AntdUI.Button bFilterList_EnableAll;
        private AntdUI.Button bFilterList_ResetCount;
        private TableLayoutPanelEx tlpSendList;
        private AntdUI.Table tSendList;
        private TableLayoutPanelEx tlpSendListButton;
        private AntdUI.Button bSendList_ResetCount;
        private AntdUI.Button bSendList_DisableAll;
        private AntdUI.Button bSendList_EnableAll;
        private AntdUI.Button bSendList_Add;
        private AntdUI.Button bSendList_Delete;
        private AntdUI.Button bSendList_Stop;
        private AntdUI.Button bSendList_Execute;
        private TableLayoutPanelEx tlpRobotList;
        private AntdUI.Table tRobotList;
        private TableLayoutPanelEx tlpRobotListButton;
        private AntdUI.Button bRobotList_Stop;
        private AntdUI.Button bRobotList_Execute;
        private AntdUI.Button bRobotList_ResetCount;
        private AntdUI.Button bRobotList_DisableAll;
        private AntdUI.Button bRobotList_EnableAll;
        private AntdUI.Button bRobotList_Add;
        private AntdUI.Button bRobotList_Delete;
    }
}

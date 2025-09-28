namespace WinsockPacketEditor
{
    partial class RobotList
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
            this.tlpRobotList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpRobotListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bRobotList_Reset = new AntdUI.Button();
            this.bDisableAll = new AntdUI.Button();
            this.bEnableAll = new AntdUI.Button();
            this.ddMenu = new AntdUI.Dropdown();
            this.bRobotList_Stop = new AntdUI.Button();
            this.bRobotList_Start = new AntdUI.Button();
            this.tRobotList = new AntdUI.Table();
            this.bgwRobotList = new System.ComponentModel.BackgroundWorker();
            this.tlpRobotList.SuspendLayout();
            this.tlpRobotListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRobotList
            // 
            this.tlpRobotList.ColumnCount = 1;
            this.tlpRobotList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.Controls.Add(this.tlpRobotListButton, 0, 0);
            this.tlpRobotList.Controls.Add(this.tRobotList, 0, 1);
            this.tlpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotList.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotList.Name = "tlpRobotList";
            this.tlpRobotList.RowCount = 2;
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpRobotList.Size = new System.Drawing.Size(1100, 700);
            this.tlpRobotList.TabIndex = 5;
            // 
            // tlpRobotListButton
            // 
            this.tlpRobotListButton.ColumnCount = 7;
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Reset, 2, 0);
            this.tlpRobotListButton.Controls.Add(this.bDisableAll, 1, 0);
            this.tlpRobotListButton.Controls.Add(this.bEnableAll, 0, 0);
            this.tlpRobotListButton.Controls.Add(this.ddMenu, 6, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Stop, 4, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Start, 3, 0);
            this.tlpRobotListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotListButton.Name = "tlpRobotListButton";
            this.tlpRobotListButton.RowCount = 1;
            this.tlpRobotListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.Size = new System.Drawing.Size(1100, 40);
            this.tlpRobotListButton.TabIndex = 4;
            // 
            // bRobotList_Reset
            // 
            this.bRobotList_Reset.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bRobotList_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Reset.IconSvg = "UndoOutlined";
            this.bRobotList_Reset.LocalizationText = "FilterList.ResetCount";
            this.bRobotList_Reset.Location = new System.Drawing.Point(184, 2);
            this.bRobotList_Reset.Margin = new System.Windows.Forms.Padding(2);
            this.bRobotList_Reset.Name = "bRobotList_Reset";
            this.bRobotList_Reset.Size = new System.Drawing.Size(87, 36);
            this.bRobotList_Reset.TabIndex = 17;
            this.bRobotList_Reset.Text = "重置计数";
            this.bRobotList_Reset.Type = AntdUI.TTypeMini.Warn;
            this.bRobotList_Reset.Click += new System.EventHandler(this.bRobotList_Reset_Click);
            // 
            // bDisableAll
            // 
            this.bDisableAll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bDisableAll.IconSvg = "CloseCircleOutlined";
            this.bDisableAll.LocalizationText = "DisableAll";
            this.bDisableAll.Location = new System.Drawing.Point(93, 2);
            this.bDisableAll.Margin = new System.Windows.Forms.Padding(2);
            this.bDisableAll.Name = "bDisableAll";
            this.bDisableAll.Size = new System.Drawing.Size(87, 36);
            this.bDisableAll.TabIndex = 16;
            this.bDisableAll.Text = "全部禁用";
            this.bDisableAll.Type = AntdUI.TTypeMini.Error;
            this.bDisableAll.Click += new System.EventHandler(this.bDisableAll_Click);
            // 
            // bEnableAll
            // 
            this.bEnableAll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bEnableAll.IconSvg = "CheckCircleOutlined";
            this.bEnableAll.LocalizationText = "EnableAll";
            this.bEnableAll.Location = new System.Drawing.Point(2, 2);
            this.bEnableAll.Margin = new System.Windows.Forms.Padding(2);
            this.bEnableAll.Name = "bEnableAll";
            this.bEnableAll.Size = new System.Drawing.Size(87, 36);
            this.bEnableAll.TabIndex = 15;
            this.bEnableAll.Text = "全部启用";
            this.bEnableAll.Type = AntdUI.TTypeMini.Success;
            this.bEnableAll.Click += new System.EventHandler(this.bEnableAll_Click);
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
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // bRobotList_Stop
            // 
            this.bRobotList_Stop.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bRobotList_Stop.BorderWidth = 1F;
            this.bRobotList_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Stop.Enabled = false;
            this.bRobotList_Stop.IconSvg = "PauseCircleOutlined";
            this.bRobotList_Stop.LocalizationText = "Stop";
            this.bRobotList_Stop.Location = new System.Drawing.Point(342, 2);
            this.bRobotList_Stop.Margin = new System.Windows.Forms.Padding(2);
            this.bRobotList_Stop.Name = "bRobotList_Stop";
            this.bRobotList_Stop.Size = new System.Drawing.Size(63, 36);
            this.bRobotList_Stop.TabIndex = 8;
            this.bRobotList_Stop.Text = "停止";
            this.bRobotList_Stop.Type = AntdUI.TTypeMini.Warn;
            this.bRobotList_Stop.Click += new System.EventHandler(this.bRobotList_Stop_Click);
            // 
            // bRobotList_Start
            // 
            this.bRobotList_Start.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bRobotList_Start.BorderWidth = 1F;
            this.bRobotList_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Start.IconSvg = "PlayCircleOutlined";
            this.bRobotList_Start.LocalizationText = "Execute";
            this.bRobotList_Start.Location = new System.Drawing.Point(275, 2);
            this.bRobotList_Start.Margin = new System.Windows.Forms.Padding(2);
            this.bRobotList_Start.Name = "bRobotList_Start";
            this.bRobotList_Start.Size = new System.Drawing.Size(63, 36);
            this.bRobotList_Start.TabIndex = 7;
            this.bRobotList_Start.Text = "执行";
            this.bRobotList_Start.Type = AntdUI.TTypeMini.Primary;
            this.bRobotList_Start.Click += new System.EventHandler(this.bRobotList_Start_Click);
            // 
            // tRobotList
            // 
            this.tRobotList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tRobotList.CellImpactHeight = false;
            this.tRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRobotList.Gap = 8;
            this.tRobotList.GapCell = 5;
            this.tRobotList.Gaps = new System.Drawing.Size(8, 8);
            this.tRobotList.Location = new System.Drawing.Point(2, 42);
            this.tRobotList.Margin = new System.Windows.Forms.Padding(2);
            this.tRobotList.MultipleRows = true;
            this.tRobotList.Name = "tRobotList";
            this.tRobotList.Size = new System.Drawing.Size(1096, 656);
            this.tRobotList.TabIndex = 1;
            this.tRobotList.CellClick += new AntdUI.Table.ClickEventHandler(this.tRobotList_CellClick);
            this.tRobotList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tRobotList_CellButtonClick);
            this.tRobotList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tRobotList_CellDoubleClick);
            // 
            // bgwRobotList
            // 
            this.bgwRobotList.WorkerSupportsCancellation = true;
            this.bgwRobotList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRobotList_DoWork);
            this.bgwRobotList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRobotList_RunWorkerCompleted);
            // 
            // RobotList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRobotList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RobotList";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.RobotList_Load);
            this.tlpRobotList.ResumeLayout(false);
            this.tlpRobotListButton.ResumeLayout(false);
            this.tlpRobotListButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpRobotList;
        private TableLayoutPanelEx tlpRobotListButton;
        private AntdUI.Button bRobotList_Stop;
        private AntdUI.Button bRobotList_Start;
        private AntdUI.Table tRobotList;
        private System.ComponentModel.BackgroundWorker bgwRobotList;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Button bEnableAll;
        private AntdUI.Button bDisableAll;
        private AntdUI.Button bRobotList_Reset;
    }
}

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
            this.tlpRobotList = new TableLayoutPanelEx();
            this.tlpRobotListButton = new TableLayoutPanelEx();
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
            this.tlpRobotList.ColumnCount = 3;
            this.tlpRobotList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpRobotList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpRobotList.Controls.Add(this.tlpRobotListButton, 1, 1);
            this.tlpRobotList.Controls.Add(this.tRobotList, 1, 2);
            this.tlpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotList.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotList.Name = "tlpRobotList";
            this.tlpRobotList.RowCount = 3;
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.Size = new System.Drawing.Size(800, 800);
            this.tlpRobotList.TabIndex = 5;
            // 
            // tlpRobotListButton
            // 
            this.tlpRobotListButton.ColumnCount = 6;
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.Controls.Add(this.bDisableAll, 1, 0);
            this.tlpRobotListButton.Controls.Add(this.bEnableAll, 0, 0);
            this.tlpRobotListButton.Controls.Add(this.ddMenu, 5, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Stop, 3, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Start, 2, 0);
            this.tlpRobotListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotListButton.Location = new System.Drawing.Point(30, 20);
            this.tlpRobotListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotListButton.Name = "tlpRobotListButton";
            this.tlpRobotListButton.RowCount = 1;
            this.tlpRobotListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.Size = new System.Drawing.Size(740, 50);
            this.tlpRobotListButton.TabIndex = 4;
            // 
            // bDisableAll
            // 
            this.bDisableAll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bDisableAll.IconSvg = "CloseCircleOutlined";
            this.bDisableAll.LocalizationText = "DisableAll";
            this.bDisableAll.Location = new System.Drawing.Point(123, 3);
            this.bDisableAll.Name = "bDisableAll";
            this.bDisableAll.Size = new System.Drawing.Size(114, 44);
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
            this.bEnableAll.Location = new System.Drawing.Point(3, 3);
            this.bEnableAll.Name = "bEnableAll";
            this.bEnableAll.Size = new System.Drawing.Size(114, 44);
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
            this.ddMenu.Location = new System.Drawing.Point(693, 3);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Size = new System.Drawing.Size(44, 44);
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
            this.bRobotList_Stop.Location = new System.Drawing.Point(331, 3);
            this.bRobotList_Stop.Name = "bRobotList_Stop";
            this.bRobotList_Stop.Size = new System.Drawing.Size(82, 44);
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
            this.bRobotList_Start.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bRobotList_Start.LoadingWaveCount = 6;
            this.bRobotList_Start.LoadingWaveSize = 6;
            this.bRobotList_Start.LoadingWaveValue = 0.6F;
            this.bRobotList_Start.LoadingWaveVertical = true;
            this.bRobotList_Start.LocalizationText = "Execute";
            this.bRobotList_Start.Location = new System.Drawing.Point(243, 3);
            this.bRobotList_Start.Name = "bRobotList_Start";
            this.bRobotList_Start.Size = new System.Drawing.Size(82, 44);
            this.bRobotList_Start.TabIndex = 7;
            this.bRobotList_Start.Text = "执行";
            this.bRobotList_Start.Type = AntdUI.TTypeMini.Info;
            this.bRobotList_Start.Click += new System.EventHandler(this.bRobotList_Start_Click);
            // 
            // tRobotList
            // 
            this.tRobotList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tRobotList.CellImpactHeight = false;
            this.tRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRobotList.Gap = 12;
            this.tRobotList.Location = new System.Drawing.Point(33, 73);
            this.tRobotList.MultipleRows = true;
            this.tRobotList.Name = "tRobotList";
            this.tRobotList.Size = new System.Drawing.Size(734, 724);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRobotList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "RobotList";
            this.Size = new System.Drawing.Size(800, 800);
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
    }
}

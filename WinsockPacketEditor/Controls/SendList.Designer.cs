namespace WinsockPacketEditor
{
    partial class SendList
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
            this.tlpSendList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tSendList = new AntdUI.Table();
            this.tlpSendListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSendList_Reset = new AntdUI.Button();
            this.bDisableAll = new AntdUI.Button();
            this.bEnableAll = new AntdUI.Button();
            this.ddMenu = new AntdUI.Dropdown();
            this.bSendList_Stop = new AntdUI.Button();
            this.bSendList_Start = new AntdUI.Button();
            this.bgwSendList = new System.ComponentModel.BackgroundWorker();
            this.tlpSendList.SuspendLayout();
            this.tlpSendListButton.SuspendLayout();
            this.SuspendLayout();
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
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpSendList.Size = new System.Drawing.Size(1100, 800);
            this.tlpSendList.TabIndex = 4;
            // 
            // tSendList
            // 
            this.tSendList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSendList.CellImpactHeight = false;
            this.tSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSendList.Gap = 12;
            this.tSendList.Location = new System.Drawing.Point(2, 42);
            this.tSendList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tSendList.MultipleRows = true;
            this.tSendList.Name = "tSendList";
            this.tSendList.Size = new System.Drawing.Size(1096, 756);
            this.tSendList.TabIndex = 1;
            this.tSendList.CellClick += new AntdUI.Table.ClickEventHandler(this.tSendList_CellClick);
            this.tSendList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tSendList_CellButtonClick);
            this.tSendList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tSendList_CellDoubleClick);
            // 
            // tlpSendListButton
            // 
            this.tlpSendListButton.ColumnCount = 7;
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.Controls.Add(this.bSendList_Reset, 2, 0);
            this.tlpSendListButton.Controls.Add(this.bDisableAll, 1, 0);
            this.tlpSendListButton.Controls.Add(this.bEnableAll, 0, 0);
            this.tlpSendListButton.Controls.Add(this.ddMenu, 6, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_Stop, 4, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_Start, 3, 0);
            this.tlpSendListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpSendListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendListButton.Name = "tlpSendListButton";
            this.tlpSendListButton.RowCount = 1;
            this.tlpSendListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendListButton.Size = new System.Drawing.Size(1100, 40);
            this.tlpSendListButton.TabIndex = 3;
            // 
            // bSendList_Reset
            // 
            this.bSendList_Reset.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSendList_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Reset.IconSvg = "UndoOutlined";
            this.bSendList_Reset.LocalizationText = "FilterList.ResetCount";
            this.bSendList_Reset.Location = new System.Drawing.Point(184, 2);
            this.bSendList_Reset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bSendList_Reset.Name = "bSendList_Reset";
            this.bSendList_Reset.Size = new System.Drawing.Size(87, 36);
            this.bSendList_Reset.TabIndex = 16;
            this.bSendList_Reset.Text = "重置计数";
            this.bSendList_Reset.Type = AntdUI.TTypeMini.Warn;
            this.bSendList_Reset.Click += new System.EventHandler(this.bSendList_Reset_Click);
            // 
            // bDisableAll
            // 
            this.bDisableAll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bDisableAll.IconSvg = "CloseCircleOutlined";
            this.bDisableAll.LocalizationText = "DisableAll";
            this.bDisableAll.Location = new System.Drawing.Point(93, 2);
            this.bDisableAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bDisableAll.Name = "bDisableAll";
            this.bDisableAll.Size = new System.Drawing.Size(87, 36);
            this.bDisableAll.TabIndex = 15;
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
            this.bEnableAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bEnableAll.Name = "bEnableAll";
            this.bEnableAll.Size = new System.Drawing.Size(87, 36);
            this.bEnableAll.TabIndex = 14;
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
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Size = new System.Drawing.Size(32, 36);
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // bSendList_Stop
            // 
            this.bSendList_Stop.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSendList_Stop.BorderWidth = 1F;
            this.bSendList_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Stop.Enabled = false;
            this.bSendList_Stop.IconSvg = "PauseCircleOutlined";
            this.bSendList_Stop.LocalizationText = "Stop";
            this.bSendList_Stop.Location = new System.Drawing.Point(342, 2);
            this.bSendList_Stop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bSendList_Stop.Name = "bSendList_Stop";
            this.bSendList_Stop.Size = new System.Drawing.Size(63, 36);
            this.bSendList_Stop.TabIndex = 8;
            this.bSendList_Stop.Text = "停止";
            this.bSendList_Stop.Type = AntdUI.TTypeMini.Warn;
            this.bSendList_Stop.Click += new System.EventHandler(this.bSendList_Stop_Click);
            // 
            // bSendList_Start
            // 
            this.bSendList_Start.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSendList_Start.BorderWidth = 1F;
            this.bSendList_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Start.IconSvg = "PlayCircleOutlined";
            this.bSendList_Start.LocalizationText = "Execute";
            this.bSendList_Start.Location = new System.Drawing.Point(275, 2);
            this.bSendList_Start.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bSendList_Start.Name = "bSendList_Start";
            this.bSendList_Start.Size = new System.Drawing.Size(63, 36);
            this.bSendList_Start.TabIndex = 7;
            this.bSendList_Start.Text = "执行";
            this.bSendList_Start.Type = AntdUI.TTypeMini.Primary;
            this.bSendList_Start.Click += new System.EventHandler(this.bSendList_Start_Click);
            // 
            // bgwSendList
            // 
            this.bgwSendList.WorkerSupportsCancellation = true;
            this.bgwSendList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendList_DoWork);
            this.bgwSendList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendList_RunWorkerCompleted);
            // 
            // SendList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSendList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SendList";
            this.Size = new System.Drawing.Size(1100, 800);
            this.Load += new System.EventHandler(this.SendList_Load);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendListButton.ResumeLayout(false);
            this.tlpSendListButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpSendList;
        private AntdUI.Table tSendList;
        private TableLayoutPanelEx tlpSendListButton;
        private AntdUI.Button bSendList_Stop;
        private AntdUI.Button bSendList_Start;
        private System.ComponentModel.BackgroundWorker bgwSendList;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Button bEnableAll;
        private AntdUI.Button bDisableAll;
        private AntdUI.Button bSendList_Reset;
    }
}

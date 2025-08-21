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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            this.tlpRobotList = new System.Windows.Forms.TableLayoutPanel();
            this.tlpRobotListButton = new System.Windows.Forms.TableLayoutPanel();
            this.bRobotList_Stop = new AntdUI.Button();
            this.mRobotList = new AntdUI.Menu();
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
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRobotList.Size = new System.Drawing.Size(800, 800);
            this.tlpRobotList.TabIndex = 5;
            // 
            // tlpRobotListButton
            // 
            this.tlpRobotListButton.ColumnCount = 4;
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Stop, 1, 0);
            this.tlpRobotListButton.Controls.Add(this.mRobotList, 3, 0);
            this.tlpRobotListButton.Controls.Add(this.bRobotList_Start, 0, 0);
            this.tlpRobotListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotListButton.Name = "tlpRobotListButton";
            this.tlpRobotListButton.RowCount = 1;
            this.tlpRobotListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotListButton.Size = new System.Drawing.Size(800, 50);
            this.tlpRobotListButton.TabIndex = 4;
            // 
            // bRobotList_Stop
            // 
            this.bRobotList_Stop.BorderWidth = 1F;
            this.bRobotList_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Stop.Enabled = false;
            this.bRobotList_Stop.IconSvg = "PauseCircleOutlined";
            this.bRobotList_Stop.LocalizationText = "Stop";
            this.bRobotList_Stop.Location = new System.Drawing.Point(123, 3);
            this.bRobotList_Stop.Name = "bRobotList_Stop";
            this.bRobotList_Stop.Size = new System.Drawing.Size(114, 44);
            this.bRobotList_Stop.TabIndex = 8;
            this.bRobotList_Stop.Text = "停止";
            this.bRobotList_Stop.Type = AntdUI.TTypeMini.Error;
            this.bRobotList_Stop.Click += new System.EventHandler(this.bRobotList_Stop_Click);
            // 
            // mRobotList
            // 
            this.mRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mRobotList.Gap = 0;
            this.mRobotList.IconRatio = 1F;
            menuItem1.IconSvg = "PlusOutlined";
            menuItem2.IconSvg = "UserAddOutlined";
            menuItem2.ID = "miAdd";
            menuItem2.LocalizationText = "RobotList.{id}";
            menuItem2.Text = "新增机器人";
            menuItem3.IconSvg = "FolderOpenOutlined";
            menuItem3.ID = "miImport";
            menuItem3.LocalizationText = "RobotList.{id}";
            menuItem3.Text = "导入机器人列表";
            menuItem4.IconSvg = "DeliveredProcedureOutlined";
            menuItem4.ID = "miExport";
            menuItem4.LocalizationText = "RobotList.{id}";
            menuItem4.Text = "导出所有机器人";
            menuItem5.IconSvg = "DeleteOutlined";
            menuItem5.ID = "miClear";
            menuItem5.LocalizationText = "RobotList.{id}";
            menuItem5.Text = "清空所有机器人";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Sub.Add(menuItem4);
            menuItem1.Sub.Add(menuItem5);
            this.mRobotList.Items.Add(menuItem1);
            this.mRobotList.Location = new System.Drawing.Point(737, 3);
            this.mRobotList.Mode = AntdUI.TMenuMode.Horizontal;
            this.mRobotList.Name = "mRobotList";
            this.mRobotList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mRobotList.Size = new System.Drawing.Size(60, 44);
            this.mRobotList.TabIndex = 6;
            this.mRobotList.Trigger = AntdUI.Trigger.Click;
            this.mRobotList.SelectChanged += new AntdUI.SelectEventHandler(this.mRobotList_SelectChanged);
            // 
            // bRobotList_Start
            // 
            this.bRobotList_Start.BorderWidth = 1F;
            this.bRobotList_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRobotList_Start.IconSvg = "PlayCircleOutlined";
            this.bRobotList_Start.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bRobotList_Start.LoadingWaveCount = 6;
            this.bRobotList_Start.LoadingWaveSize = 6;
            this.bRobotList_Start.LoadingWaveValue = 0.6F;
            this.bRobotList_Start.LoadingWaveVertical = true;
            this.bRobotList_Start.LocalizationText = "Execute";
            this.bRobotList_Start.Location = new System.Drawing.Point(3, 3);
            this.bRobotList_Start.Name = "bRobotList_Start";
            this.bRobotList_Start.Size = new System.Drawing.Size(114, 44);
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
            this.tRobotList.GapCell = 6;
            this.tRobotList.Location = new System.Drawing.Point(3, 53);
            this.tRobotList.MultipleRows = true;
            this.tRobotList.Name = "tRobotList";
            this.tRobotList.Size = new System.Drawing.Size(794, 744);
            this.tRobotList.TabIndex = 1;
            this.tRobotList.CellClick += new AntdUI.Table.ClickEventHandler(this.tRobotList_CellClick);
            this.tRobotList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tRobotList_CellButtonClick);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRobotList;
        private System.Windows.Forms.TableLayoutPanel tlpRobotListButton;
        private AntdUI.Button bRobotList_Stop;
        private AntdUI.Menu mRobotList;
        private AntdUI.Button bRobotList_Start;
        private AntdUI.Table tRobotList;
        private System.ComponentModel.BackgroundWorker bgwRobotList;
    }
}

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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            this.tlpSendList = new System.Windows.Forms.TableLayoutPanel();
            this.tSendList = new AntdUI.Table();
            this.tlpSendListButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSendList_Stop = new AntdUI.Button();
            this.mSendList = new AntdUI.Menu();
            this.bSendList_Start = new AntdUI.Button();
            this.bgwSendList = new System.ComponentModel.BackgroundWorker();
            this.tlpSendList.SuspendLayout();
            this.tlpSendListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSendList
            // 
            this.tlpSendList.ColumnCount = 3;
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpSendList.Controls.Add(this.tSendList, 1, 2);
            this.tlpSendList.Controls.Add(this.tlpSendListButton, 1, 1);
            this.tlpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendList.Location = new System.Drawing.Point(0, 0);
            this.tlpSendList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendList.Name = "tlpSendList";
            this.tlpSendList.RowCount = 3;
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.Size = new System.Drawing.Size(800, 800);
            this.tlpSendList.TabIndex = 4;
            // 
            // tSendList
            // 
            this.tSendList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSendList.CellImpactHeight = false;
            this.tSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSendList.Gap = 12;
            this.tSendList.Location = new System.Drawing.Point(33, 73);
            this.tSendList.MultipleRows = true;
            this.tSendList.Name = "tSendList";
            this.tSendList.Size = new System.Drawing.Size(734, 724);
            this.tSendList.TabIndex = 1;
            this.tSendList.CellClick += new AntdUI.Table.ClickEventHandler(this.tSendList_CellClick);
            this.tSendList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tSendList_CellButtonClick);
            // 
            // tlpSendListButton
            // 
            this.tlpSendListButton.ColumnCount = 4;
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendListButton.Controls.Add(this.bSendList_Stop, 1, 0);
            this.tlpSendListButton.Controls.Add(this.mSendList, 3, 0);
            this.tlpSendListButton.Controls.Add(this.bSendList_Start, 0, 0);
            this.tlpSendListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendListButton.Location = new System.Drawing.Point(30, 20);
            this.tlpSendListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendListButton.Name = "tlpSendListButton";
            this.tlpSendListButton.RowCount = 1;
            this.tlpSendListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendListButton.Size = new System.Drawing.Size(740, 50);
            this.tlpSendListButton.TabIndex = 3;
            // 
            // bSendList_Stop
            // 
            this.bSendList_Stop.BorderWidth = 1F;
            this.bSendList_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Stop.Enabled = false;
            this.bSendList_Stop.IconSvg = "PauseCircleOutlined";
            this.bSendList_Stop.LocalizationText = "Stop";
            this.bSendList_Stop.Location = new System.Drawing.Point(123, 3);
            this.bSendList_Stop.Name = "bSendList_Stop";
            this.bSendList_Stop.Size = new System.Drawing.Size(114, 44);
            this.bSendList_Stop.TabIndex = 8;
            this.bSendList_Stop.Text = "停止";
            this.bSendList_Stop.Type = AntdUI.TTypeMini.Error;
            this.bSendList_Stop.Click += new System.EventHandler(this.bSendList_Stop_Click);
            // 
            // mSendList
            // 
            this.mSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSendList.Gap = 5;
            this.mSendList.IconRatio = 1F;
            menuItem1.IconSvg = "PlusOutlined";
            menuItem2.IconSvg = "UserAddOutlined";
            menuItem2.ID = "miAdd";
            menuItem2.LocalizationText = "SendList.{id}";
            menuItem2.Text = "新增发送";
            menuItem3.IconSvg = "FolderOpenOutlined";
            menuItem3.ID = "miImport";
            menuItem3.LocalizationText = "SendList.{id}";
            menuItem3.Text = "导入发送列表";
            menuItem4.IconSvg = "DeliveredProcedureOutlined";
            menuItem4.ID = "miExport";
            menuItem4.LocalizationText = "SendList.{id}";
            menuItem4.Text = "导出所有发送";
            menuItem5.IconSvg = "DeleteOutlined";
            menuItem5.ID = "miClear";
            menuItem5.LocalizationText = "SendList.{id}";
            menuItem5.Text = "清空所有发送";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Sub.Add(menuItem4);
            menuItem1.Sub.Add(menuItem5);
            this.mSendList.Items.Add(menuItem1);
            this.mSendList.Location = new System.Drawing.Point(687, 3);
            this.mSendList.Mode = AntdUI.TMenuMode.Horizontal;
            this.mSendList.Name = "mSendList";
            this.mSendList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mSendList.Size = new System.Drawing.Size(50, 44);
            this.mSendList.TabIndex = 6;
            this.mSendList.Trigger = AntdUI.Trigger.Click;
            this.mSendList.SelectChanged += new AntdUI.SelectEventHandler(this.mSendList_SelectChanged);
            // 
            // bSendList_Start
            // 
            this.bSendList_Start.BorderWidth = 1F;
            this.bSendList_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSendList_Start.IconSvg = "PlayCircleOutlined";
            this.bSendList_Start.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bSendList_Start.LoadingWaveCount = 6;
            this.bSendList_Start.LoadingWaveSize = 6;
            this.bSendList_Start.LoadingWaveValue = 0.6F;
            this.bSendList_Start.LoadingWaveVertical = true;
            this.bSendList_Start.LocalizationText = "Execute";
            this.bSendList_Start.Location = new System.Drawing.Point(3, 3);
            this.bSendList_Start.Name = "bSendList_Start";
            this.bSendList_Start.Size = new System.Drawing.Size(114, 44);
            this.bSendList_Start.TabIndex = 7;
            this.bSendList_Start.Text = "执行";
            this.bSendList_Start.Type = AntdUI.TTypeMini.Info;
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSendList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SendList";
            this.Size = new System.Drawing.Size(800, 800);
            this.Load += new System.EventHandler(this.SendList_Load);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendListButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSendList;
        private AntdUI.Table tSendList;
        private System.Windows.Forms.TableLayoutPanel tlpSendListButton;
        private AntdUI.Button bSendList_Stop;
        private AntdUI.Menu mSendList;
        private AntdUI.Button bSendList_Start;
        private System.ComponentModel.BackgroundWorker bgwSendList;
    }
}

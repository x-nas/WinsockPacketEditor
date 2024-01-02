
namespace WPELibrary
{
    partial class Socket_SendList_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_SendList_Form));
            this.cmsSendList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.保存此列表数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.加载发送列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssSocketSendList = new System.Windows.Forms.StatusStrip();
            this.tlLoop_Send = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlLoop_Send_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Success_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Fail_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbSend_Bottom = new System.Windows.Forms.GroupBox();
            this.bSendListStop = new System.Windows.Forms.Button();
            this.bSendList = new System.Windows.Forms.Button();
            this.nudLoop_Int = new System.Windows.Forms.NumericUpDown();
            this.nudLoop_CNT = new System.Windows.Forms.NumericUpDown();
            this.lLoop_Int = new System.Windows.Forms.Label();
            this.lLoop_CNT = new System.Windows.Forms.Label();
            this.txtUseSocket = new System.Windows.Forms.TextBox();
            this.cbUseSocket = new System.Windows.Forms.CheckBox();
            this.bgwSendList = new System.ComponentModel.BackgroundWorker();
            this.tSendList = new System.Windows.Forms.Timer(this.components);
            this.dgSendList = new System.Windows.Forms.DataGridView();
            this.cCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.cmsSendList.SuspendLayout();
            this.ssSocketSendList.SuspendLayout();
            this.gbSend_Bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSendList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsSendList
            // 
            this.cmsSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelete,
            this.toolStripSeparator1,
            this.tsmiClear,
            this.toolStripSeparator2,
            this.保存此列表数据ToolStripMenuItem,
            this.toolStripSeparator3,
            this.加载发送列表ToolStripMenuItem});
            this.cmsSendList.Name = "cmsBatchSend";
            this.cmsSendList.Size = new System.Drawing.Size(161, 110);
            this.cmsSendList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSendList_ItemClicked);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(160, 22);
            this.tsmiDelete.Text = "从列表中移除";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiClear
            // 
            this.tsmiClear.Name = "tsmiClear";
            this.tsmiClear.Size = new System.Drawing.Size(160, 22);
            this.tsmiClear.Text = "清空发送列表";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // 保存此列表数据ToolStripMenuItem
            // 
            this.保存此列表数据ToolStripMenuItem.Name = "保存此列表数据ToolStripMenuItem";
            this.保存此列表数据ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.保存此列表数据ToolStripMenuItem.Text = "保存此列表数据";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(157, 6);
            // 
            // 加载发送列表ToolStripMenuItem
            // 
            this.加载发送列表ToolStripMenuItem.Name = "加载发送列表ToolStripMenuItem";
            this.加载发送列表ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.加载发送列表ToolStripMenuItem.Text = "加载发送列表";
            // 
            // ssSocketSendList
            // 
            this.ssSocketSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlLoop_Send,
            this.tlLoop_Send_CNT,
            this.tlSplit,
            this.tlSendList_Success,
            this.tlSendList_Success_CNT,
            this.toolStripStatusLabel3,
            this.tlSendList_Fail,
            this.tlSendList_Fail_CNT});
            this.ssSocketSendList.Location = new System.Drawing.Point(0, 326);
            this.ssSocketSendList.Name = "ssSocketSendList";
            this.ssSocketSendList.Size = new System.Drawing.Size(681, 22);
            this.ssSocketSendList.SizingGrip = false;
            this.ssSocketSendList.TabIndex = 51;
            // 
            // tlLoop_Send
            // 
            this.tlLoop_Send.Name = "tlLoop_Send";
            this.tlLoop_Send.Size = new System.Drawing.Size(71, 17);
            this.tlLoop_Send.Text = "已循环次数:";
            // 
            // tlLoop_Send_CNT
            // 
            this.tlLoop_Send_CNT.Name = "tlLoop_Send_CNT";
            this.tlLoop_Send_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlLoop_Send_CNT.Text = "0";
            // 
            // tlSplit
            // 
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            this.tlSplit.Size = new System.Drawing.Size(11, 17);
            this.tlSplit.Text = "|";
            // 
            // tlSendList_Success
            // 
            this.tlSendList_Success.Name = "tlSendList_Success";
            this.tlSendList_Success.Size = new System.Drawing.Size(59, 17);
            this.tlSendList_Success.Text = "发送成功:";
            // 
            // tlSendList_Success_CNT
            // 
            this.tlSendList_Success_CNT.Name = "tlSendList_Success_CNT";
            this.tlSendList_Success_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSendList_Success_CNT.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // tlSendList_Fail
            // 
            this.tlSendList_Fail.Name = "tlSendList_Fail";
            this.tlSendList_Fail.Size = new System.Drawing.Size(59, 17);
            this.tlSendList_Fail.Text = "发送失败:";
            // 
            // tlSendList_Fail_CNT
            // 
            this.tlSendList_Fail_CNT.Name = "tlSendList_Fail_CNT";
            this.tlSendList_Fail_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSendList_Fail_CNT.Text = "0";
            // 
            // gbSend_Bottom
            // 
            this.gbSend_Bottom.Controls.Add(this.bSendListStop);
            this.gbSend_Bottom.Controls.Add(this.bSendList);
            this.gbSend_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbSend_Bottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbSend_Bottom.Location = new System.Drawing.Point(0, 271);
            this.gbSend_Bottom.Name = "gbSend_Bottom";
            this.gbSend_Bottom.Size = new System.Drawing.Size(681, 55);
            this.gbSend_Bottom.TabIndex = 52;
            this.gbSend_Bottom.TabStop = false;
            // 
            // bSendListStop
            // 
            this.bSendListStop.Location = new System.Drawing.Point(589, 17);
            this.bSendListStop.Name = "bSendListStop";
            this.bSendListStop.Size = new System.Drawing.Size(80, 29);
            this.bSendListStop.TabIndex = 108;
            this.bSendListStop.Text = "停 止 (&T)";
            this.bSendListStop.UseVisualStyleBackColor = true;
            this.bSendListStop.Click += new System.EventHandler(this.bSendStop_Click);
            // 
            // bSendList
            // 
            this.bSendList.Location = new System.Drawing.Point(484, 17);
            this.bSendList.Name = "bSendList";
            this.bSendList.Size = new System.Drawing.Size(80, 29);
            this.bSendList.TabIndex = 107;
            this.bSendList.Text = "发 送 (&F)";
            this.bSendList.UseVisualStyleBackColor = true;
            this.bSendList.Click += new System.EventHandler(this.bSend_Click);
            // 
            // nudLoop_Int
            // 
            this.nudLoop_Int.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudLoop_Int.Location = new System.Drawing.Point(598, 16);
            this.nudLoop_Int.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudLoop_Int.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudLoop_Int.Name = "nudLoop_Int";
            this.nudLoop_Int.Size = new System.Drawing.Size(71, 23);
            this.nudLoop_Int.TabIndex = 112;
            this.nudLoop_Int.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLoop_Int.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // nudLoop_CNT
            // 
            this.nudLoop_CNT.Location = new System.Drawing.Point(421, 16);
            this.nudLoop_CNT.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudLoop_CNT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLoop_CNT.Name = "nudLoop_CNT";
            this.nudLoop_CNT.Size = new System.Drawing.Size(74, 23);
            this.nudLoop_CNT.TabIndex = 111;
            this.nudLoop_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLoop_CNT.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lLoop_Int
            // 
            this.lLoop_Int.AutoSize = true;
            this.lLoop_Int.Location = new System.Drawing.Point(501, 19);
            this.lLoop_Int.Name = "lLoop_Int";
            this.lLoop_Int.Size = new System.Drawing.Size(91, 17);
            this.lLoop_Int.TabIndex = 104;
            this.lLoop_Int.Text = "发送间隔(毫秒):";
            // 
            // lLoop_CNT
            // 
            this.lLoop_CNT.AutoSize = true;
            this.lLoop_CNT.Location = new System.Drawing.Point(360, 19);
            this.lLoop_CNT.Name = "lLoop_CNT";
            this.lLoop_CNT.Size = new System.Drawing.Size(68, 17);
            this.lLoop_CNT.TabIndex = 103;
            this.lLoop_CNT.Text = "循环次数：";
            // 
            // txtUseSocket
            // 
            this.txtUseSocket.Location = new System.Drawing.Point(250, 16);
            this.txtUseSocket.Name = "txtUseSocket";
            this.txtUseSocket.Size = new System.Drawing.Size(100, 23);
            this.txtUseSocket.TabIndex = 95;
            this.txtUseSocket.WordWrap = false;
            // 
            // cbUseSocket
            // 
            this.cbUseSocket.AutoSize = true;
            this.cbUseSocket.Location = new System.Drawing.Point(143, 18);
            this.cbUseSocket.Name = "cbUseSocket";
            this.cbUseSocket.Size = new System.Drawing.Size(111, 21);
            this.cbUseSocket.TabIndex = 94;
            this.cbUseSocket.Text = "使用此套接字：";
            this.cbUseSocket.UseVisualStyleBackColor = true;
            // 
            // bgwSendList
            // 
            this.bgwSendList.WorkerSupportsCancellation = true;
            this.bgwSendList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendList_DoWork);
            // 
            // tSendList
            // 
            this.tSendList.Enabled = true;
            this.tSendList.Interval = 1000;
            this.tSendList.Tick += new System.EventHandler(this.tSendList_Tick);
            // 
            // dgSendList
            // 
            this.dgSendList.AllowUserToAddRows = false;
            this.dgSendList.AllowUserToDeleteRows = false;
            this.dgSendList.AllowUserToResizeRows = false;
            this.dgSendList.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgSendList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSendList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cCheck,
            this.cIndex,
            this.cNote,
            this.cSocket,
            this.cIPTo,
            this.cLen,
            this.cData});
            this.dgSendList.ContextMenuStrip = this.cmsSendList;
            this.dgSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSendList.Location = new System.Drawing.Point(0, 0);
            this.dgSendList.MultiSelect = false;
            this.dgSendList.Name = "dgSendList";
            this.dgSendList.RowHeadersVisible = false;
            this.dgSendList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgSendList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgSendList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgSendList.RowTemplate.Height = 23;
            this.dgSendList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSendList.Size = new System.Drawing.Size(681, 225);
            this.dgSendList.TabIndex = 55;
            // 
            // cCheck
            // 
            this.cCheck.FalseValue = "0";
            this.cCheck.HeaderText = "";
            this.cCheck.IndeterminateValue = "0";
            this.cCheck.Name = "cCheck";
            this.cCheck.TrueValue = "1";
            this.cCheck.Width = 30;
            // 
            // cIndex
            // 
            this.cIndex.DataPropertyName = "序号";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle1;
            this.cIndex.HeaderText = "序号";
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIndex.Width = 50;
            // 
            // cNote
            // 
            this.cNote.DataPropertyName = "备注";
            this.cNote.HeaderText = "备注";
            this.cNote.Name = "cNote";
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "套接字";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle2;
            this.cSocket.HeaderText = "套接字";
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cSocket.Width = 60;
            // 
            // cIPTo
            // 
            this.cIPTo.DataPropertyName = "目的地址";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cIPTo.DefaultCellStyle = dataGridViewCellStyle3;
            this.cIPTo.HeaderText = "目的地址";
            this.cIPTo.Name = "cIPTo";
            this.cIPTo.ReadOnly = true;
            this.cIPTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIPTo.Width = 150;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "长度";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle4;
            this.cLen.HeaderText = "长度";
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLen.Width = 50;
            // 
            // cData
            // 
            this.cData.DataPropertyName = "数据";
            this.cData.HeaderText = "数据";
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cData.Width = 340;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudLoop_Int);
            this.groupBox1.Controls.Add(this.cbSelectAll);
            this.groupBox1.Controls.Add(this.nudLoop_CNT);
            this.groupBox1.Controls.Add(this.txtUseSocket);
            this.groupBox1.Controls.Add(this.cbUseSocket);
            this.groupBox1.Controls.Add(this.lLoop_CNT);
            this.groupBox1.Controls.Add(this.lLoop_Int);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(681, 46);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(15, 18);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(88, 21);
            this.cbSelectAll.TabIndex = 110;
            this.cbSelectAll.Text = "全选 / 取消";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // Socket_SendList_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(681, 348);
            this.Controls.Add(this.dgSendList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSend_Bottom);
            this.Controls.Add(this.ssSocketSendList);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Socket_SendList_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发送列表";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SocketSendList_Form_FormClosed);
            this.Load += new System.EventHandler(this.SocketSendList_Form_Load);
            this.cmsSendList.ResumeLayout(false);
            this.ssSocketSendList.ResumeLayout(false);
            this.ssSocketSendList.PerformLayout();
            this.gbSend_Bottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSendList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip ssSocketSendList;
        private System.Windows.Forms.ToolStripStatusLabel tlLoop_Send;
        private System.Windows.Forms.ToolStripStatusLabel tlLoop_Send_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Success_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Fail_CNT;
        private System.Windows.Forms.GroupBox gbSend_Bottom;
        private System.Windows.Forms.ContextMenuStrip cmsSendList;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.ComponentModel.BackgroundWorker bgwSendList;
        private System.Windows.Forms.Timer tSendList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiClear;
        private System.Windows.Forms.TextBox txtUseSocket;
        private System.Windows.Forms.CheckBox cbUseSocket;
        private System.Windows.Forms.Button bSendListStop;
        private System.Windows.Forms.Button bSendList;
        private System.Windows.Forms.Label lLoop_Int;
        private System.Windows.Forms.Label lLoop_CNT;
        private System.Windows.Forms.DataGridView dgSendList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.NumericUpDown nudLoop_Int;
        private System.Windows.Forms.NumericUpDown nudLoop_CNT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 保存此列表数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 加载发送列表ToolStripMenuItem;
    }
}
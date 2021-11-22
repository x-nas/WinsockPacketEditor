
namespace WPELibrary
{
    partial class SocketBatchSend_Form
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
            this.cmsBatchSend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClear = new System.Windows.Forms.ToolStripMenuItem();
            this.ssSocketBatchSend = new System.Windows.Forms.StatusStrip();
            this.tlSendBatch = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendBatch_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbSend_Bottom = new System.Windows.Forms.GroupBox();
            this.bSendStop = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.txtSend_CNT = new System.Windows.Forms.TextBox();
            this.txtSend_Int = new System.Windows.Forms.TextBox();
            this.lSend_Int = new System.Windows.Forms.Label();
            this.lSend_CNT = new System.Windows.Forms.Label();
            this.txtUseSocket = new System.Windows.Forms.TextBox();
            this.cbUseSocket = new System.Windows.Forms.CheckBox();
            this.bgwSendPacket = new System.ComponentModel.BackgroundWorker();
            this.tSend = new System.Windows.Forms.Timer(this.components);
            this.sfdSaveSocket = new System.Windows.Forms.SaveFileDialog();
            this.dgBatchSend = new System.Windows.Forms.DataGridView();
            this.cCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bSaveSocket = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bLoadSocket = new System.Windows.Forms.Button();
            this.ofdLoadSocket = new System.Windows.Forms.OpenFileDialog();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.cmsBatchSend.SuspendLayout();
            this.ssSocketBatchSend.SuspendLayout();
            this.gbSend_Bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBatchSend)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsBatchSend
            // 
            this.cmsBatchSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelete,
            this.toolStripSeparator1,
            this.tsmiClear});
            this.cmsBatchSend.Name = "cmsBatchSend";
            this.cmsBatchSend.Size = new System.Drawing.Size(149, 54);
            this.cmsBatchSend.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsBatchSend_ItemClicked);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(148, 22);
            this.tsmiDelete.Text = "从列表中移除";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // tsmiClear
            // 
            this.tsmiClear.Name = "tsmiClear";
            this.tsmiClear.Size = new System.Drawing.Size(148, 22);
            this.tsmiClear.Text = "清空发送列表";
            // 
            // ssSocketBatchSend
            // 
            this.ssSocketBatchSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlSendBatch,
            this.tlSendBatch_CNT,
            this.tlSplit,
            this.tlSend_Success,
            this.tlSend_Success_CNT,
            this.toolStripStatusLabel3,
            this.tlSend_Fail,
            this.tlSend_Fail_CNT});
            this.ssSocketBatchSend.Location = new System.Drawing.Point(0, 326);
            this.ssSocketBatchSend.Name = "ssSocketBatchSend";
            this.ssSocketBatchSend.Size = new System.Drawing.Size(681, 22);
            this.ssSocketBatchSend.SizingGrip = false;
            this.ssSocketBatchSend.TabIndex = 51;
            // 
            // tlSendBatch
            // 
            this.tlSendBatch.Name = "tlSendBatch";
            this.tlSendBatch.Size = new System.Drawing.Size(71, 17);
            this.tlSendBatch.Text = "已循环次数:";
            // 
            // tlSendBatch_CNT
            // 
            this.tlSendBatch_CNT.Name = "tlSendBatch_CNT";
            this.tlSendBatch_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSendBatch_CNT.Text = "0";
            // 
            // tlSplit
            // 
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            this.tlSplit.Size = new System.Drawing.Size(11, 17);
            this.tlSplit.Text = "|";
            // 
            // tlSend_Success
            // 
            this.tlSend_Success.Name = "tlSend_Success";
            this.tlSend_Success.Size = new System.Drawing.Size(59, 17);
            this.tlSend_Success.Text = "发送成功:";
            // 
            // tlSend_Success_CNT
            // 
            this.tlSend_Success_CNT.Name = "tlSend_Success_CNT";
            this.tlSend_Success_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSend_Success_CNT.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // tlSend_Fail
            // 
            this.tlSend_Fail.Name = "tlSend_Fail";
            this.tlSend_Fail.Size = new System.Drawing.Size(59, 17);
            this.tlSend_Fail.Text = "发送失败:";
            // 
            // tlSend_Fail_CNT
            // 
            this.tlSend_Fail_CNT.Name = "tlSend_Fail_CNT";
            this.tlSend_Fail_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSend_Fail_CNT.Text = "0";
            // 
            // gbSend_Bottom
            // 
            this.gbSend_Bottom.Controls.Add(this.bSendStop);
            this.gbSend_Bottom.Controls.Add(this.bSend);
            this.gbSend_Bottom.Controls.Add(this.txtSend_CNT);
            this.gbSend_Bottom.Controls.Add(this.txtSend_Int);
            this.gbSend_Bottom.Controls.Add(this.lSend_Int);
            this.gbSend_Bottom.Controls.Add(this.lSend_CNT);
            this.gbSend_Bottom.Controls.Add(this.txtUseSocket);
            this.gbSend_Bottom.Controls.Add(this.cbUseSocket);
            this.gbSend_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbSend_Bottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbSend_Bottom.Location = new System.Drawing.Point(0, 271);
            this.gbSend_Bottom.Name = "gbSend_Bottom";
            this.gbSend_Bottom.Size = new System.Drawing.Size(681, 55);
            this.gbSend_Bottom.TabIndex = 52;
            this.gbSend_Bottom.TabStop = false;
            // 
            // bSendStop
            // 
            this.bSendStop.Location = new System.Drawing.Point(609, 17);
            this.bSendStop.Name = "bSendStop";
            this.bSendStop.Size = new System.Drawing.Size(60, 29);
            this.bSendStop.TabIndex = 108;
            this.bSendStop.Text = "停止 (&T)";
            this.bSendStop.UseVisualStyleBackColor = true;
            this.bSendStop.Click += new System.EventHandler(this.bSendStop_Click);
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(537, 17);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(60, 29);
            this.bSend.TabIndex = 107;
            this.bSend.Text = "发送 (&F)";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // txtSend_CNT
            // 
            this.txtSend_CNT.Location = new System.Drawing.Point(298, 20);
            this.txtSend_CNT.Name = "txtSend_CNT";
            this.txtSend_CNT.Size = new System.Drawing.Size(50, 23);
            this.txtSend_CNT.TabIndex = 105;
            this.txtSend_CNT.Text = "1";
            this.txtSend_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSend_Int
            // 
            this.txtSend_Int.Location = new System.Drawing.Point(451, 20);
            this.txtSend_Int.Name = "txtSend_Int";
            this.txtSend_Int.Size = new System.Drawing.Size(50, 23);
            this.txtSend_Int.TabIndex = 106;
            this.txtSend_Int.Text = "1000";
            this.txtSend_Int.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lSend_Int
            // 
            this.lSend_Int.AutoSize = true;
            this.lSend_Int.Location = new System.Drawing.Point(354, 23);
            this.lSend_Int.Name = "lSend_Int";
            this.lSend_Int.Size = new System.Drawing.Size(91, 17);
            this.lSend_Int.TabIndex = 104;
            this.lSend_Int.Text = "发送间隔(毫秒):";
            // 
            // lSend_CNT
            // 
            this.lSend_CNT.AutoSize = true;
            this.lSend_CNT.Location = new System.Drawing.Point(228, 23);
            this.lSend_CNT.Name = "lSend_CNT";
            this.lSend_CNT.Size = new System.Drawing.Size(68, 17);
            this.lSend_CNT.TabIndex = 103;
            this.lSend_CNT.Text = "循环次数：";
            // 
            // txtUseSocket
            // 
            this.txtUseSocket.Location = new System.Drawing.Point(122, 20);
            this.txtUseSocket.Name = "txtUseSocket";
            this.txtUseSocket.Size = new System.Drawing.Size(100, 23);
            this.txtUseSocket.TabIndex = 95;
            // 
            // cbUseSocket
            // 
            this.cbUseSocket.AutoSize = true;
            this.cbUseSocket.Location = new System.Drawing.Point(15, 22);
            this.cbUseSocket.Name = "cbUseSocket";
            this.cbUseSocket.Size = new System.Drawing.Size(111, 21);
            this.cbUseSocket.TabIndex = 94;
            this.cbUseSocket.Text = "使用此套接字：";
            this.cbUseSocket.UseVisualStyleBackColor = true;
            // 
            // bgwSendPacket
            // 
            this.bgwSendPacket.WorkerSupportsCancellation = true;
            this.bgwSendPacket.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendPacket_DoWork);
            // 
            // tSend
            // 
            this.tSend.Enabled = true;
            this.tSend.Interval = 1000;
            this.tSend.Tick += new System.EventHandler(this.tSend_Tick);
            // 
            // sfdSaveSocket
            // 
            this.sfdSaveSocket.Filter = "封包数据文件（*.txt）|*.txt";
            this.sfdSaveSocket.RestoreDirectory = true;
            this.sfdSaveSocket.Title = "保存封包数据";
            // 
            // dgBatchSend
            // 
            this.dgBatchSend.AllowUserToAddRows = false;
            this.dgBatchSend.AllowUserToDeleteRows = false;
            this.dgBatchSend.AllowUserToResizeRows = false;
            this.dgBatchSend.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgBatchSend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBatchSend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cCheck,
            this.cIndex,
            this.cNote,
            this.cSocket,
            this.cIPTo,
            this.cLen,
            this.cData});
            this.dgBatchSend.ContextMenuStrip = this.cmsBatchSend;
            this.dgBatchSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgBatchSend.Location = new System.Drawing.Point(0, 0);
            this.dgBatchSend.MultiSelect = false;
            this.dgBatchSend.Name = "dgBatchSend";
            this.dgBatchSend.RowHeadersVisible = false;
            this.dgBatchSend.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgBatchSend.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgBatchSend.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgBatchSend.RowTemplate.Height = 23;
            this.dgBatchSend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgBatchSend.Size = new System.Drawing.Size(681, 225);
            this.dgBatchSend.TabIndex = 55;
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
            // bSaveSocket
            // 
            this.bSaveSocket.Location = new System.Drawing.Point(413, 11);
            this.bSaveSocket.Name = "bSaveSocket";
            this.bSaveSocket.Size = new System.Drawing.Size(120, 29);
            this.bSaveSocket.TabIndex = 108;
            this.bSaveSocket.Text = "保存此列表数据";
            this.bSaveSocket.UseVisualStyleBackColor = true;
            this.bSaveSocket.Click += new System.EventHandler(this.bSaveSocket_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSelectAll);
            this.groupBox1.Controls.Add(this.bLoadSocket);
            this.groupBox1.Controls.Add(this.bSaveSocket);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(681, 46);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            // 
            // bLoadSocket
            // 
            this.bLoadSocket.Location = new System.Drawing.Point(551, 11);
            this.bLoadSocket.Name = "bLoadSocket";
            this.bLoadSocket.Size = new System.Drawing.Size(120, 29);
            this.bLoadSocket.TabIndex = 109;
            this.bLoadSocket.Text = "加载发送列表";
            this.bLoadSocket.UseVisualStyleBackColor = true;
            this.bLoadSocket.Click += new System.EventHandler(this.bLoadSocket_Click);
            // 
            // ofdLoadSocket
            // 
            this.ofdLoadSocket.Filter = "封包数据文件（*.txt）|*.txt";
            this.ofdLoadSocket.RestoreDirectory = true;
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
            // SocketBatchSend_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(681, 348);
            this.Controls.Add(this.dgBatchSend);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSend_Bottom);
            this.Controls.Add(this.ssSocketBatchSend);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SocketBatchSend_Form";
            this.Text = "发送列表";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SocketBatchSend_Form_FormClosed);
            this.Load += new System.EventHandler(this.SocketBatchSend_Form_Load);
            this.cmsBatchSend.ResumeLayout(false);
            this.ssSocketBatchSend.ResumeLayout(false);
            this.ssSocketBatchSend.PerformLayout();
            this.gbSend_Bottom.ResumeLayout(false);
            this.gbSend_Bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBatchSend)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip ssSocketBatchSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSendBatch;
        private System.Windows.Forms.ToolStripStatusLabel tlSendBatch_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail_CNT;
        private System.Windows.Forms.GroupBox gbSend_Bottom;
        private System.Windows.Forms.ContextMenuStrip cmsBatchSend;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.ComponentModel.BackgroundWorker bgwSendPacket;
        private System.Windows.Forms.Timer tSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiClear;
        private System.Windows.Forms.TextBox txtUseSocket;
        private System.Windows.Forms.CheckBox cbUseSocket;
        private System.Windows.Forms.Button bSendStop;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.TextBox txtSend_CNT;
        private System.Windows.Forms.TextBox txtSend_Int;
        private System.Windows.Forms.Label lSend_Int;
        private System.Windows.Forms.Label lSend_CNT;
        private System.Windows.Forms.SaveFileDialog sfdSaveSocket;
        private System.Windows.Forms.DataGridView dgBatchSend;
        private System.Windows.Forms.Button bSaveSocket;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bLoadSocket;
        private System.Windows.Forms.OpenFileDialog ofdLoadSocket;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.CheckBox cbSelectAll;
    }
}
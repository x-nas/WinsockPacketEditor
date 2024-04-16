
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_SendList_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmsSendList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiLoadList = new System.Windows.Forms.ToolStripMenuItem();
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
            resources.ApplyResources(this.cmsSendList, "cmsSendList");
            this.cmsSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelete,
            this.toolStripSeparator1,
            this.tsmiClear,
            this.toolStripSeparator2,
            this.tsmiSaveList,
            this.toolStripSeparator3,
            this.tsmiLoadList});
            this.cmsSendList.Name = "cmsBatchSend";
            this.cmsSendList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSendList_ItemClicked);
            // 
            // tsmiDelete
            // 
            resources.ApplyResources(this.tsmiDelete, "tsmiDelete");
            this.tsmiDelete.Name = "tsmiDelete";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // tsmiClear
            // 
            resources.ApplyResources(this.tsmiClear, "tsmiClear");
            this.tsmiClear.Name = "tsmiClear";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // tsmiSaveList
            // 
            resources.ApplyResources(this.tsmiSaveList, "tsmiSaveList");
            this.tsmiSaveList.Name = "tsmiSaveList";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // tsmiLoadList
            // 
            resources.ApplyResources(this.tsmiLoadList, "tsmiLoadList");
            this.tsmiLoadList.Name = "tsmiLoadList";
            // 
            // ssSocketSendList
            // 
            resources.ApplyResources(this.ssSocketSendList, "ssSocketSendList");
            this.ssSocketSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlLoop_Send,
            this.tlLoop_Send_CNT,
            this.tlSplit,
            this.tlSendList_Success,
            this.tlSendList_Success_CNT,
            this.toolStripStatusLabel3,
            this.tlSendList_Fail,
            this.tlSendList_Fail_CNT});
            this.ssSocketSendList.Name = "ssSocketSendList";
            this.ssSocketSendList.SizingGrip = false;
            // 
            // tlLoop_Send
            // 
            resources.ApplyResources(this.tlLoop_Send, "tlLoop_Send");
            this.tlLoop_Send.Name = "tlLoop_Send";
            // 
            // tlLoop_Send_CNT
            // 
            resources.ApplyResources(this.tlLoop_Send_CNT, "tlLoop_Send_CNT");
            this.tlLoop_Send_CNT.Name = "tlLoop_Send_CNT";
            // 
            // tlSplit
            // 
            resources.ApplyResources(this.tlSplit, "tlSplit");
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            // 
            // tlSendList_Success
            // 
            resources.ApplyResources(this.tlSendList_Success, "tlSendList_Success");
            this.tlSendList_Success.Name = "tlSendList_Success";
            // 
            // tlSendList_Success_CNT
            // 
            resources.ApplyResources(this.tlSendList_Success_CNT, "tlSendList_Success_CNT");
            this.tlSendList_Success_CNT.Name = "tlSendList_Success_CNT";
            // 
            // toolStripStatusLabel3
            // 
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            // 
            // tlSendList_Fail
            // 
            resources.ApplyResources(this.tlSendList_Fail, "tlSendList_Fail");
            this.tlSendList_Fail.Name = "tlSendList_Fail";
            // 
            // tlSendList_Fail_CNT
            // 
            resources.ApplyResources(this.tlSendList_Fail_CNT, "tlSendList_Fail_CNT");
            this.tlSendList_Fail_CNT.Name = "tlSendList_Fail_CNT";
            // 
            // gbSend_Bottom
            // 
            resources.ApplyResources(this.gbSend_Bottom, "gbSend_Bottom");
            this.gbSend_Bottom.Controls.Add(this.bSendListStop);
            this.gbSend_Bottom.Controls.Add(this.bSendList);
            this.gbSend_Bottom.Name = "gbSend_Bottom";
            this.gbSend_Bottom.TabStop = false;
            // 
            // bSendListStop
            // 
            resources.ApplyResources(this.bSendListStop, "bSendListStop");
            this.bSendListStop.Name = "bSendListStop";
            this.bSendListStop.UseVisualStyleBackColor = true;
            this.bSendListStop.Click += new System.EventHandler(this.bSendStop_Click);
            // 
            // bSendList
            // 
            resources.ApplyResources(this.bSendList, "bSendList");
            this.bSendList.Name = "bSendList";
            this.bSendList.UseVisualStyleBackColor = true;
            this.bSendList.Click += new System.EventHandler(this.bSend_Click);
            // 
            // nudLoop_Int
            // 
            resources.ApplyResources(this.nudLoop_Int, "nudLoop_Int");
            this.nudLoop_Int.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
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
            this.nudLoop_Int.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // nudLoop_CNT
            // 
            resources.ApplyResources(this.nudLoop_CNT, "nudLoop_CNT");
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
            this.nudLoop_CNT.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lLoop_Int
            // 
            resources.ApplyResources(this.lLoop_Int, "lLoop_Int");
            this.lLoop_Int.Name = "lLoop_Int";
            // 
            // lLoop_CNT
            // 
            resources.ApplyResources(this.lLoop_CNT, "lLoop_CNT");
            this.lLoop_CNT.Name = "lLoop_CNT";
            // 
            // txtUseSocket
            // 
            resources.ApplyResources(this.txtUseSocket, "txtUseSocket");
            this.txtUseSocket.Name = "txtUseSocket";
            // 
            // cbUseSocket
            // 
            resources.ApplyResources(this.cbUseSocket, "cbUseSocket");
            this.cbUseSocket.Name = "cbUseSocket";
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
            resources.ApplyResources(this.dgSendList, "dgSendList");
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
            this.dgSendList.MultiSelect = false;
            this.dgSendList.Name = "dgSendList";
            this.dgSendList.RowHeadersVisible = false;
            this.dgSendList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgSendList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgSendList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgSendList.RowTemplate.Height = 23;
            this.dgSendList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // cCheck
            // 
            this.cCheck.FalseValue = "0";
            resources.ApplyResources(this.cCheck, "cCheck");
            this.cCheck.IndeterminateValue = "0";
            this.cCheck.Name = "cCheck";
            this.cCheck.TrueValue = "1";
            // 
            // cIndex
            // 
            this.cIndex.DataPropertyName = "ID";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.cIndex, "cIndex");
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cNote
            // 
            this.cNote.DataPropertyName = "Remark";
            resources.ApplyResources(this.cNote, "cNote");
            this.cNote.Name = "cNote";
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIPTo
            // 
            this.cIPTo.DataPropertyName = "ToAddress";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cIPTo.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cIPTo, "cIPTo");
            this.cIPTo.Name = "cIPTo";
            this.cIPTo.ReadOnly = true;
            this.cIPTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "Len";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cLen, "cLen");
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cData
            // 
            this.cData.DataPropertyName = "Data";
            resources.ApplyResources(this.cData, "cData");
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.nudLoop_Int);
            this.groupBox1.Controls.Add(this.cbSelectAll);
            this.groupBox1.Controls.Add(this.nudLoop_CNT);
            this.groupBox1.Controls.Add(this.txtUseSocket);
            this.groupBox1.Controls.Add(this.cbUseSocket);
            this.groupBox1.Controls.Add(this.lLoop_CNT);
            this.groupBox1.Controls.Add(this.lLoop_Int);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbSelectAll
            // 
            resources.ApplyResources(this.cbSelectAll, "cbSelectAll");
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // Socket_SendList_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.dgSendList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSend_Bottom);
            this.Controls.Add(this.ssSocketSendList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_SendList_Form";
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
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.NumericUpDown nudLoop_Int;
        private System.Windows.Forms.NumericUpDown nudLoop_CNT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
    }
}
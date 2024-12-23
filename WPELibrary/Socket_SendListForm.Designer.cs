
namespace WPELibrary
{
    partial class Socket_SendListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_SendListForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpSendList = new System.Windows.Forms.TableLayoutPanel();
            this.ssSocketSendList = new System.Windows.Forms.StatusStrip();
            this.tlLoop_Send = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlLoop_Send_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Success_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendList_Fail_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvSendList = new System.Windows.Forms.DataGridView();
            this.cmsSendList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsSendList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_CleanUp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbLoopInt = new System.Windows.Forms.GroupBox();
            this.tlpLoopInt = new System.Windows.Forms.TableLayoutPanel();
            this.nudLoop_Int = new System.Windows.Forms.NumericUpDown();
            this.lLoop_Int = new System.Windows.Forms.Label();
            this.gbLoopTimes = new System.Windows.Forms.GroupBox();
            this.tlpLoopTimes = new System.Windows.Forms.TableLayoutPanel();
            this.nudLoop_CNT = new System.Windows.Forms.NumericUpDown();
            this.lLoop_CNT = new System.Windows.Forms.Label();
            this.gbUseSocket = new System.Windows.Forms.GroupBox();
            this.tlpUseSocket = new System.Windows.Forms.TableLayoutPanel();
            this.nudSocket = new System.Windows.Forms.NumericUpDown();
            this.cbUseSocket = new System.Windows.Forms.CheckBox();
            this.gbSelectALl = new System.Windows.Forms.GroupBox();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSendListStop = new System.Windows.Forms.Button();
            this.bSendList = new System.Windows.Forms.Button();
            this.bgwSendList = new System.ComponentModel.BackgroundWorker();
            this.cCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpSendList.SuspendLayout();
            this.ssSocketSendList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendList)).BeginInit();
            this.cmsSendList.SuspendLayout();
            this.tlpParameter.SuspendLayout();
            this.gbLoopInt.SuspendLayout();
            this.tlpLoopInt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).BeginInit();
            this.gbLoopTimes.SuspendLayout();
            this.tlpLoopTimes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).BeginInit();
            this.gbUseSocket.SuspendLayout();
            this.tlpUseSocket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSocket)).BeginInit();
            this.gbSelectALl.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSendList
            // 
            resources.ApplyResources(this.tlpSendList, "tlpSendList");
            this.tlpSendList.Controls.Add(this.ssSocketSendList, 0, 2);
            this.tlpSendList.Controls.Add(this.dgvSendList, 0, 0);
            this.tlpSendList.Controls.Add(this.tlpParameter, 0, 1);
            this.tlpSendList.Name = "tlpSendList";
            // 
            // ssSocketSendList
            // 
            resources.ApplyResources(this.ssSocketSendList, "ssSocketSendList");
            this.ssSocketSendList.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.tlLoop_Send.Name = "tlLoop_Send";
            resources.ApplyResources(this.tlLoop_Send, "tlLoop_Send");
            // 
            // tlLoop_Send_CNT
            // 
            resources.ApplyResources(this.tlLoop_Send_CNT, "tlLoop_Send_CNT");
            this.tlLoop_Send_CNT.Name = "tlLoop_Send_CNT";
            // 
            // tlSplit
            // 
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            resources.ApplyResources(this.tlSplit, "tlSplit");
            // 
            // tlSendList_Success
            // 
            this.tlSendList_Success.Name = "tlSendList_Success";
            resources.ApplyResources(this.tlSendList_Success, "tlSendList_Success");
            // 
            // tlSendList_Success_CNT
            // 
            resources.ApplyResources(this.tlSendList_Success_CNT, "tlSendList_Success_CNT");
            this.tlSendList_Success_CNT.ForeColor = System.Drawing.Color.Green;
            this.tlSendList_Success_CNT.Name = "tlSendList_Success_CNT";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // tlSendList_Fail
            // 
            this.tlSendList_Fail.Name = "tlSendList_Fail";
            resources.ApplyResources(this.tlSendList_Fail, "tlSendList_Fail");
            // 
            // tlSendList_Fail_CNT
            // 
            resources.ApplyResources(this.tlSendList_Fail_CNT, "tlSendList_Fail_CNT");
            this.tlSendList_Fail_CNT.ForeColor = System.Drawing.Color.Red;
            this.tlSendList_Fail_CNT.Name = "tlSendList_Fail_CNT";
            // 
            // dgvSendList
            // 
            this.dgvSendList.AllowUserToAddRows = false;
            this.dgvSendList.AllowUserToDeleteRows = false;
            this.dgvSendList.AllowUserToResizeRows = false;
            this.dgvSendList.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgvSendList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSendList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSendList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSendList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cCheck,
            this.cID,
            this.cNote,
            this.cSocket,
            this.cIPTo,
            this.cLen,
            this.cData});
            this.dgvSendList.ContextMenuStrip = this.cmsSendList;
            resources.ApplyResources(this.dgvSendList, "dgvSendList");
            this.dgvSendList.MultiSelect = false;
            this.dgvSendList.Name = "dgvSendList";
            this.dgvSendList.RowHeadersVisible = false;
            this.dgvSendList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvSendList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSendList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgvSendList.RowTemplate.Height = 23;
            this.dgvSendList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSendList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSendList_CellFormatting);
            // 
            // cmsSendList
            // 
            this.cmsSendList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsSendList_Delete,
            this.toolStripSeparator1,
            this.cmsSendList_CleanUp,
            this.toolStripSeparator2,
            this.cmsSendList_Save,
            this.toolStripSeparator3,
            this.cmsSendList_Load});
            this.cmsSendList.Name = "cmsBatchSend";
            resources.ApplyResources(this.cmsSendList, "cmsSendList");
            this.cmsSendList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSendList_ItemClicked);
            // 
            // cmsSendList_Delete
            // 
            this.cmsSendList_Delete.Image = global::WPELibrary.Properties.Resources.ListDel;
            resources.ApplyResources(this.cmsSendList_Delete, "cmsSendList_Delete");
            this.cmsSendList_Delete.Name = "cmsSendList_Delete";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // cmsSendList_CleanUp
            // 
            this.cmsSendList_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            resources.ApplyResources(this.cmsSendList_CleanUp, "cmsSendList_CleanUp");
            this.cmsSendList_CleanUp.Name = "cmsSendList_CleanUp";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // cmsSendList_Save
            // 
            this.cmsSendList_Save.Image = global::WPELibrary.Properties.Resources.saveas;
            resources.ApplyResources(this.cmsSendList_Save, "cmsSendList_Save");
            this.cmsSendList_Save.Name = "cmsSendList_Save";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // cmsSendList_Load
            // 
            this.cmsSendList_Load.Image = global::WPELibrary.Properties.Resources.openHS;
            resources.ApplyResources(this.cmsSendList_Load, "cmsSendList_Load");
            this.cmsSendList_Load.Name = "cmsSendList_Load";
            // 
            // tlpParameter
            // 
            resources.ApplyResources(this.tlpParameter, "tlpParameter");
            this.tlpParameter.Controls.Add(this.gbLoopInt, 3, 0);
            this.tlpParameter.Controls.Add(this.gbLoopTimes, 2, 0);
            this.tlpParameter.Controls.Add(this.gbUseSocket, 1, 0);
            this.tlpParameter.Controls.Add(this.gbSelectALl, 0, 0);
            this.tlpParameter.Controls.Add(this.tlpButton, 4, 0);
            this.tlpParameter.Name = "tlpParameter";
            // 
            // gbLoopInt
            // 
            this.gbLoopInt.Controls.Add(this.tlpLoopInt);
            resources.ApplyResources(this.gbLoopInt, "gbLoopInt");
            this.gbLoopInt.Name = "gbLoopInt";
            this.gbLoopInt.TabStop = false;
            // 
            // tlpLoopInt
            // 
            resources.ApplyResources(this.tlpLoopInt, "tlpLoopInt");
            this.tlpLoopInt.Controls.Add(this.nudLoop_Int, 1, 1);
            this.tlpLoopInt.Controls.Add(this.lLoop_Int, 0, 1);
            this.tlpLoopInt.Name = "tlpLoopInt";
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
            this.nudLoop_Int.Name = "nudLoop_Int";
            this.nudLoop_Int.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lLoop_Int
            // 
            resources.ApplyResources(this.lLoop_Int, "lLoop_Int");
            this.lLoop_Int.Name = "lLoop_Int";
            // 
            // gbLoopTimes
            // 
            this.gbLoopTimes.Controls.Add(this.tlpLoopTimes);
            resources.ApplyResources(this.gbLoopTimes, "gbLoopTimes");
            this.gbLoopTimes.Name = "gbLoopTimes";
            this.gbLoopTimes.TabStop = false;
            // 
            // tlpLoopTimes
            // 
            resources.ApplyResources(this.tlpLoopTimes, "tlpLoopTimes");
            this.tlpLoopTimes.Controls.Add(this.nudLoop_CNT, 1, 1);
            this.tlpLoopTimes.Controls.Add(this.lLoop_CNT, 0, 1);
            this.tlpLoopTimes.Name = "tlpLoopTimes";
            // 
            // nudLoop_CNT
            // 
            resources.ApplyResources(this.nudLoop_CNT, "nudLoop_CNT");
            this.nudLoop_CNT.Maximum = new decimal(new int[] {
            999999,
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
            // lLoop_CNT
            // 
            resources.ApplyResources(this.lLoop_CNT, "lLoop_CNT");
            this.lLoop_CNT.Name = "lLoop_CNT";
            // 
            // gbUseSocket
            // 
            this.gbUseSocket.Controls.Add(this.tlpUseSocket);
            resources.ApplyResources(this.gbUseSocket, "gbUseSocket");
            this.gbUseSocket.Name = "gbUseSocket";
            this.gbUseSocket.TabStop = false;
            // 
            // tlpUseSocket
            // 
            resources.ApplyResources(this.tlpUseSocket, "tlpUseSocket");
            this.tlpUseSocket.Controls.Add(this.nudSocket, 1, 1);
            this.tlpUseSocket.Controls.Add(this.cbUseSocket, 0, 1);
            this.tlpUseSocket.Name = "tlpUseSocket";
            // 
            // nudSocket
            // 
            resources.ApplyResources(this.nudSocket, "nudSocket");
            this.nudSocket.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudSocket.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSocket.Name = "nudSocket";
            this.nudSocket.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbUseSocket
            // 
            resources.ApplyResources(this.cbUseSocket, "cbUseSocket");
            this.cbUseSocket.Name = "cbUseSocket";
            this.cbUseSocket.UseVisualStyleBackColor = true;
            // 
            // gbSelectALl
            // 
            this.gbSelectALl.Controls.Add(this.cbSelectAll);
            resources.ApplyResources(this.gbSelectALl, "gbSelectALl");
            this.gbSelectALl.Name = "gbSelectALl";
            this.gbSelectALl.TabStop = false;
            // 
            // cbSelectAll
            // 
            resources.ApplyResources(this.cbSelectAll, "cbSelectAll");
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bSendListStop, 2, 1);
            this.tlpButton.Controls.Add(this.bSendList, 0, 1);
            this.tlpButton.Name = "tlpButton";
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
            // bgwSendList
            // 
            this.bgwSendList.WorkerSupportsCancellation = true;
            this.bgwSendList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendList_DoWork);
            this.bgwSendList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendList_RunWorkerCompleted);
            // 
            // cCheck
            // 
            this.cCheck.FalseValue = "0";
            resources.ApplyResources(this.cCheck, "cCheck");
            this.cCheck.IndeterminateValue = "0";
            this.cCheck.Name = "cCheck";
            this.cCheck.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCheck.TrueValue = "1";
            // 
            // cID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cID, "cID");
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cNote
            // 
            this.cNote.DataPropertyName = "Remark";
            resources.ApplyResources(this.cNote, "cNote");
            this.cNote.Name = "cNote";
            this.cNote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cNote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIPTo
            // 
            this.cIPTo.DataPropertyName = "ToAddress";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cIPTo.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cIPTo, "cIPTo");
            this.cIPTo.Name = "cIPTo";
            this.cIPTo.ReadOnly = true;
            this.cIPTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "Len";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.cLen, "cLen");
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cData
            // 
            this.cData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cData.DataPropertyName = "Data";
            resources.ApplyResources(this.cData, "cData");
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Socket_SendListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSendList);
            this.DoubleBuffered = true;
            this.Name = "Socket_SendListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SocketSendList_Form_FormClosed);
            this.Load += new System.EventHandler(this.SocketSendList_Form_Load);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendList.PerformLayout();
            this.ssSocketSendList.ResumeLayout(false);
            this.ssSocketSendList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendList)).EndInit();
            this.cmsSendList.ResumeLayout(false);
            this.tlpParameter.ResumeLayout(false);
            this.gbLoopInt.ResumeLayout(false);
            this.tlpLoopInt.ResumeLayout(false);
            this.tlpLoopInt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).EndInit();
            this.gbLoopTimes.ResumeLayout(false);
            this.tlpLoopTimes.ResumeLayout(false);
            this.tlpLoopTimes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).EndInit();
            this.gbUseSocket.ResumeLayout(false);
            this.tlpUseSocket.ResumeLayout(false);
            this.tlpUseSocket.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSocket)).EndInit();
            this.gbSelectALl.ResumeLayout(false);
            this.gbSelectALl.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSendList;
        private System.Windows.Forms.DataGridView dgvSendList;
        private System.Windows.Forms.StatusStrip ssSocketSendList;
        private System.Windows.Forms.ToolStripStatusLabel tlLoop_Send;
        private System.Windows.Forms.ToolStripStatusLabel tlLoop_Send_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Success_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSendList_Fail_CNT;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.GroupBox gbSelectALl;
        private System.Windows.Forms.GroupBox gbLoopInt;
        private System.Windows.Forms.GroupBox gbLoopTimes;
        private System.Windows.Forms.GroupBox gbUseSocket;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.TableLayoutPanel tlpLoopInt;
        private System.Windows.Forms.TableLayoutPanel tlpLoopTimes;
        private System.Windows.Forms.TableLayoutPanel tlpUseSocket;
        private System.Windows.Forms.CheckBox cbUseSocket;
        private System.Windows.Forms.Label lLoop_CNT;
        private System.Windows.Forms.NumericUpDown nudLoop_CNT;
        private System.Windows.Forms.Label lLoop_Int;
        private System.Windows.Forms.NumericUpDown nudLoop_Int;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bSendList;
        private System.Windows.Forms.Button bSendListStop;
        private System.ComponentModel.BackgroundWorker bgwSendList;
        private System.Windows.Forms.ContextMenuStrip cmsSendList;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_CleanUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Load;
        private System.Windows.Forms.NumericUpDown nudSocket;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
    }
}
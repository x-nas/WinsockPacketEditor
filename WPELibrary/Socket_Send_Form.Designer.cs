
namespace WPELibrary
{
    partial class Socket_Send_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_Send_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ssSocketSend = new System.Windows.Forms.StatusStrip();
            this.tlSendPacket = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendPacket_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.bSendStop = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.gbSend2 = new System.Windows.Forms.GroupBox();
            this.txtSend_Port = new System.Windows.Forms.TextBox();
            this.lSend_Port = new System.Windows.Forms.Label();
            this.txtSend_IP = new System.Windows.Forms.TextBox();
            this.lSend_IP = new System.Windows.Forms.Label();
            this.gbSend1 = new System.Windows.Forms.GroupBox();
            this.txtSend_Len = new System.Windows.Forms.TextBox();
            this.lSend_Len = new System.Windows.Forms.Label();
            this.txtSend_Socket = new System.Windows.Forms.TextBox();
            this.lSend_Socket = new System.Windows.Forms.Label();
            this.gbSend_Bottom = new System.Windows.Forms.GroupBox();
            this.nudLoop_Int = new System.Windows.Forms.NumericUpDown();
            this.nudLoop_CNT = new System.Windows.Forms.NumericUpDown();
            this.lLoop_Int = new System.Windows.Forms.Label();
            this.lLoop_CNT = new System.Windows.Forms.Label();
            this.bgwSendPacket = new System.ComponentModel.BackgroundWorker();
            this.cmsSocketSend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.tSend = new System.Windows.Forms.Timer(this.components);
            this.gbStep = new System.Windows.Forms.GroupBox();
            this.lStepLen_Value = new System.Windows.Forms.Label();
            this.lStepIndex_Value = new System.Windows.Forms.Label();
            this.nudStepIndex = new System.Windows.Forms.NumericUpDown();
            this.nudStepLen = new System.Windows.Forms.NumericUpDown();
            this.lStepLen = new System.Windows.Forms.Label();
            this.cbStep = new System.Windows.Forms.CheckBox();
            this.pSend_Top = new System.Windows.Forms.Panel();
            this.dgvSocketSend = new System.Windows.Forms.DataGridView();
            this.ssSocketSend.SuspendLayout();
            this.gbSend2.SuspendLayout();
            this.gbSend1.SuspendLayout();
            this.gbSend_Bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).BeginInit();
            this.cmsSocketSend.SuspendLayout();
            this.gbStep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepLen)).BeginInit();
            this.pSend_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketSend)).BeginInit();
            this.SuspendLayout();
            // 
            // ssSocketSend
            // 
            this.ssSocketSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlSendPacket,
            this.tlSendPacket_CNT,
            this.tlSplit,
            this.tlSend_Success,
            this.tlSend_Success_CNT,
            this.toolStripStatusLabel3,
            this.tlSend_Fail,
            this.tlSend_Fail_CNT});
            resources.ApplyResources(this.ssSocketSend, "ssSocketSend");
            this.ssSocketSend.Name = "ssSocketSend";
            this.ssSocketSend.SizingGrip = false;
            // 
            // tlSendPacket
            // 
            this.tlSendPacket.Name = "tlSendPacket";
            resources.ApplyResources(this.tlSendPacket, "tlSendPacket");
            // 
            // tlSendPacket_CNT
            // 
            this.tlSendPacket_CNT.Name = "tlSendPacket_CNT";
            resources.ApplyResources(this.tlSendPacket_CNT, "tlSendPacket_CNT");
            // 
            // tlSplit
            // 
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            resources.ApplyResources(this.tlSplit, "tlSplit");
            // 
            // tlSend_Success
            // 
            this.tlSend_Success.Name = "tlSend_Success";
            resources.ApplyResources(this.tlSend_Success, "tlSend_Success");
            // 
            // tlSend_Success_CNT
            // 
            this.tlSend_Success_CNT.Name = "tlSend_Success_CNT";
            resources.ApplyResources(this.tlSend_Success_CNT, "tlSend_Success_CNT");
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // tlSend_Fail
            // 
            this.tlSend_Fail.Name = "tlSend_Fail";
            resources.ApplyResources(this.tlSend_Fail, "tlSend_Fail");
            // 
            // tlSend_Fail_CNT
            // 
            this.tlSend_Fail_CNT.Name = "tlSend_Fail_CNT";
            resources.ApplyResources(this.tlSend_Fail_CNT, "tlSend_Fail_CNT");
            // 
            // bSendStop
            // 
            resources.ApplyResources(this.bSendStop, "bSendStop");
            this.bSendStop.Name = "bSendStop";
            this.bSendStop.UseVisualStyleBackColor = true;
            this.bSendStop.Click += new System.EventHandler(this.bSendStop_Click);
            // 
            // bSend
            // 
            resources.ApplyResources(this.bSend, "bSend");
            this.bSend.Name = "bSend";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // gbSend2
            // 
            this.gbSend2.Controls.Add(this.txtSend_Port);
            this.gbSend2.Controls.Add(this.lSend_Port);
            this.gbSend2.Controls.Add(this.txtSend_IP);
            this.gbSend2.Controls.Add(this.lSend_IP);
            resources.ApplyResources(this.gbSend2, "gbSend2");
            this.gbSend2.Name = "gbSend2";
            this.gbSend2.TabStop = false;
            // 
            // txtSend_Port
            // 
            resources.ApplyResources(this.txtSend_Port, "txtSend_Port");
            this.txtSend_Port.Name = "txtSend_Port";
            this.txtSend_Port.ReadOnly = true;
            // 
            // lSend_Port
            // 
            resources.ApplyResources(this.lSend_Port, "lSend_Port");
            this.lSend_Port.Name = "lSend_Port";
            // 
            // txtSend_IP
            // 
            resources.ApplyResources(this.txtSend_IP, "txtSend_IP");
            this.txtSend_IP.Name = "txtSend_IP";
            this.txtSend_IP.ReadOnly = true;
            // 
            // lSend_IP
            // 
            resources.ApplyResources(this.lSend_IP, "lSend_IP");
            this.lSend_IP.Name = "lSend_IP";
            // 
            // gbSend1
            // 
            this.gbSend1.Controls.Add(this.txtSend_Len);
            this.gbSend1.Controls.Add(this.lSend_Len);
            this.gbSend1.Controls.Add(this.txtSend_Socket);
            this.gbSend1.Controls.Add(this.lSend_Socket);
            resources.ApplyResources(this.gbSend1, "gbSend1");
            this.gbSend1.Name = "gbSend1";
            this.gbSend1.TabStop = false;
            // 
            // txtSend_Len
            // 
            resources.ApplyResources(this.txtSend_Len, "txtSend_Len");
            this.txtSend_Len.Name = "txtSend_Len";
            // 
            // lSend_Len
            // 
            resources.ApplyResources(this.lSend_Len, "lSend_Len");
            this.lSend_Len.Name = "lSend_Len";
            // 
            // txtSend_Socket
            // 
            resources.ApplyResources(this.txtSend_Socket, "txtSend_Socket");
            this.txtSend_Socket.Name = "txtSend_Socket";
            // 
            // lSend_Socket
            // 
            resources.ApplyResources(this.lSend_Socket, "lSend_Socket");
            this.lSend_Socket.Name = "lSend_Socket";
            // 
            // gbSend_Bottom
            // 
            this.gbSend_Bottom.Controls.Add(this.nudLoop_Int);
            this.gbSend_Bottom.Controls.Add(this.nudLoop_CNT);
            this.gbSend_Bottom.Controls.Add(this.lLoop_Int);
            this.gbSend_Bottom.Controls.Add(this.lLoop_CNT);
            resources.ApplyResources(this.gbSend_Bottom, "gbSend_Bottom");
            this.gbSend_Bottom.Name = "gbSend_Bottom";
            this.gbSend_Bottom.TabStop = false;
            // 
            // nudLoop_Int
            // 
            this.nudLoop_Int.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            resources.ApplyResources(this.nudLoop_Int, "nudLoop_Int");
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
            // bgwSendPacket
            // 
            this.bgwSendPacket.WorkerSupportsCancellation = true;
            this.bgwSendPacket.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendPacket_DoWork);
            this.bgwSendPacket.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendPacket_RunWorkerCompleted);
            // 
            // cmsSocketSend
            // 
            this.cmsSocketSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBatchSend});
            this.cmsSocketSend.Name = "cmsSocketSend";
            resources.ApplyResources(this.cmsSocketSend, "cmsSocketSend");
            this.cmsSocketSend.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketSend_ItemClicked);
            // 
            // tsmiBatchSend
            // 
            this.tsmiBatchSend.Name = "tsmiBatchSend";
            resources.ApplyResources(this.tsmiBatchSend, "tsmiBatchSend");
            // 
            // tSend
            // 
            this.tSend.Enabled = true;
            this.tSend.Interval = 1000;
            this.tSend.Tick += new System.EventHandler(this.tSend_Tick);
            // 
            // gbStep
            // 
            this.gbStep.Controls.Add(this.lStepLen_Value);
            this.gbStep.Controls.Add(this.lStepIndex_Value);
            this.gbStep.Controls.Add(this.nudStepIndex);
            this.gbStep.Controls.Add(this.nudStepLen);
            this.gbStep.Controls.Add(this.lStepLen);
            this.gbStep.Controls.Add(this.cbStep);
            resources.ApplyResources(this.gbStep, "gbStep");
            this.gbStep.Name = "gbStep";
            this.gbStep.TabStop = false;
            // 
            // lStepLen_Value
            // 
            resources.ApplyResources(this.lStepLen_Value, "lStepLen_Value");
            this.lStepLen_Value.Name = "lStepLen_Value";
            // 
            // lStepIndex_Value
            // 
            resources.ApplyResources(this.lStepIndex_Value, "lStepIndex_Value");
            this.lStepIndex_Value.Name = "lStepIndex_Value";
            // 
            // nudStepIndex
            // 
            resources.ApplyResources(this.nudStepIndex, "nudStepIndex");
            this.nudStepIndex.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudStepIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepIndex.Name = "nudStepIndex";
            this.nudStepIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepIndex.ValueChanged += new System.EventHandler(this.nudStepIndex_ValueChanged);
            // 
            // nudStepLen
            // 
            resources.ApplyResources(this.nudStepLen, "nudStepLen");
            this.nudStepLen.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudStepLen.Name = "nudStepLen";
            this.nudStepLen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepLen.ValueChanged += new System.EventHandler(this.nudStepLen_ValueChanged);
            // 
            // lStepLen
            // 
            resources.ApplyResources(this.lStepLen, "lStepLen");
            this.lStepLen.Name = "lStepLen";
            // 
            // cbStep
            // 
            resources.ApplyResources(this.cbStep, "cbStep");
            this.cbStep.Name = "cbStep";
            this.cbStep.UseVisualStyleBackColor = true;
            // 
            // pSend_Top
            // 
            this.pSend_Top.Controls.Add(this.dgvSocketSend);
            resources.ApplyResources(this.pSend_Top, "pSend_Top");
            this.pSend_Top.Name = "pSend_Top";
            // 
            // dgvSocketSend
            // 
            this.dgvSocketSend.AllowUserToAddRows = false;
            this.dgvSocketSend.AllowUserToDeleteRows = false;
            this.dgvSocketSend.AllowUserToResizeColumns = false;
            this.dgvSocketSend.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketSend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSocketSend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSocketSend.ContextMenuStrip = this.cmsSocketSend;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketSend.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvSocketSend, "dgvSocketSend");
            this.dgvSocketSend.MultiSelect = false;
            this.dgvSocketSend.Name = "dgvSocketSend";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketSend.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSocketSend.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvSocketSend.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSocketSend.RowTemplate.Height = 23;
            this.dgvSocketSend.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSocketSend_CellClick);
            this.dgvSocketSend.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSocketSend_ColumnAdded);
            this.dgvSocketSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSocketSend_KeyDown);
            // 
            // Socket_Send_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pSend_Top);
            this.Controls.Add(this.gbStep);
            this.Controls.Add(this.bSendStop);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.gbSend2);
            this.Controls.Add(this.gbSend1);
            this.Controls.Add(this.gbSend_Bottom);
            this.Controls.Add(this.ssSocketSend);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_Send_Form";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SocketSend_Form_Load);
            this.ssSocketSend.ResumeLayout(false);
            this.ssSocketSend.PerformLayout();
            this.gbSend2.ResumeLayout(false);
            this.gbSend2.PerformLayout();
            this.gbSend1.ResumeLayout(false);
            this.gbSend1.PerformLayout();
            this.gbSend_Bottom.ResumeLayout(false);
            this.gbSend_Bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).EndInit();
            this.cmsSocketSend.ResumeLayout(false);
            this.gbStep.ResumeLayout(false);
            this.gbStep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepLen)).EndInit();
            this.pSend_Top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketSend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip ssSocketSend;
        private System.Windows.Forms.Button bSendStop;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.GroupBox gbSend2;
        private System.Windows.Forms.TextBox txtSend_Port;
        private System.Windows.Forms.Label lSend_Port;
        private System.Windows.Forms.TextBox txtSend_IP;
        private System.Windows.Forms.Label lSend_IP;
        private System.Windows.Forms.GroupBox gbSend1;
        private System.Windows.Forms.TextBox txtSend_Len;
        private System.Windows.Forms.Label lSend_Len;
        private System.Windows.Forms.TextBox txtSend_Socket;
        private System.Windows.Forms.Label lSend_Socket;
        private System.Windows.Forms.GroupBox gbSend_Bottom;
        private System.Windows.Forms.Label lLoop_Int;
        private System.Windows.Forms.Label lLoop_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSendPacket;
        private System.Windows.Forms.ToolStripStatusLabel tlSendPacket_CNT;
        private System.ComponentModel.BackgroundWorker bgwSendPacket;
        private System.Windows.Forms.Timer tSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail_CNT;
        private System.Windows.Forms.ContextMenuStrip cmsSocketSend;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchSend;
        private System.Windows.Forms.GroupBox gbStep;
        private System.Windows.Forms.NumericUpDown nudStepLen;
        private System.Windows.Forms.Label lStepLen;
        private System.Windows.Forms.CheckBox cbStep;
        private System.Windows.Forms.Panel pSend_Top;
        private System.Windows.Forms.Label lStepIndex_Value;
        private System.Windows.Forms.NumericUpDown nudStepIndex;
        private System.Windows.Forms.Label lStepLen_Value;
        private System.Windows.Forms.NumericUpDown nudLoop_CNT;
        private System.Windows.Forms.NumericUpDown nudLoop_Int;
        private System.Windows.Forms.DataGridView dgvSocketSend;
    }
}
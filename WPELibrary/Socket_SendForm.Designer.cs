
namespace WPELibrary
{
    partial class Socket_SendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_SendForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpSendForm = new System.Windows.Forms.TableLayoutPanel();
            this.ssSocketSend = new System.Windows.Forms.StatusStrip();
            this.tlSendPacket = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendPacket_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvSocketSend = new System.Windows.Forms.DataGridView();
            this.cmsSocketSend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAddToFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSendStop = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.gbSendForm4 = new System.Windows.Forms.GroupBox();
            this.tlpSendForm4 = new System.Windows.Forms.TableLayoutPanel();
            this.nudLoop_Int = new System.Windows.Forms.NumericUpDown();
            this.lLoop_Int = new System.Windows.Forms.Label();
            this.nudLoop_CNT = new System.Windows.Forms.NumericUpDown();
            this.lLoop_CNT = new System.Windows.Forms.Label();
            this.gbSendForm3 = new System.Windows.Forms.GroupBox();
            this.tlpSendForm3 = new System.Windows.Forms.TableLayoutPanel();
            this.lStepLen_Value = new System.Windows.Forms.Label();
            this.nudStepLen = new System.Windows.Forms.NumericUpDown();
            this.lStepLen = new System.Windows.Forms.Label();
            this.lStepIndex_Value = new System.Windows.Forms.Label();
            this.nudStepIndex = new System.Windows.Forms.NumericUpDown();
            this.cbStep = new System.Windows.Forms.CheckBox();
            this.gbSendForm2 = new System.Windows.Forms.GroupBox();
            this.tlpSendForm2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSend_Port = new System.Windows.Forms.TextBox();
            this.lSend_Port = new System.Windows.Forms.Label();
            this.txtSend_IP = new System.Windows.Forms.TextBox();
            this.lSend_IP = new System.Windows.Forms.Label();
            this.gbSendForm1 = new System.Windows.Forms.GroupBox();
            this.tlpSendForm1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSend_Len = new System.Windows.Forms.TextBox();
            this.lSend_Len = new System.Windows.Forms.Label();
            this.txtSend_Socket = new System.Windows.Forms.TextBox();
            this.lSend_Socket = new System.Windows.Forms.Label();
            this.bgwSendPacket = new System.ComponentModel.BackgroundWorker();
            this.tlpSendForm.SuspendLayout();
            this.ssSocketSend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketSend)).BeginInit();
            this.cmsSocketSend.SuspendLayout();
            this.tlpParameter.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.gbSendForm4.SuspendLayout();
            this.tlpSendForm4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).BeginInit();
            this.gbSendForm3.SuspendLayout();
            this.tlpSendForm3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepLen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepIndex)).BeginInit();
            this.gbSendForm2.SuspendLayout();
            this.tlpSendForm2.SuspendLayout();
            this.gbSendForm1.SuspendLayout();
            this.tlpSendForm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSendForm
            // 
            resources.ApplyResources(this.tlpSendForm, "tlpSendForm");
            this.tlpSendForm.Controls.Add(this.ssSocketSend, 0, 2);
            this.tlpSendForm.Controls.Add(this.dgvSocketSend, 0, 0);
            this.tlpSendForm.Controls.Add(this.tlpParameter, 0, 1);
            this.tlpSendForm.Name = "tlpSendForm";
            // 
            // ssSocketSend
            // 
            resources.ApplyResources(this.ssSocketSend, "ssSocketSend");
            this.ssSocketSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlSendPacket,
            this.tlSendPacket_CNT,
            this.tlSplit,
            this.tlSend_Success,
            this.tlSend_Success_CNT,
            this.toolStripStatusLabel3,
            this.tlSend_Fail,
            this.tlSend_Fail_CNT});
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
            resources.ApplyResources(this.tlSendPacket_CNT, "tlSendPacket_CNT");
            this.tlSendPacket_CNT.Name = "tlSendPacket_CNT";
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
            resources.ApplyResources(this.tlSend_Success_CNT, "tlSend_Success_CNT");
            this.tlSend_Success_CNT.ForeColor = System.Drawing.Color.Green;
            this.tlSend_Success_CNT.Name = "tlSend_Success_CNT";
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
            resources.ApplyResources(this.tlSend_Fail_CNT, "tlSend_Fail_CNT");
            this.tlSend_Fail_CNT.ForeColor = System.Drawing.Color.Red;
            this.tlSend_Fail_CNT.Name = "tlSend_Fail_CNT";
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
            this.dgvSocketSend.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvSocketSend.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSocketSend.RowTemplate.Height = 30;
            this.dgvSocketSend.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSocketSend_CellClick);
            this.dgvSocketSend.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSocketSend_CellEndEdit);
            this.dgvSocketSend.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSocketSend_ColumnAdded);
            this.dgvSocketSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSocketSend_KeyDown);
            // 
            // cmsSocketSend
            // 
            this.cmsSocketSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBatchSend,
            this.toolStripSeparator1,
            this.tsmiAddToFilter});
            this.cmsSocketSend.Name = "cmsSocketSend";
            resources.ApplyResources(this.cmsSocketSend, "cmsSocketSend");
            this.cmsSocketSend.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketSend_ItemClicked);
            // 
            // tsmiBatchSend
            // 
            this.tsmiBatchSend.Name = "tsmiBatchSend";
            resources.ApplyResources(this.tsmiBatchSend, "tsmiBatchSend");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsmiAddToFilter
            // 
            this.tsmiAddToFilter.Name = "tsmiAddToFilter";
            resources.ApplyResources(this.tsmiAddToFilter, "tsmiAddToFilter");
            // 
            // tlpParameter
            // 
            resources.ApplyResources(this.tlpParameter, "tlpParameter");
            this.tlpParameter.Controls.Add(this.tlpButton, 4, 0);
            this.tlpParameter.Controls.Add(this.gbSendForm4, 3, 0);
            this.tlpParameter.Controls.Add(this.gbSendForm3, 2, 0);
            this.tlpParameter.Controls.Add(this.gbSendForm2, 1, 0);
            this.tlpParameter.Controls.Add(this.gbSendForm1, 0, 0);
            this.tlpParameter.Name = "tlpParameter";
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bSendStop, 0, 2);
            this.tlpButton.Controls.Add(this.bSend, 0, 0);
            this.tlpButton.Name = "tlpButton";
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
            // gbSendForm4
            // 
            this.gbSendForm4.Controls.Add(this.tlpSendForm4);
            resources.ApplyResources(this.gbSendForm4, "gbSendForm4");
            this.gbSendForm4.Name = "gbSendForm4";
            this.gbSendForm4.TabStop = false;
            // 
            // tlpSendForm4
            // 
            resources.ApplyResources(this.tlpSendForm4, "tlpSendForm4");
            this.tlpSendForm4.Controls.Add(this.nudLoop_Int, 1, 1);
            this.tlpSendForm4.Controls.Add(this.lLoop_Int, 0, 1);
            this.tlpSendForm4.Controls.Add(this.nudLoop_CNT, 1, 0);
            this.tlpSendForm4.Controls.Add(this.lLoop_CNT, 0, 0);
            this.tlpSendForm4.Name = "tlpSendForm4";
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
            // gbSendForm3
            // 
            this.gbSendForm3.Controls.Add(this.tlpSendForm3);
            resources.ApplyResources(this.gbSendForm3, "gbSendForm3");
            this.gbSendForm3.Name = "gbSendForm3";
            this.gbSendForm3.TabStop = false;
            // 
            // tlpSendForm3
            // 
            resources.ApplyResources(this.tlpSendForm3, "tlpSendForm3");
            this.tlpSendForm3.Controls.Add(this.lStepLen_Value, 2, 1);
            this.tlpSendForm3.Controls.Add(this.nudStepLen, 1, 1);
            this.tlpSendForm3.Controls.Add(this.lStepLen, 0, 1);
            this.tlpSendForm3.Controls.Add(this.lStepIndex_Value, 2, 0);
            this.tlpSendForm3.Controls.Add(this.nudStepIndex, 1, 0);
            this.tlpSendForm3.Controls.Add(this.cbStep, 0, 0);
            this.tlpSendForm3.Name = "tlpSendForm3";
            // 
            // lStepLen_Value
            // 
            resources.ApplyResources(this.lStepLen_Value, "lStepLen_Value");
            this.lStepLen_Value.Name = "lStepLen_Value";
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
            // cbStep
            // 
            resources.ApplyResources(this.cbStep, "cbStep");
            this.cbStep.Name = "cbStep";
            this.cbStep.UseVisualStyleBackColor = true;
            // 
            // gbSendForm2
            // 
            this.gbSendForm2.Controls.Add(this.tlpSendForm2);
            resources.ApplyResources(this.gbSendForm2, "gbSendForm2");
            this.gbSendForm2.Name = "gbSendForm2";
            this.gbSendForm2.TabStop = false;
            // 
            // tlpSendForm2
            // 
            resources.ApplyResources(this.tlpSendForm2, "tlpSendForm2");
            this.tlpSendForm2.Controls.Add(this.txtSend_Port, 1, 1);
            this.tlpSendForm2.Controls.Add(this.lSend_Port, 0, 1);
            this.tlpSendForm2.Controls.Add(this.txtSend_IP, 1, 0);
            this.tlpSendForm2.Controls.Add(this.lSend_IP, 0, 0);
            this.tlpSendForm2.Name = "tlpSendForm2";
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
            // gbSendForm1
            // 
            this.gbSendForm1.Controls.Add(this.tlpSendForm1);
            resources.ApplyResources(this.gbSendForm1, "gbSendForm1");
            this.gbSendForm1.Name = "gbSendForm1";
            this.gbSendForm1.TabStop = false;
            // 
            // tlpSendForm1
            // 
            resources.ApplyResources(this.tlpSendForm1, "tlpSendForm1");
            this.tlpSendForm1.Controls.Add(this.txtSend_Len, 1, 1);
            this.tlpSendForm1.Controls.Add(this.lSend_Len, 0, 1);
            this.tlpSendForm1.Controls.Add(this.txtSend_Socket, 1, 0);
            this.tlpSendForm1.Controls.Add(this.lSend_Socket, 0, 0);
            this.tlpSendForm1.Name = "tlpSendForm1";
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
            // bgwSendPacket
            // 
            this.bgwSendPacket.WorkerSupportsCancellation = true;
            this.bgwSendPacket.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendPacket_DoWork);
            this.bgwSendPacket.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendPacket_RunWorkerCompleted);
            // 
            // Socket_SendForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSendForm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_SendForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SocketSend_Form_Load);
            this.tlpSendForm.ResumeLayout(false);
            this.tlpSendForm.PerformLayout();
            this.ssSocketSend.ResumeLayout(false);
            this.ssSocketSend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketSend)).EndInit();
            this.cmsSocketSend.ResumeLayout(false);
            this.tlpParameter.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.gbSendForm4.ResumeLayout(false);
            this.tlpSendForm4.ResumeLayout(false);
            this.tlpSendForm4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_Int)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).EndInit();
            this.gbSendForm3.ResumeLayout(false);
            this.tlpSendForm3.ResumeLayout(false);
            this.tlpSendForm3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepLen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepIndex)).EndInit();
            this.gbSendForm2.ResumeLayout(false);
            this.tlpSendForm2.ResumeLayout(false);
            this.tlpSendForm2.PerformLayout();
            this.gbSendForm1.ResumeLayout(false);
            this.tlpSendForm1.ResumeLayout(false);
            this.tlpSendForm1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSendForm;
        private System.Windows.Forms.DataGridView dgvSocketSend;
        private System.Windows.Forms.StatusStrip ssSocketSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSendPacket;
        private System.Windows.Forms.ToolStripStatusLabel tlSendPacket_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail_CNT;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.GroupBox gbSendForm1;
        private System.Windows.Forms.GroupBox gbSendForm4;
        private System.Windows.Forms.GroupBox gbSendForm3;
        private System.Windows.Forms.GroupBox gbSendForm2;
        private System.Windows.Forms.TableLayoutPanel tlpSendForm1;
        private System.Windows.Forms.TableLayoutPanel tlpSendForm4;
        private System.Windows.Forms.TableLayoutPanel tlpSendForm3;
        private System.Windows.Forms.TableLayoutPanel tlpSendForm2;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Label lSend_Socket;
        private System.Windows.Forms.TextBox txtSend_Socket;
        private System.Windows.Forms.Label lSend_Len;
        private System.Windows.Forms.TextBox txtSend_Len;
        private System.Windows.Forms.Label lSend_IP;
        private System.Windows.Forms.TextBox txtSend_IP;
        private System.Windows.Forms.Label lSend_Port;
        private System.Windows.Forms.TextBox txtSend_Port;
        private System.Windows.Forms.CheckBox cbStep;
        private System.Windows.Forms.NumericUpDown nudStepIndex;
        private System.Windows.Forms.Label lStepIndex_Value;
        private System.Windows.Forms.Label lStepLen;
        private System.Windows.Forms.NumericUpDown nudStepLen;
        private System.Windows.Forms.Label lStepLen_Value;
        private System.Windows.Forms.Label lLoop_CNT;
        private System.Windows.Forms.NumericUpDown nudLoop_CNT;
        private System.Windows.Forms.Label lLoop_Int;
        private System.Windows.Forms.NumericUpDown nudLoop_Int;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bSendStop;
        private System.Windows.Forms.ContextMenuStrip cmsSocketSend;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddToFilter;
        private System.ComponentModel.BackgroundWorker bgwSendPacket;
    }
}
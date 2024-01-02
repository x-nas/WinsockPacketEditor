
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_Send_Form));
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
            this.ssSocketSend.Location = new System.Drawing.Point(0, 177);
            this.ssSocketSend.Name = "ssSocketSend";
            this.ssSocketSend.Size = new System.Drawing.Size(745, 22);
            this.ssSocketSend.SizingGrip = false;
            this.ssSocketSend.TabIndex = 34;
            // 
            // tlSendPacket
            // 
            this.tlSendPacket.Name = "tlSendPacket";
            this.tlSendPacket.Size = new System.Drawing.Size(47, 17);
            this.tlSendPacket.Text = "已发送:";
            // 
            // tlSendPacket_CNT
            // 
            this.tlSendPacket_CNT.Name = "tlSendPacket_CNT";
            this.tlSendPacket_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSendPacket_CNT.Text = "0";
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
            this.tlSend_Success.Size = new System.Drawing.Size(35, 17);
            this.tlSend_Success.Text = "成功:";
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
            this.tlSend_Fail.Size = new System.Drawing.Size(35, 17);
            this.tlSend_Fail.Text = "失败:";
            // 
            // tlSend_Fail_CNT
            // 
            this.tlSend_Fail_CNT.Name = "tlSend_Fail_CNT";
            this.tlSend_Fail_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSend_Fail_CNT.Text = "0";
            // 
            // bSendStop
            // 
            this.bSendStop.Location = new System.Drawing.Point(657, 143);
            this.bSendStop.Name = "bSendStop";
            this.bSendStop.Size = new System.Drawing.Size(76, 25);
            this.bSendStop.TabIndex = 39;
            this.bSendStop.Text = "停 止 (&T)";
            this.bSendStop.UseVisualStyleBackColor = true;
            this.bSendStop.Click += new System.EventHandler(this.bSendStop_Click);
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(657, 110);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(76, 25);
            this.bSend.TabIndex = 38;
            this.bSend.Text = "发 送 (&F)";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // gbSend2
            // 
            this.gbSend2.Controls.Add(this.txtSend_Port);
            this.gbSend2.Controls.Add(this.lSend_Port);
            this.gbSend2.Controls.Add(this.txtSend_IP);
            this.gbSend2.Controls.Add(this.lSend_IP);
            this.gbSend2.Location = new System.Drawing.Point(140, 99);
            this.gbSend2.Name = "gbSend2";
            this.gbSend2.Size = new System.Drawing.Size(182, 75);
            this.gbSend2.TabIndex = 37;
            this.gbSend2.TabStop = false;
            // 
            // txtSend_Port
            // 
            this.txtSend_Port.Location = new System.Drawing.Point(68, 45);
            this.txtSend_Port.Name = "txtSend_Port";
            this.txtSend_Port.ReadOnly = true;
            this.txtSend_Port.Size = new System.Drawing.Size(106, 23);
            this.txtSend_Port.TabIndex = 17;
            this.txtSend_Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lSend_Port
            // 
            this.lSend_Port.AutoSize = true;
            this.lSend_Port.Location = new System.Drawing.Point(6, 48);
            this.lSend_Port.Name = "lSend_Port";
            this.lSend_Port.Size = new System.Drawing.Size(68, 17);
            this.lSend_Port.TabIndex = 16;
            this.lSend_Port.Text = "目的端口：";
            // 
            // txtSend_IP
            // 
            this.txtSend_IP.Location = new System.Drawing.Point(68, 16);
            this.txtSend_IP.Name = "txtSend_IP";
            this.txtSend_IP.ReadOnly = true;
            this.txtSend_IP.Size = new System.Drawing.Size(106, 23);
            this.txtSend_IP.TabIndex = 15;
            this.txtSend_IP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lSend_IP
            // 
            this.lSend_IP.AutoSize = true;
            this.lSend_IP.Location = new System.Drawing.Point(6, 19);
            this.lSend_IP.Name = "lSend_IP";
            this.lSend_IP.Size = new System.Drawing.Size(55, 17);
            this.lSend_IP.TabIndex = 14;
            this.lSend_IP.Text = "目的IP：";
            // 
            // gbSend1
            // 
            this.gbSend1.Controls.Add(this.txtSend_Len);
            this.gbSend1.Controls.Add(this.lSend_Len);
            this.gbSend1.Controls.Add(this.txtSend_Socket);
            this.gbSend1.Controls.Add(this.lSend_Socket);
            this.gbSend1.Location = new System.Drawing.Point(3, 99);
            this.gbSend1.Margin = new System.Windows.Forms.Padding(0);
            this.gbSend1.Name = "gbSend1";
            this.gbSend1.Size = new System.Drawing.Size(134, 75);
            this.gbSend1.TabIndex = 36;
            this.gbSend1.TabStop = false;
            // 
            // txtSend_Len
            // 
            this.txtSend_Len.Location = new System.Drawing.Point(58, 45);
            this.txtSend_Len.Name = "txtSend_Len";
            this.txtSend_Len.Size = new System.Drawing.Size(68, 23);
            this.txtSend_Len.TabIndex = 90;
            this.txtSend_Len.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lSend_Len
            // 
            this.lSend_Len.AutoSize = true;
            this.lSend_Len.Location = new System.Drawing.Point(6, 48);
            this.lSend_Len.Name = "lSend_Len";
            this.lSend_Len.Size = new System.Drawing.Size(44, 17);
            this.lSend_Len.TabIndex = 8;
            this.lSend_Len.Text = "长度：";
            // 
            // txtSend_Socket
            // 
            this.txtSend_Socket.Location = new System.Drawing.Point(58, 16);
            this.txtSend_Socket.Name = "txtSend_Socket";
            this.txtSend_Socket.Size = new System.Drawing.Size(68, 23);
            this.txtSend_Socket.TabIndex = 70;
            this.txtSend_Socket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lSend_Socket
            // 
            this.lSend_Socket.AutoSize = true;
            this.lSend_Socket.Location = new System.Drawing.Point(6, 19);
            this.lSend_Socket.Name = "lSend_Socket";
            this.lSend_Socket.Size = new System.Drawing.Size(56, 17);
            this.lSend_Socket.TabIndex = 6;
            this.lSend_Socket.Text = "套接字：";
            // 
            // gbSend_Bottom
            // 
            this.gbSend_Bottom.Controls.Add(this.nudLoop_Int);
            this.gbSend_Bottom.Controls.Add(this.nudLoop_CNT);
            this.gbSend_Bottom.Controls.Add(this.lLoop_Int);
            this.gbSend_Bottom.Controls.Add(this.lLoop_CNT);
            this.gbSend_Bottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbSend_Bottom.Location = new System.Drawing.Point(514, 100);
            this.gbSend_Bottom.Name = "gbSend_Bottom";
            this.gbSend_Bottom.Size = new System.Drawing.Size(126, 75);
            this.gbSend_Bottom.TabIndex = 35;
            this.gbSend_Bottom.TabStop = false;
            // 
            // nudLoop_Int
            // 
            this.nudLoop_Int.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudLoop_Int.Location = new System.Drawing.Point(45, 45);
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
            this.nudLoop_Int.TabIndex = 113;
            this.nudLoop_Int.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLoop_Int.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // nudLoop_CNT
            // 
            this.nudLoop_CNT.Location = new System.Drawing.Point(45, 15);
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
            this.nudLoop_CNT.Size = new System.Drawing.Size(71, 23);
            this.nudLoop_CNT.TabIndex = 112;
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
            this.lLoop_Int.Location = new System.Drawing.Point(7, 48);
            this.lLoop_Int.Name = "lLoop_Int";
            this.lLoop_Int.Size = new System.Drawing.Size(44, 17);
            this.lLoop_Int.TabIndex = 8;
            this.lLoop_Int.Text = "间隔：";
            // 
            // lLoop_CNT
            // 
            this.lLoop_CNT.AutoSize = true;
            this.lLoop_CNT.Location = new System.Drawing.Point(7, 17);
            this.lLoop_CNT.Name = "lLoop_CNT";
            this.lLoop_CNT.Size = new System.Drawing.Size(44, 17);
            this.lLoop_CNT.TabIndex = 6;
            this.lLoop_CNT.Text = "循环：";
            // 
            // bgwSendPacket
            // 
            this.bgwSendPacket.WorkerSupportsCancellation = true;
            this.bgwSendPacket.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendPacket_DoWork);
            // 
            // cmsSocketSend
            // 
            this.cmsSocketSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBatchSend});
            this.cmsSocketSend.Name = "cmsSocketSend";
            this.cmsSocketSend.Size = new System.Drawing.Size(161, 26);
            this.cmsSocketSend.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketSend_ItemClicked);
            // 
            // tsmiBatchSend
            // 
            this.tsmiBatchSend.Name = "tsmiBatchSend";
            this.tsmiBatchSend.Size = new System.Drawing.Size(160, 22);
            this.tsmiBatchSend.Text = "添加到发送列表";
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
            this.gbStep.Location = new System.Drawing.Point(328, 100);
            this.gbStep.Name = "gbStep";
            this.gbStep.Size = new System.Drawing.Size(180, 75);
            this.gbStep.TabIndex = 41;
            this.gbStep.TabStop = false;
            // 
            // lStepLen_Value
            // 
            this.lStepLen_Value.AutoSize = true;
            this.lStepLen_Value.Location = new System.Drawing.Point(145, 48);
            this.lStepLen_Value.Name = "lStepLen_Value";
            this.lStepLen_Value.Size = new System.Drawing.Size(14, 17);
            this.lStepLen_Value.TabIndex = 7;
            this.lStepLen_Value.Text = "?";
            // 
            // lStepIndex_Value
            // 
            this.lStepIndex_Value.AutoSize = true;
            this.lStepIndex_Value.Location = new System.Drawing.Point(145, 17);
            this.lStepIndex_Value.Name = "lStepIndex_Value";
            this.lStepIndex_Value.Size = new System.Drawing.Size(14, 17);
            this.lStepIndex_Value.TabIndex = 6;
            this.lStepIndex_Value.Text = "?";
            // 
            // nudStepIndex
            // 
            this.nudStepIndex.Location = new System.Drawing.Point(87, 15);
            this.nudStepIndex.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudStepIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepIndex.Name = "nudStepIndex";
            this.nudStepIndex.Size = new System.Drawing.Size(52, 23);
            this.nudStepIndex.TabIndex = 5;
            this.nudStepIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudStepIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepIndex.ValueChanged += new System.EventHandler(this.nudStepIndex_ValueChanged);
            // 
            // nudStepLen
            // 
            this.nudStepLen.Location = new System.Drawing.Point(87, 46);
            this.nudStepLen.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudStepLen.Name = "nudStepLen";
            this.nudStepLen.Size = new System.Drawing.Size(52, 23);
            this.nudStepLen.TabIndex = 4;
            this.nudStepLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudStepLen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepLen.ValueChanged += new System.EventHandler(this.nudStepLen_ValueChanged);
            // 
            // lStepLen
            // 
            this.lStepLen.AutoSize = true;
            this.lStepLen.Location = new System.Drawing.Point(23, 48);
            this.lStepLen.Name = "lStepLen";
            this.lStepLen.Size = new System.Drawing.Size(68, 17);
            this.lStepLen.TabIndex = 3;
            this.lStepLen.Text = "每次步长：";
            // 
            // cbStep
            // 
            this.cbStep.AutoSize = true;
            this.cbStep.Location = new System.Drawing.Point(7, 16);
            this.cbStep.Name = "cbStep";
            this.cbStep.Size = new System.Drawing.Size(87, 21);
            this.cbStep.TabIndex = 0;
            this.cbStep.Text = "递进位置：";
            this.cbStep.UseVisualStyleBackColor = true;
            // 
            // pSend_Top
            // 
            this.pSend_Top.Controls.Add(this.dgvSocketSend);
            this.pSend_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSend_Top.Location = new System.Drawing.Point(0, 0);
            this.pSend_Top.Name = "pSend_Top";
            this.pSend_Top.Padding = new System.Windows.Forms.Padding(3);
            this.pSend_Top.Size = new System.Drawing.Size(745, 96);
            this.pSend_Top.TabIndex = 44;
            // 
            // dgvSocketSend
            // 
            this.dgvSocketSend.AllowUserToAddRows = false;
            this.dgvSocketSend.AllowUserToDeleteRows = false;
            this.dgvSocketSend.AllowUserToResizeColumns = false;
            this.dgvSocketSend.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketSend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSocketSend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSocketSend.ContextMenuStrip = this.cmsSocketSend;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketSend.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSocketSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSocketSend.Location = new System.Drawing.Point(3, 3);
            this.dgvSocketSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvSocketSend.MultiSelect = false;
            this.dgvSocketSend.Name = "dgvSocketSend";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketSend.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSocketSend.RowHeadersWidth = 100;
            this.dgvSocketSend.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvSocketSend.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSocketSend.RowTemplate.Height = 23;
            this.dgvSocketSend.Size = new System.Drawing.Size(739, 90);
            this.dgvSocketSend.TabIndex = 6;
            this.dgvSocketSend.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSocketSend_CellClick);
            this.dgvSocketSend.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSocketSend_ColumnAdded);
            this.dgvSocketSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSocketSend_KeyDown);
            // 
            // Socket_Send_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(745, 199);
            this.Controls.Add(this.pSend_Top);
            this.Controls.Add(this.gbStep);
            this.Controls.Add(this.bSendStop);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.gbSend2);
            this.Controls.Add(this.gbSend1);
            this.Controls.Add(this.gbSend_Bottom);
            this.Controls.Add(this.ssSocketSend);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Socket_Send_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发送封包";
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
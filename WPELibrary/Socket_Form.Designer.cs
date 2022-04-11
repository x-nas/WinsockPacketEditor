
namespace WPELibrary
{
    partial class Socket_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_Form));
            this.cbDisplay_SendTo = new System.Windows.Forms.CheckBox();
            this.cbCheck_Packet = new System.Windows.Forms.CheckBox();
            this.bStopHook = new System.Windows.Forms.Button();
            this.txtCheck_IP = new System.Windows.Forms.TextBox();
            this.cbDisplay_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbCheck_Size = new System.Windows.Forms.CheckBox();
            this.txtCheck_Size_From = new System.Windows.Forms.TextBox();
            this.txtCheck_Size_To = new System.Windows.Forms.TextBox();
            this.cbInterecept_Recv = new System.Windows.Forms.CheckBox();
            this.cbInterecept_SendTo = new System.Windows.Forms.CheckBox();
            this.lSplit = new System.Windows.Forms.Label();
            this.txtCheck_Packet = new System.Windows.Forms.TextBox();
            this.gbFilter_Size = new System.Windows.Forms.GroupBox();
            this.gbFilter_Type = new System.Windows.Forms.GroupBox();
            this.cbInterecept_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbInterecept_Send = new System.Windows.Forms.CheckBox();
            this.cbDisplay_Recv = new System.Windows.Forms.CheckBox();
            this.cbDisplay_Send = new System.Windows.Forms.CheckBox();
            this.gbBottom = new System.Windows.Forms.GroupBox();
            this.tcPacketInfo = new System.Windows.Forms.TabControl();
            this.tpHEX = new System.Windows.Forms.TabPage();
            this.rtbHEX = new System.Windows.Forms.RichTextBox();
            this.tpDEC = new System.Windows.Forms.TabPage();
            this.rtbDEC = new System.Windows.Forms.RichTextBox();
            this.tpBIN = new System.Windows.Forms.TabPage();
            this.rtbBIN = new System.Windows.Forms.RichTextBox();
            this.tpUNICODE = new System.Windows.Forms.TabPage();
            this.rtbUNICODE = new System.Windows.Forms.RichTextBox();
            this.tpUTF8 = new System.Windows.Forms.TabPage();
            this.rtbUTF8 = new System.Windows.Forms.RichTextBox();
            this.tpGB2312 = new System.Windows.Forms.TabPage();
            this.rtbGB2312 = new System.Windows.Forms.RichTextBox();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.dgvLogList = new System.Windows.Forms.DataGridView();
            this.cmsLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出到ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.清空此列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgwSocketList = new System.ComponentModel.BackgroundWorker();
            this.tSocketInfo = new System.Windows.Forms.Timer(this.components);
            this.bStartHook = new System.Windows.Forms.Button();
            this.txtCheck_Socket = new System.Windows.Forms.TextBox();
            this.cbCheck_Socket = new System.Windows.Forms.CheckBox();
            this.cbCheck_IP = new System.Windows.Forms.CheckBox();
            this.cmsSocketList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiUseSocket = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveSocketInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tlCheck_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssStatusInfo_Top = new System.Windows.Forms.StatusStrip();
            this.tlSystemInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlALL = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlALL_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlCheck = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlInterecept = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlInterecept_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbRight = new System.Windows.Forms.GroupBox();
            this.gbClear = new System.Windows.Forms.GroupBox();
            this.cbReset_CNT = new System.Windows.Forms.CheckBox();
            this.gbSearch_Bottom = new System.Windows.Forms.GroupBox();
            this.rbSearchFrom = new System.Windows.Forms.RadioButton();
            this.rbSearchAll = new System.Windows.Forms.RadioButton();
            this.bSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lSearch = new System.Windows.Forms.Label();
            this.dgvSocketList = new System.Windows.Forms.DataGridView();
            this.cIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbFilterList = new System.Windows.Forms.GroupBox();
            this.dgvFilterList = new System.Windows.Forms.DataGridView();
            this.cCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cFilterIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFilterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFilterSearch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFilterModify = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bgwLogList = new System.ComponentModel.BackgroundWorker();
            this.cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbFilter_Size.SuspendLayout();
            this.gbFilter_Type.SuspendLayout();
            this.gbBottom.SuspendLayout();
            this.tcPacketInfo.SuspendLayout();
            this.tpHEX.SuspendLayout();
            this.tpDEC.SuspendLayout();
            this.tpBIN.SuspendLayout();
            this.tpUNICODE.SuspendLayout();
            this.tpUTF8.SuspendLayout();
            this.tpGB2312.SuspendLayout();
            this.tpLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            this.cmsLogList.SuspendLayout();
            this.cmsSocketList.SuspendLayout();
            this.ssStatusInfo_Top.SuspendLayout();
            this.gbRight.SuspendLayout();
            this.gbClear.SuspendLayout();
            this.gbSearch_Bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).BeginInit();
            this.gbFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).BeginInit();
            this.SuspendLayout();
            // 
            // cbDisplay_SendTo
            // 
            this.cbDisplay_SendTo.AutoSize = true;
            this.cbDisplay_SendTo.Checked = true;
            this.cbDisplay_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_SendTo.Location = new System.Drawing.Point(69, 34);
            this.cbDisplay_SendTo.Name = "cbDisplay_SendTo";
            this.cbDisplay_SendTo.Size = new System.Drawing.Size(63, 21);
            this.cbDisplay_SendTo.TabIndex = 1;
            this.cbDisplay_SendTo.Text = "发送到";
            this.cbDisplay_SendTo.UseVisualStyleBackColor = true;
            this.cbDisplay_SendTo.CheckedChanged += new System.EventHandler(this.cbDisplay_SendTo_CheckedChanged);
            // 
            // cbCheck_Packet
            // 
            this.cbCheck_Packet.AutoSize = true;
            this.cbCheck_Packet.Location = new System.Drawing.Point(17, 81);
            this.cbCheck_Packet.Name = "cbCheck_Packet";
            this.cbCheck_Packet.Size = new System.Drawing.Size(99, 21);
            this.cbCheck_Packet.TabIndex = 39;
            this.cbCheck_Packet.Text = "过滤封包内容";
            this.cbCheck_Packet.UseVisualStyleBackColor = true;
            this.cbCheck_Packet.CheckedChanged += new System.EventHandler(this.cbCheck_Packet_CheckedChanged);
            // 
            // bStopHook
            // 
            this.bStopHook.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bStopHook.Location = new System.Drawing.Point(796, 69);
            this.bStopHook.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bStopHook.Name = "bStopHook";
            this.bStopHook.Size = new System.Drawing.Size(75, 33);
            this.bStopHook.TabIndex = 19;
            this.bStopHook.Text = "结 束 (&J)";
            this.bStopHook.UseVisualStyleBackColor = true;
            this.bStopHook.Click += new System.EventHandler(this.bStopHook_Click);
            // 
            // txtCheck_IP
            // 
            this.txtCheck_IP.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheck_IP.Location = new System.Drawing.Point(116, 51);
            this.txtCheck_IP.Name = "txtCheck_IP";
            this.txtCheck_IP.Size = new System.Drawing.Size(399, 22);
            this.txtCheck_IP.TabIndex = 38;
            this.txtCheck_IP.Text = "0.0.0.0;127.0.0.1";
            this.txtCheck_IP.WordWrap = false;
            // 
            // cbDisplay_RecvFrom
            // 
            this.cbDisplay_RecvFrom.AutoSize = true;
            this.cbDisplay_RecvFrom.Checked = true;
            this.cbDisplay_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_RecvFrom.Location = new System.Drawing.Point(69, 78);
            this.cbDisplay_RecvFrom.Name = "cbDisplay_RecvFrom";
            this.cbDisplay_RecvFrom.Size = new System.Drawing.Size(63, 21);
            this.cbDisplay_RecvFrom.TabIndex = 25;
            this.cbDisplay_RecvFrom.Text = "接收自";
            this.cbDisplay_RecvFrom.UseVisualStyleBackColor = true;
            this.cbDisplay_RecvFrom.CheckedChanged += new System.EventHandler(this.cbDisplay_RecvFrom_CheckedChanged);
            // 
            // cbCheck_Size
            // 
            this.cbCheck_Size.AutoSize = true;
            this.cbCheck_Size.Location = new System.Drawing.Point(10, 14);
            this.cbCheck_Size.Name = "cbCheck_Size";
            this.cbCheck_Size.Size = new System.Drawing.Size(75, 21);
            this.cbCheck_Size.TabIndex = 27;
            this.cbCheck_Size.Text = "封包长度";
            this.cbCheck_Size.UseVisualStyleBackColor = true;
            this.cbCheck_Size.CheckedChanged += new System.EventHandler(this.cbCheck_Size_CheckedChanged);
            // 
            // txtCheck_Size_From
            // 
            this.txtCheck_Size_From.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheck_Size_From.Location = new System.Drawing.Point(15, 41);
            this.txtCheck_Size_From.Name = "txtCheck_Size_From";
            this.txtCheck_Size_From.Size = new System.Drawing.Size(57, 22);
            this.txtCheck_Size_From.TabIndex = 28;
            this.txtCheck_Size_From.Text = "0";
            this.txtCheck_Size_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCheck_Size_To
            // 
            this.txtCheck_Size_To.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheck_Size_To.Location = new System.Drawing.Point(15, 74);
            this.txtCheck_Size_To.Name = "txtCheck_Size_To";
            this.txtCheck_Size_To.Size = new System.Drawing.Size(57, 22);
            this.txtCheck_Size_To.TabIndex = 29;
            this.txtCheck_Size_To.Text = "100";
            this.txtCheck_Size_To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbInterecept_Recv
            // 
            this.cbInterecept_Recv.AutoSize = true;
            this.cbInterecept_Recv.Location = new System.Drawing.Point(9, 57);
            this.cbInterecept_Recv.Name = "cbInterecept_Recv";
            this.cbInterecept_Recv.Size = new System.Drawing.Size(60, 21);
            this.cbInterecept_Recv.TabIndex = 28;
            this.cbInterecept_Recv.Text = "拦截 -";
            this.cbInterecept_Recv.UseVisualStyleBackColor = true;
            this.cbInterecept_Recv.CheckedChanged += new System.EventHandler(this.cbInterecept_Recv_CheckedChanged);
            // 
            // cbInterecept_SendTo
            // 
            this.cbInterecept_SendTo.AutoSize = true;
            this.cbInterecept_SendTo.Location = new System.Drawing.Point(9, 34);
            this.cbInterecept_SendTo.Name = "cbInterecept_SendTo";
            this.cbInterecept_SendTo.Size = new System.Drawing.Size(60, 21);
            this.cbInterecept_SendTo.TabIndex = 27;
            this.cbInterecept_SendTo.Text = "拦截 -";
            this.cbInterecept_SendTo.UseVisualStyleBackColor = true;
            this.cbInterecept_SendTo.CheckedChanged += new System.EventHandler(this.cbInterecept_SendTo_CheckedChanged);
            // 
            // lSplit
            // 
            this.lSplit.AutoSize = true;
            this.lSplit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit.Location = new System.Drawing.Point(35, 59);
            this.lSplit.Name = "lSplit";
            this.lSplit.Size = new System.Drawing.Size(17, 17);
            this.lSplit.TabIndex = 30;
            this.lSplit.Text = "~";
            // 
            // txtCheck_Packet
            // 
            this.txtCheck_Packet.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheck_Packet.Location = new System.Drawing.Point(116, 79);
            this.txtCheck_Packet.Name = "txtCheck_Packet";
            this.txtCheck_Packet.Size = new System.Drawing.Size(399, 22);
            this.txtCheck_Packet.TabIndex = 40;
            this.txtCheck_Packet.WordWrap = false;
            // 
            // gbFilter_Size
            // 
            this.gbFilter_Size.Controls.Add(this.cbCheck_Size);
            this.gbFilter_Size.Controls.Add(this.txtCheck_Size_From);
            this.gbFilter_Size.Controls.Add(this.txtCheck_Size_To);
            this.gbFilter_Size.Controls.Add(this.lSplit);
            this.gbFilter_Size.Location = new System.Drawing.Point(520, 9);
            this.gbFilter_Size.Name = "gbFilter_Size";
            this.gbFilter_Size.Size = new System.Drawing.Size(89, 101);
            this.gbFilter_Size.TabIndex = 27;
            this.gbFilter_Size.TabStop = false;
            // 
            // gbFilter_Type
            // 
            this.gbFilter_Type.Controls.Add(this.cbInterecept_RecvFrom);
            this.gbFilter_Type.Controls.Add(this.cbInterecept_Recv);
            this.gbFilter_Type.Controls.Add(this.cbInterecept_SendTo);
            this.gbFilter_Type.Controls.Add(this.cbInterecept_Send);
            this.gbFilter_Type.Controls.Add(this.cbDisplay_RecvFrom);
            this.gbFilter_Type.Controls.Add(this.cbDisplay_Recv);
            this.gbFilter_Type.Controls.Add(this.cbDisplay_SendTo);
            this.gbFilter_Type.Controls.Add(this.cbDisplay_Send);
            this.gbFilter_Type.Location = new System.Drawing.Point(614, 9);
            this.gbFilter_Type.Name = "gbFilter_Type";
            this.gbFilter_Type.Padding = new System.Windows.Forms.Padding(1);
            this.gbFilter_Type.Size = new System.Drawing.Size(133, 101);
            this.gbFilter_Type.TabIndex = 25;
            this.gbFilter_Type.TabStop = false;
            // 
            // cbInterecept_RecvFrom
            // 
            this.cbInterecept_RecvFrom.AutoSize = true;
            this.cbInterecept_RecvFrom.Location = new System.Drawing.Point(9, 78);
            this.cbInterecept_RecvFrom.Name = "cbInterecept_RecvFrom";
            this.cbInterecept_RecvFrom.Size = new System.Drawing.Size(60, 21);
            this.cbInterecept_RecvFrom.TabIndex = 29;
            this.cbInterecept_RecvFrom.Text = "拦截 -";
            this.cbInterecept_RecvFrom.UseVisualStyleBackColor = true;
            this.cbInterecept_RecvFrom.CheckedChanged += new System.EventHandler(this.cbInterecept_RecvFrom_CheckedChanged);
            // 
            // cbInterecept_Send
            // 
            this.cbInterecept_Send.AutoSize = true;
            this.cbInterecept_Send.Location = new System.Drawing.Point(9, 11);
            this.cbInterecept_Send.Name = "cbInterecept_Send";
            this.cbInterecept_Send.Size = new System.Drawing.Size(60, 21);
            this.cbInterecept_Send.TabIndex = 26;
            this.cbInterecept_Send.Text = "拦截 -";
            this.cbInterecept_Send.UseVisualStyleBackColor = true;
            this.cbInterecept_Send.CheckedChanged += new System.EventHandler(this.cbInterecept_Send_CheckedChanged);
            // 
            // cbDisplay_Recv
            // 
            this.cbDisplay_Recv.AutoSize = true;
            this.cbDisplay_Recv.Checked = true;
            this.cbDisplay_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_Recv.Location = new System.Drawing.Point(69, 57);
            this.cbDisplay_Recv.Name = "cbDisplay_Recv";
            this.cbDisplay_Recv.Size = new System.Drawing.Size(51, 21);
            this.cbDisplay_Recv.TabIndex = 2;
            this.cbDisplay_Recv.Text = "接收";
            this.cbDisplay_Recv.UseVisualStyleBackColor = true;
            this.cbDisplay_Recv.CheckedChanged += new System.EventHandler(this.cbDisplay_Recv_CheckedChanged);
            // 
            // cbDisplay_Send
            // 
            this.cbDisplay_Send.AutoSize = true;
            this.cbDisplay_Send.Checked = true;
            this.cbDisplay_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_Send.Location = new System.Drawing.Point(69, 11);
            this.cbDisplay_Send.Name = "cbDisplay_Send";
            this.cbDisplay_Send.Size = new System.Drawing.Size(51, 21);
            this.cbDisplay_Send.TabIndex = 0;
            this.cbDisplay_Send.Text = "发送";
            this.cbDisplay_Send.UseVisualStyleBackColor = true;
            this.cbDisplay_Send.CheckedChanged += new System.EventHandler(this.cbDisplay_Send_CheckedChanged);
            // 
            // gbBottom
            // 
            this.gbBottom.Controls.Add(this.tcPacketInfo);
            this.gbBottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbBottom.Location = new System.Drawing.Point(158, 455);
            this.gbBottom.Margin = new System.Windows.Forms.Padding(0);
            this.gbBottom.Name = "gbBottom";
            this.gbBottom.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbBottom.Size = new System.Drawing.Size(725, 186);
            this.gbBottom.TabIndex = 43;
            this.gbBottom.TabStop = false;
            // 
            // tcPacketInfo
            // 
            this.tcPacketInfo.Controls.Add(this.tpHEX);
            this.tcPacketInfo.Controls.Add(this.tpDEC);
            this.tcPacketInfo.Controls.Add(this.tpBIN);
            this.tcPacketInfo.Controls.Add(this.tpUNICODE);
            this.tcPacketInfo.Controls.Add(this.tpUTF8);
            this.tcPacketInfo.Controls.Add(this.tpGB2312);
            this.tcPacketInfo.Controls.Add(this.tpLog);
            this.tcPacketInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPacketInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcPacketInfo.Location = new System.Drawing.Point(3, 20);
            this.tcPacketInfo.Name = "tcPacketInfo";
            this.tcPacketInfo.SelectedIndex = 0;
            this.tcPacketInfo.Size = new System.Drawing.Size(719, 162);
            this.tcPacketInfo.TabIndex = 10;
            this.tcPacketInfo.SelectedIndexChanged += new System.EventHandler(this.tcPacketInfo_SelectedIndexChanged);
            // 
            // tpHEX
            // 
            this.tpHEX.Controls.Add(this.rtbHEX);
            this.tpHEX.Location = new System.Drawing.Point(4, 26);
            this.tpHEX.Name = "tpHEX";
            this.tpHEX.Size = new System.Drawing.Size(711, 132);
            this.tpHEX.TabIndex = 0;
            this.tpHEX.Text = "十六进制";
            this.tpHEX.UseVisualStyleBackColor = true;
            // 
            // rtbHEX
            // 
            this.rtbHEX.BackColor = System.Drawing.SystemColors.Window;
            this.rtbHEX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHEX.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHEX.Location = new System.Drawing.Point(0, 0);
            this.rtbHEX.Name = "rtbHEX";
            this.rtbHEX.Size = new System.Drawing.Size(711, 132);
            this.rtbHEX.TabIndex = 3;
            this.rtbHEX.Text = "";
            // 
            // tpDEC
            // 
            this.tpDEC.Controls.Add(this.rtbDEC);
            this.tpDEC.Location = new System.Drawing.Point(4, 26);
            this.tpDEC.Name = "tpDEC";
            this.tpDEC.Size = new System.Drawing.Size(711, 132);
            this.tpDEC.TabIndex = 1;
            this.tpDEC.Text = "十进制";
            this.tpDEC.UseVisualStyleBackColor = true;
            // 
            // rtbDEC
            // 
            this.rtbDEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDEC.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDEC.Location = new System.Drawing.Point(0, 0);
            this.rtbDEC.Name = "rtbDEC";
            this.rtbDEC.Size = new System.Drawing.Size(711, 132);
            this.rtbDEC.TabIndex = 1;
            this.rtbDEC.Text = "";
            // 
            // tpBIN
            // 
            this.tpBIN.Controls.Add(this.rtbBIN);
            this.tpBIN.Location = new System.Drawing.Point(4, 26);
            this.tpBIN.Name = "tpBIN";
            this.tpBIN.Size = new System.Drawing.Size(711, 132);
            this.tpBIN.TabIndex = 2;
            this.tpBIN.Text = "二进制";
            this.tpBIN.UseVisualStyleBackColor = true;
            // 
            // rtbBIN
            // 
            this.rtbBIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbBIN.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbBIN.Location = new System.Drawing.Point(0, 0);
            this.rtbBIN.Name = "rtbBIN";
            this.rtbBIN.Size = new System.Drawing.Size(711, 132);
            this.rtbBIN.TabIndex = 3;
            this.rtbBIN.Text = "";
            // 
            // tpUNICODE
            // 
            this.tpUNICODE.Controls.Add(this.rtbUNICODE);
            this.tpUNICODE.Location = new System.Drawing.Point(4, 26);
            this.tpUNICODE.Name = "tpUNICODE";
            this.tpUNICODE.Size = new System.Drawing.Size(711, 132);
            this.tpUNICODE.TabIndex = 3;
            this.tpUNICODE.Text = "Unicode";
            this.tpUNICODE.UseVisualStyleBackColor = true;
            // 
            // rtbUNICODE
            // 
            this.rtbUNICODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbUNICODE.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbUNICODE.Location = new System.Drawing.Point(0, 0);
            this.rtbUNICODE.Name = "rtbUNICODE";
            this.rtbUNICODE.Size = new System.Drawing.Size(711, 132);
            this.rtbUNICODE.TabIndex = 2;
            this.rtbUNICODE.Text = "";
            // 
            // tpUTF8
            // 
            this.tpUTF8.Controls.Add(this.rtbUTF8);
            this.tpUTF8.Location = new System.Drawing.Point(4, 26);
            this.tpUTF8.Name = "tpUTF8";
            this.tpUTF8.Size = new System.Drawing.Size(711, 132);
            this.tpUTF8.TabIndex = 4;
            this.tpUTF8.Text = "UTF-8";
            this.tpUTF8.UseVisualStyleBackColor = true;
            // 
            // rtbUTF8
            // 
            this.rtbUTF8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbUTF8.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbUTF8.Location = new System.Drawing.Point(0, 0);
            this.rtbUTF8.Name = "rtbUTF8";
            this.rtbUTF8.Size = new System.Drawing.Size(711, 132);
            this.rtbUTF8.TabIndex = 2;
            this.rtbUTF8.Text = "";
            // 
            // tpGB2312
            // 
            this.tpGB2312.Controls.Add(this.rtbGB2312);
            this.tpGB2312.Location = new System.Drawing.Point(4, 26);
            this.tpGB2312.Name = "tpGB2312";
            this.tpGB2312.Size = new System.Drawing.Size(711, 132);
            this.tpGB2312.TabIndex = 5;
            this.tpGB2312.Text = "GB2312";
            this.tpGB2312.UseVisualStyleBackColor = true;
            // 
            // rtbGB2312
            // 
            this.rtbGB2312.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGB2312.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbGB2312.Location = new System.Drawing.Point(0, 0);
            this.rtbGB2312.Name = "rtbGB2312";
            this.rtbGB2312.Size = new System.Drawing.Size(711, 132);
            this.rtbGB2312.TabIndex = 2;
            this.rtbGB2312.Text = "";
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.dgvLogList);
            this.tpLog.Location = new System.Drawing.Point(4, 26);
            this.tpLog.Name = "tpLog";
            this.tpLog.Size = new System.Drawing.Size(711, 132);
            this.tpLog.TabIndex = 7;
            this.tpLog.Text = "日志";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // dgvLogList
            // 
            this.dgvLogList.AllowUserToAddRows = false;
            this.dgvLogList.AllowUserToDeleteRows = false;
            this.dgvLogList.AllowUserToResizeColumns = false;
            this.dgvLogList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTime,
            this.cContent});
            this.dgvLogList.ContextMenuStrip = this.cmsLogList;
            this.dgvLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogList.Location = new System.Drawing.Point(0, 0);
            this.dgvLogList.MultiSelect = false;
            this.dgvLogList.Name = "dgvLogList";
            this.dgvLogList.ReadOnly = true;
            this.dgvLogList.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLogList.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLogList.RowTemplate.Height = 23;
            this.dgvLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogList.Size = new System.Drawing.Size(711, 132);
            this.dgvLogList.TabIndex = 0;
            // 
            // cmsLogList
            // 
            this.cmsLogList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出到ExcelToolStripMenuItem,
            this.toolStripSeparator5,
            this.清空此列表ToolStripMenuItem});
            this.cmsLogList.Name = "cmsLogList";
            this.cmsLogList.Size = new System.Drawing.Size(142, 54);
            this.cmsLogList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsLogList_ItemClicked);
            // 
            // 导出到ExcelToolStripMenuItem
            // 
            this.导出到ExcelToolStripMenuItem.Name = "导出到ExcelToolStripMenuItem";
            this.导出到ExcelToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.导出到ExcelToolStripMenuItem.Text = "导出到Excel";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(138, 6);
            // 
            // 清空此列表ToolStripMenuItem
            // 
            this.清空此列表ToolStripMenuItem.Name = "清空此列表ToolStripMenuItem";
            this.清空此列表ToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.清空此列表ToolStripMenuItem.Text = "清空此列表";
            // 
            // bgwSocketList
            // 
            this.bgwSocketList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketList_DoWork);
            // 
            // tSocketInfo
            // 
            this.tSocketInfo.Tick += new System.EventHandler(this.tSocketInfo_Tick);
            // 
            // bStartHook
            // 
            this.bStartHook.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bStartHook.Location = new System.Drawing.Point(796, 18);
            this.bStartHook.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bStartHook.Name = "bStartHook";
            this.bStartHook.Size = new System.Drawing.Size(75, 33);
            this.bStartHook.TabIndex = 18;
            this.bStartHook.Text = "开 始 (&K)";
            this.bStartHook.UseVisualStyleBackColor = true;
            this.bStartHook.Click += new System.EventHandler(this.bStartHook_Click);
            // 
            // txtCheck_Socket
            // 
            this.txtCheck_Socket.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheck_Socket.Location = new System.Drawing.Point(116, 24);
            this.txtCheck_Socket.Name = "txtCheck_Socket";
            this.txtCheck_Socket.Size = new System.Drawing.Size(399, 22);
            this.txtCheck_Socket.TabIndex = 36;
            this.txtCheck_Socket.WordWrap = false;
            // 
            // cbCheck_Socket
            // 
            this.cbCheck_Socket.AutoSize = true;
            this.cbCheck_Socket.Location = new System.Drawing.Point(17, 26);
            this.cbCheck_Socket.Name = "cbCheck_Socket";
            this.cbCheck_Socket.Size = new System.Drawing.Size(87, 21);
            this.cbCheck_Socket.TabIndex = 35;
            this.cbCheck_Socket.Text = "过滤套接字";
            this.cbCheck_Socket.UseVisualStyleBackColor = true;
            this.cbCheck_Socket.CheckedChanged += new System.EventHandler(this.cbCheck_Socket_CheckedChanged);
            // 
            // cbCheck_IP
            // 
            this.cbCheck_IP.AutoSize = true;
            this.cbCheck_IP.Location = new System.Drawing.Point(17, 53);
            this.cbCheck_IP.Name = "cbCheck_IP";
            this.cbCheck_IP.Size = new System.Drawing.Size(86, 21);
            this.cbCheck_IP.TabIndex = 37;
            this.cbCheck_IP.Text = "过滤IP地址";
            this.cbCheck_IP.UseVisualStyleBackColor = true;
            this.cbCheck_IP.CheckedChanged += new System.EventHandler(this.cbCheck_IP_CheckedChanged);
            // 
            // cmsSocketList
            // 
            this.cmsSocketList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSend,
            this.toolStripSeparator1,
            this.tsmiBatchSend,
            this.toolStripSeparator2,
            this.tsmiUseSocket,
            this.toolStripSeparator3,
            this.tsmiShowBatchSend,
            this.toolStripSeparator4,
            this.tsmiSaveSocketInfo});
            this.cmsSocketList.Name = "cmsSocketInfo";
            this.cmsSocketList.Size = new System.Drawing.Size(161, 138);
            this.cmsSocketList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketInfo_ItemClicked);
            // 
            // tsmiSend
            // 
            this.tsmiSend.Name = "tsmiSend";
            this.tsmiSend.Size = new System.Drawing.Size(160, 22);
            this.tsmiSend.Text = "发送";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiBatchSend
            // 
            this.tsmiBatchSend.Name = "tsmiBatchSend";
            this.tsmiBatchSend.Size = new System.Drawing.Size(160, 22);
            this.tsmiBatchSend.Text = "添加到发送列表";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiUseSocket
            // 
            this.tsmiUseSocket.Name = "tsmiUseSocket";
            this.tsmiUseSocket.Size = new System.Drawing.Size(160, 22);
            this.tsmiUseSocket.Text = "使用此套接字";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiShowBatchSend
            // 
            this.tsmiShowBatchSend.Name = "tsmiShowBatchSend";
            this.tsmiShowBatchSend.Size = new System.Drawing.Size(160, 22);
            this.tsmiShowBatchSend.Text = "查看发送列表";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiSaveSocketInfo
            // 
            this.tsmiSaveSocketInfo.Name = "tsmiSaveSocketInfo";
            this.tsmiSaveSocketInfo.Size = new System.Drawing.Size(160, 22);
            this.tsmiSaveSocketInfo.Text = "导出到Excel";
            // 
            // tlCheck_CNT
            // 
            this.tlCheck_CNT.Name = "tlCheck_CNT";
            this.tlCheck_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlCheck_CNT.Text = "0";
            // 
            // ssStatusInfo_Top
            // 
            this.ssStatusInfo_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.ssStatusInfo_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlSystemInfo,
            this.toolStripStatusLabel3,
            this.tlALL,
            this.tlALL_CNT,
            this.tlSplit,
            this.tlQueue,
            this.tlQueue_CNT,
            this.toolStripStatusLabel5,
            this.tlSend,
            this.tlSend_CNT,
            this.toolStripStatusLabel1,
            this.tlRecv,
            this.tlRecv_CNT,
            this.toolStripStatusLabel2,
            this.tlCheck,
            this.tlCheck_CNT,
            this.toolStripStatusLabel4,
            this.tlInterecept,
            this.tlInterecept_CNT});
            this.ssStatusInfo_Top.Location = new System.Drawing.Point(0, 116);
            this.ssStatusInfo_Top.Name = "ssStatusInfo_Top";
            this.ssStatusInfo_Top.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.ssStatusInfo_Top.Size = new System.Drawing.Size(883, 22);
            this.ssStatusInfo_Top.SizingGrip = false;
            this.ssStatusInfo_Top.TabIndex = 44;
            this.ssStatusInfo_Top.Text = "statusStrip1";
            // 
            // tlSystemInfo
            // 
            this.tlSystemInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlSystemInfo.Name = "tlSystemInfo";
            this.tlSystemInfo.Size = new System.Drawing.Size(56, 17);
            this.tlSystemInfo.Text = "系统信息";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // tlALL
            // 
            this.tlALL.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tlALL.Name = "tlALL";
            this.tlALL.Size = new System.Drawing.Size(59, 17);
            this.tlALL.Text = "封包总数:";
            // 
            // tlALL_CNT
            // 
            this.tlALL_CNT.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tlALL_CNT.Name = "tlALL_CNT";
            this.tlALL_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlALL_CNT.Text = "0";
            // 
            // tlSplit
            // 
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            this.tlSplit.Size = new System.Drawing.Size(11, 17);
            this.tlSplit.Text = "|";
            // 
            // tlQueue
            // 
            this.tlQueue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlQueue.Name = "tlQueue";
            this.tlQueue.Size = new System.Drawing.Size(47, 17);
            this.tlQueue.Text = "缓存区:";
            // 
            // tlQueue_CNT
            // 
            this.tlQueue_CNT.Name = "tlQueue_CNT";
            this.tlQueue_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlQueue_CNT.Text = "0";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel5.Text = "|";
            // 
            // tlSend
            // 
            this.tlSend.Name = "tlSend";
            this.tlSend.Size = new System.Drawing.Size(35, 17);
            this.tlSend.Text = "发送:";
            // 
            // tlSend_CNT
            // 
            this.tlSend_CNT.Name = "tlSend_CNT";
            this.tlSend_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlSend_CNT.Text = "0";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // tlRecv
            // 
            this.tlRecv.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlRecv.Name = "tlRecv";
            this.tlRecv.Size = new System.Drawing.Size(35, 17);
            this.tlRecv.Text = "接收:";
            // 
            // tlRecv_CNT
            // 
            this.tlRecv_CNT.Name = "tlRecv_CNT";
            this.tlRecv_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlRecv_CNT.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // tlCheck
            // 
            this.tlCheck.Name = "tlCheck";
            this.tlCheck.Size = new System.Drawing.Size(35, 17);
            this.tlCheck.Text = "过滤:";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel4.Text = "|";
            // 
            // tlInterecept
            // 
            this.tlInterecept.Name = "tlInterecept";
            this.tlInterecept.Size = new System.Drawing.Size(47, 17);
            this.tlInterecept.Text = "已拦截:";
            // 
            // tlInterecept_CNT
            // 
            this.tlInterecept_CNT.Name = "tlInterecept_CNT";
            this.tlInterecept_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlInterecept_CNT.Text = "0";
            // 
            // gbRight
            // 
            this.gbRight.Controls.Add(this.gbClear);
            this.gbRight.Controls.Add(this.txtCheck_Packet);
            this.gbRight.Controls.Add(this.gbFilter_Size);
            this.gbRight.Controls.Add(this.cbCheck_Packet);
            this.gbRight.Controls.Add(this.bStopHook);
            this.gbRight.Controls.Add(this.txtCheck_IP);
            this.gbRight.Controls.Add(this.gbFilter_Type);
            this.gbRight.Controls.Add(this.cbCheck_IP);
            this.gbRight.Controls.Add(this.bStartHook);
            this.gbRight.Controls.Add(this.txtCheck_Socket);
            this.gbRight.Controls.Add(this.cbCheck_Socket);
            this.gbRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbRight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbRight.Location = new System.Drawing.Point(0, 0);
            this.gbRight.Name = "gbRight";
            this.gbRight.Size = new System.Drawing.Size(883, 116);
            this.gbRight.TabIndex = 42;
            this.gbRight.TabStop = false;
            this.gbRight.Text = "[ 过滤条件 ] - 支持多个内容使用 ; 分隔符";
            // 
            // gbClear
            // 
            this.gbClear.Controls.Add(this.cbReset_CNT);
            this.gbClear.Location = new System.Drawing.Point(752, 9);
            this.gbClear.Name = "gbClear";
            this.gbClear.Size = new System.Drawing.Size(29, 100);
            this.gbClear.TabIndex = 41;
            this.gbClear.TabStop = false;
            // 
            // cbReset_CNT
            // 
            this.cbReset_CNT.AutoSize = true;
            this.cbReset_CNT.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbReset_CNT.Checked = true;
            this.cbReset_CNT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReset_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbReset_CNT.Location = new System.Drawing.Point(3, 12);
            this.cbReset_CNT.Name = "cbReset_CNT";
            this.cbReset_CNT.Size = new System.Drawing.Size(24, 86);
            this.cbReset_CNT.TabIndex = 42;
            this.cbReset_CNT.Text = "清\r\n空\r\n数\r\n据";
            this.cbReset_CNT.UseVisualStyleBackColor = true;
            this.cbReset_CNT.CheckedChanged += new System.EventHandler(this.cbReset_CNT_CheckedChanged);
            // 
            // gbSearch_Bottom
            // 
            this.gbSearch_Bottom.Controls.Add(this.rbSearchFrom);
            this.gbSearch_Bottom.Controls.Add(this.rbSearchAll);
            this.gbSearch_Bottom.Controls.Add(this.bSearch);
            this.gbSearch_Bottom.Controls.Add(this.txtSearch);
            this.gbSearch_Bottom.Controls.Add(this.lSearch);
            this.gbSearch_Bottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbSearch_Bottom.Location = new System.Drawing.Point(158, 417);
            this.gbSearch_Bottom.Name = "gbSearch_Bottom";
            this.gbSearch_Bottom.Size = new System.Drawing.Size(725, 51);
            this.gbSearch_Bottom.TabIndex = 48;
            this.gbSearch_Bottom.TabStop = false;
            // 
            // rbSearchFrom
            // 
            this.rbSearchFrom.AutoSize = true;
            this.rbSearchFrom.Checked = true;
            this.rbSearchFrom.Location = new System.Drawing.Point(547, 19);
            this.rbSearchFrom.Name = "rbSearchFrom";
            this.rbSearchFrom.Size = new System.Drawing.Size(74, 21);
            this.rbSearchFrom.TabIndex = 4;
            this.rbSearchFrom.TabStop = true;
            this.rbSearchFrom.Text = "向下搜索";
            this.rbSearchFrom.UseVisualStyleBackColor = true;
            // 
            // rbSearchAll
            // 
            this.rbSearchAll.AutoSize = true;
            this.rbSearchAll.Location = new System.Drawing.Point(467, 20);
            this.rbSearchAll.Name = "rbSearchAll";
            this.rbSearchAll.Size = new System.Drawing.Size(74, 21);
            this.rbSearchAll.TabIndex = 3;
            this.rbSearchAll.Text = "从头搜索";
            this.rbSearchAll.UseVisualStyleBackColor = true;
            // 
            // bSearch
            // 
            this.bSearch.Location = new System.Drawing.Point(648, 15);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(70, 28);
            this.bSearch.TabIndex = 2;
            this.bSearch.Text = "搜 索 (&S)";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(131, 18);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(320, 23);
            this.txtSearch.TabIndex = 1;
            // 
            // lSearch
            // 
            this.lSearch.AutoSize = true;
            this.lSearch.Location = new System.Drawing.Point(6, 21);
            this.lSearch.Name = "lSearch";
            this.lSearch.Size = new System.Drawing.Size(128, 17);
            this.lSearch.TabIndex = 0;
            this.lSearch.Text = "数据搜索（十六进制）";
            // 
            // dgvSocketList
            // 
            this.dgvSocketList.AllowUserToAddRows = false;
            this.dgvSocketList.AllowUserToDeleteRows = false;
            this.dgvSocketList.AllowUserToResizeColumns = false;
            this.dgvSocketList.AllowUserToResizeRows = false;
            this.dgvSocketList.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgvSocketList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSocketList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIndex,
            this.cType,
            this.cSocket,
            this.cFrom,
            this.Column5,
            this.cLen,
            this.cData});
            this.dgvSocketList.ContextMenuStrip = this.cmsSocketList;
            this.dgvSocketList.Location = new System.Drawing.Point(0, 138);
            this.dgvSocketList.MultiSelect = false;
            this.dgvSocketList.Name = "dgvSocketList";
            this.dgvSocketList.RowHeadersVisible = false;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgvSocketList.RowTemplate.Height = 23;
            this.dgvSocketList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSocketList.Size = new System.Drawing.Size(883, 279);
            this.dgvSocketList.TabIndex = 49;
            this.dgvSocketList.SelectionChanged += new System.EventHandler(this.dgSocketInfo_SelectionChanged);
            // 
            // cIndex
            // 
            this.cIndex.DataPropertyName = "Index";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle3;
            this.cIndex.HeaderText = "序号";
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIndex.Width = 50;
            // 
            // cType
            // 
            this.cType.DataPropertyName = "Type_CN";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cType.DefaultCellStyle = dataGridViewCellStyle4;
            this.cType.HeaderText = "类别";
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cType.Width = 65;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle5;
            this.cSocket.HeaderText = "套接字";
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cSocket.Width = 60;
            // 
            // cFrom
            // 
            this.cFrom.DataPropertyName = "From";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cFrom.DefaultCellStyle = dataGridViewCellStyle6;
            this.cFrom.HeaderText = "源地址";
            this.cFrom.Name = "cFrom";
            this.cFrom.ReadOnly = true;
            this.cFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cFrom.Width = 160;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "To";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column5.HeaderText = "目的地址";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 160;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "ResLen";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle8;
            this.cLen.HeaderText = "长度";
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLen.Width = 50;
            // 
            // cData
            // 
            this.cData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cData.DataPropertyName = "Data";
            this.cData.HeaderText = "数据";
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gbFilterList
            // 
            this.gbFilterList.Controls.Add(this.dgvFilterList);
            this.gbFilterList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbFilterList.Location = new System.Drawing.Point(0, 417);
            this.gbFilterList.Name = "gbFilterList";
            this.gbFilterList.Size = new System.Drawing.Size(155, 224);
            this.gbFilterList.TabIndex = 53;
            this.gbFilterList.TabStop = false;
            this.gbFilterList.Text = "[ 滤镜列表 ]";
            // 
            // dgvFilterList
            // 
            this.dgvFilterList.AllowUserToAddRows = false;
            this.dgvFilterList.AllowUserToDeleteRows = false;
            this.dgvFilterList.AllowUserToResizeColumns = false;
            this.dgvFilterList.AllowUserToResizeRows = false;
            this.dgvFilterList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvFilterList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvFilterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterList.ColumnHeadersVisible = false;
            this.dgvFilterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cCheck,
            this.cFilterIndex,
            this.cFilterName,
            this.cFilterSearch,
            this.cFilterModify});
            this.dgvFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFilterList.Location = new System.Drawing.Point(3, 19);
            this.dgvFilterList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvFilterList.MultiSelect = false;
            this.dgvFilterList.Name = "dgvFilterList";
            this.dgvFilterList.RowHeadersVisible = false;
            this.dgvFilterList.RowTemplate.Height = 23;
            this.dgvFilterList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilterList.Size = new System.Drawing.Size(149, 202);
            this.dgvFilterList.TabIndex = 5;
            this.dgvFilterList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterList_CellContentClick);
            this.dgvFilterList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterList_CellDoubleClick);
            // 
            // cCheck
            // 
            this.cCheck.DataPropertyName = "ISCheck";
            this.cCheck.FalseValue = "false";
            this.cCheck.HeaderText = "开关";
            this.cCheck.Name = "cCheck";
            this.cCheck.TrueValue = "true";
            this.cCheck.Width = 30;
            // 
            // cFilterIndex
            // 
            this.cFilterIndex.DataPropertyName = "FilterIndex";
            this.cFilterIndex.HeaderText = "序号";
            this.cFilterIndex.Name = "cFilterIndex";
            this.cFilterIndex.ReadOnly = true;
            this.cFilterIndex.Visible = false;
            // 
            // cFilterName
            // 
            this.cFilterName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cFilterName.DataPropertyName = "FilterName";
            this.cFilterName.HeaderText = "过滤器名称";
            this.cFilterName.Name = "cFilterName";
            this.cFilterName.ReadOnly = true;
            // 
            // cFilterSearch
            // 
            this.cFilterSearch.DataPropertyName = "FilterSearch";
            this.cFilterSearch.HeaderText = "搜索封包";
            this.cFilterSearch.Name = "cFilterSearch";
            this.cFilterSearch.ReadOnly = true;
            this.cFilterSearch.Visible = false;
            // 
            // cFilterModify
            // 
            this.cFilterModify.DataPropertyName = "FilterModify";
            this.cFilterModify.HeaderText = "修改封包";
            this.cFilterModify.Name = "cFilterModify";
            this.cFilterModify.ReadOnly = true;
            this.cFilterModify.Visible = false;
            // 
            // bgwLogList
            // 
            this.bgwLogList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwLogList_DoWork);
            // 
            // cTime
            // 
            this.cTime.DataPropertyName = "Time";
            this.cTime.HeaderText = "记录时间";
            this.cTime.Name = "cTime";
            this.cTime.ReadOnly = true;
            this.cTime.Width = 120;
            // 
            // cContent
            // 
            this.cContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cContent.DataPropertyName = "Content";
            this.cContent.HeaderText = "日志内容";
            this.cContent.Name = "cContent";
            this.cContent.ReadOnly = true;
            // 
            // Socket_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(883, 641);
            this.Controls.Add(this.gbFilterList);
            this.Controls.Add(this.dgvSocketList);
            this.Controls.Add(this.gbSearch_Bottom);
            this.Controls.Add(this.gbBottom);
            this.Controls.Add(this.ssStatusInfo_Top);
            this.Controls.Add(this.gbRight);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Socket_Form";
            this.Text = "封包拦截器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DLL_Form_FormClosed);
            this.Load += new System.EventHandler(this.DLL_Form_Load);
            this.gbFilter_Size.ResumeLayout(false);
            this.gbFilter_Size.PerformLayout();
            this.gbFilter_Type.ResumeLayout(false);
            this.gbFilter_Type.PerformLayout();
            this.gbBottom.ResumeLayout(false);
            this.tcPacketInfo.ResumeLayout(false);
            this.tpHEX.ResumeLayout(false);
            this.tpDEC.ResumeLayout(false);
            this.tpBIN.ResumeLayout(false);
            this.tpUNICODE.ResumeLayout(false);
            this.tpUTF8.ResumeLayout(false);
            this.tpGB2312.ResumeLayout(false);
            this.tpLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
            this.cmsLogList.ResumeLayout(false);
            this.cmsSocketList.ResumeLayout(false);
            this.ssStatusInfo_Top.ResumeLayout(false);
            this.ssStatusInfo_Top.PerformLayout();
            this.gbRight.ResumeLayout(false);
            this.gbRight.PerformLayout();
            this.gbClear.ResumeLayout(false);
            this.gbClear.PerformLayout();
            this.gbSearch_Bottom.ResumeLayout(false);
            this.gbSearch_Bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).EndInit();
            this.gbFilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbDisplay_SendTo;
        private System.Windows.Forms.CheckBox cbCheck_Packet;
        private System.Windows.Forms.Button bStopHook;
        private System.Windows.Forms.TextBox txtCheck_IP;
        private System.Windows.Forms.CheckBox cbDisplay_RecvFrom;
        private System.Windows.Forms.CheckBox cbCheck_Size;
        private System.Windows.Forms.TextBox txtCheck_Size_From;
        private System.Windows.Forms.TextBox txtCheck_Size_To;
        private System.Windows.Forms.CheckBox cbInterecept_Recv;
        private System.Windows.Forms.CheckBox cbInterecept_SendTo;
        private System.Windows.Forms.Label lSplit;
        private System.Windows.Forms.TextBox txtCheck_Packet;
        private System.Windows.Forms.GroupBox gbFilter_Size;
        private System.Windows.Forms.GroupBox gbFilter_Type;
        private System.Windows.Forms.CheckBox cbInterecept_RecvFrom;
        private System.Windows.Forms.CheckBox cbInterecept_Send;
        private System.Windows.Forms.CheckBox cbDisplay_Recv;
        private System.Windows.Forms.CheckBox cbDisplay_Send;
        private System.Windows.Forms.GroupBox gbBottom;
        private System.Windows.Forms.TabControl tcPacketInfo;
        private System.Windows.Forms.TabPage tpHEX;
        private System.Windows.Forms.RichTextBox rtbHEX;
        private System.Windows.Forms.TabPage tpDEC;
        private System.Windows.Forms.RichTextBox rtbDEC;
        private System.Windows.Forms.TabPage tpBIN;
        private System.Windows.Forms.RichTextBox rtbBIN;
        private System.Windows.Forms.TabPage tpUNICODE;
        private System.Windows.Forms.RichTextBox rtbUNICODE;
        private System.Windows.Forms.TabPage tpUTF8;
        private System.Windows.Forms.RichTextBox rtbUTF8;
        private System.Windows.Forms.TabPage tpGB2312;
        private System.Windows.Forms.RichTextBox rtbGB2312;
        private System.Windows.Forms.TabPage tpLog;
        private System.ComponentModel.BackgroundWorker bgwSocketList;
        private System.Windows.Forms.Timer tSocketInfo;
        private System.Windows.Forms.Button bStartHook;
        private System.Windows.Forms.TextBox txtCheck_Socket;
        private System.Windows.Forms.CheckBox cbCheck_Socket;
        private System.Windows.Forms.CheckBox cbCheck_IP;
        private System.Windows.Forms.ContextMenuStrip cmsSocketList;
        private System.Windows.Forms.ToolStripMenuItem tsmiSend;
        private System.Windows.Forms.ToolStripStatusLabel tlCheck_CNT;
        private System.Windows.Forms.StatusStrip ssStatusInfo_Top;
        private System.Windows.Forms.ToolStripStatusLabel tlSystemInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlALL;
        private System.Windows.Forms.ToolStripStatusLabel tlALL_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlQueue;
        private System.Windows.Forms.ToolStripStatusLabel tlQueue_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel tlSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tlRecv;
        private System.Windows.Forms.ToolStripStatusLabel tlRecv_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tlCheck;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tlInterecept;
        private System.Windows.Forms.ToolStripStatusLabel tlInterecept_CNT;
        private System.Windows.Forms.GroupBox gbRight;
        private System.Windows.Forms.GroupBox gbClear;
        private System.Windows.Forms.CheckBox cbReset_CNT;
        private System.Windows.Forms.GroupBox gbSearch_Bottom;
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lSearch;
        private System.Windows.Forms.DataGridView dgvSocketList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowBatchSend;
        private System.Windows.Forms.ToolStripMenuItem tsmiUseSocket;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveSocketInfo;
        private System.Windows.Forms.RadioButton rbSearchFrom;
        private System.Windows.Forms.RadioButton rbSearchAll;
        private System.Windows.Forms.GroupBox gbFilterList;
        private System.Windows.Forms.DataGridView dgvFilterList;
        private System.Windows.Forms.DataGridView dgvLogList;
        private System.ComponentModel.BackgroundWorker bgwLogList;
        private System.Windows.Forms.ContextMenuStrip cmsLogList;
        private System.Windows.Forms.ToolStripMenuItem 导出到ExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 清空此列表ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFilterIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFilterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFilterSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFilterModify;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cContent;
    }
}
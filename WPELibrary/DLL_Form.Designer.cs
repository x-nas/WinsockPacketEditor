
namespace WPELibrary
{
    partial class DLL_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLL_Form));
            this.cbDisplay_SendTo = new System.Windows.Forms.CheckBox();
            this.cbFilter_Packet = new System.Windows.Forms.CheckBox();
            this.bStopHook = new System.Windows.Forms.Button();
            this.txtFilter_IP = new System.Windows.Forms.TextBox();
            this.cbDisplay_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbFilter_Size = new System.Windows.Forms.CheckBox();
            this.txtFilter_Size_From = new System.Windows.Forms.TextBox();
            this.txtFilter_Size_To = new System.Windows.Forms.TextBox();
            this.cbInterecept_Recv = new System.Windows.Forms.CheckBox();
            this.cbInterecept_SendTo = new System.Windows.Forms.CheckBox();
            this.lSplit = new System.Windows.Forms.Label();
            this.txtFilter_Packet = new System.Windows.Forms.TextBox();
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
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.rtbDEBUG = new System.Windows.Forms.RichTextBox();
            this.bgwSocketInfo = new System.ComponentModel.BackgroundWorker();
            this.tSocketInfo = new System.Windows.Forms.Timer(this.components);
            this.bStartHook = new System.Windows.Forms.Button();
            this.txtFilter_Socket = new System.Windows.Forms.TextBox();
            this.cbFilter_Socket = new System.Windows.Forms.CheckBox();
            this.cbFilter_IP = new System.Windows.Forms.CheckBox();
            this.cmsSocketInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiUseSocket = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.tlFilter_CNT = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.tlFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlInterecept = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlInterecept_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbRight = new System.Windows.Forms.GroupBox();
            this.gbClear = new System.Windows.Forms.GroupBox();
            this.cbReset_CNT = new System.Windows.Forms.CheckBox();
            this.gbSearch_Bottom = new System.Windows.Forms.GroupBox();
            this.bSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lSearch = new System.Windows.Forms.Label();
            this.dgSocketInfo = new System.Windows.Forms.DataGridView();
            this.cIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveSocketInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdSocketInfo = new System.Windows.Forms.SaveFileDialog();
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
            this.tpDebug.SuspendLayout();
            this.cmsSocketInfo.SuspendLayout();
            this.ssStatusInfo_Top.SuspendLayout();
            this.gbRight.SuspendLayout();
            this.gbClear.SuspendLayout();
            this.gbSearch_Bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSocketInfo)).BeginInit();
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
            // 
            // cbFilter_Packet
            // 
            this.cbFilter_Packet.AutoSize = true;
            this.cbFilter_Packet.Location = new System.Drawing.Point(17, 81);
            this.cbFilter_Packet.Name = "cbFilter_Packet";
            this.cbFilter_Packet.Size = new System.Drawing.Size(99, 21);
            this.cbFilter_Packet.TabIndex = 39;
            this.cbFilter_Packet.Text = "过滤封包内容";
            this.cbFilter_Packet.UseVisualStyleBackColor = true;
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
            // txtFilter_IP
            // 
            this.txtFilter_IP.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter_IP.Location = new System.Drawing.Point(116, 51);
            this.txtFilter_IP.Name = "txtFilter_IP";
            this.txtFilter_IP.Size = new System.Drawing.Size(399, 22);
            this.txtFilter_IP.TabIndex = 38;
            this.txtFilter_IP.WordWrap = false;
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
            // 
            // cbFilter_Size
            // 
            this.cbFilter_Size.AutoSize = true;
            this.cbFilter_Size.Location = new System.Drawing.Point(10, 14);
            this.cbFilter_Size.Name = "cbFilter_Size";
            this.cbFilter_Size.Size = new System.Drawing.Size(75, 21);
            this.cbFilter_Size.TabIndex = 27;
            this.cbFilter_Size.Text = "封包长度";
            this.cbFilter_Size.UseVisualStyleBackColor = true;
            // 
            // txtFilter_Size_From
            // 
            this.txtFilter_Size_From.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter_Size_From.Location = new System.Drawing.Point(15, 41);
            this.txtFilter_Size_From.Name = "txtFilter_Size_From";
            this.txtFilter_Size_From.Size = new System.Drawing.Size(57, 22);
            this.txtFilter_Size_From.TabIndex = 28;
            this.txtFilter_Size_From.Text = "0";
            this.txtFilter_Size_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFilter_Size_To
            // 
            this.txtFilter_Size_To.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter_Size_To.Location = new System.Drawing.Point(15, 74);
            this.txtFilter_Size_To.Name = "txtFilter_Size_To";
            this.txtFilter_Size_To.Size = new System.Drawing.Size(57, 22);
            this.txtFilter_Size_To.TabIndex = 29;
            this.txtFilter_Size_To.Text = "100";
            this.txtFilter_Size_To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // txtFilter_Packet
            // 
            this.txtFilter_Packet.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter_Packet.Location = new System.Drawing.Point(116, 79);
            this.txtFilter_Packet.Name = "txtFilter_Packet";
            this.txtFilter_Packet.Size = new System.Drawing.Size(399, 22);
            this.txtFilter_Packet.TabIndex = 40;
            this.txtFilter_Packet.WordWrap = false;
            // 
            // gbFilter_Size
            // 
            this.gbFilter_Size.Controls.Add(this.cbFilter_Size);
            this.gbFilter_Size.Controls.Add(this.txtFilter_Size_From);
            this.gbFilter_Size.Controls.Add(this.txtFilter_Size_To);
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
            // 
            // gbBottom
            // 
            this.gbBottom.Controls.Add(this.tcPacketInfo);
            this.gbBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbBottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbBottom.Location = new System.Drawing.Point(0, 471);
            this.gbBottom.Margin = new System.Windows.Forms.Padding(0);
            this.gbBottom.Name = "gbBottom";
            this.gbBottom.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbBottom.Size = new System.Drawing.Size(883, 170);
            this.gbBottom.TabIndex = 43;
            this.gbBottom.TabStop = false;
            this.gbBottom.Text = "[ 数据显示方式 ]";
            // 
            // tcPacketInfo
            // 
            this.tcPacketInfo.Controls.Add(this.tpHEX);
            this.tcPacketInfo.Controls.Add(this.tpDEC);
            this.tcPacketInfo.Controls.Add(this.tpBIN);
            this.tcPacketInfo.Controls.Add(this.tpUNICODE);
            this.tcPacketInfo.Controls.Add(this.tpUTF8);
            this.tcPacketInfo.Controls.Add(this.tpGB2312);
            this.tcPacketInfo.Controls.Add(this.tpDebug);
            this.tcPacketInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPacketInfo.Location = new System.Drawing.Point(3, 20);
            this.tcPacketInfo.Name = "tcPacketInfo";
            this.tcPacketInfo.SelectedIndex = 0;
            this.tcPacketInfo.Size = new System.Drawing.Size(877, 146);
            this.tcPacketInfo.TabIndex = 10;
            this.tcPacketInfo.SelectedIndexChanged += new System.EventHandler(this.tcPacketInfo_SelectedIndexChanged);
            // 
            // tpHEX
            // 
            this.tpHEX.Controls.Add(this.rtbHEX);
            this.tpHEX.Location = new System.Drawing.Point(4, 26);
            this.tpHEX.Name = "tpHEX";
            this.tpHEX.Size = new System.Drawing.Size(869, 116);
            this.tpHEX.TabIndex = 0;
            this.tpHEX.Text = "十六进制";
            this.tpHEX.UseVisualStyleBackColor = true;
            // 
            // rtbHEX
            // 
            this.rtbHEX.BackColor = System.Drawing.SystemColors.Window;
            this.rtbHEX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHEX.Location = new System.Drawing.Point(0, 0);
            this.rtbHEX.Name = "rtbHEX";
            this.rtbHEX.Size = new System.Drawing.Size(869, 116);
            this.rtbHEX.TabIndex = 3;
            this.rtbHEX.Text = "";
            // 
            // tpDEC
            // 
            this.tpDEC.Controls.Add(this.rtbDEC);
            this.tpDEC.Location = new System.Drawing.Point(4, 26);
            this.tpDEC.Name = "tpDEC";
            this.tpDEC.Size = new System.Drawing.Size(869, 116);
            this.tpDEC.TabIndex = 1;
            this.tpDEC.Text = "十进制";
            this.tpDEC.UseVisualStyleBackColor = true;
            // 
            // rtbDEC
            // 
            this.rtbDEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDEC.Location = new System.Drawing.Point(0, 0);
            this.rtbDEC.Name = "rtbDEC";
            this.rtbDEC.Size = new System.Drawing.Size(869, 116);
            this.rtbDEC.TabIndex = 1;
            this.rtbDEC.Text = "";
            // 
            // tpBIN
            // 
            this.tpBIN.Controls.Add(this.rtbBIN);
            this.tpBIN.Location = new System.Drawing.Point(4, 26);
            this.tpBIN.Name = "tpBIN";
            this.tpBIN.Size = new System.Drawing.Size(869, 116);
            this.tpBIN.TabIndex = 2;
            this.tpBIN.Text = "二进制";
            this.tpBIN.UseVisualStyleBackColor = true;
            // 
            // rtbBIN
            // 
            this.rtbBIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbBIN.Location = new System.Drawing.Point(0, 0);
            this.rtbBIN.Name = "rtbBIN";
            this.rtbBIN.Size = new System.Drawing.Size(869, 116);
            this.rtbBIN.TabIndex = 3;
            this.rtbBIN.Text = "";
            // 
            // tpUNICODE
            // 
            this.tpUNICODE.Controls.Add(this.rtbUNICODE);
            this.tpUNICODE.Location = new System.Drawing.Point(4, 26);
            this.tpUNICODE.Name = "tpUNICODE";
            this.tpUNICODE.Size = new System.Drawing.Size(869, 116);
            this.tpUNICODE.TabIndex = 3;
            this.tpUNICODE.Text = "Unicode";
            this.tpUNICODE.UseVisualStyleBackColor = true;
            // 
            // rtbUNICODE
            // 
            this.rtbUNICODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbUNICODE.Location = new System.Drawing.Point(0, 0);
            this.rtbUNICODE.Name = "rtbUNICODE";
            this.rtbUNICODE.Size = new System.Drawing.Size(869, 116);
            this.rtbUNICODE.TabIndex = 2;
            this.rtbUNICODE.Text = "";
            // 
            // tpUTF8
            // 
            this.tpUTF8.Controls.Add(this.rtbUTF8);
            this.tpUTF8.Location = new System.Drawing.Point(4, 26);
            this.tpUTF8.Name = "tpUTF8";
            this.tpUTF8.Size = new System.Drawing.Size(869, 116);
            this.tpUTF8.TabIndex = 4;
            this.tpUTF8.Text = "UTF-8";
            this.tpUTF8.UseVisualStyleBackColor = true;
            // 
            // rtbUTF8
            // 
            this.rtbUTF8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbUTF8.Location = new System.Drawing.Point(0, 0);
            this.rtbUTF8.Name = "rtbUTF8";
            this.rtbUTF8.Size = new System.Drawing.Size(869, 116);
            this.rtbUTF8.TabIndex = 2;
            this.rtbUTF8.Text = "";
            // 
            // tpGB2312
            // 
            this.tpGB2312.Controls.Add(this.rtbGB2312);
            this.tpGB2312.Location = new System.Drawing.Point(4, 26);
            this.tpGB2312.Name = "tpGB2312";
            this.tpGB2312.Size = new System.Drawing.Size(869, 116);
            this.tpGB2312.TabIndex = 5;
            this.tpGB2312.Text = "GB2312";
            this.tpGB2312.UseVisualStyleBackColor = true;
            // 
            // rtbGB2312
            // 
            this.rtbGB2312.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGB2312.Location = new System.Drawing.Point(0, 0);
            this.rtbGB2312.Name = "rtbGB2312";
            this.rtbGB2312.Size = new System.Drawing.Size(869, 116);
            this.rtbGB2312.TabIndex = 2;
            this.rtbGB2312.Text = "";
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.rtbDEBUG);
            this.tpDebug.Location = new System.Drawing.Point(4, 26);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Size = new System.Drawing.Size(869, 116);
            this.tpDebug.TabIndex = 7;
            this.tpDebug.Text = "调试信息";
            this.tpDebug.UseVisualStyleBackColor = true;
            // 
            // rtbDEBUG
            // 
            this.rtbDEBUG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDEBUG.Location = new System.Drawing.Point(0, 0);
            this.rtbDEBUG.Name = "rtbDEBUG";
            this.rtbDEBUG.Size = new System.Drawing.Size(869, 116);
            this.rtbDEBUG.TabIndex = 0;
            this.rtbDEBUG.Text = "";
            // 
            // bgwSocketInfo
            // 
            this.bgwSocketInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketInfo_DoWork);
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
            // txtFilter_Socket
            // 
            this.txtFilter_Socket.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter_Socket.Location = new System.Drawing.Point(116, 24);
            this.txtFilter_Socket.Name = "txtFilter_Socket";
            this.txtFilter_Socket.Size = new System.Drawing.Size(399, 22);
            this.txtFilter_Socket.TabIndex = 36;
            this.txtFilter_Socket.WordWrap = false;
            // 
            // cbFilter_Socket
            // 
            this.cbFilter_Socket.AutoSize = true;
            this.cbFilter_Socket.Location = new System.Drawing.Point(17, 26);
            this.cbFilter_Socket.Name = "cbFilter_Socket";
            this.cbFilter_Socket.Size = new System.Drawing.Size(87, 21);
            this.cbFilter_Socket.TabIndex = 35;
            this.cbFilter_Socket.Text = "过滤套接字";
            this.cbFilter_Socket.UseVisualStyleBackColor = true;
            // 
            // cbFilter_IP
            // 
            this.cbFilter_IP.AutoSize = true;
            this.cbFilter_IP.Location = new System.Drawing.Point(17, 53);
            this.cbFilter_IP.Name = "cbFilter_IP";
            this.cbFilter_IP.Size = new System.Drawing.Size(86, 21);
            this.cbFilter_IP.TabIndex = 37;
            this.cbFilter_IP.Text = "过滤IP地址";
            this.cbFilter_IP.UseVisualStyleBackColor = true;
            // 
            // cmsSocketInfo
            // 
            this.cmsSocketInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSend,
            this.toolStripSeparator1,
            this.tsmiBatchSend,
            this.toolStripSeparator2,
            this.tsmiUseSocket,
            this.toolStripSeparator3,
            this.tsmiShowBatchSend,
            this.toolStripSeparator4,
            this.tsmiSaveSocketInfo});
            this.cmsSocketInfo.Name = "cmsSocketInfo";
            this.cmsSocketInfo.Size = new System.Drawing.Size(161, 138);
            this.cmsSocketInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketInfo_ItemClicked);
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
            // tlFilter_CNT
            // 
            this.tlFilter_CNT.Name = "tlFilter_CNT";
            this.tlFilter_CNT.Size = new System.Drawing.Size(15, 17);
            this.tlFilter_CNT.Text = "0";
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
            this.tlFilter,
            this.tlFilter_CNT,
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
            // tlFilter
            // 
            this.tlFilter.Name = "tlFilter";
            this.tlFilter.Size = new System.Drawing.Size(35, 17);
            this.tlFilter.Text = "过滤:";
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
            this.gbRight.Controls.Add(this.txtFilter_Packet);
            this.gbRight.Controls.Add(this.gbFilter_Size);
            this.gbRight.Controls.Add(this.cbFilter_Packet);
            this.gbRight.Controls.Add(this.bStopHook);
            this.gbRight.Controls.Add(this.txtFilter_IP);
            this.gbRight.Controls.Add(this.gbFilter_Type);
            this.gbRight.Controls.Add(this.cbFilter_IP);
            this.gbRight.Controls.Add(this.bStartHook);
            this.gbRight.Controls.Add(this.txtFilter_Socket);
            this.gbRight.Controls.Add(this.cbFilter_Socket);
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
            // 
            // gbSearch_Bottom
            // 
            this.gbSearch_Bottom.Controls.Add(this.bSearch);
            this.gbSearch_Bottom.Controls.Add(this.txtSearch);
            this.gbSearch_Bottom.Controls.Add(this.lSearch);
            this.gbSearch_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbSearch_Bottom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbSearch_Bottom.Location = new System.Drawing.Point(0, 417);
            this.gbSearch_Bottom.Name = "gbSearch_Bottom";
            this.gbSearch_Bottom.Size = new System.Drawing.Size(883, 54);
            this.gbSearch_Bottom.TabIndex = 48;
            this.gbSearch_Bottom.TabStop = false;
            this.gbSearch_Bottom.Text = "[ 数据搜索 ]";
            // 
            // bSearch
            // 
            this.bSearch.Location = new System.Drawing.Point(811, 18);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(60, 28);
            this.bSearch.TabIndex = 2;
            this.bSearch.Text = "搜 索";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(153, 21);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(641, 23);
            this.txtSearch.TabIndex = 1;
            // 
            // lSearch
            // 
            this.lSearch.AutoSize = true;
            this.lSearch.Location = new System.Drawing.Point(14, 25);
            this.lSearch.Name = "lSearch";
            this.lSearch.Size = new System.Drawing.Size(140, 17);
            this.lSearch.TabIndex = 0;
            this.lSearch.Text = "搜索内容（十六进制）：";
            // 
            // dgSocketInfo
            // 
            this.dgSocketInfo.AllowUserToAddRows = false;
            this.dgSocketInfo.AllowUserToDeleteRows = false;
            this.dgSocketInfo.AllowUserToResizeRows = false;
            this.dgSocketInfo.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgSocketInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSocketInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIndex,
            this.cType,
            this.cSocket,
            this.cFrom,
            this.Column5,
            this.cLen,
            this.cData});
            this.dgSocketInfo.ContextMenuStrip = this.cmsSocketInfo;
            this.dgSocketInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSocketInfo.Location = new System.Drawing.Point(0, 138);
            this.dgSocketInfo.MultiSelect = false;
            this.dgSocketInfo.Name = "dgSocketInfo";
            this.dgSocketInfo.RowHeadersVisible = false;
            this.dgSocketInfo.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgSocketInfo.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgSocketInfo.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgSocketInfo.RowTemplate.Height = 23;
            this.dgSocketInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSocketInfo.Size = new System.Drawing.Size(883, 279);
            this.dgSocketInfo.TabIndex = 49;
            this.dgSocketInfo.SelectionChanged += new System.EventHandler(this.dgSocketInfo_SelectionChanged);
            // 
            // cIndex
            // 
            this.cIndex.DataPropertyName = "Index";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle1;
            this.cIndex.HeaderText = "序号";
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIndex.Width = 50;
            // 
            // cType
            // 
            this.cType.DataPropertyName = "Type";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cType.DefaultCellStyle = dataGridViewCellStyle2;
            this.cType.HeaderText = "类别";
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cType.Width = 55;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle3;
            this.cSocket.HeaderText = "套接字";
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cSocket.Width = 60;
            // 
            // cFrom
            // 
            this.cFrom.DataPropertyName = "From";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cFrom.DefaultCellStyle = dataGridViewCellStyle4;
            this.cFrom.HeaderText = "源地址";
            this.cFrom.Name = "cFrom";
            this.cFrom.ReadOnly = true;
            this.cFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cFrom.Width = 150;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "To";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column5.HeaderText = "目的地址";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 150;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "Length";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle6;
            this.cLen.HeaderText = "长度";
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLen.Width = 50;
            // 
            // cData
            // 
            this.cData.DataPropertyName = "Data";
            this.cData.HeaderText = "数据";
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cData.Width = 340;
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
            this.tsmiSaveSocketInfo.Text = "保存此列表数据";
            // 
            // sfdSocketInfo
            // 
            this.sfdSocketInfo.Filter = "封包数据文件（*.txt）|*.txt";
            this.sfdSocketInfo.RestoreDirectory = true;
            this.sfdSocketInfo.Title = "保存封包数据";
            // 
            // DLL_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(883, 641);
            this.Controls.Add(this.dgSocketInfo);
            this.Controls.Add(this.gbSearch_Bottom);
            this.Controls.Add(this.gbBottom);
            this.Controls.Add(this.ssStatusInfo_Top);
            this.Controls.Add(this.gbRight);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "DLL_Form";
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
            this.tpDebug.ResumeLayout(false);
            this.cmsSocketInfo.ResumeLayout(false);
            this.ssStatusInfo_Top.ResumeLayout(false);
            this.ssStatusInfo_Top.PerformLayout();
            this.gbRight.ResumeLayout(false);
            this.gbRight.PerformLayout();
            this.gbClear.ResumeLayout(false);
            this.gbClear.PerformLayout();
            this.gbSearch_Bottom.ResumeLayout(false);
            this.gbSearch_Bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSocketInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbDisplay_SendTo;
        private System.Windows.Forms.CheckBox cbFilter_Packet;
        private System.Windows.Forms.Button bStopHook;
        private System.Windows.Forms.TextBox txtFilter_IP;
        private System.Windows.Forms.CheckBox cbDisplay_RecvFrom;
        private System.Windows.Forms.CheckBox cbFilter_Size;
        private System.Windows.Forms.TextBox txtFilter_Size_From;
        private System.Windows.Forms.TextBox txtFilter_Size_To;
        private System.Windows.Forms.CheckBox cbInterecept_Recv;
        private System.Windows.Forms.CheckBox cbInterecept_SendTo;
        private System.Windows.Forms.Label lSplit;
        private System.Windows.Forms.TextBox txtFilter_Packet;
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
        private System.Windows.Forms.TabPage tpDebug;
        private System.Windows.Forms.RichTextBox rtbDEBUG;
        private System.ComponentModel.BackgroundWorker bgwSocketInfo;
        private System.Windows.Forms.Timer tSocketInfo;
        private System.Windows.Forms.Button bStartHook;
        private System.Windows.Forms.TextBox txtFilter_Socket;
        private System.Windows.Forms.CheckBox cbFilter_Socket;
        private System.Windows.Forms.CheckBox cbFilter_IP;
        private System.Windows.Forms.ContextMenuStrip cmsSocketInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiSend;
        private System.Windows.Forms.ToolStripStatusLabel tlFilter_CNT;
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
        private System.Windows.Forms.ToolStripStatusLabel tlFilter;
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
        private System.Windows.Forms.DataGridView dgSocketInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchSend;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowBatchSend;
        private System.Windows.Forms.ToolStripMenuItem tsmiUseSocket;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveSocketInfo;
        private System.Windows.Forms.SaveFileDialog sfdSocketInfo;
    }
}
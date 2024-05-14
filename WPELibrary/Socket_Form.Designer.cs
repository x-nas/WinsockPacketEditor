
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.cmsLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tslClearList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowHook = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiHideHook = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslToExcel = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tlSplit0 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlALL = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlALL_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlCheck = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlInterecept = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlInterecept_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbRight = new System.Windows.Forms.GroupBox();
            this.gbClear = new System.Windows.Forms.GroupBox();
            this.cbReset_CNT = new System.Windows.Forms.CheckBox();
            this.gbSearch = new System.Windows.Forms.GroupBox();
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
            this.cIsCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cFNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsFilterList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiShowFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAddFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDeleteFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveFilterList = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiLoadFilterList = new System.Windows.Forms.ToolStripMenuItem();
            this.bgwLogList = new System.ComponentModel.BackgroundWorker();
            this.bgwSendFrom = new System.ComponentModel.BackgroundWorker();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.bgwSocketInfo = new System.ComponentModel.BackgroundWorker();
            this.pPacketInfo = new System.Windows.Forms.Panel();
            this.tcPacketInfo = new System.Windows.Forms.TabControl();
            this.tpPacketInfo = new System.Windows.Forms.TabPage();
            this.tlpPacketInfo = new System.Windows.Forms.TableLayoutPanel();
            this.rtbPackInfo_Left = new System.Windows.Forms.RichTextBox();
            this.rtbPacketInfo_Right = new System.Windows.Forms.RichTextBox();
            this.cbPacketInfo_Left = new System.Windows.Forms.ComboBox();
            this.cbPacketInfo_Right = new System.Windows.Forms.ComboBox();
            this.tpSystemLog = new System.Windows.Forms.TabPage();
            this.dgvLogList = new System.Windows.Forms.DataGridView();
            this.cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFuncName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbFilter_Size.SuspendLayout();
            this.gbFilter_Type.SuspendLayout();
            this.cmsLogList.SuspendLayout();
            this.cmsSocketList.SuspendLayout();
            this.ssStatusInfo_Top.SuspendLayout();
            this.gbRight.SuspendLayout();
            this.gbClear.SuspendLayout();
            this.gbSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).BeginInit();
            this.gbFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).BeginInit();
            this.cmsFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.pPacketInfo.SuspendLayout();
            this.tcPacketInfo.SuspendLayout();
            this.tpPacketInfo.SuspendLayout();
            this.tlpPacketInfo.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            this.SuspendLayout();
            // 
            // cbDisplay_SendTo
            // 
            resources.ApplyResources(this.cbDisplay_SendTo, "cbDisplay_SendTo");
            this.cbDisplay_SendTo.Checked = true;
            this.cbDisplay_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_SendTo.Name = "cbDisplay_SendTo";
            this.cbDisplay_SendTo.UseVisualStyleBackColor = true;
            this.cbDisplay_SendTo.CheckedChanged += new System.EventHandler(this.cbDisplay_SendTo_CheckedChanged);
            // 
            // cbCheck_Packet
            // 
            resources.ApplyResources(this.cbCheck_Packet, "cbCheck_Packet");
            this.cbCheck_Packet.Name = "cbCheck_Packet";
            this.cbCheck_Packet.UseVisualStyleBackColor = true;
            this.cbCheck_Packet.CheckedChanged += new System.EventHandler(this.cbCheck_Packet_CheckedChanged);
            // 
            // bStopHook
            // 
            resources.ApplyResources(this.bStopHook, "bStopHook");
            this.bStopHook.Name = "bStopHook";
            this.bStopHook.UseVisualStyleBackColor = true;
            this.bStopHook.Click += new System.EventHandler(this.bStopHook_Click);
            // 
            // txtCheck_IP
            // 
            resources.ApplyResources(this.txtCheck_IP, "txtCheck_IP");
            this.txtCheck_IP.Name = "txtCheck_IP";
            // 
            // cbDisplay_RecvFrom
            // 
            resources.ApplyResources(this.cbDisplay_RecvFrom, "cbDisplay_RecvFrom");
            this.cbDisplay_RecvFrom.Checked = true;
            this.cbDisplay_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_RecvFrom.Name = "cbDisplay_RecvFrom";
            this.cbDisplay_RecvFrom.UseVisualStyleBackColor = true;
            this.cbDisplay_RecvFrom.CheckedChanged += new System.EventHandler(this.cbDisplay_RecvFrom_CheckedChanged);
            // 
            // cbCheck_Size
            // 
            resources.ApplyResources(this.cbCheck_Size, "cbCheck_Size");
            this.cbCheck_Size.Name = "cbCheck_Size";
            this.cbCheck_Size.UseVisualStyleBackColor = true;
            this.cbCheck_Size.CheckedChanged += new System.EventHandler(this.cbCheck_Size_CheckedChanged);
            // 
            // txtCheck_Size_From
            // 
            resources.ApplyResources(this.txtCheck_Size_From, "txtCheck_Size_From");
            this.txtCheck_Size_From.Name = "txtCheck_Size_From";
            // 
            // txtCheck_Size_To
            // 
            resources.ApplyResources(this.txtCheck_Size_To, "txtCheck_Size_To");
            this.txtCheck_Size_To.Name = "txtCheck_Size_To";
            // 
            // cbInterecept_Recv
            // 
            resources.ApplyResources(this.cbInterecept_Recv, "cbInterecept_Recv");
            this.cbInterecept_Recv.Name = "cbInterecept_Recv";
            this.cbInterecept_Recv.UseVisualStyleBackColor = true;
            this.cbInterecept_Recv.CheckedChanged += new System.EventHandler(this.cbInterecept_Recv_CheckedChanged);
            // 
            // cbInterecept_SendTo
            // 
            resources.ApplyResources(this.cbInterecept_SendTo, "cbInterecept_SendTo");
            this.cbInterecept_SendTo.Name = "cbInterecept_SendTo";
            this.cbInterecept_SendTo.UseVisualStyleBackColor = true;
            this.cbInterecept_SendTo.CheckedChanged += new System.EventHandler(this.cbInterecept_SendTo_CheckedChanged);
            // 
            // lSplit
            // 
            resources.ApplyResources(this.lSplit, "lSplit");
            this.lSplit.Name = "lSplit";
            // 
            // txtCheck_Packet
            // 
            resources.ApplyResources(this.txtCheck_Packet, "txtCheck_Packet");
            this.txtCheck_Packet.Name = "txtCheck_Packet";
            // 
            // gbFilter_Size
            // 
            this.gbFilter_Size.Controls.Add(this.cbCheck_Size);
            this.gbFilter_Size.Controls.Add(this.txtCheck_Size_From);
            this.gbFilter_Size.Controls.Add(this.txtCheck_Size_To);
            this.gbFilter_Size.Controls.Add(this.lSplit);
            resources.ApplyResources(this.gbFilter_Size, "gbFilter_Size");
            this.gbFilter_Size.Name = "gbFilter_Size";
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
            resources.ApplyResources(this.gbFilter_Type, "gbFilter_Type");
            this.gbFilter_Type.Name = "gbFilter_Type";
            this.gbFilter_Type.TabStop = false;
            // 
            // cbInterecept_RecvFrom
            // 
            resources.ApplyResources(this.cbInterecept_RecvFrom, "cbInterecept_RecvFrom");
            this.cbInterecept_RecvFrom.Name = "cbInterecept_RecvFrom";
            this.cbInterecept_RecvFrom.UseVisualStyleBackColor = true;
            this.cbInterecept_RecvFrom.CheckedChanged += new System.EventHandler(this.cbInterecept_RecvFrom_CheckedChanged);
            // 
            // cbInterecept_Send
            // 
            resources.ApplyResources(this.cbInterecept_Send, "cbInterecept_Send");
            this.cbInterecept_Send.Name = "cbInterecept_Send";
            this.cbInterecept_Send.UseVisualStyleBackColor = true;
            this.cbInterecept_Send.CheckedChanged += new System.EventHandler(this.cbInterecept_Send_CheckedChanged);
            // 
            // cbDisplay_Recv
            // 
            resources.ApplyResources(this.cbDisplay_Recv, "cbDisplay_Recv");
            this.cbDisplay_Recv.Checked = true;
            this.cbDisplay_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_Recv.Name = "cbDisplay_Recv";
            this.cbDisplay_Recv.UseVisualStyleBackColor = true;
            this.cbDisplay_Recv.CheckedChanged += new System.EventHandler(this.cbDisplay_Recv_CheckedChanged);
            // 
            // cbDisplay_Send
            // 
            resources.ApplyResources(this.cbDisplay_Send, "cbDisplay_Send");
            this.cbDisplay_Send.Checked = true;
            this.cbDisplay_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplay_Send.Name = "cbDisplay_Send";
            this.cbDisplay_Send.UseVisualStyleBackColor = true;
            this.cbDisplay_Send.CheckedChanged += new System.EventHandler(this.cbDisplay_Send_CheckedChanged);
            // 
            // cmsLogList
            // 
            this.cmsLogList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslClearList,
            this.toolStripSeparator8,
            this.tsmiShowHook,
            this.toolStripSeparator9,
            this.tsmiHideHook,
            this.toolStripSeparator5,
            this.tslToExcel});
            this.cmsLogList.Name = "cmsLogList";
            resources.ApplyResources(this.cmsLogList, "cmsLogList");
            this.cmsLogList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsLogList_ItemClicked);
            // 
            // tslClearList
            // 
            this.tslClearList.Name = "tslClearList";
            resources.ApplyResources(this.tslClearList, "tslClearList");
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // tsmiShowHook
            // 
            this.tsmiShowHook.Name = "tsmiShowHook";
            resources.ApplyResources(this.tsmiShowHook, "tsmiShowHook");
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // tsmiHideHook
            // 
            this.tsmiHideHook.Name = "tsmiHideHook";
            resources.ApplyResources(this.tsmiHideHook, "tsmiHideHook");
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // tslToExcel
            // 
            this.tslToExcel.Name = "tslToExcel";
            resources.ApplyResources(this.tslToExcel, "tslToExcel");
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
            resources.ApplyResources(this.bStartHook, "bStartHook");
            this.bStartHook.Name = "bStartHook";
            this.bStartHook.UseVisualStyleBackColor = true;
            this.bStartHook.Click += new System.EventHandler(this.bStartHook_Click);
            // 
            // txtCheck_Socket
            // 
            resources.ApplyResources(this.txtCheck_Socket, "txtCheck_Socket");
            this.txtCheck_Socket.Name = "txtCheck_Socket";
            // 
            // cbCheck_Socket
            // 
            resources.ApplyResources(this.cbCheck_Socket, "cbCheck_Socket");
            this.cbCheck_Socket.Name = "cbCheck_Socket";
            this.cbCheck_Socket.UseVisualStyleBackColor = true;
            this.cbCheck_Socket.CheckedChanged += new System.EventHandler(this.cbCheck_Socket_CheckedChanged);
            // 
            // cbCheck_IP
            // 
            resources.ApplyResources(this.cbCheck_IP, "cbCheck_IP");
            this.cbCheck_IP.Name = "cbCheck_IP";
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
            resources.ApplyResources(this.cmsSocketList, "cmsSocketList");
            this.cmsSocketList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketInfo_ItemClicked);
            // 
            // tsmiSend
            // 
            this.tsmiSend.Name = "tsmiSend";
            resources.ApplyResources(this.tsmiSend, "tsmiSend");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsmiBatchSend
            // 
            this.tsmiBatchSend.Name = "tsmiBatchSend";
            resources.ApplyResources(this.tsmiBatchSend, "tsmiBatchSend");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsmiUseSocket
            // 
            this.tsmiUseSocket.Name = "tsmiUseSocket";
            resources.ApplyResources(this.tsmiUseSocket, "tsmiUseSocket");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsmiShowBatchSend
            // 
            this.tsmiShowBatchSend.Name = "tsmiShowBatchSend";
            resources.ApplyResources(this.tsmiShowBatchSend, "tsmiShowBatchSend");
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // tsmiSaveSocketInfo
            // 
            this.tsmiSaveSocketInfo.Name = "tsmiSaveSocketInfo";
            resources.ApplyResources(this.tsmiSaveSocketInfo, "tsmiSaveSocketInfo");
            // 
            // tlCheck_CNT
            // 
            this.tlCheck_CNT.Name = "tlCheck_CNT";
            resources.ApplyResources(this.tlCheck_CNT, "tlCheck_CNT");
            // 
            // ssStatusInfo_Top
            // 
            resources.ApplyResources(this.ssStatusInfo_Top, "ssStatusInfo_Top");
            this.ssStatusInfo_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlSystemInfo,
            this.tlSplit0,
            this.tlALL,
            this.tlALL_CNT,
            this.tlSplit1,
            this.tlQueue,
            this.tlQueue_CNT,
            this.tlSplit2,
            this.tlSend,
            this.tlSend_CNT,
            this.tlSplit3,
            this.tlRecv,
            this.tlRecv_CNT,
            this.tlSplit4,
            this.tlCheck,
            this.tlCheck_CNT,
            this.tlSplit5,
            this.tlInterecept,
            this.tlInterecept_CNT});
            this.ssStatusInfo_Top.Name = "ssStatusInfo_Top";
            this.ssStatusInfo_Top.SizingGrip = false;
            // 
            // tlSystemInfo
            // 
            this.tlSystemInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlSystemInfo.Name = "tlSystemInfo";
            resources.ApplyResources(this.tlSystemInfo, "tlSystemInfo");
            // 
            // tlSplit0
            // 
            this.tlSplit0.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit0.Name = "tlSplit0";
            resources.ApplyResources(this.tlSplit0, "tlSplit0");
            // 
            // tlALL
            // 
            resources.ApplyResources(this.tlALL, "tlALL");
            this.tlALL.Name = "tlALL";
            // 
            // tlALL_CNT
            // 
            resources.ApplyResources(this.tlALL_CNT, "tlALL_CNT");
            this.tlALL_CNT.Name = "tlALL_CNT";
            // 
            // tlSplit1
            // 
            this.tlSplit1.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit1.Name = "tlSplit1";
            resources.ApplyResources(this.tlSplit1, "tlSplit1");
            // 
            // tlQueue
            // 
            resources.ApplyResources(this.tlQueue, "tlQueue");
            this.tlQueue.Name = "tlQueue";
            // 
            // tlQueue_CNT
            // 
            this.tlQueue_CNT.Name = "tlQueue_CNT";
            resources.ApplyResources(this.tlQueue_CNT, "tlQueue_CNT");
            // 
            // tlSplit2
            // 
            this.tlSplit2.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit2.Name = "tlSplit2";
            resources.ApplyResources(this.tlSplit2, "tlSplit2");
            // 
            // tlSend
            // 
            this.tlSend.Name = "tlSend";
            resources.ApplyResources(this.tlSend, "tlSend");
            // 
            // tlSend_CNT
            // 
            this.tlSend_CNT.Name = "tlSend_CNT";
            resources.ApplyResources(this.tlSend_CNT, "tlSend_CNT");
            // 
            // tlSplit3
            // 
            this.tlSplit3.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit3.Name = "tlSplit3";
            resources.ApplyResources(this.tlSplit3, "tlSplit3");
            // 
            // tlRecv
            // 
            resources.ApplyResources(this.tlRecv, "tlRecv");
            this.tlRecv.Name = "tlRecv";
            // 
            // tlRecv_CNT
            // 
            this.tlRecv_CNT.Name = "tlRecv_CNT";
            resources.ApplyResources(this.tlRecv_CNT, "tlRecv_CNT");
            // 
            // tlSplit4
            // 
            this.tlSplit4.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit4.Name = "tlSplit4";
            resources.ApplyResources(this.tlSplit4, "tlSplit4");
            // 
            // tlCheck
            // 
            this.tlCheck.Name = "tlCheck";
            resources.ApplyResources(this.tlCheck, "tlCheck");
            // 
            // tlSplit5
            // 
            this.tlSplit5.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit5.Name = "tlSplit5";
            resources.ApplyResources(this.tlSplit5, "tlSplit5");
            // 
            // tlInterecept
            // 
            this.tlInterecept.Name = "tlInterecept";
            resources.ApplyResources(this.tlInterecept, "tlInterecept");
            // 
            // tlInterecept_CNT
            // 
            this.tlInterecept_CNT.Name = "tlInterecept_CNT";
            resources.ApplyResources(this.tlInterecept_CNT, "tlInterecept_CNT");
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
            resources.ApplyResources(this.gbRight, "gbRight");
            this.gbRight.Name = "gbRight";
            this.gbRight.TabStop = false;
            // 
            // gbClear
            // 
            this.gbClear.Controls.Add(this.cbReset_CNT);
            resources.ApplyResources(this.gbClear, "gbClear");
            this.gbClear.Name = "gbClear";
            this.gbClear.TabStop = false;
            // 
            // cbReset_CNT
            // 
            resources.ApplyResources(this.cbReset_CNT, "cbReset_CNT");
            this.cbReset_CNT.Checked = true;
            this.cbReset_CNT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReset_CNT.Name = "cbReset_CNT";
            this.cbReset_CNT.UseVisualStyleBackColor = true;
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.rbSearchFrom);
            this.gbSearch.Controls.Add(this.rbSearchAll);
            this.gbSearch.Controls.Add(this.bSearch);
            this.gbSearch.Controls.Add(this.txtSearch);
            this.gbSearch.Controls.Add(this.lSearch);
            resources.ApplyResources(this.gbSearch, "gbSearch");
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.TabStop = false;
            // 
            // rbSearchFrom
            // 
            resources.ApplyResources(this.rbSearchFrom, "rbSearchFrom");
            this.rbSearchFrom.Checked = true;
            this.rbSearchFrom.Name = "rbSearchFrom";
            this.rbSearchFrom.TabStop = true;
            this.rbSearchFrom.UseVisualStyleBackColor = true;
            // 
            // rbSearchAll
            // 
            resources.ApplyResources(this.rbSearchAll, "rbSearchAll");
            this.rbSearchAll.Name = "rbSearchAll";
            this.rbSearchAll.UseVisualStyleBackColor = true;
            // 
            // bSearch
            // 
            resources.ApplyResources(this.bSearch, "bSearch");
            this.bSearch.Name = "bSearch";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            // 
            // lSearch
            // 
            resources.ApplyResources(this.lSearch, "lSearch");
            this.lSearch.Name = "lSearch";
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
            resources.ApplyResources(this.dgvSocketList, "dgvSocketList");
            this.dgvSocketList.MultiSelect = false;
            this.dgvSocketList.Name = "dgvSocketList";
            this.dgvSocketList.RowHeadersVisible = false;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowsDefaultCellStyle = dataGridViewCellStyle34;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgvSocketList.RowTemplate.Height = 23;
            this.dgvSocketList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSocketList.SelectionChanged += new System.EventHandler(this.dgSocketInfo_SelectionChanged);
            // 
            // cIndex
            // 
            this.cIndex.DataPropertyName = "Index";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.cIndex, "cIndex");
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cType
            // 
            this.cType.DataPropertyName = "Type_Name";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cType.DefaultCellStyle = dataGridViewCellStyle21;
            resources.ApplyResources(this.cType, "cType");
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle30;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFrom
            // 
            this.cFrom.DataPropertyName = "From";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cFrom.DefaultCellStyle = dataGridViewCellStyle31;
            resources.ApplyResources(this.cFrom, "cFrom");
            this.cFrom.Name = "cFrom";
            this.cFrom.ReadOnly = true;
            this.cFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "To";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle32;
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "ResLen";
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle33;
            resources.ApplyResources(this.cLen, "cLen");
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cData
            // 
            this.cData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cData.DataPropertyName = "Data";
            resources.ApplyResources(this.cData, "cData");
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // gbFilterList
            // 
            this.gbFilterList.Controls.Add(this.dgvFilterList);
            resources.ApplyResources(this.gbFilterList, "gbFilterList");
            this.gbFilterList.Name = "gbFilterList";
            this.gbFilterList.TabStop = false;
            // 
            // dgvFilterList
            // 
            this.dgvFilterList.AllowUserToAddRows = false;
            this.dgvFilterList.AllowUserToDeleteRows = false;
            this.dgvFilterList.AllowUserToResizeColumns = false;
            this.dgvFilterList.AllowUserToResizeRows = false;
            this.dgvFilterList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvFilterList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvFilterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterList.ColumnHeadersVisible = false;
            this.dgvFilterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIsCheck,
            this.cFNum,
            this.cFName});
            this.dgvFilterList.ContextMenuStrip = this.cmsFilterList;
            resources.ApplyResources(this.dgvFilterList, "dgvFilterList");
            this.dgvFilterList.MultiSelect = false;
            this.dgvFilterList.Name = "dgvFilterList";
            this.dgvFilterList.RowHeadersVisible = false;
            this.dgvFilterList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvFilterList.RowTemplate.Height = 23;
            this.dgvFilterList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilterList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterList_CellContentClick);
            // 
            // cIsCheck
            // 
            this.cIsCheck.DataPropertyName = "IsCheck";
            this.cIsCheck.FalseValue = "false";
            resources.ApplyResources(this.cIsCheck, "cIsCheck");
            this.cIsCheck.Name = "cIsCheck";
            this.cIsCheck.TrueValue = "true";
            // 
            // cFNum
            // 
            this.cFNum.DataPropertyName = "FNum";
            resources.ApplyResources(this.cFNum, "cFNum");
            this.cFNum.Name = "cFNum";
            this.cFNum.ReadOnly = true;
            // 
            // cFName
            // 
            this.cFName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cFName.DataPropertyName = "FName";
            resources.ApplyResources(this.cFName, "cFName");
            this.cFName.Name = "cFName";
            this.cFName.ReadOnly = true;
            // 
            // cmsFilterList
            // 
            this.cmsFilterList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowFilter,
            this.tssSep1,
            this.tsmiAddFilter,
            this.tssSep3,
            this.tsmiDeleteFilter,
            this.toolStripSeparator6,
            this.tsmiSaveFilterList,
            this.tssSep2,
            this.tsmiClearAll,
            this.toolStripSeparator7,
            this.tsmiLoadFilterList});
            this.cmsFilterList.Name = "cmsFilterList";
            resources.ApplyResources(this.cmsFilterList, "cmsFilterList");
            this.cmsFilterList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsFilterList_ItemClicked);
            // 
            // tsmiShowFilter
            // 
            this.tsmiShowFilter.Name = "tsmiShowFilter";
            resources.ApplyResources(this.tsmiShowFilter, "tsmiShowFilter");
            // 
            // tssSep1
            // 
            this.tssSep1.Name = "tssSep1";
            resources.ApplyResources(this.tssSep1, "tssSep1");
            // 
            // tsmiAddFilter
            // 
            this.tsmiAddFilter.Name = "tsmiAddFilter";
            resources.ApplyResources(this.tsmiAddFilter, "tsmiAddFilter");
            // 
            // tssSep3
            // 
            this.tssSep3.Name = "tssSep3";
            resources.ApplyResources(this.tssSep3, "tssSep3");
            // 
            // tsmiDeleteFilter
            // 
            this.tsmiDeleteFilter.Name = "tsmiDeleteFilter";
            resources.ApplyResources(this.tsmiDeleteFilter, "tsmiDeleteFilter");
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // tsmiSaveFilterList
            // 
            this.tsmiSaveFilterList.Name = "tsmiSaveFilterList";
            resources.ApplyResources(this.tsmiSaveFilterList, "tsmiSaveFilterList");
            // 
            // tssSep2
            // 
            this.tssSep2.Name = "tssSep2";
            resources.ApplyResources(this.tssSep2, "tssSep2");
            // 
            // tsmiClearAll
            // 
            this.tsmiClearAll.Name = "tsmiClearAll";
            resources.ApplyResources(this.tsmiClearAll, "tsmiClearAll");
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // tsmiLoadFilterList
            // 
            this.tsmiLoadFilterList.Name = "tsmiLoadFilterList";
            resources.ApplyResources(this.tsmiLoadFilterList, "tsmiLoadFilterList");
            // 
            // bgwLogList
            // 
            this.bgwLogList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwLogList_DoWork);
            // 
            // bgwSendFrom
            // 
            this.bgwSendFrom.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendFrom_DoWork);
            this.bgwSendFrom.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendFrom_RunWorkerCompleted);
            // 
            // pbLoading
            // 
            this.pbLoading.BackColor = System.Drawing.SystemColors.Control;
            this.pbLoading.Image = global::WPELibrary.Properties.Resources.loading;
            resources.ApplyResources(this.pbLoading, "pbLoading");
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.TabStop = false;
            // 
            // bgwSocketInfo
            // 
            this.bgwSocketInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketInfo_DoWork);
            this.bgwSocketInfo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSocketInfo_RunWorkerCompleted);
            // 
            // pPacketInfo
            // 
            this.pPacketInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPacketInfo.Controls.Add(this.tcPacketInfo);
            resources.ApplyResources(this.pPacketInfo, "pPacketInfo");
            this.pPacketInfo.Name = "pPacketInfo";
            // 
            // tcPacketInfo
            // 
            this.tcPacketInfo.Controls.Add(this.tpPacketInfo);
            this.tcPacketInfo.Controls.Add(this.tpSystemLog);
            resources.ApplyResources(this.tcPacketInfo, "tcPacketInfo");
            this.tcPacketInfo.Multiline = true;
            this.tcPacketInfo.Name = "tcPacketInfo";
            this.tcPacketInfo.SelectedIndex = 0;
            // 
            // tpPacketInfo
            // 
            this.tpPacketInfo.Controls.Add(this.tlpPacketInfo);
            resources.ApplyResources(this.tpPacketInfo, "tpPacketInfo");
            this.tpPacketInfo.Name = "tpPacketInfo";
            this.tpPacketInfo.UseVisualStyleBackColor = true;
            // 
            // tlpPacketInfo
            // 
            resources.ApplyResources(this.tlpPacketInfo, "tlpPacketInfo");
            this.tlpPacketInfo.Controls.Add(this.rtbPackInfo_Left, 0, 1);
            this.tlpPacketInfo.Controls.Add(this.rtbPacketInfo_Right, 1, 1);
            this.tlpPacketInfo.Controls.Add(this.cbPacketInfo_Left, 0, 0);
            this.tlpPacketInfo.Controls.Add(this.cbPacketInfo_Right, 1, 0);
            this.tlpPacketInfo.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpPacketInfo.Name = "tlpPacketInfo";
            // 
            // rtbPackInfo_Left
            // 
            this.rtbPackInfo_Left.BackColor = System.Drawing.SystemColors.Window;
            this.rtbPackInfo_Left.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rtbPackInfo_Left, "rtbPackInfo_Left");
            this.rtbPackInfo_Left.Name = "rtbPackInfo_Left";
            // 
            // rtbPacketInfo_Right
            // 
            this.rtbPacketInfo_Right.BackColor = System.Drawing.SystemColors.Window;
            this.rtbPacketInfo_Right.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rtbPacketInfo_Right, "rtbPacketInfo_Right");
            this.rtbPacketInfo_Right.Name = "rtbPacketInfo_Right";
            // 
            // cbPacketInfo_Left
            // 
            this.cbPacketInfo_Left.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.cbPacketInfo_Left, "cbPacketInfo_Left");
            this.cbPacketInfo_Left.FormattingEnabled = true;
            this.cbPacketInfo_Left.Name = "cbPacketInfo_Left";
            this.cbPacketInfo_Left.SelectedIndexChanged += new System.EventHandler(this.cbPacketInfo_Left_SelectedIndexChanged);
            // 
            // cbPacketInfo_Right
            // 
            this.cbPacketInfo_Right.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.cbPacketInfo_Right, "cbPacketInfo_Right");
            this.cbPacketInfo_Right.FormattingEnabled = true;
            this.cbPacketInfo_Right.Name = "cbPacketInfo_Right";
            this.cbPacketInfo_Right.SelectedIndexChanged += new System.EventHandler(this.cbPacketInfo_Right_SelectedIndexChanged);
            // 
            // tpSystemLog
            // 
            this.tpSystemLog.Controls.Add(this.dgvLogList);
            resources.ApplyResources(this.tpSystemLog, "tpSystemLog");
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.UseVisualStyleBackColor = true;
            // 
            // dgvLogList
            // 
            this.dgvLogList.AllowUserToAddRows = false;
            this.dgvLogList.AllowUserToDeleteRows = false;
            this.dgvLogList.AllowUserToResizeColumns = false;
            this.dgvLogList.AllowUserToResizeRows = false;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.dgvLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTime,
            this.cFuncName,
            this.cContent});
            this.dgvLogList.ContextMenuStrip = this.cmsLogList;
            resources.ApplyResources(this.dgvLogList, "dgvLogList");
            this.dgvLogList.MultiSelect = false;
            this.dgvLogList.Name = "dgvLogList";
            this.dgvLogList.ReadOnly = true;
            this.dgvLogList.RowHeadersVisible = false;
            dataGridViewCellStyle37.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLogList.RowsDefaultCellStyle = dataGridViewCellStyle37;
            this.dgvLogList.RowTemplate.Height = 23;
            this.dgvLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // cTime
            // 
            this.cTime.DataPropertyName = "Time";
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cTime.DefaultCellStyle = dataGridViewCellStyle35;
            resources.ApplyResources(this.cTime, "cTime");
            this.cTime.Name = "cTime";
            this.cTime.ReadOnly = true;
            // 
            // cFuncName
            // 
            this.cFuncName.DataPropertyName = "FuncName";
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cFuncName.DefaultCellStyle = dataGridViewCellStyle36;
            resources.ApplyResources(this.cFuncName, "cFuncName");
            this.cFuncName.Name = "cFuncName";
            this.cFuncName.ReadOnly = true;
            // 
            // cContent
            // 
            this.cContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cContent.DataPropertyName = "Content";
            resources.ApplyResources(this.cContent, "cContent");
            this.cContent.Name = "cContent";
            this.cContent.ReadOnly = true;
            // 
            // Socket_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pPacketInfo);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.gbFilterList);
            this.Controls.Add(this.dgvSocketList);
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.ssStatusInfo_Top);
            this.Controls.Add(this.gbRight);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Socket_Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DLL_Form_FormClosed);
            this.Load += new System.EventHandler(this.DLL_Form_Load);
            this.gbFilter_Size.ResumeLayout(false);
            this.gbFilter_Size.PerformLayout();
            this.gbFilter_Type.ResumeLayout(false);
            this.gbFilter_Type.PerformLayout();
            this.cmsLogList.ResumeLayout(false);
            this.cmsSocketList.ResumeLayout(false);
            this.ssStatusInfo_Top.ResumeLayout(false);
            this.ssStatusInfo_Top.PerformLayout();
            this.gbRight.ResumeLayout(false);
            this.gbRight.PerformLayout();
            this.gbClear.ResumeLayout(false);
            this.gbClear.PerformLayout();
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).EndInit();
            this.gbFilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).EndInit();
            this.cmsFilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.pPacketInfo.ResumeLayout(false);
            this.tcPacketInfo.ResumeLayout(false);
            this.tpPacketInfo.ResumeLayout(false);
            this.tlpPacketInfo.ResumeLayout(false);
            this.tpSystemLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
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
        private System.Windows.Forms.ToolStripStatusLabel tlSplit0;
        private System.Windows.Forms.ToolStripStatusLabel tlALL;
        private System.Windows.Forms.ToolStripStatusLabel tlALL_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit1;
        private System.Windows.Forms.ToolStripStatusLabel tlQueue;
        private System.Windows.Forms.ToolStripStatusLabel tlQueue_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit2;
        private System.Windows.Forms.ToolStripStatusLabel tlSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit3;
        private System.Windows.Forms.ToolStripStatusLabel tlRecv;
        private System.Windows.Forms.ToolStripStatusLabel tlRecv_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit4;
        private System.Windows.Forms.ToolStripStatusLabel tlCheck;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit5;
        private System.Windows.Forms.ToolStripStatusLabel tlInterecept;
        private System.Windows.Forms.ToolStripStatusLabel tlInterecept_CNT;
        private System.Windows.Forms.GroupBox gbRight;
        private System.Windows.Forms.GroupBox gbClear;
        private System.Windows.Forms.CheckBox cbReset_CNT;
        private System.Windows.Forms.GroupBox gbSearch;
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
        private System.ComponentModel.BackgroundWorker bgwLogList;
        private System.Windows.Forms.ContextMenuStrip cmsLogList;
        private System.Windows.Forms.ToolStripMenuItem tslToExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tslClearList;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.ComponentModel.BackgroundWorker bgwSendFrom;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.ComponentModel.BackgroundWorker bgwSocketInfo;
        private System.Windows.Forms.ContextMenuStrip cmsFilterList;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowFilter;
        private System.Windows.Forms.ToolStripSeparator tssSep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveFilterList;
        private System.Windows.Forms.ToolStripSeparator tssSep2;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadFilterList;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddFilter;
        private System.Windows.Forms.ToolStripSeparator tssSep3;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowHook;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsmiHideHook;
        private System.Windows.Forms.Panel pPacketInfo;
        private System.Windows.Forms.TabControl tcPacketInfo;
        private System.Windows.Forms.TabPage tpPacketInfo;
        private System.Windows.Forms.TabPage tpSystemLog;
        private System.Windows.Forms.DataGridView dgvLogList;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFuncName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cContent;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo;
        private System.Windows.Forms.RichTextBox rtbPackInfo_Left;
        private System.Windows.Forms.RichTextBox rtbPacketInfo_Right;
        private System.Windows.Forms.ComboBox cbPacketInfo_Left;
        private System.Windows.Forms.ComboBox cbPacketInfo_Right;
    }
}
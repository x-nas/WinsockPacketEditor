
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmsLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tslClearList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.bgwSocketList = new System.ComponentModel.BackgroundWorker();
            this.tSocketInfo = new System.Windows.Forms.Timer(this.components);
            this.cmsSocketList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiBatchSend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAddToFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
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
            this.gbFilterSet = new System.Windows.Forms.GroupBox();
            this.tlpClear = new System.Windows.Forms.TableLayoutPanel();
            this.bClear = new System.Windows.Forms.Button();
            this.tlpHook = new System.Windows.Forms.TableLayoutPanel();
            this.bStartHook = new System.Windows.Forms.Button();
            this.bStopHook = new System.Windows.Forms.Button();
            this.gbHookType = new System.Windows.Forms.GroupBox();
            this.tlpHookType = new System.Windows.Forms.TableLayoutPanel();
            this.cbHook_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbHook_Send = new System.Windows.Forms.CheckBox();
            this.cbHook_SendTo = new System.Windows.Forms.CheckBox();
            this.cbHook_Recv = new System.Windows.Forms.CheckBox();
            this.cbHook_WSASend = new System.Windows.Forms.CheckBox();
            this.cbHook_WSASendTo = new System.Windows.Forms.CheckBox();
            this.cbHook_WSARecv = new System.Windows.Forms.CheckBox();
            this.cbHook_WSARecvFrom = new System.Windows.Forms.CheckBox();
            this.tlpFilterSet = new System.Windows.Forms.TableLayoutPanel();
            this.cbCheck_Packet = new System.Windows.Forms.CheckBox();
            this.cbCheck_Socket = new System.Windows.Forms.CheckBox();
            this.cbCheck_IP = new System.Windows.Forms.CheckBox();
            this.txtCheck_Socket = new System.Windows.Forms.TextBox();
            this.txtCheck_IP = new System.Windows.Forms.TextBox();
            this.txtCheck_Packet = new System.Windows.Forms.TextBox();
            this.cbCheck_Size = new System.Windows.Forms.CheckBox();
            this.nudCheck_Size_From = new System.Windows.Forms.NumericUpDown();
            this.nudCheck_Size_To = new System.Windows.Forms.NumericUpDown();
            this.gbSearchData = new System.Windows.Forms.GroupBox();
            this.tlpSearchData = new System.Windows.Forms.TableLayoutPanel();
            this.bSearchData = new System.Windows.Forms.Button();
            this.txtSearchData = new System.Windows.Forms.TextBox();
            this.cbSearchType = new System.Windows.Forms.ComboBox();
            this.cbSearchOrder = new System.Windows.Forms.ComboBox();
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
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.cmsLogList.SuspendLayout();
            this.cmsSocketList.SuspendLayout();
            this.ssStatusInfo_Top.SuspendLayout();
            this.gbFilterSet.SuspendLayout();
            this.tlpClear.SuspendLayout();
            this.tlpHook.SuspendLayout();
            this.gbHookType.SuspendLayout();
            this.tlpHookType.SuspendLayout();
            this.tlpFilterSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_From)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_To)).BeginInit();
            this.gbSearchData.SuspendLayout();
            this.tlpSearchData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).BeginInit();
            this.gbFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).BeginInit();
            this.cmsFilterList.SuspendLayout();
            this.pPacketInfo.SuspendLayout();
            this.tcPacketInfo.SuspendLayout();
            this.tpPacketInfo.SuspendLayout();
            this.tlpPacketInfo.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsLogList
            // 
            this.cmsLogList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsLogList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslClearList,
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
            // cmsSocketList
            // 
            this.cmsSocketList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsSocketList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSend,
            this.toolStripSeparator1,
            this.tsmiBatchSend,
            this.toolStripSeparator2,
            this.tsmiAddToFilter,
            this.toolStripSeparator8,
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
            // tsmiAddToFilter
            // 
            this.tsmiAddToFilter.Name = "tsmiAddToFilter";
            resources.ApplyResources(this.tsmiAddToFilter, "tsmiAddToFilter");
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
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
            resources.ApplyResources(this.tlCheck_CNT, "tlCheck_CNT");
            this.tlCheck_CNT.Name = "tlCheck_CNT";
            // 
            // ssStatusInfo_Top
            // 
            resources.ApplyResources(this.ssStatusInfo_Top, "ssStatusInfo_Top");
            this.ssStatusInfo_Top.ImageScalingSize = new System.Drawing.Size(24, 24);
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
            this.tlCheck_CNT});
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
            resources.ApplyResources(this.tlQueue_CNT, "tlQueue_CNT");
            this.tlQueue_CNT.Name = "tlQueue_CNT";
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
            resources.ApplyResources(this.tlSend_CNT, "tlSend_CNT");
            this.tlSend_CNT.Name = "tlSend_CNT";
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
            resources.ApplyResources(this.tlRecv_CNT, "tlRecv_CNT");
            this.tlRecv_CNT.Name = "tlRecv_CNT";
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
            // gbFilterSet
            // 
            this.gbFilterSet.Controls.Add(this.tlpClear);
            this.gbFilterSet.Controls.Add(this.tlpHook);
            this.gbFilterSet.Controls.Add(this.gbHookType);
            this.gbFilterSet.Controls.Add(this.tlpFilterSet);
            resources.ApplyResources(this.gbFilterSet, "gbFilterSet");
            this.gbFilterSet.Name = "gbFilterSet";
            this.gbFilterSet.TabStop = false;
            // 
            // tlpClear
            // 
            resources.ApplyResources(this.tlpClear, "tlpClear");
            this.tlpClear.Controls.Add(this.bClear, 0, 0);
            this.tlpClear.Name = "tlpClear";
            // 
            // bClear
            // 
            resources.ApplyResources(this.bClear, "bClear");
            this.bClear.Name = "bClear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // tlpHook
            // 
            resources.ApplyResources(this.tlpHook, "tlpHook");
            this.tlpHook.Controls.Add(this.bStartHook, 0, 0);
            this.tlpHook.Controls.Add(this.bStopHook, 0, 2);
            this.tlpHook.Name = "tlpHook";
            // 
            // bStartHook
            // 
            resources.ApplyResources(this.bStartHook, "bStartHook");
            this.bStartHook.Name = "bStartHook";
            this.bStartHook.UseVisualStyleBackColor = true;
            this.bStartHook.Click += new System.EventHandler(this.bStartHook_Click);
            // 
            // bStopHook
            // 
            resources.ApplyResources(this.bStopHook, "bStopHook");
            this.bStopHook.Name = "bStopHook";
            this.bStopHook.UseVisualStyleBackColor = true;
            this.bStopHook.Click += new System.EventHandler(this.bStopHook_Click);
            // 
            // gbHookType
            // 
            this.gbHookType.Controls.Add(this.tlpHookType);
            resources.ApplyResources(this.gbHookType, "gbHookType");
            this.gbHookType.Name = "gbHookType";
            this.gbHookType.TabStop = false;
            // 
            // tlpHookType
            // 
            resources.ApplyResources(this.tlpHookType, "tlpHookType");
            this.tlpHookType.Controls.Add(this.cbHook_RecvFrom, 1, 3);
            this.tlpHookType.Controls.Add(this.cbHook_Send, 1, 0);
            this.tlpHookType.Controls.Add(this.cbHook_SendTo, 1, 1);
            this.tlpHookType.Controls.Add(this.cbHook_Recv, 1, 2);
            this.tlpHookType.Controls.Add(this.cbHook_WSASend, 2, 0);
            this.tlpHookType.Controls.Add(this.cbHook_WSASendTo, 2, 1);
            this.tlpHookType.Controls.Add(this.cbHook_WSARecv, 2, 2);
            this.tlpHookType.Controls.Add(this.cbHook_WSARecvFrom, 2, 3);
            this.tlpHookType.Name = "tlpHookType";
            // 
            // cbHook_RecvFrom
            // 
            resources.ApplyResources(this.cbHook_RecvFrom, "cbHook_RecvFrom");
            this.cbHook_RecvFrom.Checked = true;
            this.cbHook_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_RecvFrom.Name = "cbHook_RecvFrom";
            this.cbHook_RecvFrom.UseVisualStyleBackColor = true;
            this.cbHook_RecvFrom.CheckedChanged += new System.EventHandler(this.cbHook_RecvFrom_CheckedChanged);
            // 
            // cbHook_Send
            // 
            resources.ApplyResources(this.cbHook_Send, "cbHook_Send");
            this.cbHook_Send.Checked = true;
            this.cbHook_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_Send.Name = "cbHook_Send";
            this.cbHook_Send.UseVisualStyleBackColor = true;
            this.cbHook_Send.CheckedChanged += new System.EventHandler(this.cbHook_Send_CheckedChanged);
            // 
            // cbHook_SendTo
            // 
            resources.ApplyResources(this.cbHook_SendTo, "cbHook_SendTo");
            this.cbHook_SendTo.Checked = true;
            this.cbHook_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_SendTo.Name = "cbHook_SendTo";
            this.cbHook_SendTo.UseVisualStyleBackColor = true;
            this.cbHook_SendTo.CheckedChanged += new System.EventHandler(this.cbHook_SendTo_CheckedChanged);
            // 
            // cbHook_Recv
            // 
            resources.ApplyResources(this.cbHook_Recv, "cbHook_Recv");
            this.cbHook_Recv.Checked = true;
            this.cbHook_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_Recv.Name = "cbHook_Recv";
            this.cbHook_Recv.UseVisualStyleBackColor = true;
            this.cbHook_Recv.CheckedChanged += new System.EventHandler(this.cbHook_Recv_CheckedChanged);
            // 
            // cbHook_WSASend
            // 
            resources.ApplyResources(this.cbHook_WSASend, "cbHook_WSASend");
            this.cbHook_WSASend.Checked = true;
            this.cbHook_WSASend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSASend.Name = "cbHook_WSASend";
            this.cbHook_WSASend.UseVisualStyleBackColor = true;
            this.cbHook_WSASend.CheckedChanged += new System.EventHandler(this.cbHook_WSASend_CheckedChanged);
            // 
            // cbHook_WSASendTo
            // 
            resources.ApplyResources(this.cbHook_WSASendTo, "cbHook_WSASendTo");
            this.cbHook_WSASendTo.Checked = true;
            this.cbHook_WSASendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSASendTo.Name = "cbHook_WSASendTo";
            this.cbHook_WSASendTo.UseVisualStyleBackColor = true;
            this.cbHook_WSASendTo.CheckedChanged += new System.EventHandler(this.cbHook_WSASendTo_CheckedChanged);
            // 
            // cbHook_WSARecv
            // 
            resources.ApplyResources(this.cbHook_WSARecv, "cbHook_WSARecv");
            this.cbHook_WSARecv.Checked = true;
            this.cbHook_WSARecv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSARecv.Name = "cbHook_WSARecv";
            this.cbHook_WSARecv.UseVisualStyleBackColor = true;
            this.cbHook_WSARecv.CheckedChanged += new System.EventHandler(this.cbHook_WSARecv_CheckedChanged);
            // 
            // cbHook_WSARecvFrom
            // 
            resources.ApplyResources(this.cbHook_WSARecvFrom, "cbHook_WSARecvFrom");
            this.cbHook_WSARecvFrom.Checked = true;
            this.cbHook_WSARecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSARecvFrom.Name = "cbHook_WSARecvFrom";
            this.cbHook_WSARecvFrom.UseVisualStyleBackColor = true;
            this.cbHook_WSARecvFrom.CheckedChanged += new System.EventHandler(this.cbHook_WSARecvFrom_CheckedChanged);
            // 
            // tlpFilterSet
            // 
            resources.ApplyResources(this.tlpFilterSet, "tlpFilterSet");
            this.tlpFilterSet.Controls.Add(this.cbCheck_Packet, 1, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheck_Socket, 1, 0);
            this.tlpFilterSet.Controls.Add(this.cbCheck_IP, 1, 1);
            this.tlpFilterSet.Controls.Add(this.txtCheck_Socket, 2, 0);
            this.tlpFilterSet.Controls.Add(this.txtCheck_IP, 2, 1);
            this.tlpFilterSet.Controls.Add(this.txtCheck_Packet, 2, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheck_Size, 4, 0);
            this.tlpFilterSet.Controls.Add(this.nudCheck_Size_From, 4, 1);
            this.tlpFilterSet.Controls.Add(this.nudCheck_Size_To, 4, 2);
            this.tlpFilterSet.Name = "tlpFilterSet";
            // 
            // cbCheck_Packet
            // 
            resources.ApplyResources(this.cbCheck_Packet, "cbCheck_Packet");
            this.cbCheck_Packet.Name = "cbCheck_Packet";
            this.cbCheck_Packet.UseVisualStyleBackColor = true;
            this.cbCheck_Packet.CheckedChanged += new System.EventHandler(this.cbCheck_Packet_CheckedChanged);
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
            // txtCheck_Socket
            // 
            resources.ApplyResources(this.txtCheck_Socket, "txtCheck_Socket");
            this.txtCheck_Socket.Name = "txtCheck_Socket";
            // 
            // txtCheck_IP
            // 
            resources.ApplyResources(this.txtCheck_IP, "txtCheck_IP");
            this.txtCheck_IP.Name = "txtCheck_IP";
            // 
            // txtCheck_Packet
            // 
            resources.ApplyResources(this.txtCheck_Packet, "txtCheck_Packet");
            this.txtCheck_Packet.Name = "txtCheck_Packet";
            // 
            // cbCheck_Size
            // 
            resources.ApplyResources(this.cbCheck_Size, "cbCheck_Size");
            this.cbCheck_Size.Name = "cbCheck_Size";
            this.cbCheck_Size.UseVisualStyleBackColor = true;
            this.cbCheck_Size.CheckedChanged += new System.EventHandler(this.cbCheck_Size_CheckedChanged);
            // 
            // nudCheck_Size_From
            // 
            resources.ApplyResources(this.nudCheck_Size_From, "nudCheck_Size_From");
            this.nudCheck_Size_From.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudCheck_Size_From.Name = "nudCheck_Size_From";
            // 
            // nudCheck_Size_To
            // 
            resources.ApplyResources(this.nudCheck_Size_To, "nudCheck_Size_To");
            this.nudCheck_Size_To.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudCheck_Size_To.Name = "nudCheck_Size_To";
            this.nudCheck_Size_To.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // gbSearchData
            // 
            this.gbSearchData.Controls.Add(this.tlpSearchData);
            resources.ApplyResources(this.gbSearchData, "gbSearchData");
            this.gbSearchData.Name = "gbSearchData";
            this.gbSearchData.TabStop = false;
            // 
            // tlpSearchData
            // 
            resources.ApplyResources(this.tlpSearchData, "tlpSearchData");
            this.tlpSearchData.Controls.Add(this.bSearchData, 6, 0);
            this.tlpSearchData.Controls.Add(this.txtSearchData, 2, 0);
            this.tlpSearchData.Controls.Add(this.cbSearchType, 0, 0);
            this.tlpSearchData.Controls.Add(this.cbSearchOrder, 4, 0);
            this.tlpSearchData.Name = "tlpSearchData";
            // 
            // bSearchData
            // 
            resources.ApplyResources(this.bSearchData, "bSearchData");
            this.bSearchData.Name = "bSearchData";
            this.bSearchData.UseVisualStyleBackColor = true;
            this.bSearchData.Click += new System.EventHandler(this.bSearchData_Click);
            // 
            // txtSearchData
            // 
            resources.ApplyResources(this.txtSearchData, "txtSearchData");
            this.txtSearchData.Name = "txtSearchData";
            // 
            // cbSearchType
            // 
            this.cbSearchType.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.cbSearchType, "cbSearchType");
            this.cbSearchType.FormattingEnabled = true;
            this.cbSearchType.Name = "cbSearchType";
            // 
            // cbSearchOrder
            // 
            this.cbSearchOrder.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.cbSearchOrder, "cbSearchOrder");
            this.cbSearchOrder.FormattingEnabled = true;
            this.cbSearchOrder.Items.AddRange(new object[] {
            resources.GetString("cbSearchOrder.Items"),
            resources.GetString("cbSearchOrder.Items1")});
            this.cbSearchOrder.Name = "cbSearchOrder";
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
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowsDefaultCellStyle = dataGridViewCellStyle7;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.cIndex, "cIndex");
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cType
            // 
            this.cType.DataPropertyName = "Type_Name";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cType.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cType, "cType");
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFrom
            // 
            this.cFrom.DataPropertyName = "From";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cFrom.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cFrom, "cFrom");
            this.cFrom.Name = "cFrom";
            this.cFrom.ReadOnly = true;
            this.cFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "To";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "ResLen";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle6;
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
            this.dgvFilterList.RowTemplate.Height = 25;
            this.dgvFilterList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilterList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterList_CellContentClick);
            // 
            // cIsCheck
            // 
            this.cIsCheck.DataPropertyName = "IsCheck";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = false;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIsCheck.DefaultCellStyle = dataGridViewCellStyle8;
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
            this.cmsFilterList.ImageScalingSize = new System.Drawing.Size(24, 24);
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
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
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLogList.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvLogList.RowTemplate.Height = 23;
            this.dgvLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // cTime
            // 
            this.cTime.DataPropertyName = "Time";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cTime.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.cTime, "cTime");
            this.cTime.Name = "cTime";
            this.cTime.ReadOnly = true;
            // 
            // cFuncName
            // 
            this.cFuncName.DataPropertyName = "FuncName";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cFuncName.DefaultCellStyle = dataGridViewCellStyle11;
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
            // pbLoading
            // 
            this.pbLoading.BackColor = System.Drawing.SystemColors.Control;
            this.pbLoading.Image = global::WPELibrary.Properties.Resources.loading;
            resources.ApplyResources(this.pbLoading, "pbLoading");
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.TabStop = false;
            // 
            // Socket_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pPacketInfo);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.gbFilterList);
            this.Controls.Add(this.dgvSocketList);
            this.Controls.Add(this.gbSearchData);
            this.Controls.Add(this.ssStatusInfo_Top);
            this.Controls.Add(this.gbFilterSet);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Socket_Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DLL_Form_FormClosed);
            this.Load += new System.EventHandler(this.DLL_Form_Load);
            this.cmsLogList.ResumeLayout(false);
            this.cmsSocketList.ResumeLayout(false);
            this.ssStatusInfo_Top.ResumeLayout(false);
            this.ssStatusInfo_Top.PerformLayout();
            this.gbFilterSet.ResumeLayout(false);
            this.tlpClear.ResumeLayout(false);
            this.tlpHook.ResumeLayout(false);
            this.gbHookType.ResumeLayout(false);
            this.tlpHookType.ResumeLayout(false);
            this.tlpHookType.PerformLayout();
            this.tlpFilterSet.ResumeLayout(false);
            this.tlpFilterSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_From)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_To)).EndInit();
            this.gbSearchData.ResumeLayout(false);
            this.tlpSearchData.ResumeLayout(false);
            this.tlpSearchData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).EndInit();
            this.gbFilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).EndInit();
            this.cmsFilterList.ResumeLayout(false);
            this.pPacketInfo.ResumeLayout(false);
            this.tcPacketInfo.ResumeLayout(false);
            this.tpPacketInfo.ResumeLayout(false);
            this.tlpPacketInfo.ResumeLayout(false);
            this.tpSystemLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker bgwSocketList;
        private System.Windows.Forms.Timer tSocketInfo;
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
        private System.Windows.Forms.GroupBox gbFilterSet;
        private System.Windows.Forms.GroupBox gbSearchData;
        private System.Windows.Forms.DataGridView dgvSocketList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowBatchSend;
        private System.Windows.Forms.ToolStripMenuItem tsmiUseSocket;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveSocketInfo;
        private System.Windows.Forms.GroupBox gbFilterList;
        private System.Windows.Forms.DataGridView dgvFilterList;
        private System.ComponentModel.BackgroundWorker bgwLogList;
        private System.Windows.Forms.ContextMenuStrip cmsLogList;
        private System.Windows.Forms.ToolStripMenuItem tslToExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tslClearList;
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
        private System.Windows.Forms.TableLayoutPanel tlpSearchData;
        private System.Windows.Forms.Button bSearchData;
        private System.Windows.Forms.TextBox txtSearchData;
        private System.Windows.Forms.TableLayoutPanel tlpFilterSet;
        private System.Windows.Forms.CheckBox cbCheck_Socket;
        private System.Windows.Forms.TextBox txtCheck_Socket;
        private System.Windows.Forms.CheckBox cbCheck_IP;
        private System.Windows.Forms.TextBox txtCheck_IP;
        private System.Windows.Forms.CheckBox cbCheck_Packet;
        private System.Windows.Forms.TextBox txtCheck_Packet;
        private System.Windows.Forms.CheckBox cbCheck_Size;
        private System.Windows.Forms.GroupBox gbHookType;
        private System.Windows.Forms.TableLayoutPanel tlpHookType;
        private System.Windows.Forms.CheckBox cbHook_Send;
        private System.Windows.Forms.CheckBox cbHook_SendTo;
        private System.Windows.Forms.CheckBox cbHook_Recv;
        private System.Windows.Forms.CheckBox cbHook_RecvFrom;
        private System.Windows.Forms.CheckBox cbHook_WSASend;
        private System.Windows.Forms.CheckBox cbHook_WSASendTo;
        private System.Windows.Forms.CheckBox cbHook_WSARecv;
        private System.Windows.Forms.CheckBox cbHook_WSARecvFrom;
        private System.Windows.Forms.TableLayoutPanel tlpHook;
        private System.Windows.Forms.Button bStartHook;
        private System.Windows.Forms.Button bStopHook;
        private System.Windows.Forms.TableLayoutPanel tlpClear;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.NumericUpDown nudCheck_Size_From;
        private System.Windows.Forms.NumericUpDown nudCheck_Size_To;
        private System.Windows.Forms.ComboBox cbSearchType;
        private System.Windows.Forms.ComboBox cbSearchOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddToFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFName;
    }
}
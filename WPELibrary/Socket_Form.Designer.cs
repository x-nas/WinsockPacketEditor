﻿
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpSocketForm = new System.Windows.Forms.TableLayoutPanel();
            this.ssSocketList = new System.Windows.Forms.StatusStrip();
            this.tlALL = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlALL_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlFilter_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendTo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendTo_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecvFrom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecvFrom_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASend_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecv = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecv_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel14 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASendTo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASendTo_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel17 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecvFrom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecvFrom_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvSocketList = new System.Windows.Forms.DataGridView();
            this.cTypeImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.cIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbFilterSet = new System.Windows.Forms.GroupBox();
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
            this.gbHookButton_Search = new System.Windows.Forms.GroupBox();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.pSearchSocketList = new System.Windows.Forms.Panel();
            this.rbFromIndex = new System.Windows.Forms.RadioButton();
            this.rbFromHead = new System.Windows.Forms.RadioButton();
            this.tlpSearchButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSearchNext = new System.Windows.Forms.Button();
            this.bSearch = new System.Windows.Forms.Button();
            this.tlpHookButton = new System.Windows.Forms.TableLayoutPanel();
            this.tlpHookButton_Start = new System.Windows.Forms.TableLayoutPanel();
            this.bStopHook = new System.Windows.Forms.Button();
            this.bStartHook = new System.Windows.Forms.Button();
            this.bCleanUp = new System.Windows.Forms.Button();
            this.tlpInformation = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tcPacketInfo = new System.Windows.Forms.TabControl();
            this.tpPacketData = new System.Windows.Forms.TabPage();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.tlpHexBox = new System.Windows.Forms.TableLayoutPanel();
            this.tlpConversion = new System.Windows.Forms.TableLayoutPanel();
            this.lBits_Value = new System.Windows.Forms.Label();
            this.lBits = new System.Windows.Forms.Label();
            this.lDouble_Value = new System.Windows.Forms.Label();
            this.lDouble = new System.Windows.Forms.Label();
            this.lFloat_Value = new System.Windows.Forms.Label();
            this.lFloat = new System.Windows.Forms.Label();
            this.lUInt64_Value = new System.Windows.Forms.Label();
            this.lUInt64 = new System.Windows.Forms.Label();
            this.lInt64_Value = new System.Windows.Forms.Label();
            this.lInt64 = new System.Windows.Forms.Label();
            this.lUInt32_Value = new System.Windows.Forms.Label();
            this.lUInt32 = new System.Windows.Forms.Label();
            this.lInt32_Value = new System.Windows.Forms.Label();
            this.lInt32 = new System.Windows.Forms.Label();
            this.lUShort_Value = new System.Windows.Forms.Label();
            this.lUShort = new System.Windows.Forms.Label();
            this.lShort_Value = new System.Windows.Forms.Label();
            this.lShort = new System.Windows.Forms.Label();
            this.lByte_Value = new System.Windows.Forms.Label();
            this.lByte = new System.Windows.Forms.Label();
            this.tsPacketData = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCopy = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyHex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPaste = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPasteHex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFindNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbEncoding = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbPerLine = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tslPacketLen = new System.Windows.Forms.ToolStripLabel();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.lPacketDataPosition = new System.Windows.Forms.Label();
            this.tpSystemLog = new System.Windows.Forms.TabPage();
            this.dgvLogList = new System.Windows.Forms.DataGridView();
            this.cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFuncName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tslClearList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpFilterList = new System.Windows.Forms.TableLayoutPanel();
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
            this.ssProcessInfo = new System.Windows.Forms.StatusStrip();
            this.tsslProcessName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslProcessInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslWinSock = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslTotalBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this.bgwSocketList = new System.ComponentModel.BackgroundWorker();
            this.tSocketInfo = new System.Windows.Forms.Timer(this.components);
            this.bgwLogList = new System.ComponentModel.BackgroundWorker();
            this.bgwSendFrom = new System.ComponentModel.BackgroundWorker();
            this.bgwSearchPacketData = new System.ComponentModel.BackgroundWorker();
            this.niWPE = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiStartHook = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStopHook = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCleanUp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowSendList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tlpSocketForm.SuspendLayout();
            this.ssSocketList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).BeginInit();
            this.cmsSocketList.SuspendLayout();
            this.tlpParameter.SuspendLayout();
            this.gbFilterSet.SuspendLayout();
            this.tlpFilterSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_From)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_To)).BeginInit();
            this.gbHookType.SuspendLayout();
            this.tlpHookType.SuspendLayout();
            this.gbHookButton_Search.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.pSearchSocketList.SuspendLayout();
            this.tlpSearchButton.SuspendLayout();
            this.tlpHookButton.SuspendLayout();
            this.tlpHookButton_Start.SuspendLayout();
            this.tlpInformation.SuspendLayout();
            this.tlpPacketInfo.SuspendLayout();
            this.tcPacketInfo.SuspendLayout();
            this.tpPacketData.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.tlpHexBox.SuspendLayout();
            this.tlpConversion.SuspendLayout();
            this.tsPacketData.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            this.cmsLogList.SuspendLayout();
            this.tlpFilterList.SuspendLayout();
            this.gbFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).BeginInit();
            this.cmsFilterList.SuspendLayout();
            this.ssProcessInfo.SuspendLayout();
            this.cmsIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSocketForm
            // 
            resources.ApplyResources(this.tlpSocketForm, "tlpSocketForm");
            this.tlpSocketForm.Controls.Add(this.ssSocketList, 0, 1);
            this.tlpSocketForm.Controls.Add(this.dgvSocketList, 0, 2);
            this.tlpSocketForm.Controls.Add(this.tlpParameter, 0, 0);
            this.tlpSocketForm.Controls.Add(this.tlpInformation, 0, 3);
            this.tlpSocketForm.Controls.Add(this.ssProcessInfo, 0, 4);
            this.tlpSocketForm.Name = "tlpSocketForm";
            // 
            // ssSocketList
            // 
            resources.ApplyResources(this.ssSocketList, "ssSocketList");
            this.ssSocketList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ssSocketList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlALL,
            this.tlALL_CNT,
            this.tlSplit1,
            this.tlQueue,
            this.tlQueue_CNT,
            this.tlSplit2,
            this.tlFilter,
            this.tlFilter_CNT,
            this.tlSplit4,
            this.tlSend,
            this.tlSend_CNT,
            this.tlSplit3,
            this.tlRecv,
            this.tlRecv_CNT,
            this.toolStripStatusLabel2,
            this.tlSendTo,
            this.tlSendTo_CNT,
            this.toolStripStatusLabel5,
            this.tlRecvFrom,
            this.tlRecvFrom_CNT,
            this.toolStripStatusLabel8,
            this.tlWSASend,
            this.tlWSASend_CNT,
            this.toolStripStatusLabel11,
            this.tlWSARecv,
            this.tlWSARecv_CNT,
            this.toolStripStatusLabel14,
            this.tlWSASendTo,
            this.tlWSASendTo_CNT,
            this.toolStripStatusLabel17,
            this.tlWSARecvFrom,
            this.tlWSARecvFrom_CNT});
            this.ssSocketList.Name = "ssSocketList";
            this.ssSocketList.SizingGrip = false;
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
            // tlFilter
            // 
            this.tlFilter.Name = "tlFilter";
            resources.ApplyResources(this.tlFilter, "tlFilter");
            // 
            // tlFilter_CNT
            // 
            resources.ApplyResources(this.tlFilter_CNT, "tlFilter_CNT");
            this.tlFilter_CNT.Name = "tlFilter_CNT";
            // 
            // tlSplit4
            // 
            this.tlSplit4.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit4.Name = "tlSplit4";
            resources.ApplyResources(this.tlSplit4, "tlSplit4");
            // 
            // tlSend
            // 
            resources.ApplyResources(this.tlSend, "tlSend");
            this.tlSend.Name = "tlSend";
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
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            // 
            // tlSendTo
            // 
            resources.ApplyResources(this.tlSendTo, "tlSendTo");
            this.tlSendTo.Name = "tlSendTo";
            // 
            // tlSendTo_CNT
            // 
            resources.ApplyResources(this.tlSendTo_CNT, "tlSendTo_CNT");
            this.tlSendTo_CNT.Name = "tlSendTo_CNT";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            resources.ApplyResources(this.toolStripStatusLabel5, "toolStripStatusLabel5");
            // 
            // tlRecvFrom
            // 
            resources.ApplyResources(this.tlRecvFrom, "tlRecvFrom");
            this.tlRecvFrom.Name = "tlRecvFrom";
            // 
            // tlRecvFrom_CNT
            // 
            resources.ApplyResources(this.tlRecvFrom_CNT, "tlRecvFrom_CNT");
            this.tlRecvFrom_CNT.Name = "tlRecvFrom_CNT";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            resources.ApplyResources(this.toolStripStatusLabel8, "toolStripStatusLabel8");
            // 
            // tlWSASend
            // 
            resources.ApplyResources(this.tlWSASend, "tlWSASend");
            this.tlWSASend.Name = "tlWSASend";
            // 
            // tlWSASend_CNT
            // 
            resources.ApplyResources(this.tlWSASend_CNT, "tlWSASend_CNT");
            this.tlWSASend_CNT.Name = "tlWSASend_CNT";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            resources.ApplyResources(this.toolStripStatusLabel11, "toolStripStatusLabel11");
            // 
            // tlWSARecv
            // 
            resources.ApplyResources(this.tlWSARecv, "tlWSARecv");
            this.tlWSARecv.Name = "tlWSARecv";
            // 
            // tlWSARecv_CNT
            // 
            resources.ApplyResources(this.tlWSARecv_CNT, "tlWSARecv_CNT");
            this.tlWSARecv_CNT.Name = "tlWSARecv_CNT";
            // 
            // toolStripStatusLabel14
            // 
            this.toolStripStatusLabel14.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel14.Name = "toolStripStatusLabel14";
            resources.ApplyResources(this.toolStripStatusLabel14, "toolStripStatusLabel14");
            // 
            // tlWSASendTo
            // 
            resources.ApplyResources(this.tlWSASendTo, "tlWSASendTo");
            this.tlWSASendTo.Name = "tlWSASendTo";
            // 
            // tlWSASendTo_CNT
            // 
            resources.ApplyResources(this.tlWSASendTo_CNT, "tlWSASendTo_CNT");
            this.tlWSASendTo_CNT.Name = "tlWSASendTo_CNT";
            // 
            // toolStripStatusLabel17
            // 
            this.toolStripStatusLabel17.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel17.Name = "toolStripStatusLabel17";
            resources.ApplyResources(this.toolStripStatusLabel17, "toolStripStatusLabel17");
            // 
            // tlWSARecvFrom
            // 
            resources.ApplyResources(this.tlWSARecvFrom, "tlWSARecvFrom");
            this.tlWSARecvFrom.Name = "tlWSARecvFrom";
            // 
            // tlWSARecvFrom_CNT
            // 
            resources.ApplyResources(this.tlWSARecvFrom_CNT, "tlWSARecvFrom_CNT");
            this.tlWSARecvFrom_CNT.Name = "tlWSARecvFrom_CNT";
            // 
            // dgvSocketList
            // 
            this.dgvSocketList.AllowUserToAddRows = false;
            this.dgvSocketList.AllowUserToDeleteRows = false;
            this.dgvSocketList.AllowUserToResizeColumns = false;
            this.dgvSocketList.AllowUserToResizeRows = false;
            this.dgvSocketList.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvSocketList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSocketList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSocketList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTypeImg,
            this.cIndex,
            this.cType,
            this.cSocket,
            this.cFrom,
            this.cTo,
            this.cLen,
            this.cData});
            this.dgvSocketList.ContextMenuStrip = this.cmsSocketList;
            resources.ApplyResources(this.dgvSocketList, "dgvSocketList");
            this.dgvSocketList.MultiSelect = false;
            this.dgvSocketList.Name = "dgvSocketList";
            this.dgvSocketList.RowHeadersVisible = false;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.WindowText;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgvSocketList.RowTemplate.Height = 23;
            this.dgvSocketList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSocketList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSocketList_CellFormatting);
            this.dgvSocketList.SelectionChanged += new System.EventHandler(this.dgvSocketInfo_SelectionChanged);
            // 
            // cTypeImg
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cTypeImg.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cTypeImg, "cTypeImg");
            this.cTypeImg.Image = global::WPELibrary.Properties.Resources.Info16;
            this.cTypeImg.Name = "cTypeImg";
            this.cTypeImg.ReadOnly = true;
            this.cTypeImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cIndex
            // 
            this.cIndex.DataPropertyName = "PacketIndex";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cIndex.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cIndex, "cIndex");
            this.cIndex.Name = "cIndex";
            this.cIndex.ReadOnly = true;
            this.cIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cType
            // 
            this.cType.DataPropertyName = "PacketType";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cType.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cType, "cType");
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "PacketSocket";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFrom
            // 
            this.cFrom.DataPropertyName = "PacketFrom";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cFrom.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.cFrom, "cFrom");
            this.cFrom.Name = "cFrom";
            this.cFrom.ReadOnly = true;
            this.cFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cTo
            // 
            this.cTo.DataPropertyName = "PacketTo";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cTo.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.cTo, "cTo");
            this.cTo.Name = "cTo";
            this.cTo.ReadOnly = true;
            this.cTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "PacketLen";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.cLen, "cLen");
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cData
            // 
            this.cData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cData.DataPropertyName = "PacketData";
            resources.ApplyResources(this.cData, "cData");
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.cmsSocketList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketList_ItemClicked);
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
            // tlpParameter
            // 
            resources.ApplyResources(this.tlpParameter, "tlpParameter");
            this.tlpParameter.Controls.Add(this.gbFilterSet, 0, 0);
            this.tlpParameter.Controls.Add(this.gbHookType, 1, 0);
            this.tlpParameter.Controls.Add(this.gbHookButton_Search, 2, 0);
            this.tlpParameter.Controls.Add(this.tlpHookButton, 3, 0);
            this.tlpParameter.Name = "tlpParameter";
            // 
            // gbFilterSet
            // 
            this.gbFilterSet.Controls.Add(this.tlpFilterSet);
            resources.ApplyResources(this.gbFilterSet, "gbFilterSet");
            this.gbFilterSet.Name = "gbFilterSet";
            this.gbFilterSet.TabStop = false;
            // 
            // tlpFilterSet
            // 
            resources.ApplyResources(this.tlpFilterSet, "tlpFilterSet");
            this.tlpFilterSet.Controls.Add(this.cbCheck_Packet, 0, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheck_Socket, 0, 0);
            this.tlpFilterSet.Controls.Add(this.cbCheck_IP, 0, 1);
            this.tlpFilterSet.Controls.Add(this.txtCheck_Socket, 1, 0);
            this.tlpFilterSet.Controls.Add(this.txtCheck_IP, 1, 1);
            this.tlpFilterSet.Controls.Add(this.txtCheck_Packet, 1, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheck_Size, 3, 0);
            this.tlpFilterSet.Controls.Add(this.nudCheck_Size_From, 3, 1);
            this.tlpFilterSet.Controls.Add(this.nudCheck_Size_To, 3, 2);
            this.tlpFilterSet.Name = "tlpFilterSet";
            // 
            // cbCheck_Packet
            // 
            resources.ApplyResources(this.cbCheck_Packet, "cbCheck_Packet");
            this.cbCheck_Packet.Name = "cbCheck_Packet";
            this.cbCheck_Packet.UseVisualStyleBackColor = true;
            // 
            // cbCheck_Socket
            // 
            resources.ApplyResources(this.cbCheck_Socket, "cbCheck_Socket");
            this.cbCheck_Socket.Name = "cbCheck_Socket";
            this.cbCheck_Socket.UseVisualStyleBackColor = true;
            // 
            // cbCheck_IP
            // 
            resources.ApplyResources(this.cbCheck_IP, "cbCheck_IP");
            this.cbCheck_IP.Name = "cbCheck_IP";
            this.cbCheck_IP.UseVisualStyleBackColor = true;
            // 
            // txtCheck_Socket
            // 
            this.txtCheck_Socket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtCheck_Socket, "txtCheck_Socket");
            this.txtCheck_Socket.Name = "txtCheck_Socket";
            // 
            // txtCheck_IP
            // 
            this.txtCheck_IP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtCheck_IP, "txtCheck_IP");
            this.txtCheck_IP.Name = "txtCheck_IP";
            // 
            // txtCheck_Packet
            // 
            this.txtCheck_Packet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtCheck_Packet, "txtCheck_Packet");
            this.txtCheck_Packet.Name = "txtCheck_Packet";
            // 
            // cbCheck_Size
            // 
            resources.ApplyResources(this.cbCheck_Size, "cbCheck_Size");
            this.cbCheck_Size.Name = "cbCheck_Size";
            this.cbCheck_Size.UseVisualStyleBackColor = true;
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
            this.tlpHookType.Controls.Add(this.cbHook_RecvFrom, 0, 3);
            this.tlpHookType.Controls.Add(this.cbHook_Send, 0, 0);
            this.tlpHookType.Controls.Add(this.cbHook_SendTo, 0, 1);
            this.tlpHookType.Controls.Add(this.cbHook_Recv, 0, 2);
            this.tlpHookType.Controls.Add(this.cbHook_WSASend, 1, 0);
            this.tlpHookType.Controls.Add(this.cbHook_WSASendTo, 1, 1);
            this.tlpHookType.Controls.Add(this.cbHook_WSARecv, 1, 2);
            this.tlpHookType.Controls.Add(this.cbHook_WSARecvFrom, 1, 3);
            this.tlpHookType.Name = "tlpHookType";
            // 
            // cbHook_RecvFrom
            // 
            resources.ApplyResources(this.cbHook_RecvFrom, "cbHook_RecvFrom");
            this.cbHook_RecvFrom.Checked = true;
            this.cbHook_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_RecvFrom.Name = "cbHook_RecvFrom";
            this.cbHook_RecvFrom.UseVisualStyleBackColor = true;
            // 
            // cbHook_Send
            // 
            resources.ApplyResources(this.cbHook_Send, "cbHook_Send");
            this.cbHook_Send.Checked = true;
            this.cbHook_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_Send.Name = "cbHook_Send";
            this.cbHook_Send.UseVisualStyleBackColor = true;
            // 
            // cbHook_SendTo
            // 
            resources.ApplyResources(this.cbHook_SendTo, "cbHook_SendTo");
            this.cbHook_SendTo.Checked = true;
            this.cbHook_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_SendTo.Name = "cbHook_SendTo";
            this.cbHook_SendTo.UseVisualStyleBackColor = true;
            // 
            // cbHook_Recv
            // 
            resources.ApplyResources(this.cbHook_Recv, "cbHook_Recv");
            this.cbHook_Recv.Checked = true;
            this.cbHook_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_Recv.Name = "cbHook_Recv";
            this.cbHook_Recv.UseVisualStyleBackColor = true;
            // 
            // cbHook_WSASend
            // 
            resources.ApplyResources(this.cbHook_WSASend, "cbHook_WSASend");
            this.cbHook_WSASend.Checked = true;
            this.cbHook_WSASend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSASend.Name = "cbHook_WSASend";
            this.cbHook_WSASend.UseVisualStyleBackColor = true;
            // 
            // cbHook_WSASendTo
            // 
            resources.ApplyResources(this.cbHook_WSASendTo, "cbHook_WSASendTo");
            this.cbHook_WSASendTo.Checked = true;
            this.cbHook_WSASendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSASendTo.Name = "cbHook_WSASendTo";
            this.cbHook_WSASendTo.UseVisualStyleBackColor = true;
            // 
            // cbHook_WSARecv
            // 
            resources.ApplyResources(this.cbHook_WSARecv, "cbHook_WSARecv");
            this.cbHook_WSARecv.Checked = true;
            this.cbHook_WSARecv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSARecv.Name = "cbHook_WSARecv";
            this.cbHook_WSARecv.UseVisualStyleBackColor = true;
            // 
            // cbHook_WSARecvFrom
            // 
            resources.ApplyResources(this.cbHook_WSARecvFrom, "cbHook_WSARecvFrom");
            this.cbHook_WSARecvFrom.Checked = true;
            this.cbHook_WSARecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHook_WSARecvFrom.Name = "cbHook_WSARecvFrom";
            this.cbHook_WSARecvFrom.UseVisualStyleBackColor = true;
            // 
            // gbHookButton_Search
            // 
            this.gbHookButton_Search.Controls.Add(this.tlpSearch);
            resources.ApplyResources(this.gbHookButton_Search, "gbHookButton_Search");
            this.gbHookButton_Search.Name = "gbHookButton_Search";
            this.gbHookButton_Search.TabStop = false;
            // 
            // tlpSearch
            // 
            resources.ApplyResources(this.tlpSearch, "tlpSearch");
            this.tlpSearch.Controls.Add(this.pSearchSocketList, 0, 0);
            this.tlpSearch.Controls.Add(this.tlpSearchButton, 0, 2);
            this.tlpSearch.Name = "tlpSearch";
            // 
            // pSearchSocketList
            // 
            this.pSearchSocketList.Controls.Add(this.rbFromIndex);
            this.pSearchSocketList.Controls.Add(this.rbFromHead);
            resources.ApplyResources(this.pSearchSocketList, "pSearchSocketList");
            this.pSearchSocketList.Name = "pSearchSocketList";
            // 
            // rbFromIndex
            // 
            resources.ApplyResources(this.rbFromIndex, "rbFromIndex");
            this.rbFromIndex.Name = "rbFromIndex";
            this.rbFromIndex.UseVisualStyleBackColor = true;
            // 
            // rbFromHead
            // 
            resources.ApplyResources(this.rbFromHead, "rbFromHead");
            this.rbFromHead.Checked = true;
            this.rbFromHead.Name = "rbFromHead";
            this.rbFromHead.TabStop = true;
            this.rbFromHead.UseVisualStyleBackColor = true;
            // 
            // tlpSearchButton
            // 
            resources.ApplyResources(this.tlpSearchButton, "tlpSearchButton");
            this.tlpSearchButton.Controls.Add(this.bSearchNext, 2, 0);
            this.tlpSearchButton.Controls.Add(this.bSearch, 0, 0);
            this.tlpSearchButton.Name = "tlpSearchButton";
            // 
            // bSearchNext
            // 
            resources.ApplyResources(this.bSearchNext, "bSearchNext");
            this.bSearchNext.FlatAppearance.BorderSize = 0;
            this.bSearchNext.Image = global::WPELibrary.Properties.Resources.Search16;
            this.bSearchNext.Name = "bSearchNext";
            this.bSearchNext.UseVisualStyleBackColor = true;
            this.bSearchNext.Click += new System.EventHandler(this.bSearchNext_Click);
            // 
            // bSearch
            // 
            resources.ApplyResources(this.bSearch, "bSearch");
            this.bSearch.FlatAppearance.BorderSize = 0;
            this.bSearch.Image = global::WPELibrary.Properties.Resources.File_info16;
            this.bSearch.Name = "bSearch";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // tlpHookButton
            // 
            resources.ApplyResources(this.tlpHookButton, "tlpHookButton");
            this.tlpHookButton.Controls.Add(this.tlpHookButton_Start, 3, 1);
            this.tlpHookButton.Controls.Add(this.bCleanUp, 1, 1);
            this.tlpHookButton.Name = "tlpHookButton";
            // 
            // tlpHookButton_Start
            // 
            resources.ApplyResources(this.tlpHookButton_Start, "tlpHookButton_Start");
            this.tlpHookButton_Start.Controls.Add(this.bStopHook, 0, 2);
            this.tlpHookButton_Start.Controls.Add(this.bStartHook, 0, 0);
            this.tlpHookButton_Start.Name = "tlpHookButton_Start";
            // 
            // bStopHook
            // 
            resources.ApplyResources(this.bStopHook, "bStopHook");
            this.bStopHook.Image = global::WPELibrary.Properties.Resources.Stop16;
            this.bStopHook.Name = "bStopHook";
            this.bStopHook.UseVisualStyleBackColor = true;
            this.bStopHook.Click += new System.EventHandler(this.bStopHook_Click);
            // 
            // bStartHook
            // 
            resources.ApplyResources(this.bStartHook, "bStartHook");
            this.bStartHook.Image = global::WPELibrary.Properties.Resources.Play16;
            this.bStartHook.Name = "bStartHook";
            this.bStartHook.UseVisualStyleBackColor = true;
            this.bStartHook.Click += new System.EventHandler(this.bStartHook_Click);
            // 
            // bCleanUp
            // 
            resources.ApplyResources(this.bCleanUp, "bCleanUp");
            this.bCleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.bCleanUp.Name = "bCleanUp";
            this.bCleanUp.UseVisualStyleBackColor = true;
            this.bCleanUp.Click += new System.EventHandler(this.bCleanUp_Click);
            // 
            // tlpInformation
            // 
            resources.ApplyResources(this.tlpInformation, "tlpInformation");
            this.tlpInformation.Controls.Add(this.tlpPacketInfo, 1, 0);
            this.tlpInformation.Controls.Add(this.tlpFilterList, 0, 0);
            this.tlpInformation.Name = "tlpInformation";
            // 
            // tlpPacketInfo
            // 
            resources.ApplyResources(this.tlpPacketInfo, "tlpPacketInfo");
            this.tlpPacketInfo.Controls.Add(this.tcPacketInfo, 0, 0);
            this.tlpPacketInfo.Name = "tlpPacketInfo";
            // 
            // tcPacketInfo
            // 
            this.tcPacketInfo.Controls.Add(this.tpPacketData);
            this.tcPacketInfo.Controls.Add(this.tpSystemLog);
            resources.ApplyResources(this.tcPacketInfo, "tcPacketInfo");
            this.tcPacketInfo.Multiline = true;
            this.tcPacketInfo.Name = "tcPacketInfo";
            this.tcPacketInfo.SelectedIndex = 0;
            // 
            // tpPacketData
            // 
            this.tpPacketData.Controls.Add(this.tlpPacketData);
            resources.ApplyResources(this.tpPacketData, "tpPacketData");
            this.tpPacketData.Name = "tpPacketData";
            this.tpPacketData.UseVisualStyleBackColor = true;
            // 
            // tlpPacketData
            // 
            this.tlpPacketData.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.tlpPacketData, "tlpPacketData");
            this.tlpPacketData.Controls.Add(this.tlpHexBox, 0, 0);
            this.tlpPacketData.Name = "tlpPacketData";
            // 
            // tlpHexBox
            // 
            resources.ApplyResources(this.tlpHexBox, "tlpHexBox");
            this.tlpHexBox.Controls.Add(this.tlpConversion, 2, 1);
            this.tlpHexBox.Controls.Add(this.tsPacketData, 0, 0);
            this.tlpHexBox.Controls.Add(this.hbPacketData, 0, 1);
            this.tlpHexBox.Controls.Add(this.lPacketDataPosition, 2, 0);
            this.tlpHexBox.Name = "tlpHexBox";
            // 
            // tlpConversion
            // 
            resources.ApplyResources(this.tlpConversion, "tlpConversion");
            this.tlpConversion.Controls.Add(this.lBits_Value, 1, 0);
            this.tlpConversion.Controls.Add(this.lBits, 0, 0);
            this.tlpConversion.Controls.Add(this.lDouble_Value, 1, 9);
            this.tlpConversion.Controls.Add(this.lDouble, 0, 9);
            this.tlpConversion.Controls.Add(this.lFloat_Value, 1, 8);
            this.tlpConversion.Controls.Add(this.lFloat, 0, 8);
            this.tlpConversion.Controls.Add(this.lUInt64_Value, 1, 7);
            this.tlpConversion.Controls.Add(this.lUInt64, 0, 7);
            this.tlpConversion.Controls.Add(this.lInt64_Value, 1, 6);
            this.tlpConversion.Controls.Add(this.lInt64, 0, 6);
            this.tlpConversion.Controls.Add(this.lUInt32_Value, 1, 5);
            this.tlpConversion.Controls.Add(this.lUInt32, 0, 5);
            this.tlpConversion.Controls.Add(this.lInt32_Value, 1, 4);
            this.tlpConversion.Controls.Add(this.lInt32, 0, 4);
            this.tlpConversion.Controls.Add(this.lUShort_Value, 1, 3);
            this.tlpConversion.Controls.Add(this.lUShort, 0, 3);
            this.tlpConversion.Controls.Add(this.lShort_Value, 1, 2);
            this.tlpConversion.Controls.Add(this.lShort, 0, 2);
            this.tlpConversion.Controls.Add(this.lByte_Value, 1, 1);
            this.tlpConversion.Controls.Add(this.lByte, 0, 1);
            this.tlpConversion.Name = "tlpConversion";
            // 
            // lBits_Value
            // 
            resources.ApplyResources(this.lBits_Value, "lBits_Value");
            this.lBits_Value.Name = "lBits_Value";
            // 
            // lBits
            // 
            resources.ApplyResources(this.lBits, "lBits");
            this.lBits.Name = "lBits";
            // 
            // lDouble_Value
            // 
            resources.ApplyResources(this.lDouble_Value, "lDouble_Value");
            this.lDouble_Value.Name = "lDouble_Value";
            // 
            // lDouble
            // 
            resources.ApplyResources(this.lDouble, "lDouble");
            this.lDouble.Name = "lDouble";
            // 
            // lFloat_Value
            // 
            resources.ApplyResources(this.lFloat_Value, "lFloat_Value");
            this.lFloat_Value.Name = "lFloat_Value";
            // 
            // lFloat
            // 
            resources.ApplyResources(this.lFloat, "lFloat");
            this.lFloat.Name = "lFloat";
            // 
            // lUInt64_Value
            // 
            resources.ApplyResources(this.lUInt64_Value, "lUInt64_Value");
            this.lUInt64_Value.Name = "lUInt64_Value";
            // 
            // lUInt64
            // 
            resources.ApplyResources(this.lUInt64, "lUInt64");
            this.lUInt64.Name = "lUInt64";
            // 
            // lInt64_Value
            // 
            resources.ApplyResources(this.lInt64_Value, "lInt64_Value");
            this.lInt64_Value.Name = "lInt64_Value";
            // 
            // lInt64
            // 
            resources.ApplyResources(this.lInt64, "lInt64");
            this.lInt64.Name = "lInt64";
            // 
            // lUInt32_Value
            // 
            resources.ApplyResources(this.lUInt32_Value, "lUInt32_Value");
            this.lUInt32_Value.Name = "lUInt32_Value";
            // 
            // lUInt32
            // 
            resources.ApplyResources(this.lUInt32, "lUInt32");
            this.lUInt32.Name = "lUInt32";
            // 
            // lInt32_Value
            // 
            resources.ApplyResources(this.lInt32_Value, "lInt32_Value");
            this.lInt32_Value.Name = "lInt32_Value";
            // 
            // lInt32
            // 
            resources.ApplyResources(this.lInt32, "lInt32");
            this.lInt32.Name = "lInt32";
            // 
            // lUShort_Value
            // 
            resources.ApplyResources(this.lUShort_Value, "lUShort_Value");
            this.lUShort_Value.Name = "lUShort_Value";
            // 
            // lUShort
            // 
            resources.ApplyResources(this.lUShort, "lUShort");
            this.lUShort.Name = "lUShort";
            // 
            // lShort_Value
            // 
            resources.ApplyResources(this.lShort_Value, "lShort_Value");
            this.lShort_Value.Name = "lShort_Value";
            // 
            // lShort
            // 
            resources.ApplyResources(this.lShort, "lShort");
            this.lShort.Name = "lShort";
            // 
            // lByte_Value
            // 
            resources.ApplyResources(this.lByte_Value, "lByte_Value");
            this.lByte_Value.Name = "lByte_Value";
            // 
            // lByte
            // 
            resources.ApplyResources(this.lByte, "lByte");
            this.lByte.Name = "lByte";
            // 
            // tsPacketData
            // 
            this.tsPacketData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.toolStripSeparator9,
            this.tsbCut,
            this.toolStripSeparator10,
            this.tsbCopy,
            this.toolStripSeparator11,
            this.tsbPaste,
            this.toolStripSeparator12,
            this.tsbFind,
            this.toolStripSeparator13,
            this.tsbFindNext,
            this.toolStripSeparator14,
            this.tscbEncoding,
            this.toolStripSeparator15,
            this.tscbPerLine,
            this.toolStripSeparator16,
            this.tslPacketLen});
            resources.ApplyResources(this.tsPacketData, "tsPacketData");
            this.tsPacketData.Name = "tsPacketData";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::WPELibrary.Properties.Resources.saveHS;
            resources.ApplyResources(this.tsbSave, "tsbSave");
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Image = global::WPELibrary.Properties.Resources.CutHS;
            resources.ApplyResources(this.tsbCut, "tsbCut");
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopy,
            this.tsmiCopyHex});
            this.tsbCopy.Image = global::WPELibrary.Properties.Resources.CopyHS;
            resources.ApplyResources(this.tsbCopy, "tsbCopy");
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.ButtonClick += new System.EventHandler(this.tsbCopy_ButtonClick);
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Image = global::WPELibrary.Properties.Resources.CopyHS;
            this.tsmiCopy.Name = "tsmiCopy";
            resources.ApplyResources(this.tsmiCopy, "tsmiCopy");
            this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiCopyHex
            // 
            this.tsmiCopyHex.Image = global::WPELibrary.Properties.Resources.CopyHS;
            this.tsmiCopyHex.Name = "tsmiCopyHex";
            resources.ApplyResources(this.tsmiCopyHex, "tsmiCopyHex");
            this.tsmiCopyHex.Click += new System.EventHandler(this.tsmiCopyHex_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPaste,
            this.tsmiPasteHex});
            this.tsbPaste.Image = global::WPELibrary.Properties.Resources.PasteHS;
            resources.ApplyResources(this.tsbPaste, "tsbPaste");
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.ButtonClick += new System.EventHandler(this.tsbPaste_ButtonClick);
            // 
            // tsmiPaste
            // 
            this.tsmiPaste.Image = global::WPELibrary.Properties.Resources.PasteHS;
            this.tsmiPaste.Name = "tsmiPaste";
            resources.ApplyResources(this.tsmiPaste, "tsmiPaste");
            this.tsmiPaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tsmiPasteHex
            // 
            this.tsmiPasteHex.Image = global::WPELibrary.Properties.Resources.PasteHS;
            this.tsmiPasteHex.Name = "tsmiPasteHex";
            resources.ApplyResources(this.tsmiPasteHex, "tsmiPasteHex");
            this.tsmiPasteHex.Click += new System.EventHandler(this.tsmiPasteHex_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // tsbFind
            // 
            this.tsbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFind.Image = global::WPELibrary.Properties.Resources.FindHS;
            resources.ApplyResources(this.tsbFind, "tsbFind");
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // tsbFindNext
            // 
            this.tsbFindNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFindNext.Image = global::WPELibrary.Properties.Resources.FindNextHS;
            resources.ApplyResources(this.tsbFindNext, "tsbFindNext");
            this.tsbFindNext.Name = "tsbFindNext";
            this.tsbFindNext.Click += new System.EventHandler(this.tsbFindNext_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            // 
            // tscbEncoding
            // 
            this.tscbEncoding.BackColor = System.Drawing.SystemColors.Control;
            this.tscbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tscbEncoding, "tscbEncoding");
            this.tscbEncoding.Name = "tscbEncoding";
            this.tscbEncoding.SelectedIndexChanged += new System.EventHandler(this.tscbEncoding_SelectedIndexChanged);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            // 
            // tscbPerLine
            // 
            this.tscbPerLine.BackColor = System.Drawing.SystemColors.Control;
            this.tscbPerLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tscbPerLine, "tscbPerLine");
            this.tscbPerLine.Items.AddRange(new object[] {
            resources.GetString("tscbPerLine.Items"),
            resources.GetString("tscbPerLine.Items1")});
            this.tscbPerLine.Name = "tscbPerLine";
            this.tscbPerLine.SelectedIndexChanged += new System.EventHandler(this.tscbPerLine_SelectedIndexChanged);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            resources.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
            // 
            // tslPacketLen
            // 
            resources.ApplyResources(this.tslPacketLen, "tslPacketLen");
            this.tslPacketLen.Name = "tslPacketLen";
            // 
            // hbPacketData
            // 
            this.hbPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            this.hbPacketData.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.CopyHS;
            this.hbPacketData.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.CopyMenuItemText");
            this.hbPacketData.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.CutHS;
            this.hbPacketData.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.CutMenuItemText");
            this.hbPacketData.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.PasteHS;
            this.hbPacketData.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.PasteMenuItemText");
            this.hbPacketData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbPacketData.ColumnInfoVisible = true;
            resources.ApplyResources(this.hbPacketData, "hbPacketData");
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.UseFixedBytesPerLine = true;
            this.hbPacketData.VScrollBarVisible = true;
            this.hbPacketData.SelectionStartChanged += new System.EventHandler(this.hbPacketData_SelectionStartChanged);
            this.hbPacketData.SelectionLengthChanged += new System.EventHandler(this.hbPacketData_SelectionLengthChanged);
            this.hbPacketData.CurrentLineChanged += new System.EventHandler(this.hbPacketData_CurrentLineChanged);
            this.hbPacketData.CurrentPositionInLineChanged += new System.EventHandler(this.hbPacketData_CurrentPositionInLineChanged);
            this.hbPacketData.Copied += new System.EventHandler(this.hbPacketData_Copied);
            this.hbPacketData.CopiedHex += new System.EventHandler(this.hbPacketData_CopiedHex);
            // 
            // lPacketDataPosition
            // 
            resources.ApplyResources(this.lPacketDataPosition, "lPacketDataPosition");
            this.lPacketDataPosition.Name = "lPacketDataPosition";
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
            this.dgvLogList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
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
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLogList.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvLogList.RowTemplate.Height = 23;
            this.dgvLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // cTime
            // 
            this.cTime.DataPropertyName = "Time";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cTime.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.cTime, "cTime");
            this.cTime.Name = "cTime";
            this.cTime.ReadOnly = true;
            // 
            // cFuncName
            // 
            this.cFuncName.DataPropertyName = "FuncName";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cFuncName.DefaultCellStyle = dataGridViewCellStyle12;
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
            // tlpFilterList
            // 
            resources.ApplyResources(this.tlpFilterList, "tlpFilterList");
            this.tlpFilterList.Controls.Add(this.gbFilterList, 0, 0);
            this.tlpFilterList.Name = "tlpFilterList";
            // 
            // gbFilterList
            // 
            this.gbFilterList.Controls.Add(this.dgvFilterList);
            resources.ApplyResources(this.gbFilterList, "gbFilterList");
            this.gbFilterList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.dgvFilterList.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.NullValue = false;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIsCheck.DefaultCellStyle = dataGridViewCellStyle14;
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
            // ssProcessInfo
            // 
            this.ssProcessInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslProcessName,
            this.toolStripStatusLabel1,
            this.tsslProcessInfo,
            this.toolStripStatusLabel4,
            this.tsslWinSock,
            this.toolStripStatusLabel3,
            this.tsslTotalBytes});
            resources.ApplyResources(this.ssProcessInfo, "ssProcessInfo");
            this.ssProcessInfo.Name = "ssProcessInfo";
            // 
            // tsslProcessName
            // 
            this.tsslProcessName.Name = "tsslProcessName";
            resources.ApplyResources(this.tsslProcessName, "tsslProcessName");
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // tsslProcessInfo
            // 
            this.tsslProcessInfo.Name = "tsslProcessInfo";
            resources.ApplyResources(this.tsslProcessInfo, "tsslProcessInfo");
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
            // 
            // tsslWinSock
            // 
            this.tsslWinSock.Name = "tsslWinSock";
            resources.ApplyResources(this.tsslWinSock, "tsslWinSock");
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // tsslTotalBytes
            // 
            this.tsslTotalBytes.Name = "tsslTotalBytes";
            resources.ApplyResources(this.tsslTotalBytes, "tsslTotalBytes");
            // 
            // bgwSocketList
            // 
            this.bgwSocketList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSocketList_DoWork);
            // 
            // tSocketInfo
            // 
            this.tSocketInfo.Interval = 10;
            this.tSocketInfo.Tick += new System.EventHandler(this.tSocketInfo_Tick);
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
            // bgwSearchPacketData
            // 
            this.bgwSearchPacketData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSearchPacketData_DoWork);
            this.bgwSearchPacketData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSearchPacketData_RunWorkerCompleted);
            // 
            // niWPE
            // 
            this.niWPE.ContextMenuStrip = this.cmsIcon;
            resources.ApplyResources(this.niWPE, "niWPE");
            this.niWPE.Click += new System.EventHandler(this.niWPE_Click);
            // 
            // cmsIcon
            // 
            this.cmsIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShow,
            this.toolStripSeparator17,
            this.tsmiStartHook,
            this.tsmiStopHook,
            this.toolStripSeparator18,
            this.tsmiCleanUp,
            this.toolStripSeparator19,
            this.tsmiShowSendList,
            this.toolStripSeparator20,
            this.tsmiExit});
            this.cmsIcon.Name = "cmsIcon";
            resources.ApplyResources(this.cmsIcon, "cmsIcon");
            this.cmsIcon.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsIcon_ItemClicked);
            // 
            // tsmiShow
            // 
            this.tsmiShow.Image = global::WPELibrary.Properties.Resources.Counterclockwise_arrow16;
            this.tsmiShow.Name = "tsmiShow";
            resources.ApplyResources(this.tsmiShow, "tsmiShow");
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            resources.ApplyResources(this.toolStripSeparator17, "toolStripSeparator17");
            // 
            // tsmiStartHook
            // 
            this.tsmiStartHook.Image = global::WPELibrary.Properties.Resources.Play16;
            this.tsmiStartHook.Name = "tsmiStartHook";
            resources.ApplyResources(this.tsmiStartHook, "tsmiStartHook");
            // 
            // tsmiStopHook
            // 
            this.tsmiStopHook.Image = global::WPELibrary.Properties.Resources.Stop16;
            this.tsmiStopHook.Name = "tsmiStopHook";
            resources.ApplyResources(this.tsmiStopHook, "tsmiStopHook");
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            resources.ApplyResources(this.toolStripSeparator18, "toolStripSeparator18");
            // 
            // tsmiCleanUp
            // 
            this.tsmiCleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.tsmiCleanUp.Name = "tsmiCleanUp";
            resources.ApplyResources(this.tsmiCleanUp, "tsmiCleanUp");
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            resources.ApplyResources(this.toolStripSeparator19, "toolStripSeparator19");
            // 
            // tsmiShowSendList
            // 
            this.tsmiShowSendList.Image = global::WPELibrary.Properties.Resources.File_info16;
            this.tsmiShowSendList.Name = "tsmiShowSendList";
            resources.ApplyResources(this.tsmiShowSendList, "tsmiShowSendList");
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            resources.ApplyResources(this.toolStripSeparator20, "toolStripSeparator20");
            // 
            // tsmiExit
            // 
            this.tsmiExit.Image = global::WPELibrary.Properties.Resources.logout24;
            this.tsmiExit.Name = "tsmiExit";
            resources.ApplyResources(this.tsmiExit, "tsmiExit");
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle15.NullValue")));
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle15;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Image = global::WPELibrary.Properties.Resources.sent;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Socket_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSocketForm);
            this.DoubleBuffered = true;
            this.Name = "Socket_Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DLL_Form_FormClosed);
            this.Load += new System.EventHandler(this.DLL_Form_Load);
            this.Resize += new System.EventHandler(this.Socket_Form_Resize);
            this.tlpSocketForm.ResumeLayout(false);
            this.tlpSocketForm.PerformLayout();
            this.ssSocketList.ResumeLayout(false);
            this.ssSocketList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).EndInit();
            this.cmsSocketList.ResumeLayout(false);
            this.tlpParameter.ResumeLayout(false);
            this.gbFilterSet.ResumeLayout(false);
            this.tlpFilterSet.ResumeLayout(false);
            this.tlpFilterSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_From)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCheck_Size_To)).EndInit();
            this.gbHookType.ResumeLayout(false);
            this.tlpHookType.ResumeLayout(false);
            this.tlpHookType.PerformLayout();
            this.gbHookButton_Search.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.pSearchSocketList.ResumeLayout(false);
            this.pSearchSocketList.PerformLayout();
            this.tlpSearchButton.ResumeLayout(false);
            this.tlpHookButton.ResumeLayout(false);
            this.tlpHookButton_Start.ResumeLayout(false);
            this.tlpInformation.ResumeLayout(false);
            this.tlpPacketInfo.ResumeLayout(false);
            this.tcPacketInfo.ResumeLayout(false);
            this.tpPacketData.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.tlpHexBox.ResumeLayout(false);
            this.tlpHexBox.PerformLayout();
            this.tlpConversion.ResumeLayout(false);
            this.tlpConversion.PerformLayout();
            this.tsPacketData.ResumeLayout(false);
            this.tsPacketData.PerformLayout();
            this.tpSystemLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
            this.cmsLogList.ResumeLayout(false);
            this.tlpFilterList.ResumeLayout(false);
            this.gbFilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).EndInit();
            this.cmsFilterList.ResumeLayout(false);
            this.ssProcessInfo.ResumeLayout(false);
            this.ssProcessInfo.PerformLayout();
            this.cmsIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSocketForm;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.GroupBox gbHookType;
        private System.Windows.Forms.TableLayoutPanel tlpHookType;
        private System.Windows.Forms.CheckBox cbHook_RecvFrom;
        private System.Windows.Forms.CheckBox cbHook_Send;
        private System.Windows.Forms.CheckBox cbHook_SendTo;
        private System.Windows.Forms.CheckBox cbHook_Recv;
        private System.Windows.Forms.CheckBox cbHook_WSASend;
        private System.Windows.Forms.CheckBox cbHook_WSASendTo;
        private System.Windows.Forms.CheckBox cbHook_WSARecv;
        private System.Windows.Forms.CheckBox cbHook_WSARecvFrom;
        private System.Windows.Forms.GroupBox gbFilterSet;
        private System.Windows.Forms.TableLayoutPanel tlpFilterSet;
        private System.Windows.Forms.CheckBox cbCheck_Packet;
        private System.Windows.Forms.CheckBox cbCheck_Socket;
        private System.Windows.Forms.CheckBox cbCheck_IP;
        private System.Windows.Forms.TextBox txtCheck_Socket;
        private System.Windows.Forms.TextBox txtCheck_IP;
        private System.Windows.Forms.TextBox txtCheck_Packet;
        private System.Windows.Forms.CheckBox cbCheck_Size;
        private System.Windows.Forms.NumericUpDown nudCheck_Size_From;
        private System.Windows.Forms.NumericUpDown nudCheck_Size_To;
        private System.Windows.Forms.DataGridView dgvSocketList;
        private System.Windows.Forms.TableLayoutPanel tlpInformation;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo;
        private System.Windows.Forms.TabControl tcPacketInfo;
        private System.Windows.Forms.TabPage tpSystemLog;
        private System.Windows.Forms.DataGridView dgvLogList;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFuncName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cContent;
        private System.Windows.Forms.ContextMenuStrip cmsFilterList;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowFilter;
        private System.Windows.Forms.ToolStripSeparator tssSep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddFilter;
        private System.Windows.Forms.ToolStripSeparator tssSep3;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveFilterList;
        private System.Windows.Forms.ToolStripSeparator tssSep2;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadFilterList;
        private System.Windows.Forms.ContextMenuStrip cmsSocketList;
        private System.Windows.Forms.ToolStripMenuItem tsmiSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiBatchSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddToFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsmiUseSocket;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowBatchSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveSocketInfo;
        private System.Windows.Forms.ContextMenuStrip cmsLogList;
        private System.Windows.Forms.ToolStripMenuItem tslClearList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tslToExcel;
        private System.ComponentModel.BackgroundWorker bgwSocketList;
        private System.Windows.Forms.Timer tSocketInfo;
        private System.ComponentModel.BackgroundWorker bgwLogList;
        private System.ComponentModel.BackgroundWorker bgwSendFrom;
        private System.Windows.Forms.TabPage tpPacketData;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.TableLayoutPanel tlpHexBox;
        private Be.Windows.Forms.HexBox hbPacketData;
        private System.Windows.Forms.ToolStrip tsPacketData;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripSplitButton tsbCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyHex;
        private System.Windows.Forms.ToolStripSplitButton tsbPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiPasteHex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton tsbFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton tsbFindNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripComboBox tscbEncoding;
        private System.Windows.Forms.ToolStripComboBox tscbPerLine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.StatusStrip ssSocketList;
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
        private System.Windows.Forms.ToolStripStatusLabel tlFilter;
        private System.Windows.Forms.ToolStripStatusLabel tlFilter_CNT;
        private System.Windows.Forms.GroupBox gbHookButton_Search;
        private System.Windows.Forms.TableLayoutPanel tlpHookButton;
        private System.Windows.Forms.Button bCleanUp;
        private System.Windows.Forms.TableLayoutPanel tlpHookButton_Start;
        private System.Windows.Forms.Button bStopHook;
        private System.Windows.Forms.Button bStartHook;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private System.Windows.Forms.Panel pSearchSocketList;
        private System.Windows.Forms.RadioButton rbFromHead;
        private System.Windows.Forms.RadioButton rbFromIndex;
        private System.Windows.Forms.TableLayoutPanel tlpSearchButton;
        private System.Windows.Forms.Button bSearchNext;
        private System.Windows.Forms.Button bSearch;
        private System.ComponentModel.BackgroundWorker bgwSearchPacketData;
        private System.Windows.Forms.StatusStrip ssProcessInfo;
        private System.Windows.Forms.ToolStripStatusLabel tsslProcessName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tlSendTo;
        private System.Windows.Forms.ToolStripStatusLabel tlSendTo_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel tlRecvFrom;
        private System.Windows.Forms.ToolStripStatusLabel tlRecvFrom_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASend;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASend_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecv;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecv_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel14;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASendTo;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASendTo_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel17;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecvFrom;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecvFrom_CNT;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn cTypeImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.TableLayoutPanel tlpFilterList;
        private System.Windows.Forms.GroupBox gbFilterList;
        private System.Windows.Forms.DataGridView dgvFilterList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFName;
        private System.Windows.Forms.ToolStripLabel tslPacketLen;
        private System.Windows.Forms.Label lPacketDataPosition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripStatusLabel tsslTotalBytes;
        private System.Windows.Forms.ToolStripStatusLabel tsslProcessInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsslWinSock;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.TableLayoutPanel tlpConversion;
        private System.Windows.Forms.Label lBits_Value;
        private System.Windows.Forms.Label lBits;
        private System.Windows.Forms.Label lDouble_Value;
        private System.Windows.Forms.Label lDouble;
        private System.Windows.Forms.Label lFloat_Value;
        private System.Windows.Forms.Label lFloat;
        private System.Windows.Forms.Label lUInt64_Value;
        private System.Windows.Forms.Label lUInt64;
        private System.Windows.Forms.Label lInt64_Value;
        private System.Windows.Forms.Label lInt64;
        private System.Windows.Forms.Label lUInt32_Value;
        private System.Windows.Forms.Label lUInt32;
        private System.Windows.Forms.Label lInt32_Value;
        private System.Windows.Forms.Label lInt32;
        private System.Windows.Forms.Label lUShort_Value;
        private System.Windows.Forms.Label lUShort;
        private System.Windows.Forms.Label lShort_Value;
        private System.Windows.Forms.Label lShort;
        private System.Windows.Forms.Label lByte_Value;
        private System.Windows.Forms.Label lByte;
        private System.Windows.Forms.NotifyIcon niWPE;
        private System.Windows.Forms.ContextMenuStrip cmsIcon;
        private System.Windows.Forms.ToolStripMenuItem tsmiShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiStartHook;
        private System.Windows.Forms.ToolStripMenuItem tsmiStopHook;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem tsmiCleanUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowSendList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
    }
}
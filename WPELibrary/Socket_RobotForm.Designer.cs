namespace WPELibrary
{
    partial class Socket_RobotForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_RobotForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpRobotForm = new System.Windows.Forms.TableLayoutPanel();
            this.ssRobotInstruction = new System.Windows.Forms.StatusStrip();
            this.ssRobotInstruction_Total = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssRobotInstruction_Total_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssRobotInstruction_Split1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssRobotInstruction_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssRobotInstruction_Success_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssRobotInstruction_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.ssRobotInstruction_Fail_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.gbSendList = new System.Windows.Forms.GroupBox();
            this.dgvSendList = new System.Windows.Forms.DataGridView();
            this.cSendList_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpRobotInfo = new System.Windows.Forms.TableLayoutPanel();
            this.gbRobotName = new System.Windows.Forms.GroupBox();
            this.txtRobotName = new System.Windows.Forms.TextBox();
            this.gbRobotInstruction = new System.Windows.Forms.GroupBox();
            this.dgvRobotInstruction = new System.Windows.Forms.DataGridView();
            this.cRobotInstruction_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRobotInstruction_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRobotInstruction_Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsRobotInstruction = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsRobotInstruction_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRobotInstruction_Up = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotInstruction_Down = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRobotInstruction_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRobotInstruction_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotInstruction_CleanUp = new System.Windows.Forms.ToolStripMenuItem();
            this.tcRobotInstruction = new System.Windows.Forms.TabControl();
            this.tpInstruction_Socket = new System.Windows.Forms.TabPage();
            this.tlpInstruction_Socket = new System.Windows.Forms.TableLayoutPanel();
            this.tlpDelayAndLoop = new System.Windows.Forms.TableLayoutPanel();
            this.gbLoop = new System.Windows.Forms.GroupBox();
            this.tlpLoop = new System.Windows.Forms.TableLayoutPanel();
            this.bLoopEnd = new System.Windows.Forms.Button();
            this.bLoopStart = new System.Windows.Forms.Button();
            this.lLoop = new System.Windows.Forms.Label();
            this.nudLoop = new System.Windows.Forms.NumericUpDown();
            this.gbDelay = new System.Windows.Forms.GroupBox();
            this.tlpDelay = new System.Windows.Forms.TableLayoutPanel();
            this.bDelay = new System.Windows.Forms.Button();
            this.lDelay = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.gbSend = new System.Windows.Forms.GroupBox();
            this.tlpSend = new System.Windows.Forms.TableLayoutPanel();
            this.nudSendSocket = new System.Windows.Forms.NumericUpDown();
            this.lSendSocket = new System.Windows.Forms.Label();
            this.nudSendInterval = new System.Windows.Forms.NumericUpDown();
            this.lSendInterval = new System.Windows.Forms.Label();
            this.nudSendTimes = new System.Windows.Forms.NumericUpDown();
            this.lSendTimes = new System.Windows.Forms.Label();
            this.lSendIndex = new System.Windows.Forms.Label();
            this.nudSendIndex = new System.Windows.Forms.NumericUpDown();
            this.bSend = new System.Windows.Forms.Button();
            this.tpInstruction_keyboard = new System.Windows.Forms.TabPage();
            this.tlpInstruction_keyboard = new System.Windows.Forms.TableLayoutPanel();
            this.gbKeyboard_combination = new System.Windows.Forms.GroupBox();
            this.gbKeyboard = new System.Windows.Forms.GroupBox();
            this.tlpKeyboard = new System.Windows.Forms.TableLayoutPanel();
            this.nudKeyBoard_Interval = new System.Windows.Forms.NumericUpDown();
            this.lKeyBoard_Interval = new System.Windows.Forms.Label();
            this.lKeyBoard_Times = new System.Windows.Forms.Label();
            this.lKeyBoard_Type = new System.Windows.Forms.Label();
            this.lKeyBoard_KeyCode = new System.Windows.Forms.Label();
            this.txtKeyBoard_KeyCode = new System.Windows.Forms.TextBox();
            this.cbKeyBoard_Type = new System.Windows.Forms.ComboBox();
            this.nudKeyBoard_Times = new System.Windows.Forms.NumericUpDown();
            this.bKeyBoard = new System.Windows.Forms.Button();
            this.tpInstruction_Mouse = new System.Windows.Forms.TabPage();
            this.tlpInstruction_Mouse = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bStop = new System.Windows.Forms.Button();
            this.bExecute = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.tlpRobotForm.SuspendLayout();
            this.ssRobotInstruction.SuspendLayout();
            this.tlpListInfo.SuspendLayout();
            this.gbSendList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendList)).BeginInit();
            this.tlpRobotInfo.SuspendLayout();
            this.gbRobotName.SuspendLayout();
            this.gbRobotInstruction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRobotInstruction)).BeginInit();
            this.cmsRobotInstruction.SuspendLayout();
            this.tcRobotInstruction.SuspendLayout();
            this.tpInstruction_Socket.SuspendLayout();
            this.tlpInstruction_Socket.SuspendLayout();
            this.tlpDelayAndLoop.SuspendLayout();
            this.gbLoop.SuspendLayout();
            this.tlpLoop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop)).BeginInit();
            this.gbDelay.SuspendLayout();
            this.tlpDelay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.gbSend.SuspendLayout();
            this.tlpSend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendSocket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendIndex)).BeginInit();
            this.tpInstruction_keyboard.SuspendLayout();
            this.tlpInstruction_keyboard.SuspendLayout();
            this.gbKeyboard.SuspendLayout();
            this.tlpKeyboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudKeyBoard_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudKeyBoard_Times)).BeginInit();
            this.tpInstruction_Mouse.SuspendLayout();
            this.tlpInstruction_Mouse.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRobotForm
            // 
            resources.ApplyResources(this.tlpRobotForm, "tlpRobotForm");
            this.tlpRobotForm.Controls.Add(this.ssRobotInstruction, 0, 3);
            this.tlpRobotForm.Controls.Add(this.tlpListInfo, 0, 0);
            this.tlpRobotForm.Controls.Add(this.tcRobotInstruction, 0, 1);
            this.tlpRobotForm.Controls.Add(this.tlpButton, 0, 2);
            this.tlpRobotForm.Name = "tlpRobotForm";
            // 
            // ssRobotInstruction
            // 
            resources.ApplyResources(this.ssRobotInstruction, "ssRobotInstruction");
            this.ssRobotInstruction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssRobotInstruction_Total,
            this.ssRobotInstruction_Total_Value,
            this.ssRobotInstruction_Split1,
            this.ssRobotInstruction_Success,
            this.ssRobotInstruction_Success_Value,
            this.toolStripStatusLabel3,
            this.ssRobotInstruction_Fail,
            this.ssRobotInstruction_Fail_Value});
            this.ssRobotInstruction.Name = "ssRobotInstruction";
            // 
            // ssRobotInstruction_Total
            // 
            this.ssRobotInstruction_Total.Name = "ssRobotInstruction_Total";
            resources.ApplyResources(this.ssRobotInstruction_Total, "ssRobotInstruction_Total");
            // 
            // ssRobotInstruction_Total_Value
            // 
            resources.ApplyResources(this.ssRobotInstruction_Total_Value, "ssRobotInstruction_Total_Value");
            this.ssRobotInstruction_Total_Value.ForeColor = System.Drawing.Color.RoyalBlue;
            this.ssRobotInstruction_Total_Value.Name = "ssRobotInstruction_Total_Value";
            // 
            // ssRobotInstruction_Split1
            // 
            this.ssRobotInstruction_Split1.ForeColor = System.Drawing.Color.DarkGray;
            this.ssRobotInstruction_Split1.Name = "ssRobotInstruction_Split1";
            resources.ApplyResources(this.ssRobotInstruction_Split1, "ssRobotInstruction_Split1");
            // 
            // ssRobotInstruction_Success
            // 
            this.ssRobotInstruction_Success.Name = "ssRobotInstruction_Success";
            resources.ApplyResources(this.ssRobotInstruction_Success, "ssRobotInstruction_Success");
            // 
            // ssRobotInstruction_Success_Value
            // 
            resources.ApplyResources(this.ssRobotInstruction_Success_Value, "ssRobotInstruction_Success_Value");
            this.ssRobotInstruction_Success_Value.ForeColor = System.Drawing.Color.DarkGreen;
            this.ssRobotInstruction_Success_Value.Name = "ssRobotInstruction_Success_Value";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // ssRobotInstruction_Fail
            // 
            this.ssRobotInstruction_Fail.Name = "ssRobotInstruction_Fail";
            resources.ApplyResources(this.ssRobotInstruction_Fail, "ssRobotInstruction_Fail");
            // 
            // ssRobotInstruction_Fail_Value
            // 
            resources.ApplyResources(this.ssRobotInstruction_Fail_Value, "ssRobotInstruction_Fail_Value");
            this.ssRobotInstruction_Fail_Value.ForeColor = System.Drawing.Color.DarkRed;
            this.ssRobotInstruction_Fail_Value.Name = "ssRobotInstruction_Fail_Value";
            // 
            // tlpListInfo
            // 
            resources.ApplyResources(this.tlpListInfo, "tlpListInfo");
            this.tlpListInfo.Controls.Add(this.gbSendList, 1, 0);
            this.tlpListInfo.Controls.Add(this.tlpRobotInfo, 0, 0);
            this.tlpListInfo.Name = "tlpListInfo";
            // 
            // gbSendList
            // 
            this.gbSendList.Controls.Add(this.dgvSendList);
            resources.ApplyResources(this.gbSendList, "gbSendList");
            this.gbSendList.Name = "gbSendList";
            this.gbSendList.TabStop = false;
            // 
            // dgvSendList
            // 
            this.dgvSendList.AllowUserToAddRows = false;
            this.dgvSendList.AllowUserToDeleteRows = false;
            this.dgvSendList.AllowUserToResizeRows = false;
            this.dgvSendList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvSendList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSendList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSendList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cSendList_ID,
            this.cNote,
            this.cSocket,
            this.cIPTo,
            this.cLen});
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
            this.dgvSendList.SelectionChanged += new System.EventHandler(this.dgvSendList_SelectionChanged);
            // 
            // cSendList_ID
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSendList_ID.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.cSendList_ID, "cSendList_ID");
            this.cSendList_ID.Name = "cSendList_ID";
            this.cSendList_ID.ReadOnly = true;
            this.cSendList_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cNote
            // 
            this.cNote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
            // tlpRobotInfo
            // 
            resources.ApplyResources(this.tlpRobotInfo, "tlpRobotInfo");
            this.tlpRobotInfo.Controls.Add(this.gbRobotName, 0, 0);
            this.tlpRobotInfo.Controls.Add(this.gbRobotInstruction, 0, 1);
            this.tlpRobotInfo.Name = "tlpRobotInfo";
            // 
            // gbRobotName
            // 
            this.gbRobotName.Controls.Add(this.txtRobotName);
            resources.ApplyResources(this.gbRobotName, "gbRobotName");
            this.gbRobotName.Name = "gbRobotName";
            this.gbRobotName.TabStop = false;
            // 
            // txtRobotName
            // 
            resources.ApplyResources(this.txtRobotName, "txtRobotName");
            this.txtRobotName.Name = "txtRobotName";
            // 
            // gbRobotInstruction
            // 
            this.gbRobotInstruction.Controls.Add(this.dgvRobotInstruction);
            resources.ApplyResources(this.gbRobotInstruction, "gbRobotInstruction");
            this.gbRobotInstruction.Name = "gbRobotInstruction";
            this.gbRobotInstruction.TabStop = false;
            // 
            // dgvRobotInstruction
            // 
            this.dgvRobotInstruction.AllowUserToAddRows = false;
            this.dgvRobotInstruction.AllowUserToDeleteRows = false;
            this.dgvRobotInstruction.AllowUserToResizeRows = false;
            this.dgvRobotInstruction.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvRobotInstruction.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvRobotInstruction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRobotInstruction.ColumnHeadersVisible = false;
            this.dgvRobotInstruction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cRobotInstruction_ID,
            this.cRobotInstruction_Type,
            this.cRobotInstruction_Content});
            this.dgvRobotInstruction.ContextMenuStrip = this.cmsRobotInstruction;
            resources.ApplyResources(this.dgvRobotInstruction, "dgvRobotInstruction");
            this.dgvRobotInstruction.MultiSelect = false;
            this.dgvRobotInstruction.Name = "dgvRobotInstruction";
            this.dgvRobotInstruction.RowHeadersVisible = false;
            this.dgvRobotInstruction.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvRobotInstruction.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvRobotInstruction.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.Window;
            this.dgvRobotInstruction.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkMagenta;
            this.dgvRobotInstruction.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRobotInstruction.RowTemplate.Height = 23;
            this.dgvRobotInstruction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRobotInstruction.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRobotInstruction_CellFormatting);
            // 
            // cRobotInstruction_ID
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cRobotInstruction_ID.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.cRobotInstruction_ID, "cRobotInstruction_ID");
            this.cRobotInstruction_ID.Name = "cRobotInstruction_ID";
            this.cRobotInstruction_ID.ReadOnly = true;
            this.cRobotInstruction_ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRobotInstruction_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cRobotInstruction_Type
            // 
            this.cRobotInstruction_Type.DataPropertyName = "Type";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cRobotInstruction_Type.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.cRobotInstruction_Type, "cRobotInstruction_Type");
            this.cRobotInstruction_Type.Name = "cRobotInstruction_Type";
            this.cRobotInstruction_Type.ReadOnly = true;
            this.cRobotInstruction_Type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRobotInstruction_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cRobotInstruction_Content
            // 
            this.cRobotInstruction_Content.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cRobotInstruction_Content.DataPropertyName = "Content";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cRobotInstruction_Content.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.cRobotInstruction_Content, "cRobotInstruction_Content");
            this.cRobotInstruction_Content.Name = "cRobotInstruction_Content";
            this.cRobotInstruction_Content.ReadOnly = true;
            this.cRobotInstruction_Content.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRobotInstruction_Content.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmsRobotInstruction
            // 
            this.cmsRobotInstruction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsRobotInstruction_Top,
            this.toolStripSeparator1,
            this.cmsRobotInstruction_Up,
            this.cmsRobotInstruction_Down,
            this.toolStripSeparator2,
            this.cmsRobotInstruction_Bottom,
            this.toolStripSeparator3,
            this.cmsRobotInstruction_Delete,
            this.cmsRobotInstruction_CleanUp});
            this.cmsRobotInstruction.Name = "cmsRobotInstruction";
            resources.ApplyResources(this.cmsRobotInstruction, "cmsRobotInstruction");
            this.cmsRobotInstruction.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsRobotInstruction_ItemClicked);
            // 
            // cmsRobotInstruction_Top
            // 
            this.cmsRobotInstruction_Top.Image = global::WPELibrary.Properties.Resources.go_top;
            this.cmsRobotInstruction_Top.Name = "cmsRobotInstruction_Top";
            resources.ApplyResources(this.cmsRobotInstruction_Top, "cmsRobotInstruction_Top");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // cmsRobotInstruction_Up
            // 
            this.cmsRobotInstruction_Up.Image = global::WPELibrary.Properties.Resources.Up;
            this.cmsRobotInstruction_Up.Name = "cmsRobotInstruction_Up";
            resources.ApplyResources(this.cmsRobotInstruction_Up, "cmsRobotInstruction_Up");
            // 
            // cmsRobotInstruction_Down
            // 
            this.cmsRobotInstruction_Down.Image = global::WPELibrary.Properties.Resources.Down;
            this.cmsRobotInstruction_Down.Name = "cmsRobotInstruction_Down";
            resources.ApplyResources(this.cmsRobotInstruction_Down, "cmsRobotInstruction_Down");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // cmsRobotInstruction_Bottom
            // 
            this.cmsRobotInstruction_Bottom.Image = global::WPELibrary.Properties.Resources.go_bottom;
            this.cmsRobotInstruction_Bottom.Name = "cmsRobotInstruction_Bottom";
            resources.ApplyResources(this.cmsRobotInstruction_Bottom, "cmsRobotInstruction_Bottom");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // cmsRobotInstruction_Delete
            // 
            this.cmsRobotInstruction_Delete.Image = global::WPELibrary.Properties.Resources.Delete;
            this.cmsRobotInstruction_Delete.Name = "cmsRobotInstruction_Delete";
            resources.ApplyResources(this.cmsRobotInstruction_Delete, "cmsRobotInstruction_Delete");
            // 
            // cmsRobotInstruction_CleanUp
            // 
            this.cmsRobotInstruction_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.cmsRobotInstruction_CleanUp.Name = "cmsRobotInstruction_CleanUp";
            resources.ApplyResources(this.cmsRobotInstruction_CleanUp, "cmsRobotInstruction_CleanUp");
            // 
            // tcRobotInstruction
            // 
            this.tcRobotInstruction.Controls.Add(this.tpInstruction_Socket);
            this.tcRobotInstruction.Controls.Add(this.tpInstruction_keyboard);
            this.tcRobotInstruction.Controls.Add(this.tpInstruction_Mouse);
            resources.ApplyResources(this.tcRobotInstruction, "tcRobotInstruction");
            this.tcRobotInstruction.Name = "tcRobotInstruction";
            this.tcRobotInstruction.SelectedIndex = 0;
            // 
            // tpInstruction_Socket
            // 
            this.tpInstruction_Socket.BackColor = System.Drawing.SystemColors.Control;
            this.tpInstruction_Socket.Controls.Add(this.tlpInstruction_Socket);
            resources.ApplyResources(this.tpInstruction_Socket, "tpInstruction_Socket");
            this.tpInstruction_Socket.Name = "tpInstruction_Socket";
            // 
            // tlpInstruction_Socket
            // 
            resources.ApplyResources(this.tlpInstruction_Socket, "tlpInstruction_Socket");
            this.tlpInstruction_Socket.Controls.Add(this.tlpDelayAndLoop, 1, 0);
            this.tlpInstruction_Socket.Controls.Add(this.gbSend, 0, 0);
            this.tlpInstruction_Socket.Name = "tlpInstruction_Socket";
            // 
            // tlpDelayAndLoop
            // 
            resources.ApplyResources(this.tlpDelayAndLoop, "tlpDelayAndLoop");
            this.tlpDelayAndLoop.Controls.Add(this.gbLoop, 0, 1);
            this.tlpDelayAndLoop.Controls.Add(this.gbDelay, 0, 0);
            this.tlpDelayAndLoop.Name = "tlpDelayAndLoop";
            // 
            // gbLoop
            // 
            this.gbLoop.Controls.Add(this.tlpLoop);
            resources.ApplyResources(this.gbLoop, "gbLoop");
            this.gbLoop.Name = "gbLoop";
            this.gbLoop.TabStop = false;
            // 
            // tlpLoop
            // 
            resources.ApplyResources(this.tlpLoop, "tlpLoop");
            this.tlpLoop.Controls.Add(this.bLoopEnd, 4, 0);
            this.tlpLoop.Controls.Add(this.bLoopStart, 2, 0);
            this.tlpLoop.Controls.Add(this.lLoop, 1, 0);
            this.tlpLoop.Controls.Add(this.nudLoop, 0, 0);
            this.tlpLoop.Name = "tlpLoop";
            // 
            // bLoopEnd
            // 
            resources.ApplyResources(this.bLoopEnd, "bLoopEnd");
            this.bLoopEnd.Name = "bLoopEnd";
            this.bLoopEnd.UseVisualStyleBackColor = true;
            this.bLoopEnd.Click += new System.EventHandler(this.bLoopEnd_Click);
            // 
            // bLoopStart
            // 
            resources.ApplyResources(this.bLoopStart, "bLoopStart");
            this.bLoopStart.Name = "bLoopStart";
            this.bLoopStart.UseVisualStyleBackColor = true;
            this.bLoopStart.Click += new System.EventHandler(this.bLoopStart_Click);
            // 
            // lLoop
            // 
            resources.ApplyResources(this.lLoop, "lLoop");
            this.lLoop.Name = "lLoop";
            // 
            // nudLoop
            // 
            resources.ApplyResources(this.nudLoop, "nudLoop");
            this.nudLoop.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudLoop.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLoop.Name = "nudLoop";
            this.nudLoop.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbDelay
            // 
            this.gbDelay.Controls.Add(this.tlpDelay);
            resources.ApplyResources(this.gbDelay, "gbDelay");
            this.gbDelay.Name = "gbDelay";
            this.gbDelay.TabStop = false;
            // 
            // tlpDelay
            // 
            resources.ApplyResources(this.tlpDelay, "tlpDelay");
            this.tlpDelay.Controls.Add(this.bDelay, 2, 0);
            this.tlpDelay.Controls.Add(this.lDelay, 1, 0);
            this.tlpDelay.Controls.Add(this.nudDelay, 0, 0);
            this.tlpDelay.Name = "tlpDelay";
            // 
            // bDelay
            // 
            resources.ApplyResources(this.bDelay, "bDelay");
            this.bDelay.Name = "bDelay";
            this.bDelay.UseVisualStyleBackColor = true;
            this.bDelay.Click += new System.EventHandler(this.bDelay_Click);
            // 
            // lDelay
            // 
            resources.ApplyResources(this.lDelay, "lDelay");
            this.lDelay.Name = "lDelay";
            // 
            // nudDelay
            // 
            resources.ApplyResources(this.nudDelay, "nudDelay");
            this.nudDelay.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // gbSend
            // 
            this.gbSend.Controls.Add(this.tlpSend);
            resources.ApplyResources(this.gbSend, "gbSend");
            this.gbSend.Name = "gbSend";
            this.gbSend.TabStop = false;
            // 
            // tlpSend
            // 
            resources.ApplyResources(this.tlpSend, "tlpSend");
            this.tlpSend.Controls.Add(this.nudSendSocket, 3, 0);
            this.tlpSend.Controls.Add(this.lSendSocket, 2, 0);
            this.tlpSend.Controls.Add(this.nudSendInterval, 3, 1);
            this.tlpSend.Controls.Add(this.lSendInterval, 2, 1);
            this.tlpSend.Controls.Add(this.nudSendTimes, 1, 1);
            this.tlpSend.Controls.Add(this.lSendTimes, 0, 1);
            this.tlpSend.Controls.Add(this.lSendIndex, 0, 0);
            this.tlpSend.Controls.Add(this.nudSendIndex, 1, 0);
            this.tlpSend.Controls.Add(this.bSend, 5, 1);
            this.tlpSend.Name = "tlpSend";
            // 
            // nudSendSocket
            // 
            resources.ApplyResources(this.nudSendSocket, "nudSendSocket");
            this.nudSendSocket.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudSendSocket.Name = "nudSendSocket";
            // 
            // lSendSocket
            // 
            resources.ApplyResources(this.lSendSocket, "lSendSocket");
            this.lSendSocket.Name = "lSendSocket";
            // 
            // nudSendInterval
            // 
            resources.ApplyResources(this.nudSendInterval, "nudSendInterval");
            this.nudSendInterval.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSendInterval.Name = "nudSendInterval";
            this.nudSendInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lSendInterval
            // 
            resources.ApplyResources(this.lSendInterval, "lSendInterval");
            this.lSendInterval.Name = "lSendInterval";
            // 
            // nudSendTimes
            // 
            resources.ApplyResources(this.nudSendTimes, "nudSendTimes");
            this.nudSendTimes.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSendTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendTimes.Name = "nudSendTimes";
            this.nudSendTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lSendTimes
            // 
            resources.ApplyResources(this.lSendTimes, "lSendTimes");
            this.lSendTimes.Name = "lSendTimes";
            // 
            // lSendIndex
            // 
            resources.ApplyResources(this.lSendIndex, "lSendIndex");
            this.lSendIndex.Name = "lSendIndex";
            // 
            // nudSendIndex
            // 
            resources.ApplyResources(this.nudSendIndex, "nudSendIndex");
            this.nudSendIndex.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSendIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendIndex.Name = "nudSendIndex";
            this.nudSendIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bSend
            // 
            resources.ApplyResources(this.bSend, "bSend");
            this.bSend.Name = "bSend";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // tpInstruction_keyboard
            // 
            this.tpInstruction_keyboard.BackColor = System.Drawing.SystemColors.Control;
            this.tpInstruction_keyboard.Controls.Add(this.tlpInstruction_keyboard);
            resources.ApplyResources(this.tpInstruction_keyboard, "tpInstruction_keyboard");
            this.tpInstruction_keyboard.Name = "tpInstruction_keyboard";
            // 
            // tlpInstruction_keyboard
            // 
            resources.ApplyResources(this.tlpInstruction_keyboard, "tlpInstruction_keyboard");
            this.tlpInstruction_keyboard.Controls.Add(this.gbKeyboard_combination, 1, 0);
            this.tlpInstruction_keyboard.Controls.Add(this.gbKeyboard, 0, 0);
            this.tlpInstruction_keyboard.Name = "tlpInstruction_keyboard";
            // 
            // gbKeyboard_combination
            // 
            resources.ApplyResources(this.gbKeyboard_combination, "gbKeyboard_combination");
            this.gbKeyboard_combination.Name = "gbKeyboard_combination";
            this.gbKeyboard_combination.TabStop = false;
            // 
            // gbKeyboard
            // 
            this.gbKeyboard.Controls.Add(this.tlpKeyboard);
            resources.ApplyResources(this.gbKeyboard, "gbKeyboard");
            this.gbKeyboard.Name = "gbKeyboard";
            this.gbKeyboard.TabStop = false;
            // 
            // tlpKeyboard
            // 
            resources.ApplyResources(this.tlpKeyboard, "tlpKeyboard");
            this.tlpKeyboard.Controls.Add(this.nudKeyBoard_Interval, 3, 1);
            this.tlpKeyboard.Controls.Add(this.lKeyBoard_Interval, 2, 1);
            this.tlpKeyboard.Controls.Add(this.lKeyBoard_Times, 0, 1);
            this.tlpKeyboard.Controls.Add(this.lKeyBoard_Type, 2, 0);
            this.tlpKeyboard.Controls.Add(this.lKeyBoard_KeyCode, 0, 0);
            this.tlpKeyboard.Controls.Add(this.txtKeyBoard_KeyCode, 1, 0);
            this.tlpKeyboard.Controls.Add(this.cbKeyBoard_Type, 3, 0);
            this.tlpKeyboard.Controls.Add(this.nudKeyBoard_Times, 1, 1);
            this.tlpKeyboard.Controls.Add(this.bKeyBoard, 5, 1);
            this.tlpKeyboard.Name = "tlpKeyboard";
            // 
            // nudKeyBoard_Interval
            // 
            resources.ApplyResources(this.nudKeyBoard_Interval, "nudKeyBoard_Interval");
            this.nudKeyBoard_Interval.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudKeyBoard_Interval.Name = "nudKeyBoard_Interval";
            this.nudKeyBoard_Interval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lKeyBoard_Interval
            // 
            resources.ApplyResources(this.lKeyBoard_Interval, "lKeyBoard_Interval");
            this.lKeyBoard_Interval.Name = "lKeyBoard_Interval";
            // 
            // lKeyBoard_Times
            // 
            resources.ApplyResources(this.lKeyBoard_Times, "lKeyBoard_Times");
            this.lKeyBoard_Times.Name = "lKeyBoard_Times";
            // 
            // lKeyBoard_Type
            // 
            resources.ApplyResources(this.lKeyBoard_Type, "lKeyBoard_Type");
            this.lKeyBoard_Type.Name = "lKeyBoard_Type";
            // 
            // lKeyBoard_KeyCode
            // 
            resources.ApplyResources(this.lKeyBoard_KeyCode, "lKeyBoard_KeyCode");
            this.lKeyBoard_KeyCode.Name = "lKeyBoard_KeyCode";
            // 
            // txtKeyBoard_KeyCode
            // 
            resources.ApplyResources(this.txtKeyBoard_KeyCode, "txtKeyBoard_KeyCode");
            this.txtKeyBoard_KeyCode.Name = "txtKeyBoard_KeyCode";
            this.txtKeyBoard_KeyCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyBoard_Key_KeyDown);
            // 
            // cbKeyBoard_Type
            // 
            resources.ApplyResources(this.cbKeyBoard_Type, "cbKeyBoard_Type");
            this.cbKeyBoard_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeyBoard_Type.FormattingEnabled = true;
            this.cbKeyBoard_Type.Items.AddRange(new object[] {
            resources.GetString("cbKeyBoard_Type.Items"),
            resources.GetString("cbKeyBoard_Type.Items1"),
            resources.GetString("cbKeyBoard_Type.Items2")});
            this.cbKeyBoard_Type.Name = "cbKeyBoard_Type";
            // 
            // nudKeyBoard_Times
            // 
            resources.ApplyResources(this.nudKeyBoard_Times, "nudKeyBoard_Times");
            this.nudKeyBoard_Times.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudKeyBoard_Times.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudKeyBoard_Times.Name = "nudKeyBoard_Times";
            this.nudKeyBoard_Times.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bKeyBoard
            // 
            resources.ApplyResources(this.bKeyBoard, "bKeyBoard");
            this.bKeyBoard.Name = "bKeyBoard";
            this.bKeyBoard.UseVisualStyleBackColor = true;
            this.bKeyBoard.Click += new System.EventHandler(this.bKeyBoard_Click);
            // 
            // tpInstruction_Mouse
            // 
            this.tpInstruction_Mouse.BackColor = System.Drawing.SystemColors.Control;
            this.tpInstruction_Mouse.Controls.Add(this.tlpInstruction_Mouse);
            resources.ApplyResources(this.tpInstruction_Mouse, "tpInstruction_Mouse");
            this.tpInstruction_Mouse.Name = "tpInstruction_Mouse";
            // 
            // tlpInstruction_Mouse
            // 
            resources.ApplyResources(this.tlpInstruction_Mouse, "tlpInstruction_Mouse");
            this.tlpInstruction_Mouse.Controls.Add(this.groupBox4, 1, 0);
            this.tlpInstruction_Mouse.Controls.Add(this.groupBox3, 0, 0);
            this.tlpInstruction_Mouse.Name = "tlpInstruction_Mouse";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bStop, 3, 0);
            this.tlpButton.Controls.Add(this.bExecute, 1, 0);
            this.tlpButton.Controls.Add(this.bClose, 7, 0);
            this.tlpButton.Controls.Add(this.bSave, 5, 0);
            this.tlpButton.Name = "tlpButton";
            // 
            // bStop
            // 
            resources.ApplyResources(this.bStop, "bStop");
            this.bStop.Name = "bStop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bExecute
            // 
            resources.ApplyResources(this.bExecute, "bExecute");
            this.bExecute.Name = "bExecute";
            this.bExecute.UseVisualStyleBackColor = true;
            this.bExecute.Click += new System.EventHandler(this.bExecute_Click);
            // 
            // bClose
            // 
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // Socket_RobotForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpRobotForm);
            this.DoubleBuffered = true;
            this.Name = "Socket_RobotForm";
            this.tlpRobotForm.ResumeLayout(false);
            this.tlpRobotForm.PerformLayout();
            this.ssRobotInstruction.ResumeLayout(false);
            this.ssRobotInstruction.PerformLayout();
            this.tlpListInfo.ResumeLayout(false);
            this.gbSendList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendList)).EndInit();
            this.tlpRobotInfo.ResumeLayout(false);
            this.gbRobotName.ResumeLayout(false);
            this.gbRobotName.PerformLayout();
            this.gbRobotInstruction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRobotInstruction)).EndInit();
            this.cmsRobotInstruction.ResumeLayout(false);
            this.tcRobotInstruction.ResumeLayout(false);
            this.tpInstruction_Socket.ResumeLayout(false);
            this.tlpInstruction_Socket.ResumeLayout(false);
            this.tlpDelayAndLoop.ResumeLayout(false);
            this.gbLoop.ResumeLayout(false);
            this.tlpLoop.ResumeLayout(false);
            this.tlpLoop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop)).EndInit();
            this.gbDelay.ResumeLayout(false);
            this.tlpDelay.ResumeLayout(false);
            this.tlpDelay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.gbSend.ResumeLayout(false);
            this.tlpSend.ResumeLayout(false);
            this.tlpSend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendSocket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendIndex)).EndInit();
            this.tpInstruction_keyboard.ResumeLayout(false);
            this.tlpInstruction_keyboard.ResumeLayout(false);
            this.gbKeyboard.ResumeLayout(false);
            this.tlpKeyboard.ResumeLayout(false);
            this.tlpKeyboard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudKeyBoard_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudKeyBoard_Times)).EndInit();
            this.tpInstruction_Mouse.ResumeLayout(false);
            this.tlpInstruction_Mouse.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRobotForm;
        private System.Windows.Forms.TableLayoutPanel tlpListInfo;
        private System.Windows.Forms.TabControl tcRobotInstruction;
        private System.Windows.Forms.TabPage tpInstruction_Socket;
        private System.Windows.Forms.TableLayoutPanel tlpInstruction_Socket;
        private System.Windows.Forms.GroupBox gbSend;
        private System.Windows.Forms.TableLayoutPanel tlpSend;
        private System.Windows.Forms.Label lSendIndex;
        private System.Windows.Forms.NumericUpDown nudSendIndex;
        private System.Windows.Forms.Label lSendTimes;
        private System.Windows.Forms.NumericUpDown nudSendTimes;
        private System.Windows.Forms.NumericUpDown nudSendInterval;
        private System.Windows.Forms.Label lSendInterval;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.ContextMenuStrip cmsRobotInstruction;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotInstruction_Top;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotInstruction_Bottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotInstruction_Up;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotInstruction_Down;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotInstruction_Delete;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.TableLayoutPanel tlpRobotInfo;
        private System.Windows.Forms.GroupBox gbRobotInstruction;
        private System.Windows.Forms.DataGridView dgvRobotInstruction;
        private System.Windows.Forms.GroupBox gbSendList;
        private System.Windows.Forms.DataGridView dgvSendList;
        private System.Windows.Forms.GroupBox gbRobotName;
        private System.Windows.Forms.TextBox txtRobotName;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotInstruction_CleanUp;
        private System.Windows.Forms.Button bExecute;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSendList_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRobotInstruction_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRobotInstruction_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRobotInstruction_Content;
        private System.Windows.Forms.TableLayoutPanel tlpDelayAndLoop;
        private System.Windows.Forms.GroupBox gbLoop;
        private System.Windows.Forms.TableLayoutPanel tlpLoop;
        private System.Windows.Forms.Button bLoopEnd;
        private System.Windows.Forms.Button bLoopStart;
        private System.Windows.Forms.Label lLoop;
        private System.Windows.Forms.NumericUpDown nudLoop;
        private System.Windows.Forms.GroupBox gbDelay;
        private System.Windows.Forms.TableLayoutPanel tlpDelay;
        private System.Windows.Forms.Button bDelay;
        private System.Windows.Forms.Label lDelay;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.NumericUpDown nudSendSocket;
        private System.Windows.Forms.Label lSendSocket;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.StatusStrip ssRobotInstruction;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Total;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Total_Value;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Split1;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Success;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Success_Value;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Fail;
        private System.Windows.Forms.ToolStripStatusLabel ssRobotInstruction_Fail_Value;
        private System.Windows.Forms.TabPage tpInstruction_keyboard;
        private System.Windows.Forms.TableLayoutPanel tlpInstruction_keyboard;
        private System.Windows.Forms.TabPage tpInstruction_Mouse;
        private System.Windows.Forms.TableLayoutPanel tlpInstruction_Mouse;
        private System.Windows.Forms.GroupBox gbKeyboard;
        private System.Windows.Forms.GroupBox gbKeyboard_combination;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tlpKeyboard;
        private System.Windows.Forms.Label lKeyBoard_Times;
        private System.Windows.Forms.Label lKeyBoard_Type;
        private System.Windows.Forms.Label lKeyBoard_KeyCode;
        private System.Windows.Forms.TextBox txtKeyBoard_KeyCode;
        private System.Windows.Forms.ComboBox cbKeyBoard_Type;
        private System.Windows.Forms.NumericUpDown nudKeyBoard_Times;
        private System.Windows.Forms.Button bKeyBoard;
        private System.Windows.Forms.NumericUpDown nudKeyBoard_Interval;
        private System.Windows.Forms.Label lKeyBoard_Interval;
    }
}
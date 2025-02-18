
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpSendList = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bClose = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bStop = new System.Windows.Forms.Button();
            this.bExecute = new System.Windows.Forms.Button();
            this.ssSocketSendList = new System.Windows.Forms.StatusStrip();
            this.tlTotal_Send = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlTotal_Send_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbSendName = new System.Windows.Forms.GroupBox();
            this.tlpSendName = new System.Windows.Forms.TableLayoutPanel();
            this.txtSendName = new System.Windows.Forms.TextBox();
            this.gbLoopINT = new System.Windows.Forms.GroupBox();
            this.tlpLoopINT = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.nudLoop_INT = new System.Windows.Forms.NumericUpDown();
            this.gbLoopCNT = new System.Windows.Forms.GroupBox();
            this.tlpLoopCNT = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.nudLoop_CNT = new System.Windows.Forms.NumericUpDown();
            this.gbSystemSocket = new System.Windows.Forms.GroupBox();
            this.tlpSystemSocket = new System.Windows.Forms.TableLayoutPanel();
            this.cbSystemSocket = new System.Windows.Forms.CheckBox();
            this.lSystemSocket = new System.Windows.Forms.Label();
            this.tlpDGV = new System.Windows.Forms.TableLayoutPanel();
            this.dgvSendCollection = new System.Windows.Forms.DataGridView();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsSendCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsSendList_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Up = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSendList_Down = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSendList_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_CleanUp = new System.Windows.Forms.ToolStripMenuItem();
            this.gbNotes = new System.Windows.Forms.GroupBox();
            this.rtbNotes = new System.Windows.Forms.RichTextBox();
            this.tlpSendList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.ssSocketSendList.SuspendLayout();
            this.tlpParameter.SuspendLayout();
            this.gbSendName.SuspendLayout();
            this.tlpSendName.SuspendLayout();
            this.gbLoopINT.SuspendLayout();
            this.tlpLoopINT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_INT)).BeginInit();
            this.gbLoopCNT.SuspendLayout();
            this.tlpLoopCNT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).BeginInit();
            this.gbSystemSocket.SuspendLayout();
            this.tlpSystemSocket.SuspendLayout();
            this.tlpDGV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendCollection)).BeginInit();
            this.cmsSendCollection.SuspendLayout();
            this.gbNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSendList
            // 
            resources.ApplyResources(this.tlpSendList, "tlpSendList");
            this.tlpSendList.Controls.Add(this.tlpButton, 0, 2);
            this.tlpSendList.Controls.Add(this.ssSocketSendList, 0, 3);
            this.tlpSendList.Controls.Add(this.tlpParameter, 0, 1);
            this.tlpSendList.Controls.Add(this.tlpDGV, 0, 0);
            this.tlpSendList.Name = "tlpSendList";
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bClose, 7, 1);
            this.tlpButton.Controls.Add(this.bSave, 5, 1);
            this.tlpButton.Controls.Add(this.bStop, 3, 1);
            this.tlpButton.Controls.Add(this.bExecute, 1, 1);
            this.tlpButton.Name = "tlpButton";
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
            // ssSocketSendList
            // 
            resources.ApplyResources(this.ssSocketSendList, "ssSocketSendList");
            this.ssSocketSendList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssSocketSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlTotal_Send,
            this.tlTotal_Send_CNT,
            this.tlSplit,
            this.tlSend_Success,
            this.tlSend_Success_CNT,
            this.toolStripStatusLabel3,
            this.tlSend_Fail,
            this.tlSend_Fail_CNT});
            this.ssSocketSendList.Name = "ssSocketSendList";
            this.ssSocketSendList.SizingGrip = false;
            // 
            // tlTotal_Send
            // 
            resources.ApplyResources(this.tlTotal_Send, "tlTotal_Send");
            this.tlTotal_Send.Name = "tlTotal_Send";
            // 
            // tlTotal_Send_CNT
            // 
            resources.ApplyResources(this.tlTotal_Send_CNT, "tlTotal_Send_CNT");
            this.tlTotal_Send_CNT.Name = "tlTotal_Send_CNT";
            // 
            // tlSplit
            // 
            resources.ApplyResources(this.tlSplit, "tlSplit");
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            // 
            // tlSend_Success
            // 
            resources.ApplyResources(this.tlSend_Success, "tlSend_Success");
            this.tlSend_Success.Name = "tlSend_Success";
            // 
            // tlSend_Success_CNT
            // 
            resources.ApplyResources(this.tlSend_Success_CNT, "tlSend_Success_CNT");
            this.tlSend_Success_CNT.ForeColor = System.Drawing.Color.Green;
            this.tlSend_Success_CNT.Name = "tlSend_Success_CNT";
            // 
            // toolStripStatusLabel3
            // 
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            // 
            // tlSend_Fail
            // 
            resources.ApplyResources(this.tlSend_Fail, "tlSend_Fail");
            this.tlSend_Fail.Name = "tlSend_Fail";
            // 
            // tlSend_Fail_CNT
            // 
            resources.ApplyResources(this.tlSend_Fail_CNT, "tlSend_Fail_CNT");
            this.tlSend_Fail_CNT.ForeColor = System.Drawing.Color.Red;
            this.tlSend_Fail_CNT.Name = "tlSend_Fail_CNT";
            // 
            // tlpParameter
            // 
            resources.ApplyResources(this.tlpParameter, "tlpParameter");
            this.tlpParameter.Controls.Add(this.gbSendName, 0, 0);
            this.tlpParameter.Controls.Add(this.gbLoopINT, 3, 0);
            this.tlpParameter.Controls.Add(this.gbLoopCNT, 2, 0);
            this.tlpParameter.Controls.Add(this.gbSystemSocket, 1, 0);
            this.tlpParameter.Name = "tlpParameter";
            // 
            // gbSendName
            // 
            resources.ApplyResources(this.gbSendName, "gbSendName");
            this.gbSendName.Controls.Add(this.tlpSendName);
            this.gbSendName.Name = "gbSendName";
            this.gbSendName.TabStop = false;
            // 
            // tlpSendName
            // 
            resources.ApplyResources(this.tlpSendName, "tlpSendName");
            this.tlpSendName.Controls.Add(this.txtSendName, 0, 0);
            this.tlpSendName.Name = "tlpSendName";
            // 
            // txtSendName
            // 
            resources.ApplyResources(this.txtSendName, "txtSendName");
            this.txtSendName.Name = "txtSendName";
            // 
            // gbLoopINT
            // 
            resources.ApplyResources(this.gbLoopINT, "gbLoopINT");
            this.gbLoopINT.Controls.Add(this.tlpLoopINT);
            this.gbLoopINT.Name = "gbLoopINT";
            this.gbLoopINT.TabStop = false;
            // 
            // tlpLoopINT
            // 
            resources.ApplyResources(this.tlpLoopINT, "tlpLoopINT");
            this.tlpLoopINT.Controls.Add(this.label3, 1, 0);
            this.tlpLoopINT.Controls.Add(this.nudLoop_INT, 0, 0);
            this.tlpLoopINT.Name = "tlpLoopINT";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // nudLoop_INT
            // 
            resources.ApplyResources(this.nudLoop_INT, "nudLoop_INT");
            this.nudLoop_INT.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudLoop_INT.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudLoop_INT.Name = "nudLoop_INT";
            this.nudLoop_INT.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // gbLoopCNT
            // 
            resources.ApplyResources(this.gbLoopCNT, "gbLoopCNT");
            this.gbLoopCNT.Controls.Add(this.tlpLoopCNT);
            this.gbLoopCNT.Name = "gbLoopCNT";
            this.gbLoopCNT.TabStop = false;
            // 
            // tlpLoopCNT
            // 
            resources.ApplyResources(this.tlpLoopCNT, "tlpLoopCNT");
            this.tlpLoopCNT.Controls.Add(this.label2, 0, 0);
            this.tlpLoopCNT.Controls.Add(this.nudLoop_CNT, 1, 0);
            this.tlpLoopCNT.Name = "tlpLoopCNT";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // nudLoop_CNT
            // 
            resources.ApplyResources(this.nudLoop_CNT, "nudLoop_CNT");
            this.nudLoop_CNT.Maximum = new decimal(new int[] {
            999999999,
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
            // gbSystemSocket
            // 
            resources.ApplyResources(this.gbSystemSocket, "gbSystemSocket");
            this.gbSystemSocket.Controls.Add(this.tlpSystemSocket);
            this.gbSystemSocket.Name = "gbSystemSocket";
            this.gbSystemSocket.TabStop = false;
            // 
            // tlpSystemSocket
            // 
            resources.ApplyResources(this.tlpSystemSocket, "tlpSystemSocket");
            this.tlpSystemSocket.Controls.Add(this.cbSystemSocket, 0, 0);
            this.tlpSystemSocket.Controls.Add(this.lSystemSocket, 1, 0);
            this.tlpSystemSocket.Name = "tlpSystemSocket";
            // 
            // cbSystemSocket
            // 
            resources.ApplyResources(this.cbSystemSocket, "cbSystemSocket");
            this.cbSystemSocket.Name = "cbSystemSocket";
            this.cbSystemSocket.UseVisualStyleBackColor = true;
            // 
            // lSystemSocket
            // 
            resources.ApplyResources(this.lSystemSocket, "lSystemSocket");
            this.lSystemSocket.Name = "lSystemSocket";
            // 
            // tlpDGV
            // 
            resources.ApplyResources(this.tlpDGV, "tlpDGV");
            this.tlpDGV.Controls.Add(this.dgvSendCollection, 0, 0);
            this.tlpDGV.Controls.Add(this.gbNotes, 1, 0);
            this.tlpDGV.Name = "tlpDGV";
            // 
            // dgvSendCollection
            // 
            resources.ApplyResources(this.dgvSendCollection, "dgvSendCollection");
            this.dgvSendCollection.AllowUserToAddRows = false;
            this.dgvSendCollection.AllowUserToDeleteRows = false;
            this.dgvSendCollection.AllowUserToResizeRows = false;
            this.dgvSendCollection.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSendCollection.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSendCollection.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgvSendCollection.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSendCollection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvSendCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSendCollection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cID,
            this.cSocket,
            this.cType,
            this.cIPTo,
            this.cLength,
            this.cData});
            this.dgvSendCollection.ContextMenuStrip = this.cmsSendCollection;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSendCollection.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvSendCollection.MultiSelect = false;
            this.dgvSendCollection.Name = "dgvSendCollection";
            this.dgvSendCollection.RowHeadersVisible = false;
            this.dgvSendCollection.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvSendCollection.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSendCollection.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgvSendCollection.RowTemplate.Height = 23;
            this.dgvSendCollection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSendCollection.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSendCollection_CellFormatting);
            // 
            // cID
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.cID, "cID");
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "Socket";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cType
            // 
            this.cType.DataPropertyName = "Type";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cType.DefaultCellStyle = dataGridViewCellStyle11;
            resources.ApplyResources(this.cType, "cType");
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIPTo
            // 
            this.cIPTo.DataPropertyName = "IPTo";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cIPTo.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.cIPTo, "cIPTo");
            this.cIPTo.Name = "cIPTo";
            this.cIPTo.ReadOnly = true;
            this.cIPTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLength
            // 
            this.cLength.DataPropertyName = "Length";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLength.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.cLength, "cLength");
            this.cLength.Name = "cLength";
            this.cLength.ReadOnly = true;
            this.cLength.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cData
            // 
            this.cData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.cData, "cData");
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmsSendCollection
            // 
            resources.ApplyResources(this.cmsSendCollection, "cmsSendCollection");
            this.cmsSendCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsSendList_Top,
            this.toolStripSeparator1,
            this.cmsSendList_Up,
            this.cmsSendList_Down,
            this.toolStripSeparator2,
            this.cmsSendList_Bottom,
            this.toolStripSeparator3,
            this.cmsSendList_Delete,
            this.toolStripSeparator4,
            this.cmsSendList_Export,
            this.cmsSendList_Import,
            this.toolStripSeparator5,
            this.cmsSendList_CleanUp});
            this.cmsSendCollection.Name = "cmsSendCollection";
            this.cmsSendCollection.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSendCollection_ItemClicked);
            // 
            // cmsSendList_Top
            // 
            resources.ApplyResources(this.cmsSendList_Top, "cmsSendList_Top");
            this.cmsSendList_Top.Image = global::WPELibrary.Properties.Resources.go_top;
            this.cmsSendList_Top.Name = "cmsSendList_Top";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // cmsSendList_Up
            // 
            resources.ApplyResources(this.cmsSendList_Up, "cmsSendList_Up");
            this.cmsSendList_Up.Image = global::WPELibrary.Properties.Resources.Up;
            this.cmsSendList_Up.Name = "cmsSendList_Up";
            // 
            // cmsSendList_Down
            // 
            resources.ApplyResources(this.cmsSendList_Down, "cmsSendList_Down");
            this.cmsSendList_Down.Image = global::WPELibrary.Properties.Resources.Down;
            this.cmsSendList_Down.Name = "cmsSendList_Down";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // cmsSendList_Bottom
            // 
            resources.ApplyResources(this.cmsSendList_Bottom, "cmsSendList_Bottom");
            this.cmsSendList_Bottom.Image = global::WPELibrary.Properties.Resources.go_bottom;
            this.cmsSendList_Bottom.Name = "cmsSendList_Bottom";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // cmsSendList_Delete
            // 
            resources.ApplyResources(this.cmsSendList_Delete, "cmsSendList_Delete");
            this.cmsSendList_Delete.Image = global::WPELibrary.Properties.Resources.ListDel;
            this.cmsSendList_Delete.Name = "cmsSendList_Delete";
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // cmsSendList_Export
            // 
            resources.ApplyResources(this.cmsSendList_Export, "cmsSendList_Export");
            this.cmsSendList_Export.Image = global::WPELibrary.Properties.Resources.export;
            this.cmsSendList_Export.Name = "cmsSendList_Export";
            // 
            // cmsSendList_Import
            // 
            resources.ApplyResources(this.cmsSendList_Import, "cmsSendList_Import");
            this.cmsSendList_Import.Image = global::WPELibrary.Properties.Resources.open;
            this.cmsSendList_Import.Name = "cmsSendList_Import";
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // cmsSendList_CleanUp
            // 
            resources.ApplyResources(this.cmsSendList_CleanUp, "cmsSendList_CleanUp");
            this.cmsSendList_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.cmsSendList_CleanUp.Name = "cmsSendList_CleanUp";
            // 
            // gbNotes
            // 
            resources.ApplyResources(this.gbNotes, "gbNotes");
            this.gbNotes.Controls.Add(this.rtbNotes);
            this.gbNotes.Name = "gbNotes";
            this.gbNotes.TabStop = false;
            // 
            // rtbNotes
            // 
            resources.ApplyResources(this.rtbNotes, "rtbNotes");
            this.rtbNotes.BackColor = System.Drawing.SystemColors.Control;
            this.rtbNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbNotes.Name = "rtbNotes";
            // 
            // Socket_SendListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSendList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_SendListForm";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SocketSendList_Form_FormClosed);
            this.Load += new System.EventHandler(this.SocketSendList_Form_Load);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendList.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.ssSocketSendList.ResumeLayout(false);
            this.ssSocketSendList.PerformLayout();
            this.tlpParameter.ResumeLayout(false);
            this.gbSendName.ResumeLayout(false);
            this.tlpSendName.ResumeLayout(false);
            this.tlpSendName.PerformLayout();
            this.gbLoopINT.ResumeLayout(false);
            this.tlpLoopINT.ResumeLayout(false);
            this.tlpLoopINT.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_INT)).EndInit();
            this.gbLoopCNT.ResumeLayout(false);
            this.tlpLoopCNT.ResumeLayout(false);
            this.tlpLoopCNT.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLoop_CNT)).EndInit();
            this.gbSystemSocket.ResumeLayout(false);
            this.tlpSystemSocket.ResumeLayout(false);
            this.tlpSystemSocket.PerformLayout();
            this.tlpDGV.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendCollection)).EndInit();
            this.cmsSendCollection.ResumeLayout(false);
            this.gbNotes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSendList;
        private System.Windows.Forms.StatusStrip ssSocketSendList;
        private System.Windows.Forms.ToolStripStatusLabel tlTotal_Send;
        private System.Windows.Forms.ToolStripStatusLabel tlTotal_Send_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail_CNT;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bExecute;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.GroupBox gbLoopINT;
        private System.Windows.Forms.TableLayoutPanel tlpLoopINT;
        private System.Windows.Forms.NumericUpDown nudLoop_INT;
        private System.Windows.Forms.GroupBox gbLoopCNT;
        private System.Windows.Forms.TableLayoutPanel tlpLoopCNT;
        private System.Windows.Forms.NumericUpDown nudLoop_CNT;
        private System.Windows.Forms.GroupBox gbSystemSocket;
        private System.Windows.Forms.TableLayoutPanel tlpSystemSocket;
        private System.Windows.Forms.CheckBox cbSystemSocket;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lSystemSocket;
        private System.Windows.Forms.GroupBox gbSendName;
        private System.Windows.Forms.TableLayoutPanel tlpSendName;
        private System.Windows.Forms.TextBox txtSendName;
        private System.Windows.Forms.ContextMenuStrip cmsSendCollection;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Delete;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Top;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Up;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Down;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Bottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_CleanUp;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Export;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Import;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.TableLayoutPanel tlpDGV;
        private System.Windows.Forms.DataGridView dgvSendCollection;
        private System.Windows.Forms.DataGridViewTextBoxColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.GroupBox gbNotes;
        private System.Windows.Forms.RichTextBox rtbNotes;
    }
}
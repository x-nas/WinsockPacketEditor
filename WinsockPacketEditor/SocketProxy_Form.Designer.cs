﻿namespace WinsockPacketEditor
{
    partial class SocketProxy_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SocketProxy_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsslServerInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpSocketProxy_Parameter = new System.Windows.Forms.TableLayoutPanel();
            this.tcSocketProxySet = new System.Windows.Forms.TabControl();
            this.tpProxySet = new System.Windows.Forms.TabPage();
            this.tlpProxySet = new System.Windows.Forms.TableLayoutPanel();
            this.gbProxyType = new System.Windows.Forms.GroupBox();
            this.tlpProxyType = new System.Windows.Forms.TableLayoutPanel();
            this.cbSOCKS5 = new System.Windows.Forms.CheckBox();
            this.gbProxySet_Port = new System.Windows.Forms.GroupBox();
            this.tlpProxySet_Port = new System.Windows.Forms.TableLayoutPanel();
            this.nudProxySet_Port = new System.Windows.Forms.NumericUpDown();
            this.gbProxySet_Auth = new System.Windows.Forms.GroupBox();
            this.tlpProxySet_Auth = new System.Windows.Forms.TableLayoutPanel();
            this.txtAuth_PassWord = new System.Windows.Forms.TextBox();
            this.lAuth_PassWord = new System.Windows.Forms.Label();
            this.cbProxySet_Auth = new System.Windows.Forms.CheckBox();
            this.lAuth_UserName = new System.Windows.Forms.Label();
            this.txtAuth_UserName = new System.Windows.Forms.TextBox();
            this.tpListSet = new System.Windows.Forms.TabPage();
            this.tlpListSet = new System.Windows.Forms.TableLayoutPanel();
            this.gbListSet_LogList = new System.Windows.Forms.GroupBox();
            this.tlpListSet_LogList = new System.Windows.Forms.TableLayoutPanel();
            this.nudLogList_AutoClearValue = new System.Windows.Forms.NumericUpDown();
            this.cbLogList_AutoClear = new System.Windows.Forms.CheckBox();
            this.cbLogList_AutoRoll = new System.Windows.Forms.CheckBox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bCleanUp = new System.Windows.Forms.Button();
            this.tlpButton_Start = new System.Windows.Forms.TableLayoutPanel();
            this.bStop = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            this.tvProxyData = new System.Windows.Forms.TreeView();
            this.ilSocketProxy = new System.Windows.Forms.ImageList(this.components);
            this.tpProxyList = new System.Windows.Forms.TabPage();
            this.tcSocketProxy = new System.Windows.Forms.TabControl();
            this.ssSocketProxy = new System.Windows.Forms.StatusStrip();
            this.tcSocketProxy_Data = new System.Windows.Forms.TabControl();
            this.tpData = new System.Windows.Forms.TabPage();
            this.hbData = new Be.Windows.Forms.HexBox();
            this.tlpSocketProxy_Data = new System.Windows.Forms.TableLayoutPanel();
            this.tcSocketProxy_Log = new System.Windows.Forms.TabControl();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.dgvLogList = new System.Windows.Forms.DataGridView();
            this.cLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFuncName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpClientList = new System.Windows.Forms.TabPage();
            this.tvClientList = new System.Windows.Forms.TreeView();
            this.tlpSocketProxy = new System.Windows.Forms.TableLayoutPanel();
            this.bgwLogList = new System.ComponentModel.BackgroundWorker();
            this.tSocketProxy = new System.Windows.Forms.Timer(this.components);
            this.bgwProxyList = new System.ComponentModel.BackgroundWorker();
            this.tlpSocketProxy_Parameter.SuspendLayout();
            this.tcSocketProxySet.SuspendLayout();
            this.tpProxySet.SuspendLayout();
            this.tlpProxySet.SuspendLayout();
            this.gbProxyType.SuspendLayout();
            this.tlpProxyType.SuspendLayout();
            this.gbProxySet_Port.SuspendLayout();
            this.tlpProxySet_Port.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxySet_Port)).BeginInit();
            this.gbProxySet_Auth.SuspendLayout();
            this.tlpProxySet_Auth.SuspendLayout();
            this.tpListSet.SuspendLayout();
            this.tlpListSet.SuspendLayout();
            this.gbListSet_LogList.SuspendLayout();
            this.tlpListSet_LogList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogList_AutoClearValue)).BeginInit();
            this.tlpButton.SuspendLayout();
            this.tlpButton_Start.SuspendLayout();
            this.tpProxyList.SuspendLayout();
            this.tcSocketProxy.SuspendLayout();
            this.ssSocketProxy.SuspendLayout();
            this.tcSocketProxy_Data.SuspendLayout();
            this.tpData.SuspendLayout();
            this.tlpSocketProxy_Data.SuspendLayout();
            this.tcSocketProxy_Log.SuspendLayout();
            this.tpLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpClientList.SuspendLayout();
            this.tlpSocketProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsslServerInfo
            // 
            resources.ApplyResources(this.tsslServerInfo, "tsslServerInfo");
            this.tsslServerInfo.Name = "tsslServerInfo";
            // 
            // tlpSocketProxy_Parameter
            // 
            resources.ApplyResources(this.tlpSocketProxy_Parameter, "tlpSocketProxy_Parameter");
            this.tlpSocketProxy_Parameter.Controls.Add(this.tcSocketProxySet, 0, 0);
            this.tlpSocketProxy_Parameter.Controls.Add(this.tlpButton, 1, 0);
            this.tlpSocketProxy_Parameter.Name = "tlpSocketProxy_Parameter";
            // 
            // tcSocketProxySet
            // 
            resources.ApplyResources(this.tcSocketProxySet, "tcSocketProxySet");
            this.tcSocketProxySet.Controls.Add(this.tpProxySet);
            this.tcSocketProxySet.Controls.Add(this.tpListSet);
            this.tcSocketProxySet.Name = "tcSocketProxySet";
            this.tcSocketProxySet.SelectedIndex = 0;
            // 
            // tpProxySet
            // 
            resources.ApplyResources(this.tpProxySet, "tpProxySet");
            this.tpProxySet.BackColor = System.Drawing.SystemColors.Control;
            this.tpProxySet.Controls.Add(this.tlpProxySet);
            this.tpProxySet.Name = "tpProxySet";
            // 
            // tlpProxySet
            // 
            resources.ApplyResources(this.tlpProxySet, "tlpProxySet");
            this.tlpProxySet.Controls.Add(this.gbProxyType, 0, 0);
            this.tlpProxySet.Controls.Add(this.gbProxySet_Port, 1, 0);
            this.tlpProxySet.Controls.Add(this.gbProxySet_Auth, 2, 0);
            this.tlpProxySet.Name = "tlpProxySet";
            // 
            // gbProxyType
            // 
            resources.ApplyResources(this.gbProxyType, "gbProxyType");
            this.gbProxyType.Controls.Add(this.tlpProxyType);
            this.gbProxyType.Name = "gbProxyType";
            this.gbProxyType.TabStop = false;
            // 
            // tlpProxyType
            // 
            resources.ApplyResources(this.tlpProxyType, "tlpProxyType");
            this.tlpProxyType.Controls.Add(this.cbSOCKS5, 0, 1);
            this.tlpProxyType.Name = "tlpProxyType";
            // 
            // cbSOCKS5
            // 
            resources.ApplyResources(this.cbSOCKS5, "cbSOCKS5");
            this.cbSOCKS5.Checked = true;
            this.cbSOCKS5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSOCKS5.Name = "cbSOCKS5";
            this.cbSOCKS5.UseVisualStyleBackColor = true;
            // 
            // gbProxySet_Port
            // 
            resources.ApplyResources(this.gbProxySet_Port, "gbProxySet_Port");
            this.gbProxySet_Port.Controls.Add(this.tlpProxySet_Port);
            this.gbProxySet_Port.Name = "gbProxySet_Port";
            this.gbProxySet_Port.TabStop = false;
            // 
            // tlpProxySet_Port
            // 
            resources.ApplyResources(this.tlpProxySet_Port, "tlpProxySet_Port");
            this.tlpProxySet_Port.Controls.Add(this.nudProxySet_Port, 0, 1);
            this.tlpProxySet_Port.Name = "tlpProxySet_Port";
            // 
            // nudProxySet_Port
            // 
            resources.ApplyResources(this.nudProxySet_Port, "nudProxySet_Port");
            this.nudProxySet_Port.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudProxySet_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudProxySet_Port.Name = "nudProxySet_Port";
            this.nudProxySet_Port.Value = new decimal(new int[] {
            8899,
            0,
            0,
            0});
            // 
            // gbProxySet_Auth
            // 
            resources.ApplyResources(this.gbProxySet_Auth, "gbProxySet_Auth");
            this.gbProxySet_Auth.Controls.Add(this.tlpProxySet_Auth);
            this.gbProxySet_Auth.Name = "gbProxySet_Auth";
            this.gbProxySet_Auth.TabStop = false;
            // 
            // tlpProxySet_Auth
            // 
            resources.ApplyResources(this.tlpProxySet_Auth, "tlpProxySet_Auth");
            this.tlpProxySet_Auth.Controls.Add(this.txtAuth_PassWord, 4, 1);
            this.tlpProxySet_Auth.Controls.Add(this.lAuth_PassWord, 3, 1);
            this.tlpProxySet_Auth.Controls.Add(this.cbProxySet_Auth, 0, 1);
            this.tlpProxySet_Auth.Controls.Add(this.lAuth_UserName, 1, 1);
            this.tlpProxySet_Auth.Controls.Add(this.txtAuth_UserName, 2, 1);
            this.tlpProxySet_Auth.Name = "tlpProxySet_Auth";
            // 
            // txtAuth_PassWord
            // 
            resources.ApplyResources(this.txtAuth_PassWord, "txtAuth_PassWord");
            this.txtAuth_PassWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuth_PassWord.Name = "txtAuth_PassWord";
            // 
            // lAuth_PassWord
            // 
            resources.ApplyResources(this.lAuth_PassWord, "lAuth_PassWord");
            this.lAuth_PassWord.Name = "lAuth_PassWord";
            // 
            // cbProxySet_Auth
            // 
            resources.ApplyResources(this.cbProxySet_Auth, "cbProxySet_Auth");
            this.cbProxySet_Auth.Name = "cbProxySet_Auth";
            this.cbProxySet_Auth.UseVisualStyleBackColor = true;
            this.cbProxySet_Auth.CheckedChanged += new System.EventHandler(this.cbProxySet_Auth_CheckedChanged);
            // 
            // lAuth_UserName
            // 
            resources.ApplyResources(this.lAuth_UserName, "lAuth_UserName");
            this.lAuth_UserName.Name = "lAuth_UserName";
            // 
            // txtAuth_UserName
            // 
            resources.ApplyResources(this.txtAuth_UserName, "txtAuth_UserName");
            this.txtAuth_UserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuth_UserName.Name = "txtAuth_UserName";
            // 
            // tpListSet
            // 
            resources.ApplyResources(this.tpListSet, "tpListSet");
            this.tpListSet.BackColor = System.Drawing.SystemColors.Control;
            this.tpListSet.Controls.Add(this.tlpListSet);
            this.tpListSet.Name = "tpListSet";
            // 
            // tlpListSet
            // 
            resources.ApplyResources(this.tlpListSet, "tlpListSet");
            this.tlpListSet.Controls.Add(this.gbListSet_LogList, 0, 0);
            this.tlpListSet.Name = "tlpListSet";
            // 
            // gbListSet_LogList
            // 
            resources.ApplyResources(this.gbListSet_LogList, "gbListSet_LogList");
            this.gbListSet_LogList.Controls.Add(this.tlpListSet_LogList);
            this.gbListSet_LogList.Name = "gbListSet_LogList";
            this.gbListSet_LogList.TabStop = false;
            // 
            // tlpListSet_LogList
            // 
            resources.ApplyResources(this.tlpListSet_LogList, "tlpListSet_LogList");
            this.tlpListSet_LogList.Controls.Add(this.nudLogList_AutoClearValue, 2, 0);
            this.tlpListSet_LogList.Controls.Add(this.cbLogList_AutoClear, 1, 0);
            this.tlpListSet_LogList.Controls.Add(this.cbLogList_AutoRoll, 0, 0);
            this.tlpListSet_LogList.Name = "tlpListSet_LogList";
            // 
            // nudLogList_AutoClearValue
            // 
            resources.ApplyResources(this.nudLogList_AutoClearValue, "nudLogList_AutoClearValue");
            this.nudLogList_AutoClearValue.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudLogList_AutoClearValue.Name = "nudLogList_AutoClearValue";
            this.nudLogList_AutoClearValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // cbLogList_AutoClear
            // 
            resources.ApplyResources(this.cbLogList_AutoClear, "cbLogList_AutoClear");
            this.cbLogList_AutoClear.Name = "cbLogList_AutoClear";
            this.cbLogList_AutoClear.UseVisualStyleBackColor = true;
            this.cbLogList_AutoClear.CheckedChanged += new System.EventHandler(this.cbLogList_AutoClear_CheckedChanged);
            // 
            // cbLogList_AutoRoll
            // 
            resources.ApplyResources(this.cbLogList_AutoRoll, "cbLogList_AutoRoll");
            this.cbLogList_AutoRoll.Checked = true;
            this.cbLogList_AutoRoll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLogList_AutoRoll.Name = "cbLogList_AutoRoll";
            this.cbLogList_AutoRoll.UseVisualStyleBackColor = true;
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bCleanUp, 1, 1);
            this.tlpButton.Controls.Add(this.tlpButton_Start, 2, 1);
            this.tlpButton.Name = "tlpButton";
            // 
            // bCleanUp
            // 
            resources.ApplyResources(this.bCleanUp, "bCleanUp");
            this.bCleanUp.Name = "bCleanUp";
            this.bCleanUp.UseVisualStyleBackColor = true;
            this.bCleanUp.Click += new System.EventHandler(this.bCleanUp_Click);
            // 
            // tlpButton_Start
            // 
            resources.ApplyResources(this.tlpButton_Start, "tlpButton_Start");
            this.tlpButton_Start.Controls.Add(this.bStop, 1, 2);
            this.tlpButton_Start.Controls.Add(this.bStart, 1, 0);
            this.tlpButton_Start.Name = "tlpButton_Start";
            // 
            // bStop
            // 
            resources.ApplyResources(this.bStop, "bStop");
            this.bStop.Image = global::WinsockPacketEditor.Properties.Resources.Stop16;
            this.bStop.Name = "bStop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bStart
            // 
            resources.ApplyResources(this.bStart, "bStart");
            this.bStart.Image = global::WinsockPacketEditor.Properties.Resources.Play16;
            this.bStart.Name = "bStart";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // tvProxyData
            // 
            resources.ApplyResources(this.tvProxyData, "tvProxyData");
            this.tvProxyData.ImageList = this.ilSocketProxy;
            this.tvProxyData.Name = "tvProxyData";
            this.tvProxyData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSocketProxy_AfterSelect);
            // 
            // ilSocketProxy
            // 
            this.ilSocketProxy.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSocketProxy.ImageStream")));
            this.ilSocketProxy.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSocketProxy.Images.SetKeyName(0, "Socket.png");
            this.ilSocketProxy.Images.SetKeyName(1, "unknown.png");
            this.ilSocketProxy.Images.SetKeyName(2, "Request.png");
            this.ilSocketProxy.Images.SetKeyName(3, "Response.png");
            this.ilSocketProxy.Images.SetKeyName(4, "computer.png");
            this.ilSocketProxy.Images.SetKeyName(5, "pass.png");
            this.ilSocketProxy.Images.SetKeyName(6, "fail.png");
            this.ilSocketProxy.Images.SetKeyName(7, "http.png");
            this.ilSocketProxy.Images.SetKeyName(8, "js.png");
            // 
            // tpProxyList
            // 
            resources.ApplyResources(this.tpProxyList, "tpProxyList");
            this.tpProxyList.Controls.Add(this.tvProxyData);
            this.tpProxyList.Name = "tpProxyList";
            this.tpProxyList.UseVisualStyleBackColor = true;
            // 
            // tcSocketProxy
            // 
            resources.ApplyResources(this.tcSocketProxy, "tcSocketProxy");
            this.tcSocketProxy.Controls.Add(this.tpProxyList);
            this.tcSocketProxy.Name = "tcSocketProxy";
            this.tcSocketProxy.SelectedIndex = 0;
            // 
            // ssSocketProxy
            // 
            resources.ApplyResources(this.ssSocketProxy, "ssSocketProxy");
            this.ssSocketProxy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslServerInfo});
            this.ssSocketProxy.Name = "ssSocketProxy";
            // 
            // tcSocketProxy_Data
            // 
            resources.ApplyResources(this.tcSocketProxy_Data, "tcSocketProxy_Data");
            this.tcSocketProxy_Data.Controls.Add(this.tpData);
            this.tcSocketProxy_Data.Name = "tcSocketProxy_Data";
            this.tcSocketProxy_Data.SelectedIndex = 0;
            // 
            // tpData
            // 
            resources.ApplyResources(this.tpData, "tpData");
            this.tpData.Controls.Add(this.hbData);
            this.tpData.Name = "tpData";
            this.tpData.UseVisualStyleBackColor = true;
            // 
            // hbData
            // 
            resources.ApplyResources(this.hbData, "hbData");
            this.hbData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // 
            // 
            this.hbData.BuiltInContextMenu.CopyMenuItemImage = global::WinsockPacketEditor.Properties.Resources.copy;
            this.hbData.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbData.BuiltInContextMenu.CopyMenuItemText");
            this.hbData.BuiltInContextMenu.CutMenuItemImage = global::WinsockPacketEditor.Properties.Resources.cut;
            this.hbData.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbData.BuiltInContextMenu.CutMenuItemText");
            this.hbData.BuiltInContextMenu.PasteMenuItemImage = global::WinsockPacketEditor.Properties.Resources.paste;
            this.hbData.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbData.BuiltInContextMenu.PasteMenuItemText");
            this.hbData.BuiltInContextMenu.SelectAllMenuItemImage = global::WinsockPacketEditor.Properties.Resources.SelectAll;
            this.hbData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbData.ColumnInfoVisible = true;
            this.hbData.LineInfoVisible = true;
            this.hbData.Name = "hbData";
            this.hbData.ReadOnly = true;
            this.hbData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbData.StringViewVisible = true;
            this.hbData.VScrollBarVisible = true;
            // 
            // tlpSocketProxy_Data
            // 
            resources.ApplyResources(this.tlpSocketProxy_Data, "tlpSocketProxy_Data");
            this.tlpSocketProxy_Data.Controls.Add(this.tcSocketProxy_Log, 1, 1);
            this.tlpSocketProxy_Data.Controls.Add(this.tcSocketProxy_Data, 1, 0);
            this.tlpSocketProxy_Data.Controls.Add(this.tcSocketProxy, 0, 0);
            this.tlpSocketProxy_Data.Controls.Add(this.tabControl1, 0, 1);
            this.tlpSocketProxy_Data.Name = "tlpSocketProxy_Data";
            // 
            // tcSocketProxy_Log
            // 
            resources.ApplyResources(this.tcSocketProxy_Log, "tcSocketProxy_Log");
            this.tcSocketProxy_Log.Controls.Add(this.tpLog);
            this.tcSocketProxy_Log.Name = "tcSocketProxy_Log";
            this.tcSocketProxy_Log.SelectedIndex = 0;
            // 
            // tpLog
            // 
            resources.ApplyResources(this.tpLog, "tpLog");
            this.tpLog.Controls.Add(this.dgvLogList);
            this.tpLog.Name = "tpLog";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // dgvLogList
            // 
            resources.ApplyResources(this.dgvLogList, "dgvLogList");
            this.dgvLogList.AllowUserToAddRows = false;
            this.dgvLogList.AllowUserToDeleteRows = false;
            this.dgvLogList.AllowUserToResizeColumns = false;
            this.dgvLogList.AllowUserToResizeRows = false;
            this.dgvLogList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvLogList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cLogID,
            this.cLogTime,
            this.cFuncName,
            this.cLogContent});
            this.dgvLogList.MultiSelect = false;
            this.dgvLogList.Name = "dgvLogList";
            this.dgvLogList.ReadOnly = true;
            this.dgvLogList.RowHeadersVisible = false;
            this.dgvLogList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLogList.RowTemplate.Height = 23;
            this.dgvLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLogList_CellFormatting);
            // 
            // cLogID
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogID.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.cLogID, "cLogID");
            this.cLogID.Name = "cLogID";
            this.cLogID.ReadOnly = true;
            this.cLogID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLogTime
            // 
            this.cLogTime.DataPropertyName = "LogTime";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogTime.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.cLogTime, "cLogTime");
            this.cLogTime.Name = "cLogTime";
            this.cLogTime.ReadOnly = true;
            this.cLogTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFuncName
            // 
            this.cFuncName.DataPropertyName = "FuncName";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cFuncName.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.cFuncName, "cFuncName");
            this.cFuncName.Name = "cFuncName";
            this.cFuncName.ReadOnly = true;
            this.cFuncName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cFuncName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLogContent
            // 
            this.cLogContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLogContent.DataPropertyName = "LogContent";
            resources.ApplyResources(this.cLogContent, "cLogContent");
            this.cLogContent.Name = "cLogContent";
            this.cLogContent.ReadOnly = true;
            this.cLogContent.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tpClientList);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tpClientList
            // 
            resources.ApplyResources(this.tpClientList, "tpClientList");
            this.tpClientList.BackColor = System.Drawing.SystemColors.Control;
            this.tpClientList.Controls.Add(this.tvClientList);
            this.tpClientList.Name = "tpClientList";
            // 
            // tvClientList
            // 
            resources.ApplyResources(this.tvClientList, "tvClientList");
            this.tvClientList.ImageList = this.ilSocketProxy;
            this.tvClientList.Name = "tvClientList";
            // 
            // tlpSocketProxy
            // 
            resources.ApplyResources(this.tlpSocketProxy, "tlpSocketProxy");
            this.tlpSocketProxy.Controls.Add(this.tlpSocketProxy_Data, 0, 1);
            this.tlpSocketProxy.Controls.Add(this.tlpSocketProxy_Parameter, 0, 0);
            this.tlpSocketProxy.Controls.Add(this.ssSocketProxy, 0, 2);
            this.tlpSocketProxy.Name = "tlpSocketProxy";
            // 
            // bgwLogList
            // 
            this.bgwLogList.WorkerSupportsCancellation = true;
            this.bgwLogList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwLogList_DoWork);
            this.bgwLogList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwLogList_RunWorkerCompleted);
            // 
            // tSocketProxy
            // 
            this.tSocketProxy.Enabled = true;
            this.tSocketProxy.Interval = 10;
            this.tSocketProxy.Tick += new System.EventHandler(this.tSocketProxy_Tick);
            // 
            // bgwProxyList
            // 
            this.bgwProxyList.WorkerSupportsCancellation = true;
            this.bgwProxyList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwProxyList_DoWork);
            // 
            // SocketProxy_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSocketProxy);
            this.DoubleBuffered = true;
            this.Name = "SocketProxy_Form";
            this.tlpSocketProxy_Parameter.ResumeLayout(false);
            this.tcSocketProxySet.ResumeLayout(false);
            this.tpProxySet.ResumeLayout(false);
            this.tlpProxySet.ResumeLayout(false);
            this.gbProxyType.ResumeLayout(false);
            this.tlpProxyType.ResumeLayout(false);
            this.tlpProxyType.PerformLayout();
            this.gbProxySet_Port.ResumeLayout(false);
            this.tlpProxySet_Port.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudProxySet_Port)).EndInit();
            this.gbProxySet_Auth.ResumeLayout(false);
            this.tlpProxySet_Auth.ResumeLayout(false);
            this.tlpProxySet_Auth.PerformLayout();
            this.tpListSet.ResumeLayout(false);
            this.tlpListSet.ResumeLayout(false);
            this.gbListSet_LogList.ResumeLayout(false);
            this.tlpListSet_LogList.ResumeLayout(false);
            this.tlpListSet_LogList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogList_AutoClearValue)).EndInit();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton_Start.ResumeLayout(false);
            this.tpProxyList.ResumeLayout(false);
            this.tcSocketProxy.ResumeLayout(false);
            this.ssSocketProxy.ResumeLayout(false);
            this.ssSocketProxy.PerformLayout();
            this.tcSocketProxy_Data.ResumeLayout(false);
            this.tpData.ResumeLayout(false);
            this.tlpSocketProxy_Data.ResumeLayout(false);
            this.tcSocketProxy_Log.ResumeLayout(false);
            this.tpLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpClientList.ResumeLayout(false);
            this.tlpSocketProxy.ResumeLayout(false);
            this.tlpSocketProxy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tsslServerInfo;
        private System.Windows.Forms.TableLayoutPanel tlpSocketProxy_Parameter;
        private System.Windows.Forms.TreeView tvProxyData;
        private System.Windows.Forms.ImageList ilSocketProxy;
        private System.Windows.Forms.TabPage tpProxyList;
        private System.Windows.Forms.TabControl tcSocketProxy;
        private System.Windows.Forms.StatusStrip ssSocketProxy;
        private System.Windows.Forms.TabControl tcSocketProxy_Data;
        private System.Windows.Forms.TabPage tpData;
        private System.Windows.Forms.TableLayoutPanel tlpSocketProxy_Data;
        private System.Windows.Forms.TableLayoutPanel tlpSocketProxy;
        private System.ComponentModel.BackgroundWorker bgwLogList;
        private System.Windows.Forms.Timer tSocketProxy;
        private Be.Windows.Forms.HexBox hbData;
        private System.Windows.Forms.TabControl tcSocketProxySet;
        private System.Windows.Forms.TabPage tpProxySet;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bCleanUp;
        private System.Windows.Forms.TableLayoutPanel tlpButton_Start;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.TabPage tpListSet;
        private System.Windows.Forms.TableLayoutPanel tlpListSet;
        private System.Windows.Forms.GroupBox gbListSet_LogList;
        private System.Windows.Forms.TableLayoutPanel tlpListSet_LogList;
        private System.Windows.Forms.NumericUpDown nudLogList_AutoClearValue;
        private System.Windows.Forms.CheckBox cbLogList_AutoClear;
        private System.Windows.Forms.CheckBox cbLogList_AutoRoll;
        private System.Windows.Forms.TableLayoutPanel tlpProxySet;
        private System.Windows.Forms.GroupBox gbProxySet_Port;
        private System.Windows.Forms.GroupBox gbProxySet_Auth;
        private System.Windows.Forms.TableLayoutPanel tlpProxySet_Port;
        private System.Windows.Forms.NumericUpDown nudProxySet_Port;
        private System.Windows.Forms.TableLayoutPanel tlpProxySet_Auth;
        private System.Windows.Forms.CheckBox cbProxySet_Auth;
        private System.Windows.Forms.TextBox txtAuth_PassWord;
        private System.Windows.Forms.Label lAuth_PassWord;
        private System.Windows.Forms.Label lAuth_UserName;
        private System.Windows.Forms.TextBox txtAuth_UserName;
        private System.Windows.Forms.TabControl tcSocketProxy_Log;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.DataGridView dgvLogList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpClientList;
        private System.Windows.Forms.TreeView tvClientList;
        private System.ComponentModel.BackgroundWorker bgwProxyList;
        private System.Windows.Forms.GroupBox gbProxyType;
        private System.Windows.Forms.TableLayoutPanel tlpProxyType;
        private System.Windows.Forms.CheckBox cbSOCKS5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFuncName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogContent;
    }
}
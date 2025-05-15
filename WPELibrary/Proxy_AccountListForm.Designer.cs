namespace WPELibrary
{
    partial class Proxy_AccountListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountListForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpProxyAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.ssProxyAccount = new System.Windows.Forms.StatusStrip();
            this.tsslAccount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccount_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountEnable = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountEnable_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountOnLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountOnLine_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountExpiry = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountExpiry_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAccountList = new System.Windows.Forms.DataGridView();
            this.cAccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsOnLine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsExpiry = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cExpiryTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpAccountButton = new System.Windows.Forms.TableLayoutPanel();
            this.bDelete = new System.Windows.Forms.Button();
            this.bAccount_New = new System.Windows.Forms.Button();
            this.bExport = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.tlpSearchAccount = new System.Windows.Forms.TableLayoutPanel();
            this.gbSearch_UserName = new System.Windows.Forms.GroupBox();
            this.tlpSearch_UserName = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch_UserName = new System.Windows.Forms.TextBox();
            this.bSearch_UserName = new System.Windows.Forms.Button();
            this.gbSearch_State = new System.Windows.Forms.GroupBox();
            this.tlpSearch_State = new System.Windows.Forms.TableLayoutPanel();
            this.cbbSearch_State = new System.Windows.Forms.ComboBox();
            this.cbIs = new System.Windows.Forms.CheckBox();
            this.tTimer = new System.Windows.Forms.Timer(this.components);
            this.tlpProxyAccountList.SuspendLayout();
            this.ssProxyAccount.SuspendLayout();
            this.tlpAccountList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).BeginInit();
            this.tlpAccountButton.SuspendLayout();
            this.tlpSearchAccount.SuspendLayout();
            this.gbSearch_UserName.SuspendLayout();
            this.tlpSearch_UserName.SuspendLayout();
            this.gbSearch_State.SuspendLayout();
            this.tlpSearch_State.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyAccountList
            // 
            resources.ApplyResources(this.tlpProxyAccountList, "tlpProxyAccountList");
            this.tlpProxyAccountList.Controls.Add(this.ssProxyAccount, 0, 2);
            this.tlpProxyAccountList.Controls.Add(this.tlpAccountList, 0, 1);
            this.tlpProxyAccountList.Controls.Add(this.tlpSearchAccount, 0, 0);
            this.tlpProxyAccountList.Name = "tlpProxyAccountList";
            // 
            // ssProxyAccount
            // 
            resources.ApplyResources(this.ssProxyAccount, "ssProxyAccount");
            this.ssProxyAccount.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslAccount,
            this.tsslAccount_CNT,
            this.tsslSplit,
            this.tsslAccountEnable,
            this.tsslAccountEnable_CNT,
            this.tsslSplit1,
            this.tsslAccountOnLine,
            this.tsslAccountOnLine_CNT,
            this.tsslSplit2,
            this.tsslAccountExpiry,
            this.tsslAccountExpiry_CNT});
            this.ssProxyAccount.Name = "ssProxyAccount";
            // 
            // tsslAccount
            // 
            this.tsslAccount.Name = "tsslAccount";
            resources.ApplyResources(this.tsslAccount, "tsslAccount");
            // 
            // tsslAccount_CNT
            // 
            resources.ApplyResources(this.tsslAccount_CNT, "tsslAccount_CNT");
            this.tsslAccount_CNT.Name = "tsslAccount_CNT";
            // 
            // tsslSplit
            // 
            resources.ApplyResources(this.tsslSplit, "tsslSplit");
            this.tsslSplit.Name = "tsslSplit";
            // 
            // tsslAccountEnable
            // 
            this.tsslAccountEnable.Name = "tsslAccountEnable";
            resources.ApplyResources(this.tsslAccountEnable, "tsslAccountEnable");
            // 
            // tsslAccountEnable_CNT
            // 
            resources.ApplyResources(this.tsslAccountEnable_CNT, "tsslAccountEnable_CNT");
            this.tsslAccountEnable_CNT.Name = "tsslAccountEnable_CNT";
            // 
            // tsslSplit1
            // 
            resources.ApplyResources(this.tsslSplit1, "tsslSplit1");
            this.tsslSplit1.Name = "tsslSplit1";
            // 
            // tsslAccountOnLine
            // 
            this.tsslAccountOnLine.Name = "tsslAccountOnLine";
            resources.ApplyResources(this.tsslAccountOnLine, "tsslAccountOnLine");
            // 
            // tsslAccountOnLine_CNT
            // 
            resources.ApplyResources(this.tsslAccountOnLine_CNT, "tsslAccountOnLine_CNT");
            this.tsslAccountOnLine_CNT.Name = "tsslAccountOnLine_CNT";
            // 
            // tsslSplit2
            // 
            resources.ApplyResources(this.tsslSplit2, "tsslSplit2");
            this.tsslSplit2.Name = "tsslSplit2";
            // 
            // tsslAccountExpiry
            // 
            this.tsslAccountExpiry.Name = "tsslAccountExpiry";
            resources.ApplyResources(this.tsslAccountExpiry, "tsslAccountExpiry");
            // 
            // tsslAccountExpiry_CNT
            // 
            resources.ApplyResources(this.tsslAccountExpiry_CNT, "tsslAccountExpiry_CNT");
            this.tsslAccountExpiry_CNT.Name = "tsslAccountExpiry_CNT";
            // 
            // tlpAccountList
            // 
            resources.ApplyResources(this.tlpAccountList, "tlpAccountList");
            this.tlpAccountList.Controls.Add(this.dgvAccountList, 0, 0);
            this.tlpAccountList.Controls.Add(this.tlpAccountButton, 1, 0);
            this.tlpAccountList.Name = "tlpAccountList";
            // 
            // dgvAccountList
            // 
            this.dgvAccountList.AllowUserToAddRows = false;
            this.dgvAccountList.AllowUserToDeleteRows = false;
            this.dgvAccountList.AllowUserToResizeColumns = false;
            this.dgvAccountList.AllowUserToResizeRows = false;
            this.dgvAccountList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAccountList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAccountList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAccountList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cAccountID,
            this.cAID,
            this.cIsEnable,
            this.cUserName,
            this.cLoginIP,
            this.cIsOnLine,
            this.cCreateTime,
            this.cIsExpiry,
            this.cExpiryTime});
            resources.ApplyResources(this.dgvAccountList, "dgvAccountList");
            this.dgvAccountList.Name = "dgvAccountList";
            this.dgvAccountList.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvAccountList.RowHeadersVisible = false;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvAccountList.RowTemplate.Height = 23;
            this.dgvAccountList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccountList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountList_CellDoubleClick);
            this.dgvAccountList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAccountList_CellFormatting);
            // 
            // cAccountID
            // 
            this.cAccountID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccountID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cAccountID, "cAccountID");
            this.cAccountID.Name = "cAccountID";
            this.cAccountID.ReadOnly = true;
            this.cAccountID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccountID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAID
            // 
            this.cAID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cAID.DataPropertyName = "AID";
            resources.ApplyResources(this.cAID, "cAID");
            this.cAID.Name = "cAID";
            this.cAID.ReadOnly = true;
            this.cAID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIsEnable
            // 
            this.cIsEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsEnable.DataPropertyName = "IsEnable";
            resources.ApplyResources(this.cIsEnable, "cIsEnable");
            this.cIsEnable.Name = "cIsEnable";
            this.cIsEnable.ReadOnly = true;
            this.cIsEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cUserName
            // 
            this.cUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cUserName.DataPropertyName = "UserName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cUserName, "cUserName");
            this.cUserName.Name = "cUserName";
            this.cUserName.ReadOnly = true;
            this.cUserName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLoginIP
            // 
            this.cLoginIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLoginIP.DataPropertyName = "LoginIP";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cLoginIP, "cLoginIP");
            this.cLoginIP.Name = "cLoginIP";
            this.cLoginIP.ReadOnly = true;
            this.cLoginIP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIsOnLine
            // 
            this.cIsOnLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsOnLine.DataPropertyName = "IsOnLine";
            resources.ApplyResources(this.cIsOnLine, "cIsOnLine");
            this.cIsOnLine.Name = "cIsOnLine";
            this.cIsOnLine.ReadOnly = true;
            this.cIsOnLine.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cCreateTime
            // 
            this.cCreateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cCreateTime.DataPropertyName = "CreateTime";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cCreateTime.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.cCreateTime, "cCreateTime");
            this.cCreateTime.Name = "cCreateTime";
            this.cCreateTime.ReadOnly = true;
            this.cCreateTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCreateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIsExpiry
            // 
            this.cIsExpiry.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsExpiry.DataPropertyName = "IsExpiry";
            resources.ApplyResources(this.cIsExpiry, "cIsExpiry");
            this.cIsExpiry.Name = "cIsExpiry";
            this.cIsExpiry.ReadOnly = true;
            this.cIsExpiry.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cExpiryTime
            // 
            this.cExpiryTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cExpiryTime.DataPropertyName = "ExpiryTime";
            resources.ApplyResources(this.cExpiryTime, "cExpiryTime");
            this.cExpiryTime.Name = "cExpiryTime";
            this.cExpiryTime.ReadOnly = true;
            this.cExpiryTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cExpiryTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tlpAccountButton
            // 
            resources.ApplyResources(this.tlpAccountButton, "tlpAccountButton");
            this.tlpAccountButton.Controls.Add(this.bDelete, 1, 3);
            this.tlpAccountButton.Controls.Add(this.bAccount_New, 1, 1);
            this.tlpAccountButton.Controls.Add(this.bExport, 1, 5);
            this.tlpAccountButton.Controls.Add(this.bImport, 1, 7);
            this.tlpAccountButton.Name = "tlpAccountButton";
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bAccount_New
            // 
            resources.ApplyResources(this.bAccount_New, "bAccount_New");
            this.bAccount_New.Name = "bAccount_New";
            this.bAccount_New.UseVisualStyleBackColor = true;
            this.bAccount_New.Click += new System.EventHandler(this.bAccount_New_Click);
            // 
            // bExport
            // 
            resources.ApplyResources(this.bExport, "bExport");
            this.bExport.Name = "bExport";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bImport
            // 
            resources.ApplyResources(this.bImport, "bImport");
            this.bImport.Name = "bImport";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // tlpSearchAccount
            // 
            resources.ApplyResources(this.tlpSearchAccount, "tlpSearchAccount");
            this.tlpSearchAccount.Controls.Add(this.gbSearch_UserName, 0, 0);
            this.tlpSearchAccount.Controls.Add(this.gbSearch_State, 1, 0);
            this.tlpSearchAccount.Name = "tlpSearchAccount";
            // 
            // gbSearch_UserName
            // 
            this.gbSearch_UserName.Controls.Add(this.tlpSearch_UserName);
            resources.ApplyResources(this.gbSearch_UserName, "gbSearch_UserName");
            this.gbSearch_UserName.Name = "gbSearch_UserName";
            this.gbSearch_UserName.TabStop = false;
            // 
            // tlpSearch_UserName
            // 
            resources.ApplyResources(this.tlpSearch_UserName, "tlpSearch_UserName");
            this.tlpSearch_UserName.Controls.Add(this.txtSearch_UserName, 1, 1);
            this.tlpSearch_UserName.Controls.Add(this.bSearch_UserName, 3, 1);
            this.tlpSearch_UserName.Name = "tlpSearch_UserName";
            // 
            // txtSearch_UserName
            // 
            resources.ApplyResources(this.txtSearch_UserName, "txtSearch_UserName");
            this.txtSearch_UserName.Name = "txtSearch_UserName";
            // 
            // bSearch_UserName
            // 
            resources.ApplyResources(this.bSearch_UserName, "bSearch_UserName");
            this.bSearch_UserName.Name = "bSearch_UserName";
            this.bSearch_UserName.UseVisualStyleBackColor = true;
            this.bSearch_UserName.Click += new System.EventHandler(this.bSearch_UserName_Click);
            // 
            // gbSearch_State
            // 
            this.gbSearch_State.Controls.Add(this.tlpSearch_State);
            resources.ApplyResources(this.gbSearch_State, "gbSearch_State");
            this.gbSearch_State.Name = "gbSearch_State";
            this.gbSearch_State.TabStop = false;
            // 
            // tlpSearch_State
            // 
            resources.ApplyResources(this.tlpSearch_State, "tlpSearch_State");
            this.tlpSearch_State.Controls.Add(this.cbbSearch_State, 2, 1);
            this.tlpSearch_State.Controls.Add(this.cbIs, 1, 1);
            this.tlpSearch_State.Name = "tlpSearch_State";
            // 
            // cbbSearch_State
            // 
            resources.ApplyResources(this.cbbSearch_State, "cbbSearch_State");
            this.cbbSearch_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSearch_State.FormattingEnabled = true;
            this.cbbSearch_State.Items.AddRange(new object[] {
            resources.GetString("cbbSearch_State.Items"),
            resources.GetString("cbbSearch_State.Items1"),
            resources.GetString("cbbSearch_State.Items2")});
            this.cbbSearch_State.Name = "cbbSearch_State";
            this.cbbSearch_State.SelectedIndexChanged += new System.EventHandler(this.cbbSearch_State_SelectedIndexChanged);
            // 
            // cbIs
            // 
            resources.ApplyResources(this.cbIs, "cbIs");
            this.cbIs.Name = "cbIs";
            this.cbIs.UseVisualStyleBackColor = true;
            this.cbIs.CheckedChanged += new System.EventHandler(this.cbIs_CheckedChanged);
            // 
            // tTimer
            // 
            this.tTimer.Enabled = true;
            this.tTimer.Interval = 1000;
            this.tTimer.Tick += new System.EventHandler(this.tTimer_Tick);
            // 
            // Proxy_AccountListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpProxyAccountList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Proxy_AccountListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Proxy_AccountListForm_FormClosed);
            this.Load += new System.EventHandler(this.Proxy_AccountListForm_Load);
            this.tlpProxyAccountList.ResumeLayout(false);
            this.tlpProxyAccountList.PerformLayout();
            this.ssProxyAccount.ResumeLayout(false);
            this.ssProxyAccount.PerformLayout();
            this.tlpAccountList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).EndInit();
            this.tlpAccountButton.ResumeLayout(false);
            this.tlpSearchAccount.ResumeLayout(false);
            this.gbSearch_UserName.ResumeLayout(false);
            this.tlpSearch_UserName.ResumeLayout(false);
            this.tlpSearch_UserName.PerformLayout();
            this.gbSearch_State.ResumeLayout(false);
            this.tlpSearch_State.ResumeLayout(false);
            this.tlpSearch_State.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProxyAccountList;
        private System.Windows.Forms.StatusStrip ssProxyAccount;
        private System.Windows.Forms.TableLayoutPanel tlpAccountList;
        private System.Windows.Forms.DataGridView dgvAccountList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountButton;
        private System.Windows.Forms.Button bAccount_New;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccount;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccount_CNT;
        private System.Windows.Forms.Timer tTimer;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountEnable;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountEnable_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit2;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountExpiry;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountExpiry_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit1;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountOnLine;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountOnLine_CNT;
        private System.Windows.Forms.TableLayoutPanel tlpSearchAccount;
        private System.Windows.Forms.GroupBox gbSearch_State;
        private System.Windows.Forms.GroupBox gbSearch_UserName;
        private System.Windows.Forms.TableLayoutPanel tlpSearch_UserName;
        private System.Windows.Forms.TextBox txtSearch_UserName;
        private System.Windows.Forms.Button bSearch_UserName;
        private System.Windows.Forms.TableLayoutPanel tlpSearch_State;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.ComboBox cbbSearch_State;
        private System.Windows.Forms.CheckBox cbIs;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAccountID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginIP;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsOnLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCreateTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn cExpiryTime;
    }
}
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.tlpAccountButton = new System.Windows.Forms.TableLayoutPanel();
            this.bDelete = new System.Windows.Forms.Button();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bAccount_New = new System.Windows.Forms.Button();
            this.tTimer = new System.Windows.Forms.Timer(this.components);
            this.cAccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsOnLine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsExpiry = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cExpiryTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpProxyAccountList.SuspendLayout();
            this.ssProxyAccount.SuspendLayout();
            this.tlpAccountList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).BeginInit();
            this.tlpAccountButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyAccountList
            // 
            resources.ApplyResources(this.tlpProxyAccountList, "tlpProxyAccountList");
            this.tlpProxyAccountList.Controls.Add(this.ssProxyAccount, 0, 1);
            this.tlpProxyAccountList.Controls.Add(this.tlpAccountList, 0, 0);
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
            resources.ApplyResources(this.tsslAccount, "tsslAccount");
            this.tsslAccount.Name = "tsslAccount";
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
            resources.ApplyResources(this.tsslAccountEnable, "tsslAccountEnable");
            this.tsslAccountEnable.Name = "tsslAccountEnable";
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
            resources.ApplyResources(this.tsslAccountOnLine, "tsslAccountOnLine");
            this.tsslAccountOnLine.Name = "tsslAccountOnLine";
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
            resources.ApplyResources(this.tsslAccountExpiry, "tsslAccountExpiry");
            this.tsslAccountExpiry.Name = "tsslAccountExpiry";
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
            resources.ApplyResources(this.dgvAccountList, "dgvAccountList");
            this.dgvAccountList.AllowUserToAddRows = false;
            this.dgvAccountList.AllowUserToDeleteRows = false;
            this.dgvAccountList.AllowUserToResizeColumns = false;
            this.dgvAccountList.AllowUserToResizeRows = false;
            this.dgvAccountList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAccountList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAccountList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvAccountList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cAccountID,
            this.cIsEnable,
            this.cUserName,
            this.cLoginIP,
            this.cIsOnLine,
            this.cCreateTime,
            this.cIsExpiry,
            this.cExpiryTime});
            this.dgvAccountList.Name = "dgvAccountList";
            this.dgvAccountList.ReadOnly = true;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvAccountList.RowHeadersVisible = false;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.RowsDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvAccountList.RowTemplate.Height = 23;
            this.dgvAccountList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccountList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAccountList_CellFormatting);
            // 
            // tlpAccountButton
            // 
            resources.ApplyResources(this.tlpAccountButton, "tlpAccountButton");
            this.tlpAccountButton.Controls.Add(this.bDelete, 1, 5);
            this.tlpAccountButton.Controls.Add(this.bUpdate, 1, 3);
            this.tlpAccountButton.Controls.Add(this.bAccount_New, 1, 1);
            this.tlpAccountButton.Name = "tlpAccountButton";
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bUpdate
            // 
            resources.ApplyResources(this.bUpdate, "bUpdate");
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.UseVisualStyleBackColor = true;
            this.bUpdate.Click += new System.EventHandler(this.bUpdate_Click);
            // 
            // bAccount_New
            // 
            resources.ApplyResources(this.bAccount_New, "bAccount_New");
            this.bAccount_New.Name = "bAccount_New";
            this.bAccount_New.UseVisualStyleBackColor = true;
            this.bAccount_New.Click += new System.EventHandler(this.bAccount_New_Click);
            // 
            // tTimer
            // 
            this.tTimer.Enabled = true;
            this.tTimer.Interval = 1000;
            this.tTimer.Tick += new System.EventHandler(this.tTimer_Tick);
            // 
            // cAccountID
            // 
            this.cAccountID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccountID.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.cAccountID, "cAccountID");
            this.cAccountID.Name = "cAccountID";
            this.cAccountID.ReadOnly = true;
            this.cAccountID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccountID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.DefaultCellStyle = dataGridViewCellStyle10;
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
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.DefaultCellStyle = dataGridViewCellStyle11;
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
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cCreateTime.DefaultCellStyle = dataGridViewCellStyle12;
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
            // Proxy_AccountListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpProxyAccountList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Proxy_AccountListForm";
            this.tlpProxyAccountList.ResumeLayout(false);
            this.tlpProxyAccountList.PerformLayout();
            this.ssProxyAccount.ResumeLayout(false);
            this.ssProxyAccount.PerformLayout();
            this.tlpAccountList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).EndInit();
            this.tlpAccountButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProxyAccountList;
        private System.Windows.Forms.StatusStrip ssProxyAccount;
        private System.Windows.Forms.TableLayoutPanel tlpAccountList;
        private System.Windows.Forms.DataGridView dgvAccountList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountButton;
        private System.Windows.Forms.Button bAccount_New;
        private System.Windows.Forms.Button bUpdate;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn cAccountID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginIP;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsOnLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCreateTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn cExpiryTime;
    }
}
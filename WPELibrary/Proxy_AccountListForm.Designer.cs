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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountListForm));
            this.tlpProxyAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.ssProxyAccount = new System.Windows.Forms.StatusStrip();
            this.tlpAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAccountList = new System.Windows.Forms.DataGridView();
            this.cAccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIsExpiry = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cExpiryTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpAccountButton = new System.Windows.Forms.TableLayoutPanel();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bAccount_New = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.tsslAccount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccount_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tTimer = new System.Windows.Forms.Timer(this.components);
            this.tsslSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountEnable = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountEnable_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpProxyAccountList.SuspendLayout();
            this.ssProxyAccount.SuspendLayout();
            this.tlpAccountList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).BeginInit();
            this.tlpAccountButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyAccountList
            // 
            this.tlpProxyAccountList.ColumnCount = 1;
            this.tlpProxyAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyAccountList.Controls.Add(this.ssProxyAccount, 0, 1);
            this.tlpProxyAccountList.Controls.Add(this.tlpAccountList, 0, 0);
            this.tlpProxyAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyAccountList.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyAccountList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyAccountList.Name = "tlpProxyAccountList";
            this.tlpProxyAccountList.RowCount = 2;
            this.tlpProxyAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyAccountList.Size = new System.Drawing.Size(684, 411);
            this.tlpProxyAccountList.TabIndex = 0;
            // 
            // ssProxyAccount
            // 
            this.ssProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssProxyAccount.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslAccount,
            this.tsslAccount_CNT,
            this.tsslSplit,
            this.tsslAccountEnable,
            this.tsslAccountEnable_CNT});
            this.ssProxyAccount.Location = new System.Drawing.Point(3, 386);
            this.ssProxyAccount.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.ssProxyAccount.Name = "ssProxyAccount";
            this.ssProxyAccount.Size = new System.Drawing.Size(678, 22);
            this.ssProxyAccount.TabIndex = 0;
            // 
            // tlpAccountList
            // 
            this.tlpAccountList.ColumnCount = 2;
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpAccountList.Controls.Add(this.dgvAccountList, 0, 0);
            this.tlpAccountList.Controls.Add(this.tlpAccountButton, 1, 0);
            this.tlpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountList.Location = new System.Drawing.Point(0, 0);
            this.tlpAccountList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountList.Name = "tlpAccountList";
            this.tlpAccountList.RowCount = 1;
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.Size = new System.Drawing.Size(684, 386);
            this.tlpAccountList.TabIndex = 1;
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
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.cCreateTime,
            this.cIsExpiry,
            this.cExpiryTime});
            this.dgvAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccountList.Location = new System.Drawing.Point(3, 3);
            this.dgvAccountList.Name = "dgvAccountList";
            this.dgvAccountList.ReadOnly = true;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.dgvAccountList.Size = new System.Drawing.Size(558, 380);
            this.dgvAccountList.TabIndex = 0;
            this.dgvAccountList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAccountList_CellFormatting);
            // 
            // cAccountID
            // 
            this.cAccountID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccountID.DefaultCellStyle = dataGridViewCellStyle9;
            this.cAccountID.HeaderText = "序号";
            this.cAccountID.Name = "cAccountID";
            this.cAccountID.ReadOnly = true;
            this.cAccountID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccountID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cAccountID.Width = 38;
            // 
            // cIsEnable
            // 
            this.cIsEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsEnable.DataPropertyName = "IsEnable";
            this.cIsEnable.HeaderText = "启用";
            this.cIsEnable.Name = "cIsEnable";
            this.cIsEnable.ReadOnly = true;
            this.cIsEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIsEnable.Width = 38;
            // 
            // cUserName
            // 
            this.cUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cUserName.DataPropertyName = "UserName";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.DefaultCellStyle = dataGridViewCellStyle10;
            this.cUserName.HeaderText = "用户名";
            this.cUserName.Name = "cUserName";
            this.cUserName.ReadOnly = true;
            this.cUserName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cUserName.Width = 50;
            // 
            // cLoginIP
            // 
            this.cLoginIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLoginIP.DataPropertyName = "LoginIP";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.DefaultCellStyle = dataGridViewCellStyle11;
            this.cLoginIP.HeaderText = "登录 IP";
            this.cLoginIP.Name = "cLoginIP";
            this.cLoginIP.ReadOnly = true;
            this.cLoginIP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cCreateTime
            // 
            this.cCreateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cCreateTime.DataPropertyName = "CreateTime";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cCreateTime.DefaultCellStyle = dataGridViewCellStyle12;
            this.cCreateTime.HeaderText = "开通时间";
            this.cCreateTime.Name = "cCreateTime";
            this.cCreateTime.ReadOnly = true;
            this.cCreateTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCreateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCreateTime.Width = 62;
            // 
            // cIsExpiry
            // 
            this.cIsExpiry.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsExpiry.DataPropertyName = "IsExpiry";
            this.cIsExpiry.HeaderText = "是否过期";
            this.cIsExpiry.Name = "cIsExpiry";
            this.cIsExpiry.ReadOnly = true;
            this.cIsExpiry.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIsExpiry.Width = 62;
            // 
            // cExpiryTime
            // 
            this.cExpiryTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cExpiryTime.DataPropertyName = "ExpiryTime";
            this.cExpiryTime.HeaderText = "过期时间";
            this.cExpiryTime.Name = "cExpiryTime";
            this.cExpiryTime.ReadOnly = true;
            this.cExpiryTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cExpiryTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cExpiryTime.Width = 62;
            // 
            // tlpAccountButton
            // 
            this.tlpAccountButton.ColumnCount = 3;
            this.tlpAccountButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpAccountButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpAccountButton.Controls.Add(this.bDelete, 1, 5);
            this.tlpAccountButton.Controls.Add(this.bUpdate, 1, 3);
            this.tlpAccountButton.Controls.Add(this.bAccount_New, 1, 1);
            this.tlpAccountButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountButton.Location = new System.Drawing.Point(564, 0);
            this.tlpAccountButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountButton.Name = "tlpAccountButton";
            this.tlpAccountButton.RowCount = 7;
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpAccountButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAccountButton.Size = new System.Drawing.Size(120, 386);
            this.tlpAccountButton.TabIndex = 1;
            // 
            // bUpdate
            // 
            this.bUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bUpdate.Location = new System.Drawing.Point(13, 178);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(94, 29);
            this.bUpdate.TabIndex = 1;
            this.bUpdate.Text = "编 辑";
            this.bUpdate.UseVisualStyleBackColor = true;
            this.bUpdate.Click += new System.EventHandler(this.bUpdate_Click);
            // 
            // bAccount_New
            // 
            this.bAccount_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bAccount_New.Location = new System.Drawing.Point(13, 123);
            this.bAccount_New.Name = "bAccount_New";
            this.bAccount_New.Size = new System.Drawing.Size(94, 29);
            this.bAccount_New.TabIndex = 0;
            this.bAccount_New.Text = "新 建";
            this.bAccount_New.UseVisualStyleBackColor = true;
            this.bAccount_New.Click += new System.EventHandler(this.bAccount_New_Click);
            // 
            // bDelete
            // 
            this.bDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bDelete.Location = new System.Drawing.Point(13, 233);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(94, 29);
            this.bDelete.TabIndex = 2;
            this.bDelete.Text = "删 除";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // tsslAccount
            // 
            this.tsslAccount.Name = "tsslAccount";
            this.tsslAccount.Size = new System.Drawing.Size(59, 17);
            this.tsslAccount.Text = "账户总数:";
            // 
            // tsslAccount_CNT
            // 
            this.tsslAccount_CNT.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsslAccount_CNT.Name = "tsslAccount_CNT";
            this.tsslAccount_CNT.Size = new System.Drawing.Size(15, 17);
            this.tsslAccount_CNT.Text = "0";
            // 
            // tTimer
            // 
            this.tTimer.Enabled = true;
            this.tTimer.Interval = 1000;
            this.tTimer.Tick += new System.EventHandler(this.tTimer_Tick);
            // 
            // tsslSplit
            // 
            this.tsslSplit.Enabled = false;
            this.tsslSplit.Name = "tsslSplit";
            this.tsslSplit.Size = new System.Drawing.Size(11, 17);
            this.tsslSplit.Text = "|";
            // 
            // tsslAccountEnable
            // 
            this.tsslAccountEnable.Name = "tsslAccountEnable";
            this.tsslAccountEnable.Size = new System.Drawing.Size(35, 17);
            this.tsslAccountEnable.Text = "启用:";
            // 
            // tsslAccountEnable_CNT
            // 
            this.tsslAccountEnable_CNT.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsslAccountEnable_CNT.Name = "tsslAccountEnable_CNT";
            this.tsslAccountEnable_CNT.Size = new System.Drawing.Size(15, 17);
            this.tsslAccountEnable_CNT.Text = "0";
            // 
            // Proxy_AccountListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.tlpProxyAccountList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Proxy_AccountListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "账号管理";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn cAccountID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCreateTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn cExpiryTime;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccount;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccount_CNT;
        private System.Windows.Forms.Timer tTimer;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountEnable;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountEnable_CNT;
    }
}
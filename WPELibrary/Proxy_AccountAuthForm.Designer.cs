namespace WPELibrary
{
    partial class Proxy_AccountAuthForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountAuthForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpAccountAuth = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAuth = new System.Windows.Forms.DataGridView();
            this.cAuthID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAuthTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLinksNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDevicesNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAuthResult = new System.Windows.Forms.DataGridViewImageColumn();
            this.ssAccountAuth = new System.Windows.Forms.StatusStrip();
            this.tsslAuthCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAuthCount_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLinksCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLinksCount_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDevicesCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDevicesCount_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbAccountAuth_Search = new System.Windows.Forms.GroupBox();
            this.tlpAccountAuth_Search = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch_Account = new System.Windows.Forms.TextBox();
            this.bSearch_Account = new System.Windows.Forms.Button();
            this.lSearch_Account = new System.Windows.Forms.Label();
            this.tlpAccountAuth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuth)).BeginInit();
            this.ssAccountAuth.SuspendLayout();
            this.gbAccountAuth_Search.SuspendLayout();
            this.tlpAccountAuth_Search.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpAccountAuth
            // 
            resources.ApplyResources(this.tlpAccountAuth, "tlpAccountAuth");
            this.tlpAccountAuth.Controls.Add(this.dgvAuth, 0, 1);
            this.tlpAccountAuth.Controls.Add(this.ssAccountAuth, 0, 2);
            this.tlpAccountAuth.Controls.Add(this.gbAccountAuth_Search, 0, 0);
            this.tlpAccountAuth.Name = "tlpAccountAuth";
            // 
            // dgvAuth
            // 
            this.dgvAuth.AllowUserToAddRows = false;
            this.dgvAuth.AllowUserToDeleteRows = false;
            this.dgvAuth.AllowUserToResizeColumns = false;
            this.dgvAuth.AllowUserToResizeRows = false;
            this.dgvAuth.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAuth.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAuth.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvAuth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAuth.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAuth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuth.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cAuthID,
            this.cAuthTime,
            this.cAID,
            this.cIPAddress,
            this.cUserName,
            this.cLinksNumber,
            this.cDevicesNumber,
            this.cAuthResult});
            resources.ApplyResources(this.dgvAuth, "dgvAuth");
            this.dgvAuth.MultiSelect = false;
            this.dgvAuth.Name = "dgvAuth";
            this.dgvAuth.ReadOnly = true;
            this.dgvAuth.RowHeadersVisible = false;
            this.dgvAuth.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAuth.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvAuth.RowTemplate.Height = 23;
            this.dgvAuth.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAuth.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAuth_CellFormatting);
            // 
            // cAuthID
            // 
            this.cAuthID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAuthID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cAuthID, "cAuthID");
            this.cAuthID.Name = "cAuthID";
            this.cAuthID.ReadOnly = true;
            this.cAuthID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAuthID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAuthTime
            // 
            this.cAuthTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cAuthTime.DataPropertyName = "AuthTime";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAuthTime.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cAuthTime, "cAuthTime");
            this.cAuthTime.Name = "cAuthTime";
            this.cAuthTime.ReadOnly = true;
            this.cAuthTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAuthTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAID
            // 
            this.cAID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cAID.DataPropertyName = "AID";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAID.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cAID, "cAID");
            this.cAID.Name = "cAID";
            this.cAID.ReadOnly = true;
            this.cAID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIPAddress
            // 
            this.cIPAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIPAddress.DataPropertyName = "IPAddress";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPAddress.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.cIPAddress, "cIPAddress");
            this.cIPAddress.Name = "cIPAddress";
            this.cIPAddress.ReadOnly = true;
            this.cIPAddress.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cUserName
            // 
            this.cUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.cUserName, "cUserName");
            this.cUserName.Name = "cUserName";
            this.cUserName.ReadOnly = true;
            this.cUserName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLinksNumber
            // 
            this.cLinksNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cLinksNumber.DataPropertyName = "LinksNumber";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLinksNumber.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.cLinksNumber, "cLinksNumber");
            this.cLinksNumber.Name = "cLinksNumber";
            this.cLinksNumber.ReadOnly = true;
            this.cLinksNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLinksNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cDevicesNumber
            // 
            this.cDevicesNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cDevicesNumber.DataPropertyName = "DevicesNumber";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cDevicesNumber.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.cDevicesNumber, "cDevicesNumber");
            this.cDevicesNumber.Name = "cDevicesNumber";
            this.cDevicesNumber.ReadOnly = true;
            this.cDevicesNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cDevicesNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAuthResult
            // 
            this.cAuthResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cAuthResult.DataPropertyName = "AuthResult";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle9.NullValue")));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cAuthResult.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.cAuthResult, "cAuthResult");
            this.cAuthResult.Name = "cAuthResult";
            this.cAuthResult.ReadOnly = true;
            this.cAuthResult.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ssAccountAuth
            // 
            resources.ApplyResources(this.ssAccountAuth, "ssAccountAuth");
            this.ssAccountAuth.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslAuthCount,
            this.tsslAuthCount_Value,
            this.toolStripStatusLabel1,
            this.tsslLinksCount,
            this.tsslLinksCount_Value,
            this.toolStripStatusLabel4,
            this.tsslDevicesCount,
            this.tsslDevicesCount_Value});
            this.ssAccountAuth.Name = "ssAccountAuth";
            // 
            // tsslAuthCount
            // 
            this.tsslAuthCount.Name = "tsslAuthCount";
            resources.ApplyResources(this.tsslAuthCount, "tsslAuthCount");
            // 
            // tsslAuthCount_Value
            // 
            resources.ApplyResources(this.tsslAuthCount_Value, "tsslAuthCount_Value");
            this.tsslAuthCount_Value.Name = "tsslAuthCount_Value";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // tsslLinksCount
            // 
            this.tsslLinksCount.Name = "tsslLinksCount";
            resources.ApplyResources(this.tsslLinksCount, "tsslLinksCount");
            // 
            // tsslLinksCount_Value
            // 
            resources.ApplyResources(this.tsslLinksCount_Value, "tsslLinksCount_Value");
            this.tsslLinksCount_Value.Name = "tsslLinksCount_Value";
            // 
            // toolStripStatusLabel4
            // 
            resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            // 
            // tsslDevicesCount
            // 
            this.tsslDevicesCount.Name = "tsslDevicesCount";
            resources.ApplyResources(this.tsslDevicesCount, "tsslDevicesCount");
            // 
            // tsslDevicesCount_Value
            // 
            resources.ApplyResources(this.tsslDevicesCount_Value, "tsslDevicesCount_Value");
            this.tsslDevicesCount_Value.Name = "tsslDevicesCount_Value";
            // 
            // gbAccountAuth_Search
            // 
            this.gbAccountAuth_Search.Controls.Add(this.tlpAccountAuth_Search);
            resources.ApplyResources(this.gbAccountAuth_Search, "gbAccountAuth_Search");
            this.gbAccountAuth_Search.Name = "gbAccountAuth_Search";
            this.gbAccountAuth_Search.TabStop = false;
            // 
            // tlpAccountAuth_Search
            // 
            resources.ApplyResources(this.tlpAccountAuth_Search, "tlpAccountAuth_Search");
            this.tlpAccountAuth_Search.Controls.Add(this.txtSearch_Account, 1, 1);
            this.tlpAccountAuth_Search.Controls.Add(this.bSearch_Account, 3, 1);
            this.tlpAccountAuth_Search.Controls.Add(this.lSearch_Account, 0, 1);
            this.tlpAccountAuth_Search.Name = "tlpAccountAuth_Search";
            // 
            // txtSearch_Account
            // 
            resources.ApplyResources(this.txtSearch_Account, "txtSearch_Account");
            this.txtSearch_Account.Name = "txtSearch_Account";
            // 
            // bSearch_Account
            // 
            resources.ApplyResources(this.bSearch_Account, "bSearch_Account");
            this.bSearch_Account.Name = "bSearch_Account";
            this.bSearch_Account.UseVisualStyleBackColor = true;
            this.bSearch_Account.Click += new System.EventHandler(this.bSearch_Account_Click);
            // 
            // lSearch_Account
            // 
            resources.ApplyResources(this.lSearch_Account, "lSearch_Account");
            this.lSearch_Account.Name = "lSearch_Account";
            // 
            // Proxy_AccountAuthForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpAccountAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Proxy_AccountAuthForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Proxy_AccountAuthForm_FormClosed);
            this.Load += new System.EventHandler(this.Proxy_AccountAuthForm_Load);
            this.tlpAccountAuth.ResumeLayout(false);
            this.tlpAccountAuth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuth)).EndInit();
            this.ssAccountAuth.ResumeLayout(false);
            this.ssAccountAuth.PerformLayout();
            this.gbAccountAuth_Search.ResumeLayout(false);
            this.tlpAccountAuth_Search.ResumeLayout(false);
            this.tlpAccountAuth_Search.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpAccountAuth;
        private System.Windows.Forms.DataGridView dgvAuth;
        private System.Windows.Forms.StatusStrip ssAccountAuth;
        private System.Windows.Forms.GroupBox gbAccountAuth_Search;
        private System.Windows.Forms.TableLayoutPanel tlpAccountAuth_Search;
        private System.Windows.Forms.TextBox txtSearch_Account;
        private System.Windows.Forms.Button bSearch_Account;
        private System.Windows.Forms.Label lSearch_Account;
        private System.Windows.Forms.ToolStripStatusLabel tsslAuthCount;
        private System.Windows.Forms.ToolStripStatusLabel tsslAuthCount_Value;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslLinksCount;
        private System.Windows.Forms.ToolStripStatusLabel tsslLinksCount_Value;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsslDevicesCount;
        private System.Windows.Forms.ToolStripStatusLabel tsslDevicesCount_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAuthID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAuthTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLinksNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDevicesNumber;
        private System.Windows.Forms.DataGridViewImageColumn cAuthResult;
    }
}
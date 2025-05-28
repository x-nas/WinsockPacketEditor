namespace WPELibrary
{
    partial class Proxy_AccountLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountLoginForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpAccountLogin = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAccountLogin = new System.Windows.Forms.DataGridView();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpAccountLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountLogin)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpAccountLogin
            // 
            resources.ApplyResources(this.tlpAccountLogin, "tlpAccountLogin");
            this.tlpAccountLogin.Controls.Add(this.dgvAccountLogin, 0, 0);
            this.tlpAccountLogin.Name = "tlpAccountLogin";
            // 
            // dgvAccountLogin
            // 
            resources.ApplyResources(this.dgvAccountLogin, "dgvAccountLogin");
            this.dgvAccountLogin.AllowUserToAddRows = false;
            this.dgvAccountLogin.AllowUserToDeleteRows = false;
            this.dgvAccountLogin.AllowUserToResizeColumns = false;
            this.dgvAccountLogin.AllowUserToResizeRows = false;
            this.dgvAccountLogin.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAccountLogin.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAccountLogin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountLogin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cID,
            this.cLoginTime,
            this.cLoginIP,
            this.cIPLocation});
            this.dgvAccountLogin.Name = "dgvAccountLogin";
            this.dgvAccountLogin.RowHeadersVisible = false;
            this.dgvAccountLogin.RowTemplate.Height = 23;
            this.dgvAccountLogin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccountLogin.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAccountLogin_CellFormatting);
            // 
            // cID
            // 
            this.cID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.cID, "cID");
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLoginTime
            // 
            this.cLoginTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cLoginTime.DataPropertyName = "LoginTime";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginTime.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cLoginTime, "cLoginTime");
            this.cLoginTime.Name = "cLoginTime";
            this.cLoginTime.ReadOnly = true;
            this.cLoginTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLoginIP
            // 
            this.cLoginIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cLoginIP.DataPropertyName = "LoginIP";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cLoginIP, "cLoginIP");
            this.cLoginIP.Name = "cLoginIP";
            this.cLoginIP.ReadOnly = true;
            this.cLoginIP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIPLocation
            // 
            this.cIPLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cIPLocation.DataPropertyName = "IPLocation";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPLocation.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cIPLocation, "cIPLocation");
            this.cIPLocation.Name = "cIPLocation";
            this.cIPLocation.ReadOnly = true;
            this.cIPLocation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPLocation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Proxy_AccountLoginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpAccountLogin);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Proxy_AccountLoginForm";
            this.tlpAccountLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountLogin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpAccountLogin;
        private System.Windows.Forms.DataGridView dgvAccountLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPLocation;
    }
}
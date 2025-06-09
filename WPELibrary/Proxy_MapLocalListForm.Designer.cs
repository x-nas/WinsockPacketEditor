namespace WPELibrary
{
    partial class Proxy_MapLocalListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_MapLocalListForm));
            this.tlpMapLocal = new System.Windows.Forms.TableLayoutPanel();
            this.lMapLocal = new System.Windows.Forms.Label();
            this.dgvMapLocal = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bAdd = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.cIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cProtocolType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRemotePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLocalPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpMapLocal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapLocal)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMapLocal
            // 
            resources.ApplyResources(this.tlpMapLocal, "tlpMapLocal");
            this.tlpMapLocal.Controls.Add(this.lMapLocal, 0, 0);
            this.tlpMapLocal.Controls.Add(this.dgvMapLocal, 0, 1);
            this.tlpMapLocal.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tlpMapLocal.Name = "tlpMapLocal";
            // 
            // lMapLocal
            // 
            resources.ApplyResources(this.lMapLocal, "lMapLocal");
            this.lMapLocal.Name = "lMapLocal";
            // 
            // dgvMapLocal
            // 
            this.dgvMapLocal.AllowUserToAddRows = false;
            this.dgvMapLocal.AllowUserToDeleteRows = false;
            this.dgvMapLocal.AllowUserToResizeColumns = false;
            this.dgvMapLocal.AllowUserToResizeRows = false;
            this.dgvMapLocal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMapLocal.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvMapLocal.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvMapLocal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMapLocal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIsEnable,
            this.cProtocolType,
            this.cHost,
            this.cPort,
            this.cRemotePath,
            this.cURL,
            this.cLocalPath});
            resources.ApplyResources(this.dgvMapLocal, "dgvMapLocal");
            this.dgvMapLocal.MultiSelect = false;
            this.dgvMapLocal.Name = "dgvMapLocal";
            this.dgvMapLocal.RowHeadersVisible = false;
            this.dgvMapLocal.RowTemplate.Height = 23;
            this.dgvMapLocal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMapLocal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMapLocal_CellContentClick);
            this.dgvMapLocal.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMapLocal_CellDoubleClick);
            this.dgvMapLocal.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMapLocal_CellFormatting);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.bAdd, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.bRemove, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.bClose, 7, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // bAdd
            // 
            resources.ApplyResources(this.bAdd, "bAdd");
            this.bAdd.Name = "bAdd";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bRemove
            // 
            resources.ApplyResources(this.bRemove, "bRemove");
            this.bRemove.Name = "bRemove";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bClose
            // 
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // cIsEnable
            // 
            this.cIsEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsEnable.DataPropertyName = "IsEnable";
            resources.ApplyResources(this.cIsEnable, "cIsEnable");
            this.cIsEnable.Name = "cIsEnable";
            this.cIsEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cProtocolType
            // 
            this.cProtocolType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProtocolType.DataPropertyName = "ProtocolType";
            resources.ApplyResources(this.cProtocolType, "cProtocolType");
            this.cProtocolType.Name = "cProtocolType";
            this.cProtocolType.ReadOnly = true;
            this.cProtocolType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cProtocolType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cHost
            // 
            this.cHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cHost.DataPropertyName = "Host";
            resources.ApplyResources(this.cHost, "cHost");
            this.cHost.Name = "cHost";
            this.cHost.ReadOnly = true;
            this.cHost.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cHost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPort
            // 
            this.cPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cPort.DataPropertyName = "Port";
            resources.ApplyResources(this.cPort, "cPort");
            this.cPort.Name = "cPort";
            this.cPort.ReadOnly = true;
            this.cPort.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPort.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cRemotePath
            // 
            this.cRemotePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cRemotePath.DataPropertyName = "RemotePath";
            resources.ApplyResources(this.cRemotePath, "cRemotePath");
            this.cRemotePath.Name = "cRemotePath";
            this.cRemotePath.ReadOnly = true;
            this.cRemotePath.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRemotePath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cURL
            // 
            this.cURL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cURL, "cURL");
            this.cURL.Name = "cURL";
            this.cURL.ReadOnly = true;
            this.cURL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cURL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLocalPath
            // 
            this.cLocalPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLocalPath.DataPropertyName = "LocalPath";
            resources.ApplyResources(this.cLocalPath, "cLocalPath");
            this.cLocalPath.Name = "cLocalPath";
            this.cLocalPath.ReadOnly = true;
            this.cLocalPath.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLocalPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Proxy_MapLocalListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpMapLocal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_MapLocalListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Proxy_MapLocalForm_FormClosed);
            this.Load += new System.EventHandler(this.Proxy_MapLocalForm_Load);
            this.tlpMapLocal.ResumeLayout(false);
            this.tlpMapLocal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapLocal)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMapLocal;
        private System.Windows.Forms.Label lMapLocal;
        private System.Windows.Forms.DataGridView dgvMapLocal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProtocolType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRemotePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn cURL;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLocalPath;
    }
}
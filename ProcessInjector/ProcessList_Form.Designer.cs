
namespace ProcessInjector
{
    partial class ProcessList_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessList_Form));
            this.bCreate = new System.Windows.Forms.Button();
            this.bSelected = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.dgvProcessList = new System.Windows.Forms.DataGridView();
            this.cICO = new System.Windows.Forms.DataGridViewImageColumn();
            this.cProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProcessID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcessList)).BeginInit();
            this.SuspendLayout();
            // 
            // bCreate
            // 
            this.bCreate.Location = new System.Drawing.Point(10, 419);
            this.bCreate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(87, 33);
            this.bCreate.TabIndex = 14;
            this.bCreate.Text = "选择程序";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // bSelected
            // 
            this.bSelected.Location = new System.Drawing.Point(308, 419);
            this.bSelected.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bSelected.Name = "bSelected";
            this.bSelected.Size = new System.Drawing.Size(87, 33);
            this.bSelected.TabIndex = 12;
            this.bSelected.Text = "确定";
            this.bSelected.UseVisualStyleBackColor = true;
            this.bSelected.Click += new System.EventHandler(this.bSelected_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(119, 419);
            this.bRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(87, 33);
            this.bRefresh.TabIndex = 11;
            this.bRefresh.Text = "刷新";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // dgvProcessList
            // 
            this.dgvProcessList.AllowUserToAddRows = false;
            this.dgvProcessList.AllowUserToDeleteRows = false;
            this.dgvProcessList.AllowUserToResizeColumns = false;
            this.dgvProcessList.AllowUserToResizeRows = false;
            this.dgvProcessList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProcessList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProcessList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProcessList.ColumnHeadersHeight = 29;
            this.dgvProcessList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProcessList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cICO,
            this.cProcessName,
            this.cProcessID});
            this.dgvProcessList.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvProcessList.EnableHeadersVisualStyles = false;
            this.dgvProcessList.Location = new System.Drawing.Point(0, 0);
            this.dgvProcessList.Margin = new System.Windows.Forms.Padding(2);
            this.dgvProcessList.MultiSelect = false;
            this.dgvProcessList.Name = "dgvProcessList";
            this.dgvProcessList.ReadOnly = true;
            this.dgvProcessList.RowHeadersVisible = false;
            this.dgvProcessList.RowHeadersWidth = 51;
            this.dgvProcessList.RowTemplate.Height = 27;
            this.dgvProcessList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProcessList.Size = new System.Drawing.Size(405, 413);
            this.dgvProcessList.TabIndex = 15;
            this.dgvProcessList.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProcessList_CellMouseLeave);
            this.dgvProcessList.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProcessList_CellMouseMove);
            this.dgvProcessList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvProcessList_DataBindingComplete);
            // 
            // cICO
            // 
            this.cICO.DataPropertyName = "ICO";
            this.cICO.HeaderText = "";
            this.cICO.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.cICO.MinimumWidth = 6;
            this.cICO.Name = "cICO";
            this.cICO.ReadOnly = true;
            this.cICO.Width = 50;
            // 
            // cProcessName
            // 
            this.cProcessName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cProcessName.DataPropertyName = "PName";
            this.cProcessName.HeaderText = "进程名称";
            this.cProcessName.MinimumWidth = 6;
            this.cProcessName.Name = "cProcessName";
            this.cProcessName.ReadOnly = true;
            // 
            // cProcessID
            // 
            this.cProcessID.DataPropertyName = "PID";
            this.cProcessID.HeaderText = "进程编号";
            this.cProcessID.MinimumWidth = 6;
            this.cProcessID.Name = "cProcessID";
            this.cProcessID.ReadOnly = true;
            // 
            // ProcessList_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(405, 459);
            this.Controls.Add(this.dgvProcessList);
            this.Controls.Add(this.bCreate);
            this.Controls.Add(this.bSelected);
            this.Controls.Add(this.bRefresh);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ProcessList_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "进程列表";
            this.Load += new System.EventHandler(this.ProcessList_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcessList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button bCreate;
        private System.Windows.Forms.Button bSelected;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.DataGridView dgvProcessList;
        private System.Windows.Forms.DataGridViewImageColumn cICO;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProcessID;
    }
}

namespace WPELibrary
{
    partial class Socket_Filter_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_Filter_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bApply = new System.Windows.Forms.Button();
            this.bReset = new System.Windows.Forms.Button();
            this.gbFilterName = new System.Windows.Forms.GroupBox();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.gbFilterList = new System.Windows.Forms.GroupBox();
            this.dgvFilter = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.gbFilterName.SuspendLayout();
            this.gbFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bApply);
            this.panel1.Controls.Add(this.bReset);
            this.panel1.Controls.Add(this.gbFilterName);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // bApply
            // 
            resources.ApplyResources(this.bApply, "bApply");
            this.bApply.Name = "bApply";
            this.bApply.UseVisualStyleBackColor = true;
            this.bApply.Click += new System.EventHandler(this.bApply_Click);
            // 
            // bReset
            // 
            resources.ApplyResources(this.bReset, "bReset");
            this.bReset.Name = "bReset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // gbFilterName
            // 
            this.gbFilterName.Controls.Add(this.txtFilterName);
            resources.ApplyResources(this.gbFilterName, "gbFilterName");
            this.gbFilterName.Name = "gbFilterName";
            this.gbFilterName.TabStop = false;
            // 
            // txtFilterName
            // 
            resources.ApplyResources(this.txtFilterName, "txtFilterName");
            this.txtFilterName.Name = "txtFilterName";
            // 
            // gbFilterList
            // 
            this.gbFilterList.Controls.Add(this.dgvFilter);
            resources.ApplyResources(this.gbFilterList, "gbFilterList");
            this.gbFilterList.Name = "gbFilterList";
            this.gbFilterList.TabStop = false;
            // 
            // dgvFilter
            // 
            this.dgvFilter.AllowUserToAddRows = false;
            this.dgvFilter.AllowUserToDeleteRows = false;
            this.dgvFilter.AllowUserToResizeColumns = false;
            this.dgvFilter.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvFilter, "dgvFilter");
            this.dgvFilter.MultiSelect = false;
            this.dgvFilter.Name = "dgvFilter";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFilter.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilter.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFilter.RowTemplate.Height = 30;
            this.dgvFilter.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvFilter_ColumnAdded);
            this.dgvFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilterList_KeyDown);
            // 
            // Socket_Filter_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.gbFilterList);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_Filter_Form";
            this.Load += new System.EventHandler(this.Filter_Form_Load);
            this.panel1.ResumeLayout(false);
            this.gbFilterName.ResumeLayout(false);
            this.gbFilterName.PerformLayout();
            this.gbFilterList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bApply;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.GroupBox gbFilterName;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.GroupBox gbFilterList;
        private System.Windows.Forms.DataGridView dgvFilter;
    }
}
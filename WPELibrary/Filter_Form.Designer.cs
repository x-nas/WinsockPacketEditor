
namespace WPELibrary
{
    partial class Filter_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Filter_Form));
            this.dgvFilter = new System.Windows.Forms.DataGridView();
            this.gbFilterName = new System.Windows.Forms.GroupBox();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.bReset = new System.Windows.Forms.Button();
            this.bApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).BeginInit();
            this.gbFilterName.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvFilter
            // 
            this.dgvFilter.AllowUserToAddRows = false;
            this.dgvFilter.AllowUserToDeleteRows = false;
            this.dgvFilter.AllowUserToResizeColumns = false;
            this.dgvFilter.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvFilter.Location = new System.Drawing.Point(0, 0);
            this.dgvFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvFilter.MultiSelect = false;
            this.dgvFilter.Name = "dgvFilter";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFilter.RowHeadersWidth = 100;
            this.dgvFilter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilter.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFilter.RowTemplate.Height = 23;
            this.dgvFilter.Size = new System.Drawing.Size(684, 90);
            this.dgvFilter.TabIndex = 2;
            this.dgvFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilterList_KeyDown);
            // 
            // gbFilterName
            // 
            this.gbFilterName.Controls.Add(this.txtFilterName);
            this.gbFilterName.Location = new System.Drawing.Point(12, 97);
            this.gbFilterName.Name = "gbFilterName";
            this.gbFilterName.Size = new System.Drawing.Size(305, 53);
            this.gbFilterName.TabIndex = 4;
            this.gbFilterName.TabStop = false;
            this.gbFilterName.Text = "[ 滤镜名称 ]";
            // 
            // txtFilterName
            // 
            this.txtFilterName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilterName.Location = new System.Drawing.Point(3, 19);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(299, 23);
            this.txtFilterName.TabIndex = 4;
            this.txtFilterName.WordWrap = false;
            // 
            // bReset
            // 
            this.bReset.Location = new System.Drawing.Point(337, 111);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(75, 32);
            this.bReset.TabIndex = 5;
            this.bReset.Text = "重 置";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // bApply
            // 
            this.bApply.Location = new System.Drawing.Point(597, 111);
            this.bApply.Name = "bApply";
            this.bApply.Size = new System.Drawing.Size(75, 32);
            this.bApply.TabIndex = 6;
            this.bApply.Text = "应 用";
            this.bApply.UseVisualStyleBackColor = true;
            this.bApply.Click += new System.EventHandler(this.bApply_Click);
            // 
            // Filter_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(684, 159);
            this.Controls.Add(this.bApply);
            this.Controls.Add(this.bReset);
            this.Controls.Add(this.gbFilterName);
            this.Controls.Add(this.dgvFilter);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Filter_Form";
            this.Text = "Filter_Form";
            this.Load += new System.EventHandler(this.Filter_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).EndInit();
            this.gbFilterName.ResumeLayout(false);
            this.gbFilterName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFilter;
        private System.Windows.Forms.GroupBox gbFilterName;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.Button bApply;
    }
}
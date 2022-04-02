
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessList_Form));
            this.lvProcessList = new System.Windows.Forms.ListView();
            this.chPName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRAM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lProcessCNT = new System.Windows.Forms.Label();
            this.bSelected = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvProcessList
            // 
            this.lvProcessList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPName,
            this.chPID,
            this.chRAM});
            this.lvProcessList.FullRowSelect = true;
            this.lvProcessList.GridLines = true;
            this.lvProcessList.HideSelection = false;
            this.lvProcessList.Location = new System.Drawing.Point(2, 4);
            this.lvProcessList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lvProcessList.MultiSelect = false;
            this.lvProcessList.Name = "lvProcessList";
            this.lvProcessList.Size = new System.Drawing.Size(399, 403);
            this.lvProcessList.TabIndex = 5;
            this.lvProcessList.UseCompatibleStateImageBehavior = false;
            this.lvProcessList.View = System.Windows.Forms.View.Details;
            // 
            // chPName
            // 
            this.chPName.Text = "进程名称";
            this.chPName.Width = 210;
            // 
            // chPID
            // 
            this.chPID.Text = "PID";
            // 
            // chRAM
            // 
            this.chRAM.Text = "RAM";
            this.chRAM.Width = 100;
            // 
            // lProcessCNT
            // 
            this.lProcessCNT.AutoSize = true;
            this.lProcessCNT.Location = new System.Drawing.Point(14, 425);
            this.lProcessCNT.Name = "lProcessCNT";
            this.lProcessCNT.Size = new System.Drawing.Size(44, 17);
            this.lProcessCNT.TabIndex = 8;
            this.lProcessCNT.Text = "进程数";
            // 
            // bSelected
            // 
            this.bSelected.Location = new System.Drawing.Point(315, 418);
            this.bSelected.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bSelected.Name = "bSelected";
            this.bSelected.Size = new System.Drawing.Size(87, 33);
            this.bSelected.TabIndex = 7;
            this.bSelected.Text = "确定";
            this.bSelected.UseVisualStyleBackColor = true;
            this.bSelected.Click += new System.EventHandler(this.bSelected_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(220, 418);
            this.bRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(87, 33);
            this.bRefresh.TabIndex = 6;
            this.bRefresh.Text = "刷新";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bCreate
            // 
            this.bCreate.Location = new System.Drawing.Point(127, 418);
            this.bCreate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(87, 33);
            this.bCreate.TabIndex = 9;
            this.bCreate.Text = "选择程序";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // ProcessList_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(405, 463);
            this.Controls.Add(this.bCreate);
            this.Controls.Add(this.lvProcessList);
            this.Controls.Add(this.lProcessCNT);
            this.Controls.Add(this.bSelected);
            this.Controls.Add(this.bRefresh);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ProcessList_Form";
            this.Text = "进程列表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvProcessList;
        private System.Windows.Forms.ColumnHeader chPName;
        private System.Windows.Forms.ColumnHeader chPID;
        private System.Windows.Forms.ColumnHeader chRAM;
        private System.Windows.Forms.Label lProcessCNT;
        private System.Windows.Forms.Button bSelected;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bCreate;
    }
}
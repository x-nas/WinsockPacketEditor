namespace WPE.InjectMode
{
    partial class RobotEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobotEditForm));
            this.tlpRobotEdit = new System.Windows.Forms.TableLayoutPanel();
            this.tlpRobotINST = new System.Windows.Forms.TableLayoutPanel();
            this.cRobotINST = new AntdUI.Collapse();
            this.collapseItem1 = new AntdUI.CollapseItem();
            this.collapseItem2 = new AntdUI.CollapseItem();
            this.collapseItem3 = new AntdUI.CollapseItem();
            this.tlpRobotEdit.SuspendLayout();
            this.tlpRobotINST.SuspendLayout();
            this.cRobotINST.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRobotEdit
            // 
            this.tlpRobotEdit.ColumnCount = 1;
            this.tlpRobotEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRobotEdit.Controls.Add(this.tlpRobotINST, 0, 0);
            this.tlpRobotEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotEdit.Name = "tlpRobotEdit";
            this.tlpRobotEdit.RowCount = 2;
            this.tlpRobotEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRobotEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRobotEdit.Size = new System.Drawing.Size(984, 761);
            this.tlpRobotEdit.TabIndex = 0;
            // 
            // tlpRobotINST
            // 
            this.tlpRobotINST.ColumnCount = 2;
            this.tlpRobotINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRobotINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRobotINST.Controls.Add(this.cRobotINST, 0, 0);
            this.tlpRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotINST.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotINST.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotINST.Name = "tlpRobotINST";
            this.tlpRobotINST.RowCount = 1;
            this.tlpRobotINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRobotINST.Size = new System.Drawing.Size(984, 380);
            this.tlpRobotINST.TabIndex = 0;
            // 
            // cRobotINST
            // 
            this.cRobotINST.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cRobotINST.Items.Add(this.collapseItem1);
            this.cRobotINST.Items.Add(this.collapseItem2);
            this.cRobotINST.Items.Add(this.collapseItem3);
            this.cRobotINST.Location = new System.Drawing.Point(3, 3);
            this.cRobotINST.Name = "cRobotINST";
            this.cRobotINST.Size = new System.Drawing.Size(486, 374);
            this.cRobotINST.TabIndex = 0;
            this.cRobotINST.Text = "collapse1";
            // 
            // collapseItem1
            // 
            this.collapseItem1.Location = new System.Drawing.Point(-100, -60);
            this.collapseItem1.Name = "collapseItem1";
            this.collapseItem1.Size = new System.Drawing.Size(100, 60);
            this.collapseItem1.TabIndex = 0;
            this.collapseItem1.Text = "collapseItem1";
            // 
            // collapseItem2
            // 
            this.collapseItem2.Location = new System.Drawing.Point(-100, -60);
            this.collapseItem2.Name = "collapseItem2";
            this.collapseItem2.Size = new System.Drawing.Size(100, 60);
            this.collapseItem2.TabIndex = 1;
            this.collapseItem2.Text = "collapseItem2";
            // 
            // collapseItem3
            // 
            this.collapseItem3.Location = new System.Drawing.Point(-100, -60);
            this.collapseItem3.Name = "collapseItem3";
            this.collapseItem3.Size = new System.Drawing.Size(100, 60);
            this.collapseItem3.TabIndex = 2;
            this.collapseItem3.Text = "collapseItem3";
            // 
            // RobotEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tlpRobotEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "RobotEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RobotEditForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobotEditForm_FormClosing);
            this.Load += new System.EventHandler(this.RobotEditForm_Load);
            this.tlpRobotEdit.ResumeLayout(false);
            this.tlpRobotINST.ResumeLayout(false);
            this.cRobotINST.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRobotEdit;
        private System.Windows.Forms.TableLayoutPanel tlpRobotINST;
        private AntdUI.Collapse cRobotINST;
        private AntdUI.CollapseItem collapseItem1;
        private AntdUI.CollapseItem collapseItem2;
        private AntdUI.CollapseItem collapseItem3;
    }
}
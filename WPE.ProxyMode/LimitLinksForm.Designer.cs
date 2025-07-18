namespace WPE.ProxyMode
{
    partial class LimitLinksForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LimitLinksForm));
            this.tlpLimitLinks = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpSet = new System.Windows.Forms.TableLayoutPanel();
            this.cbIsLimitLinks = new AntdUI.Checkbox();
            this.nudLimitLinks = new AntdUI.InputNumber();
            this.lAccountCNT = new AntdUI.Label();
            this.tlpLimitLinks.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLimitLinks
            // 
            this.tlpLimitLinks.ColumnCount = 1;
            this.tlpLimitLinks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLimitLinks.Controls.Add(this.tlpButton, 0, 4);
            this.tlpLimitLinks.Controls.Add(this.tlpSet, 0, 2);
            this.tlpLimitLinks.Controls.Add(this.lAccountCNT, 0, 0);
            this.tlpLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLimitLinks.Location = new System.Drawing.Point(0, 0);
            this.tlpLimitLinks.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLimitLinks.Name = "tlpLimitLinks";
            this.tlpLimitLinks.RowCount = 5;
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpLimitLinks.Size = new System.Drawing.Size(334, 761);
            this.tlpLimitLinks.TabIndex = 2;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(334, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.Location = new System.Drawing.Point(40, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(180, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpSet
            // 
            this.tlpSet.ColumnCount = 2;
            this.tlpSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSet.Controls.Add(this.cbIsLimitLinks, 0, 0);
            this.tlpSet.Controls.Add(this.nudLimitLinks, 1, 0);
            this.tlpSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSet.Location = new System.Drawing.Point(0, 70);
            this.tlpSet.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSet.Name = "tlpSet";
            this.tlpSet.RowCount = 2;
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSet.Size = new System.Drawing.Size(334, 300);
            this.tlpSet.TabIndex = 0;
            // 
            // cbIsLimitLinks
            // 
            this.cbIsLimitLinks.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsLimitLinks.Location = new System.Drawing.Point(3, 3);
            this.cbIsLimitLinks.Name = "cbIsLimitLinks";
            this.cbIsLimitLinks.Size = new System.Drawing.Size(122, 42);
            this.cbIsLimitLinks.TabIndex = 6;
            this.cbIsLimitLinks.Text = "限制链接数";
            this.cbIsLimitLinks.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsLimitLinks_CheckedChanged);
            // 
            // nudLimitLinks
            // 
            this.nudLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitLinks.Location = new System.Drawing.Point(131, 3);
            this.nudLimitLinks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimitLinks.Name = "nudLimitLinks";
            this.nudLimitLinks.PrefixSvg = "";
            this.nudLimitLinks.SelectionStart = 1;
            this.nudLimitLinks.Size = new System.Drawing.Size(200, 45);
            this.nudLimitLinks.SuffixSvg = "ForkOutlined";
            this.nudLimitLinks.TabIndex = 7;
            this.nudLimitLinks.Text = "1";
            this.nudLimitLinks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLimitLinks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lAccountCNT
            // 
            this.lAccountCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAccountCNT.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAccountCNT.Location = new System.Drawing.Point(3, 3);
            this.lAccountCNT.Name = "lAccountCNT";
            this.lAccountCNT.Size = new System.Drawing.Size(328, 44);
            this.lAccountCNT.TabIndex = 18;
            this.lAccountCNT.Text = "0";
            // 
            // LimitLinksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 761);
            this.Controls.Add(this.tlpLimitLinks);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "LimitLinksForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LimitLinksForm";
            this.Load += new System.EventHandler(this.LimitLinksForm_Load);
            this.tlpLimitLinks.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpSet.ResumeLayout(false);
            this.tlpSet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLimitLinks;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private System.Windows.Forms.TableLayoutPanel tlpSet;
        private AntdUI.Label lAccountCNT;
        private AntdUI.Checkbox cbIsLimitLinks;
        private AntdUI.InputNumber nudLimitLinks;
    }
}
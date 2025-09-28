namespace WinsockPacketEditor
{
    partial class LimitLinks
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpLimitLinks = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpSet = new WinsockPacketEditor.TableLayoutPanelEx();
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
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLimitLinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpLimitLinks.Size = new System.Drawing.Size(500, 700);
            this.tlpLimitLinks.TabIndex = 3;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 651);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 49);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(178, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 37);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(259, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
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
            this.tlpSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpSet.Controls.Add(this.cbIsLimitLinks, 0, 0);
            this.tlpSet.Controls.Add(this.nudLimitLinks, 1, 0);
            this.tlpSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSet.Location = new System.Drawing.Point(0, 56);
            this.tlpSet.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSet.Name = "tlpSet";
            this.tlpSet.RowCount = 2;
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSet.Size = new System.Drawing.Size(500, 243);
            this.tlpSet.TabIndex = 0;
            // 
            // cbIsLimitLinks
            // 
            this.cbIsLimitLinks.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsLimitLinks.LocalizationText = "LimitLinksForm.LimitLinks";
            this.cbIsLimitLinks.Location = new System.Drawing.Point(2, 2);
            this.cbIsLimitLinks.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbIsLimitLinks.Name = "cbIsLimitLinks";
            this.cbIsLimitLinks.Size = new System.Drawing.Size(99, 36);
            this.cbIsLimitLinks.TabIndex = 6;
            this.cbIsLimitLinks.Text = "限制链接数 :";
            this.cbIsLimitLinks.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsLimitLinks_CheckedChanged);
            // 
            // nudLimitLinks
            // 
            this.nudLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitLinks.Location = new System.Drawing.Point(105, 2);
            this.nudLimitLinks.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudLimitLinks.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudLimitLinks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimitLinks.Name = "nudLimitLinks";
            this.nudLimitLinks.PrefixSvg = "";
            this.nudLimitLinks.SelectionStart = 1;
            this.nudLimitLinks.Size = new System.Drawing.Size(393, 36);
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
            this.lAccountCNT.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAccountCNT.Location = new System.Drawing.Point(2, 2);
            this.lAccountCNT.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lAccountCNT.Name = "lAccountCNT";
            this.lAccountCNT.Size = new System.Drawing.Size(496, 36);
            this.lAccountCNT.TabIndex = 18;
            this.lAccountCNT.Text = "0";
            this.lAccountCNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LimitLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLimitLinks);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LimitLinks";
            this.Size = new System.Drawing.Size(500, 700);
            this.Load += new System.EventHandler(this.LimitLinks_Load);
            this.tlpLimitLinks.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpSet.ResumeLayout(false);
            this.tlpSet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpLimitLinks;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpSet;
        private AntdUI.Checkbox cbIsLimitLinks;
        private AntdUI.InputNumber nudLimitLinks;
        private AntdUI.Label lAccountCNT;
    }
}

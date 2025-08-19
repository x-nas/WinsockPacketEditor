namespace WinsockPacketEditor
{
    partial class ComparisonText
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            this.splitterComparison = new AntdUI.Splitter();
            this.tlpComparisonText = new System.Windows.Forms.TableLayoutPanel();
            this.lComparison_B = new AntdUI.Label();
            this.txtComparison_B = new AntdUI.Input();
            this.txtComparison_A = new AntdUI.Input();
            this.lComparison_A = new AntdUI.Label();
            this.tlpComparisonResult = new System.Windows.Forms.TableLayoutPanel();
            this.tlpComparisonButton = new System.Windows.Forms.TableLayoutPanel();
            this.bComparison = new AntdUI.Button();
            this.bComparison_Change = new AntdUI.Button();
            this.bComparison_Reset = new AntdUI.Button();
            this.bComparison_Clean = new AntdUI.Button();
            this.nudComparison_DuplicateNum = new AntdUI.InputNumber();
            this.ddlComparisonType = new AntdUI.Select();
            this.tabComparisonText = new AntdUI.Tabs();
            this.tpComparison = new AntdUI.TabPage();
            this.tpDuplicate = new AntdUI.TabPage();
            this.tComparison = new AntdUI.Table();
            this.tDuplicate = new AntdUI.Table();
            ((System.ComponentModel.ISupportInitialize)(this.splitterComparison)).BeginInit();
            this.splitterComparison.Panel1.SuspendLayout();
            this.splitterComparison.Panel2.SuspendLayout();
            this.splitterComparison.SuspendLayout();
            this.tlpComparisonText.SuspendLayout();
            this.tlpComparisonResult.SuspendLayout();
            this.tlpComparisonButton.SuspendLayout();
            this.tabComparisonText.SuspendLayout();
            this.tpComparison.SuspendLayout();
            this.tpDuplicate.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitterComparison
            // 
            this.splitterComparison.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterComparison.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterComparison.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterComparison.Location = new System.Drawing.Point(0, 0);
            this.splitterComparison.Name = "splitterComparison";
            this.splitterComparison.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterComparison.Panel1
            // 
            this.splitterComparison.Panel1.Controls.Add(this.tlpComparisonText);
            this.splitterComparison.Panel1MinSize = 0;
            // 
            // splitterComparison.Panel2
            // 
            this.splitterComparison.Panel2.Controls.Add(this.tlpComparisonResult);
            this.splitterComparison.Panel2MinSize = 0;
            this.splitterComparison.Size = new System.Drawing.Size(800, 800);
            this.splitterComparison.SplitterDistance = 370;
            this.splitterComparison.SplitterSize = 80;
            this.splitterComparison.SplitterWidth = 10;
            this.splitterComparison.TabIndex = 2;
            // 
            // tlpComparisonText
            // 
            this.tlpComparisonText.ColumnCount = 2;
            this.tlpComparisonText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpComparisonText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpComparisonText.Controls.Add(this.lComparison_B, 1, 0);
            this.tlpComparisonText.Controls.Add(this.txtComparison_B, 1, 1);
            this.tlpComparisonText.Controls.Add(this.txtComparison_A, 0, 1);
            this.tlpComparisonText.Controls.Add(this.lComparison_A, 0, 0);
            this.tlpComparisonText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpComparisonText.Location = new System.Drawing.Point(0, 0);
            this.tlpComparisonText.Margin = new System.Windows.Forms.Padding(0);
            this.tlpComparisonText.Name = "tlpComparisonText";
            this.tlpComparisonText.RowCount = 2;
            this.tlpComparisonText.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpComparisonText.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpComparisonText.Size = new System.Drawing.Size(800, 370);
            this.tlpComparisonText.TabIndex = 0;
            // 
            // lComparison_B
            // 
            this.lComparison_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lComparison_B.Location = new System.Drawing.Point(403, 3);
            this.lComparison_B.Name = "lComparison_B";
            this.lComparison_B.Size = new System.Drawing.Size(394, 23);
            this.lComparison_B.TabIndex = 3;
            this.lComparison_B.Text = "Text B";
            this.lComparison_B.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtComparison_B
            // 
            this.txtComparison_B.AutoScroll = true;
            this.txtComparison_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComparison_B.Location = new System.Drawing.Point(403, 32);
            this.txtComparison_B.Multiline = true;
            this.txtComparison_B.Name = "txtComparison_B";
            this.txtComparison_B.Size = new System.Drawing.Size(394, 335);
            this.txtComparison_B.TabIndex = 1;
            this.txtComparison_B.TextChanged += new System.EventHandler(this.txtComparison_B_TextChanged);
            // 
            // txtComparison_A
            // 
            this.txtComparison_A.AutoScroll = true;
            this.txtComparison_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComparison_A.Location = new System.Drawing.Point(3, 32);
            this.txtComparison_A.Multiline = true;
            this.txtComparison_A.Name = "txtComparison_A";
            this.txtComparison_A.Size = new System.Drawing.Size(394, 335);
            this.txtComparison_A.TabIndex = 0;
            this.txtComparison_A.TextChanged += new System.EventHandler(this.txtComparison_A_TextChanged);
            // 
            // lComparison_A
            // 
            this.lComparison_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lComparison_A.Location = new System.Drawing.Point(3, 3);
            this.lComparison_A.Name = "lComparison_A";
            this.lComparison_A.Size = new System.Drawing.Size(394, 23);
            this.lComparison_A.TabIndex = 2;
            this.lComparison_A.Text = "Text A";
            this.lComparison_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpComparisonResult
            // 
            this.tlpComparisonResult.ColumnCount = 1;
            this.tlpComparisonResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpComparisonResult.Controls.Add(this.tlpComparisonButton, 0, 0);
            this.tlpComparisonResult.Controls.Add(this.tabComparisonText, 0, 1);
            this.tlpComparisonResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpComparisonResult.Location = new System.Drawing.Point(0, 0);
            this.tlpComparisonResult.Margin = new System.Windows.Forms.Padding(0);
            this.tlpComparisonResult.Name = "tlpComparisonResult";
            this.tlpComparisonResult.RowCount = 2;
            this.tlpComparisonResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpComparisonResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpComparisonResult.Size = new System.Drawing.Size(800, 420);
            this.tlpComparisonResult.TabIndex = 0;
            // 
            // tlpComparisonButton
            // 
            this.tlpComparisonButton.ColumnCount = 7;
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpComparisonButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpComparisonButton.Controls.Add(this.bComparison, 2, 1);
            this.tlpComparisonButton.Controls.Add(this.bComparison_Change, 5, 1);
            this.tlpComparisonButton.Controls.Add(this.bComparison_Reset, 4, 1);
            this.tlpComparisonButton.Controls.Add(this.bComparison_Clean, 6, 1);
            this.tlpComparisonButton.Controls.Add(this.nudComparison_DuplicateNum, 1, 1);
            this.tlpComparisonButton.Controls.Add(this.ddlComparisonType, 0, 1);
            this.tlpComparisonButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpComparisonButton.Location = new System.Drawing.Point(0, 0);
            this.tlpComparisonButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpComparisonButton.Name = "tlpComparisonButton";
            this.tlpComparisonButton.RowCount = 3;
            this.tlpComparisonButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpComparisonButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpComparisonButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpComparisonButton.Size = new System.Drawing.Size(800, 60);
            this.tlpComparisonButton.TabIndex = 2;
            // 
            // bComparison
            // 
            this.bComparison.BackExtend = "135, #6253E1, #04BEFE";
            this.bComparison.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bComparison.IconSvg = "ScanOutlined";
            this.bComparison.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bComparison.LoadingWaveCount = 6;
            this.bComparison.LoadingWaveSize = 6;
            this.bComparison.LoadingWaveValue = 0.6F;
            this.bComparison.LoadingWaveVertical = true;
            this.bComparison.Location = new System.Drawing.Point(359, 7);
            this.bComparison.Name = "bComparison";
            this.bComparison.Size = new System.Drawing.Size(144, 46);
            this.bComparison.TabIndex = 5;
            this.bComparison.Text = "分析文本";
            this.bComparison.Type = AntdUI.TTypeMini.Info;
            this.bComparison.Click += new System.EventHandler(this.bComparison_Click);
            // 
            // bComparison_Change
            // 
            this.bComparison_Change.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bComparison_Change.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bComparison_Change.IconSvg = "SwapOutlined";
            this.bComparison_Change.Location = new System.Drawing.Point(617, 7);
            this.bComparison_Change.Name = "bComparison_Change";
            this.bComparison_Change.Size = new System.Drawing.Size(87, 46);
            this.bComparison_Change.TabIndex = 4;
            this.bComparison_Change.Text = "交换";
            this.bComparison_Change.Type = AntdUI.TTypeMini.Primary;
            this.bComparison_Change.Click += new System.EventHandler(this.bComparison_Change_Click);
            // 
            // bComparison_Reset
            // 
            this.bComparison_Reset.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bComparison_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bComparison_Reset.IconSvg = "RollbackOutlined";
            this.bComparison_Reset.Location = new System.Drawing.Point(524, 7);
            this.bComparison_Reset.Name = "bComparison_Reset";
            this.bComparison_Reset.Size = new System.Drawing.Size(87, 46);
            this.bComparison_Reset.TabIndex = 3;
            this.bComparison_Reset.Text = "还原";
            this.bComparison_Reset.Type = AntdUI.TTypeMini.Primary;
            this.bComparison_Reset.Click += new System.EventHandler(this.bComparison_Reset_Click);
            // 
            // bComparison_Clean
            // 
            this.bComparison_Clean.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bComparison_Clean.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bComparison_Clean.IconSvg = "ClearOutlined";
            this.bComparison_Clean.Location = new System.Drawing.Point(710, 7);
            this.bComparison_Clean.Name = "bComparison_Clean";
            this.bComparison_Clean.Size = new System.Drawing.Size(87, 46);
            this.bComparison_Clean.TabIndex = 2;
            this.bComparison_Clean.Text = "清空";
            this.bComparison_Clean.Type = AntdUI.TTypeMini.Primary;
            this.bComparison_Clean.Click += new System.EventHandler(this.bComparison_Clean_Click);
            // 
            // nudComparison_DuplicateNum
            // 
            this.nudComparison_DuplicateNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudComparison_DuplicateNum.Location = new System.Drawing.Point(203, 7);
            this.nudComparison_DuplicateNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudComparison_DuplicateNum.Name = "nudComparison_DuplicateNum";
            this.nudComparison_DuplicateNum.PrefixText = "查重位数:";
            this.nudComparison_DuplicateNum.SelectionStart = 1;
            this.nudComparison_DuplicateNum.Size = new System.Drawing.Size(150, 46);
            this.nudComparison_DuplicateNum.SuffixText = "";
            this.nudComparison_DuplicateNum.TabIndex = 6;
            this.nudComparison_DuplicateNum.Text = "2";
            this.nudComparison_DuplicateNum.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // ddlComparisonType
            // 
            this.ddlComparisonType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlComparisonType.List = true;
            this.ddlComparisonType.Location = new System.Drawing.Point(3, 7);
            this.ddlComparisonType.Name = "ddlComparisonType";
            this.ddlComparisonType.PlaceholderText = "请选择";
            this.ddlComparisonType.Size = new System.Drawing.Size(194, 46);
            this.ddlComparisonType.TabIndex = 7;
            this.ddlComparisonType.SelectedIndexChanged += new AntdUI.IntEventHandler(this.ddlComparisonType_SelectedIndexChanged);
            // 
            // tabComparisonText
            // 
            this.tabComparisonText.Controls.Add(this.tpComparison);
            this.tabComparisonText.Controls.Add(this.tpDuplicate);
            this.tabComparisonText.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabComparisonText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabComparisonText.Location = new System.Drawing.Point(3, 63);
            this.tabComparisonText.Name = "tabComparisonText";
            this.tabComparisonText.Pages.Add(this.tpComparison);
            this.tabComparisonText.Pages.Add(this.tpDuplicate);
            this.tabComparisonText.SelectedIndex = 1;
            this.tabComparisonText.Size = new System.Drawing.Size(794, 354);
            this.tabComparisonText.Style = styleLine1;
            this.tabComparisonText.TabIndex = 3;
            // 
            // tpComparison
            // 
            this.tpComparison.Controls.Add(this.tComparison);
            this.tpComparison.Location = new System.Drawing.Point(0, 0);
            this.tpComparison.Name = "tpComparison";
            this.tpComparison.Size = new System.Drawing.Size(0, 0);
            this.tpComparison.TabIndex = 0;
            this.tpComparison.Text = "tpComparison";
            // 
            // tpDuplicate
            // 
            this.tpDuplicate.Controls.Add(this.tDuplicate);
            this.tpDuplicate.Location = new System.Drawing.Point(3, 33);
            this.tpDuplicate.Name = "tpDuplicate";
            this.tpDuplicate.Size = new System.Drawing.Size(788, 318);
            this.tpDuplicate.TabIndex = 1;
            this.tpDuplicate.Text = "tpDuplicate";
            // 
            // tComparison
            // 
            this.tComparison.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tComparison.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tComparison.Gap = 12;
            this.tComparison.Location = new System.Drawing.Point(0, 0);
            this.tComparison.Name = "tComparison";
            this.tComparison.Size = new System.Drawing.Size(0, 0);
            this.tComparison.TabIndex = 4;
            this.tComparison.Text = "table1";
            // 
            // tDuplicate
            // 
            this.tDuplicate.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tDuplicate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDuplicate.Gap = 12;
            this.tDuplicate.Location = new System.Drawing.Point(0, 0);
            this.tDuplicate.Name = "tDuplicate";
            this.tDuplicate.Size = new System.Drawing.Size(788, 318);
            this.tDuplicate.TabIndex = 5;
            this.tDuplicate.Text = "table1";
            // 
            // ComparisonText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitterComparison);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ComparisonText";
            this.Size = new System.Drawing.Size(800, 800);
            this.Load += new System.EventHandler(this.ComparisonText_Load);
            this.splitterComparison.Panel1.ResumeLayout(false);
            this.splitterComparison.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterComparison)).EndInit();
            this.splitterComparison.ResumeLayout(false);
            this.tlpComparisonText.ResumeLayout(false);
            this.tlpComparisonResult.ResumeLayout(false);
            this.tlpComparisonButton.ResumeLayout(false);
            this.tlpComparisonButton.PerformLayout();
            this.tabComparisonText.ResumeLayout(false);
            this.tpComparison.ResumeLayout(false);
            this.tpDuplicate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Splitter splitterComparison;
        private System.Windows.Forms.TableLayoutPanel tlpComparisonText;
        private AntdUI.Label lComparison_B;
        private AntdUI.Input txtComparison_B;
        private AntdUI.Input txtComparison_A;
        private AntdUI.Label lComparison_A;
        private System.Windows.Forms.TableLayoutPanel tlpComparisonResult;
        private System.Windows.Forms.TableLayoutPanel tlpComparisonButton;
        private AntdUI.Button bComparison;
        private AntdUI.Button bComparison_Change;
        private AntdUI.Button bComparison_Reset;
        private AntdUI.Button bComparison_Clean;
        private AntdUI.InputNumber nudComparison_DuplicateNum;
        private AntdUI.Select ddlComparisonType;
        private AntdUI.Tabs tabComparisonText;
        private AntdUI.TabPage tpComparison;
        private AntdUI.TabPage tpDuplicate;
        private AntdUI.Table tComparison;
        private AntdUI.Table tDuplicate;
    }
}

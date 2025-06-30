namespace WPE.InjectMode
{
    partial class SearchPacketForm
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchPacketForm));
            this.tlpSearchPacket = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSearch = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbString = new AntdUI.Radio();
            this.rbHex = new AntdUI.Radio();
            this.tabSearchType = new AntdUI.Tabs();
            this.tpString = new AntdUI.TabPage();
            this.txtFind = new AntdUI.Input();
            this.tpHex = new AntdUI.TabPage();
            this.hexFind = new Be.Windows.Forms.HexBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbFromHead = new AntdUI.Radio();
            this.rbFromIndex = new AntdUI.Radio();
            this.divider1 = new AntdUI.Divider();
            this.tlpSearchPacket.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabSearchType.SuspendLayout();
            this.tpString.SuspendLayout();
            this.tpHex.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSearchPacket
            // 
            this.tlpSearchPacket.ColumnCount = 1;
            this.tlpSearchPacket.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchPacket.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tlpSearchPacket.Controls.Add(this.tlpButton, 0, 4);
            this.tlpSearchPacket.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlpSearchPacket.Controls.Add(this.tabSearchType, 0, 2);
            this.tlpSearchPacket.Controls.Add(this.divider1, 0, 0);
            this.tlpSearchPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearchPacket.Location = new System.Drawing.Point(0, 0);
            this.tlpSearchPacket.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearchPacket.Name = "tlpSearchPacket";
            this.tlpSearchPacket.RowCount = 5;
            this.tlpSearchPacket.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSearchPacket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSearchPacket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchPacket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSearchPacket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSearchPacket.Size = new System.Drawing.Size(284, 461);
            this.tlpSearchPacket.TabIndex = 3;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Controls.Add(this.bSearch, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 401);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(284, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bSearch
            // 
            this.bSearch.BackExtend = "135, #6253E1, #04BEFE";
            this.bSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearch.IconSvg = "SearchOutlined";
            this.bSearch.Location = new System.Drawing.Point(0, 7);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(144, 46);
            this.bSearch.TabIndex = 0;
            this.bSearch.Text = "查找下一个";
            this.bSearch.Type = AntdUI.TTypeMini.Primary;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(170, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.rbString, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbHex, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 29);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 60);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rbString
            // 
            this.rbString.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbString.Checked = true;
            this.rbString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbString.Location = new System.Drawing.Point(3, 15);
            this.rbString.Name = "rbString";
            this.rbString.Size = new System.Drawing.Size(74, 42);
            this.rbString.TabIndex = 0;
            this.rbString.Text = "文本";
            this.rbString.CheckedChanged += new AntdUI.BoolEventHandler(this.rbString_CheckedChanged);
            // 
            // rbHex
            // 
            this.rbHex.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbHex.Location = new System.Drawing.Point(83, 15);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(106, 42);
            this.rbHex.TabIndex = 1;
            this.rbHex.Text = "十六进制";
            this.rbHex.CheckedChanged += new AntdUI.BoolEventHandler(this.rbHex_CheckedChanged);
            // 
            // tabSearchType
            // 
            this.tabSearchType.Controls.Add(this.tpString);
            this.tabSearchType.Controls.Add(this.tpHex);
            this.tabSearchType.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabSearchType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSearchType.Location = new System.Drawing.Point(3, 92);
            this.tabSearchType.Name = "tabSearchType";
            this.tabSearchType.Pages.Add(this.tpString);
            this.tabSearchType.Pages.Add(this.tpHex);
            this.tabSearchType.SelectedIndex = 1;
            this.tabSearchType.Size = new System.Drawing.Size(278, 246);
            this.tabSearchType.Style = styleLine1;
            this.tabSearchType.TabIndex = 18;
            this.tabSearchType.Text = "tabs1";
            // 
            // tpString
            // 
            this.tpString.Controls.Add(this.txtFind);
            this.tpString.Location = new System.Drawing.Point(0, 0);
            this.tpString.Name = "tpString";
            this.tpString.Size = new System.Drawing.Size(0, 0);
            this.tpString.TabIndex = 0;
            this.tpString.Text = "String";
            // 
            // txtFind
            // 
            this.txtFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFind.Location = new System.Drawing.Point(0, 0);
            this.txtFind.Multiline = true;
            this.txtFind.Name = "txtFind";
            this.txtFind.PlaceholderText = "请输入字符和数字";
            this.txtFind.Size = new System.Drawing.Size(0, 0);
            this.txtFind.TabIndex = 0;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // tpHex
            // 
            this.tpHex.Controls.Add(this.hexFind);
            this.tpHex.Location = new System.Drawing.Point(3, 33);
            this.tpHex.Name = "tpHex";
            this.tpHex.Size = new System.Drawing.Size(272, 210);
            this.tpHex.TabIndex = 1;
            this.tpHex.Text = "Hex";
            // 
            // hexFind
            // 
            this.hexFind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hexFind.ColumnInfoVisible = true;
            this.hexFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexFind.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hexFind.Location = new System.Drawing.Point(0, 0);
            this.hexFind.Name = "hexFind";
            this.hexFind.Padding = new System.Windows.Forms.Padding(3);
            this.hexFind.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexFind.Size = new System.Drawing.Size(272, 210);
            this.hexFind.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.rbFromHead, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbFromIndex, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 341);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(284, 60);
            this.tableLayoutPanel2.TabIndex = 19;
            // 
            // rbFromHead
            // 
            this.rbFromHead.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFromHead.Checked = true;
            this.rbFromHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromHead.Location = new System.Drawing.Point(33, 9);
            this.rbFromHead.Name = "rbFromHead";
            this.rbFromHead.Size = new System.Drawing.Size(106, 42);
            this.rbFromHead.TabIndex = 2;
            this.rbFromHead.Text = "从头开始";
            // 
            // rbFromIndex
            // 
            this.rbFromIndex.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFromIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromIndex.Location = new System.Drawing.Point(145, 9);
            this.rbFromIndex.Name = "rbFromIndex";
            this.rbFromIndex.Size = new System.Drawing.Size(106, 42);
            this.rbFromIndex.TabIndex = 3;
            this.rbFromIndex.Text = "向下搜索";
            // 
            // divider1
            // 
            this.divider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider1.Location = new System.Drawing.Point(3, 3);
            this.divider1.Name = "divider1";
            this.divider1.Orientation = AntdUI.TOrientation.Left;
            this.divider1.Size = new System.Drawing.Size(278, 23);
            this.divider1.TabIndex = 20;
            this.divider1.Text = "查找封包";
            // 
            // SearchPacketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.tlpSearchPacket);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SearchPacketForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SearchPacketForm";
            this.Load += new System.EventHandler(this.SearchPacketForm_Load);
            this.tlpSearchPacket.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabSearchType.ResumeLayout(false);
            this.tpString.ResumeLayout(false);
            this.tpHex.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpSearchPacket;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Radio rbString;
        private AntdUI.Radio rbHex;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSearch;
        private AntdUI.Button bExit;
        private AntdUI.Tabs tabSearchType;
        private AntdUI.TabPage tpString;
        private AntdUI.Input txtFind;
        private AntdUI.TabPage tpHex;
        private Be.Windows.Forms.HexBox hexFind;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Radio rbFromHead;
        private AntdUI.Radio rbFromIndex;
        private AntdUI.Divider divider1;
    }
}
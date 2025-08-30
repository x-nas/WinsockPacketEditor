namespace WinsockPacketEditor
{
    partial class SearchPacket
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
            AntdUI.Tabs.StyleLine styleLine3 = new AntdUI.Tabs.StyleLine();
            this.tlpSearchSettings = new System.Windows.Forms.TableLayoutPanel();
            this.tabSearchType = new AntdUI.Tabs();
            this.tpString = new AntdUI.TabPage();
            this.txtFind = new AntdUI.Input();
            this.tpHex = new AntdUI.TabPage();
            this.pHex = new AntdUI.Panel();
            this.hexFind = new Be.Windows.Forms.HexBox();
            this.pSearchType = new AntdUI.Panel();
            this.tlpSearchType = new System.Windows.Forms.TableLayoutPanel();
            this.rbString = new AntdUI.Radio();
            this.rbHex = new AntdUI.Radio();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bExit = new AntdUI.Button();
            this.bSearch = new AntdUI.Button();
            this.pSearchFrom = new AntdUI.Panel();
            this.tlpSearchFrom = new System.Windows.Forms.TableLayoutPanel();
            this.rbFromIndex = new AntdUI.Radio();
            this.rbFromHead = new AntdUI.Radio();
            this.tlpSearchSettings.SuspendLayout();
            this.tabSearchType.SuspendLayout();
            this.tpString.SuspendLayout();
            this.tpHex.SuspendLayout();
            this.pHex.SuspendLayout();
            this.pSearchType.SuspendLayout();
            this.tlpSearchType.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.pSearchFrom.SuspendLayout();
            this.tlpSearchFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSearchSettings
            // 
            this.tlpSearchSettings.ColumnCount = 4;
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpSearchSettings.Controls.Add(this.tabSearchType, 1, 0);
            this.tlpSearchSettings.Controls.Add(this.pSearchType, 0, 0);
            this.tlpSearchSettings.Controls.Add(this.tlpButton, 3, 0);
            this.tlpSearchSettings.Controls.Add(this.pSearchFrom, 2, 0);
            this.tlpSearchSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearchSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSearchSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearchSettings.Name = "tlpSearchSettings";
            this.tlpSearchSettings.RowCount = 1;
            this.tlpSearchSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchSettings.Size = new System.Drawing.Size(1000, 100);
            this.tlpSearchSettings.TabIndex = 3;
            // 
            // tabSearchType
            // 
            this.tabSearchType.Controls.Add(this.tpString);
            this.tabSearchType.Controls.Add(this.tpHex);
            this.tabSearchType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabSearchType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSearchType.Location = new System.Drawing.Point(203, 3);
            this.tabSearchType.Name = "tabSearchType";
            this.tabSearchType.Pages.Add(this.tpString);
            this.tabSearchType.Pages.Add(this.tpHex);
            this.tabSearchType.Size = new System.Drawing.Size(494, 94);
            this.tabSearchType.Style = styleLine3;
            this.tabSearchType.TabIndex = 19;
            this.tabSearchType.Text = "tabs1";
            // 
            // tpString
            // 
            this.tpString.Controls.Add(this.txtFind);
            this.tpString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpString.Location = new System.Drawing.Point(3, 33);
            this.tpString.Name = "tpString";
            this.tpString.Size = new System.Drawing.Size(488, 58);
            this.tpString.TabIndex = 0;
            this.tpString.Text = "String";
            // 
            // txtFind
            // 
            this.txtFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFind.LocalizationPlaceholderText = "Input.Text";
            this.txtFind.Location = new System.Drawing.Point(0, 0);
            this.txtFind.Multiline = true;
            this.txtFind.Name = "txtFind";
            this.txtFind.PlaceholderText = "请输入文本";
            this.txtFind.Size = new System.Drawing.Size(488, 58);
            this.txtFind.TabIndex = 0;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // tpHex
            // 
            this.tpHex.Controls.Add(this.pHex);
            this.tpHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpHex.Location = new System.Drawing.Point(3, 33);
            this.tpHex.Name = "tpHex";
            this.tpHex.Padding = new System.Windows.Forms.Padding(3);
            this.tpHex.Size = new System.Drawing.Size(488, 58);
            this.tpHex.TabIndex = 1;
            this.tpHex.Text = "Hex";
            // 
            // pHex
            // 
            this.pHex.BorderWidth = 1F;
            this.pHex.Controls.Add(this.hexFind);
            this.pHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pHex.Location = new System.Drawing.Point(3, 3);
            this.pHex.Name = "pHex";
            this.pHex.Padding = new System.Windows.Forms.Padding(3);
            this.pHex.Size = new System.Drawing.Size(482, 52);
            this.pHex.TabIndex = 0;
            // 
            // hexFind
            // 
            this.hexFind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hexFind.ColumnInfoVisible = true;
            this.hexFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexFind.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hexFind.Location = new System.Drawing.Point(4, 4);
            this.hexFind.Name = "hexFind";
            this.hexFind.Padding = new System.Windows.Forms.Padding(3);
            this.hexFind.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexFind.Size = new System.Drawing.Size(474, 44);
            this.hexFind.TabIndex = 1;
            // 
            // pSearchType
            // 
            this.pSearchType.BorderWidth = 1F;
            this.pSearchType.Controls.Add(this.tlpSearchType);
            this.pSearchType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSearchType.Location = new System.Drawing.Point(6, 6);
            this.pSearchType.Margin = new System.Windows.Forms.Padding(6);
            this.pSearchType.Name = "pSearchType";
            this.pSearchType.Padding = new System.Windows.Forms.Padding(3);
            this.pSearchType.Size = new System.Drawing.Size(188, 88);
            this.pSearchType.TabIndex = 22;
            this.pSearchType.Text = "panel1";
            // 
            // tlpSearchType
            // 
            this.tlpSearchType.ColumnCount = 2;
            this.tlpSearchType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchType.Controls.Add(this.rbString, 0, 0);
            this.tlpSearchType.Controls.Add(this.rbHex, 0, 1);
            this.tlpSearchType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearchType.Location = new System.Drawing.Point(4, 4);
            this.tlpSearchType.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearchType.Name = "tlpSearchType";
            this.tlpSearchType.RowCount = 3;
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSearchType.Size = new System.Drawing.Size(180, 80);
            this.tlpSearchType.TabIndex = 21;
            // 
            // rbString
            // 
            this.rbString.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbString.Checked = true;
            this.rbString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbString.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbString.LocalizationText = "SearchPacketForm.FindText";
            this.rbString.Location = new System.Drawing.Point(1, 1);
            this.rbString.Margin = new System.Windows.Forms.Padding(1);
            this.rbString.Name = "rbString";
            this.rbString.Size = new System.Drawing.Size(95, 39);
            this.rbString.TabIndex = 3;
            this.rbString.Text = "查找文本";
            this.rbString.CheckedChanged += new AntdUI.BoolEventHandler(this.rbString_CheckedChanged);
            // 
            // rbHex
            // 
            this.rbHex.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbHex.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbHex.LocalizationText = "SearchPacketForm.FindHex";
            this.rbHex.Location = new System.Drawing.Point(1, 42);
            this.rbHex.Margin = new System.Windows.Forms.Padding(1);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(123, 35);
            this.rbHex.TabIndex = 2;
            this.rbHex.Text = "查找十六进制";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 1;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Controls.Add(this.bExit, 0, 1);
            this.tlpButton.Controls.Add(this.bSearch, 0, 0);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(850, 0);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 2;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(150, 100);
            this.tlpButton.TabIndex = 23;
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(3, 53);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(144, 44);
            this.bExit.TabIndex = 9;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bSearch
            // 
            this.bSearch.BackExtend = "135, #6253E1, #04BEFE";
            this.bSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearch.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bSearch.IconSvg = "SearchOutlined";
            this.bSearch.LocalizationText = "SearchPacketForm.FindNext";
            this.bSearch.Location = new System.Drawing.Point(3, 3);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(144, 44);
            this.bSearch.TabIndex = 8;
            this.bSearch.Text = "查找下一个";
            this.bSearch.Type = AntdUI.TTypeMini.Primary;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // pSearchFrom
            // 
            this.pSearchFrom.BorderWidth = 1F;
            this.pSearchFrom.Controls.Add(this.tlpSearchFrom);
            this.pSearchFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSearchFrom.Location = new System.Drawing.Point(706, 6);
            this.pSearchFrom.Margin = new System.Windows.Forms.Padding(6);
            this.pSearchFrom.Name = "pSearchFrom";
            this.pSearchFrom.Padding = new System.Windows.Forms.Padding(3);
            this.pSearchFrom.Size = new System.Drawing.Size(138, 88);
            this.pSearchFrom.TabIndex = 24;
            // 
            // tlpSearchFrom
            // 
            this.tlpSearchFrom.ColumnCount = 2;
            this.tlpSearchFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchFrom.Controls.Add(this.rbFromIndex, 0, 1);
            this.tlpSearchFrom.Controls.Add(this.rbFromHead, 0, 0);
            this.tlpSearchFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearchFrom.Location = new System.Drawing.Point(4, 4);
            this.tlpSearchFrom.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearchFrom.Name = "tlpSearchFrom";
            this.tlpSearchFrom.RowCount = 3;
            this.tlpSearchFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSearchFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSearchFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSearchFrom.Size = new System.Drawing.Size(130, 80);
            this.tlpSearchFrom.TabIndex = 22;
            // 
            // rbFromIndex
            // 
            this.rbFromIndex.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFromIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromIndex.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbFromIndex.LocalizationText = "SearchPacketForm.SeekDown";
            this.rbFromIndex.Location = new System.Drawing.Point(1, 38);
            this.rbFromIndex.Margin = new System.Windows.Forms.Padding(1);
            this.rbFromIndex.Name = "rbFromIndex";
            this.rbFromIndex.Size = new System.Drawing.Size(95, 35);
            this.rbFromIndex.TabIndex = 5;
            this.rbFromIndex.Text = "向下搜索";
            // 
            // rbFromHead
            // 
            this.rbFromHead.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFromHead.Checked = true;
            this.rbFromHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromHead.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbFromHead.LocalizationText = "SearchPacketForm.FromScratch";
            this.rbFromHead.Location = new System.Drawing.Point(1, 1);
            this.rbFromHead.Margin = new System.Windows.Forms.Padding(1);
            this.rbFromHead.Name = "rbFromHead";
            this.rbFromHead.Size = new System.Drawing.Size(95, 35);
            this.rbFromHead.TabIndex = 4;
            this.rbFromHead.Text = "从头开始";
            // 
            // SearchPacket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSearchSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SearchPacket";
            this.Size = new System.Drawing.Size(1000, 100);
            this.Load += new System.EventHandler(this.SearchPacket_Load);
            this.tlpSearchSettings.ResumeLayout(false);
            this.tabSearchType.ResumeLayout(false);
            this.tpString.ResumeLayout(false);
            this.tpHex.ResumeLayout(false);
            this.pHex.ResumeLayout(false);
            this.pSearchType.ResumeLayout(false);
            this.tlpSearchType.ResumeLayout(false);
            this.tlpSearchType.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.pSearchFrom.ResumeLayout(false);
            this.tlpSearchFrom.ResumeLayout(false);
            this.tlpSearchFrom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSearchSettings;
        private AntdUI.Tabs tabSearchType;
        private AntdUI.TabPage tpString;
        private AntdUI.Input txtFind;
        private AntdUI.TabPage tpHex;
        private AntdUI.Panel pHex;
        private Be.Windows.Forms.HexBox hexFind;
        private AntdUI.Panel pSearchType;
        private System.Windows.Forms.TableLayoutPanel tlpSearchType;
        private AntdUI.Radio rbString;
        private AntdUI.Radio rbHex;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bExit;
        private AntdUI.Button bSearch;
        private AntdUI.Panel pSearchFrom;
        private System.Windows.Forms.TableLayoutPanel tlpSearchFrom;
        private AntdUI.Radio rbFromIndex;
        private AntdUI.Radio rbFromHead;
    }
}

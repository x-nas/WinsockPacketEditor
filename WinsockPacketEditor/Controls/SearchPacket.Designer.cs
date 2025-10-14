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
            this.tlpSearchSettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bExit = new AntdUI.Button();
            this.bSearch = new AntdUI.Button();
            this.rbFromIndex = new AntdUI.Radio();
            this.rbFromHead = new AntdUI.Radio();
            this.txtFind = new AntdUI.Input();
            this.tlpSearchType = new WinsockPacketEditor.TableLayoutPanelEx();
            this.rbString = new AntdUI.Radio();
            this.rbHex = new AntdUI.Radio();
            this.tlpSearchSettings.SuspendLayout();
            this.tlpSearchType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSearchSettings
            // 
            this.tlpSearchSettings.ColumnCount = 6;
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchSettings.Controls.Add(this.bExit, 5, 1);
            this.tlpSearchSettings.Controls.Add(this.bSearch, 4, 1);
            this.tlpSearchSettings.Controls.Add(this.rbFromIndex, 3, 1);
            this.tlpSearchSettings.Controls.Add(this.rbFromHead, 2, 1);
            this.tlpSearchSettings.Controls.Add(this.txtFind, 1, 1);
            this.tlpSearchSettings.Controls.Add(this.tlpSearchType, 0, 1);
            this.tlpSearchSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearchSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSearchSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearchSettings.Name = "tlpSearchSettings";
            this.tlpSearchSettings.RowCount = 3;
            this.tlpSearchSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearchSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpSearchSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearchSettings.Size = new System.Drawing.Size(1100, 50);
            this.tlpSearchSettings.TabIndex = 3;
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(1035, 4);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 41);
            this.bExit.TabIndex = 31;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bSearch
            // 
            this.bSearch.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSearch.BackExtend = "135, #6253E1, #04BEFE";
            this.bSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearch.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bSearch.IconSvg = "SearchOutlined";
            this.bSearch.LocalizationText = "SearchPacketForm.FindNext";
            this.bSearch.Location = new System.Drawing.Point(932, 4);
            this.bSearch.Margin = new System.Windows.Forms.Padding(2);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(99, 41);
            this.bSearch.TabIndex = 30;
            this.bSearch.Text = "查找下一个";
            this.bSearch.Type = AntdUI.TTypeMini.Primary;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // rbFromIndex
            // 
            this.rbFromIndex.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFromIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromIndex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbFromIndex.LocalizationText = "SearchPacketForm.SeekDown";
            this.rbFromIndex.Location = new System.Drawing.Point(849, 3);
            this.rbFromIndex.Margin = new System.Windows.Forms.Padding(1);
            this.rbFromIndex.Name = "rbFromIndex";
            this.rbFromIndex.Size = new System.Drawing.Size(80, 43);
            this.rbFromIndex.TabIndex = 29;
            this.rbFromIndex.Text = "向下搜索";
            // 
            // rbFromHead
            // 
            this.rbFromHead.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFromHead.Checked = true;
            this.rbFromHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromHead.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbFromHead.LocalizationText = "SearchPacketForm.FromScratch";
            this.rbFromHead.Location = new System.Drawing.Point(767, 3);
            this.rbFromHead.Margin = new System.Windows.Forms.Padding(1);
            this.rbFromHead.Name = "rbFromHead";
            this.rbFromHead.Size = new System.Drawing.Size(80, 43);
            this.rbFromHead.TabIndex = 28;
            this.rbFromHead.Text = "从头开始";
            // 
            // txtFind
            // 
            this.txtFind.AllowClear = true;
            this.txtFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFind.LocalizationPlaceholderText = "Input.Regex";
            this.txtFind.Location = new System.Drawing.Point(202, 4);
            this.txtFind.Margin = new System.Windows.Forms.Padding(2);
            this.txtFind.Name = "txtFind";
            this.txtFind.PlaceholderText = "请输入正则表达式";
            this.txtFind.Size = new System.Drawing.Size(562, 41);
            this.txtFind.TabIndex = 25;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // tlpSearchType
            // 
            this.tlpSearchType.ColumnCount = 3;
            this.tlpSearchType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSearchType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchType.Controls.Add(this.rbHex, 1, 0);
            this.tlpSearchType.Controls.Add(this.rbString, 0, 0);
            this.tlpSearchType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearchType.Location = new System.Drawing.Point(3, 5);
            this.tlpSearchType.Name = "tlpSearchType";
            this.tlpSearchType.RowCount = 1;
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSearchType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSearchType.Size = new System.Drawing.Size(194, 39);
            this.tlpSearchType.TabIndex = 32;
            // 
            // rbString
            // 
            this.rbString.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbString.Checked = true;
            this.rbString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbString.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbString.LocalizationText = "SearchPacketForm.FindText";
            this.rbString.Location = new System.Drawing.Point(1, 1);
            this.rbString.Margin = new System.Windows.Forms.Padding(1);
            this.rbString.Name = "rbString";
            this.rbString.Size = new System.Drawing.Size(80, 37);
            this.rbString.TabIndex = 27;
            this.rbString.Text = "查找文本";
            // 
            // rbHex
            // 
            this.rbHex.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbHex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbHex.LocalizationText = "SearchPacketForm.FindHex";
            this.rbHex.Location = new System.Drawing.Point(83, 1);
            this.rbHex.Margin = new System.Windows.Forms.Padding(1);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(104, 37);
            this.rbHex.TabIndex = 28;
            this.rbHex.Text = "查找十六进制";
            // 
            // SearchPacket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSearchSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SearchPacket";
            this.Size = new System.Drawing.Size(1100, 50);
            this.Load += new System.EventHandler(this.SearchPacket_Load);
            this.tlpSearchSettings.ResumeLayout(false);
            this.tlpSearchSettings.PerformLayout();
            this.tlpSearchType.ResumeLayout(false);
            this.tlpSearchType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpSearchSettings;
        private AntdUI.Input txtFind;
        private AntdUI.Button bExit;
        private AntdUI.Button bSearch;
        private AntdUI.Radio rbFromIndex;
        private AntdUI.Radio rbFromHead;
        private TableLayoutPanelEx tlpSearchType;
        private AntdUI.Radio rbHex;
        private AntdUI.Radio rbString;
    }
}

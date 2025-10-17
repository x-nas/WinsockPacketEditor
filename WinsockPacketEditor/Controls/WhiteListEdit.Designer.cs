namespace WinsockPacketEditor
{
    partial class WhiteListEdit
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
            this.tlpWhiteListAdd = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpWhiteListAddInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.rbSingleIP = new AntdUI.Radio();
            this.rbIPRange = new AntdUI.Radio();
            this.txtSingleIP = new AntdUI.Input();
            this.txtIPRangeFrom = new AntdUI.Input();
            this.txtIPRangeTo = new AntdUI.Input();
            this.lDash = new AntdUI.Label();
            this.tlpWhiteListAdd.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpWhiteListAddInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpWhiteListAdd
            // 
            this.tlpWhiteListAdd.ColumnCount = 1;
            this.tlpWhiteListAdd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWhiteListAdd.Controls.Add(this.tlpButton, 0, 1);
            this.tlpWhiteListAdd.Controls.Add(this.tlpWhiteListAddInfo, 0, 0);
            this.tlpWhiteListAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWhiteListAdd.Location = new System.Drawing.Point(0, 0);
            this.tlpWhiteListAdd.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWhiteListAdd.Name = "tlpWhiteListAdd";
            this.tlpWhiteListAdd.RowCount = 2;
            this.tlpWhiteListAdd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWhiteListAdd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpWhiteListAdd.Size = new System.Drawing.Size(400, 200);
            this.tlpWhiteListAdd.TabIndex = 0;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 150);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(400, 50);
            this.tlpButton.TabIndex = 5;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(128, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
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
            this.bExit.Location = new System.Drawing.Point(209, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpWhiteListAddInfo
            // 
            this.tlpWhiteListAddInfo.ColumnCount = 4;
            this.tlpWhiteListAddInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteListAddInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListAddInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteListAddInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListAddInfo.Controls.Add(this.rbSingleIP, 0, 1);
            this.tlpWhiteListAddInfo.Controls.Add(this.rbIPRange, 0, 2);
            this.tlpWhiteListAddInfo.Controls.Add(this.txtSingleIP, 1, 1);
            this.tlpWhiteListAddInfo.Controls.Add(this.txtIPRangeFrom, 1, 2);
            this.tlpWhiteListAddInfo.Controls.Add(this.txtIPRangeTo, 3, 2);
            this.tlpWhiteListAddInfo.Controls.Add(this.lDash, 2, 2);
            this.tlpWhiteListAddInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWhiteListAddInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpWhiteListAddInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWhiteListAddInfo.Name = "tlpWhiteListAddInfo";
            this.tlpWhiteListAddInfo.RowCount = 4;
            this.tlpWhiteListAddInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListAddInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteListAddInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteListAddInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListAddInfo.Size = new System.Drawing.Size(400, 150);
            this.tlpWhiteListAddInfo.TabIndex = 6;
            // 
            // rbSingleIP
            // 
            this.rbSingleIP.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSingleIP.Checked = true;
            this.rbSingleIP.Location = new System.Drawing.Point(3, 40);
            this.rbSingleIP.Name = "rbSingleIP";
            this.rbSingleIP.Size = new System.Drawing.Size(74, 32);
            this.rbSingleIP.TabIndex = 0;
            this.rbSingleIP.Text = "单个IP :";
            this.rbSingleIP.CheckedChanged += new AntdUI.BoolEventHandler(this.rbSingleIP_CheckedChanged);
            // 
            // rbIPRange
            // 
            this.rbIPRange.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbIPRange.Location = new System.Drawing.Point(3, 78);
            this.rbIPRange.Name = "rbIPRange";
            this.rbIPRange.Size = new System.Drawing.Size(74, 32);
            this.rbIPRange.TabIndex = 1;
            this.rbIPRange.Text = "IP范围 :";
            this.rbIPRange.CheckedChanged += new AntdUI.BoolEventHandler(this.rbIPRange_CheckedChanged);
            // 
            // txtSingleIP
            // 
            this.txtSingleIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSingleIP.Location = new System.Drawing.Point(83, 40);
            this.txtSingleIP.Name = "txtSingleIP";
            this.txtSingleIP.Size = new System.Drawing.Size(150, 32);
            this.txtSingleIP.TabIndex = 2;
            this.txtSingleIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIPRangeFrom
            // 
            this.txtIPRangeFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPRangeFrom.Location = new System.Drawing.Point(83, 78);
            this.txtIPRangeFrom.Name = "txtIPRangeFrom";
            this.txtIPRangeFrom.Size = new System.Drawing.Size(150, 32);
            this.txtIPRangeFrom.TabIndex = 3;
            this.txtIPRangeFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIPRangeTo
            // 
            this.txtIPRangeTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPRangeTo.Location = new System.Drawing.Point(247, 78);
            this.txtIPRangeTo.Name = "txtIPRangeTo";
            this.txtIPRangeTo.Size = new System.Drawing.Size(150, 32);
            this.txtIPRangeTo.TabIndex = 4;
            this.txtIPRangeTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lDash
            // 
            this.lDash.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDash.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDash.Location = new System.Drawing.Point(237, 76);
            this.lDash.Margin = new System.Windows.Forms.Padding(1);
            this.lDash.Name = "lDash";
            this.lDash.Size = new System.Drawing.Size(6, 36);
            this.lDash.TabIndex = 5;
            this.lDash.Text = "-";
            this.lDash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WhiteListAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpWhiteListAdd);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WhiteListAdd";
            this.Size = new System.Drawing.Size(400, 200);
            this.Load += new System.EventHandler(this.WhiteListAdd_Load);
            this.tlpWhiteListAdd.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpWhiteListAddInfo.ResumeLayout(false);
            this.tlpWhiteListAddInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpWhiteListAdd;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpWhiteListAddInfo;
        private AntdUI.Radio rbSingleIP;
        private AntdUI.Radio rbIPRange;
        private AntdUI.Input txtSingleIP;
        private AntdUI.Input txtIPRangeFrom;
        private AntdUI.Input txtIPRangeTo;
        private AntdUI.Label lDash;
    }
}

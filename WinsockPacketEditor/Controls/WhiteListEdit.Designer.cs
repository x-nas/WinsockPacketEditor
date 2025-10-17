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
            this.tlpWhiteList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpWhiteListInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dtpExpiryTime = new AntdUI.DatePicker();
            this.cbExpiryTime = new AntdUI.Checkbox();
            this.rbSingleIP = new AntdUI.Radio();
            this.rbIPRange = new AntdUI.Radio();
            this.txtSingleIP = new AntdUI.Input();
            this.txtIPRangeFrom = new AntdUI.Input();
            this.txtIPRangeTo = new AntdUI.Input();
            this.lDash = new AntdUI.Label();
            this.tlpWhiteList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpWhiteListInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpWhiteList
            // 
            this.tlpWhiteList.ColumnCount = 1;
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWhiteList.Controls.Add(this.tlpButton, 0, 1);
            this.tlpWhiteList.Controls.Add(this.tlpWhiteListInfo, 0, 0);
            this.tlpWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWhiteList.Location = new System.Drawing.Point(0, 0);
            this.tlpWhiteList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWhiteList.Name = "tlpWhiteList";
            this.tlpWhiteList.RowCount = 2;
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpWhiteList.Size = new System.Drawing.Size(500, 200);
            this.tlpWhiteList.TabIndex = 0;
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
            this.tlpButton.Size = new System.Drawing.Size(500, 50);
            this.tlpButton.TabIndex = 5;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(178, 6);
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
            this.bExit.Location = new System.Drawing.Point(259, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpWhiteListInfo
            // 
            this.tlpWhiteListInfo.ColumnCount = 4;
            this.tlpWhiteListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListInfo.Controls.Add(this.dtpExpiryTime, 1, 3);
            this.tlpWhiteListInfo.Controls.Add(this.cbExpiryTime, 0, 3);
            this.tlpWhiteListInfo.Controls.Add(this.rbSingleIP, 0, 1);
            this.tlpWhiteListInfo.Controls.Add(this.rbIPRange, 0, 2);
            this.tlpWhiteListInfo.Controls.Add(this.txtSingleIP, 1, 1);
            this.tlpWhiteListInfo.Controls.Add(this.txtIPRangeFrom, 1, 2);
            this.tlpWhiteListInfo.Controls.Add(this.txtIPRangeTo, 3, 2);
            this.tlpWhiteListInfo.Controls.Add(this.lDash, 2, 2);
            this.tlpWhiteListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWhiteListInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpWhiteListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWhiteListInfo.Name = "tlpWhiteListInfo";
            this.tlpWhiteListInfo.RowCount = 5;
            this.tlpWhiteListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteListInfo.Size = new System.Drawing.Size(500, 150);
            this.tlpWhiteListInfo.TabIndex = 6;
            // 
            // dtpExpiryTime
            // 
            this.dtpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpExpiryTime.Format = "yyyy-MM-dd HH:mm:ss";
            this.dtpExpiryTime.Location = new System.Drawing.Point(93, 96);
            this.dtpExpiryTime.Margin = new System.Windows.Forms.Padding(2);
            this.dtpExpiryTime.MaxDate = new System.DateTime(8888, 12, 31, 0, 0, 0, 0);
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            this.dtpExpiryTime.Size = new System.Drawing.Size(196, 34);
            this.dtpExpiryTime.TabIndex = 23;
            this.dtpExpiryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbExpiryTime
            // 
            this.cbExpiryTime.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbExpiryTime.LocalizationText = "FireWallSetting.ExpiryTime";
            this.cbExpiryTime.Location = new System.Drawing.Point(2, 96);
            this.cbExpiryTime.Margin = new System.Windows.Forms.Padding(2);
            this.cbExpiryTime.Name = "cbExpiryTime";
            this.cbExpiryTime.Size = new System.Drawing.Size(87, 34);
            this.cbExpiryTime.TabIndex = 22;
            this.cbExpiryTime.Text = "过期时间 :";
            this.cbExpiryTime.CheckedChanged += new AntdUI.BoolEventHandler(this.cbExpiryTime_CheckedChanged);
            // 
            // rbSingleIP
            // 
            this.rbSingleIP.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSingleIP.Checked = true;
            this.rbSingleIP.LocalizationText = "FireWallSetting.SingleIP";
            this.rbSingleIP.Location = new System.Drawing.Point(3, 21);
            this.rbSingleIP.Name = "rbSingleIP";
            this.rbSingleIP.Size = new System.Drawing.Size(77, 32);
            this.rbSingleIP.TabIndex = 0;
            this.rbSingleIP.Text = "单个 IP :";
            this.rbSingleIP.CheckedChanged += new AntdUI.BoolEventHandler(this.rbSingleIP_CheckedChanged);
            // 
            // rbIPRange
            // 
            this.rbIPRange.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbIPRange.LocalizationText = "FireWallSetting.IPRange";
            this.rbIPRange.Location = new System.Drawing.Point(3, 59);
            this.rbIPRange.Name = "rbIPRange";
            this.rbIPRange.Size = new System.Drawing.Size(77, 32);
            this.rbIPRange.TabIndex = 1;
            this.rbIPRange.Text = "IP 范围 :";
            this.rbIPRange.CheckedChanged += new AntdUI.BoolEventHandler(this.rbIPRange_CheckedChanged);
            // 
            // txtSingleIP
            // 
            this.txtSingleIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSingleIP.LocalizationPlaceholderText = "";
            this.txtSingleIP.Location = new System.Drawing.Point(94, 21);
            this.txtSingleIP.Name = "txtSingleIP";
            this.txtSingleIP.PlaceholderText = "";
            this.txtSingleIP.Size = new System.Drawing.Size(194, 32);
            this.txtSingleIP.TabIndex = 2;
            this.txtSingleIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIPRangeFrom
            // 
            this.txtIPRangeFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPRangeFrom.LocalizationPlaceholderText = "";
            this.txtIPRangeFrom.Location = new System.Drawing.Point(94, 59);
            this.txtIPRangeFrom.Name = "txtIPRangeFrom";
            this.txtIPRangeFrom.PlaceholderText = "";
            this.txtIPRangeFrom.Size = new System.Drawing.Size(194, 32);
            this.txtIPRangeFrom.TabIndex = 3;
            this.txtIPRangeFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIPRangeTo
            // 
            this.txtIPRangeTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPRangeTo.LocalizationPlaceholderText = "";
            this.txtIPRangeTo.Location = new System.Drawing.Point(302, 59);
            this.txtIPRangeTo.Name = "txtIPRangeTo";
            this.txtIPRangeTo.PlaceholderText = "";
            this.txtIPRangeTo.Size = new System.Drawing.Size(195, 32);
            this.txtIPRangeTo.TabIndex = 4;
            this.txtIPRangeTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lDash
            // 
            this.lDash.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDash.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDash.Location = new System.Drawing.Point(292, 57);
            this.lDash.Margin = new System.Windows.Forms.Padding(1);
            this.lDash.Name = "lDash";
            this.lDash.Size = new System.Drawing.Size(6, 36);
            this.lDash.TabIndex = 5;
            this.lDash.Text = "-";
            this.lDash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WhiteListEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpWhiteList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WhiteListEdit";
            this.Size = new System.Drawing.Size(500, 200);
            this.Load += new System.EventHandler(this.WhiteListAdd_Load);
            this.tlpWhiteList.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpWhiteListInfo.ResumeLayout(false);
            this.tlpWhiteListInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpWhiteList;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpWhiteListInfo;
        private AntdUI.Radio rbSingleIP;
        private AntdUI.Radio rbIPRange;
        private AntdUI.Input txtSingleIP;
        private AntdUI.Input txtIPRangeFrom;
        private AntdUI.Input txtIPRangeTo;
        private AntdUI.Label lDash;
        private AntdUI.Checkbox cbExpiryTime;
        private AntdUI.DatePicker dtpExpiryTime;
    }
}

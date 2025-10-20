namespace WinsockPacketEditor
{
    partial class BatchAccounts
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
            this.tlpBatchAccounts = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.splitterBatchAccounts = new AntdUI.Splitter();
            this.tlpBatchInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lPrefix = new AntdUI.Label();
            this.nudLimitLinks = new AntdUI.InputNumber();
            this.dpExpiryTime = new AntdUI.DatePicker();
            this.cbExpiryTime = new AntdUI.Checkbox();
            this.nudLimitDevices = new AntdUI.InputNumber();
            this.cbLimitDevices = new AntdUI.Checkbox();
            this.nudAccountNum = new AntdUI.InputNumber();
            this.lAccountNum = new AntdUI.Label();
            this.lPasswordLength = new AntdUI.Label();
            this.lAccountRule = new AntdUI.Label();
            this.ddlAccountRule = new AntdUI.Select();
            this.nudPasswordLength = new AntdUI.InputNumber();
            this.bPreview = new AntdUI.Button();
            this.cbLimitLinks = new AntdUI.Checkbox();
            this.txtPrefix = new AntdUI.Input();
            this.tBatchAccounts = new AntdUI.Table();
            this.tlpBatchAccounts.SuspendLayout();
            this.tlpButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterBatchAccounts)).BeginInit();
            this.splitterBatchAccounts.Panel1.SuspendLayout();
            this.splitterBatchAccounts.Panel2.SuspendLayout();
            this.splitterBatchAccounts.SuspendLayout();
            this.tlpBatchInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpBatchAccounts
            // 
            this.tlpBatchAccounts.ColumnCount = 1;
            this.tlpBatchAccounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBatchAccounts.Controls.Add(this.tlpButton, 0, 1);
            this.tlpBatchAccounts.Controls.Add(this.splitterBatchAccounts, 0, 0);
            this.tlpBatchAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBatchAccounts.Location = new System.Drawing.Point(0, 0);
            this.tlpBatchAccounts.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBatchAccounts.Name = "tlpBatchAccounts";
            this.tlpBatchAccounts.RowCount = 2;
            this.tlpBatchAccounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBatchAccounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpBatchAccounts.Size = new System.Drawing.Size(1000, 500);
            this.tlpBatchAccounts.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 450);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(1000, 50);
            this.tlpButton.TabIndex = 19;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(430, 6);
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
            this.bExit.Location = new System.Drawing.Point(507, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // splitterBatchAccounts
            // 
            this.splitterBatchAccounts.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterBatchAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterBatchAccounts.Location = new System.Drawing.Point(0, 0);
            this.splitterBatchAccounts.Margin = new System.Windows.Forms.Padding(0);
            this.splitterBatchAccounts.Name = "splitterBatchAccounts";
            // 
            // splitterBatchAccounts.Panel1
            // 
            this.splitterBatchAccounts.Panel1.Controls.Add(this.tlpBatchInfo);
            this.splitterBatchAccounts.Panel1MinSize = 0;
            // 
            // splitterBatchAccounts.Panel2
            // 
            this.splitterBatchAccounts.Panel2.Controls.Add(this.tBatchAccounts);
            this.splitterBatchAccounts.Panel2MinSize = 0;
            this.splitterBatchAccounts.Size = new System.Drawing.Size(1000, 450);
            this.splitterBatchAccounts.SplitterDistance = 301;
            this.splitterBatchAccounts.SplitterSize = 80;
            this.splitterBatchAccounts.SplitterWidth = 5;
            this.splitterBatchAccounts.TabIndex = 20;
            // 
            // tlpBatchInfo
            // 
            this.tlpBatchInfo.ColumnCount = 2;
            this.tlpBatchInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBatchInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBatchInfo.Controls.Add(this.lPrefix, 0, 1);
            this.tlpBatchInfo.Controls.Add(this.nudLimitLinks, 1, 3);
            this.tlpBatchInfo.Controls.Add(this.dpExpiryTime, 1, 5);
            this.tlpBatchInfo.Controls.Add(this.cbExpiryTime, 0, 5);
            this.tlpBatchInfo.Controls.Add(this.nudLimitDevices, 1, 4);
            this.tlpBatchInfo.Controls.Add(this.cbLimitDevices, 0, 4);
            this.tlpBatchInfo.Controls.Add(this.nudAccountNum, 1, 6);
            this.tlpBatchInfo.Controls.Add(this.lAccountNum, 0, 6);
            this.tlpBatchInfo.Controls.Add(this.lPasswordLength, 0, 2);
            this.tlpBatchInfo.Controls.Add(this.lAccountRule, 0, 0);
            this.tlpBatchInfo.Controls.Add(this.ddlAccountRule, 1, 0);
            this.tlpBatchInfo.Controls.Add(this.nudPasswordLength, 1, 2);
            this.tlpBatchInfo.Controls.Add(this.bPreview, 1, 7);
            this.tlpBatchInfo.Controls.Add(this.cbLimitLinks, 0, 3);
            this.tlpBatchInfo.Controls.Add(this.txtPrefix, 1, 1);
            this.tlpBatchInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBatchInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpBatchInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBatchInfo.Name = "tlpBatchInfo";
            this.tlpBatchInfo.RowCount = 9;
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBatchInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBatchInfo.Size = new System.Drawing.Size(301, 450);
            this.tlpBatchInfo.TabIndex = 21;
            // 
            // lPrefix
            // 
            this.lPrefix.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPrefix.LocalizationText = "BatchAccounts.CustomPrefix";
            this.lPrefix.Location = new System.Drawing.Point(3, 44);
            this.lPrefix.Name = "lPrefix";
            this.lPrefix.Size = new System.Drawing.Size(67, 35);
            this.lPrefix.TabIndex = 18;
            this.lPrefix.Text = "自定义前缀 :";
            // 
            // nudLimitLinks
            // 
            this.nudLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitLinks.Location = new System.Drawing.Point(108, 126);
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
            this.nudLimitLinks.Size = new System.Drawing.Size(190, 35);
            this.nudLimitLinks.TabIndex = 17;
            this.nudLimitLinks.Text = "1";
            this.nudLimitLinks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLimitLinks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dpExpiryTime
            // 
            this.dpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpExpiryTime.Format = "yyyy-MM-dd HH:mm:ss";
            this.dpExpiryTime.Location = new System.Drawing.Point(108, 208);
            this.dpExpiryTime.MaxDate = new System.DateTime(8888, 12, 31, 0, 0, 0, 0);
            this.dpExpiryTime.Name = "dpExpiryTime";
            this.dpExpiryTime.Size = new System.Drawing.Size(190, 35);
            this.dpExpiryTime.TabIndex = 16;
            this.dpExpiryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbExpiryTime
            // 
            this.cbExpiryTime.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbExpiryTime.LocalizationText = "AccountEditForm.ExpireTime";
            this.cbExpiryTime.Location = new System.Drawing.Point(3, 208);
            this.cbExpiryTime.Name = "cbExpiryTime";
            this.cbExpiryTime.Size = new System.Drawing.Size(87, 35);
            this.cbExpiryTime.TabIndex = 15;
            this.cbExpiryTime.Text = "过期时间 :";
            this.cbExpiryTime.CheckedChanged += new AntdUI.BoolEventHandler(this.cbExpiryTime_CheckedChanged);
            // 
            // nudLimitDevices
            // 
            this.nudLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitDevices.Location = new System.Drawing.Point(108, 167);
            this.nudLimitDevices.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudLimitDevices.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimitDevices.Name = "nudLimitDevices";
            this.nudLimitDevices.Size = new System.Drawing.Size(190, 35);
            this.nudLimitDevices.TabIndex = 14;
            this.nudLimitDevices.Text = "1";
            this.nudLimitDevices.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLimitDevices.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbLimitDevices
            // 
            this.cbLimitDevices.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLimitDevices.LocalizationText = "AccountEditForm.LimitDevices";
            this.cbLimitDevices.Location = new System.Drawing.Point(3, 167);
            this.cbLimitDevices.Name = "cbLimitDevices";
            this.cbLimitDevices.Size = new System.Drawing.Size(99, 35);
            this.cbLimitDevices.TabIndex = 13;
            this.cbLimitDevices.Text = "限制设备数 :";
            this.cbLimitDevices.CheckedChanged += new AntdUI.BoolEventHandler(this.cbLimitDevices_CheckedChanged);
            // 
            // nudAccountNum
            // 
            this.nudAccountNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudAccountNum.Location = new System.Drawing.Point(108, 249);
            this.nudAccountNum.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudAccountNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAccountNum.Name = "nudAccountNum";
            this.nudAccountNum.Size = new System.Drawing.Size(190, 35);
            this.nudAccountNum.TabIndex = 5;
            this.nudAccountNum.Text = "10";
            this.nudAccountNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAccountNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lAccountNum
            // 
            this.lAccountNum.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAccountNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAccountNum.LocalizationText = "BatchAccounts.GeneratedQuantity";
            this.lAccountNum.Location = new System.Drawing.Point(3, 249);
            this.lAccountNum.Name = "lAccountNum";
            this.lAccountNum.Size = new System.Drawing.Size(55, 35);
            this.lAccountNum.TabIndex = 4;
            this.lAccountNum.Text = "生成数量 :";
            // 
            // lPasswordLength
            // 
            this.lPasswordLength.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPasswordLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPasswordLength.LocalizationText = "BatchAccounts.PasswordLen";
            this.lPasswordLength.Location = new System.Drawing.Point(3, 85);
            this.lPasswordLength.Name = "lPasswordLength";
            this.lPasswordLength.Size = new System.Drawing.Size(55, 35);
            this.lPasswordLength.TabIndex = 2;
            this.lPasswordLength.Text = "密码长度 :";
            // 
            // lAccountRule
            // 
            this.lAccountRule.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAccountRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAccountRule.LocalizationText = "BatchAccounts.AccountRule";
            this.lAccountRule.Location = new System.Drawing.Point(3, 3);
            this.lAccountRule.Name = "lAccountRule";
            this.lAccountRule.Size = new System.Drawing.Size(55, 35);
            this.lAccountRule.TabIndex = 0;
            this.lAccountRule.Text = "账号规则 :";
            // 
            // ddlAccountRule
            // 
            this.ddlAccountRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlAccountRule.List = true;
            this.ddlAccountRule.Location = new System.Drawing.Point(108, 3);
            this.ddlAccountRule.Name = "ddlAccountRule";
            this.ddlAccountRule.Size = new System.Drawing.Size(190, 35);
            this.ddlAccountRule.TabIndex = 1;
            this.ddlAccountRule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ddlAccountRule.SelectedIndexChanged += new AntdUI.IntEventHandler(this.ddlAccountRule_SelectedIndexChanged);
            // 
            // nudPasswordLength
            // 
            this.nudPasswordLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPasswordLength.Location = new System.Drawing.Point(108, 85);
            this.nudPasswordLength.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudPasswordLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPasswordLength.Name = "nudPasswordLength";
            this.nudPasswordLength.Size = new System.Drawing.Size(190, 35);
            this.nudPasswordLength.TabIndex = 3;
            this.nudPasswordLength.Text = "6";
            this.nudPasswordLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPasswordLength.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // bPreview
            // 
            this.bPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bPreview.LocalizationText = "Preview";
            this.bPreview.Location = new System.Drawing.Point(108, 290);
            this.bPreview.Name = "bPreview";
            this.bPreview.Size = new System.Drawing.Size(190, 35);
            this.bPreview.TabIndex = 6;
            this.bPreview.Text = "预览";
            this.bPreview.Type = AntdUI.TTypeMini.Primary;
            this.bPreview.Click += new System.EventHandler(this.bPreview_Click);
            // 
            // cbLimitLinks
            // 
            this.cbLimitLinks.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLimitLinks.LocalizationText = "AccountEditForm.LimitLinks";
            this.cbLimitLinks.Location = new System.Drawing.Point(3, 126);
            this.cbLimitLinks.Name = "cbLimitLinks";
            this.cbLimitLinks.Size = new System.Drawing.Size(99, 35);
            this.cbLimitLinks.TabIndex = 12;
            this.cbLimitLinks.Text = "限制链接数 :";
            this.cbLimitLinks.CheckedChanged += new AntdUI.BoolEventHandler(this.cbLimitLinks_CheckedChanged);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrefix.Location = new System.Drawing.Point(108, 44);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(190, 35);
            this.txtPrefix.TabIndex = 19;
            // 
            // tBatchAccounts
            // 
            this.tBatchAccounts.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tBatchAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tBatchAccounts.Gap = 10;
            this.tBatchAccounts.GapCell = 5;
            this.tBatchAccounts.Gaps = new System.Drawing.Size(10, 10);
            this.tBatchAccounts.Location = new System.Drawing.Point(0, 0);
            this.tBatchAccounts.Name = "tBatchAccounts";
            this.tBatchAccounts.Size = new System.Drawing.Size(694, 450);
            this.tBatchAccounts.TabIndex = 20;
            this.tBatchAccounts.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tBatchAccounts_CellButtonClick);
            this.tBatchAccounts.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tBatchAccounts_CellDoubleClick);
            this.tBatchAccounts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tBatchAccounts_MouseClick);
            // 
            // BatchAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpBatchAccounts);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BatchAccounts";
            this.Size = new System.Drawing.Size(1000, 500);
            this.Load += new System.EventHandler(this.BatchAccounts_Load);
            this.tlpBatchAccounts.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.splitterBatchAccounts.Panel1.ResumeLayout(false);
            this.splitterBatchAccounts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterBatchAccounts)).EndInit();
            this.splitterBatchAccounts.ResumeLayout(false);
            this.tlpBatchInfo.ResumeLayout(false);
            this.tlpBatchInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpBatchAccounts;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Splitter splitterBatchAccounts;
        private TableLayoutPanelEx tlpBatchInfo;
        private AntdUI.InputNumber nudLimitLinks;
        private AntdUI.DatePicker dpExpiryTime;
        private AntdUI.Checkbox cbExpiryTime;
        private AntdUI.InputNumber nudLimitDevices;
        private AntdUI.Checkbox cbLimitDevices;
        private AntdUI.InputNumber nudAccountNum;
        private AntdUI.Label lAccountNum;
        private AntdUI.Label lPasswordLength;
        private AntdUI.Label lAccountRule;
        private AntdUI.Select ddlAccountRule;
        private AntdUI.InputNumber nudPasswordLength;
        private AntdUI.Button bPreview;
        private AntdUI.Checkbox cbLimitLinks;
        private AntdUI.Table tBatchAccounts;
        private AntdUI.Label lPrefix;
        private AntdUI.Input txtPrefix;
    }
}

namespace WinsockPacketEditor
{
    partial class LimitDevices
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
            this.tlpLimitDevices = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpSet = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbIsLimitDevices = new AntdUI.Checkbox();
            this.nudLimitDevices = new AntdUI.InputNumber();
            this.lAccountCNT = new AntdUI.Label();
            this.tlpLimitDevices.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLimitDevices
            // 
            this.tlpLimitDevices.ColumnCount = 1;
            this.tlpLimitDevices.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLimitDevices.Controls.Add(this.tlpButton, 0, 4);
            this.tlpLimitDevices.Controls.Add(this.tlpSet, 0, 2);
            this.tlpLimitDevices.Controls.Add(this.lAccountCNT, 0, 0);
            this.tlpLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLimitDevices.Location = new System.Drawing.Point(0, 0);
            this.tlpLimitDevices.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLimitDevices.Name = "tlpLimitDevices";
            this.tlpLimitDevices.RowCount = 5;
            this.tlpLimitDevices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpLimitDevices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpLimitDevices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tlpLimitDevices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLimitDevices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpLimitDevices.Size = new System.Drawing.Size(350, 250);
            this.tlpLimitDevices.TabIndex = 4;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 201);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(350, 49);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(103, 6);
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
            this.bExit.Location = new System.Drawing.Point(184, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
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
            this.tlpSet.Controls.Add(this.cbIsLimitDevices, 0, 0);
            this.tlpSet.Controls.Add(this.nudLimitDevices, 1, 0);
            this.tlpSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSet.Location = new System.Drawing.Point(0, 56);
            this.tlpSet.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSet.Name = "tlpSet";
            this.tlpSet.RowCount = 2;
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSet.Size = new System.Drawing.Size(350, 243);
            this.tlpSet.TabIndex = 0;
            // 
            // cbIsLimitDevices
            // 
            this.cbIsLimitDevices.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsLimitDevices.LocalizationText = "LimitDevicesForm.LimitDevices";
            this.cbIsLimitDevices.Location = new System.Drawing.Point(2, 2);
            this.cbIsLimitDevices.Margin = new System.Windows.Forms.Padding(2);
            this.cbIsLimitDevices.Name = "cbIsLimitDevices";
            this.cbIsLimitDevices.Size = new System.Drawing.Size(99, 36);
            this.cbIsLimitDevices.TabIndex = 6;
            this.cbIsLimitDevices.Text = "限制设备数 :";
            this.cbIsLimitDevices.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsLimitDevices_CheckedChanged);
            // 
            // nudLimitDevices
            // 
            this.nudLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitDevices.Location = new System.Drawing.Point(105, 2);
            this.nudLimitDevices.Margin = new System.Windows.Forms.Padding(2);
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
            this.nudLimitDevices.PrefixSvg = "";
            this.nudLimitDevices.SelectionStart = 1;
            this.nudLimitDevices.Size = new System.Drawing.Size(243, 36);
            this.nudLimitDevices.SuffixSvg = "TabletOutlined";
            this.nudLimitDevices.TabIndex = 7;
            this.nudLimitDevices.Text = "1";
            this.nudLimitDevices.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLimitDevices.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lAccountCNT
            // 
            this.lAccountCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAccountCNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAccountCNT.Location = new System.Drawing.Point(2, 2);
            this.lAccountCNT.Margin = new System.Windows.Forms.Padding(2);
            this.lAccountCNT.Name = "lAccountCNT";
            this.lAccountCNT.Size = new System.Drawing.Size(346, 36);
            this.lAccountCNT.TabIndex = 18;
            this.lAccountCNT.Text = "0";
            this.lAccountCNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LimitDevices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLimitDevices);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LimitDevices";
            this.Size = new System.Drawing.Size(350, 250);
            this.Load += new System.EventHandler(this.LimitDevices_Load);
            this.tlpLimitDevices.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpSet.ResumeLayout(false);
            this.tlpSet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpLimitDevices;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpSet;
        private AntdUI.Checkbox cbIsLimitDevices;
        private AntdUI.InputNumber nudLimitDevices;
        private AntdUI.Label lAccountCNT;
    }
}

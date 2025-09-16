namespace WinsockPacketEditor
{
    partial class ExpiryTime
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
            this.tlpExpiryTime = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpAddTime = new WinsockPacketEditor.TableLayoutPanelEx();
            this.rbFromNow = new AntdUI.Radio();
            this.rbFromExpiryTime = new AntdUI.Radio();
            this.nudAddTime = new AntdUI.InputNumber();
            this.tlpTimeType = new WinsockPacketEditor.TableLayoutPanelEx();
            this.rbAddHour = new AntdUI.Radio();
            this.rbAddDay = new AntdUI.Radio();
            this.lAccountCNT = new AntdUI.Label();
            this.tlpExpiryTime.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpAddTime.SuspendLayout();
            this.tlpTimeType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpExpiryTime
            // 
            this.tlpExpiryTime.ColumnCount = 1;
            this.tlpExpiryTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExpiryTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpExpiryTime.Controls.Add(this.tlpButton, 0, 4);
            this.tlpExpiryTime.Controls.Add(this.tlpAddTime, 0, 2);
            this.tlpExpiryTime.Controls.Add(this.lAccountCNT, 0, 0);
            this.tlpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExpiryTime.Location = new System.Drawing.Point(0, 0);
            this.tlpExpiryTime.Margin = new System.Windows.Forms.Padding(0);
            this.tlpExpiryTime.Name = "tlpExpiryTime";
            this.tlpExpiryTime.RowCount = 5;
            this.tlpExpiryTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpExpiryTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpExpiryTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpExpiryTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExpiryTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpExpiryTime.Size = new System.Drawing.Size(500, 700);
            this.tlpExpiryTime.TabIndex = 2;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 640);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(155, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(82, 46);
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
            this.bExit.Location = new System.Drawing.Point(263, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(82, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpAddTime
            // 
            this.tlpAddTime.ColumnCount = 2;
            this.tlpAddTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAddTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAddTime.Controls.Add(this.rbFromNow, 1, 0);
            this.tlpAddTime.Controls.Add(this.rbFromExpiryTime, 0, 0);
            this.tlpAddTime.Controls.Add(this.nudAddTime, 0, 1);
            this.tlpAddTime.Controls.Add(this.tlpTimeType, 1, 1);
            this.tlpAddTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAddTime.Location = new System.Drawing.Point(0, 70);
            this.tlpAddTime.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAddTime.Name = "tlpAddTime";
            this.tlpAddTime.RowCount = 3;
            this.tlpAddTime.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAddTime.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAddTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAddTime.Size = new System.Drawing.Size(500, 300);
            this.tlpAddTime.TabIndex = 0;
            // 
            // rbFromNow
            // 
            this.rbFromNow.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFromNow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromNow.LocalizationText = "ExpiryTimeForm.FromNow";
            this.rbFromNow.Location = new System.Drawing.Point(253, 3);
            this.rbFromNow.Name = "rbFromNow";
            this.rbFromNow.Size = new System.Drawing.Size(140, 42);
            this.rbFromNow.TabIndex = 5;
            this.rbFromNow.Text = "基于当前时间";
            // 
            // rbFromExpiryTime
            // 
            this.rbFromExpiryTime.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFromExpiryTime.Checked = true;
            this.rbFromExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFromExpiryTime.LocalizationText = "ExpiryTimeForm.FromExpiryTime";
            this.rbFromExpiryTime.Location = new System.Drawing.Point(3, 3);
            this.rbFromExpiryTime.Name = "rbFromExpiryTime";
            this.rbFromExpiryTime.Size = new System.Drawing.Size(140, 42);
            this.rbFromExpiryTime.TabIndex = 4;
            this.rbFromExpiryTime.Text = "基于原有时间";
            // 
            // nudAddTime
            // 
            this.nudAddTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudAddTime.LocalizationPrefixText = "Add";
            this.nudAddTime.Location = new System.Drawing.Point(3, 51);
            this.nudAddTime.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudAddTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAddTime.Name = "nudAddTime";
            this.nudAddTime.PrefixSvg = "";
            this.nudAddTime.PrefixText = "添加:";
            this.nudAddTime.SelectionStart = 1;
            this.nudAddTime.Size = new System.Drawing.Size(244, 45);
            this.nudAddTime.SuffixSvg = "FieldTimeOutlined";
            this.nudAddTime.TabIndex = 6;
            this.nudAddTime.Text = "1";
            this.nudAddTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAddTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tlpTimeType
            // 
            this.tlpTimeType.ColumnCount = 2;
            this.tlpTimeType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTimeType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTimeType.Controls.Add(this.rbAddHour, 0, 0);
            this.tlpTimeType.Controls.Add(this.rbAddDay, 1, 0);
            this.tlpTimeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTimeType.Location = new System.Drawing.Point(250, 48);
            this.tlpTimeType.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTimeType.Name = "tlpTimeType";
            this.tlpTimeType.RowCount = 1;
            this.tlpTimeType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTimeType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tlpTimeType.Size = new System.Drawing.Size(250, 51);
            this.tlpTimeType.TabIndex = 7;
            // 
            // rbAddHour
            // 
            this.rbAddHour.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbAddHour.Checked = true;
            this.rbAddHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbAddHour.LocalizationText = "Hour";
            this.rbAddHour.Location = new System.Drawing.Point(3, 3);
            this.rbAddHour.Name = "rbAddHour";
            this.rbAddHour.Size = new System.Drawing.Size(76, 45);
            this.rbAddHour.TabIndex = 0;
            this.rbAddHour.Text = "小时";
            // 
            // rbAddDay
            // 
            this.rbAddDay.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbAddDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbAddDay.LocalizationText = "Day";
            this.rbAddDay.Location = new System.Drawing.Point(128, 3);
            this.rbAddDay.Name = "rbAddDay";
            this.rbAddDay.Size = new System.Drawing.Size(60, 45);
            this.rbAddDay.TabIndex = 1;
            this.rbAddDay.Text = "天";
            // 
            // lAccountCNT
            // 
            this.lAccountCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAccountCNT.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAccountCNT.Location = new System.Drawing.Point(3, 3);
            this.lAccountCNT.Name = "lAccountCNT";
            this.lAccountCNT.Size = new System.Drawing.Size(494, 44);
            this.lAccountCNT.TabIndex = 18;
            this.lAccountCNT.Text = "0";
            this.lAccountCNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExpiryTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpExpiryTime);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ExpiryTime";
            this.Size = new System.Drawing.Size(500, 700);
            this.Load += new System.EventHandler(this.ExpiryTime_Load);
            this.tlpExpiryTime.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpAddTime.ResumeLayout(false);
            this.tlpAddTime.PerformLayout();
            this.tlpTimeType.ResumeLayout(false);
            this.tlpTimeType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpExpiryTime;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpAddTime;
        private AntdUI.Radio rbFromNow;
        private AntdUI.Radio rbFromExpiryTime;
        private AntdUI.InputNumber nudAddTime;
        private TableLayoutPanelEx tlpTimeType;
        private AntdUI.Radio rbAddHour;
        private AntdUI.Radio rbAddDay;
        private AntdUI.Label lAccountCNT;
    }
}

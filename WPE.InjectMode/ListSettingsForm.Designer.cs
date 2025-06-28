namespace WPE.InjectMode
{
    partial class ListSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListSettingsForm));
            this.tlpListSettings = new System.Windows.Forms.TableLayoutPanel();
            this.divider1 = new AntdUI.Divider();
            this.divider2 = new AntdUI.Divider();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpPacketList = new System.Windows.Forms.TableLayoutPanel();
            this.tlpLogList = new System.Windows.Forms.TableLayoutPanel();
            this.cbPacketList_AutoRoll = new AntdUI.Checkbox();
            this.cbPacketList_AutoClear = new AntdUI.Checkbox();
            this.txtPacketList_AutoClear = new AntdUI.InputNumber();
            this.cbLogList_AutoRoll = new AntdUI.Checkbox();
            this.cbLogList_AutoClear = new AntdUI.Checkbox();
            this.txtLogList_AutoClear = new AntdUI.InputNumber();
            this.tlpListSettings.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpPacketList.SuspendLayout();
            this.tlpLogList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpListSettings
            // 
            this.tlpListSettings.ColumnCount = 1;
            this.tlpListSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSettings.Controls.Add(this.tlpLogList, 0, 5);
            this.tlpListSettings.Controls.Add(this.tlpButton, 0, 6);
            this.tlpListSettings.Controls.Add(this.divider1, 0, 0);
            this.tlpListSettings.Controls.Add(this.divider2, 0, 3);
            this.tlpListSettings.Controls.Add(this.tlpPacketList, 0, 2);
            this.tlpListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpListSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpListSettings.Name = "tlpListSettings";
            this.tlpListSettings.RowCount = 7;
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpListSettings.Size = new System.Drawing.Size(484, 461);
            this.tlpListSettings.TabIndex = 0;
            // 
            // divider1
            // 
            this.divider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider1.Location = new System.Drawing.Point(3, 3);
            this.divider1.Name = "divider1";
            this.divider1.Orientation = AntdUI.TOrientation.Left;
            this.divider1.Size = new System.Drawing.Size(478, 23);
            this.divider1.TabIndex = 0;
            this.divider1.Text = "封包列表";
            // 
            // divider2
            // 
            this.divider2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider2.Location = new System.Drawing.Point(3, 172);
            this.divider2.Name = "divider2";
            this.divider2.Orientation = AntdUI.TOrientation.Left;
            this.divider2.Size = new System.Drawing.Size(478, 23);
            this.divider2.TabIndex = 1;
            this.divider2.Text = "日志列表";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 401);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 4;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.Location = new System.Drawing.Point(115, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(255, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpPacketList
            // 
            this.tlpPacketList.ColumnCount = 2;
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Controls.Add(this.cbPacketList_AutoRoll, 0, 0);
            this.tlpPacketList.Controls.Add(this.cbPacketList_AutoClear, 0, 1);
            this.tlpPacketList.Controls.Add(this.txtPacketList_AutoClear, 1, 1);
            this.tlpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList.Location = new System.Drawing.Point(0, 49);
            this.tlpPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList.Name = "tlpPacketList";
            this.tlpPacketList.RowCount = 3;
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Size = new System.Drawing.Size(484, 120);
            this.tlpPacketList.TabIndex = 5;
            // 
            // tlpLogList
            // 
            this.tlpLogList.ColumnCount = 2;
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.Controls.Add(this.txtLogList_AutoClear, 1, 1);
            this.tlpLogList.Controls.Add(this.cbLogList_AutoClear, 0, 1);
            this.tlpLogList.Controls.Add(this.cbLogList_AutoRoll, 0, 0);
            this.tlpLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLogList.Location = new System.Drawing.Point(0, 218);
            this.tlpLogList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLogList.Name = "tlpLogList";
            this.tlpLogList.RowCount = 3;
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.Size = new System.Drawing.Size(484, 183);
            this.tlpLogList.TabIndex = 6;
            // 
            // cbPacketList_AutoRoll
            // 
            this.cbPacketList_AutoRoll.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbPacketList_AutoRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPacketList_AutoRoll.Location = new System.Drawing.Point(3, 3);
            this.cbPacketList_AutoRoll.Name = "cbPacketList_AutoRoll";
            this.cbPacketList_AutoRoll.Size = new System.Drawing.Size(106, 42);
            this.cbPacketList_AutoRoll.TabIndex = 0;
            this.cbPacketList_AutoRoll.Text = "自动滚动";
            // 
            // cbPacketList_AutoClear
            // 
            this.cbPacketList_AutoClear.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbPacketList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPacketList_AutoClear.Location = new System.Drawing.Point(3, 51);
            this.cbPacketList_AutoClear.Name = "cbPacketList_AutoClear";
            this.cbPacketList_AutoClear.Size = new System.Drawing.Size(106, 42);
            this.cbPacketList_AutoClear.TabIndex = 1;
            this.cbPacketList_AutoClear.Text = "自动清理";
            this.cbPacketList_AutoClear.CheckedChanged += new AntdUI.BoolEventHandler(this.cbPacketList_AutoClear_CheckedChanged);
            // 
            // txtPacketList_AutoClear
            // 
            this.txtPacketList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketList_AutoClear.Location = new System.Drawing.Point(115, 51);
            this.txtPacketList_AutoClear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPacketList_AutoClear.Name = "txtPacketList_AutoClear";
            this.txtPacketList_AutoClear.Size = new System.Drawing.Size(366, 42);
            this.txtPacketList_AutoClear.TabIndex = 2;
            this.txtPacketList_AutoClear.Text = "5000";
            this.txtPacketList_AutoClear.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // cbLogList_AutoRoll
            // 
            this.cbLogList_AutoRoll.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbLogList_AutoRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogList_AutoRoll.Location = new System.Drawing.Point(3, 3);
            this.cbLogList_AutoRoll.Name = "cbLogList_AutoRoll";
            this.cbLogList_AutoRoll.Size = new System.Drawing.Size(106, 42);
            this.cbLogList_AutoRoll.TabIndex = 1;
            this.cbLogList_AutoRoll.Text = "自动滚动";
            // 
            // cbLogList_AutoClear
            // 
            this.cbLogList_AutoClear.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbLogList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogList_AutoClear.Location = new System.Drawing.Point(3, 51);
            this.cbLogList_AutoClear.Name = "cbLogList_AutoClear";
            this.cbLogList_AutoClear.Size = new System.Drawing.Size(106, 42);
            this.cbLogList_AutoClear.TabIndex = 3;
            this.cbLogList_AutoClear.Text = "自动清理";
            this.cbLogList_AutoClear.CheckedChanged += new AntdUI.BoolEventHandler(this.cbLogList_AutoClear_CheckedChanged);
            // 
            // txtLogList_AutoClear
            // 
            this.txtLogList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogList_AutoClear.Location = new System.Drawing.Point(115, 51);
            this.txtLogList_AutoClear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtLogList_AutoClear.Name = "txtLogList_AutoClear";
            this.txtLogList_AutoClear.Size = new System.Drawing.Size(366, 42);
            this.txtLogList_AutoClear.TabIndex = 4;
            this.txtLogList_AutoClear.Text = "5000";
            this.txtLogList_AutoClear.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // ListSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tlpListSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "ListSettingsForm";
            this.Text = "ListSettingsForm";
            this.Load += new System.EventHandler(this.ListSettingsForm_Load);
            this.tlpListSettings.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpPacketList.ResumeLayout(false);
            this.tlpPacketList.PerformLayout();
            this.tlpLogList.ResumeLayout(false);
            this.tlpLogList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpListSettings;
        private AntdUI.Divider divider1;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.TableLayoutPanel tlpLogList;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private System.Windows.Forms.TableLayoutPanel tlpPacketList;
        private AntdUI.Checkbox cbPacketList_AutoRoll;
        private AntdUI.Checkbox cbPacketList_AutoClear;
        private AntdUI.InputNumber txtPacketList_AutoClear;
        private AntdUI.InputNumber txtLogList_AutoClear;
        private AntdUI.Checkbox cbLogList_AutoClear;
        private AntdUI.Checkbox cbLogList_AutoRoll;
    }
}
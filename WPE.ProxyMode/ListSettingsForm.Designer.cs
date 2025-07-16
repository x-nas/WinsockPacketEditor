namespace WPE.ProxyMode
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
            this.tlpLogList = new System.Windows.Forms.TableLayoutPanel();
            this.txtLogList_AutoClear = new AntdUI.InputNumber();
            this.cbLogList_AutoClear = new AntdUI.Checkbox();
            this.cbLogList_AutoRoll = new AntdUI.Checkbox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dProxyList = new AntdUI.Divider();
            this.dLogList = new AntdUI.Divider();
            this.tlpProxyList = new System.Windows.Forms.TableLayoutPanel();
            this.cbProxyList_AutoRoll = new AntdUI.Checkbox();
            this.cbProxyList_AutoClear = new AntdUI.Checkbox();
            this.txtProxyList_AutoClear = new AntdUI.InputNumber();
            this.dServerList = new AntdUI.Divider();
            this.cbNoRecordData = new AntdUI.Checkbox();
            this.divider1 = new AntdUI.Divider();
            this.cbDeleteClosed = new AntdUI.Checkbox();
            this.tlpListSettings.SuspendLayout();
            this.tlpLogList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpProxyList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpListSettings
            // 
            this.tlpListSettings.ColumnCount = 1;
            this.tlpListSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSettings.Controls.Add(this.divider1, 0, 10);
            this.tlpListSettings.Controls.Add(this.dServerList, 0, 6);
            this.tlpListSettings.Controls.Add(this.tlpLogList, 0, 5);
            this.tlpListSettings.Controls.Add(this.tlpButton, 0, 14);
            this.tlpListSettings.Controls.Add(this.dProxyList, 0, 0);
            this.tlpListSettings.Controls.Add(this.dLogList, 0, 3);
            this.tlpListSettings.Controls.Add(this.tlpProxyList, 0, 2);
            this.tlpListSettings.Controls.Add(this.cbNoRecordData, 0, 8);
            this.tlpListSettings.Controls.Add(this.cbDeleteClosed, 0, 12);
            this.tlpListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpListSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpListSettings.Name = "tlpListSettings";
            this.tlpListSettings.RowCount = 15;
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpListSettings.Size = new System.Drawing.Size(484, 761);
            this.tlpListSettings.TabIndex = 1;
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
            this.tlpLogList.Size = new System.Drawing.Size(484, 120);
            this.tlpLogList.TabIndex = 6;
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
            this.txtLogList_AutoClear.SelectionStart = 4;
            this.txtLogList_AutoClear.Size = new System.Drawing.Size(366, 42);
            this.txtLogList_AutoClear.TabIndex = 4;
            this.txtLogList_AutoClear.Text = "5000";
            this.txtLogList_AutoClear.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
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
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
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
            // dProxyList
            // 
            this.dProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProxyList.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProxyList.Location = new System.Drawing.Point(3, 3);
            this.dProxyList.Name = "dProxyList";
            this.dProxyList.Orientation = AntdUI.TOrientation.Left;
            this.dProxyList.Size = new System.Drawing.Size(478, 23);
            this.dProxyList.TabIndex = 0;
            this.dProxyList.Text = "代理列表";
            // 
            // dLogList
            // 
            this.dLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLogList.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLogList.Location = new System.Drawing.Point(3, 172);
            this.dLogList.Name = "dLogList";
            this.dLogList.Orientation = AntdUI.TOrientation.Left;
            this.dLogList.Size = new System.Drawing.Size(478, 23);
            this.dLogList.TabIndex = 1;
            this.dLogList.Text = "日志列表";
            // 
            // tlpProxyList
            // 
            this.tlpProxyList.ColumnCount = 2;
            this.tlpProxyList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList.Controls.Add(this.cbProxyList_AutoRoll, 0, 0);
            this.tlpProxyList.Controls.Add(this.cbProxyList_AutoClear, 0, 1);
            this.tlpProxyList.Controls.Add(this.txtProxyList_AutoClear, 1, 1);
            this.tlpProxyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyList.Location = new System.Drawing.Point(0, 49);
            this.tlpProxyList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyList.Name = "tlpProxyList";
            this.tlpProxyList.RowCount = 3;
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyList.Size = new System.Drawing.Size(484, 120);
            this.tlpProxyList.TabIndex = 5;
            // 
            // cbProxyList_AutoRoll
            // 
            this.cbProxyList_AutoRoll.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbProxyList_AutoRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbProxyList_AutoRoll.Location = new System.Drawing.Point(3, 3);
            this.cbProxyList_AutoRoll.Name = "cbProxyList_AutoRoll";
            this.cbProxyList_AutoRoll.Size = new System.Drawing.Size(106, 42);
            this.cbProxyList_AutoRoll.TabIndex = 0;
            this.cbProxyList_AutoRoll.Text = "自动滚动";
            // 
            // cbProxyList_AutoClear
            // 
            this.cbProxyList_AutoClear.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbProxyList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbProxyList_AutoClear.Location = new System.Drawing.Point(3, 51);
            this.cbProxyList_AutoClear.Name = "cbProxyList_AutoClear";
            this.cbProxyList_AutoClear.Size = new System.Drawing.Size(106, 42);
            this.cbProxyList_AutoClear.TabIndex = 1;
            this.cbProxyList_AutoClear.Text = "自动清理";
            this.cbProxyList_AutoClear.CheckedChanged += new AntdUI.BoolEventHandler(this.cbProxyList_AutoClear_CheckedChanged);
            // 
            // txtProxyList_AutoClear
            // 
            this.txtProxyList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyList_AutoClear.Location = new System.Drawing.Point(115, 51);
            this.txtProxyList_AutoClear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtProxyList_AutoClear.Name = "txtProxyList_AutoClear";
            this.txtProxyList_AutoClear.SelectionStart = 4;
            this.txtProxyList_AutoClear.Size = new System.Drawing.Size(366, 42);
            this.txtProxyList_AutoClear.TabIndex = 2;
            this.txtProxyList_AutoClear.Text = "5000";
            this.txtProxyList_AutoClear.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // dServerList
            // 
            this.dServerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dServerList.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dServerList.Location = new System.Drawing.Point(3, 341);
            this.dServerList.Name = "dServerList";
            this.dServerList.Orientation = AntdUI.TOrientation.Left;
            this.dServerList.Size = new System.Drawing.Size(478, 23);
            this.dServerList.TabIndex = 7;
            this.dServerList.Text = "服务端列表";
            // 
            // cbNoRecordData
            // 
            this.cbNoRecordData.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbNoRecordData.Checked = true;
            this.cbNoRecordData.Location = new System.Drawing.Point(3, 390);
            this.cbNoRecordData.Name = "cbNoRecordData";
            this.cbNoRecordData.Size = new System.Drawing.Size(122, 42);
            this.cbNoRecordData.TabIndex = 8;
            this.cbNoRecordData.Text = "不记录数据";
            // 
            // divider1
            // 
            this.divider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider1.Location = new System.Drawing.Point(3, 458);
            this.divider1.Name = "divider1";
            this.divider1.Orientation = AntdUI.TOrientation.Left;
            this.divider1.Size = new System.Drawing.Size(478, 23);
            this.divider1.TabIndex = 9;
            this.divider1.Text = "服务端列表";
            // 
            // cbDeleteClosed
            // 
            this.cbDeleteClosed.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbDeleteClosed.Checked = true;
            this.cbDeleteClosed.Location = new System.Drawing.Point(3, 507);
            this.cbDeleteClosed.Name = "cbDeleteClosed";
            this.cbDeleteClosed.Size = new System.Drawing.Size(154, 42);
            this.cbDeleteClosed.TabIndex = 10;
            this.cbDeleteClosed.Text = "清理关闭的链接";
            // 
            // ListSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 761);
            this.Controls.Add(this.tlpListSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ListSettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ListSettingsForm";
            this.Load += new System.EventHandler(this.ListSettingsForm_Load);
            this.tlpListSettings.ResumeLayout(false);
            this.tlpListSettings.PerformLayout();
            this.tlpLogList.ResumeLayout(false);
            this.tlpLogList.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpProxyList.ResumeLayout(false);
            this.tlpProxyList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpListSettings;
        private AntdUI.Divider dServerList;
        private System.Windows.Forms.TableLayoutPanel tlpLogList;
        private AntdUI.InputNumber txtLogList_AutoClear;
        private AntdUI.Checkbox cbLogList_AutoClear;
        private AntdUI.Checkbox cbLogList_AutoRoll;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dProxyList;
        private AntdUI.Divider dLogList;
        private System.Windows.Forms.TableLayoutPanel tlpProxyList;
        private AntdUI.Checkbox cbProxyList_AutoRoll;
        private AntdUI.Checkbox cbProxyList_AutoClear;
        private AntdUI.InputNumber txtProxyList_AutoClear;
        private AntdUI.Checkbox cbNoRecordData;
        private AntdUI.Divider divider1;
        private AntdUI.Checkbox cbDeleteClosed;
    }
}
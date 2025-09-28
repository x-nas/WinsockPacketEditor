namespace WinsockPacketEditor
{
    partial class LogList
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
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            this.tlpLogList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpLogList_Button = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtLogList_AutoClear = new AntdUI.InputNumber();
            this.cbLogList_AutoClear = new AntdUI.Checkbox();
            this.cbLogList_AutoRoll = new AntdUI.Checkbox();
            this.tabLogList = new AntdUI.Tabs();
            this.tpSystemLog = new AntdUI.TabPage();
            this.tSystemLog = new AntdUI.Table();
            this.tpFilterLog = new AntdUI.TabPage();
            this.tFilterLog = new AntdUI.Table();
            this.tpProxyLog = new AntdUI.TabPage();
            this.tProxyLog = new AntdUI.Table();
            this.tlpLogList.SuspendLayout();
            this.tlpLogList_Button.SuspendLayout();
            this.tabLogList.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
            this.tpFilterLog.SuspendLayout();
            this.tpProxyLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLogList
            // 
            this.tlpLogList.ColumnCount = 1;
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.Controls.Add(this.tlpLogList_Button, 0, 0);
            this.tlpLogList.Controls.Add(this.tabLogList, 0, 1);
            this.tlpLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLogList.Location = new System.Drawing.Point(0, 0);
            this.tlpLogList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLogList.Name = "tlpLogList";
            this.tlpLogList.RowCount = 2;
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpLogList.Size = new System.Drawing.Size(1100, 800);
            this.tlpLogList.TabIndex = 0;
            // 
            // tlpLogList_Button
            // 
            this.tlpLogList_Button.ColumnCount = 4;
            this.tlpLogList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLogList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tlpLogList_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLogList_Button.Controls.Add(this.txtLogList_AutoClear, 2, 0);
            this.tlpLogList_Button.Controls.Add(this.cbLogList_AutoClear, 1, 0);
            this.tlpLogList_Button.Controls.Add(this.cbLogList_AutoRoll, 3, 0);
            this.tlpLogList_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLogList_Button.Location = new System.Drawing.Point(0, 0);
            this.tlpLogList_Button.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLogList_Button.Name = "tlpLogList_Button";
            this.tlpLogList_Button.RowCount = 1;
            this.tlpLogList_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList_Button.Size = new System.Drawing.Size(1100, 40);
            this.tlpLogList_Button.TabIndex = 8;
            // 
            // txtLogList_AutoClear
            // 
            this.txtLogList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogList_AutoClear.Location = new System.Drawing.Point(934, 2);
            this.txtLogList_AutoClear.Margin = new System.Windows.Forms.Padding(2);
            this.txtLogList_AutoClear.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.txtLogList_AutoClear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtLogList_AutoClear.Name = "txtLogList_AutoClear";
            this.txtLogList_AutoClear.SelectionStart = 1;
            this.txtLogList_AutoClear.Size = new System.Drawing.Size(80, 36);
            this.txtLogList_AutoClear.TabIndex = 17;
            this.txtLogList_AutoClear.Text = "5000";
            this.txtLogList_AutoClear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLogList_AutoClear.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtLogList_AutoClear.ValueChanged += new AntdUI.DecimalEventHandler(this.txtLogList_AutoClear_ValueChanged);
            // 
            // cbLogList_AutoClear
            // 
            this.cbLogList_AutoClear.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbLogList_AutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogList_AutoClear.LocalizationText = "ListSettingsForm.AutoClear";
            this.cbLogList_AutoClear.Location = new System.Drawing.Point(850, 2);
            this.cbLogList_AutoClear.Margin = new System.Windows.Forms.Padding(2);
            this.cbLogList_AutoClear.Name = "cbLogList_AutoClear";
            this.cbLogList_AutoClear.Size = new System.Drawing.Size(80, 36);
            this.cbLogList_AutoClear.TabIndex = 16;
            this.cbLogList_AutoClear.Text = "自动清理";
            this.cbLogList_AutoClear.CheckedChanged += new AntdUI.BoolEventHandler(this.cbLogList_AutoClear_CheckedChanged);
            // 
            // cbLogList_AutoRoll
            // 
            this.cbLogList_AutoRoll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbLogList_AutoRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLogList_AutoRoll.LocalizationText = "ListSettingsForm.AutoRoll";
            this.cbLogList_AutoRoll.Location = new System.Drawing.Point(1018, 2);
            this.cbLogList_AutoRoll.Margin = new System.Windows.Forms.Padding(2);
            this.cbLogList_AutoRoll.Name = "cbLogList_AutoRoll";
            this.cbLogList_AutoRoll.Size = new System.Drawing.Size(80, 36);
            this.cbLogList_AutoRoll.TabIndex = 15;
            this.cbLogList_AutoRoll.Text = "自动滚动";
            this.cbLogList_AutoRoll.CheckedChanged += new AntdUI.BoolEventHandler(this.cbLogList_AutoRoll_CheckedChanged);
            // 
            // tabLogList
            // 
            this.tabLogList.Controls.Add(this.tpProxyLog);
            this.tabLogList.Controls.Add(this.tpFilterLog);
            this.tabLogList.Controls.Add(this.tpSystemLog);
            this.tabLogList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLogList.Gap = 20;
            this.tabLogList.Location = new System.Drawing.Point(2, 42);
            this.tabLogList.Margin = new System.Windows.Forms.Padding(2);
            this.tabLogList.Name = "tabLogList";
            this.tabLogList.Pages.Add(this.tpSystemLog);
            this.tabLogList.Pages.Add(this.tpFilterLog);
            this.tabLogList.Pages.Add(this.tpProxyLog);
            this.tabLogList.SelectedIndex = 2;
            this.tabLogList.Size = new System.Drawing.Size(1096, 756);
            this.tabLogList.Style = styleCard1;
            this.tabLogList.TabIndex = 0;
            this.tabLogList.Type = AntdUI.TabType.Card;
            // 
            // tpSystemLog
            // 
            this.tpSystemLog.Controls.Add(this.tSystemLog);
            this.tpSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSystemLog.LocalizationText = "LogList.LogList.{id}";
            this.tpSystemLog.Location = new System.Drawing.Point(0, 38);
            this.tpSystemLog.Margin = new System.Windows.Forms.Padding(2);
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.Size = new System.Drawing.Size(1096, 718);
            this.tpSystemLog.TabIndex = 0;
            this.tpSystemLog.Text = "系统日志";
            // 
            // tSystemLog
            // 
            this.tSystemLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSystemLog.CellImpactHeight = false;
            this.tSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSystemLog.Gap = 8;
            this.tSystemLog.GapCell = 5;
            this.tSystemLog.Gaps = new System.Drawing.Size(8, 8);
            this.tSystemLog.Location = new System.Drawing.Point(0, 0);
            this.tSystemLog.Margin = new System.Windows.Forms.Padding(2);
            this.tSystemLog.MultipleRows = true;
            this.tSystemLog.Name = "tSystemLog";
            this.tSystemLog.Size = new System.Drawing.Size(1096, 718);
            this.tSystemLog.TabIndex = 5;
            this.tSystemLog.Text = "table1";
            this.tSystemLog.CellClick += new AntdUI.Table.ClickEventHandler(this.tSystemLog_CellClick);
            // 
            // tpFilterLog
            // 
            this.tpFilterLog.Controls.Add(this.tFilterLog);
            this.tpFilterLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterLog.LocalizationText = "LogList.LogList.{id}";
            this.tpFilterLog.Location = new System.Drawing.Point(0, 38);
            this.tpFilterLog.Margin = new System.Windows.Forms.Padding(2);
            this.tpFilterLog.Name = "tpFilterLog";
            this.tpFilterLog.Size = new System.Drawing.Size(1096, 718);
            this.tpFilterLog.TabIndex = 1;
            this.tpFilterLog.Text = "滤镜日志";
            // 
            // tFilterLog
            // 
            this.tFilterLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tFilterLog.CellImpactHeight = false;
            this.tFilterLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterLog.Gap = 8;
            this.tFilterLog.GapCell = 5;
            this.tFilterLog.Gaps = new System.Drawing.Size(8, 8);
            this.tFilterLog.Location = new System.Drawing.Point(0, 0);
            this.tFilterLog.Margin = new System.Windows.Forms.Padding(2);
            this.tFilterLog.Name = "tFilterLog";
            this.tFilterLog.Size = new System.Drawing.Size(1096, 718);
            this.tFilterLog.TabIndex = 11;
            this.tFilterLog.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterLog_CellClick);
            // 
            // tpProxyLog
            // 
            this.tpProxyLog.Controls.Add(this.tProxyLog);
            this.tpProxyLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxyLog.LocalizationText = "LogList.LogList.{id}";
            this.tpProxyLog.Location = new System.Drawing.Point(0, 38);
            this.tpProxyLog.Margin = new System.Windows.Forms.Padding(2);
            this.tpProxyLog.Name = "tpProxyLog";
            this.tpProxyLog.Size = new System.Drawing.Size(1096, 718);
            this.tpProxyLog.TabIndex = 2;
            this.tpProxyLog.Text = "代理日志";
            // 
            // tProxyLog
            // 
            this.tProxyLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProxyLog.CellImpactHeight = false;
            this.tProxyLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProxyLog.Gap = 8;
            this.tProxyLog.GapCell = 5;
            this.tProxyLog.Gaps = new System.Drawing.Size(8, 8);
            this.tProxyLog.Location = new System.Drawing.Point(0, 0);
            this.tProxyLog.Margin = new System.Windows.Forms.Padding(2);
            this.tProxyLog.Name = "tProxyLog";
            this.tProxyLog.Size = new System.Drawing.Size(1096, 718);
            this.tProxyLog.TabIndex = 10;
            this.tProxyLog.CellClick += new AntdUI.Table.ClickEventHandler(this.tProxyLog_CellClick);
            // 
            // LogList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLogList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LogList";
            this.Size = new System.Drawing.Size(1100, 800);
            this.Load += new System.EventHandler(this.LogList_Load);
            this.tlpLogList.ResumeLayout(false);
            this.tlpLogList_Button.ResumeLayout(false);
            this.tlpLogList_Button.PerformLayout();
            this.tabLogList.ResumeLayout(false);
            this.tpSystemLog.ResumeLayout(false);
            this.tpFilterLog.ResumeLayout(false);
            this.tpProxyLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpLogList;
        private AntdUI.Tabs tabLogList;
        private AntdUI.TabPage tpSystemLog;
        private AntdUI.Table tSystemLog;
        private AntdUI.TabPage tpFilterLog;
        private AntdUI.TabPage tpProxyLog;
        private AntdUI.Table tProxyLog;
        private AntdUI.Table tFilterLog;
        private TableLayoutPanelEx tlpLogList_Button;
        private AntdUI.InputNumber txtLogList_AutoClear;
        private AntdUI.Checkbox cbLogList_AutoClear;
        private AntdUI.Checkbox cbLogList_AutoRoll;
    }
}

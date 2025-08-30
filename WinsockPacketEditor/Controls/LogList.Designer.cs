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
            this.tlpLogList = new System.Windows.Forms.TableLayoutPanel();
            this.tabLogList = new AntdUI.Tabs();
            this.tpSystemLog = new AntdUI.TabPage();
            this.tSystemLog = new AntdUI.Table();
            this.tpFilterLog = new AntdUI.TabPage();
            this.tFilterLog = new AntdUI.Table();
            this.tpProxyLog = new AntdUI.TabPage();
            this.tProxyLog = new AntdUI.Table();
            this.tlpLogList.SuspendLayout();
            this.tabLogList.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
            this.tpFilterLog.SuspendLayout();
            this.tpProxyLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLogList
            // 
            this.tlpLogList.ColumnCount = 3;
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpLogList.Controls.Add(this.tabLogList, 1, 1);
            this.tlpLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLogList.Location = new System.Drawing.Point(0, 0);
            this.tlpLogList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLogList.Name = "tlpLogList";
            this.tlpLogList.RowCount = 2;
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLogList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.Size = new System.Drawing.Size(800, 800);
            this.tlpLogList.TabIndex = 0;
            // 
            // tabLogList
            // 
            this.tabLogList.Controls.Add(this.tpSystemLog);
            this.tabLogList.Controls.Add(this.tpFilterLog);
            this.tabLogList.Controls.Add(this.tpProxyLog);
            this.tabLogList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLogList.Gap = 20;
            this.tabLogList.Location = new System.Drawing.Point(33, 23);
            this.tabLogList.Name = "tabLogList";
            this.tabLogList.Pages.Add(this.tpSystemLog);
            this.tabLogList.Pages.Add(this.tpFilterLog);
            this.tabLogList.Pages.Add(this.tpProxyLog);
            this.tabLogList.Size = new System.Drawing.Size(734, 774);
            this.tabLogList.Style = styleCard1;
            this.tabLogList.TabIndex = 0;
            this.tabLogList.Type = AntdUI.TabType.Card;
            // 
            // tpSystemLog
            // 
            this.tpSystemLog.Controls.Add(this.tSystemLog);
            this.tpSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSystemLog.LocalizationText = "LogList.LogList.{id}";
            this.tpSystemLog.Location = new System.Drawing.Point(0, 45);
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.Size = new System.Drawing.Size(734, 729);
            this.tpSystemLog.TabIndex = 0;
            this.tpSystemLog.Text = "系统日志";
            // 
            // tSystemLog
            // 
            this.tSystemLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSystemLog.CellImpactHeight = false;
            this.tSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSystemLog.Gap = 12;
            this.tSystemLog.Location = new System.Drawing.Point(0, 0);
            this.tSystemLog.MultipleRows = true;
            this.tSystemLog.Name = "tSystemLog";
            this.tSystemLog.Size = new System.Drawing.Size(734, 729);
            this.tSystemLog.TabIndex = 5;
            this.tSystemLog.Text = "table1";
            this.tSystemLog.CellClick += new AntdUI.Table.ClickEventHandler(this.tSystemLog_CellClick);
            // 
            // tpFilterLog
            // 
            this.tpFilterLog.Controls.Add(this.tFilterLog);
            this.tpFilterLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterLog.LocalizationText = "LogList.LogList.{id}";
            this.tpFilterLog.Location = new System.Drawing.Point(0, 45);
            this.tpFilterLog.Name = "tpFilterLog";
            this.tpFilterLog.Size = new System.Drawing.Size(734, 729);
            this.tpFilterLog.TabIndex = 1;
            this.tpFilterLog.Text = "滤镜日志";
            // 
            // tFilterLog
            // 
            this.tFilterLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tFilterLog.CellImpactHeight = false;
            this.tFilterLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterLog.Gap = 12;
            this.tFilterLog.Location = new System.Drawing.Point(0, 0);
            this.tFilterLog.Name = "tFilterLog";
            this.tFilterLog.Size = new System.Drawing.Size(734, 729);
            this.tFilterLog.TabIndex = 11;
            // 
            // tpProxyLog
            // 
            this.tpProxyLog.Controls.Add(this.tProxyLog);
            this.tpProxyLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxyLog.LocalizationText = "LogList.LogList.{id}";
            this.tpProxyLog.Location = new System.Drawing.Point(0, 45);
            this.tpProxyLog.Name = "tpProxyLog";
            this.tpProxyLog.Size = new System.Drawing.Size(734, 729);
            this.tpProxyLog.TabIndex = 2;
            this.tpProxyLog.Text = "代理日志";
            // 
            // tProxyLog
            // 
            this.tProxyLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProxyLog.CellImpactHeight = false;
            this.tProxyLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProxyLog.Gap = 12;
            this.tProxyLog.Location = new System.Drawing.Point(0, 0);
            this.tProxyLog.Name = "tProxyLog";
            this.tProxyLog.Size = new System.Drawing.Size(734, 729);
            this.tProxyLog.TabIndex = 10;
            // 
            // LogList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLogList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "LogList";
            this.Size = new System.Drawing.Size(800, 800);
            this.Load += new System.EventHandler(this.LogList_Load);
            this.tlpLogList.ResumeLayout(false);
            this.tabLogList.ResumeLayout(false);
            this.tpSystemLog.ResumeLayout(false);
            this.tpFilterLog.ResumeLayout(false);
            this.tpProxyLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLogList;
        private AntdUI.Tabs tabLogList;
        private AntdUI.TabPage tpSystemLog;
        private AntdUI.Table tSystemLog;
        private AntdUI.TabPage tpFilterLog;
        private AntdUI.TabPage tpProxyLog;
        private AntdUI.Table tProxyLog;
        private AntdUI.Table tFilterLog;
    }
}

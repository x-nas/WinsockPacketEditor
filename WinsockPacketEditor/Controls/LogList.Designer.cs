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
            this.tlpLogList = new System.Windows.Forms.TableLayoutPanel();
            this.tSystemLog = new AntdUI.Table();
            this.tlpLogList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLogList
            // 
            this.tlpLogList.ColumnCount = 3;
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLogList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpLogList.Controls.Add(this.tSystemLog, 1, 1);
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
            // tSystemLog
            // 
            this.tSystemLog.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSystemLog.CellImpactHeight = false;
            this.tSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSystemLog.EmptyHeader = true;
            this.tSystemLog.Gap = 8;
            this.tSystemLog.GapCell = 0;
            this.tSystemLog.Gaps = new System.Drawing.Size(8, 8);
            this.tSystemLog.Location = new System.Drawing.Point(33, 23);
            this.tSystemLog.MultipleRows = true;
            this.tSystemLog.Name = "tSystemLog";
            this.tSystemLog.Size = new System.Drawing.Size(734, 774);
            this.tSystemLog.TabIndex = 4;
            this.tSystemLog.Text = "table1";
            this.tSystemLog.CellClick += new AntdUI.Table.ClickEventHandler(this.tSystemLog_CellClick);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLogList;
        private AntdUI.Table tSystemLog;
    }
}

namespace WinsockPacketEditor
{
    partial class QuickList
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
            this.tlpQuickList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tabQuickList = new AntdUI.Tabs();
            this.tpFilterList = new AntdUI.TabPage();
            this.tlpFilterList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tFilterList = new AntdUI.Table();
            this.tpSendList = new AntdUI.TabPage();
            this.tlpSendList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tpRobotList = new AntdUI.TabPage();
            this.tlpRobotList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpQuickList.SuspendLayout();
            this.tabQuickList.SuspendLayout();
            this.tpFilterList.SuspendLayout();
            this.tlpFilterList.SuspendLayout();
            this.tpSendList.SuspendLayout();
            this.tpRobotList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpQuickList
            // 
            this.tlpQuickList.ColumnCount = 1;
            this.tlpQuickList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpQuickList.Controls.Add(this.tabQuickList, 0, 0);
            this.tlpQuickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpQuickList.Location = new System.Drawing.Point(0, 0);
            this.tlpQuickList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpQuickList.Name = "tlpQuickList";
            this.tlpQuickList.RowCount = 1;
            this.tlpQuickList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpQuickList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpQuickList.Size = new System.Drawing.Size(500, 500);
            this.tlpQuickList.TabIndex = 0;
            // 
            // tabQuickList
            // 
            this.tabQuickList.Controls.Add(this.tpFilterList);
            this.tabQuickList.Controls.Add(this.tpSendList);
            this.tabQuickList.Controls.Add(this.tpRobotList);
            this.tabQuickList.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabQuickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabQuickList.Location = new System.Drawing.Point(0, 0);
            this.tabQuickList.Margin = new System.Windows.Forms.Padding(0);
            this.tabQuickList.Name = "tabQuickList";
            this.tabQuickList.Pages.Add(this.tpFilterList);
            this.tabQuickList.Pages.Add(this.tpSendList);
            this.tabQuickList.Pages.Add(this.tpRobotList);
            this.tabQuickList.Size = new System.Drawing.Size(500, 500);
            this.tabQuickList.Style = styleCard1;
            this.tabQuickList.TabIndex = 0;
            this.tabQuickList.Type = AntdUI.TabType.Card;
            // 
            // tpFilterList
            // 
            this.tpFilterList.Controls.Add(this.tlpFilterList);
            this.tpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilterList.LocalizationText = "FilterList";
            this.tpFilterList.Location = new System.Drawing.Point(0, 24);
            this.tpFilterList.Name = "tpFilterList";
            this.tpFilterList.Size = new System.Drawing.Size(500, 476);
            this.tpFilterList.TabIndex = 0;
            this.tpFilterList.Text = "滤镜列表";
            // 
            // tlpFilterList
            // 
            this.tlpFilterList.ColumnCount = 1;
            this.tlpFilterList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.Controls.Add(this.tFilterList, 0, 0);
            this.tlpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterList.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterList.Name = "tlpFilterList";
            this.tlpFilterList.RowCount = 1;
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterList.Size = new System.Drawing.Size(500, 476);
            this.tlpFilterList.TabIndex = 0;
            // 
            // tFilterList
            // 
            this.tFilterList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterList.Gap = 8;
            this.tFilterList.GapCell = 5;
            this.tFilterList.Gaps = new System.Drawing.Size(8, 8);
            this.tFilterList.Location = new System.Drawing.Point(0, 0);
            this.tFilterList.Margin = new System.Windows.Forms.Padding(0);
            this.tFilterList.Name = "tFilterList";
            this.tFilterList.Size = new System.Drawing.Size(500, 476);
            this.tFilterList.SwitchSize = 12;
            this.tFilterList.TabIndex = 1;
            this.tFilterList.VisibleHeader = false;
            // 
            // tpSendList
            // 
            this.tpSendList.Controls.Add(this.tlpSendList);
            this.tpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSendList.LocalizationText = "SendList";
            this.tpSendList.Location = new System.Drawing.Point(0, 24);
            this.tpSendList.Name = "tpSendList";
            this.tpSendList.Size = new System.Drawing.Size(500, 476);
            this.tpSendList.TabIndex = 1;
            this.tpSendList.Text = "发送列表";
            // 
            // tlpSendList
            // 
            this.tlpSendList.ColumnCount = 1;
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendList.Location = new System.Drawing.Point(0, 0);
            this.tlpSendList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendList.Name = "tlpSendList";
            this.tlpSendList.RowCount = 2;
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.Size = new System.Drawing.Size(500, 476);
            this.tlpSendList.TabIndex = 1;
            // 
            // tpRobotList
            // 
            this.tpRobotList.Controls.Add(this.tlpRobotList);
            this.tpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpRobotList.LocalizationText = "RobotList";
            this.tpRobotList.Location = new System.Drawing.Point(0, 24);
            this.tpRobotList.Name = "tpRobotList";
            this.tpRobotList.Size = new System.Drawing.Size(500, 476);
            this.tpRobotList.TabIndex = 2;
            this.tpRobotList.Text = "机器人列表";
            // 
            // tlpRobotList
            // 
            this.tlpRobotList.ColumnCount = 1;
            this.tlpRobotList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotList.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotList.Name = "tlpRobotList";
            this.tlpRobotList.RowCount = 2;
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRobotList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotList.Size = new System.Drawing.Size(500, 476);
            this.tlpRobotList.TabIndex = 1;
            // 
            // QuickList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpQuickList);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "QuickList";
            this.Size = new System.Drawing.Size(500, 500);
            this.Load += new System.EventHandler(this.QuickList_Load);
            this.tlpQuickList.ResumeLayout(false);
            this.tabQuickList.ResumeLayout(false);
            this.tpFilterList.ResumeLayout(false);
            this.tlpFilterList.ResumeLayout(false);
            this.tpSendList.ResumeLayout(false);
            this.tpRobotList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpQuickList;
        private AntdUI.Tabs tabQuickList;
        private AntdUI.TabPage tpFilterList;
        private AntdUI.TabPage tpSendList;
        private AntdUI.TabPage tpRobotList;
        private TableLayoutPanelEx tlpFilterList;
        private TableLayoutPanelEx tlpRobotList;
        private TableLayoutPanelEx tlpSendList;
        private AntdUI.Table tFilterList;
    }
}

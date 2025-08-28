namespace WinsockPacketEditor
{
    partial class ClientList
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
            this.bgwClientList = new System.ComponentModel.BackgroundWorker();
            this.tlpClientList = new System.Windows.Forms.TableLayoutPanel();
            this.splitterClientList = new AntdUI.Splitter();
            this.treeClientList = new AntdUI.Tree();
            this.tlpAuthListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lDevicesCount_Value = new AntdUI.Label();
            this.lLinksCount_Value = new AntdUI.Label();
            this.lAuthCount_Value = new AntdUI.Label();
            this.lSplit13 = new AntdUI.Label();
            this.lSplit3 = new AntdUI.Label();
            this.lDevicesCount = new AntdUI.Label();
            this.lLinksCount = new AntdUI.Label();
            this.lAuthCount = new AntdUI.Label();
            this.tabClientList = new AntdUI.Tabs();
            this.tpAuthList = new AntdUI.TabPage();
            this.tAuthList = new AntdUI.Table();
            this.tpProxyLog = new AntdUI.TabPage();
            this.tProxyLog = new AntdUI.Table();
            this.tlpClientList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).BeginInit();
            this.splitterClientList.Panel1.SuspendLayout();
            this.splitterClientList.Panel2.SuspendLayout();
            this.splitterClientList.SuspendLayout();
            this.tlpAuthListInfo.SuspendLayout();
            this.tabClientList.SuspendLayout();
            this.tpAuthList.SuspendLayout();
            this.tpProxyLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwClientList
            // 
            this.bgwClientList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwClientList_DoWork);
            this.bgwClientList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwClientList_RunWorkerCompleted);
            // 
            // tlpClientList
            // 
            this.tlpClientList.ColumnCount = 3;
            this.tlpClientList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpClientList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpClientList.Controls.Add(this.tlpAuthListInfo, 1, 1);
            this.tlpClientList.Controls.Add(this.splitterClientList, 1, 2);
            this.tlpClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpClientList.Location = new System.Drawing.Point(0, 0);
            this.tlpClientList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpClientList.Name = "tlpClientList";
            this.tlpClientList.RowCount = 3;
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.Size = new System.Drawing.Size(1200, 800);
            this.tlpClientList.TabIndex = 0;
            // 
            // splitterClientList
            // 
            this.splitterClientList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterClientList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterClientList.Location = new System.Drawing.Point(33, 53);
            this.splitterClientList.Name = "splitterClientList";
            // 
            // splitterClientList.Panel1
            // 
            this.splitterClientList.Panel1.Controls.Add(this.treeClientList);
            this.splitterClientList.Panel1MinSize = 0;
            // 
            // splitterClientList.Panel2
            // 
            this.splitterClientList.Panel2.Controls.Add(this.tabClientList);
            this.splitterClientList.Panel2MinSize = 0;
            this.splitterClientList.Size = new System.Drawing.Size(1134, 744);
            this.splitterClientList.SplitterDistance = 280;
            this.splitterClientList.SplitterSize = 80;
            this.splitterClientList.SplitterWidth = 10;
            this.splitterClientList.TabIndex = 2;
            // 
            // treeClientList
            // 
            this.treeClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeClientList.Location = new System.Drawing.Point(0, 0);
            this.treeClientList.Name = "treeClientList";
            this.treeClientList.Size = new System.Drawing.Size(280, 744);
            this.treeClientList.TabIndex = 0;
            // 
            // tlpAuthListInfo
            // 
            this.tlpAuthListInfo.ColumnCount = 9;
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAuthListInfo.Controls.Add(this.lDevicesCount_Value, 7, 1);
            this.tlpAuthListInfo.Controls.Add(this.lLinksCount_Value, 4, 1);
            this.tlpAuthListInfo.Controls.Add(this.lAuthCount_Value, 1, 1);
            this.tlpAuthListInfo.Controls.Add(this.lSplit13, 5, 1);
            this.tlpAuthListInfo.Controls.Add(this.lSplit3, 2, 1);
            this.tlpAuthListInfo.Controls.Add(this.lDevicesCount, 6, 1);
            this.tlpAuthListInfo.Controls.Add(this.lLinksCount, 3, 1);
            this.tlpAuthListInfo.Controls.Add(this.lAuthCount, 0, 1);
            this.tlpAuthListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAuthListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpAuthListInfo.Location = new System.Drawing.Point(30, 20);
            this.tlpAuthListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAuthListInfo.Name = "tlpAuthListInfo";
            this.tlpAuthListInfo.RowCount = 3;
            this.tlpAuthListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAuthListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAuthListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAuthListInfo.Size = new System.Drawing.Size(1140, 30);
            this.tlpAuthListInfo.TabIndex = 7;
            // 
            // lDevicesCount_Value
            // 
            this.lDevicesCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDevicesCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDevicesCount_Value.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDevicesCount_Value.Location = new System.Drawing.Point(297, 3);
            this.lDevicesCount_Value.Name = "lDevicesCount_Value";
            this.lDevicesCount_Value.Size = new System.Drawing.Size(10, 24);
            this.lDevicesCount_Value.TabIndex = 14;
            this.lDevicesCount_Value.Text = "0";
            // 
            // lLinksCount_Value
            // 
            this.lLinksCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lLinksCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lLinksCount_Value.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lLinksCount_Value.Location = new System.Drawing.Point(195, 3);
            this.lLinksCount_Value.Name = "lLinksCount_Value";
            this.lLinksCount_Value.Size = new System.Drawing.Size(10, 24);
            this.lLinksCount_Value.TabIndex = 13;
            this.lLinksCount_Value.Text = "0";
            // 
            // lAuthCount_Value
            // 
            this.lAuthCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAuthCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAuthCount_Value.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAuthCount_Value.Location = new System.Drawing.Point(93, 3);
            this.lAuthCount_Value.Name = "lAuthCount_Value";
            this.lAuthCount_Value.Size = new System.Drawing.Size(10, 24);
            this.lAuthCount_Value.TabIndex = 12;
            this.lAuthCount_Value.Text = "0";
            // 
            // lSplit13
            // 
            this.lSplit13.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit13.ForeColor = System.Drawing.Color.Silver;
            this.lSplit13.Location = new System.Drawing.Point(211, 3);
            this.lSplit13.Name = "lSplit13";
            this.lSplit13.Size = new System.Drawing.Size(6, 24);
            this.lSplit13.TabIndex = 9;
            this.lSplit13.Text = "|";
            this.lSplit13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSplit3
            // 
            this.lSplit3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit3.ForeColor = System.Drawing.Color.Silver;
            this.lSplit3.Location = new System.Drawing.Point(109, 3);
            this.lSplit3.Name = "lSplit3";
            this.lSplit3.Size = new System.Drawing.Size(6, 24);
            this.lSplit3.TabIndex = 8;
            this.lSplit3.Text = "|";
            this.lSplit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lDevicesCount
            // 
            this.lDevicesCount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDevicesCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDevicesCount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDevicesCount.LocalizationText = "ClientList.TotalDevices";
            this.lDevicesCount.Location = new System.Drawing.Point(223, 3);
            this.lDevicesCount.Name = "lDevicesCount";
            this.lDevicesCount.Size = new System.Drawing.Size(68, 24);
            this.lDevicesCount.TabIndex = 7;
            this.lDevicesCount.Text = "设备总数:";
            // 
            // lLinksCount
            // 
            this.lLinksCount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lLinksCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lLinksCount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lLinksCount.LocalizationText = "ClientList.TotalLinks";
            this.lLinksCount.Location = new System.Drawing.Point(121, 3);
            this.lLinksCount.Name = "lLinksCount";
            this.lLinksCount.Size = new System.Drawing.Size(68, 24);
            this.lLinksCount.TabIndex = 6;
            this.lLinksCount.Text = "链接总数:";
            // 
            // lAuthCount
            // 
            this.lAuthCount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAuthCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAuthCount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAuthCount.LocalizationText = "ClientList.TotalClients";
            this.lAuthCount.Location = new System.Drawing.Point(3, 3);
            this.lAuthCount.Name = "lAuthCount";
            this.lAuthCount.Size = new System.Drawing.Size(84, 24);
            this.lAuthCount.TabIndex = 5;
            this.lAuthCount.Text = "客户端总数:";
            // 
            // tabClientList
            // 
            this.tabClientList.Controls.Add(this.tpAuthList);
            this.tabClientList.Controls.Add(this.tpProxyLog);
            this.tabClientList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabClientList.Gap = 20;
            this.tabClientList.Location = new System.Drawing.Point(0, 0);
            this.tabClientList.Name = "tabClientList";
            this.tabClientList.Pages.Add(this.tpAuthList);
            this.tabClientList.Pages.Add(this.tpProxyLog);
            this.tabClientList.Size = new System.Drawing.Size(844, 744);
            this.tabClientList.Style = styleCard1;
            this.tabClientList.TabIndex = 8;
            this.tabClientList.Type = AntdUI.TabType.Card;
            // 
            // tpAuthList
            // 
            this.tpAuthList.Controls.Add(this.tAuthList);
            this.tpAuthList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpAuthList.LocalizationText = "ClientList.{id}";
            this.tpAuthList.Location = new System.Drawing.Point(3, 45);
            this.tpAuthList.Name = "tpAuthList";
            this.tpAuthList.Size = new System.Drawing.Size(838, 696);
            this.tpAuthList.TabIndex = 0;
            this.tpAuthList.Text = "认证记录";
            // 
            // tAuthList
            // 
            this.tAuthList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAuthList.CellImpactHeight = false;
            this.tAuthList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAuthList.Gap = 12;
            this.tAuthList.Location = new System.Drawing.Point(0, 0);
            this.tAuthList.Name = "tAuthList";
            this.tAuthList.Size = new System.Drawing.Size(838, 696);
            this.tAuthList.TabIndex = 8;
            // 
            // tpProxyLog
            // 
            this.tpProxyLog.Controls.Add(this.tProxyLog);
            this.tpProxyLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxyLog.LocalizationText = "ClientList.{id}";
            this.tpProxyLog.Location = new System.Drawing.Point(3, 45);
            this.tpProxyLog.Name = "tpProxyLog";
            this.tpProxyLog.Size = new System.Drawing.Size(838, 696);
            this.tpProxyLog.TabIndex = 1;
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
            this.tProxyLog.Size = new System.Drawing.Size(838, 696);
            this.tProxyLog.TabIndex = 9;
            // 
            // ClientList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpClientList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ClientList";
            this.Size = new System.Drawing.Size(1200, 800);
            this.Load += new System.EventHandler(this.ClientList_Load);
            this.tlpClientList.ResumeLayout(false);
            this.splitterClientList.Panel1.ResumeLayout(false);
            this.splitterClientList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).EndInit();
            this.splitterClientList.ResumeLayout(false);
            this.tlpAuthListInfo.ResumeLayout(false);
            this.tlpAuthListInfo.PerformLayout();
            this.tabClientList.ResumeLayout(false);
            this.tpAuthList.ResumeLayout(false);
            this.tpProxyLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker bgwClientList;
        private System.Windows.Forms.TableLayoutPanel tlpClientList;
        private AntdUI.Splitter splitterClientList;
        private AntdUI.Tree treeClientList;
        private System.Windows.Forms.TableLayoutPanel tlpAuthListInfo;
        private AntdUI.Label lDevicesCount_Value;
        private AntdUI.Label lLinksCount_Value;
        private AntdUI.Label lAuthCount_Value;
        private AntdUI.Label lSplit13;
        private AntdUI.Label lSplit3;
        private AntdUI.Label lDevicesCount;
        private AntdUI.Label lLinksCount;
        private AntdUI.Label lAuthCount;
        private AntdUI.Tabs tabClientList;
        private AntdUI.TabPage tpAuthList;
        private AntdUI.Table tAuthList;
        private AntdUI.TabPage tpProxyLog;
        private AntdUI.Table tProxyLog;
    }
}

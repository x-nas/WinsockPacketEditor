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
            this.splitterClientList = new AntdUI.Splitter();
            this.treeClientList = new AntdUI.Tree();
            this.tlpAuthInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tAuthList = new AntdUI.Table();
            this.tlpAuthListInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lDevicesCount_Value = new AntdUI.Label();
            this.lLinksCount_Value = new AntdUI.Label();
            this.lAuthCount_Value = new AntdUI.Label();
            this.lSplit13 = new AntdUI.Label();
            this.lSplit3 = new AntdUI.Label();
            this.lDevicesCount = new AntdUI.Label();
            this.lLinksCount = new AntdUI.Label();
            this.lAuthCount = new AntdUI.Label();
            this.bgwClientList = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).BeginInit();
            this.splitterClientList.Panel1.SuspendLayout();
            this.splitterClientList.Panel2.SuspendLayout();
            this.splitterClientList.SuspendLayout();
            this.tlpAuthInfo.SuspendLayout();
            this.tlpAuthListInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitterClientList
            // 
            this.splitterClientList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterClientList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterClientList.Location = new System.Drawing.Point(0, 0);
            this.splitterClientList.Name = "splitterClientList";
            // 
            // splitterClientList.Panel1
            // 
            this.splitterClientList.Panel1.Controls.Add(this.treeClientList);
            this.splitterClientList.Panel1MinSize = 0;
            // 
            // splitterClientList.Panel2
            // 
            this.splitterClientList.Panel2.Controls.Add(this.tlpAuthInfo);
            this.splitterClientList.Panel2MinSize = 0;
            this.splitterClientList.Size = new System.Drawing.Size(1200, 800);
            this.splitterClientList.SplitterDistance = 298;
            this.splitterClientList.SplitterSize = 80;
            this.splitterClientList.SplitterWidth = 10;
            this.splitterClientList.TabIndex = 1;
            // 
            // treeClientList
            // 
            this.treeClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeClientList.Location = new System.Drawing.Point(0, 0);
            this.treeClientList.Name = "treeClientList";
            this.treeClientList.Size = new System.Drawing.Size(298, 800);
            this.treeClientList.TabIndex = 0;
            // 
            // tlpAuthInfo
            // 
            this.tlpAuthInfo.ColumnCount = 1;
            this.tlpAuthInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAuthInfo.Controls.Add(this.tAuthList, 0, 1);
            this.tlpAuthInfo.Controls.Add(this.tlpAuthListInfo, 0, 0);
            this.tlpAuthInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAuthInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpAuthInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAuthInfo.Name = "tlpAuthInfo";
            this.tlpAuthInfo.RowCount = 2;
            this.tlpAuthInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAuthInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAuthInfo.Size = new System.Drawing.Size(892, 800);
            this.tlpAuthInfo.TabIndex = 0;
            // 
            // tAuthList
            // 
            this.tAuthList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAuthList.CellImpactHeight = false;
            this.tAuthList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAuthList.Gap = 12;
            this.tAuthList.GapCell = 6;
            this.tAuthList.Location = new System.Drawing.Point(3, 33);
            this.tAuthList.Name = "tAuthList";
            this.tAuthList.Size = new System.Drawing.Size(886, 764);
            this.tAuthList.TabIndex = 7;
            // 
            // tlpAuthListInfo
            // 
            this.tlpAuthListInfo.ColumnCount = 10;
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAuthListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAuthListInfo.Controls.Add(this.lDevicesCount_Value, 8, 1);
            this.tlpAuthListInfo.Controls.Add(this.lLinksCount_Value, 5, 1);
            this.tlpAuthListInfo.Controls.Add(this.lAuthCount_Value, 2, 1);
            this.tlpAuthListInfo.Controls.Add(this.lSplit13, 6, 1);
            this.tlpAuthListInfo.Controls.Add(this.lSplit3, 3, 1);
            this.tlpAuthListInfo.Controls.Add(this.lDevicesCount, 7, 1);
            this.tlpAuthListInfo.Controls.Add(this.lLinksCount, 4, 1);
            this.tlpAuthListInfo.Controls.Add(this.lAuthCount, 1, 1);
            this.tlpAuthListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAuthListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpAuthListInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpAuthListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAuthListInfo.Name = "tlpAuthListInfo";
            this.tlpAuthListInfo.RowCount = 3;
            this.tlpAuthListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAuthListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAuthListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAuthListInfo.Size = new System.Drawing.Size(892, 30);
            this.tlpAuthListInfo.TabIndex = 6;
            // 
            // lDevicesCount_Value
            // 
            this.lDevicesCount_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDevicesCount_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDevicesCount_Value.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDevicesCount_Value.Location = new System.Drawing.Point(302, 3);
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
            this.lLinksCount_Value.Location = new System.Drawing.Point(200, 3);
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
            this.lAuthCount_Value.Location = new System.Drawing.Point(98, 3);
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
            this.lSplit13.Location = new System.Drawing.Point(216, 3);
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
            this.lSplit3.Location = new System.Drawing.Point(114, 3);
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
            this.lDevicesCount.Location = new System.Drawing.Point(228, 3);
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
            this.lLinksCount.Location = new System.Drawing.Point(126, 3);
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
            this.lAuthCount.Location = new System.Drawing.Point(8, 3);
            this.lAuthCount.Name = "lAuthCount";
            this.lAuthCount.Size = new System.Drawing.Size(84, 24);
            this.lAuthCount.TabIndex = 5;
            this.lAuthCount.Text = "客户端总数:";
            // 
            // bgwClientList
            // 
            this.bgwClientList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwClientList_DoWork);
            this.bgwClientList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwClientList_RunWorkerCompleted);
            // 
            // ClientList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitterClientList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ClientList";
            this.Size = new System.Drawing.Size(1200, 800);
            this.Load += new System.EventHandler(this.ClientList_Load);
            this.splitterClientList.Panel1.ResumeLayout(false);
            this.splitterClientList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).EndInit();
            this.splitterClientList.ResumeLayout(false);
            this.tlpAuthInfo.ResumeLayout(false);
            this.tlpAuthListInfo.ResumeLayout(false);
            this.tlpAuthListInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Splitter splitterClientList;
        private AntdUI.Tree treeClientList;
        private System.Windows.Forms.TableLayoutPanel tlpAuthInfo;
        private AntdUI.Table tAuthList;
        private System.Windows.Forms.TableLayoutPanel tlpAuthListInfo;
        private AntdUI.Label lDevicesCount_Value;
        private AntdUI.Label lLinksCount_Value;
        private AntdUI.Label lAuthCount_Value;
        private AntdUI.Label lSplit13;
        private AntdUI.Label lSplit3;
        private AntdUI.Label lDevicesCount;
        private AntdUI.Label lLinksCount;
        private AntdUI.Label lAuthCount;
        private System.ComponentModel.BackgroundWorker bgwClientList;
    }
}

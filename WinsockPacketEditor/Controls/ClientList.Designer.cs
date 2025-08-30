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
            this.bgwClientList = new System.ComponentModel.BackgroundWorker();
            this.tlpClientList = new System.Windows.Forms.TableLayoutPanel();
            this.splitterClientList = new AntdUI.Splitter();
            this.treeClientList = new AntdUI.Tree();
            this.tAuthList = new AntdUI.Table();
            this.tlpClientList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).BeginInit();
            this.splitterClientList.Panel1.SuspendLayout();
            this.splitterClientList.Panel2.SuspendLayout();
            this.splitterClientList.SuspendLayout();
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
            this.tlpClientList.Controls.Add(this.splitterClientList, 1, 1);
            this.tlpClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpClientList.Location = new System.Drawing.Point(0, 0);
            this.tlpClientList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpClientList.Name = "tlpClientList";
            this.tlpClientList.RowCount = 2;
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpClientList.Size = new System.Drawing.Size(1200, 800);
            this.tlpClientList.TabIndex = 0;
            // 
            // splitterClientList
            // 
            this.splitterClientList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterClientList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterClientList.Location = new System.Drawing.Point(33, 23);
            this.splitterClientList.Name = "splitterClientList";
            // 
            // splitterClientList.Panel1
            // 
            this.splitterClientList.Panel1.Controls.Add(this.treeClientList);
            this.splitterClientList.Panel1MinSize = 0;
            // 
            // splitterClientList.Panel2
            // 
            this.splitterClientList.Panel2.Controls.Add(this.tAuthList);
            this.splitterClientList.Panel2MinSize = 0;
            this.splitterClientList.Size = new System.Drawing.Size(1134, 774);
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
            this.treeClientList.Size = new System.Drawing.Size(280, 774);
            this.treeClientList.TabIndex = 0;
            // 
            // tAuthList
            // 
            this.tAuthList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAuthList.CellImpactHeight = false;
            this.tAuthList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAuthList.Gap = 12;
            this.tAuthList.Location = new System.Drawing.Point(0, 0);
            this.tAuthList.Name = "tAuthList";
            this.tAuthList.Size = new System.Drawing.Size(844, 774);
            this.tAuthList.TabIndex = 9;
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker bgwClientList;
        private System.Windows.Forms.TableLayoutPanel tlpClientList;
        private AntdUI.Splitter splitterClientList;
        private AntdUI.Tree treeClientList;
        private AntdUI.Table tAuthList;
    }
}

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
            this.tlpClientList = new WinsockPacketEditor.TableLayoutPanelEx();
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
            // tlpClientList
            // 
            this.tlpClientList.ColumnCount = 1;
            this.tlpClientList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.Controls.Add(this.splitterClientList, 0, 0);
            this.tlpClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpClientList.Location = new System.Drawing.Point(0, 0);
            this.tlpClientList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpClientList.Name = "tlpClientList";
            this.tlpClientList.RowCount = 1;
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 486F));
            this.tlpClientList.Size = new System.Drawing.Size(1100, 600);
            this.tlpClientList.TabIndex = 0;
            // 
            // splitterClientList
            // 
            this.splitterClientList.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterClientList.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterClientList.Location = new System.Drawing.Point(2, 2);
            this.splitterClientList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.splitterClientList.Size = new System.Drawing.Size(1096, 596);
            this.splitterClientList.SplitterDistance = 270;
            this.splitterClientList.SplitterSize = 80;
            this.splitterClientList.SplitterWidth = 10;
            this.splitterClientList.TabIndex = 2;
            // 
            // treeClientList
            // 
            this.treeClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeClientList.Location = new System.Drawing.Point(0, 0);
            this.treeClientList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeClientList.Name = "treeClientList";
            this.treeClientList.Size = new System.Drawing.Size(270, 596);
            this.treeClientList.TabIndex = 0;
            // 
            // tAuthList
            // 
            this.tAuthList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAuthList.CellImpactHeight = false;
            this.tAuthList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAuthList.Gap = 12;
            this.tAuthList.Location = new System.Drawing.Point(0, 0);
            this.tAuthList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tAuthList.Name = "tAuthList";
            this.tAuthList.Size = new System.Drawing.Size(819, 596);
            this.tAuthList.TabIndex = 9;
            // 
            // ClientList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpClientList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ClientList";
            this.Size = new System.Drawing.Size(1100, 600);
            this.Load += new System.EventHandler(this.ClientList_Load);
            this.tlpClientList.ResumeLayout(false);
            this.splitterClientList.Panel1.ResumeLayout(false);
            this.splitterClientList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterClientList)).EndInit();
            this.splitterClientList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TableLayoutPanelEx tlpClientList;
        private AntdUI.Splitter splitterClientList;
        private AntdUI.Tree treeClientList;
        private AntdUI.Table tAuthList;
    }
}

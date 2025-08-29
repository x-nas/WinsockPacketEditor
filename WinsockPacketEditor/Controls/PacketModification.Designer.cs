namespace WinsockPacketEditor
{
    partial class PacketModification
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
            this.tlpModification = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bExit = new AntdUI.Button();
            this.splitterModification = new AntdUI.Splitter();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.txtPacketData_New = new AntdUI.Input();
            this.lPacketData_New = new AntdUI.Label();
            this.lPacketData_Raw = new AntdUI.Label();
            this.txtPacketData_Raw = new AntdUI.Input();
            this.tPacketModification = new AntdUI.Table();
            this.tlpModification.SuspendLayout();
            this.tlpButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterModification)).BeginInit();
            this.splitterModification.Panel1.SuspendLayout();
            this.splitterModification.Panel2.SuspendLayout();
            this.splitterModification.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpModification
            // 
            this.tlpModification.ColumnCount = 1;
            this.tlpModification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModification.Controls.Add(this.tlpButton, 0, 1);
            this.tlpModification.Controls.Add(this.splitterModification, 0, 0);
            this.tlpModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpModification.Location = new System.Drawing.Point(0, 0);
            this.tlpModification.Margin = new System.Windows.Forms.Padding(0);
            this.tlpModification.Name = "tlpModification";
            this.tlpModification.RowCount = 2;
            this.tlpModification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpModification.Size = new System.Drawing.Size(1000, 700);
            this.tlpModification.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 3;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Controls.Add(this.bExit, 1, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 640);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(1000, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(443, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // splitterModification
            // 
            this.splitterModification.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterModification.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterModification.Location = new System.Drawing.Point(3, 3);
            this.splitterModification.Name = "splitterModification";
            this.splitterModification.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterModification.Panel1
            // 
            this.splitterModification.Panel1.Controls.Add(this.tlpPacketData);
            this.splitterModification.Panel1MinSize = 0;
            // 
            // splitterModification.Panel2
            // 
            this.splitterModification.Panel2.Controls.Add(this.tPacketModification);
            this.splitterModification.Panel2MinSize = 0;
            this.splitterModification.Size = new System.Drawing.Size(994, 634);
            this.splitterModification.SplitterDistance = 364;
            this.splitterModification.SplitterSize = 80;
            this.splitterModification.SplitterWidth = 10;
            this.splitterModification.TabIndex = 2;
            // 
            // tlpPacketData
            // 
            this.tlpPacketData.ColumnCount = 2;
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.Controls.Add(this.txtPacketData_New, 1, 1);
            this.tlpPacketData.Controls.Add(this.lPacketData_New, 1, 0);
            this.tlpPacketData.Controls.Add(this.lPacketData_Raw, 0, 0);
            this.tlpPacketData.Controls.Add(this.txtPacketData_Raw, 0, 1);
            this.tlpPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketData.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketData.Name = "tlpPacketData";
            this.tlpPacketData.RowCount = 2;
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketData.Size = new System.Drawing.Size(994, 364);
            this.tlpPacketData.TabIndex = 0;
            // 
            // txtPacketData_New
            // 
            this.txtPacketData_New.AutoScroll = true;
            this.txtPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketData_New.Location = new System.Drawing.Point(500, 49);
            this.txtPacketData_New.Multiline = true;
            this.txtPacketData_New.Name = "txtPacketData_New";
            this.txtPacketData_New.ReadOnly = true;
            this.txtPacketData_New.Size = new System.Drawing.Size(491, 312);
            this.txtPacketData_New.TabIndex = 6;
            // 
            // lPacketData_New
            // 
            this.lPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketData_New.LocalizationText = "PacketModificationForm.Modified";
            this.lPacketData_New.Location = new System.Drawing.Point(500, 3);
            this.lPacketData_New.Name = "lPacketData_New";
            this.lPacketData_New.Size = new System.Drawing.Size(491, 40);
            this.lPacketData_New.TabIndex = 4;
            this.lPacketData_New.Text = "修改后封包数据";
            this.lPacketData_New.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lPacketData_Raw
            // 
            this.lPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketData_Raw.LocalizationText = "PacketModificationForm.Raw";
            this.lPacketData_Raw.Location = new System.Drawing.Point(3, 3);
            this.lPacketData_Raw.Name = "lPacketData_Raw";
            this.lPacketData_Raw.Size = new System.Drawing.Size(491, 40);
            this.lPacketData_Raw.TabIndex = 3;
            this.lPacketData_Raw.Text = "原始封包数据";
            this.lPacketData_Raw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPacketData_Raw
            // 
            this.txtPacketData_Raw.AutoScroll = true;
            this.txtPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketData_Raw.Location = new System.Drawing.Point(3, 49);
            this.txtPacketData_Raw.Multiline = true;
            this.txtPacketData_Raw.Name = "txtPacketData_Raw";
            this.txtPacketData_Raw.ReadOnly = true;
            this.txtPacketData_Raw.Size = new System.Drawing.Size(491, 312);
            this.txtPacketData_Raw.TabIndex = 5;
            // 
            // tPacketModification
            // 
            this.tPacketModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPacketModification.Gap = 12;
            this.tPacketModification.Location = new System.Drawing.Point(0, 0);
            this.tPacketModification.Name = "tPacketModification";
            this.tPacketModification.Size = new System.Drawing.Size(994, 260);
            this.tPacketModification.TabIndex = 0;
            this.tPacketModification.Text = "table1";
            // 
            // PacketModification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpModification);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PacketModification";
            this.Size = new System.Drawing.Size(1000, 700);
            this.Load += new System.EventHandler(this.PacketModification_Load);
            this.tlpModification.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.splitterModification.Panel1.ResumeLayout(false);
            this.splitterModification.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterModification)).EndInit();
            this.splitterModification.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpModification;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bExit;
        private AntdUI.Splitter splitterModification;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private AntdUI.Input txtPacketData_New;
        private AntdUI.Label lPacketData_New;
        private AntdUI.Label lPacketData_Raw;
        private AntdUI.Input txtPacketData_Raw;
        private AntdUI.Table tPacketModification;
    }
}

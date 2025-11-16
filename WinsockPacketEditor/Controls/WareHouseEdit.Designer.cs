namespace WinsockPacketEditor
{
    partial class WareHouseEdit
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
            this.tlpWareHouseEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpWareHouseEditInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
            this.lWName = new AntdUI.Label();
            this.txtWName = new AntdUI.Input();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.splitterPacketData = new AntdUI.Splitter();
            this.tStores = new AntdUI.Table();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.tlpWareHouseEdit.SuspendLayout();
            this.tlpWareHouseEditInfo.SuspendLayout();
            this.tlpButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterPacketData)).BeginInit();
            this.splitterPacketData.Panel1.SuspendLayout();
            this.splitterPacketData.Panel2.SuspendLayout();
            this.splitterPacketData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpWareHouseEdit
            // 
            this.tlpWareHouseEdit.ColumnCount = 1;
            this.tlpWareHouseEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseEdit.Controls.Add(this.tlpWareHouseEditInfo, 0, 0);
            this.tlpWareHouseEdit.Controls.Add(this.tlpButton, 0, 2);
            this.tlpWareHouseEdit.Controls.Add(this.splitterPacketData, 0, 1);
            this.tlpWareHouseEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWareHouseEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpWareHouseEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWareHouseEdit.Name = "tlpWareHouseEdit";
            this.tlpWareHouseEdit.RowCount = 3;
            this.tlpWareHouseEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpWareHouseEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpWareHouseEdit.Size = new System.Drawing.Size(1100, 700);
            this.tlpWareHouseEdit.TabIndex = 0;
            // 
            // tlpWareHouseEditInfo
            // 
            this.tlpWareHouseEditInfo.ColumnCount = 3;
            this.tlpWareHouseEditInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWareHouseEditInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseEditInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWareHouseEditInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWareHouseEditInfo.Controls.Add(this.ddMenu, 2, 0);
            this.tlpWareHouseEditInfo.Controls.Add(this.lWName, 0, 0);
            this.tlpWareHouseEditInfo.Controls.Add(this.txtWName, 1, 0);
            this.tlpWareHouseEditInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWareHouseEditInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpWareHouseEditInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpWareHouseEditInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWareHouseEditInfo.Name = "tlpWareHouseEditInfo";
            this.tlpWareHouseEditInfo.RowCount = 2;
            this.tlpWareHouseEditInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWareHouseEditInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseEditInfo.Size = new System.Drawing.Size(1100, 45);
            this.tlpWareHouseEditInfo.TabIndex = 18;
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(1066, 2);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu.Size = new System.Drawing.Size(32, 37);
            this.ddMenu.TabIndex = 18;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // lWName
            // 
            this.lWName.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWName.Location = new System.Drawing.Point(3, 3);
            this.lWName.Name = "lWName";
            this.lWName.Size = new System.Drawing.Size(55, 35);
            this.lWName.TabIndex = 19;
            this.lWName.Text = "仓库名称 :";
            // 
            // txtWName
            // 
            this.txtWName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWName.Location = new System.Drawing.Point(63, 2);
            this.txtWName.Margin = new System.Windows.Forms.Padding(2);
            this.txtWName.MaxLength = 100;
            this.txtWName.Name = "txtWName";
            this.txtWName.Size = new System.Drawing.Size(999, 37);
            this.txtWName.TabIndex = 20;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 650);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 2;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Size = new System.Drawing.Size(1100, 50);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(478, 11);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 37);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(559, 11);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // splitterPacketData
            // 
            this.splitterPacketData.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterPacketData.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterPacketData.Location = new System.Drawing.Point(0, 45);
            this.splitterPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.splitterPacketData.Name = "splitterPacketData";
            this.splitterPacketData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterPacketData.Panel1
            // 
            this.splitterPacketData.Panel1.Controls.Add(this.tStores);
            this.splitterPacketData.Panel1MinSize = 0;
            // 
            // splitterPacketData.Panel2
            // 
            this.splitterPacketData.Panel2.Controls.Add(this.hbPacketData);
            this.splitterPacketData.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitterPacketData.Panel2MinSize = 0;
            this.splitterPacketData.Size = new System.Drawing.Size(1100, 605);
            this.splitterPacketData.SplitterDistance = 446;
            this.splitterPacketData.SplitterSize = 80;
            this.splitterPacketData.SplitterWidth = 5;
            this.splitterPacketData.TabIndex = 19;
            // 
            // tStores
            // 
            this.tStores.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tStores.CellImpactHeight = false;
            this.tStores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tStores.EmptyHeader = true;
            this.tStores.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tStores.Gap = 10;
            this.tStores.GapCell = 5;
            this.tStores.Gaps = new System.Drawing.Size(10, 10);
            this.tStores.Location = new System.Drawing.Point(0, 0);
            this.tStores.Name = "tStores";
            this.tStores.Size = new System.Drawing.Size(1100, 446);
            this.tStores.TabIndex = 20;
            this.tStores.VirtualMode = true;
            this.tStores.CellClick += new AntdUI.Table.ClickEventHandler(this.tStores_CellClick);
            this.tStores.SelectIndexChanged += new System.EventHandler(this.tStores_SelectIndexChanged);
            // 
            // hbPacketData
            // 
            this.hbPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketData.ColumnInfoVisible = true;
            this.hbPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketData.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Location = new System.Drawing.Point(3, 3);
            this.hbPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ReadOnly = true;
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.Size = new System.Drawing.Size(1094, 148);
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.TabIndex = 5;
            this.hbPacketData.VScrollBarVisible = true;
            // 
            // WareHouseEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpWareHouseEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WareHouseEdit";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.WareHouseEdit_Load);
            this.tlpWareHouseEdit.ResumeLayout(false);
            this.tlpWareHouseEditInfo.ResumeLayout(false);
            this.tlpWareHouseEditInfo.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.splitterPacketData.Panel1.ResumeLayout(false);
            this.splitterPacketData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterPacketData)).EndInit();
            this.splitterPacketData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpWareHouseEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpWareHouseEditInfo;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Label lWName;
        private AntdUI.Input txtWName;
        private AntdUI.Splitter splitterPacketData;
        private AntdUI.Table tStores;
        private Be.Windows.Forms.HexBox hbPacketData;
    }
}

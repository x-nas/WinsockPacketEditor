namespace WinsockPacketEditor
{
    partial class WareHouseList
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
            this.tlpWareHouseList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tWareHouseList = new AntdUI.Table();
            this.tlpWareHouseListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
            this.bAutoStores = new AntdUI.Button();
            this.tlpWareHouseList.SuspendLayout();
            this.tlpWareHouseListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpWareHouseList
            // 
            this.tlpWareHouseList.ColumnCount = 1;
            this.tlpWareHouseList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseList.Controls.Add(this.tWareHouseList, 0, 1);
            this.tlpWareHouseList.Controls.Add(this.tlpWareHouseListButton, 0, 0);
            this.tlpWareHouseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWareHouseList.Location = new System.Drawing.Point(0, 0);
            this.tlpWareHouseList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWareHouseList.Name = "tlpWareHouseList";
            this.tlpWareHouseList.RowCount = 2;
            this.tlpWareHouseList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpWareHouseList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpWareHouseList.Size = new System.Drawing.Size(1100, 700);
            this.tlpWareHouseList.TabIndex = 5;
            // 
            // tWareHouseList
            // 
            this.tWareHouseList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tWareHouseList.CellImpactHeight = false;
            this.tWareHouseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tWareHouseList.Gap = 10;
            this.tWareHouseList.GapCell = 5;
            this.tWareHouseList.Gaps = new System.Drawing.Size(10, 10);
            this.tWareHouseList.Location = new System.Drawing.Point(2, 42);
            this.tWareHouseList.Margin = new System.Windows.Forms.Padding(2);
            this.tWareHouseList.MultipleRows = true;
            this.tWareHouseList.Name = "tWareHouseList";
            this.tWareHouseList.Size = new System.Drawing.Size(1096, 656);
            this.tWareHouseList.SwitchSize = 12;
            this.tWareHouseList.TabIndex = 1;
            this.tWareHouseList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tWareHouseList_CellButtonClick);
            this.tWareHouseList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tWareHouseList_CellDoubleClick);
            // 
            // tlpWareHouseListButton
            // 
            this.tlpWareHouseListButton.ColumnCount = 3;
            this.tlpWareHouseListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWareHouseListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWareHouseListButton.Controls.Add(this.ddMenu, 2, 0);
            this.tlpWareHouseListButton.Controls.Add(this.bAutoStores, 0, 0);
            this.tlpWareHouseListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWareHouseListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpWareHouseListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWareHouseListButton.Name = "tlpWareHouseListButton";
            this.tlpWareHouseListButton.RowCount = 1;
            this.tlpWareHouseListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWareHouseListButton.Size = new System.Drawing.Size(1100, 40);
            this.tlpWareHouseListButton.TabIndex = 3;
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(1066, 2);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu.Size = new System.Drawing.Size(32, 36);
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // bAutoStores
            // 
            this.bAutoStores.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bAutoStores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bAutoStores.IconSvg = "ShoppingCartOutlined";
            this.bAutoStores.Location = new System.Drawing.Point(3, 3);
            this.bAutoStores.Name = "bAutoStores";
            this.bAutoStores.Size = new System.Drawing.Size(87, 36);
            this.bAutoStores.TabIndex = 13;
            this.bAutoStores.Text = "自动入库";
            this.bAutoStores.Type = AntdUI.TTypeMini.Warn;
            this.bAutoStores.Click += new System.EventHandler(this.bAutoStores_Click);
            // 
            // WareHouseList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpWareHouseList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WareHouseList";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.WareHouseList_Load);
            this.tlpWareHouseList.ResumeLayout(false);
            this.tlpWareHouseListButton.ResumeLayout(false);
            this.tlpWareHouseListButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpWareHouseList;
        private AntdUI.Table tWareHouseList;
        private TableLayoutPanelEx tlpWareHouseListButton;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Button bAutoStores;
    }
}

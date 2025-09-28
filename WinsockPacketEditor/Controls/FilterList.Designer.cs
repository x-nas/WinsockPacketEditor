namespace WinsockPacketEditor
{
    partial class FilterList
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
            this.tlpFilterList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tFilterList = new AntdUI.Table();
            this.tlpFilterListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bDisableAll = new AntdUI.Button();
            this.ddMenu = new AntdUI.Dropdown();
            this.bFilterList_Reset = new AntdUI.Button();
            this.bEnableAll = new AntdUI.Button();
            this.tlpFilterList.SuspendLayout();
            this.tlpFilterListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFilterList
            // 
            this.tlpFilterList.ColumnCount = 1;
            this.tlpFilterList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.Controls.Add(this.tFilterList, 0, 1);
            this.tlpFilterList.Controls.Add(this.tlpFilterListButton, 0, 0);
            this.tlpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterList.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterList.Name = "tlpFilterList";
            this.tlpFilterList.RowCount = 2;
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpFilterList.Size = new System.Drawing.Size(1100, 700);
            this.tlpFilterList.TabIndex = 3;
            // 
            // tFilterList
            // 
            this.tFilterList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tFilterList.CellImpactHeight = false;
            this.tFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterList.Gap = 8;
            this.tFilterList.GapCell = 5;
            this.tFilterList.Gaps = new System.Drawing.Size(8, 8);
            this.tFilterList.Location = new System.Drawing.Point(2, 42);
            this.tFilterList.Margin = new System.Windows.Forms.Padding(2);
            this.tFilterList.MultipleRows = true;
            this.tFilterList.Name = "tFilterList";
            this.tFilterList.Size = new System.Drawing.Size(1096, 656);
            this.tFilterList.SwitchSize = 12;
            this.tFilterList.TabIndex = 1;
            this.tFilterList.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellClick);
            this.tFilterList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tFilterList_CellButtonClick);
            this.tFilterList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellDoubleClick);
            // 
            // tlpFilterListButton
            // 
            this.tlpFilterListButton.ColumnCount = 5;
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpFilterListButton.Controls.Add(this.bDisableAll, 1, 0);
            this.tlpFilterListButton.Controls.Add(this.ddMenu, 4, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_Reset, 2, 0);
            this.tlpFilterListButton.Controls.Add(this.bEnableAll, 0, 0);
            this.tlpFilterListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterListButton.Name = "tlpFilterListButton";
            this.tlpFilterListButton.RowCount = 1;
            this.tlpFilterListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.Size = new System.Drawing.Size(1100, 40);
            this.tlpFilterListButton.TabIndex = 2;
            // 
            // bDisableAll
            // 
            this.bDisableAll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bDisableAll.IconSvg = "CloseCircleOutlined";
            this.bDisableAll.LocalizationText = "DisableAll";
            this.bDisableAll.Location = new System.Drawing.Point(93, 2);
            this.bDisableAll.Margin = new System.Windows.Forms.Padding(2);
            this.bDisableAll.Name = "bDisableAll";
            this.bDisableAll.Size = new System.Drawing.Size(87, 36);
            this.bDisableAll.TabIndex = 14;
            this.bDisableAll.Text = "全部禁用";
            this.bDisableAll.Type = AntdUI.TTypeMini.Error;
            this.bDisableAll.Click += new System.EventHandler(this.bDisableAll_Click);
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
            this.ddMenu.Size = new System.Drawing.Size(32, 36);
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // bFilterList_Reset
            // 
            this.bFilterList_Reset.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bFilterList_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_Reset.IconSvg = "UndoOutlined";
            this.bFilterList_Reset.LocalizationText = "FilterList.ResetCount";
            this.bFilterList_Reset.Location = new System.Drawing.Point(184, 2);
            this.bFilterList_Reset.Margin = new System.Windows.Forms.Padding(2);
            this.bFilterList_Reset.Name = "bFilterList_Reset";
            this.bFilterList_Reset.Size = new System.Drawing.Size(87, 36);
            this.bFilterList_Reset.TabIndex = 6;
            this.bFilterList_Reset.Text = "重置计数";
            this.bFilterList_Reset.Type = AntdUI.TTypeMini.Warn;
            this.bFilterList_Reset.Click += new System.EventHandler(this.bFilterList_Reset_Click);
            // 
            // bEnableAll
            // 
            this.bEnableAll.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bEnableAll.IconSvg = "CheckCircleOutlined";
            this.bEnableAll.LocalizationText = "EnableAll";
            this.bEnableAll.Location = new System.Drawing.Point(2, 2);
            this.bEnableAll.Margin = new System.Windows.Forms.Padding(2);
            this.bEnableAll.Name = "bEnableAll";
            this.bEnableAll.Size = new System.Drawing.Size(87, 36);
            this.bEnableAll.TabIndex = 13;
            this.bEnableAll.Text = "全部启用";
            this.bEnableAll.Type = AntdUI.TTypeMini.Success;
            this.bEnableAll.Click += new System.EventHandler(this.bEnableAll_Click);
            // 
            // FilterList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFilterList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FilterList";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.FilterList_Load);
            this.tlpFilterList.ResumeLayout(false);
            this.tlpFilterListButton.ResumeLayout(false);
            this.tlpFilterListButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFilterList;
        private AntdUI.Table tFilterList;
        private TableLayoutPanelEx tlpFilterListButton;
        private AntdUI.Button bFilterList_Reset;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Button bDisableAll;
        private AntdUI.Button bEnableAll;
    }
}

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
            this.tlpFilterList = new System.Windows.Forms.TableLayoutPanel();
            this.tFilterList = new AntdUI.Table();
            this.tlpFilterListButton = new System.Windows.Forms.TableLayoutPanel();
            this.bFilterList_Reset = new AntdUI.Button();
            this.ddMenu = new AntdUI.Dropdown();
            this.tlpFilterList.SuspendLayout();
            this.tlpFilterListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFilterList
            // 
            this.tlpFilterList.ColumnCount = 3;
            this.tlpFilterList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFilterList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFilterList.Controls.Add(this.tFilterList, 1, 2);
            this.tlpFilterList.Controls.Add(this.tlpFilterListButton, 1, 1);
            this.tlpFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterList.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterList.Name = "tlpFilterList";
            this.tlpFilterList.RowCount = 3;
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFilterList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterList.Size = new System.Drawing.Size(800, 800);
            this.tlpFilterList.TabIndex = 3;
            // 
            // tFilterList
            // 
            this.tFilterList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tFilterList.CellImpactHeight = false;
            this.tFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterList.Gap = 12;
            this.tFilterList.Location = new System.Drawing.Point(33, 73);
            this.tFilterList.MultipleRows = true;
            this.tFilterList.Name = "tFilterList";
            this.tFilterList.Size = new System.Drawing.Size(734, 724);
            this.tFilterList.TabIndex = 1;
            this.tFilterList.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellClick);
            this.tFilterList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tFilterList_CellButtonClick);
            this.tFilterList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellDoubleClick);
            // 
            // tlpFilterListButton
            // 
            this.tlpFilterListButton.ColumnCount = 3;
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterListButton.Controls.Add(this.ddMenu, 2, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_Reset, 0, 0);
            this.tlpFilterListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterListButton.Location = new System.Drawing.Point(30, 20);
            this.tlpFilterListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterListButton.Name = "tlpFilterListButton";
            this.tlpFilterListButton.RowCount = 1;
            this.tlpFilterListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.Size = new System.Drawing.Size(740, 50);
            this.tlpFilterListButton.TabIndex = 2;
            // 
            // bFilterList_Reset
            // 
            this.bFilterList_Reset.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bFilterList_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_Reset.IconSvg = "UndoOutlined";
            this.bFilterList_Reset.LocalizationText = "FilterList.ResetCount";
            this.bFilterList_Reset.Location = new System.Drawing.Point(3, 3);
            this.bFilterList_Reset.Name = "bFilterList_Reset";
            this.bFilterList_Reset.Size = new System.Drawing.Size(114, 44);
            this.bFilterList_Reset.TabIndex = 6;
            this.bFilterList_Reset.Text = "重置计数";
            this.bFilterList_Reset.Type = AntdUI.TTypeMini.Info;
            this.bFilterList_Reset.Click += new System.EventHandler(this.bFilterList_Reset_Click);
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(693, 3);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Size = new System.Drawing.Size(44, 44);
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // FilterList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFilterList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FilterList";
            this.Size = new System.Drawing.Size(800, 800);
            this.Load += new System.EventHandler(this.FilterList_Load);
            this.tlpFilterList.ResumeLayout(false);
            this.tlpFilterListButton.ResumeLayout(false);
            this.tlpFilterListButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFilterList;
        private AntdUI.Table tFilterList;
        private System.Windows.Forms.TableLayoutPanel tlpFilterListButton;
        private AntdUI.Button bFilterList_Reset;
        private AntdUI.Dropdown ddMenu;
    }
}

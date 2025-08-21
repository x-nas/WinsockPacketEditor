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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            this.tlpFilterList = new System.Windows.Forms.TableLayoutPanel();
            this.tFilterList = new AntdUI.Table();
            this.tlpFilterListButton = new System.Windows.Forms.TableLayoutPanel();
            this.mFilterList = new AntdUI.Menu();
            this.bFilterList_Reset = new AntdUI.Button();
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
            this.tFilterList.GapCell = 6;
            this.tFilterList.Location = new System.Drawing.Point(3, 53);
            this.tFilterList.MultipleRows = true;
            this.tFilterList.Name = "tFilterList";
            this.tFilterList.Size = new System.Drawing.Size(794, 744);
            this.tFilterList.TabIndex = 1;
            this.tFilterList.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterList_CellClick);
            this.tFilterList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tFilterList_CellButtonClick);
            // 
            // tlpFilterListButton
            // 
            this.tlpFilterListButton.ColumnCount = 3;
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterListButton.Controls.Add(this.mFilterList, 2, 0);
            this.tlpFilterListButton.Controls.Add(this.bFilterList_Reset, 0, 0);
            this.tlpFilterListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterListButton.Name = "tlpFilterListButton";
            this.tlpFilterListButton.RowCount = 1;
            this.tlpFilterListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterListButton.Size = new System.Drawing.Size(800, 50);
            this.tlpFilterListButton.TabIndex = 2;
            // 
            // mFilterList
            // 
            this.mFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mFilterList.Gap = 0;
            this.mFilterList.IconRatio = 1F;
            menuItem1.IconSvg = "PlusOutlined";
            menuItem2.IconSvg = "UserAddOutlined";
            menuItem2.ID = "miAdd";
            menuItem2.LocalizationText = "FilterList.{id}";
            menuItem2.Text = "新增滤镜";
            menuItem3.IconSvg = "FolderOpenOutlined";
            menuItem3.ID = "miImport";
            menuItem3.LocalizationText = "FilterList.{id}";
            menuItem3.Text = "导入滤镜列表";
            menuItem4.IconSvg = "DeliveredProcedureOutlined";
            menuItem4.ID = "miExport";
            menuItem4.LocalizationText = "FilterList.{id}";
            menuItem4.Text = "导出所有滤镜";
            menuItem5.IconSvg = "DeleteOutlined";
            menuItem5.ID = "miClear";
            menuItem5.LocalizationText = "FilterList.{id}";
            menuItem5.Text = "清空所有滤镜";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Sub.Add(menuItem4);
            menuItem1.Sub.Add(menuItem5);
            this.mFilterList.Items.Add(menuItem1);
            this.mFilterList.Location = new System.Drawing.Point(737, 3);
            this.mFilterList.Mode = AntdUI.TMenuMode.Horizontal;
            this.mFilterList.Name = "mFilterList";
            this.mFilterList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mFilterList.Size = new System.Drawing.Size(60, 44);
            this.mFilterList.TabIndex = 5;
            this.mFilterList.Trigger = AntdUI.Trigger.Click;
            this.mFilterList.SelectChanged += new AntdUI.SelectEventHandler(this.mFilterList_SelectChanged);
            // 
            // bFilterList_Reset
            // 
            this.bFilterList_Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bFilterList_Reset.IconSvg = "UndoOutlined";
            this.bFilterList_Reset.LocalizationText = "FilterList.ResetCount";
            this.bFilterList_Reset.Location = new System.Drawing.Point(3, 3);
            this.bFilterList_Reset.Name = "bFilterList_Reset";
            this.bFilterList_Reset.Size = new System.Drawing.Size(144, 44);
            this.bFilterList_Reset.TabIndex = 6;
            this.bFilterList_Reset.Text = "重置计数";
            this.bFilterList_Reset.Type = AntdUI.TTypeMini.Info;
            this.bFilterList_Reset.Click += new System.EventHandler(this.bFilterList_Reset_Click);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFilterList;
        private AntdUI.Table tFilterList;
        private System.Windows.Forms.TableLayoutPanel tlpFilterListButton;
        private AntdUI.Menu mFilterList;
        private AntdUI.Button bFilterList_Reset;
    }
}

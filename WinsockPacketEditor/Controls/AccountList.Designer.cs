namespace WinsockPacketEditor
{
    partial class AccountList
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
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem9 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem10 = new AntdUI.MenuItem();
            this.tlpAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.tAccountList = new AntdUI.Table();
            this.tlpAccountListButton = new System.Windows.Forms.TableLayoutPanel();
            this.bReset = new AntdUI.Button();
            this.mAccountList = new AntdUI.Menu();
            this.txtSearchUserName = new AntdUI.Input();
            this.dtpExpiryTime = new AntdUI.DatePickerRange();
            this.bSearchExpiryTime = new AntdUI.Button();
            this.pAccountList = new AntdUI.Pagination();
            this.tlpAccountList.SuspendLayout();
            this.tlpAccountListButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpAccountList
            // 
            this.tlpAccountList.ColumnCount = 3;
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpAccountList.Controls.Add(this.tAccountList, 1, 2);
            this.tlpAccountList.Controls.Add(this.tlpAccountListButton, 1, 1);
            this.tlpAccountList.Controls.Add(this.pAccountList, 1, 3);
            this.tlpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountList.Location = new System.Drawing.Point(0, 0);
            this.tlpAccountList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountList.Name = "tlpAccountList";
            this.tlpAccountList.RowCount = 4;
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountList.Size = new System.Drawing.Size(1200, 800);
            this.tlpAccountList.TabIndex = 2;
            // 
            // tAccountList
            // 
            this.tAccountList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAccountList.CellImpactHeight = false;
            this.tAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAccountList.Gap = 12;
            this.tAccountList.Location = new System.Drawing.Point(33, 73);
            this.tAccountList.Name = "tAccountList";
            this.tAccountList.Size = new System.Drawing.Size(1134, 678);
            this.tAccountList.TabIndex = 1;
            this.tAccountList.CellClick += new AntdUI.Table.ClickEventHandler(this.tAccountList_CellClick);
            this.tAccountList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tAccountList_CellButtonClick);
            this.tAccountList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tAccountList_CellDoubleClick);
            // 
            // tlpAccountListButton
            // 
            this.tlpAccountListButton.ColumnCount = 6;
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountListButton.Controls.Add(this.bReset, 2, 1);
            this.tlpAccountListButton.Controls.Add(this.mAccountList, 5, 1);
            this.tlpAccountListButton.Controls.Add(this.txtSearchUserName, 4, 1);
            this.tlpAccountListButton.Controls.Add(this.dtpExpiryTime, 0, 1);
            this.tlpAccountListButton.Controls.Add(this.bSearchExpiryTime, 1, 1);
            this.tlpAccountListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountListButton.Location = new System.Drawing.Point(30, 20);
            this.tlpAccountListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountListButton.Name = "tlpAccountListButton";
            this.tlpAccountListButton.RowCount = 2;
            this.tlpAccountListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountListButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountListButton.Size = new System.Drawing.Size(1140, 50);
            this.tlpAccountListButton.TabIndex = 2;
            // 
            // bReset
            // 
            this.bReset.BorderWidth = 1F;
            this.bReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bReset.LocalizationText = "Reset";
            this.bReset.Location = new System.Drawing.Point(603, 2);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(94, 45);
            this.bReset.TabIndex = 8;
            this.bReset.Text = "重置";
            this.bReset.Type = AntdUI.TTypeMini.Warn;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // mAccountList
            // 
            this.mAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mAccountList.Gap = 5;
            this.mAccountList.IconRatio = 1F;
            this.mAccountList.itemMargin = 0;
            menuItem6.IconSvg = "PlusOutlined";
            menuItem7.IconSvg = "UserAddOutlined";
            menuItem7.ID = "miAdd";
            menuItem7.LocalizationText = "AccountList.{id}";
            menuItem7.Text = "新增账号";
            menuItem8.IconSvg = "FolderOpenOutlined";
            menuItem8.ID = "miImport";
            menuItem8.LocalizationText = "AccountList.{id}";
            menuItem8.Text = "导入账号列表";
            menuItem9.IconSvg = "DeliveredProcedureOutlined";
            menuItem9.ID = "miExport";
            menuItem9.LocalizationText = "AccountList.{id}";
            menuItem9.Text = "导出所有账号";
            menuItem10.IconSvg = "DeleteOutlined";
            menuItem10.ID = "miClear";
            menuItem10.LocalizationText = "AccountList.{id}";
            menuItem10.Text = "清空所有账号";
            menuItem6.Sub.Add(menuItem7);
            menuItem6.Sub.Add(menuItem8);
            menuItem6.Sub.Add(menuItem9);
            menuItem6.Sub.Add(menuItem10);
            this.mAccountList.Items.Add(menuItem6);
            this.mAccountList.Location = new System.Drawing.Point(1092, 2);
            this.mAccountList.Mode = AntdUI.TMenuMode.Horizontal;
            this.mAccountList.Name = "mAccountList";
            this.mAccountList.Size = new System.Drawing.Size(45, 45);
            this.mAccountList.TabIndex = 3;
            this.mAccountList.Trigger = AntdUI.Trigger.Click;
            this.mAccountList.SelectChanged += new AntdUI.SelectEventHandler(this.mAccountList_SelectChanged);
            // 
            // txtSearchUserName
            // 
            this.txtSearchUserName.AllowClear = true;
            this.txtSearchUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchUserName.LocalizationPlaceholderText = "AccountList.SearchAccount";
            this.txtSearchUserName.LocalizationText = "";
            this.txtSearchUserName.Location = new System.Drawing.Point(842, 2);
            this.txtSearchUserName.Name = "txtSearchUserName";
            this.txtSearchUserName.PlaceholderText = "请输入用户名查询";
            this.txtSearchUserName.PrefixSvg = "SearchOutlined";
            this.txtSearchUserName.Size = new System.Drawing.Size(244, 45);
            this.txtSearchUserName.TabIndex = 4;
            this.txtSearchUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearchUserName_KeyPress);
            // 
            // dtpExpiryTime
            // 
            this.dtpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpExpiryTime.Format = "yyyy-MM-dd HH:mm:ss";
            this.dtpExpiryTime.LocalizationPlaceholderEnd = "DatePicker.PlaceholderE";
            this.dtpExpiryTime.LocalizationPlaceholderStart = "DatePicker.PlaceholderS";
            this.dtpExpiryTime.Location = new System.Drawing.Point(3, 2);
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            this.dtpExpiryTime.PlaceholderEnd = "过期结束时间";
            this.dtpExpiryTime.PlaceholderStart = "过期开始时间";
            this.dtpExpiryTime.Size = new System.Drawing.Size(494, 45);
            this.dtpExpiryTime.TabIndex = 5;
            this.dtpExpiryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bSearchExpiryTime
            // 
            this.bSearchExpiryTime.BorderWidth = 1F;
            this.bSearchExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearchExpiryTime.LocalizationText = "Search";
            this.bSearchExpiryTime.Location = new System.Drawing.Point(503, 2);
            this.bSearchExpiryTime.Name = "bSearchExpiryTime";
            this.bSearchExpiryTime.Size = new System.Drawing.Size(94, 45);
            this.bSearchExpiryTime.TabIndex = 7;
            this.bSearchExpiryTime.Text = "查询";
            this.bSearchExpiryTime.Type = AntdUI.TTypeMini.Primary;
            this.bSearchExpiryTime.Click += new System.EventHandler(this.bSearchExpiryTime_Click);
            // 
            // pAccountList
            // 
            this.pAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAccountList.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pAccountList.Location = new System.Drawing.Point(33, 757);
            this.pAccountList.Name = "pAccountList";
            this.pAccountList.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pAccountList.ShowSizeChanger = true;
            this.pAccountList.Size = new System.Drawing.Size(1134, 40);
            this.pAccountList.TabIndex = 3;
            this.pAccountList.ValueChanged += new AntdUI.PageValueEventHandler(this.pAccountList_ValueChanged);
            this.pAccountList.ShowTotalChanged += new AntdUI.PageValueRtEventHandler(this.pAccountList_ShowTotalChanged);
            // 
            // AccountList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpAccountList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AccountList";
            this.Size = new System.Drawing.Size(1200, 800);
            this.Load += new System.EventHandler(this.AccountList_Load);
            this.tlpAccountList.ResumeLayout(false);
            this.tlpAccountListButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpAccountList;
        private AntdUI.Table tAccountList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountListButton;
        private AntdUI.Menu mAccountList;
        private AntdUI.Input txtSearchUserName;
        private AntdUI.DatePickerRange dtpExpiryTime;
        private AntdUI.Button bSearchExpiryTime;
        private AntdUI.Pagination pAccountList;
        private AntdUI.Button bReset;
    }
}

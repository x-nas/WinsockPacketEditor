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
            this.tlpAccountList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tAccountList = new AntdUI.Table();
            this.tlpAccountListButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
            this.bReset = new AntdUI.Button();
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
            this.tlpAccountList.ColumnCount = 1;
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.Controls.Add(this.tAccountList, 0, 1);
            this.tlpAccountList.Controls.Add(this.tlpAccountListButton, 0, 0);
            this.tlpAccountList.Controls.Add(this.pAccountList, 0, 2);
            this.tlpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountList.Location = new System.Drawing.Point(0, 0);
            this.tlpAccountList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountList.Name = "tlpAccountList";
            this.tlpAccountList.RowCount = 3;
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpAccountList.Size = new System.Drawing.Size(1100, 600);
            this.tlpAccountList.TabIndex = 2;
            // 
            // tAccountList
            // 
            this.tAccountList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAccountList.CellImpactHeight = false;
            this.tAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAccountList.Gap = 12;
            this.tAccountList.Location = new System.Drawing.Point(2, 42);
            this.tAccountList.Margin = new System.Windows.Forms.Padding(2);
            this.tAccountList.Name = "tAccountList";
            this.tAccountList.Size = new System.Drawing.Size(1096, 520);
            this.tAccountList.TabIndex = 1;
            this.tAccountList.CellClick += new AntdUI.Table.ClickEventHandler(this.tAccountList_CellClick);
            this.tAccountList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tAccountList_CellButtonClick);
            this.tAccountList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tAccountList_CellDoubleClick);
            // 
            // tlpAccountListButton
            // 
            this.tlpAccountListButton.ColumnCount = 6;
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountListButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpAccountListButton.Controls.Add(this.ddMenu, 5, 1);
            this.tlpAccountListButton.Controls.Add(this.bReset, 2, 1);
            this.tlpAccountListButton.Controls.Add(this.txtSearchUserName, 4, 1);
            this.tlpAccountListButton.Controls.Add(this.dtpExpiryTime, 0, 1);
            this.tlpAccountListButton.Controls.Add(this.bSearchExpiryTime, 1, 1);
            this.tlpAccountListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountListButton.Location = new System.Drawing.Point(0, 0);
            this.tlpAccountListButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountListButton.Name = "tlpAccountListButton";
            this.tlpAccountListButton.RowCount = 2;
            this.tlpAccountListButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountListButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountListButton.Size = new System.Drawing.Size(1100, 40);
            this.tlpAccountListButton.TabIndex = 2;
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(1066, 1);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Size = new System.Drawing.Size(32, 37);
            this.ddMenu.TabIndex = 11;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // bReset
            // 
            this.bReset.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bReset.BorderWidth = 1F;
            this.bReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bReset.IconSvg = "RedoOutlined";
            this.bReset.LocalizationText = "Reset";
            this.bReset.Location = new System.Drawing.Point(419, 1);
            this.bReset.Margin = new System.Windows.Forms.Padding(2);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(63, 37);
            this.bReset.TabIndex = 8;
            this.bReset.Text = "重置";
            this.bReset.Type = AntdUI.TTypeMini.Warn;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // txtSearchUserName
            // 
            this.txtSearchUserName.AllowClear = true;
            this.txtSearchUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchUserName.LocalizationPlaceholderText = "AccountList.SearchAccount";
            this.txtSearchUserName.LocalizationText = "";
            this.txtSearchUserName.Location = new System.Drawing.Point(891, 1);
            this.txtSearchUserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearchUserName.Name = "txtSearchUserName";
            this.txtSearchUserName.PlaceholderText = "请输入用户名查询";
            this.txtSearchUserName.PrefixSvg = "SearchOutlined";
            this.txtSearchUserName.Size = new System.Drawing.Size(171, 37);
            this.txtSearchUserName.TabIndex = 4;
            this.txtSearchUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearchUserName_KeyPress);
            // 
            // dtpExpiryTime
            // 
            this.dtpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpExpiryTime.Format = "yyyy-MM-dd HH:mm:ss";
            this.dtpExpiryTime.LocalizationPlaceholderEnd = "DatePicker.PlaceholderE";
            this.dtpExpiryTime.LocalizationPlaceholderStart = "DatePicker.PlaceholderS";
            this.dtpExpiryTime.Location = new System.Drawing.Point(2, 1);
            this.dtpExpiryTime.Margin = new System.Windows.Forms.Padding(2);
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            this.dtpExpiryTime.PlaceholderEnd = "过期结束时间";
            this.dtpExpiryTime.PlaceholderStart = "过期开始时间";
            this.dtpExpiryTime.Size = new System.Drawing.Size(346, 37);
            this.dtpExpiryTime.TabIndex = 5;
            this.dtpExpiryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bSearchExpiryTime
            // 
            this.bSearchExpiryTime.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSearchExpiryTime.BorderWidth = 1F;
            this.bSearchExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearchExpiryTime.IconSvg = "SearchOutlined";
            this.bSearchExpiryTime.LocalizationText = "Search";
            this.bSearchExpiryTime.Location = new System.Drawing.Point(352, 1);
            this.bSearchExpiryTime.Margin = new System.Windows.Forms.Padding(2);
            this.bSearchExpiryTime.Name = "bSearchExpiryTime";
            this.bSearchExpiryTime.Size = new System.Drawing.Size(63, 37);
            this.bSearchExpiryTime.TabIndex = 7;
            this.bSearchExpiryTime.Text = "查询";
            this.bSearchExpiryTime.Type = AntdUI.TTypeMini.Primary;
            this.bSearchExpiryTime.Click += new System.EventHandler(this.bSearchExpiryTime_Click);
            // 
            // pAccountList
            // 
            this.pAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAccountList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pAccountList.Location = new System.Drawing.Point(2, 566);
            this.pAccountList.Margin = new System.Windows.Forms.Padding(2);
            this.pAccountList.Name = "pAccountList";
            this.pAccountList.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pAccountList.ShowSizeChanger = true;
            this.pAccountList.Size = new System.Drawing.Size(1096, 32);
            this.pAccountList.TabIndex = 3;
            this.pAccountList.ValueChanged += new AntdUI.PageValueEventHandler(this.pAccountList_ValueChanged);
            this.pAccountList.ShowTotalChanged += new AntdUI.PageValueRtEventHandler(this.pAccountList_ShowTotalChanged);
            // 
            // AccountList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpAccountList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AccountList";
            this.Size = new System.Drawing.Size(1100, 600);
            this.Load += new System.EventHandler(this.AccountList_Load);
            this.tlpAccountList.ResumeLayout(false);
            this.tlpAccountListButton.ResumeLayout(false);
            this.tlpAccountListButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpAccountList;
        private AntdUI.Table tAccountList;
        private TableLayoutPanelEx tlpAccountListButton;
        private AntdUI.Input txtSearchUserName;
        private AntdUI.DatePickerRange dtpExpiryTime;
        private AntdUI.Button bSearchExpiryTime;
        private AntdUI.Pagination pAccountList;
        private AntdUI.Button bReset;
        private AntdUI.Dropdown ddMenu;
    }
}

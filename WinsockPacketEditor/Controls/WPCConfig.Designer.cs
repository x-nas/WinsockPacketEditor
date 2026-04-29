namespace WinsockPacketEditor
{
    partial class WPCConfig
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
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            this.tlpWPCConfig = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpWPCConfigButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
            this.tabWPCConfig = new AntdUI.Tabs();
            this.tpNoticeList = new AntdUI.TabPage();
            this.tNoticeList = new AntdUI.Table();
            this.tpServerList = new AntdUI.TabPage();
            this.tServerList = new AntdUI.Table();
            this.tlpWPCConfig.SuspendLayout();
            this.tlpWPCConfigButton.SuspendLayout();
            this.tabWPCConfig.SuspendLayout();
            this.tpNoticeList.SuspendLayout();
            this.tpServerList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpWPCConfig
            // 
            this.tlpWPCConfig.ColumnCount = 1;
            this.tlpWPCConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWPCConfig.Controls.Add(this.tlpWPCConfigButton, 0, 0);
            this.tlpWPCConfig.Controls.Add(this.tabWPCConfig, 0, 1);
            this.tlpWPCConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWPCConfig.Location = new System.Drawing.Point(0, 0);
            this.tlpWPCConfig.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWPCConfig.Name = "tlpWPCConfig";
            this.tlpWPCConfig.RowCount = 2;
            this.tlpWPCConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpWPCConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWPCConfig.Size = new System.Drawing.Size(1100, 700);
            this.tlpWPCConfig.TabIndex = 0;
            // 
            // tlpWPCConfigButton
            // 
            this.tlpWPCConfigButton.ColumnCount = 2;
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPCConfigButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPCConfigButton.Controls.Add(this.ddMenu, 1, 0);
            this.tlpWPCConfigButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWPCConfigButton.Location = new System.Drawing.Point(0, 0);
            this.tlpWPCConfigButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWPCConfigButton.Name = "tlpWPCConfigButton";
            this.tlpWPCConfigButton.RowCount = 1;
            this.tlpWPCConfigButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWPCConfigButton.Size = new System.Drawing.Size(1100, 40);
            this.tlpWPCConfigButton.TabIndex = 4;
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
            // tabWPCConfig
            // 
            this.tabWPCConfig.Controls.Add(this.tpNoticeList);
            this.tabWPCConfig.Controls.Add(this.tpServerList);
            this.tabWPCConfig.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabWPCConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabWPCConfig.Gap = 20;
            this.tabWPCConfig.Location = new System.Drawing.Point(3, 43);
            this.tabWPCConfig.Name = "tabWPCConfig";
            this.tabWPCConfig.Pages.Add(this.tpServerList);
            this.tabWPCConfig.Pages.Add(this.tpNoticeList);
            this.tabWPCConfig.SelectedIndex = 1;
            this.tabWPCConfig.Size = new System.Drawing.Size(1094, 654);
            this.tabWPCConfig.Style = styleCard1;
            this.tabWPCConfig.TabIndex = 0;
            this.tabWPCConfig.Type = AntdUI.TabType.Card;
            this.tabWPCConfig.SelectedIndexChanged += new AntdUI.IntEventHandler(this.tabWPCConfig_SelectedIndexChanged);
            // 
            // tpNoticeList
            // 
            this.tpNoticeList.Controls.Add(this.tNoticeList);
            this.tpNoticeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpNoticeList.LocalizationText = "WPCConfig.{id}";
            this.tpNoticeList.Location = new System.Drawing.Point(0, 39);
            this.tpNoticeList.Name = "tpNoticeList";
            this.tpNoticeList.Showed = true;
            this.tpNoticeList.Size = new System.Drawing.Size(1094, 615);
            this.tpNoticeList.TabIndex = 1;
            this.tpNoticeList.Text = "公告列表";
            // 
            // tNoticeList
            // 
            this.tNoticeList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tNoticeList.CellImpactHeight = false;
            this.tNoticeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tNoticeList.Gap = 10;
            this.tNoticeList.GapCell = 5;
            this.tNoticeList.Gaps = new System.Drawing.Size(10, 10);
            this.tNoticeList.Location = new System.Drawing.Point(0, 0);
            this.tNoticeList.Margin = new System.Windows.Forms.Padding(2);
            this.tNoticeList.MultipleRows = true;
            this.tNoticeList.Name = "tNoticeList";
            this.tNoticeList.Size = new System.Drawing.Size(1094, 615);
            this.tNoticeList.SwitchSize = 12;
            this.tNoticeList.TabIndex = 3;
            this.tNoticeList.CellClick += new AntdUI.Table.ClickEventHandler(this.tNoticeList_CellClick);
            this.tNoticeList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tNoticeList_CellButtonClick);
            this.tNoticeList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tNoticeList_CellDoubleClick);
            // 
            // tpServerList
            // 
            this.tpServerList.Controls.Add(this.tServerList);
            this.tpServerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpServerList.LocalizationText = "WPCConfig.{id}";
            this.tpServerList.Location = new System.Drawing.Point(0, 39);
            this.tpServerList.Name = "tpServerList";
            this.tpServerList.Size = new System.Drawing.Size(1094, 615);
            this.tpServerList.TabIndex = 0;
            this.tpServerList.Text = "服务器列表";
            // 
            // tServerList
            // 
            this.tServerList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tServerList.CellImpactHeight = false;
            this.tServerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tServerList.Gap = 10;
            this.tServerList.GapCell = 5;
            this.tServerList.Gaps = new System.Drawing.Size(10, 10);
            this.tServerList.Location = new System.Drawing.Point(0, 0);
            this.tServerList.Margin = new System.Windows.Forms.Padding(2);
            this.tServerList.MultipleRows = true;
            this.tServerList.Name = "tServerList";
            this.tServerList.Size = new System.Drawing.Size(1094, 615);
            this.tServerList.SwitchSize = 12;
            this.tServerList.TabIndex = 2;
            this.tServerList.CellClick += new AntdUI.Table.ClickEventHandler(this.tServerList_CellClick);
            this.tServerList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tServerList_CellButtonClick);
            this.tServerList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tServerList_CellDoubleClick);
            // 
            // WPCConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpWPCConfig);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WPCConfig";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.WPCConfig_Load);
            this.tlpWPCConfig.ResumeLayout(false);
            this.tlpWPCConfigButton.ResumeLayout(false);
            this.tlpWPCConfigButton.PerformLayout();
            this.tabWPCConfig.ResumeLayout(false);
            this.tpNoticeList.ResumeLayout(false);
            this.tpServerList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpWPCConfig;
        private AntdUI.Tabs tabWPCConfig;
        private AntdUI.TabPage tpServerList;
        private AntdUI.TabPage tpNoticeList;
        private TableLayoutPanelEx tlpWPCConfigButton;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Table tServerList;
        private AntdUI.Table tNoticeList;
    }
}

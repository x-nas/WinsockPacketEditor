namespace WinsockPacketEditor
{
    partial class AutoStoresList
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
            this.tlpAutoStores = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tAutoStores = new AntdUI.Table();
            this.tlpMenu = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
            this.cbEnable_AutoStores = new AntdUI.Checkbox();
            this.lEnableNotice = new AntdUI.Label();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.lSplit = new AntdUI.Label();
            this.tlpAutoStores.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpAutoStores
            // 
            this.tlpAutoStores.ColumnCount = 1;
            this.tlpAutoStores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAutoStores.Controls.Add(this.tAutoStores, 0, 1);
            this.tlpAutoStores.Controls.Add(this.tlpMenu, 0, 0);
            this.tlpAutoStores.Controls.Add(this.tlpButton, 0, 2);
            this.tlpAutoStores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAutoStores.Location = new System.Drawing.Point(0, 0);
            this.tlpAutoStores.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAutoStores.Name = "tlpAutoStores";
            this.tlpAutoStores.RowCount = 3;
            this.tlpAutoStores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpAutoStores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAutoStores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpAutoStores.Size = new System.Drawing.Size(700, 500);
            this.tlpAutoStores.TabIndex = 0;
            // 
            // tAutoStores
            // 
            this.tAutoStores.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tAutoStores.Bordered = true;
            this.tAutoStores.CellImpactHeight = false;
            this.tAutoStores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tAutoStores.EmptyHeader = true;
            this.tAutoStores.Gap = 8;
            this.tAutoStores.GapCell = 5;
            this.tAutoStores.Gaps = new System.Drawing.Size(8, 8);
            this.tAutoStores.Location = new System.Drawing.Point(2, 47);
            this.tAutoStores.Margin = new System.Windows.Forms.Padding(2);
            this.tAutoStores.Name = "tAutoStores";
            this.tAutoStores.Size = new System.Drawing.Size(696, 401);
            this.tAutoStores.TabIndex = 19;
            this.tAutoStores.CellClick += new AntdUI.Table.ClickEventHandler(this.tAutoStores_CellClick);
            this.tAutoStores.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tAutoStores_CellButtonClick);
            this.tAutoStores.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tAutoStores_CellDoubleClick);
            // 
            // tlpMenu
            // 
            this.tlpMenu.ColumnCount = 5;
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMenu.Controls.Add(this.ddMenu, 4, 0);
            this.tlpMenu.Controls.Add(this.cbEnable_AutoStores, 0, 0);
            this.tlpMenu.Controls.Add(this.lEnableNotice, 2, 0);
            this.tlpMenu.Controls.Add(this.lSplit, 1, 0);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMenu.Location = new System.Drawing.Point(0, 0);
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMenu.Name = "tlpMenu";
            this.tlpMenu.RowCount = 2;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMenu.Size = new System.Drawing.Size(700, 45);
            this.tlpMenu.TabIndex = 18;
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(666, 2);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu.Size = new System.Drawing.Size(32, 37);
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // cbEnable_AutoStores
            // 
            this.cbEnable_AutoStores.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_AutoStores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_AutoStores.LocalizationText = "AutoStores.Enable";
            this.cbEnable_AutoStores.Location = new System.Drawing.Point(2, 2);
            this.cbEnable_AutoStores.Margin = new System.Windows.Forms.Padding(2);
            this.cbEnable_AutoStores.Name = "cbEnable_AutoStores";
            this.cbEnable_AutoStores.Size = new System.Drawing.Size(104, 37);
            this.cbEnable_AutoStores.TabIndex = 8;
            this.cbEnable_AutoStores.Text = "启用自动入库";
            this.cbEnable_AutoStores.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_AutoStores_CheckedChanged);
            // 
            // lEnableNotice
            // 
            this.lEnableNotice.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lEnableNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEnableNotice.ForeColor = System.Drawing.Color.Red;
            this.lEnableNotice.Location = new System.Drawing.Point(123, 3);
            this.lEnableNotice.Name = "lEnableNotice";
            this.lEnableNotice.Size = new System.Drawing.Size(72, 35);
            this.lEnableNotice.TabIndex = 13;
            this.lEnableNotice.Text = "自动入库须知";
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
            this.tlpButton.Location = new System.Drawing.Point(0, 450);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(700, 50);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(278, 6);
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
            this.bExit.Location = new System.Drawing.Point(359, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // lSplit
            // 
            this.lSplit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit.Location = new System.Drawing.Point(111, 3);
            this.lSplit.Name = "lSplit";
            this.lSplit.Size = new System.Drawing.Size(6, 35);
            this.lSplit.TabIndex = 14;
            this.lSplit.Text = "-";
            this.lSplit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AutoStoresList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpAutoStores);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AutoStoresList";
            this.Size = new System.Drawing.Size(700, 500);
            this.Load += new System.EventHandler(this.AutoStores_Load);
            this.tlpAutoStores.ResumeLayout(false);
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpAutoStores;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpMenu;
        private AntdUI.Dropdown ddMenu;
        private AntdUI.Checkbox cbEnable_AutoStores;
        private AntdUI.Table tAutoStores;
        private AntdUI.Label lEnableNotice;
        private AntdUI.Label lSplit;
    }
}

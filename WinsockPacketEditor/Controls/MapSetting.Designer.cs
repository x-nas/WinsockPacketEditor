namespace WinsockPacketEditor
{
    partial class MapSetting
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
            AntdUI.MenuItem menuItem16 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem17 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem18 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem19 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem20 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem11 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem12 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem13 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem14 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem15 = new AntdUI.MenuItem();
            this.tlpMapSettings = new System.Windows.Forms.TableLayoutPanel();
            this.tMapRemote = new AntdUI.Table();
            this.tlpMapRemote = new System.Windows.Forms.TableLayoutPanel();
            this.mMapRemote = new AntdUI.Menu();
            this.cbEnable_MapRemote = new AntdUI.Checkbox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dMapLocal = new AntdUI.Divider();
            this.dMapRemote = new AntdUI.Divider();
            this.tMapLocal = new AntdUI.Table();
            this.tlpMapLocal = new System.Windows.Forms.TableLayoutPanel();
            this.mMapLocal = new AntdUI.Menu();
            this.cbEnable_MapLocal = new AntdUI.Checkbox();
            this.tlpMapSettings.SuspendLayout();
            this.tlpMapRemote.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpMapLocal.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMapSettings
            // 
            this.tlpMapSettings.ColumnCount = 1;
            this.tlpMapSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapSettings.Controls.Add(this.tMapRemote, 0, 6);
            this.tlpMapSettings.Controls.Add(this.tlpMapRemote, 0, 5);
            this.tlpMapSettings.Controls.Add(this.tlpButton, 0, 7);
            this.tlpMapSettings.Controls.Add(this.dMapLocal, 0, 0);
            this.tlpMapSettings.Controls.Add(this.dMapRemote, 0, 4);
            this.tlpMapSettings.Controls.Add(this.tMapLocal, 0, 2);
            this.tlpMapSettings.Controls.Add(this.tlpMapLocal, 0, 1);
            this.tlpMapSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpMapSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMapSettings.Name = "tlpMapSettings";
            this.tlpMapSettings.RowCount = 8;
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapSettings.Size = new System.Drawing.Size(800, 700);
            this.tlpMapSettings.TabIndex = 2;
            // 
            // tMapRemote
            // 
            this.tMapRemote.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tMapRemote.Bordered = true;
            this.tMapRemote.CellImpactHeight = false;
            this.tMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tMapRemote.EmptyHeader = true;
            this.tMapRemote.Gap = 8;
            this.tMapRemote.GapCell = 0;
            this.tMapRemote.Gaps = new System.Drawing.Size(8, 8);
            this.tMapRemote.Location = new System.Drawing.Point(3, 417);
            this.tMapRemote.Name = "tMapRemote";
            this.tMapRemote.Size = new System.Drawing.Size(794, 220);
            this.tMapRemote.TabIndex = 11;
            this.tMapRemote.CellClick += new AntdUI.Table.ClickEventHandler(this.tMapRemote_CellClick);
            this.tMapRemote.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tMapRemote_CellButtonUp);
            this.tMapRemote.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tMapRemote_CellDoubleClick);
            // 
            // tlpMapRemote
            // 
            this.tlpMapRemote.ColumnCount = 3;
            this.tlpMapRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMapRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMapRemote.Controls.Add(this.mMapRemote, 2, 0);
            this.tlpMapRemote.Controls.Add(this.cbEnable_MapRemote, 0, 0);
            this.tlpMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapRemote.Location = new System.Drawing.Point(0, 359);
            this.tlpMapRemote.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMapRemote.Name = "tlpMapRemote";
            this.tlpMapRemote.RowCount = 2;
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapRemote.Size = new System.Drawing.Size(800, 55);
            this.tlpMapRemote.TabIndex = 10;
            // 
            // mMapRemote
            // 
            this.mMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mMapRemote.Gap = 5;
            this.mMapRemote.IconRatio = 1F;
            menuItem16.IconSvg = "PlusOutlined";
            menuItem17.IconSvg = "BlockOutlined";
            menuItem17.ID = "miAdd";
            menuItem17.LocalizationText = "MapSettingsForm.MapRemote.{id}";
            menuItem17.Text = "新增";
            menuItem18.IconSvg = "FolderOpenOutlined";
            menuItem18.ID = "miImport";
            menuItem18.LocalizationText = "MapSettingsForm.MapRemote.{id}";
            menuItem18.Text = "导入远程映射";
            menuItem19.IconSvg = "DeliveredProcedureOutlined";
            menuItem19.ID = "miExport";
            menuItem19.LocalizationText = "MapSettingsForm.MapRemote.{id}";
            menuItem19.Text = "导出远程映射";
            menuItem20.IconSvg = "DeleteOutlined";
            menuItem20.ID = "miClear";
            menuItem20.LocalizationText = "MapSettingsForm.MapRemote.{id}";
            menuItem20.Text = "清空远程映射";
            menuItem16.Sub.Add(menuItem17);
            menuItem16.Sub.Add(menuItem18);
            menuItem16.Sub.Add(menuItem19);
            menuItem16.Sub.Add(menuItem20);
            this.mMapRemote.Items.Add(menuItem16);
            this.mMapRemote.Location = new System.Drawing.Point(747, 3);
            this.mMapRemote.Mode = AntdUI.TMenuMode.Horizontal;
            this.mMapRemote.Name = "mMapRemote";
            this.mMapRemote.Size = new System.Drawing.Size(50, 45);
            this.mMapRemote.TabIndex = 10;
            this.mMapRemote.SelectChanged += new AntdUI.SelectEventHandler(this.mMapRemote_SelectChanged);
            // 
            // cbEnable_MapRemote
            // 
            this.cbEnable_MapRemote.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_MapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_MapRemote.LocalizationText = "Enable";
            this.cbEnable_MapRemote.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_MapRemote.Name = "cbEnable_MapRemote";
            this.cbEnable_MapRemote.Size = new System.Drawing.Size(138, 45);
            this.cbEnable_MapRemote.TabIndex = 8;
            this.cbEnable_MapRemote.Text = "启用远程映射";
            this.cbEnable_MapRemote.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_MapRemote_CheckedChanged);
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 640);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(800, 60);
            this.tlpButton.TabIndex = 3;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(273, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(413, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dMapLocal
            // 
            this.dMapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMapLocal.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMapLocal.LocalizationText = "MapSettingsForm.MapLocal";
            this.dMapLocal.Location = new System.Drawing.Point(3, 3);
            this.dMapLocal.Name = "dMapLocal";
            this.dMapLocal.Orientation = AntdUI.TOrientation.Left;
            this.dMapLocal.Size = new System.Drawing.Size(794, 23);
            this.dMapLocal.TabIndex = 4;
            this.dMapLocal.Text = "本地映射";
            // 
            // dMapRemote
            // 
            this.dMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMapRemote.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMapRemote.LocalizationText = "MapSettingsForm.MapRemote";
            this.dMapRemote.Location = new System.Drawing.Point(3, 333);
            this.dMapRemote.Name = "dMapRemote";
            this.dMapRemote.Orientation = AntdUI.TOrientation.Left;
            this.dMapRemote.Size = new System.Drawing.Size(794, 23);
            this.dMapRemote.TabIndex = 6;
            this.dMapRemote.Text = "远程映射";
            // 
            // tMapLocal
            // 
            this.tMapLocal.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tMapLocal.Bordered = true;
            this.tMapLocal.CellImpactHeight = false;
            this.tMapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tMapLocal.EmptyHeader = true;
            this.tMapLocal.Gap = 8;
            this.tMapLocal.GapCell = 0;
            this.tMapLocal.Gaps = new System.Drawing.Size(8, 8);
            this.tMapLocal.Location = new System.Drawing.Point(3, 87);
            this.tMapLocal.Name = "tMapLocal";
            this.tMapLocal.Size = new System.Drawing.Size(794, 220);
            this.tMapLocal.TabIndex = 8;
            this.tMapLocal.CellClick += new AntdUI.Table.ClickEventHandler(this.tMapLocal_CellClick);
            this.tMapLocal.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tMapLocal_CellButtonClick);
            this.tMapLocal.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tMapLocal_CellDoubleClick);
            // 
            // tlpMapLocal
            // 
            this.tlpMapLocal.ColumnCount = 3;
            this.tlpMapLocal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMapLocal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapLocal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMapLocal.Controls.Add(this.mMapLocal, 2, 0);
            this.tlpMapLocal.Controls.Add(this.cbEnable_MapLocal, 0, 0);
            this.tlpMapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapLocal.Location = new System.Drawing.Point(0, 29);
            this.tlpMapLocal.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMapLocal.Name = "tlpMapLocal";
            this.tlpMapLocal.RowCount = 2;
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapLocal.Size = new System.Drawing.Size(800, 55);
            this.tlpMapLocal.TabIndex = 9;
            // 
            // mMapLocal
            // 
            this.mMapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mMapLocal.Gap = 5;
            this.mMapLocal.IconRatio = 1F;
            menuItem11.IconSvg = "PlusOutlined";
            menuItem12.IconSvg = "BlockOutlined";
            menuItem12.ID = "miAdd";
            menuItem12.LocalizationText = "MapSettingsForm.MapLocal.{id}";
            menuItem12.Text = "新增";
            menuItem13.IconSvg = "FolderOpenOutlined";
            menuItem13.ID = "miImport";
            menuItem13.LocalizationText = "MapSettingsForm.MapLocal.{id}";
            menuItem13.Text = "导入本地映射";
            menuItem14.IconSvg = "DeliveredProcedureOutlined";
            menuItem14.ID = "miExport";
            menuItem14.LocalizationText = "MapSettingsForm.MapLocal.{id}";
            menuItem14.Text = "导出本地映射";
            menuItem15.IconSvg = "DeleteOutlined";
            menuItem15.ID = "miClear";
            menuItem15.LocalizationText = "MapSettingsForm.MapLocal.{id}";
            menuItem15.Text = "清空本地映射";
            menuItem11.Sub.Add(menuItem12);
            menuItem11.Sub.Add(menuItem13);
            menuItem11.Sub.Add(menuItem14);
            menuItem11.Sub.Add(menuItem15);
            this.mMapLocal.Items.Add(menuItem11);
            this.mMapLocal.Location = new System.Drawing.Point(747, 3);
            this.mMapLocal.Mode = AntdUI.TMenuMode.Horizontal;
            this.mMapLocal.Name = "mMapLocal";
            this.mMapLocal.Size = new System.Drawing.Size(50, 45);
            this.mMapLocal.TabIndex = 9;
            this.mMapLocal.SelectChanged += new AntdUI.SelectEventHandler(this.mMapLocal_SelectChanged);
            // 
            // cbEnable_MapLocal
            // 
            this.cbEnable_MapLocal.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_MapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_MapLocal.LocalizationText = "Enable";
            this.cbEnable_MapLocal.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_MapLocal.Name = "cbEnable_MapLocal";
            this.cbEnable_MapLocal.Size = new System.Drawing.Size(138, 45);
            this.cbEnable_MapLocal.TabIndex = 8;
            this.cbEnable_MapLocal.Text = "启用本地映射";
            this.cbEnable_MapLocal.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_MapLocal_CheckedChanged);
            // 
            // MapSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMapSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MapSetting";
            this.Size = new System.Drawing.Size(800, 700);
            this.Load += new System.EventHandler(this.MapSetting_Load);
            this.tlpMapSettings.ResumeLayout(false);
            this.tlpMapRemote.ResumeLayout(false);
            this.tlpMapRemote.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpMapLocal.ResumeLayout(false);
            this.tlpMapLocal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMapSettings;
        private AntdUI.Table tMapRemote;
        private System.Windows.Forms.TableLayoutPanel tlpMapRemote;
        private AntdUI.Menu mMapRemote;
        private AntdUI.Checkbox cbEnable_MapRemote;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dMapLocal;
        private AntdUI.Divider dMapRemote;
        private AntdUI.Table tMapLocal;
        private System.Windows.Forms.TableLayoutPanel tlpMapLocal;
        private AntdUI.Menu mMapLocal;
        private AntdUI.Checkbox cbEnable_MapLocal;
    }
}

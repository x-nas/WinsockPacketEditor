namespace WPE.Lib
{
    partial class MapSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem9 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem10 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSettingsForm));
            this.tlpMapSettings = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dMapLocal = new AntdUI.Divider();
            this.dMapRemote = new AntdUI.Divider();
            this.tMapLocal = new AntdUI.Table();
            this.tlpMapLocal = new System.Windows.Forms.TableLayoutPanel();
            this.cbEnable_MapLocal = new AntdUI.Checkbox();
            this.tlpMapRemote = new System.Windows.Forms.TableLayoutPanel();
            this.cbEnable_MapRemote = new AntdUI.Checkbox();
            this.tMapRemote = new AntdUI.Table();
            this.mMapLocal = new AntdUI.Menu();
            this.mMapRemote = new AntdUI.Menu();
            this.tlpMapSettings.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpMapLocal.SuspendLayout();
            this.tlpMapRemote.SuspendLayout();
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
            this.tlpMapSettings.Size = new System.Drawing.Size(784, 761);
            this.tlpMapSettings.TabIndex = 1;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 700);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(784, 61);
            this.tlpButton.TabIndex = 3;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.Location = new System.Drawing.Point(265, 7);
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
            this.bExit.Location = new System.Drawing.Point(405, 7);
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
            this.dMapLocal.Location = new System.Drawing.Point(3, 3);
            this.dMapLocal.Name = "dMapLocal";
            this.dMapLocal.Orientation = AntdUI.TOrientation.Left;
            this.dMapLocal.Size = new System.Drawing.Size(778, 23);
            this.dMapLocal.TabIndex = 4;
            this.dMapLocal.Text = "本地映射";
            // 
            // dMapRemote
            // 
            this.dMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMapRemote.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMapRemote.Location = new System.Drawing.Point(3, 363);
            this.dMapRemote.Name = "dMapRemote";
            this.dMapRemote.Orientation = AntdUI.TOrientation.Left;
            this.dMapRemote.Size = new System.Drawing.Size(778, 23);
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
            this.tMapLocal.Location = new System.Drawing.Point(3, 87);
            this.tMapLocal.Name = "tMapLocal";
            this.tMapLocal.Size = new System.Drawing.Size(778, 250);
            this.tMapLocal.TabIndex = 8;
            this.tMapLocal.CellClick += new AntdUI.Table.ClickEventHandler(this.tMapLocal_CellClick);
            this.tMapLocal.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tMapLocal_CellButtonClick);
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
            this.tlpMapLocal.Size = new System.Drawing.Size(784, 55);
            this.tlpMapLocal.TabIndex = 9;
            // 
            // cbEnable_MapLocal
            // 
            this.cbEnable_MapLocal.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_MapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_MapLocal.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_MapLocal.Name = "cbEnable_MapLocal";
            this.cbEnable_MapLocal.Size = new System.Drawing.Size(138, 45);
            this.cbEnable_MapLocal.TabIndex = 8;
            this.cbEnable_MapLocal.Text = "启用本地映射";
            this.cbEnable_MapLocal.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_MapLocal_CheckedChanged);
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
            this.tlpMapRemote.Location = new System.Drawing.Point(0, 389);
            this.tlpMapRemote.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMapRemote.Name = "tlpMapRemote";
            this.tlpMapRemote.RowCount = 2;
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapRemote.Size = new System.Drawing.Size(784, 55);
            this.tlpMapRemote.TabIndex = 10;
            // 
            // cbEnable_MapRemote
            // 
            this.cbEnable_MapRemote.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_MapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_MapRemote.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_MapRemote.Name = "cbEnable_MapRemote";
            this.cbEnable_MapRemote.Size = new System.Drawing.Size(138, 45);
            this.cbEnable_MapRemote.TabIndex = 8;
            this.cbEnable_MapRemote.Text = "启用远程映射";
            this.cbEnable_MapRemote.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_MapRemote_CheckedChanged);
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
            this.tMapRemote.Location = new System.Drawing.Point(3, 447);
            this.tMapRemote.Name = "tMapRemote";
            this.tMapRemote.Size = new System.Drawing.Size(778, 250);
            this.tMapRemote.TabIndex = 11;
            this.tMapRemote.CellClick += new AntdUI.Table.ClickEventHandler(this.tMapRemote_CellClick);
            this.tMapRemote.CellButtonUp += new AntdUI.Table.ClickButtonEventHandler(this.tMapRemote_CellButtonUp);
            // 
            // mMapLocal
            // 
            this.mMapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mMapLocal.Gap = 0;
            this.mMapLocal.IconRatio = 1F;
            menuItem6.IconSvg = "PlusOutlined";
            menuItem7.IconSvg = "UserAddOutlined";
            menuItem7.ID = "miAdd";
            menuItem7.Text = "新增";
            menuItem8.IconSvg = "FolderOpenOutlined";
            menuItem8.ID = "miImport";
            menuItem8.Text = "导入本地映射";
            menuItem9.IconSvg = "DeliveredProcedureOutlined";
            menuItem9.ID = "miExport";
            menuItem9.Text = "导出本地映射";
            menuItem10.IconSvg = "DeleteOutlined";
            menuItem10.ID = "miClear";
            menuItem10.Text = "清空本地映射";
            menuItem6.Sub.Add(menuItem7);
            menuItem6.Sub.Add(menuItem8);
            menuItem6.Sub.Add(menuItem9);
            menuItem6.Sub.Add(menuItem10);
            this.mMapLocal.Items.Add(menuItem6);
            this.mMapLocal.Location = new System.Drawing.Point(721, 3);
            this.mMapLocal.Mode = AntdUI.TMenuMode.Horizontal;
            this.mMapLocal.Name = "mMapLocal";
            this.mMapLocal.Size = new System.Drawing.Size(60, 45);
            this.mMapLocal.TabIndex = 9;
            this.mMapLocal.Trigger = AntdUI.Trigger.Click;
            this.mMapLocal.SelectChanged += new AntdUI.SelectEventHandler(this.mMapLocal_SelectChanged);
            // 
            // mMapRemote
            // 
            this.mMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mMapRemote.Gap = 0;
            this.mMapRemote.IconRatio = 1F;
            menuItem1.IconSvg = "PlusOutlined";
            menuItem2.IconSvg = "UserAddOutlined";
            menuItem2.ID = "miAdd";
            menuItem2.Text = "新增";
            menuItem3.IconSvg = "FolderOpenOutlined";
            menuItem3.ID = "miImport";
            menuItem3.Text = "导入远程映射";
            menuItem4.IconSvg = "DeliveredProcedureOutlined";
            menuItem4.ID = "miExport";
            menuItem4.Text = "导出远程映射";
            menuItem5.IconSvg = "DeleteOutlined";
            menuItem5.ID = "miClear";
            menuItem5.Text = "清空远程映射";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Sub.Add(menuItem4);
            menuItem1.Sub.Add(menuItem5);
            this.mMapRemote.Items.Add(menuItem1);
            this.mMapRemote.Location = new System.Drawing.Point(721, 3);
            this.mMapRemote.Mode = AntdUI.TMenuMode.Horizontal;
            this.mMapRemote.Name = "mMapRemote";
            this.mMapRemote.Size = new System.Drawing.Size(60, 45);
            this.mMapRemote.TabIndex = 10;
            this.mMapRemote.Trigger = AntdUI.Trigger.Click;
            this.mMapRemote.SelectChanged += new AntdUI.SelectEventHandler(this.mMapRemote_SelectChanged);
            // 
            // MapSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.tlpMapSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "MapSettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapSettingsForm";
            this.Load += new System.EventHandler(this.MapSettingsForm_Load);
            this.tlpMapSettings.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpMapLocal.ResumeLayout(false);
            this.tlpMapLocal.PerformLayout();
            this.tlpMapRemote.ResumeLayout(false);
            this.tlpMapRemote.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMapSettings;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dMapLocal;
        private AntdUI.Divider dMapRemote;
        private AntdUI.Table tMapLocal;
        private System.Windows.Forms.TableLayoutPanel tlpMapLocal;
        private AntdUI.Checkbox cbEnable_MapLocal;
        private System.Windows.Forms.TableLayoutPanel tlpMapRemote;
        private AntdUI.Checkbox cbEnable_MapRemote;
        private AntdUI.Table tMapRemote;
        private AntdUI.Menu mMapLocal;
        private AntdUI.Menu mMapRemote;
    }
}
namespace WinsockPacketEditor
{
    partial class DataBaseSetting
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
            this.tlpDataBaseSetting = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpDataBaseInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lDataBaseVersion = new AntdUI.Label();
            this.lDataBasePath = new AntdUI.Label();
            this.txtDataBasePath = new AntdUI.Input();
            this.txtDataBaseVersion = new AntdUI.Input();
            this.bSelectPath = new AntdUI.Button();
            this.lNotice = new AntdUI.Label();
            this.tlpDataBaseSetting.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpDataBaseInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpDataBaseSetting
            // 
            this.tlpDataBaseSetting.ColumnCount = 1;
            this.tlpDataBaseSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDataBaseSetting.Controls.Add(this.lNotice, 0, 1);
            this.tlpDataBaseSetting.Controls.Add(this.tlpButton, 0, 2);
            this.tlpDataBaseSetting.Controls.Add(this.tlpDataBaseInfo, 0, 0);
            this.tlpDataBaseSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDataBaseSetting.Location = new System.Drawing.Point(0, 0);
            this.tlpDataBaseSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDataBaseSetting.Name = "tlpDataBaseSetting";
            this.tlpDataBaseSetting.RowCount = 3;
            this.tlpDataBaseSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpDataBaseSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDataBaseSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpDataBaseSetting.Size = new System.Drawing.Size(500, 250);
            this.tlpDataBaseSetting.TabIndex = 0;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 200);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 50);
            this.tlpButton.TabIndex = 19;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(178, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 38);
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
            this.bExit.Location = new System.Drawing.Point(259, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 38);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpDataBaseInfo
            // 
            this.tlpDataBaseInfo.ColumnCount = 3;
            this.tlpDataBaseInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDataBaseInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDataBaseInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDataBaseInfo.Controls.Add(this.lDataBaseVersion, 0, 3);
            this.tlpDataBaseInfo.Controls.Add(this.lDataBasePath, 0, 2);
            this.tlpDataBaseInfo.Controls.Add(this.txtDataBasePath, 1, 2);
            this.tlpDataBaseInfo.Controls.Add(this.txtDataBaseVersion, 1, 3);
            this.tlpDataBaseInfo.Controls.Add(this.bSelectPath, 2, 2);
            this.tlpDataBaseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDataBaseInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpDataBaseInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDataBaseInfo.Name = "tlpDataBaseInfo";
            this.tlpDataBaseInfo.RowCount = 5;
            this.tlpDataBaseInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDataBaseInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDataBaseInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDataBaseInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDataBaseInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDataBaseInfo.Size = new System.Drawing.Size(500, 100);
            this.tlpDataBaseInfo.TabIndex = 20;
            // 
            // lDataBaseVersion
            // 
            this.lDataBaseVersion.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDataBaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDataBaseVersion.Location = new System.Drawing.Point(3, 55);
            this.lDataBaseVersion.Name = "lDataBaseVersion";
            this.lDataBaseVersion.Size = new System.Drawing.Size(67, 31);
            this.lDataBaseVersion.TabIndex = 4;
            this.lDataBaseVersion.Text = "数据库名称 :";
            // 
            // lDataBasePath
            // 
            this.lDataBasePath.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDataBasePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDataBasePath.Location = new System.Drawing.Point(3, 13);
            this.lDataBasePath.Name = "lDataBasePath";
            this.lDataBasePath.Size = new System.Drawing.Size(67, 36);
            this.lDataBasePath.TabIndex = 0;
            this.lDataBasePath.Text = "数据库路径 :";
            // 
            // txtDataBasePath
            // 
            this.txtDataBasePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataBasePath.Location = new System.Drawing.Point(74, 11);
            this.txtDataBasePath.Margin = new System.Windows.Forms.Padding(1);
            this.txtDataBasePath.Name = "txtDataBasePath";
            this.txtDataBasePath.ReadOnly = true;
            this.txtDataBasePath.Size = new System.Drawing.Size(383, 40);
            this.txtDataBasePath.TabIndex = 5;
            // 
            // txtDataBaseVersion
            // 
            this.txtDataBaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataBaseVersion.Location = new System.Drawing.Point(74, 53);
            this.txtDataBaseVersion.Margin = new System.Windows.Forms.Padding(1);
            this.txtDataBaseVersion.Name = "txtDataBaseVersion";
            this.txtDataBaseVersion.ReadOnly = true;
            this.txtDataBaseVersion.Size = new System.Drawing.Size(383, 35);
            this.txtDataBaseVersion.TabIndex = 6;
            // 
            // bSelectPath
            // 
            this.bSelectPath.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bSelectPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSelectPath.IconRatio = 0.9F;
            this.bSelectPath.IconSvg = "FolderOpenOutlined";
            this.bSelectPath.Location = new System.Drawing.Point(461, 13);
            this.bSelectPath.Name = "bSelectPath";
            this.bSelectPath.Size = new System.Drawing.Size(36, 36);
            this.bSelectPath.TabIndex = 8;
            this.bSelectPath.Type = AntdUI.TTypeMini.Success;
            this.bSelectPath.Click += new System.EventHandler(this.bSelectPath_Click);
            // 
            // lNotice
            // 
            this.lNotice.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNotice.ForeColor = System.Drawing.Color.Firebrick;
            this.lNotice.Location = new System.Drawing.Point(3, 103);
            this.lNotice.Name = "lNotice";
            this.lNotice.Size = new System.Drawing.Size(408, 94);
            this.lNotice.TabIndex = 21;
            this.lNotice.Text = "如需多开软件，请在每次选择模式前设置不同的 [ 数据库路径 ]\r\n数据库路径下如果没有当前版本号的文件，则系统会自动创建一个空的数据库";
            this.lNotice.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // DataBaseSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpDataBaseSetting);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "DataBaseSetting";
            this.Size = new System.Drawing.Size(500, 250);
            this.Load += new System.EventHandler(this.DataBaseSetting_Load);
            this.tlpDataBaseSetting.ResumeLayout(false);
            this.tlpDataBaseSetting.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpDataBaseInfo.ResumeLayout(false);
            this.tlpDataBaseInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpDataBaseSetting;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpDataBaseInfo;
        private AntdUI.Label lDataBaseVersion;
        private AntdUI.Label lDataBasePath;
        private AntdUI.Input txtDataBasePath;
        private AntdUI.Input txtDataBaseVersion;
        private AntdUI.Button bSelectPath;
        private AntdUI.Label lNotice;
    }
}

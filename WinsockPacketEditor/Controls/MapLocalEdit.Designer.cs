namespace WinsockPacketEditor
{
    partial class MapLocalEdit
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
            this.tlpMapLocal = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dLocal = new AntdUI.Divider();
            this.tlpRemote = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lRemotePath = new AntdUI.Label();
            this.lPort = new AntdUI.Label();
            this.lHost = new AntdUI.Label();
            this.lProtocolType = new AntdUI.Label();
            this.ddlProtocolType = new AntdUI.Select();
            this.txtHost = new AntdUI.Input();
            this.nudPort = new AntdUI.InputNumber();
            this.txtRemotePath = new AntdUI.Input();
            this.dRemote = new AntdUI.Divider();
            this.udLocalPath = new AntdUI.UploadDragger();
            this.txtLocalPath = new AntdUI.Input();
            this.tlpMapLocal.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMapLocal
            // 
            this.tlpMapLocal.ColumnCount = 1;
            this.tlpMapLocal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapLocal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpMapLocal.Controls.Add(this.tlpButton, 0, 5);
            this.tlpMapLocal.Controls.Add(this.dLocal, 0, 2);
            this.tlpMapLocal.Controls.Add(this.tlpRemote, 0, 1);
            this.tlpMapLocal.Controls.Add(this.dRemote, 0, 0);
            this.tlpMapLocal.Controls.Add(this.udLocalPath, 0, 4);
            this.tlpMapLocal.Controls.Add(this.txtLocalPath, 0, 3);
            this.tlpMapLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapLocal.Location = new System.Drawing.Point(0, 0);
            this.tlpMapLocal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlpMapLocal.Name = "tlpMapLocal";
            this.tlpMapLocal.RowCount = 6;
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 202F));
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapLocal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpMapLocal.Size = new System.Drawing.Size(500, 700);
            this.tlpMapLocal.TabIndex = 2;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 651);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 49);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(178, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.bExit.Location = new System.Drawing.Point(259, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dLocal
            // 
            this.dLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLocal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLocal.LocalizationText = "MapLocalForm.Local";
            this.dLocal.Location = new System.Drawing.Point(2, 227);
            this.dLocal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dLocal.Name = "dLocal";
            this.dLocal.Orientation = AntdUI.TOrientation.Left;
            this.dLocal.Size = new System.Drawing.Size(496, 19);
            this.dLocal.TabIndex = 5;
            this.dLocal.Text = "本地文件";
            // 
            // tlpRemote
            // 
            this.tlpRemote.ColumnCount = 2;
            this.tlpRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemote.Controls.Add(this.lRemotePath, 0, 3);
            this.tlpRemote.Controls.Add(this.lPort, 0, 2);
            this.tlpRemote.Controls.Add(this.lHost, 0, 1);
            this.tlpRemote.Controls.Add(this.lProtocolType, 0, 0);
            this.tlpRemote.Controls.Add(this.ddlProtocolType, 1, 0);
            this.tlpRemote.Controls.Add(this.txtHost, 1, 1);
            this.tlpRemote.Controls.Add(this.nudPort, 1, 2);
            this.tlpRemote.Controls.Add(this.txtRemotePath, 1, 3);
            this.tlpRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemote.Location = new System.Drawing.Point(0, 23);
            this.tlpRemote.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRemote.Name = "tlpRemote";
            this.tlpRemote.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tlpRemote.RowCount = 5;
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpRemote.Size = new System.Drawing.Size(500, 202);
            this.tlpRemote.TabIndex = 4;
            // 
            // lRemotePath
            // 
            this.lRemotePath.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRemotePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemotePath.LocalizationText = "Path";
            this.lRemotePath.Location = new System.Drawing.Point(4, 124);
            this.lRemotePath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lRemotePath.Name = "lRemotePath";
            this.lRemotePath.Size = new System.Drawing.Size(24, 36);
            this.lRemotePath.TabIndex = 16;
            this.lRemotePath.Text = "路径";
            // 
            // lPort
            // 
            this.lPort.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPort.LocalizationText = "Port";
            this.lPort.Location = new System.Drawing.Point(4, 84);
            this.lPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(24, 36);
            this.lPort.TabIndex = 14;
            this.lPort.Text = "端口";
            // 
            // lHost
            // 
            this.lHost.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lHost.LocalizationText = "Host";
            this.lHost.Location = new System.Drawing.Point(4, 44);
            this.lHost.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lHost.Name = "lHost";
            this.lHost.Size = new System.Drawing.Size(48, 36);
            this.lHost.TabIndex = 12;
            this.lHost.Text = "主机地址";
            // 
            // lProtocolType
            // 
            this.lProtocolType.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProtocolType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProtocolType.LocalizationText = "Protocol";
            this.lProtocolType.Location = new System.Drawing.Point(4, 4);
            this.lProtocolType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lProtocolType.Name = "lProtocolType";
            this.lProtocolType.Size = new System.Drawing.Size(24, 36);
            this.lProtocolType.TabIndex = 10;
            this.lProtocolType.Text = "协议";
            // 
            // ddlProtocolType
            // 
            this.ddlProtocolType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlProtocolType.Items.AddRange(new object[] {
            "http"});
            this.ddlProtocolType.List = true;
            this.ddlProtocolType.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlProtocolType.Location = new System.Drawing.Point(56, 4);
            this.ddlProtocolType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ddlProtocolType.Name = "ddlProtocolType";
            this.ddlProtocolType.PlaceholderText = "请选择";
            this.ddlProtocolType.Size = new System.Drawing.Size(440, 36);
            this.ddlProtocolType.TabIndex = 11;
            // 
            // txtHost
            // 
            this.txtHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHost.Location = new System.Drawing.Point(56, 44);
            this.txtHost.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtHost.Name = "txtHost";
            this.txtHost.PrefixText = "http://";
            this.txtHost.Size = new System.Drawing.Size(440, 36);
            this.txtHost.TabIndex = 13;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // nudPort
            // 
            this.nudPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPort.Location = new System.Drawing.Point(56, 84);
            this.nudPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.SelectionStart = 1;
            this.nudPort.Size = new System.Drawing.Size(440, 36);
            this.nudPort.TabIndex = 15;
            this.nudPort.Text = "80";
            this.nudPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // txtRemotePath
            // 
            this.txtRemotePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemotePath.LocalizationPlaceholderText = "MapLocalForm.InputPath";
            this.txtRemotePath.Location = new System.Drawing.Point(56, 124);
            this.txtRemotePath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRemotePath.Name = "txtRemotePath";
            this.txtRemotePath.PlaceholderText = "请填写远端路径";
            this.txtRemotePath.Size = new System.Drawing.Size(440, 36);
            this.txtRemotePath.TabIndex = 17;
            // 
            // dRemote
            // 
            this.dRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dRemote.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dRemote.LocalizationText = "MapLocalForm.Remote";
            this.dRemote.Location = new System.Drawing.Point(2, 2);
            this.dRemote.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dRemote.Name = "dRemote";
            this.dRemote.Orientation = AntdUI.TOrientation.Left;
            this.dRemote.Size = new System.Drawing.Size(496, 19);
            this.dRemote.TabIndex = 3;
            this.dRemote.Text = "远端地址";
            // 
            // udLocalPath
            // 
            this.udLocalPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udLocalPath.Filter = "";
            this.udLocalPath.LocalizationText = "MapLocalForm.DragFiles";
            this.udLocalPath.Location = new System.Drawing.Point(2, 290);
            this.udLocalPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.udLocalPath.Multiselect = false;
            this.udLocalPath.Name = "udLocalPath";
            this.udLocalPath.Size = new System.Drawing.Size(496, 359);
            this.udLocalPath.TabIndex = 6;
            this.udLocalPath.Text = "点击或拖拽文件到此区域";
            this.udLocalPath.TextDesc = "";
            this.udLocalPath.DragChanged += new AntdUI.IControl.DragEventHandler(this.udLocalPath_DragChanged);
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocalPath.LocalizationPlaceholderText = "MapLocalForm.SelectLocal";
            this.txtLocalPath.Location = new System.Drawing.Point(2, 250);
            this.txtLocalPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.PlaceholderText = "请选择本地文件";
            this.txtLocalPath.PrefixSvg = "FolderOpenOutlined";
            this.txtLocalPath.ReadOnly = true;
            this.txtLocalPath.Size = new System.Drawing.Size(496, 36);
            this.txtLocalPath.TabIndex = 7;
            this.txtLocalPath.TextChanged += new System.EventHandler(this.txtLocalPath_TextChanged);
            // 
            // MapLocalEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMapLocal);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MapLocalEdit";
            this.Size = new System.Drawing.Size(500, 700);
            this.Load += new System.EventHandler(this.MapLocalEdit_Load);
            this.tlpMapLocal.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpRemote.ResumeLayout(false);
            this.tlpRemote.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpMapLocal;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dLocal;
        private TableLayoutPanelEx tlpRemote;
        private AntdUI.Label lRemotePath;
        private AntdUI.Label lPort;
        private AntdUI.Label lHost;
        private AntdUI.Label lProtocolType;
        private AntdUI.Select ddlProtocolType;
        private AntdUI.Input txtHost;
        private AntdUI.InputNumber nudPort;
        private AntdUI.Input txtRemotePath;
        private AntdUI.Divider dRemote;
        private AntdUI.UploadDragger udLocalPath;
        private AntdUI.Input txtLocalPath;
    }
}

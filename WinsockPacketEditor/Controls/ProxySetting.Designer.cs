namespace WinsockPacketEditor
{
    partial class ProxySetting
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
            this.tlpProxySettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dMaxConnection = new AntdUI.Divider();
            this.tableLayoutPanel1 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbEnable_Auth = new AntdUI.Checkbox();
            this.ddlAuthType = new AntdUI.Select();
            this.dProxyAuth = new AntdUI.Divider();
            this.dSystemProxy = new AntdUI.Divider();
            this.tlpProxyType = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbEnable_SOCKS5 = new AntdUI.Checkbox();
            this.nudSOCKS5Port = new AntdUI.InputNumber();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dProxyServerIP = new AntdUI.Divider();
            this.dProxyType = new AntdUI.Divider();
            this.tlpProxyServerIP = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbProxyIP_Auto = new AntdUI.Checkbox();
            this.ddlProxyIP_Appoint = new AntdUI.Select();
            this.switchSystemProxy = new AntdUI.Switch();
            this.txtMaxConnectionNumber = new AntdUI.InputNumber();
            this.tlpProxySettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlpProxyType.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpProxyServerIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxySettings
            // 
            this.tlpProxySettings.ColumnCount = 1;
            this.tlpProxySettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxySettings.Controls.Add(this.dMaxConnection, 0, 16);
            this.tlpProxySettings.Controls.Add(this.tableLayoutPanel1, 0, 10);
            this.tlpProxySettings.Controls.Add(this.dProxyAuth, 0, 8);
            this.tlpProxySettings.Controls.Add(this.dSystemProxy, 0, 12);
            this.tlpProxySettings.Controls.Add(this.tlpProxyType, 0, 6);
            this.tlpProxySettings.Controls.Add(this.tlpButton, 0, 20);
            this.tlpProxySettings.Controls.Add(this.dProxyServerIP, 0, 0);
            this.tlpProxySettings.Controls.Add(this.dProxyType, 0, 4);
            this.tlpProxySettings.Controls.Add(this.tlpProxyServerIP, 0, 2);
            this.tlpProxySettings.Controls.Add(this.switchSystemProxy, 0, 14);
            this.tlpProxySettings.Controls.Add(this.txtMaxConnectionNumber, 0, 18);
            this.tlpProxySettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxySettings.Location = new System.Drawing.Point(0, 0);
            this.tlpProxySettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxySettings.Name = "tlpProxySettings";
            this.tlpProxySettings.RowCount = 21;
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxySettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpProxySettings.Size = new System.Drawing.Size(500, 700);
            this.tlpProxySettings.TabIndex = 2;
            // 
            // dMaxConnection
            // 
            this.dMaxConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMaxConnection.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMaxConnection.LocalizationText = "ProxySettingsForm.MaxConnection";
            this.dMaxConnection.Location = new System.Drawing.Point(3, 495);
            this.dMaxConnection.Name = "dMaxConnection";
            this.dMaxConnection.Orientation = AntdUI.TOrientation.Left;
            this.dMaxConnection.Size = new System.Drawing.Size(494, 23);
            this.dMaxConnection.TabIndex = 13;
            this.dMaxConnection.Text = "最大连接数";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbEnable_Auth, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ddlAuthType, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 307);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 60);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // cbEnable_Auth
            // 
            this.cbEnable_Auth.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_Auth.Checked = true;
            this.cbEnable_Auth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_Auth.LocalizationText = "ProxySettingsForm.EnableAuth";
            this.cbEnable_Auth.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_Auth.Name = "cbEnable_Auth";
            this.cbEnable_Auth.Size = new System.Drawing.Size(140, 42);
            this.cbEnable_Auth.TabIndex = 0;
            this.cbEnable_Auth.Text = "启用代理认证";
            this.cbEnable_Auth.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_Auth_CheckedChanged);
            // 
            // ddlAuthType
            // 
            this.ddlAuthType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlAuthType.List = true;
            this.ddlAuthType.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlAuthType.Location = new System.Drawing.Point(149, 3);
            this.ddlAuthType.Name = "ddlAuthType";
            this.ddlAuthType.PlaceholderText = "请选择";
            this.ddlAuthType.Size = new System.Drawing.Size(348, 42);
            this.ddlAuthType.TabIndex = 1;
            // 
            // dProxyAuth
            // 
            this.dProxyAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProxyAuth.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProxyAuth.LocalizationText = "ProxySettingsForm.ProxyAuth";
            this.dProxyAuth.Location = new System.Drawing.Point(3, 261);
            this.dProxyAuth.Name = "dProxyAuth";
            this.dProxyAuth.Orientation = AntdUI.TOrientation.Left;
            this.dProxyAuth.Size = new System.Drawing.Size(494, 23);
            this.dProxyAuth.TabIndex = 11;
            this.dProxyAuth.Text = "代理认证";
            // 
            // dSystemProxy
            // 
            this.dSystemProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSystemProxy.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSystemProxy.LocalizationText = "ProxySettingsForm.SystemProxy";
            this.dSystemProxy.Location = new System.Drawing.Point(3, 390);
            this.dSystemProxy.Name = "dSystemProxy";
            this.dSystemProxy.Orientation = AntdUI.TOrientation.Left;
            this.dSystemProxy.Size = new System.Drawing.Size(494, 23);
            this.dSystemProxy.TabIndex = 9;
            this.dSystemProxy.Text = "系统代理";
            // 
            // tlpProxyType
            // 
            this.tlpProxyType.ColumnCount = 2;
            this.tlpProxyType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyType.Controls.Add(this.cbEnable_SOCKS5, 0, 0);
            this.tlpProxyType.Controls.Add(this.nudSOCKS5Port, 1, 0);
            this.tlpProxyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyType.Location = new System.Drawing.Point(0, 178);
            this.tlpProxyType.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyType.Name = "tlpProxyType";
            this.tlpProxyType.RowCount = 2;
            this.tlpProxyType.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyType.Size = new System.Drawing.Size(500, 60);
            this.tlpProxyType.TabIndex = 8;
            // 
            // cbEnable_SOCKS5
            // 
            this.cbEnable_SOCKS5.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_SOCKS5.Checked = true;
            this.cbEnable_SOCKS5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_SOCKS5.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_SOCKS5.Name = "cbEnable_SOCKS5";
            this.cbEnable_SOCKS5.Size = new System.Drawing.Size(111, 42);
            this.cbEnable_SOCKS5.TabIndex = 0;
            this.cbEnable_SOCKS5.Text = "SOCKS 5";
            this.cbEnable_SOCKS5.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_SOCKS5_CheckedChanged);
            // 
            // nudSOCKS5Port
            // 
            this.nudSOCKS5Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudSOCKS5Port.LocalizationPrefixText = "ProxySettingsForm.Port";
            this.nudSOCKS5Port.Location = new System.Drawing.Point(120, 3);
            this.nudSOCKS5Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSOCKS5Port.Name = "nudSOCKS5Port";
            this.nudSOCKS5Port.PrefixText = "端口:";
            this.nudSOCKS5Port.SelectionStart = 1;
            this.nudSOCKS5Port.Size = new System.Drawing.Size(377, 42);
            this.nudSOCKS5Port.TabIndex = 1;
            this.nudSOCKS5Port.Text = "1080";
            this.nudSOCKS5Port.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
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
            this.tlpButton.Size = new System.Drawing.Size(500, 60);
            this.tlpButton.TabIndex = 3;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(155, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(82, 46);
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
            this.bExit.Location = new System.Drawing.Point(263, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(82, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dProxyServerIP
            // 
            this.dProxyServerIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProxyServerIP.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProxyServerIP.LocalizationText = "ProxySettingsForm.ProxyServerIP";
            this.dProxyServerIP.Location = new System.Drawing.Point(3, 3);
            this.dProxyServerIP.Name = "dProxyServerIP";
            this.dProxyServerIP.Orientation = AntdUI.TOrientation.Left;
            this.dProxyServerIP.Size = new System.Drawing.Size(494, 23);
            this.dProxyServerIP.TabIndex = 4;
            this.dProxyServerIP.Text = "代理服务IP地址";
            // 
            // dProxyType
            // 
            this.dProxyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProxyType.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProxyType.LocalizationText = "ProxySettingsForm.ProxyType";
            this.dProxyType.Location = new System.Drawing.Point(3, 132);
            this.dProxyType.Name = "dProxyType";
            this.dProxyType.Orientation = AntdUI.TOrientation.Left;
            this.dProxyType.Size = new System.Drawing.Size(494, 23);
            this.dProxyType.TabIndex = 6;
            this.dProxyType.Text = "代理类型";
            // 
            // tlpProxyServerIP
            // 
            this.tlpProxyServerIP.ColumnCount = 2;
            this.tlpProxyServerIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpProxyServerIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyServerIP.Controls.Add(this.cbProxyIP_Auto, 0, 0);
            this.tlpProxyServerIP.Controls.Add(this.ddlProxyIP_Appoint, 1, 0);
            this.tlpProxyServerIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyServerIP.Location = new System.Drawing.Point(0, 49);
            this.tlpProxyServerIP.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyServerIP.Name = "tlpProxyServerIP";
            this.tlpProxyServerIP.RowCount = 2;
            this.tlpProxyServerIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyServerIP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyServerIP.Size = new System.Drawing.Size(500, 60);
            this.tlpProxyServerIP.TabIndex = 7;
            // 
            // cbProxyIP_Auto
            // 
            this.cbProxyIP_Auto.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbProxyIP_Auto.Checked = true;
            this.cbProxyIP_Auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbProxyIP_Auto.LocalizationText = "ProxySettingsForm.ProxyIPAuto";
            this.cbProxyIP_Auto.Location = new System.Drawing.Point(3, 3);
            this.cbProxyIP_Auto.Name = "cbProxyIP_Auto";
            this.cbProxyIP_Auto.Size = new System.Drawing.Size(108, 42);
            this.cbProxyIP_Auto.TabIndex = 0;
            this.cbProxyIP_Auto.Text = "自动检测";
            this.cbProxyIP_Auto.CheckedChanged += new AntdUI.BoolEventHandler(this.cbProxyIP_Auto_CheckedChanged);
            // 
            // ddlProxyIP_Appoint
            // 
            this.ddlProxyIP_Appoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlProxyIP_Appoint.List = true;
            this.ddlProxyIP_Appoint.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlProxyIP_Appoint.Location = new System.Drawing.Point(117, 3);
            this.ddlProxyIP_Appoint.Name = "ddlProxyIP_Appoint";
            this.ddlProxyIP_Appoint.PlaceholderText = "请选择";
            this.ddlProxyIP_Appoint.Size = new System.Drawing.Size(380, 42);
            this.ddlProxyIP_Appoint.TabIndex = 1;
            // 
            // switchSystemProxy
            // 
            this.switchSystemProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.switchSystemProxy.Location = new System.Drawing.Point(6, 439);
            this.switchSystemProxy.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.switchSystemProxy.Name = "switchSystemProxy";
            this.switchSystemProxy.Size = new System.Drawing.Size(50, 30);
            this.switchSystemProxy.TabIndex = 10;
            this.switchSystemProxy.CheckedChanged += new AntdUI.BoolEventHandler(this.switchSystemProxy_CheckedChanged);
            // 
            // txtMaxConnectionNumber
            // 
            this.txtMaxConnectionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMaxConnectionNumber.Location = new System.Drawing.Point(3, 544);
            this.txtMaxConnectionNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtMaxConnectionNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMaxConnectionNumber.Name = "txtMaxConnectionNumber";
            this.txtMaxConnectionNumber.Size = new System.Drawing.Size(494, 42);
            this.txtMaxConnectionNumber.TabIndex = 14;
            this.txtMaxConnectionNumber.Text = "1";
            this.txtMaxConnectionNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ProxySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpProxySettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ProxySetting";
            this.Size = new System.Drawing.Size(500, 700);
            this.Load += new System.EventHandler(this.ProxySetting_Load);
            this.tlpProxySettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tlpProxyType.ResumeLayout(false);
            this.tlpProxyType.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpProxyServerIP.ResumeLayout(false);
            this.tlpProxyServerIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpProxySettings;
        private TableLayoutPanelEx tableLayoutPanel1;
        private AntdUI.Checkbox cbEnable_Auth;
        private AntdUI.Select ddlAuthType;
        private AntdUI.Divider dProxyAuth;
        private AntdUI.Divider dSystemProxy;
        private TableLayoutPanelEx tlpProxyType;
        private AntdUI.Checkbox cbEnable_SOCKS5;
        private AntdUI.InputNumber nudSOCKS5Port;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dProxyServerIP;
        private AntdUI.Divider dProxyType;
        private TableLayoutPanelEx tlpProxyServerIP;
        private AntdUI.Checkbox cbProxyIP_Auto;
        private AntdUI.Select ddlProxyIP_Appoint;
        private AntdUI.Switch switchSystemProxy;
        private AntdUI.Divider dMaxConnection;
        private AntdUI.InputNumber txtMaxConnectionNumber;
    }
}

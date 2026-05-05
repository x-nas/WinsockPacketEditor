namespace WinsockPacketEditor
{
    partial class ServerEdit
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
            this.tlpServerEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpServerInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtVerifyURL = new AntdUI.Input();
            this.lVerifyURL = new AntdUI.Label();
            this.txtForgotURL = new AntdUI.Input();
            this.txtRegisterURL = new AntdUI.Input();
            this.lRegisterURL = new AntdUI.Label();
            this.lForgorURL = new AntdUI.Label();
            this.lServerPort = new AntdUI.Label();
            this.lServerName = new AntdUI.Label();
            this.lServerIP = new AntdUI.Label();
            this.txtServerName = new AntdUI.Input();
            this.txtServerIP = new AntdUI.Input();
            this.nudServerPort = new AntdUI.InputNumber();
            this.cbIsEnable = new AntdUI.Checkbox();
            this.tlpServerEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpServerEdit
            // 
            this.tlpServerEdit.ColumnCount = 1;
            this.tlpServerEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerEdit.Controls.Add(this.tlpButton, 0, 2);
            this.tlpServerEdit.Controls.Add(this.tlpServerInfo, 0, 1);
            this.tlpServerEdit.Controls.Add(this.cbIsEnable, 0, 0);
            this.tlpServerEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpServerEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpServerEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpServerEdit.Name = "tlpServerEdit";
            this.tlpServerEdit.RowCount = 3;
            this.tlpServerEdit.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpServerEdit.Size = new System.Drawing.Size(450, 350);
            this.tlpServerEdit.TabIndex = 3;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 300);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(450, 50);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(153, 6);
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
            this.bExit.Location = new System.Drawing.Point(234, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpServerInfo
            // 
            this.tlpServerInfo.ColumnCount = 4;
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpServerInfo.Controls.Add(this.txtVerifyURL, 2, 5);
            this.tlpServerInfo.Controls.Add(this.lVerifyURL, 1, 5);
            this.tlpServerInfo.Controls.Add(this.txtForgotURL, 2, 3);
            this.tlpServerInfo.Controls.Add(this.txtRegisterURL, 2, 4);
            this.tlpServerInfo.Controls.Add(this.lRegisterURL, 1, 4);
            this.tlpServerInfo.Controls.Add(this.lForgorURL, 1, 3);
            this.tlpServerInfo.Controls.Add(this.lServerPort, 1, 2);
            this.tlpServerInfo.Controls.Add(this.lServerName, 1, 0);
            this.tlpServerInfo.Controls.Add(this.lServerIP, 1, 1);
            this.tlpServerInfo.Controls.Add(this.txtServerName, 2, 0);
            this.tlpServerInfo.Controls.Add(this.txtServerIP, 2, 1);
            this.tlpServerInfo.Controls.Add(this.nudServerPort, 2, 2);
            this.tlpServerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpServerInfo.Location = new System.Drawing.Point(0, 36);
            this.tlpServerInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpServerInfo.Name = "tlpServerInfo";
            this.tlpServerInfo.RowCount = 7;
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerInfo.Size = new System.Drawing.Size(450, 264);
            this.tlpServerInfo.TabIndex = 1;
            // 
            // txtVerifyURL
            // 
            this.txtVerifyURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVerifyURL.LocalizationPlaceholderText = "WPCConfig.ServerList.URL.Input";
            this.txtVerifyURL.Location = new System.Drawing.Point(125, 202);
            this.txtVerifyURL.Margin = new System.Windows.Forms.Padding(2);
            this.txtVerifyURL.MaxLength = 20;
            this.txtVerifyURL.Name = "txtVerifyURL";
            this.txtVerifyURL.PlaceholderText = "请输入网址";
            this.txtVerifyURL.PrefixText = "http://";
            this.txtVerifyURL.Size = new System.Drawing.Size(303, 36);
            this.txtVerifyURL.TabIndex = 26;
            // 
            // lVerifyURL
            // 
            this.lVerifyURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lVerifyURL.LocalizationText = "WPCConfig.ServerList.VerifyURL";
            this.lVerifyURL.Location = new System.Drawing.Point(22, 202);
            this.lVerifyURL.Margin = new System.Windows.Forms.Padding(2);
            this.lVerifyURL.Name = "lVerifyURL";
            this.lVerifyURL.Size = new System.Drawing.Size(99, 36);
            this.lVerifyURL.TabIndex = 25;
            this.lVerifyURL.Text = "验证地址 :";
            this.lVerifyURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtForgotURL
            // 
            this.txtForgotURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtForgotURL.LocalizationPlaceholderText = "WPCConfig.ServerList.URL.Input";
            this.txtForgotURL.Location = new System.Drawing.Point(125, 122);
            this.txtForgotURL.Margin = new System.Windows.Forms.Padding(2);
            this.txtForgotURL.MaxLength = 20;
            this.txtForgotURL.Name = "txtForgotURL";
            this.txtForgotURL.PlaceholderText = "请输入网址";
            this.txtForgotURL.PrefixText = "http://";
            this.txtForgotURL.Size = new System.Drawing.Size(303, 36);
            this.txtForgotURL.TabIndex = 24;
            // 
            // txtRegisterURL
            // 
            this.txtRegisterURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRegisterURL.LocalizationPlaceholderText = "WPCConfig.ServerList.URL.Input";
            this.txtRegisterURL.Location = new System.Drawing.Point(125, 162);
            this.txtRegisterURL.Margin = new System.Windows.Forms.Padding(2);
            this.txtRegisterURL.MaxLength = 20;
            this.txtRegisterURL.Name = "txtRegisterURL";
            this.txtRegisterURL.PlaceholderText = "请输入网址";
            this.txtRegisterURL.PrefixText = "http://";
            this.txtRegisterURL.Size = new System.Drawing.Size(303, 36);
            this.txtRegisterURL.TabIndex = 23;
            // 
            // lRegisterURL
            // 
            this.lRegisterURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRegisterURL.LocalizationText = "WPCConfig.ServerList.RegisterURL";
            this.lRegisterURL.Location = new System.Drawing.Point(22, 162);
            this.lRegisterURL.Margin = new System.Windows.Forms.Padding(2);
            this.lRegisterURL.Name = "lRegisterURL";
            this.lRegisterURL.Size = new System.Drawing.Size(99, 36);
            this.lRegisterURL.TabIndex = 22;
            this.lRegisterURL.Text = "立即注册地址 :";
            this.lRegisterURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lForgorURL
            // 
            this.lForgorURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lForgorURL.LocalizationText = "WPCConfig.ServerList.ForgotURL";
            this.lForgorURL.Location = new System.Drawing.Point(22, 122);
            this.lForgorURL.Margin = new System.Windows.Forms.Padding(2);
            this.lForgorURL.Name = "lForgorURL";
            this.lForgorURL.Size = new System.Drawing.Size(99, 36);
            this.lForgorURL.TabIndex = 21;
            this.lForgorURL.Text = "找回密码地址 :";
            this.lForgorURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lServerPort
            // 
            this.lServerPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lServerPort.LocalizationText = "WPCConfig.ServerList.ServerPort";
            this.lServerPort.Location = new System.Drawing.Point(22, 82);
            this.lServerPort.Margin = new System.Windows.Forms.Padding(2);
            this.lServerPort.Name = "lServerPort";
            this.lServerPort.Size = new System.Drawing.Size(99, 36);
            this.lServerPort.TabIndex = 20;
            this.lServerPort.Text = "端口号 :";
            this.lServerPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lServerName
            // 
            this.lServerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lServerName.LocalizationText = "WPCConfig.ServerList.ServerName";
            this.lServerName.Location = new System.Drawing.Point(22, 2);
            this.lServerName.Margin = new System.Windows.Forms.Padding(2);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(99, 36);
            this.lServerName.TabIndex = 10;
            this.lServerName.Text = "服务器名称 :";
            this.lServerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lServerIP
            // 
            this.lServerIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lServerIP.LocalizationText = "WPCConfig.ServerList.ServerIP";
            this.lServerIP.Location = new System.Drawing.Point(22, 42);
            this.lServerIP.Margin = new System.Windows.Forms.Padding(2);
            this.lServerIP.Name = "lServerIP";
            this.lServerIP.Size = new System.Drawing.Size(99, 36);
            this.lServerIP.TabIndex = 11;
            this.lServerIP.Text = "IP地址 :";
            this.lServerIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtServerName
            // 
            this.txtServerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServerName.LocalizationPlaceholderText = "WPCConfig.ServerList.ServerName.Input";
            this.txtServerName.Location = new System.Drawing.Point(125, 2);
            this.txtServerName.Margin = new System.Windows.Forms.Padding(2);
            this.txtServerName.MaxLength = 50;
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.PlaceholderText = "请输入服务器名称";
            this.txtServerName.Size = new System.Drawing.Size(303, 36);
            this.txtServerName.TabIndex = 12;
            this.txtServerName.TextChanged += new System.EventHandler(this.txtServerName_TextChanged);
            // 
            // txtServerIP
            // 
            this.txtServerIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServerIP.LocalizationPlaceholderText = "WPCConfig.ServerList.ServerIP.Input";
            this.txtServerIP.Location = new System.Drawing.Point(125, 42);
            this.txtServerIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtServerIP.MaxLength = 20;
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.PlaceholderText = "请输入IP地址";
            this.txtServerIP.Size = new System.Drawing.Size(303, 36);
            this.txtServerIP.TabIndex = 13;
            this.txtServerIP.TextChanged += new System.EventHandler(this.txtServerIP_TextChanged);
            // 
            // nudServerPort
            // 
            this.nudServerPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudServerPort.Location = new System.Drawing.Point(125, 82);
            this.nudServerPort.Margin = new System.Windows.Forms.Padding(2);
            this.nudServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudServerPort.Name = "nudServerPort";
            this.nudServerPort.SelectionStart = 1;
            this.nudServerPort.Size = new System.Drawing.Size(303, 36);
            this.nudServerPort.TabIndex = 17;
            this.nudServerPort.Text = "1080";
            this.nudServerPort.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // cbIsEnable
            // 
            this.cbIsEnable.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsEnable.LocalizationText = "Enable";
            this.cbIsEnable.Location = new System.Drawing.Point(2, 2);
            this.cbIsEnable.Margin = new System.Windows.Forms.Padding(2);
            this.cbIsEnable.Name = "cbIsEnable";
            this.cbIsEnable.Size = new System.Drawing.Size(56, 32);
            this.cbIsEnable.TabIndex = 18;
            this.cbIsEnable.Text = "启用";
            this.cbIsEnable.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsEnable_CheckedChanged);
            // 
            // ServerEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpServerEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ServerEdit";
            this.Size = new System.Drawing.Size(450, 350);
            this.Load += new System.EventHandler(this.ServerEdit_Load);
            this.tlpServerEdit.ResumeLayout(false);
            this.tlpServerEdit.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpServerInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpServerEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpServerInfo;
        private AntdUI.Label lServerName;
        private AntdUI.Label lServerIP;
        private AntdUI.Input txtServerName;
        private AntdUI.Input txtServerIP;
        private AntdUI.Checkbox cbIsEnable;
        private AntdUI.Label lServerPort;
        private AntdUI.InputNumber nudServerPort;
        private AntdUI.Input txtForgotURL;
        private AntdUI.Input txtRegisterURL;
        private AntdUI.Label lRegisterURL;
        private AntdUI.Label lForgorURL;
        private AntdUI.Input txtVerifyURL;
        private AntdUI.Label lVerifyURL;
    }
}

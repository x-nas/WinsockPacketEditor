namespace WinsockPacketEditor
{
    partial class EXTProxySetting
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
            this.tlpExternalProxy = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbEnable_ExternalProxy = new AntdUI.Checkbox();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dEXTProxy = new AntdUI.Divider();
            this.tlpServerInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtExternalProxy_Port = new AntdUI.InputNumber();
            this.bExternalProxy_Detection = new AntdUI.Button();
            this.txtExternalProxy_PassWord = new AntdUI.Input();
            this.txtExternalProxy_UserName = new AntdUI.Input();
            this.cbExternalProxy_EnableAuth = new AntdUI.Checkbox();
            this.cbExternalProxy_AppointPort = new AntdUI.Checkbox();
            this.txtExternalProxy_IP = new AntdUI.Input();
            this.txtExternalProxy_AppointPort = new AntdUI.Input();
            this.tlpExternalProxy.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpExternalProxy
            // 
            this.tlpExternalProxy.ColumnCount = 1;
            this.tlpExternalProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExternalProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpExternalProxy.Controls.Add(this.cbEnable_ExternalProxy, 0, 2);
            this.tlpExternalProxy.Controls.Add(this.tlpButton, 0, 5);
            this.tlpExternalProxy.Controls.Add(this.dEXTProxy, 0, 0);
            this.tlpExternalProxy.Controls.Add(this.tlpServerInfo, 0, 3);
            this.tlpExternalProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExternalProxy.Location = new System.Drawing.Point(0, 0);
            this.tlpExternalProxy.Margin = new System.Windows.Forms.Padding(0);
            this.tlpExternalProxy.Name = "tlpExternalProxy";
            this.tlpExternalProxy.RowCount = 6;
            this.tlpExternalProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpExternalProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpExternalProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpExternalProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 486F));
            this.tlpExternalProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExternalProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpExternalProxy.Size = new System.Drawing.Size(350, 700);
            this.tlpExternalProxy.TabIndex = 3;
            // 
            // cbEnable_ExternalProxy
            // 
            this.cbEnable_ExternalProxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_ExternalProxy.Checked = true;
            this.cbEnable_ExternalProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_ExternalProxy.LocalizationText = "Enable";
            this.cbEnable_ExternalProxy.Location = new System.Drawing.Point(2, 41);
            this.cbEnable_ExternalProxy.Margin = new System.Windows.Forms.Padding(2);
            this.cbEnable_ExternalProxy.Name = "cbEnable_ExternalProxy";
            this.cbEnable_ExternalProxy.Size = new System.Drawing.Size(104, 36);
            this.cbEnable_ExternalProxy.TabIndex = 8;
            this.cbEnable_ExternalProxy.Text = "启用外部代理";
            this.cbEnable_ExternalProxy.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_ExternalProxy_CheckedChanged);
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
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
            this.tlpButton.Size = new System.Drawing.Size(350, 49);
            this.tlpButton.TabIndex = 3;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(103, 6);
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
            this.bExit.Location = new System.Drawing.Point(184, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dEXTProxy
            // 
            this.dEXTProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dEXTProxy.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dEXTProxy.LocalizationText = "EXTProxySettingsForm.EXTProxy";
            this.dEXTProxy.Location = new System.Drawing.Point(2, 2);
            this.dEXTProxy.Margin = new System.Windows.Forms.Padding(2);
            this.dEXTProxy.Name = "dEXTProxy";
            this.dEXTProxy.Orientation = AntdUI.TOrientation.Left;
            this.dEXTProxy.Size = new System.Drawing.Size(346, 19);
            this.dEXTProxy.TabIndex = 4;
            this.dEXTProxy.Text = "外部 SOCKS 代理";
            // 
            // tlpServerInfo
            // 
            this.tlpServerInfo.ColumnCount = 2;
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerInfo.Controls.Add(this.txtExternalProxy_Port, 1, 1);
            this.tlpServerInfo.Controls.Add(this.bExternalProxy_Detection, 0, 0);
            this.tlpServerInfo.Controls.Add(this.txtExternalProxy_PassWord, 1, 6);
            this.tlpServerInfo.Controls.Add(this.txtExternalProxy_UserName, 1, 5);
            this.tlpServerInfo.Controls.Add(this.cbExternalProxy_EnableAuth, 0, 5);
            this.tlpServerInfo.Controls.Add(this.cbExternalProxy_AppointPort, 0, 3);
            this.tlpServerInfo.Controls.Add(this.txtExternalProxy_IP, 1, 0);
            this.tlpServerInfo.Controls.Add(this.txtExternalProxy_AppointPort, 1, 3);
            this.tlpServerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpServerInfo.Location = new System.Drawing.Point(0, 79);
            this.tlpServerInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpServerInfo.Name = "tlpServerInfo";
            this.tlpServerInfo.RowCount = 8;
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpServerInfo.Size = new System.Drawing.Size(350, 486);
            this.tlpServerInfo.TabIndex = 7;
            // 
            // txtExternalProxy_Port
            // 
            this.txtExternalProxy_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExternalProxy_Port.LocalizationPrefixText = "EXTProxySettingsForm.Port";
            this.txtExternalProxy_Port.Location = new System.Drawing.Point(110, 43);
            this.txtExternalProxy_Port.Margin = new System.Windows.Forms.Padding(2);
            this.txtExternalProxy_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtExternalProxy_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtExternalProxy_Port.Name = "txtExternalProxy_Port";
            this.txtExternalProxy_Port.PlaceholderText = "请输入端口号";
            this.txtExternalProxy_Port.PrefixText = "端口号:";
            this.txtExternalProxy_Port.SelectionStart = 1;
            this.txtExternalProxy_Port.Size = new System.Drawing.Size(238, 36);
            this.txtExternalProxy_Port.TabIndex = 25;
            this.txtExternalProxy_Port.Text = "8889";
            this.txtExternalProxy_Port.Value = new decimal(new int[] {
            8889,
            0,
            0,
            0});
            // 
            // bExternalProxy_Detection
            // 
            this.bExternalProxy_Detection.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExternalProxy_Detection.BackExtend = "135, #6253E1, #04BEFE";
            this.bExternalProxy_Detection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExternalProxy_Detection.IconSvg = "CompassOutlined";
            this.bExternalProxy_Detection.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bExternalProxy_Detection.LoadingWaveCount = 6;
            this.bExternalProxy_Detection.LoadingWaveSize = 6;
            this.bExternalProxy_Detection.LoadingWaveValue = 0.6F;
            this.bExternalProxy_Detection.LoadingWaveVertical = true;
            this.bExternalProxy_Detection.LocalizationText = "Detection";
            this.bExternalProxy_Detection.Location = new System.Drawing.Point(2, 2);
            this.bExternalProxy_Detection.Margin = new System.Windows.Forms.Padding(2);
            this.bExternalProxy_Detection.Name = "bExternalProxy_Detection";
            this.bExternalProxy_Detection.Size = new System.Drawing.Size(63, 37);
            this.bExternalProxy_Detection.TabIndex = 24;
            this.bExternalProxy_Detection.Text = "检测";
            this.bExternalProxy_Detection.Type = AntdUI.TTypeMini.Info;
            this.bExternalProxy_Detection.Click += new System.EventHandler(this.bExternalProxy_Detection_Click);
            // 
            // txtExternalProxy_PassWord
            // 
            this.txtExternalProxy_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExternalProxy_PassWord.LocalizationPlaceholderText = "EXTProxySettingsForm.InputPassword";
            this.txtExternalProxy_PassWord.LocalizationPrefixText = "EXTProxySettingsForm.Password";
            this.txtExternalProxy_PassWord.Location = new System.Drawing.Point(110, 195);
            this.txtExternalProxy_PassWord.Margin = new System.Windows.Forms.Padding(2);
            this.txtExternalProxy_PassWord.Name = "txtExternalProxy_PassWord";
            this.txtExternalProxy_PassWord.PlaceholderText = "请输入密码";
            this.txtExternalProxy_PassWord.PrefixText = "密码:";
            this.txtExternalProxy_PassWord.Size = new System.Drawing.Size(238, 36);
            this.txtExternalProxy_PassWord.TabIndex = 23;
            this.txtExternalProxy_PassWord.TextChanged += new System.EventHandler(this.txtExternalProxy_PassWord_TextChanged);
            // 
            // txtExternalProxy_UserName
            // 
            this.txtExternalProxy_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExternalProxy_UserName.LocalizationPlaceholderText = "EXTProxySettingsForm.InputUsername";
            this.txtExternalProxy_UserName.LocalizationPrefixText = "EXTProxySettingsForm.Username";
            this.txtExternalProxy_UserName.Location = new System.Drawing.Point(110, 155);
            this.txtExternalProxy_UserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtExternalProxy_UserName.Name = "txtExternalProxy_UserName";
            this.txtExternalProxy_UserName.PlaceholderText = "请输入账号";
            this.txtExternalProxy_UserName.PrefixText = "账号:";
            this.txtExternalProxy_UserName.Size = new System.Drawing.Size(238, 36);
            this.txtExternalProxy_UserName.TabIndex = 21;
            this.txtExternalProxy_UserName.TextChanged += new System.EventHandler(this.txtExternalProxy_UserName_TextChanged);
            // 
            // cbExternalProxy_EnableAuth
            // 
            this.cbExternalProxy_EnableAuth.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbExternalProxy_EnableAuth.Checked = true;
            this.cbExternalProxy_EnableAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbExternalProxy_EnableAuth.LocalizationText = "EXTProxySettingsForm.RequireAuth";
            this.cbExternalProxy_EnableAuth.Location = new System.Drawing.Point(2, 155);
            this.cbExternalProxy_EnableAuth.Margin = new System.Windows.Forms.Padding(2);
            this.cbExternalProxy_EnableAuth.Name = "cbExternalProxy_EnableAuth";
            this.cbExternalProxy_EnableAuth.Size = new System.Drawing.Size(104, 36);
            this.cbExternalProxy_EnableAuth.TabIndex = 19;
            this.cbExternalProxy_EnableAuth.Text = "外部代理认证";
            this.cbExternalProxy_EnableAuth.CheckedChanged += new AntdUI.BoolEventHandler(this.cbExternalProxy_EnableAuth_CheckedChanged);
            // 
            // cbExternalProxy_AppointPort
            // 
            this.cbExternalProxy_AppointPort.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbExternalProxy_AppointPort.Checked = true;
            this.cbExternalProxy_AppointPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbExternalProxy_AppointPort.LocalizationText = "EXTProxySettingsForm.SpecifyPort";
            this.cbExternalProxy_AppointPort.Location = new System.Drawing.Point(2, 99);
            this.cbExternalProxy_AppointPort.Margin = new System.Windows.Forms.Padding(2);
            this.cbExternalProxy_AppointPort.Name = "cbExternalProxy_AppointPort";
            this.cbExternalProxy_AppointPort.Size = new System.Drawing.Size(80, 36);
            this.cbExternalProxy_AppointPort.TabIndex = 12;
            this.cbExternalProxy_AppointPort.Text = "指定端口";
            this.cbExternalProxy_AppointPort.CheckedChanged += new AntdUI.BoolEventHandler(this.cbExternalProxy_AppointPort_CheckedChanged);
            // 
            // txtExternalProxy_IP
            // 
            this.txtExternalProxy_IP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExternalProxy_IP.LocalizationPlaceholderText = "EXTProxySettingsForm.InputIP";
            this.txtExternalProxy_IP.Location = new System.Drawing.Point(110, 2);
            this.txtExternalProxy_IP.Margin = new System.Windows.Forms.Padding(2);
            this.txtExternalProxy_IP.Name = "txtExternalProxy_IP";
            this.txtExternalProxy_IP.PlaceholderText = "请输入IP或者域名";
            this.txtExternalProxy_IP.PrefixText = "http://";
            this.txtExternalProxy_IP.Size = new System.Drawing.Size(238, 37);
            this.txtExternalProxy_IP.TabIndex = 1;
            this.txtExternalProxy_IP.TextChanged += new System.EventHandler(this.txtExternalProxy_IP_TextChanged);
            // 
            // txtExternalProxy_AppointPort
            // 
            this.txtExternalProxy_AppointPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExternalProxy_AppointPort.LocalizationPlaceholderText = "EXTProxySettingsForm.PortExample";
            this.txtExternalProxy_AppointPort.LocalizationPrefixText = "EXTProxySettingsForm.Port";
            this.txtExternalProxy_AppointPort.Location = new System.Drawing.Point(110, 99);
            this.txtExternalProxy_AppointPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtExternalProxy_AppointPort.Name = "txtExternalProxy_AppointPort";
            this.txtExternalProxy_AppointPort.PlaceholderText = "比如 80,443";
            this.txtExternalProxy_AppointPort.PrefixText = "端口号:";
            this.txtExternalProxy_AppointPort.Size = new System.Drawing.Size(238, 36);
            this.txtExternalProxy_AppointPort.TabIndex = 13;
            this.txtExternalProxy_AppointPort.TextChanged += new System.EventHandler(this.txtExternalProxy_AppointPort_TextChanged);
            // 
            // EXTProxySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpExternalProxy);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EXTProxySetting";
            this.Size = new System.Drawing.Size(350, 700);
            this.Load += new System.EventHandler(this.EXTProxySetting_Load);
            this.tlpExternalProxy.ResumeLayout(false);
            this.tlpExternalProxy.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpServerInfo.ResumeLayout(false);
            this.tlpServerInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpExternalProxy;
        private AntdUI.Checkbox cbEnable_ExternalProxy;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dEXTProxy;
        private TableLayoutPanelEx tlpServerInfo;
        private AntdUI.InputNumber txtExternalProxy_Port;
        private AntdUI.Button bExternalProxy_Detection;
        private AntdUI.Input txtExternalProxy_PassWord;
        private AntdUI.Input txtExternalProxy_UserName;
        private AntdUI.Checkbox cbExternalProxy_EnableAuth;
        private AntdUI.Checkbox cbExternalProxy_AppointPort;
        private AntdUI.Input txtExternalProxy_IP;
        private AntdUI.Input txtExternalProxy_AppointPort;
    }
}

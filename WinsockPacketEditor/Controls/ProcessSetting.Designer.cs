namespace WinsockPacketEditor
{
    partial class ProcessSetting
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
            this.tlpProcessSetting = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lSaveReminder = new AntdUI.Label();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bRefresh = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpLoadDriver = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bUninstallDriver = new AntdUI.Button();
            this.rbWinDivert = new AntdUI.Radio();
            this.rbNFAPI = new AntdUI.Radio();
            this.lSelectDriver = new AntdUI.Label();
            this.rbProxifier = new AntdUI.Radio();
            this.tlpMustTCP = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtMustTCP_AppointPort = new AntdUI.Input();
            this.cbMustTCP_AppointPort = new AntdUI.Checkbox();
            this.txtMustTCP_PassWord = new AntdUI.Input();
            this.txtMustTCP_UserName = new AntdUI.Input();
            this.cbMustTCP_Auth = new AntdUI.Checkbox();
            this.bMustTCP_Detection = new AntdUI.Button();
            this.nudMustTCP_Port = new AntdUI.InputNumber();
            this.txtMustTCP_IP = new AntdUI.Input();
            this.tlpSelectProcess = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tProcessName = new AntdUI.Table();
            this.lProcessName = new AntdUI.Label();
            this.tProcessID = new AntdUI.Table();
            this.lProcessID = new AntdUI.Label();
            this.ttcLoadDriver = new AntdUI.TooltipComponent();
            this.cbMustTCP = new AntdUI.Checkbox();
            this.tlpProcessSetting.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpLoadDriver.SuspendLayout();
            this.tlpMustTCP.SuspendLayout();
            this.tlpSelectProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProcessSetting
            // 
            this.tlpProcessSetting.ColumnCount = 1;
            this.tlpProcessSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessSetting.Controls.Add(this.lSaveReminder, 0, 4);
            this.tlpProcessSetting.Controls.Add(this.tlpButton, 0, 5);
            this.tlpProcessSetting.Controls.Add(this.tlpLoadDriver, 0, 0);
            this.tlpProcessSetting.Controls.Add(this.tlpMustTCP, 0, 1);
            this.tlpProcessSetting.Controls.Add(this.tlpSelectProcess, 0, 2);
            this.tlpProcessSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProcessSetting.Location = new System.Drawing.Point(0, 0);
            this.tlpProcessSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProcessSetting.Name = "tlpProcessSetting";
            this.tlpProcessSetting.RowCount = 6;
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProcessSetting.Size = new System.Drawing.Size(700, 700);
            this.tlpProcessSetting.TabIndex = 0;
            // 
            // lSaveReminder
            // 
            this.lSaveReminder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lSaveReminder.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lSaveReminder.ForeColor = System.Drawing.Color.Red;
            this.lSaveReminder.LocalizationText = "ProcessSetting.Save.Reminder";
            this.lSaveReminder.Location = new System.Drawing.Point(123, 615);
            this.lSaveReminder.Name = "lSaveReminder";
            this.lSaveReminder.Size = new System.Drawing.Size(454, 32);
            this.lSaveReminder.TabIndex = 11;
            this.lSaveReminder.Text = "需要启动 HTTP 代理后，才可以拦截进程的数据\r\n保存时，会断开目标进程已建立的 TCP 连接，若无法拦截数据，请重启目标进程再试!";
            this.lSaveReminder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 7;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bRefresh, 1, 1);
            this.tlpButton.Controls.Add(this.bSave, 3, 1);
            this.tlpButton.Controls.Add(this.bExit, 5, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 650);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 2;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.Size = new System.Drawing.Size(700, 50);
            this.tlpButton.TabIndex = 5;
            // 
            // bRefresh
            // 
            this.bRefresh.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bRefresh.BackExtend = "135, #6253E1, #04BEFE";
            this.bRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRefresh.IconSvg = "RedoOutlined";
            this.bRefresh.LocalizationText = "ProcessSetting.Refresh";
            this.bRefresh.Location = new System.Drawing.Point(224, 10);
            this.bRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(87, 38);
            this.bRefresh.TabIndex = 2;
            this.bRefresh.Text = "刷新进程";
            this.bRefresh.Type = AntdUI.TTypeMini.Primary;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(330, 10);
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
            this.bExit.Location = new System.Drawing.Point(412, 10);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 38);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpLoadDriver
            // 
            this.tlpLoadDriver.ColumnCount = 6;
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoadDriver.Controls.Add(this.bUninstallDriver, 4, 0);
            this.tlpLoadDriver.Controls.Add(this.rbWinDivert, 3, 0);
            this.tlpLoadDriver.Controls.Add(this.rbNFAPI, 1, 0);
            this.tlpLoadDriver.Controls.Add(this.lSelectDriver, 0, 0);
            this.tlpLoadDriver.Controls.Add(this.rbProxifier, 2, 0);
            this.tlpLoadDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLoadDriver.Location = new System.Drawing.Point(0, 0);
            this.tlpLoadDriver.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLoadDriver.Name = "tlpLoadDriver";
            this.tlpLoadDriver.RowCount = 2;
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoadDriver.Size = new System.Drawing.Size(700, 50);
            this.tlpLoadDriver.TabIndex = 6;
            // 
            // bUninstallDriver
            // 
            this.bUninstallDriver.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bUninstallDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bUninstallDriver.IconSvg = "UndoOutlined";
            this.bUninstallDriver.LocalizationText = "ProcessSetting.UninstallDriver";
            this.bUninstallDriver.Location = new System.Drawing.Point(344, 3);
            this.bUninstallDriver.Name = "bUninstallDriver";
            this.bUninstallDriver.Size = new System.Drawing.Size(87, 36);
            this.bUninstallDriver.TabIndex = 16;
            this.bUninstallDriver.Text = "卸载驱动";
            this.bUninstallDriver.Type = AntdUI.TTypeMini.Warn;
            this.bUninstallDriver.Click += new System.EventHandler(this.bUninstallDriver_Click);
            // 
            // rbWinDivert
            // 
            this.rbWinDivert.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbWinDivert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbWinDivert.Location = new System.Drawing.Point(248, 3);
            this.rbWinDivert.Name = "rbWinDivert";
            this.rbWinDivert.Size = new System.Drawing.Size(90, 36);
            this.rbWinDivert.TabIndex = 3;
            this.rbWinDivert.Text = "WinDivert";
            this.ttcLoadDriver.SetTip(this.rbWinDivert, "");
            // 
            // rbNFAPI
            // 
            this.rbNFAPI.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbNFAPI.Checked = true;
            this.rbNFAPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbNFAPI.Location = new System.Drawing.Point(88, 3);
            this.rbNFAPI.Name = "rbNFAPI";
            this.rbNFAPI.Size = new System.Drawing.Size(68, 36);
            this.rbNFAPI.TabIndex = 2;
            this.rbNFAPI.Text = "NFAPI";
            this.ttcLoadDriver.SetTip(this.rbNFAPI, "");
            // 
            // lSelectDriver
            // 
            this.lSelectDriver.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSelectDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSelectDriver.LocalizationText = "ProcessSetting.SelectDriver";
            this.lSelectDriver.Location = new System.Drawing.Point(3, 3);
            this.lSelectDriver.Name = "lSelectDriver";
            this.lSelectDriver.Size = new System.Drawing.Size(79, 36);
            this.lSelectDriver.TabIndex = 0;
            this.lSelectDriver.Text = "选择驱动类型 :";
            // 
            // rbProxifier
            // 
            this.rbProxifier.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbProxifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbProxifier.Location = new System.Drawing.Point(162, 3);
            this.rbProxifier.Name = "rbProxifier";
            this.rbProxifier.Size = new System.Drawing.Size(80, 36);
            this.rbProxifier.TabIndex = 1;
            this.rbProxifier.Text = "Proxifier";
            this.ttcLoadDriver.SetTip(this.rbProxifier, "");
            // 
            // tlpMustTCP
            // 
            this.tlpMustTCP.ColumnCount = 3;
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMustTCP.Controls.Add(this.cbMustTCP, 0, 0);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_AppointPort, 1, 2);
            this.tlpMustTCP.Controls.Add(this.cbMustTCP_AppointPort, 0, 2);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_PassWord, 2, 1);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_UserName, 1, 1);
            this.tlpMustTCP.Controls.Add(this.cbMustTCP_Auth, 0, 1);
            this.tlpMustTCP.Controls.Add(this.bMustTCP_Detection, 2, 2);
            this.tlpMustTCP.Controls.Add(this.nudMustTCP_Port, 2, 0);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_IP, 1, 0);
            this.tlpMustTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMustTCP.Location = new System.Drawing.Point(0, 50);
            this.tlpMustTCP.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMustTCP.Name = "tlpMustTCP";
            this.tlpMustTCP.RowCount = 4;
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMustTCP.Size = new System.Drawing.Size(700, 130);
            this.tlpMustTCP.TabIndex = 8;
            // 
            // txtMustTCP_AppointPort
            // 
            this.txtMustTCP_AppointPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_AppointPort.LocalizationPlaceholderText = "EXTProxySettingsForm.PortExample";
            this.txtMustTCP_AppointPort.LocalizationPrefixText = "EXTProxySettingsForm.Port";
            this.txtMustTCP_AppointPort.Location = new System.Drawing.Point(105, 83);
            this.txtMustTCP_AppointPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_AppointPort.Name = "txtMustTCP_AppointPort";
            this.txtMustTCP_AppointPort.PlaceholderText = "比如 80,443";
            this.txtMustTCP_AppointPort.PrefixText = "端口 :";
            this.txtMustTCP_AppointPort.Size = new System.Drawing.Size(294, 37);
            this.txtMustTCP_AppointPort.TabIndex = 34;
            // 
            // cbMustTCP_AppointPort
            // 
            this.cbMustTCP_AppointPort.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbMustTCP_AppointPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMustTCP_AppointPort.LocalizationText = "EXTProxySettingsForm.SpecifyPort";
            this.cbMustTCP_AppointPort.Location = new System.Drawing.Point(2, 83);
            this.cbMustTCP_AppointPort.Margin = new System.Windows.Forms.Padding(2);
            this.cbMustTCP_AppointPort.Name = "cbMustTCP_AppointPort";
            this.cbMustTCP_AppointPort.Size = new System.Drawing.Size(87, 37);
            this.cbMustTCP_AppointPort.TabIndex = 33;
            this.cbMustTCP_AppointPort.Text = "指定端口 :";
            this.cbMustTCP_AppointPort.CheckedChanged += new AntdUI.BoolEventHandler(this.cbMustTCP_AppointPort_CheckedChanged);
            // 
            // txtMustTCP_PassWord
            // 
            this.txtMustTCP_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_PassWord.LocalizationPlaceholderText = "EXTProxySettingsForm.InputPassword";
            this.txtMustTCP_PassWord.LocalizationPrefixText = "EXTProxySettingsForm.Password";
            this.txtMustTCP_PassWord.Location = new System.Drawing.Point(403, 43);
            this.txtMustTCP_PassWord.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_PassWord.Name = "txtMustTCP_PassWord";
            this.txtMustTCP_PassWord.PlaceholderText = "请输入密码";
            this.txtMustTCP_PassWord.PrefixText = "密码 :";
            this.txtMustTCP_PassWord.Size = new System.Drawing.Size(295, 36);
            this.txtMustTCP_PassWord.TabIndex = 31;
            // 
            // txtMustTCP_UserName
            // 
            this.txtMustTCP_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_UserName.LocalizationPlaceholderText = "EXTProxySettingsForm.InputUsername";
            this.txtMustTCP_UserName.LocalizationPrefixText = "EXTProxySettingsForm.Username";
            this.txtMustTCP_UserName.Location = new System.Drawing.Point(105, 43);
            this.txtMustTCP_UserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_UserName.Name = "txtMustTCP_UserName";
            this.txtMustTCP_UserName.PlaceholderText = "请输入账号";
            this.txtMustTCP_UserName.PrefixText = "账号 :";
            this.txtMustTCP_UserName.Size = new System.Drawing.Size(294, 36);
            this.txtMustTCP_UserName.TabIndex = 30;
            // 
            // cbMustTCP_Auth
            // 
            this.cbMustTCP_Auth.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbMustTCP_Auth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMustTCP_Auth.LocalizationText = "ProcessSetting.RequireAuth";
            this.cbMustTCP_Auth.Location = new System.Drawing.Point(2, 43);
            this.cbMustTCP_Auth.Margin = new System.Windows.Forms.Padding(2);
            this.cbMustTCP_Auth.Name = "cbMustTCP_Auth";
            this.cbMustTCP_Auth.Size = new System.Drawing.Size(87, 36);
            this.cbMustTCP_Auth.TabIndex = 29;
            this.cbMustTCP_Auth.Text = "需要认证 :";
            this.cbMustTCP_Auth.CheckedChanged += new AntdUI.BoolEventHandler(this.cbMustTCP_Auth_CheckedChanged);
            // 
            // bMustTCP_Detection
            // 
            this.bMustTCP_Detection.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bMustTCP_Detection.BackExtend = "135, #6253E1, #04BEFE";
            this.bMustTCP_Detection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bMustTCP_Detection.IconSvg = "CompassOutlined";
            this.bMustTCP_Detection.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bMustTCP_Detection.LoadingWaveCount = 6;
            this.bMustTCP_Detection.LoadingWaveSize = 6;
            this.bMustTCP_Detection.LoadingWaveValue = 0.6F;
            this.bMustTCP_Detection.LoadingWaveVertical = true;
            this.bMustTCP_Detection.LocalizationText = "Detection";
            this.bMustTCP_Detection.Location = new System.Drawing.Point(403, 83);
            this.bMustTCP_Detection.Margin = new System.Windows.Forms.Padding(2);
            this.bMustTCP_Detection.Name = "bMustTCP_Detection";
            this.bMustTCP_Detection.Size = new System.Drawing.Size(87, 37);
            this.bMustTCP_Detection.TabIndex = 27;
            this.bMustTCP_Detection.Text = "检测代理";
            this.bMustTCP_Detection.Type = AntdUI.TTypeMini.Info;
            this.bMustTCP_Detection.Click += new System.EventHandler(this.bMustTCP_Detection_Click);
            // 
            // nudMustTCP_Port
            // 
            this.nudMustTCP_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMustTCP_Port.LocalizationPrefixText = "EXTProxySettingsForm.Port";
            this.nudMustTCP_Port.Location = new System.Drawing.Point(403, 2);
            this.nudMustTCP_Port.Margin = new System.Windows.Forms.Padding(2);
            this.nudMustTCP_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudMustTCP_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMustTCP_Port.Name = "nudMustTCP_Port";
            this.nudMustTCP_Port.PlaceholderText = "请输入端口号";
            this.nudMustTCP_Port.PrefixText = "端口 :";
            this.nudMustTCP_Port.SelectionStart = 1;
            this.nudMustTCP_Port.Size = new System.Drawing.Size(295, 37);
            this.nudMustTCP_Port.TabIndex = 26;
            this.nudMustTCP_Port.Text = "1080";
            this.nudMustTCP_Port.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // txtMustTCP_IP
            // 
            this.txtMustTCP_IP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_IP.LocalizationPlaceholderText = "EXTProxySettingsForm.InputIP";
            this.txtMustTCP_IP.Location = new System.Drawing.Point(105, 2);
            this.txtMustTCP_IP.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_IP.Name = "txtMustTCP_IP";
            this.txtMustTCP_IP.PlaceholderText = "请输入IP地址";
            this.txtMustTCP_IP.PrefixText = "Socket5: //";
            this.txtMustTCP_IP.Size = new System.Drawing.Size(294, 37);
            this.txtMustTCP_IP.TabIndex = 2;
            // 
            // tlpSelectProcess
            // 
            this.tlpSelectProcess.ColumnCount = 2;
            this.tlpSelectProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSelectProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSelectProcess.Controls.Add(this.tProcessName, 1, 1);
            this.tlpSelectProcess.Controls.Add(this.lProcessName, 1, 0);
            this.tlpSelectProcess.Controls.Add(this.tProcessID, 0, 1);
            this.tlpSelectProcess.Controls.Add(this.lProcessID, 0, 0);
            this.tlpSelectProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelectProcess.Location = new System.Drawing.Point(0, 180);
            this.tlpSelectProcess.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSelectProcess.Name = "tlpSelectProcess";
            this.tlpSelectProcess.RowCount = 2;
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectProcess.Size = new System.Drawing.Size(700, 417);
            this.tlpSelectProcess.TabIndex = 12;
            // 
            // tProcessName
            // 
            this.tProcessName.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProcessName.CellImpactHeight = false;
            this.tProcessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProcessName.EmptyHeader = true;
            this.tProcessName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tProcessName.Gap = 8;
            this.tProcessName.GapCell = 0;
            this.tProcessName.Gaps = new System.Drawing.Size(8, 8);
            this.tProcessName.Location = new System.Drawing.Point(352, 31);
            this.tProcessName.Margin = new System.Windows.Forms.Padding(2);
            this.tProcessName.Name = "tProcessName";
            this.tProcessName.Radius = 6;
            this.tProcessName.Size = new System.Drawing.Size(346, 384);
            this.tProcessName.TabIndex = 16;
            this.tProcessName.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tProcessName_CellDoubleClick);
            // 
            // lProcessName
            // 
            this.lProcessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProcessName.LocalizationText = "ProcessSetting.ProcessName";
            this.lProcessName.Location = new System.Drawing.Point(353, 3);
            this.lProcessName.Name = "lProcessName";
            this.lProcessName.Size = new System.Drawing.Size(344, 23);
            this.lProcessName.TabIndex = 15;
            this.lProcessName.Text = "按 [ 进程名称 ] 拦截 ( 双击可删除名称 )";
            // 
            // tProcessID
            // 
            this.tProcessID.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProcessID.CellImpactHeight = false;
            this.tProcessID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProcessID.EmptyHeader = true;
            this.tProcessID.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tProcessID.Gap = 8;
            this.tProcessID.GapCell = 0;
            this.tProcessID.Gaps = new System.Drawing.Size(8, 8);
            this.tProcessID.Location = new System.Drawing.Point(2, 31);
            this.tProcessID.Margin = new System.Windows.Forms.Padding(2);
            this.tProcessID.Name = "tProcessID";
            this.tProcessID.Radius = 6;
            this.tProcessID.Size = new System.Drawing.Size(346, 384);
            this.tProcessID.TabIndex = 13;
            this.tProcessID.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tProcessID_CellDoubleClick);
            // 
            // lProcessID
            // 
            this.lProcessID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProcessID.LocalizationText = "ProcessSetting.ProcessID";
            this.lProcessID.Location = new System.Drawing.Point(3, 3);
            this.lProcessID.Name = "lProcessID";
            this.lProcessID.Size = new System.Drawing.Size(344, 23);
            this.lProcessID.TabIndex = 14;
            this.lProcessID.Text = "按 [ 进程编号 ] 拦截 ( 双击添加到名称 )";
            // 
            // cbMustTCP
            // 
            this.cbMustTCP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbMustTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMustTCP.LocalizationText = "ProcessSetting.MustTCP";
            this.cbMustTCP.Location = new System.Drawing.Point(2, 2);
            this.cbMustTCP.Margin = new System.Windows.Forms.Padding(2);
            this.cbMustTCP.Name = "cbMustTCP";
            this.cbMustTCP.Size = new System.Drawing.Size(99, 37);
            this.cbMustTCP.TabIndex = 35;
            this.cbMustTCP.Text = "强制转代理 :";
            this.cbMustTCP.CheckedChanged += new AntdUI.BoolEventHandler(this.cbMustTCP_CheckedChanged);
            // 
            // ProcessSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpProcessSetting);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ProcessSetting";
            this.Size = new System.Drawing.Size(700, 700);
            this.Load += new System.EventHandler(this.ProcessSetting_Load);
            this.tlpProcessSetting.ResumeLayout(false);
            this.tlpProcessSetting.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpLoadDriver.ResumeLayout(false);
            this.tlpLoadDriver.PerformLayout();
            this.tlpMustTCP.ResumeLayout(false);
            this.tlpMustTCP.PerformLayout();
            this.tlpSelectProcess.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpProcessSetting;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpLoadDriver;
        private AntdUI.Label lSelectDriver;
        private AntdUI.Radio rbProxifier;
        private AntdUI.Radio rbWinDivert;
        private AntdUI.Radio rbNFAPI;
        private AntdUI.Button bUninstallDriver;
        private TableLayoutPanelEx tlpMustTCP;
        private AntdUI.Input txtMustTCP_IP;
        private AntdUI.InputNumber nudMustTCP_Port;
        private AntdUI.Button bMustTCP_Detection;
        private AntdUI.Checkbox cbMustTCP_Auth;
        private AntdUI.Input txtMustTCP_UserName;
        private AntdUI.Input txtMustTCP_PassWord;
        private AntdUI.Button bRefresh;
        private AntdUI.Checkbox cbMustTCP_AppointPort;
        private AntdUI.Input txtMustTCP_AppointPort;
        private AntdUI.TooltipComponent ttcLoadDriver;
        private AntdUI.Label lSaveReminder;
        private TableLayoutPanelEx tlpSelectProcess;
        private AntdUI.Table tProcessID;
        private AntdUI.Label lProcessName;
        private AntdUI.Label lProcessID;
        private AntdUI.Table tProcessName;
        private AntdUI.Checkbox cbMustTCP;
    }
}

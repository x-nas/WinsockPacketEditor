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
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bRefresh = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpLoadDriver = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bUninstallDriver = new AntdUI.Button();
            this.rbWinDivert = new AntdUI.Radio();
            this.rbNFAPI = new AntdUI.Radio();
            this.label1 = new AntdUI.Label();
            this.rbProxifier = new AntdUI.Radio();
            this.transferProcessList = new AntdUI.Transfer();
            this.tlpMustTCP = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtMustTCP_PassWord = new AntdUI.Input();
            this.txtMustTCP_UserName = new AntdUI.Input();
            this.cbMustTCP_Auth = new AntdUI.Checkbox();
            this.bExternalProxy_Detection = new AntdUI.Button();
            this.txtMustTCP_Port = new AntdUI.InputNumber();
            this.txtMustTCP_IP = new AntdUI.Input();
            this.lMustTCP = new AntdUI.Label();
            this.tlpProcessSetting.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpLoadDriver.SuspendLayout();
            this.tlpMustTCP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProcessSetting
            // 
            this.tlpProcessSetting.ColumnCount = 1;
            this.tlpProcessSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessSetting.Controls.Add(this.tlpButton, 0, 3);
            this.tlpProcessSetting.Controls.Add(this.tlpLoadDriver, 0, 0);
            this.tlpProcessSetting.Controls.Add(this.transferProcessList, 0, 2);
            this.tlpProcessSetting.Controls.Add(this.tlpMustTCP, 0, 1);
            this.tlpProcessSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProcessSetting.Location = new System.Drawing.Point(0, 0);
            this.tlpProcessSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProcessSetting.Name = "tlpProcessSetting";
            this.tlpProcessSetting.RowCount = 4;
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProcessSetting.Size = new System.Drawing.Size(700, 700);
            this.tlpProcessSetting.TabIndex = 0;
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
            this.bRefresh.LocalizationText = "Save";
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
            this.tlpLoadDriver.Controls.Add(this.label1, 0, 0);
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
            this.bUninstallDriver.Location = new System.Drawing.Point(323, 3);
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
            this.rbWinDivert.Location = new System.Drawing.Point(227, 3);
            this.rbWinDivert.Name = "rbWinDivert";
            this.rbWinDivert.Size = new System.Drawing.Size(90, 36);
            this.rbWinDivert.TabIndex = 3;
            this.rbWinDivert.Text = "WinDivert";
            // 
            // rbNFAPI
            // 
            this.rbNFAPI.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbNFAPI.Checked = true;
            this.rbNFAPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbNFAPI.Location = new System.Drawing.Point(64, 3);
            this.rbNFAPI.Name = "rbNFAPI";
            this.rbNFAPI.Size = new System.Drawing.Size(71, 36);
            this.rbNFAPI.TabIndex = 2;
            this.rbNFAPI.Text = "NF API";
            // 
            // label1
            // 
            this.label1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "驱动类型 :";
            // 
            // rbProxifier
            // 
            this.rbProxifier.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbProxifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbProxifier.Location = new System.Drawing.Point(141, 3);
            this.rbProxifier.Name = "rbProxifier";
            this.rbProxifier.Size = new System.Drawing.Size(80, 36);
            this.rbProxifier.TabIndex = 1;
            this.rbProxifier.Text = "Proxifier";
            // 
            // transferProcessList
            // 
            this.transferProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transferProcessList.Location = new System.Drawing.Point(3, 153);
            this.transferProcessList.Name = "transferProcessList";
            this.transferProcessList.Size = new System.Drawing.Size(694, 494);
            this.transferProcessList.TabIndex = 7;
            this.transferProcessList.Text = "transfer1";
            // 
            // tlpMustTCP
            // 
            this.tlpMustTCP.ColumnCount = 4;
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMustTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_PassWord, 2, 1);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_UserName, 1, 1);
            this.tlpMustTCP.Controls.Add(this.cbMustTCP_Auth, 0, 1);
            this.tlpMustTCP.Controls.Add(this.bExternalProxy_Detection, 3, 0);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_Port, 2, 0);
            this.tlpMustTCP.Controls.Add(this.txtMustTCP_IP, 1, 0);
            this.tlpMustTCP.Controls.Add(this.lMustTCP, 0, 0);
            this.tlpMustTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMustTCP.Location = new System.Drawing.Point(0, 50);
            this.tlpMustTCP.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMustTCP.Name = "tlpMustTCP";
            this.tlpMustTCP.RowCount = 3;
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMustTCP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMustTCP.Size = new System.Drawing.Size(700, 100);
            this.tlpMustTCP.TabIndex = 8;
            // 
            // txtMustTCP_PassWord
            // 
            this.txtMustTCP_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_PassWord.LocalizationPlaceholderText = "EXTProxySettingsForm.InputPassword";
            this.txtMustTCP_PassWord.LocalizationPrefixText = "EXTProxySettingsForm.Password";
            this.txtMustTCP_PassWord.Location = new System.Drawing.Point(364, 43);
            this.txtMustTCP_PassWord.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_PassWord.Name = "txtMustTCP_PassWord";
            this.txtMustTCP_PassWord.PlaceholderText = "请输入密码";
            this.txtMustTCP_PassWord.PrefixText = "密码 :";
            this.txtMustTCP_PassWord.Size = new System.Drawing.Size(267, 36);
            this.txtMustTCP_PassWord.TabIndex = 31;
            // 
            // txtMustTCP_UserName
            // 
            this.txtMustTCP_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_UserName.LocalizationPlaceholderText = "EXTProxySettingsForm.InputUsername";
            this.txtMustTCP_UserName.LocalizationPrefixText = "EXTProxySettingsForm.Username";
            this.txtMustTCP_UserName.Location = new System.Drawing.Point(93, 43);
            this.txtMustTCP_UserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_UserName.Name = "txtMustTCP_UserName";
            this.txtMustTCP_UserName.PlaceholderText = "请输入账号";
            this.txtMustTCP_UserName.PrefixText = "账号 :";
            this.txtMustTCP_UserName.Size = new System.Drawing.Size(267, 36);
            this.txtMustTCP_UserName.TabIndex = 30;
            // 
            // cbMustTCP_Auth
            // 
            this.cbMustTCP_Auth.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbMustTCP_Auth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMustTCP_Auth.LocalizationText = "EXTProxySettingsForm.RequireAuth";
            this.cbMustTCP_Auth.Location = new System.Drawing.Point(2, 43);
            this.cbMustTCP_Auth.Margin = new System.Windows.Forms.Padding(2);
            this.cbMustTCP_Auth.Name = "cbMustTCP_Auth";
            this.cbMustTCP_Auth.Size = new System.Drawing.Size(87, 36);
            this.cbMustTCP_Auth.TabIndex = 29;
            this.cbMustTCP_Auth.Text = "需要认证 :";
            this.cbMustTCP_Auth.CheckedChanged += new AntdUI.BoolEventHandler(this.cbMustTCP_Auth_CheckedChanged);
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
            this.bExternalProxy_Detection.Location = new System.Drawing.Point(635, 2);
            this.bExternalProxy_Detection.Margin = new System.Windows.Forms.Padding(2);
            this.bExternalProxy_Detection.Name = "bExternalProxy_Detection";
            this.bExternalProxy_Detection.Size = new System.Drawing.Size(63, 37);
            this.bExternalProxy_Detection.TabIndex = 27;
            this.bExternalProxy_Detection.Text = "检测";
            this.bExternalProxy_Detection.Type = AntdUI.TTypeMini.Info;
            // 
            // txtMustTCP_Port
            // 
            this.txtMustTCP_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_Port.LocalizationPrefixText = "EXTProxySettingsForm.Port";
            this.txtMustTCP_Port.Location = new System.Drawing.Point(364, 2);
            this.txtMustTCP_Port.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtMustTCP_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMustTCP_Port.Name = "txtMustTCP_Port";
            this.txtMustTCP_Port.PlaceholderText = "请输入端口号";
            this.txtMustTCP_Port.PrefixText = "端口 :";
            this.txtMustTCP_Port.SelectionStart = 1;
            this.txtMustTCP_Port.Size = new System.Drawing.Size(267, 37);
            this.txtMustTCP_Port.TabIndex = 26;
            this.txtMustTCP_Port.Text = "1080";
            this.txtMustTCP_Port.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // txtMustTCP_IP
            // 
            this.txtMustTCP_IP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMustTCP_IP.LocalizationPlaceholderText = "EXTProxySettingsForm.InputIP";
            this.txtMustTCP_IP.Location = new System.Drawing.Point(93, 2);
            this.txtMustTCP_IP.Margin = new System.Windows.Forms.Padding(2);
            this.txtMustTCP_IP.Name = "txtMustTCP_IP";
            this.txtMustTCP_IP.PlaceholderText = "请输入IP地址";
            this.txtMustTCP_IP.PrefixText = "Socket5: //";
            this.txtMustTCP_IP.Size = new System.Drawing.Size(267, 37);
            this.txtMustTCP_IP.TabIndex = 2;
            // 
            // lMustTCP
            // 
            this.lMustTCP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lMustTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMustTCP.Location = new System.Drawing.Point(3, 3);
            this.lMustTCP.Name = "lMustTCP";
            this.lMustTCP.Size = new System.Drawing.Size(67, 35);
            this.lMustTCP.TabIndex = 0;
            this.lMustTCP.Text = "强制转代理 :";
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
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpLoadDriver.ResumeLayout(false);
            this.tlpLoadDriver.PerformLayout();
            this.tlpMustTCP.ResumeLayout(false);
            this.tlpMustTCP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpProcessSetting;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpLoadDriver;
        private AntdUI.Label label1;
        private AntdUI.Radio rbProxifier;
        private AntdUI.Radio rbWinDivert;
        private AntdUI.Radio rbNFAPI;
        private AntdUI.Transfer transferProcessList;
        private AntdUI.Button bUninstallDriver;
        private TableLayoutPanelEx tlpMustTCP;
        private AntdUI.Label lMustTCP;
        private AntdUI.Input txtMustTCP_IP;
        private AntdUI.InputNumber txtMustTCP_Port;
        private AntdUI.Button bExternalProxy_Detection;
        private AntdUI.Checkbox cbMustTCP_Auth;
        private AntdUI.Input txtMustTCP_UserName;
        private AntdUI.Input txtMustTCP_PassWord;
        private AntdUI.Button bRefresh;
    }
}

namespace WinsockPacketEditor
{
    partial class LeachSetting
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
            this.tlpFLeachSettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpContent = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtCheckData = new AntdUI.Input();
            this.cbCheckData = new AntdUI.Checkbox();
            this.txtCheckHead = new AntdUI.Input();
            this.cbCheckHead = new AntdUI.Checkbox();
            this.txtCheckPort = new AntdUI.Input();
            this.cbCheckPort = new AntdUI.Checkbox();
            this.txtCheckIP = new AntdUI.Input();
            this.cbCheckIP = new AntdUI.Checkbox();
            this.txtCheckLen = new AntdUI.Input();
            this.cbCheckLen = new AntdUI.Checkbox();
            this.cbCheckSocket = new AntdUI.Checkbox();
            this.txtCheckSocket = new AntdUI.Input();
            this.lIsShow = new AntdUI.Label();
            this.sIsShow = new AntdUI.Switch();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpFLeachSettings.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFLeachSettings
            // 
            this.tlpFLeachSettings.ColumnCount = 1;
            this.tlpFLeachSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFLeachSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpFLeachSettings.Controls.Add(this.tlpContent, 0, 0);
            this.tlpFLeachSettings.Controls.Add(this.tlpButton, 0, 1);
            this.tlpFLeachSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFLeachSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpFLeachSettings.Margin = new System.Windows.Forms.Padding(2);
            this.tlpFLeachSettings.Name = "tlpFLeachSettings";
            this.tlpFLeachSettings.RowCount = 2;
            this.tlpFLeachSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFLeachSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpFLeachSettings.Size = new System.Drawing.Size(350, 700);
            this.tlpFLeachSettings.TabIndex = 2;
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Controls.Add(this.txtCheckData, 1, 7);
            this.tlpContent.Controls.Add(this.cbCheckData, 0, 7);
            this.tlpContent.Controls.Add(this.txtCheckHead, 1, 6);
            this.tlpContent.Controls.Add(this.cbCheckHead, 0, 6);
            this.tlpContent.Controls.Add(this.txtCheckPort, 1, 5);
            this.tlpContent.Controls.Add(this.cbCheckPort, 0, 5);
            this.tlpContent.Controls.Add(this.txtCheckIP, 1, 4);
            this.tlpContent.Controls.Add(this.cbCheckIP, 0, 4);
            this.tlpContent.Controls.Add(this.txtCheckLen, 1, 3);
            this.tlpContent.Controls.Add(this.cbCheckLen, 0, 3);
            this.tlpContent.Controls.Add(this.cbCheckSocket, 0, 2);
            this.tlpContent.Controls.Add(this.txtCheckSocket, 1, 2);
            this.tlpContent.Controls.Add(this.lIsShow, 0, 0);
            this.tlpContent.Controls.Add(this.sIsShow, 1, 0);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(0, 0);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 9;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Size = new System.Drawing.Size(350, 651);
            this.tlpContent.TabIndex = 1;
            // 
            // txtCheckData
            // 
            this.txtCheckData.AllowClear = true;
            this.txtCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckData.LocalizationPlaceholderText = "HEXSemicolonDelimiter";
            this.txtCheckData.Location = new System.Drawing.Point(86, 248);
            this.txtCheckData.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckData.Name = "txtCheckData";
            this.txtCheckData.PlaceholderText = "十六进制带空格，支持 ; 分隔符";
            this.txtCheckData.Size = new System.Drawing.Size(262, 36);
            this.txtCheckData.TabIndex = 11;
            this.txtCheckData.TextChanged += new System.EventHandler(this.txtCheckData_TextChanged);
            // 
            // cbCheckData
            // 
            this.cbCheckData.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckData.LocalizationText = "PacketContent";
            this.cbCheckData.Location = new System.Drawing.Point(2, 248);
            this.cbCheckData.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckData.Name = "cbCheckData";
            this.cbCheckData.Size = new System.Drawing.Size(80, 32);
            this.cbCheckData.TabIndex = 10;
            this.cbCheckData.Text = "指定内容";
            this.cbCheckData.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckData_CheckedChanged);
            // 
            // txtCheckHead
            // 
            this.txtCheckHead.AllowClear = true;
            this.txtCheckHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckHead.LocalizationPlaceholderText = "HEXSemicolonDelimiter";
            this.txtCheckHead.Location = new System.Drawing.Point(86, 208);
            this.txtCheckHead.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckHead.Name = "txtCheckHead";
            this.txtCheckHead.PlaceholderText = "十六进制带空格，支持 ; 分隔符";
            this.txtCheckHead.Size = new System.Drawing.Size(262, 36);
            this.txtCheckHead.TabIndex = 9;
            this.txtCheckHead.TextChanged += new System.EventHandler(this.txtCheckHead_TextChanged);
            // 
            // cbCheckHead
            // 
            this.cbCheckHead.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckHead.LocalizationText = "PacketHead";
            this.cbCheckHead.Location = new System.Drawing.Point(2, 208);
            this.cbCheckHead.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckHead.Name = "cbCheckHead";
            this.cbCheckHead.Size = new System.Drawing.Size(80, 32);
            this.cbCheckHead.TabIndex = 8;
            this.cbCheckHead.Text = "指定包头";
            this.cbCheckHead.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckHead_CheckedChanged);
            // 
            // txtCheckPort
            // 
            this.txtCheckPort.AllowClear = true;
            this.txtCheckPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckPort.LocalizationPlaceholderText = "SemicolonDelimiter";
            this.txtCheckPort.Location = new System.Drawing.Point(86, 168);
            this.txtCheckPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckPort.Name = "txtCheckPort";
            this.txtCheckPort.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckPort.Size = new System.Drawing.Size(262, 36);
            this.txtCheckPort.TabIndex = 7;
            this.txtCheckPort.TextChanged += new System.EventHandler(this.txtCheckPort_TextChanged);
            // 
            // cbCheckPort
            // 
            this.cbCheckPort.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckPort.LocalizationText = "Port";
            this.cbCheckPort.Location = new System.Drawing.Point(2, 168);
            this.cbCheckPort.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckPort.Name = "cbCheckPort";
            this.cbCheckPort.Size = new System.Drawing.Size(68, 32);
            this.cbCheckPort.TabIndex = 6;
            this.cbCheckPort.Text = "端口号";
            this.cbCheckPort.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckPort_CheckedChanged);
            // 
            // txtCheckIP
            // 
            this.txtCheckIP.AllowClear = true;
            this.txtCheckIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckIP.LocalizationPlaceholderText = "SemicolonDelimiter";
            this.txtCheckIP.Location = new System.Drawing.Point(86, 128);
            this.txtCheckIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckIP.Name = "txtCheckIP";
            this.txtCheckIP.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckIP.Size = new System.Drawing.Size(262, 36);
            this.txtCheckIP.TabIndex = 5;
            this.txtCheckIP.TextChanged += new System.EventHandler(this.txtCheckIP_TextChanged);
            // 
            // cbCheckIP
            // 
            this.cbCheckIP.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckIP.LocalizationText = "IPAddress";
            this.cbCheckIP.Location = new System.Drawing.Point(2, 128);
            this.cbCheckIP.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckIP.Name = "cbCheckIP";
            this.cbCheckIP.Size = new System.Drawing.Size(67, 32);
            this.cbCheckIP.TabIndex = 4;
            this.cbCheckIP.Text = "IP地址";
            this.cbCheckIP.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckIP_CheckedChanged);
            // 
            // txtCheckLen
            // 
            this.txtCheckLen.AllowClear = true;
            this.txtCheckLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckLen.LocalizationPlaceholderText = "LeachSetting.Length";
            this.txtCheckLen.Location = new System.Drawing.Point(86, 88);
            this.txtCheckLen.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckLen.Name = "txtCheckLen";
            this.txtCheckLen.PlaceholderText = "例如：0-99;100";
            this.txtCheckLen.Size = new System.Drawing.Size(262, 36);
            this.txtCheckLen.TabIndex = 3;
            this.txtCheckLen.TextChanged += new System.EventHandler(this.txtCheckLen_TextChanged);
            // 
            // cbCheckLen
            // 
            this.cbCheckLen.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckLen.LocalizationText = "Length";
            this.cbCheckLen.Location = new System.Drawing.Point(2, 88);
            this.cbCheckLen.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckLen.Name = "cbCheckLen";
            this.cbCheckLen.Size = new System.Drawing.Size(56, 32);
            this.cbCheckLen.TabIndex = 2;
            this.cbCheckLen.Text = "长度";
            this.cbCheckLen.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckLen_CheckedChanged);
            // 
            // cbCheckSocket
            // 
            this.cbCheckSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckSocket.LocalizationText = "Socket";
            this.cbCheckSocket.Location = new System.Drawing.Point(2, 48);
            this.cbCheckSocket.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckSocket.Name = "cbCheckSocket";
            this.cbCheckSocket.Size = new System.Drawing.Size(68, 32);
            this.cbCheckSocket.TabIndex = 0;
            this.cbCheckSocket.Text = "套接字";
            this.cbCheckSocket.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckSocket_CheckedChanged);
            // 
            // txtCheckSocket
            // 
            this.txtCheckSocket.AllowClear = true;
            this.txtCheckSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckSocket.LocalizationPlaceholderText = "SemicolonDelimiter";
            this.txtCheckSocket.Location = new System.Drawing.Point(86, 48);
            this.txtCheckSocket.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckSocket.Name = "txtCheckSocket";
            this.txtCheckSocket.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckSocket.Size = new System.Drawing.Size(262, 36);
            this.txtCheckSocket.TabIndex = 12;
            this.txtCheckSocket.TextChanged += new System.EventHandler(this.txtCheckSocket_TextChanged);
            // 
            // lIsShow
            // 
            this.lIsShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lIsShow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lIsShow.LocalizationText = "LeachSetting.IsShow";
            this.lIsShow.Location = new System.Drawing.Point(2, 2);
            this.lIsShow.Margin = new System.Windows.Forms.Padding(2);
            this.lIsShow.Name = "lIsShow";
            this.lIsShow.Size = new System.Drawing.Size(80, 26);
            this.lIsShow.TabIndex = 13;
            this.lIsShow.Text = "是否显示";
            this.lIsShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sIsShow
            // 
            this.sIsShow.Checked = true;
            this.sIsShow.CheckedText = "是";
            this.sIsShow.LocalizationCheckedText = "Yes";
            this.sIsShow.LocalizationUnCheckedText = "No";
            this.sIsShow.Location = new System.Drawing.Point(86, 2);
            this.sIsShow.Margin = new System.Windows.Forms.Padding(2);
            this.sIsShow.Name = "sIsShow";
            this.sIsShow.Size = new System.Drawing.Size(49, 26);
            this.sIsShow.TabIndex = 14;
            this.sIsShow.UnCheckedText = "否";
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
            this.tlpButton.Size = new System.Drawing.Size(350, 49);
            this.tlpButton.TabIndex = 2;
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
            // LeachSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFLeachSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LeachSetting";
            this.Size = new System.Drawing.Size(350, 700);
            this.Load += new System.EventHandler(this.LeachSetting_Load);
            this.tlpFLeachSettings.ResumeLayout(false);
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFLeachSettings;
        private TableLayoutPanelEx tlpContent;
        private AntdUI.Input txtCheckData;
        private AntdUI.Checkbox cbCheckData;
        private AntdUI.Input txtCheckHead;
        private AntdUI.Checkbox cbCheckHead;
        private AntdUI.Input txtCheckPort;
        private AntdUI.Checkbox cbCheckPort;
        private AntdUI.Input txtCheckIP;
        private AntdUI.Checkbox cbCheckIP;
        private AntdUI.Input txtCheckLen;
        private AntdUI.Checkbox cbCheckLen;
        private AntdUI.Checkbox cbCheckSocket;
        private AntdUI.Input txtCheckSocket;
        private AntdUI.Label lIsShow;
        private AntdUI.Switch sIsShow;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
    }
}

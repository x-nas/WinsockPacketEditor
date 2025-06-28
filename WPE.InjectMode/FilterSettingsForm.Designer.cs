namespace WPE.InjectMode
{
    partial class FilterSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterSettingsForm));
            this.bExit = new AntdUI.Button();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
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
            this.tlpFilterSettings = new System.Windows.Forms.TableLayoutPanel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.lIsShow = new AntdUI.Label();
            this.sIsShow = new AntdUI.Switch();
            this.tlpButton.SuspendLayout();
            this.tlpFilterSettings.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(255, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
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
            this.tlpButton.Location = new System.Drawing.Point(0, 401);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 2;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.Location = new System.Drawing.Point(115, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // txtCheckData
            // 
            this.txtCheckData.AllowClear = true;
            this.txtCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckData.Location = new System.Drawing.Point(115, 301);
            this.txtCheckData.Name = "txtCheckData";
            this.txtCheckData.PlaceholderText = "十六进制带空格，支持 ; 分隔符";
            this.txtCheckData.Size = new System.Drawing.Size(366, 42);
            this.txtCheckData.TabIndex = 11;
            this.txtCheckData.TextChanged += new System.EventHandler(this.txtCheckData_TextChanged);
            // 
            // cbCheckData
            // 
            this.cbCheckData.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckData.Location = new System.Drawing.Point(3, 301);
            this.cbCheckData.Name = "cbCheckData";
            this.cbCheckData.Size = new System.Drawing.Size(106, 42);
            this.cbCheckData.TabIndex = 10;
            this.cbCheckData.Text = "指定内容";
            this.cbCheckData.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckData_CheckedChanged);
            // 
            // txtCheckHead
            // 
            this.txtCheckHead.AllowClear = true;
            this.txtCheckHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckHead.Location = new System.Drawing.Point(115, 253);
            this.txtCheckHead.Name = "txtCheckHead";
            this.txtCheckHead.PlaceholderText = "十六进制带空格，支持 ; 分隔符";
            this.txtCheckHead.Size = new System.Drawing.Size(366, 42);
            this.txtCheckHead.TabIndex = 9;
            this.txtCheckHead.TextChanged += new System.EventHandler(this.txtCheckHead_TextChanged);
            // 
            // cbCheckHead
            // 
            this.cbCheckHead.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckHead.Location = new System.Drawing.Point(3, 253);
            this.cbCheckHead.Name = "cbCheckHead";
            this.cbCheckHead.Size = new System.Drawing.Size(106, 42);
            this.cbCheckHead.TabIndex = 8;
            this.cbCheckHead.Text = "指定包头";
            this.cbCheckHead.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckHead_CheckedChanged);
            // 
            // txtCheckPort
            // 
            this.txtCheckPort.AllowClear = true;
            this.txtCheckPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckPort.Location = new System.Drawing.Point(115, 205);
            this.txtCheckPort.Name = "txtCheckPort";
            this.txtCheckPort.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckPort.Size = new System.Drawing.Size(366, 42);
            this.txtCheckPort.TabIndex = 7;
            this.txtCheckPort.TextChanged += new System.EventHandler(this.txtCheckPort_TextChanged);
            // 
            // cbCheckPort
            // 
            this.cbCheckPort.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckPort.Location = new System.Drawing.Point(3, 205);
            this.cbCheckPort.Name = "cbCheckPort";
            this.cbCheckPort.Size = new System.Drawing.Size(90, 42);
            this.cbCheckPort.TabIndex = 6;
            this.cbCheckPort.Text = "端口号";
            this.cbCheckPort.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckPort_CheckedChanged);
            // 
            // txtCheckIP
            // 
            this.txtCheckIP.AllowClear = true;
            this.txtCheckIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckIP.Location = new System.Drawing.Point(115, 157);
            this.txtCheckIP.Name = "txtCheckIP";
            this.txtCheckIP.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckIP.Size = new System.Drawing.Size(366, 42);
            this.txtCheckIP.TabIndex = 5;
            this.txtCheckIP.TextChanged += new System.EventHandler(this.txtCheckIP_TextChanged);
            // 
            // cbCheckIP
            // 
            this.cbCheckIP.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckIP.Location = new System.Drawing.Point(3, 157);
            this.cbCheckIP.Name = "cbCheckIP";
            this.cbCheckIP.Size = new System.Drawing.Size(89, 42);
            this.cbCheckIP.TabIndex = 4;
            this.cbCheckIP.Text = "IP地址";
            this.cbCheckIP.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckIP_CheckedChanged);
            // 
            // txtCheckLen
            // 
            this.txtCheckLen.AllowClear = true;
            this.txtCheckLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckLen.Location = new System.Drawing.Point(115, 109);
            this.txtCheckLen.Name = "txtCheckLen";
            this.txtCheckLen.PlaceholderText = "例如：0-99;100";
            this.txtCheckLen.Size = new System.Drawing.Size(366, 42);
            this.txtCheckLen.TabIndex = 3;
            this.txtCheckLen.TextChanged += new System.EventHandler(this.txtCheckLen_TextChanged);
            // 
            // cbCheckLen
            // 
            this.cbCheckLen.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckLen.Location = new System.Drawing.Point(3, 109);
            this.cbCheckLen.Name = "cbCheckLen";
            this.cbCheckLen.Size = new System.Drawing.Size(74, 42);
            this.cbCheckLen.TabIndex = 2;
            this.cbCheckLen.Text = "长度";
            this.cbCheckLen.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckLen_CheckedChanged);
            // 
            // cbCheckSocket
            // 
            this.cbCheckSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckSocket.Location = new System.Drawing.Point(3, 61);
            this.cbCheckSocket.Name = "cbCheckSocket";
            this.cbCheckSocket.Size = new System.Drawing.Size(90, 42);
            this.cbCheckSocket.TabIndex = 0;
            this.cbCheckSocket.Text = "套接字";
            this.cbCheckSocket.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckSocket_CheckedChanged);
            // 
            // txtCheckSocket
            // 
            this.txtCheckSocket.AllowClear = true;
            this.txtCheckSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckSocket.Location = new System.Drawing.Point(115, 61);
            this.txtCheckSocket.Name = "txtCheckSocket";
            this.txtCheckSocket.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckSocket.Size = new System.Drawing.Size(366, 42);
            this.txtCheckSocket.TabIndex = 12;
            this.txtCheckSocket.TextChanged += new System.EventHandler(this.txtCheckSocket_TextChanged);
            // 
            // tlpFilterSettings
            // 
            this.tlpFilterSettings.ColumnCount = 1;
            this.tlpFilterSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterSettings.Controls.Add(this.tlpContent, 0, 0);
            this.tlpFilterSettings.Controls.Add(this.tlpButton, 0, 1);
            this.tlpFilterSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterSettings.Name = "tlpFilterSettings";
            this.tlpFilterSettings.RowCount = 2;
            this.tlpFilterSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpFilterSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterSettings.Size = new System.Drawing.Size(484, 461);
            this.tlpFilterSettings.TabIndex = 1;
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
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Size = new System.Drawing.Size(484, 401);
            this.tlpContent.TabIndex = 1;
            // 
            // lIsShow
            // 
            this.lIsShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lIsShow.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lIsShow.Location = new System.Drawing.Point(3, 3);
            this.lIsShow.Name = "lIsShow";
            this.lIsShow.Size = new System.Drawing.Size(106, 32);
            this.lIsShow.TabIndex = 13;
            this.lIsShow.Text = "是否显示";
            this.lIsShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sIsShow
            // 
            this.sIsShow.Checked = true;
            this.sIsShow.CheckedText = "是";
            this.sIsShow.Location = new System.Drawing.Point(115, 3);
            this.sIsShow.Name = "sIsShow";
            this.sIsShow.Size = new System.Drawing.Size(60, 32);
            this.sIsShow.TabIndex = 14;
            this.sIsShow.UnCheckedText = "否";
            // 
            // FilterSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tlpFilterSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FilterSettingsForm";
            this.Text = "FilterSettings";
            this.Load += new System.EventHandler(this.FilterSettingsForm_Load);
            this.tlpButton.ResumeLayout(false);
            this.tlpFilterSettings.ResumeLayout(false);
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Button bExit;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
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
        private System.Windows.Forms.TableLayoutPanel tlpFilterSettings;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private AntdUI.Label lIsShow;
        private AntdUI.Switch sIsShow;
    }
}
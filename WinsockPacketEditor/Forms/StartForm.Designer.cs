namespace WinsockPacketEditor
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.tlpStartForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSelectMode = new System.Windows.Forms.TableLayoutPanel();
            this.bInject = new AntdUI.Button();
            this.bProxy = new AntdUI.Button();
            this.cbIsRemote = new AntdUI.Checkbox();
            this.tlpRemoteInfo = new System.Windows.Forms.TableLayoutPanel();
            this.txtRemote_UserName = new AntdUI.Input();
            this.txtRemote_PassWord = new AntdUI.Input();
            this.nudRemote_Port = new AntdUI.InputNumber();
            this.lRemote_UserName = new AntdUI.Label();
            this.lRemote_PassWord = new AntdUI.Label();
            this.lRemote_Port = new AntdUI.Label();
            this.tlpRemoteURL = new System.Windows.Forms.TableLayoutPanel();
            this.lRemoteMGT = new AntdUI.Label();
            this.lRemoteURL = new System.Windows.Forms.LinkLabel();
            this.lWPE = new AntdUI.Label();
            this.pageHeader = new AntdUI.PageHeader();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.btn_setting = new AntdUI.Button();
            this.tlpStartForm.SuspendLayout();
            this.tlpSelectMode.SuspendLayout();
            this.tlpRemoteInfo.SuspendLayout();
            this.tlpRemoteURL.SuspendLayout();
            this.pageHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpStartForm
            // 
            this.tlpStartForm.ColumnCount = 1;
            this.tlpStartForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStartForm.Controls.Add(this.tlpSelectMode, 0, 1);
            this.tlpStartForm.Controls.Add(this.cbIsRemote, 0, 2);
            this.tlpStartForm.Controls.Add(this.tlpRemoteInfo, 0, 3);
            this.tlpStartForm.Controls.Add(this.tlpRemoteURL, 0, 4);
            this.tlpStartForm.Controls.Add(this.lWPE, 0, 0);
            this.tlpStartForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStartForm.Location = new System.Drawing.Point(0, 40);
            this.tlpStartForm.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStartForm.Name = "tlpStartForm";
            this.tlpStartForm.RowCount = 5;
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpStartForm.Size = new System.Drawing.Size(420, 410);
            this.tlpStartForm.TabIndex = 1;
            // 
            // tlpSelectMode
            // 
            this.tlpSelectMode.ColumnCount = 5;
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSelectMode.Controls.Add(this.bInject, 3, 1);
            this.tlpSelectMode.Controls.Add(this.bProxy, 1, 1);
            this.tlpSelectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelectMode.Location = new System.Drawing.Point(0, 70);
            this.tlpSelectMode.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSelectMode.Name = "tlpSelectMode";
            this.tlpSelectMode.RowCount = 3;
            this.tlpSelectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSelectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSelectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSelectMode.Size = new System.Drawing.Size(420, 120);
            this.tlpSelectMode.TabIndex = 8;
            // 
            // bInject
            // 
            this.bInject.BackExtend = "135, #6253E1, #04BEFE";
            this.bInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInject.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bInject.LocalizationText = "StartForm.{id}";
            this.bInject.Location = new System.Drawing.Point(223, 28);
            this.bInject.Name = "bInject";
            this.bInject.Radius = 0;
            this.bInject.Shape = AntdUI.TShape.Round;
            this.bInject.Size = new System.Drawing.Size(174, 64);
            this.bInject.TabIndex = 1;
            this.bInject.Text = "注入模式";
            this.bInject.Type = AntdUI.TTypeMini.Primary;
            this.bInject.Click += new System.EventHandler(this.bInject_Click);
            // 
            // bProxy
            // 
            this.bProxy.BackExtend = "135, #6253E1, #04BEFE";
            this.bProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bProxy.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bProxy.LocalizationText = "StartForm.{id}";
            this.bProxy.Location = new System.Drawing.Point(23, 28);
            this.bProxy.Name = "bProxy";
            this.bProxy.Radius = 0;
            this.bProxy.Shape = AntdUI.TShape.Round;
            this.bProxy.Size = new System.Drawing.Size(174, 64);
            this.bProxy.TabIndex = 0;
            this.bProxy.Text = "代理模式";
            this.bProxy.Type = AntdUI.TTypeMini.Primary;
            this.bProxy.Click += new System.EventHandler(this.bProxy_Click);
            // 
            // cbIsRemote
            // 
            this.cbIsRemote.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsRemote.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbIsRemote.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIsRemote.LocalizationText = "StartForm.{id}";
            this.cbIsRemote.Location = new System.Drawing.Point(6, 193);
            this.cbIsRemote.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.cbIsRemote.Name = "cbIsRemote";
            this.cbIsRemote.Size = new System.Drawing.Size(138, 24);
            this.cbIsRemote.TabIndex = 9;
            this.cbIsRemote.Text = "启用远程管理";
            this.cbIsRemote.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsRemote_CheckedChanged);
            // 
            // tlpRemoteInfo
            // 
            this.tlpRemoteInfo.ColumnCount = 4;
            this.tlpRemoteInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpRemoteInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRemoteInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpRemoteInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpRemoteInfo.Controls.Add(this.txtRemote_UserName, 2, 1);
            this.tlpRemoteInfo.Controls.Add(this.txtRemote_PassWord, 2, 2);
            this.tlpRemoteInfo.Controls.Add(this.nudRemote_Port, 2, 3);
            this.tlpRemoteInfo.Controls.Add(this.lRemote_UserName, 1, 1);
            this.tlpRemoteInfo.Controls.Add(this.lRemote_PassWord, 1, 2);
            this.tlpRemoteInfo.Controls.Add(this.lRemote_Port, 1, 3);
            this.tlpRemoteInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemoteInfo.Location = new System.Drawing.Point(0, 220);
            this.tlpRemoteInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRemoteInfo.Name = "tlpRemoteInfo";
            this.tlpRemoteInfo.RowCount = 5;
            this.tlpRemoteInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRemoteInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRemoteInfo.Size = new System.Drawing.Size(420, 150);
            this.tlpRemoteInfo.TabIndex = 10;
            // 
            // txtRemote_UserName
            // 
            this.txtRemote_UserName.AllowClear = true;
            this.txtRemote_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote_UserName.LocalizationPlaceholderText = "Input.LetterOrNum";
            this.txtRemote_UserName.Location = new System.Drawing.Point(108, 9);
            this.txtRemote_UserName.Name = "txtRemote_UserName";
            this.txtRemote_UserName.PlaceholderText = "输入英文字母，或者数字";
            this.txtRemote_UserName.Size = new System.Drawing.Size(274, 40);
            this.txtRemote_UserName.TabIndex = 9;
            this.txtRemote_UserName.TextChanged += new System.EventHandler(this.txtRemote_UserName_TextChanged);
            // 
            // txtRemote_PassWord
            // 
            this.txtRemote_PassWord.AllowClear = true;
            this.txtRemote_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote_PassWord.LocalizationPlaceholderText = "Input.LetterOrNum";
            this.txtRemote_PassWord.Location = new System.Drawing.Point(108, 55);
            this.txtRemote_PassWord.Name = "txtRemote_PassWord";
            this.txtRemote_PassWord.PasswordCopy = true;
            this.txtRemote_PassWord.PlaceholderText = "输入英文字母，或者数字";
            this.txtRemote_PassWord.Size = new System.Drawing.Size(274, 40);
            this.txtRemote_PassWord.TabIndex = 3;
            this.txtRemote_PassWord.UseSystemPasswordChar = true;
            this.txtRemote_PassWord.TextChanged += new System.EventHandler(this.txtRemote_PassWord_TextChanged);
            // 
            // nudRemote_Port
            // 
            this.nudRemote_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudRemote_Port.Location = new System.Drawing.Point(108, 101);
            this.nudRemote_Port.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudRemote_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRemote_Port.Name = "nudRemote_Port";
            this.nudRemote_Port.PlaceholderText = "";
            this.nudRemote_Port.Size = new System.Drawing.Size(274, 40);
            this.nudRemote_Port.TabIndex = 5;
            this.nudRemote_Port.Text = "88";
            this.nudRemote_Port.Value = new decimal(new int[] {
            88,
            0,
            0,
            0});
            // 
            // lRemote_UserName
            // 
            this.lRemote_UserName.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRemote_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemote_UserName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemote_UserName.LocalizationText = "StartForm.{id}";
            this.lRemote_UserName.Location = new System.Drawing.Point(38, 9);
            this.lRemote_UserName.Name = "lRemote_UserName";
            this.lRemote_UserName.Size = new System.Drawing.Size(64, 40);
            this.lRemote_UserName.TabIndex = 6;
            this.lRemote_UserName.Text = "管理账号";
            this.lRemote_UserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lRemote_PassWord
            // 
            this.lRemote_PassWord.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRemote_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemote_PassWord.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemote_PassWord.LocalizationText = "StartForm.{id}";
            this.lRemote_PassWord.Location = new System.Drawing.Point(38, 55);
            this.lRemote_PassWord.Name = "lRemote_PassWord";
            this.lRemote_PassWord.Size = new System.Drawing.Size(32, 40);
            this.lRemote_PassWord.TabIndex = 7;
            this.lRemote_PassWord.Text = "密码";
            this.lRemote_PassWord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lRemote_Port
            // 
            this.lRemote_Port.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRemote_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemote_Port.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemote_Port.LocalizationText = "StartForm.{id}";
            this.lRemote_Port.Location = new System.Drawing.Point(38, 101);
            this.lRemote_Port.Name = "lRemote_Port";
            this.lRemote_Port.Size = new System.Drawing.Size(48, 40);
            this.lRemote_Port.TabIndex = 8;
            this.lRemote_Port.Text = "端口号";
            this.lRemote_Port.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tlpRemoteURL
            // 
            this.tlpRemoteURL.ColumnCount = 4;
            this.tlpRemoteURL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRemoteURL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRemoteURL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRemoteURL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRemoteURL.Controls.Add(this.lRemoteMGT, 1, 0);
            this.tlpRemoteURL.Controls.Add(this.lRemoteURL, 2, 0);
            this.tlpRemoteURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemoteURL.Location = new System.Drawing.Point(0, 370);
            this.tlpRemoteURL.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRemoteURL.Name = "tlpRemoteURL";
            this.tlpRemoteURL.RowCount = 2;
            this.tlpRemoteURL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteURL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemoteURL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRemoteURL.Size = new System.Drawing.Size(420, 40);
            this.tlpRemoteURL.TabIndex = 11;
            // 
            // lRemoteMGT
            // 
            this.lRemoteMGT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRemoteMGT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemoteMGT.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemoteMGT.LocalizationText = "StartForm.{id}";
            this.lRemoteMGT.Location = new System.Drawing.Point(79, 3);
            this.lRemoteMGT.Name = "lRemoteMGT";
            this.lRemoteMGT.Size = new System.Drawing.Size(88, 22);
            this.lRemoteMGT.TabIndex = 0;
            this.lRemoteMGT.Text = "远程管理网址:";
            // 
            // lRemoteURL
            // 
            this.lRemoteURL.AutoSize = true;
            this.lRemoteURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemoteURL.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemoteURL.LinkColor = System.Drawing.Color.RoyalBlue;
            this.lRemoteURL.Location = new System.Drawing.Point(173, 3);
            this.lRemoteURL.Margin = new System.Windows.Forms.Padding(3);
            this.lRemoteURL.Name = "lRemoteURL";
            this.lRemoteURL.Size = new System.Drawing.Size(168, 22);
            this.lRemoteURL.TabIndex = 1;
            this.lRemoteURL.TabStop = true;
            this.lRemoteURL.Text = "http://192.168.88.119:88";
            this.lRemoteURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lRemoteURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lRemoteURL_LinkClicked);
            // 
            // lWPE
            // 
            this.lWPE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWPE.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lWPE.Location = new System.Drawing.Point(3, 3);
            this.lWPE.Name = "lWPE";
            this.lWPE.Shadow = 3;
            this.lWPE.Size = new System.Drawing.Size(414, 64);
            this.lWPE.TabIndex = 12;
            this.lWPE.Text = "Winsock Packet Editor";
            this.lWPE.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pageHeader
            // 
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.Controls.Add(this.btn_mode);
            this.pageHeader.Controls.Add(this.btn_global);
            this.pageHeader.Controls.Add(this.btn_setting);
            this.pageHeader.DividerMargin = 3;
            this.pageHeader.DividerShow = true;
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.MaximizeBox = false;
            this.pageHeader.MinimizeBox = false;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(420, 40);
            this.pageHeader.SubText = "2.0.0.0";
            this.pageHeader.TabIndex = 0;
            this.pageHeader.Text = "WPE x64";
            // 
            // btn_mode
            // 
            this.btn_mode.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_mode.Ghost = true;
            this.btn_mode.IconSvg = "SunOutlined";
            this.btn_mode.Location = new System.Drawing.Point(230, 0);
            this.btn_mode.Name = "btn_mode";
            this.btn_mode.Radius = 0;
            this.btn_mode.Size = new System.Drawing.Size(50, 40);
            this.btn_mode.TabIndex = 12;
            this.btn_mode.ToggleIconSvg = "MoonOutlined";
            this.btn_mode.WaveSize = 0;
            this.btn_mode.Click += new System.EventHandler(this.btn_mode_Click);
            // 
            // btn_global
            // 
            this.btn_global.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_global.DropDownRadius = 6;
            this.btn_global.Ghost = true;
            this.btn_global.IconSvg = "GlobalOutlined";
            this.btn_global.Location = new System.Drawing.Point(280, 0);
            this.btn_global.Name = "btn_global";
            this.btn_global.Placement = AntdUI.TAlignFrom.BR;
            this.btn_global.Radius = 0;
            this.btn_global.Size = new System.Drawing.Size(50, 40);
            this.btn_global.TabIndex = 11;
            this.btn_global.WaveSize = 0;
            this.btn_global.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.btn_global_SelectedValueChanged);
            // 
            // btn_setting
            // 
            this.btn_setting.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_setting.Ghost = true;
            this.btn_setting.IconSvg = "SettingOutlined";
            this.btn_setting.Location = new System.Drawing.Point(330, 0);
            this.btn_setting.Name = "btn_setting";
            this.btn_setting.Radius = 0;
            this.btn_setting.Size = new System.Drawing.Size(50, 40);
            this.btn_setting.TabIndex = 10;
            this.btn_setting.WaveSize = 0;
            this.btn_setting.Click += new System.EventHandler(this.btn_setting_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(420, 450);
            this.Controls.Add(this.tlpStartForm);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "StartForm";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WPE x64";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartForm_FormClosing);
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.tlpStartForm.ResumeLayout(false);
            this.tlpStartForm.PerformLayout();
            this.tlpSelectMode.ResumeLayout(false);
            this.tlpRemoteInfo.ResumeLayout(false);
            this.tlpRemoteInfo.PerformLayout();
            this.tlpRemoteURL.ResumeLayout(false);
            this.tlpRemoteURL.PerformLayout();
            this.pageHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private System.Windows.Forms.TableLayoutPanel tlpStartForm;
        private System.Windows.Forms.TableLayoutPanel tlpSelectMode;
        private AntdUI.Button bInject;
        private AntdUI.Button bProxy;
        private AntdUI.Checkbox cbIsRemote;
        private System.Windows.Forms.TableLayoutPanel tlpRemoteInfo;
        private AntdUI.Input txtRemote_PassWord;
        private AntdUI.InputNumber nudRemote_Port;
        private AntdUI.Label lRemote_UserName;
        private AntdUI.Label lRemote_PassWord;
        private AntdUI.Label lRemote_Port;
        private AntdUI.Input txtRemote_UserName;
        private System.Windows.Forms.TableLayoutPanel tlpRemoteURL;
        private AntdUI.Label lRemoteMGT;
        private System.Windows.Forms.LinkLabel lRemoteURL;
        private AntdUI.Label lWPE;
        private AntdUI.Button btn_setting;
        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
    }
}
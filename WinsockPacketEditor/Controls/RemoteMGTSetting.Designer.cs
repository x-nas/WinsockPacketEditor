namespace WinsockPacketEditor
{
    partial class RemoteMGTSetting
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
            this.tlpRemoteMGT = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.lRemote = new AntdUI.Label();
            this.cbIsRemote = new AntdUI.Checkbox();
            this.txtRemote_UserName = new AntdUI.Input();
            this.txtRemote_PassWord = new AntdUI.Input();
            this.nudRemote_Port = new AntdUI.InputNumber();
            this.ddlRemoteIP = new AntdUI.Select();
            this.tlpRemoteMGT.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRemoteMGT
            // 
            this.tlpRemoteMGT.ColumnCount = 1;
            this.tlpRemoteMGT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemoteMGT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRemoteMGT.Controls.Add(this.tlpButton, 0, 6);
            this.tlpRemoteMGT.Controls.Add(this.lRemote, 0, 5);
            this.tlpRemoteMGT.Controls.Add(this.cbIsRemote, 0, 0);
            this.tlpRemoteMGT.Controls.Add(this.txtRemote_UserName, 0, 2);
            this.tlpRemoteMGT.Controls.Add(this.txtRemote_PassWord, 0, 3);
            this.tlpRemoteMGT.Controls.Add(this.nudRemote_Port, 0, 4);
            this.tlpRemoteMGT.Controls.Add(this.ddlRemoteIP, 0, 1);
            this.tlpRemoteMGT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemoteMGT.Location = new System.Drawing.Point(0, 0);
            this.tlpRemoteMGT.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRemoteMGT.Name = "tlpRemoteMGT";
            this.tlpRemoteMGT.RowCount = 7;
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemoteMGT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpRemoteMGT.Size = new System.Drawing.Size(280, 350);
            this.tlpRemoteMGT.TabIndex = 0;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 290);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(280, 60);
            this.tlpButton.TabIndex = 18;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(45, 7);
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
            this.bExit.Location = new System.Drawing.Point(153, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(82, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // lRemote
            // 
            this.lRemote.ColorScheme = AntdUI.TAMode.Light;
            this.lRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemote.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemote.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lRemote.Location = new System.Drawing.Point(3, 255);
            this.lRemote.Name = "lRemote";
            this.lRemote.Size = new System.Drawing.Size(274, 32);
            this.lRemote.TabIndex = 6;
            this.lRemote.Text = "lRemote";
            this.lRemote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbIsRemote
            // 
            this.cbIsRemote.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsRemote.LocalizationText = "RemoteMGTSetting.EnableMGT";
            this.cbIsRemote.Location = new System.Drawing.Point(3, 3);
            this.cbIsRemote.Name = "cbIsRemote";
            this.cbIsRemote.Size = new System.Drawing.Size(140, 42);
            this.cbIsRemote.TabIndex = 0;
            this.cbIsRemote.Text = "启用远程管理";
            this.cbIsRemote.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsRemote_CheckedChanged);
            // 
            // txtRemote_UserName
            // 
            this.txtRemote_UserName.AllowClear = true;
            this.txtRemote_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote_UserName.LocalizationPlaceholderText = "RemoteMGTSetting.UserName";
            this.txtRemote_UserName.Location = new System.Drawing.Point(3, 102);
            this.txtRemote_UserName.Name = "txtRemote_UserName";
            this.txtRemote_UserName.PlaceholderText = "请输入管理员账号";
            this.txtRemote_UserName.Size = new System.Drawing.Size(274, 45);
            this.txtRemote_UserName.TabIndex = 1;
            this.txtRemote_UserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRemote_PassWord
            // 
            this.txtRemote_PassWord.AllowClear = true;
            this.txtRemote_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote_PassWord.LocalizationPlaceholderText = "RemoteMGTSetting.PassWord";
            this.txtRemote_PassWord.Location = new System.Drawing.Point(3, 153);
            this.txtRemote_PassWord.Name = "txtRemote_PassWord";
            this.txtRemote_PassWord.PlaceholderText = "请输入密码";
            this.txtRemote_PassWord.Size = new System.Drawing.Size(274, 45);
            this.txtRemote_PassWord.TabIndex = 2;
            this.txtRemote_PassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRemote_PassWord.UseSystemPasswordChar = true;
            // 
            // nudRemote_Port
            // 
            this.nudRemote_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudRemote_Port.Location = new System.Drawing.Point(3, 204);
            this.nudRemote_Port.Name = "nudRemote_Port";
            this.nudRemote_Port.Size = new System.Drawing.Size(274, 45);
            this.nudRemote_Port.TabIndex = 3;
            this.nudRemote_Port.Text = "88";
            this.nudRemote_Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRemote_Port.Value = new decimal(new int[] {
            88,
            0,
            0,
            0});
            this.nudRemote_Port.ValueChanged += new AntdUI.DecimalEventHandler(this.nudRemote_Port_ValueChanged);
            // 
            // ddlRemoteIP
            // 
            this.ddlRemoteIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlRemoteIP.List = true;
            this.ddlRemoteIP.Location = new System.Drawing.Point(3, 51);
            this.ddlRemoteIP.Name = "ddlRemoteIP";
            this.ddlRemoteIP.Size = new System.Drawing.Size(274, 45);
            this.ddlRemoteIP.TabIndex = 7;
            this.ddlRemoteIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ddlRemoteIP.SelectedIndexChanged += new AntdUI.IntEventHandler(this.ddlRemoteIP_SelectedIndexChanged);
            // 
            // RemoteMGTSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRemoteMGT);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "RemoteMGTSetting";
            this.Size = new System.Drawing.Size(280, 350);
            this.Load += new System.EventHandler(this.RemoteMGTSetting_Load);
            this.tlpRemoteMGT.ResumeLayout(false);
            this.tlpRemoteMGT.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpRemoteMGT;
        private AntdUI.Checkbox cbIsRemote;
        private AntdUI.Input txtRemote_UserName;
        private AntdUI.Input txtRemote_PassWord;
        private AntdUI.InputNumber nudRemote_Port;
        private AntdUI.Select ddlRemoteIP;
        private AntdUI.Label lRemote;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
    }
}

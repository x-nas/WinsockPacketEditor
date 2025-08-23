namespace WinsockPacketEditor
{
    partial class AccountEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountEditForm));
            this.tlpProxyAccount = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpAccountInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new AntdUI.Label();
            this.label2 = new AntdUI.Label();
            this.txtUserName = new AntdUI.Input();
            this.txtPassword = new AntdUI.Input();
            this.cbIsLimitLinks = new AntdUI.Checkbox();
            this.cbIsLimitDevices = new AntdUI.Checkbox();
            this.cbIsExpiry = new AntdUI.Checkbox();
            this.nudLimitLinks = new AntdUI.InputNumber();
            this.nudLimitDevices = new AntdUI.InputNumber();
            this.dtpExpiryTime = new AntdUI.DatePicker();
            this.cbIsEnable = new AntdUI.Checkbox();
            this.tlpProxyAccount.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpAccountInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyAccount
            // 
            this.tlpProxyAccount.ColumnCount = 1;
            this.tlpProxyAccount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyAccount.Controls.Add(this.tlpButton, 0, 2);
            this.tlpProxyAccount.Controls.Add(this.tlpAccountInfo, 0, 1);
            this.tlpProxyAccount.Controls.Add(this.cbIsEnable, 0, 0);
            this.tlpProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyAccount.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyAccount.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyAccount.Name = "tlpProxyAccount";
            this.tlpProxyAccount.RowCount = 3;
            this.tlpProxyAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyAccount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyAccount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpProxyAccount.Size = new System.Drawing.Size(484, 761);
            this.tlpProxyAccount.TabIndex = 1;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(115, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(255, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpAccountInfo
            // 
            this.tlpAccountInfo.ColumnCount = 4;
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAccountInfo.Controls.Add(this.label1, 1, 0);
            this.tlpAccountInfo.Controls.Add(this.label2, 1, 1);
            this.tlpAccountInfo.Controls.Add(this.txtUserName, 2, 0);
            this.tlpAccountInfo.Controls.Add(this.txtPassword, 2, 1);
            this.tlpAccountInfo.Controls.Add(this.cbIsLimitLinks, 1, 2);
            this.tlpAccountInfo.Controls.Add(this.cbIsLimitDevices, 1, 3);
            this.tlpAccountInfo.Controls.Add(this.cbIsExpiry, 1, 4);
            this.tlpAccountInfo.Controls.Add(this.nudLimitLinks, 2, 2);
            this.tlpAccountInfo.Controls.Add(this.nudLimitDevices, 2, 3);
            this.tlpAccountInfo.Controls.Add(this.dtpExpiryTime, 2, 4);
            this.tlpAccountInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountInfo.Location = new System.Drawing.Point(0, 48);
            this.tlpAccountInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountInfo.Name = "tlpAccountInfo";
            this.tlpAccountInfo.RowCount = 6;
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountInfo.Size = new System.Drawing.Size(484, 653);
            this.tlpAccountInfo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.LocalizationText = "Username";
            this.label1.Location = new System.Drawing.Point(31, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 45);
            this.label1.TabIndex = 10;
            this.label1.Text = "用户名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.LocalizationText = "Password";
            this.label2.Location = new System.Drawing.Point(31, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 45);
            this.label2.TabIndex = 11;
            this.label2.Text = "密码";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUserName
            // 
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Location = new System.Drawing.Point(159, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(294, 45);
            this.txtUserName.TabIndex = 12;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Location = new System.Drawing.Point(159, 54);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(294, 45);
            this.txtPassword.TabIndex = 13;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // cbIsLimitLinks
            // 
            this.cbIsLimitLinks.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsLimitLinks.LocalizationText = "AccountEditForm.LimitLinks";
            this.cbIsLimitLinks.Location = new System.Drawing.Point(31, 105);
            this.cbIsLimitLinks.Name = "cbIsLimitLinks";
            this.cbIsLimitLinks.Size = new System.Drawing.Size(122, 42);
            this.cbIsLimitLinks.TabIndex = 14;
            this.cbIsLimitLinks.Text = "限制链接数";
            this.cbIsLimitLinks.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsLimitLinks_CheckedChanged);
            // 
            // cbIsLimitDevices
            // 
            this.cbIsLimitDevices.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsLimitDevices.LocalizationText = "AccountEditForm.LimitDevices";
            this.cbIsLimitDevices.Location = new System.Drawing.Point(31, 156);
            this.cbIsLimitDevices.Name = "cbIsLimitDevices";
            this.cbIsLimitDevices.Size = new System.Drawing.Size(122, 42);
            this.cbIsLimitDevices.TabIndex = 15;
            this.cbIsLimitDevices.Text = "限制设备数";
            this.cbIsLimitDevices.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsLimitDevices_CheckedChanged);
            // 
            // cbIsExpiry
            // 
            this.cbIsExpiry.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsExpiry.LocalizationText = "AccountEditForm.ExpireTime";
            this.cbIsExpiry.Location = new System.Drawing.Point(31, 207);
            this.cbIsExpiry.Name = "cbIsExpiry";
            this.cbIsExpiry.Size = new System.Drawing.Size(106, 42);
            this.cbIsExpiry.TabIndex = 16;
            this.cbIsExpiry.Text = "过期时间";
            this.cbIsExpiry.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsExpiry_CheckedChanged);
            // 
            // nudLimitLinks
            // 
            this.nudLimitLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitLinks.Location = new System.Drawing.Point(159, 105);
            this.nudLimitLinks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimitLinks.Name = "nudLimitLinks";
            this.nudLimitLinks.SelectionStart = 1;
            this.nudLimitLinks.Size = new System.Drawing.Size(294, 45);
            this.nudLimitLinks.TabIndex = 17;
            this.nudLimitLinks.Text = "1";
            this.nudLimitLinks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudLimitDevices
            // 
            this.nudLimitDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLimitDevices.Location = new System.Drawing.Point(159, 156);
            this.nudLimitDevices.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimitDevices.Name = "nudLimitDevices";
            this.nudLimitDevices.SelectionStart = 1;
            this.nudLimitDevices.Size = new System.Drawing.Size(294, 45);
            this.nudLimitDevices.TabIndex = 18;
            this.nudLimitDevices.Text = "1";
            this.nudLimitDevices.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dtpExpiryTime
            // 
            this.dtpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpExpiryTime.Format = "yyyy-MM-dd HH:mm:ss";
            this.dtpExpiryTime.Location = new System.Drawing.Point(159, 207);
            this.dtpExpiryTime.MaxDate = new System.DateTime(8888, 12, 31, 0, 0, 0, 0);
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            this.dtpExpiryTime.Size = new System.Drawing.Size(294, 42);
            this.dtpExpiryTime.TabIndex = 19;
            // 
            // cbIsEnable
            // 
            this.cbIsEnable.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsEnable.LocalizationText = "Enable";
            this.cbIsEnable.Location = new System.Drawing.Point(3, 3);
            this.cbIsEnable.Name = "cbIsEnable";
            this.cbIsEnable.Size = new System.Drawing.Size(74, 42);
            this.cbIsEnable.TabIndex = 18;
            this.cbIsEnable.Text = "启用";
            this.cbIsEnable.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsEnable_CheckedChanged);
            // 
            // AccountEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 761);
            this.Controls.Add(this.tlpProxyAccount);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AccountEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccountEditForm";
            this.Load += new System.EventHandler(this.AccountEditForm_Load);
            this.tlpProxyAccount.ResumeLayout(false);
            this.tlpProxyAccount.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpAccountInfo.ResumeLayout(false);
            this.tlpAccountInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProxyAccount;
        private System.Windows.Forms.TableLayoutPanel tlpAccountInfo;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Input txtUserName;
        private AntdUI.Input txtPassword;
        private AntdUI.Checkbox cbIsLimitLinks;
        private AntdUI.Checkbox cbIsEnable;
        private AntdUI.Checkbox cbIsLimitDevices;
        private AntdUI.Checkbox cbIsExpiry;
        private AntdUI.InputNumber nudLimitLinks;
        private AntdUI.InputNumber nudLimitDevices;
        private AntdUI.DatePicker dtpExpiryTime;
    }
}
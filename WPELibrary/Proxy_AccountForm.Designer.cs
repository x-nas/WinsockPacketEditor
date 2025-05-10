namespace WPELibrary
{
    partial class Proxy_AccountForm
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
            this.tlpProxyAccount = new System.Windows.Forms.TableLayoutPanel();
            this.cbIsEnable = new System.Windows.Forms.CheckBox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tlpAccountInfo = new System.Windows.Forms.TableLayoutPanel();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.lPassWord = new System.Windows.Forms.Label();
            this.lUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.cbIsExpiry = new System.Windows.Forms.CheckBox();
            this.dtpExpiryTime = new System.Windows.Forms.DateTimePicker();
            this.tlpProxyAccount.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpAccountInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyAccount
            // 
            this.tlpProxyAccount.ColumnCount = 1;
            this.tlpProxyAccount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyAccount.Controls.Add(this.cbIsEnable, 0, 0);
            this.tlpProxyAccount.Controls.Add(this.tlpButton, 0, 2);
            this.tlpProxyAccount.Controls.Add(this.tlpAccountInfo, 0, 1);
            this.tlpProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyAccount.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyAccount.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyAccount.Name = "tlpProxyAccount";
            this.tlpProxyAccount.RowCount = 3;
            this.tlpProxyAccount.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyAccount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyAccount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProxyAccount.Size = new System.Drawing.Size(384, 261);
            this.tlpProxyAccount.TabIndex = 0;
            // 
            // cbIsEnable
            // 
            this.cbIsEnable.AutoSize = true;
            this.cbIsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsEnable.Location = new System.Drawing.Point(3, 3);
            this.cbIsEnable.Name = "cbIsEnable";
            this.cbIsEnable.Padding = new System.Windows.Forms.Padding(3);
            this.cbIsEnable.Size = new System.Drawing.Size(378, 27);
            this.cbIsEnable.TabIndex = 7;
            this.cbIsEnable.Text = "启用";
            this.cbIsEnable.UseVisualStyleBackColor = true;
            this.cbIsEnable.CheckedChanged += new System.EventHandler(this.cbIsEnable_CheckedChanged);
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bCancel, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 211);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(384, 50);
            this.tlpButton.TabIndex = 0;
            // 
            // bSave
            // 
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.Location = new System.Drawing.Point(70, 10);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(94, 29);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保 存";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bCancel
            // 
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bCancel.Location = new System.Drawing.Point(220, 10);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(94, 29);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "取 消";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tlpAccountInfo
            // 
            this.tlpAccountInfo.ColumnCount = 4;
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpAccountInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAccountInfo.Controls.Add(this.txtPassWord, 2, 2);
            this.tlpAccountInfo.Controls.Add(this.lPassWord, 1, 2);
            this.tlpAccountInfo.Controls.Add(this.lUserName, 1, 1);
            this.tlpAccountInfo.Controls.Add(this.txtUserName, 2, 1);
            this.tlpAccountInfo.Controls.Add(this.cbIsExpiry, 1, 3);
            this.tlpAccountInfo.Controls.Add(this.dtpExpiryTime, 2, 3);
            this.tlpAccountInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountInfo.Location = new System.Drawing.Point(0, 33);
            this.tlpAccountInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountInfo.Name = "tlpAccountInfo";
            this.tlpAccountInfo.RowCount = 5;
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpAccountInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountInfo.Size = new System.Drawing.Size(384, 178);
            this.tlpAccountInfo.TabIndex = 1;
            // 
            // txtPassWord
            // 
            this.txtPassWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassWord.Location = new System.Drawing.Point(135, 29);
            this.txtPassWord.Margin = new System.Windows.Forms.Padding(0);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Size = new System.Drawing.Size(200, 23);
            this.txtPassWord.TabIndex = 3;
            this.txtPassWord.UseSystemPasswordChar = true;
            this.txtPassWord.WordWrap = false;
            // 
            // lPassWord
            // 
            this.lPassWord.AutoSize = true;
            this.lPassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPassWord.Location = new System.Drawing.Point(51, 32);
            this.lPassWord.Margin = new System.Windows.Forms.Padding(3);
            this.lPassWord.Name = "lPassWord";
            this.lPassWord.Padding = new System.Windows.Forms.Padding(3);
            this.lPassWord.Size = new System.Drawing.Size(81, 23);
            this.lPassWord.TabIndex = 2;
            this.lPassWord.Text = "密码";
            // 
            // lUserName
            // 
            this.lUserName.AutoSize = true;
            this.lUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUserName.Location = new System.Drawing.Point(51, 3);
            this.lUserName.Margin = new System.Windows.Forms.Padding(3);
            this.lUserName.Name = "lUserName";
            this.lUserName.Padding = new System.Windows.Forms.Padding(3);
            this.lUserName.Size = new System.Drawing.Size(81, 23);
            this.lUserName.TabIndex = 0;
            this.lUserName.Text = "用户名";
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserName.Location = new System.Drawing.Point(135, 0);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 23);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.WordWrap = false;
            // 
            // cbIsExpiry
            // 
            this.cbIsExpiry.AutoSize = true;
            this.cbIsExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsExpiry.Location = new System.Drawing.Point(51, 61);
            this.cbIsExpiry.Name = "cbIsExpiry";
            this.cbIsExpiry.Padding = new System.Windows.Forms.Padding(3);
            this.cbIsExpiry.Size = new System.Drawing.Size(81, 27);
            this.cbIsExpiry.TabIndex = 4;
            this.cbIsExpiry.Text = "到期时间";
            this.cbIsExpiry.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbIsExpiry.UseVisualStyleBackColor = true;
            this.cbIsExpiry.CheckedChanged += new System.EventHandler(this.cbExpiryTime_CheckedChanged);
            // 
            // dtpExpiryTime
            // 
            this.dtpExpiryTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpExpiryTime.Location = new System.Drawing.Point(135, 58);
            this.dtpExpiryTime.Margin = new System.Windows.Forms.Padding(0);
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            this.dtpExpiryTime.Size = new System.Drawing.Size(200, 23);
            this.dtpExpiryTime.TabIndex = 5;
            // 
            // Proxy_AccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.tlpProxyAccount);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_AccountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "账号";
            this.tlpProxyAccount.ResumeLayout(false);
            this.tlpProxyAccount.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpAccountInfo.ResumeLayout(false);
            this.tlpAccountInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProxyAccount;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TableLayoutPanel tlpAccountInfo;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.Label lPassWord;
        private System.Windows.Forms.Label lUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.CheckBox cbIsExpiry;
        private System.Windows.Forms.DateTimePicker dtpExpiryTime;
        private System.Windows.Forms.CheckBox cbIsEnable;
    }
}
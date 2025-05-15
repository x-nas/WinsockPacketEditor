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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountForm));
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
            resources.ApplyResources(this.tlpProxyAccount, "tlpProxyAccount");
            this.tlpProxyAccount.Controls.Add(this.cbIsEnable, 0, 0);
            this.tlpProxyAccount.Controls.Add(this.tlpButton, 0, 2);
            this.tlpProxyAccount.Controls.Add(this.tlpAccountInfo, 0, 1);
            this.tlpProxyAccount.Name = "tlpProxyAccount";
            // 
            // cbIsEnable
            // 
            resources.ApplyResources(this.cbIsEnable, "cbIsEnable");
            this.cbIsEnable.Name = "cbIsEnable";
            this.cbIsEnable.UseVisualStyleBackColor = true;
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bCancel, 3, 1);
            this.tlpButton.Name = "tlpButton";
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bCancel
            // 
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tlpAccountInfo
            // 
            resources.ApplyResources(this.tlpAccountInfo, "tlpAccountInfo");
            this.tlpAccountInfo.Controls.Add(this.txtPassWord, 2, 2);
            this.tlpAccountInfo.Controls.Add(this.lPassWord, 1, 2);
            this.tlpAccountInfo.Controls.Add(this.lUserName, 1, 1);
            this.tlpAccountInfo.Controls.Add(this.txtUserName, 2, 1);
            this.tlpAccountInfo.Controls.Add(this.cbIsExpiry, 1, 3);
            this.tlpAccountInfo.Controls.Add(this.dtpExpiryTime, 2, 3);
            this.tlpAccountInfo.Name = "tlpAccountInfo";
            // 
            // txtPassWord
            // 
            this.txtPassWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtPassWord, "txtPassWord");
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.UseSystemPasswordChar = true;
            // 
            // lPassWord
            // 
            resources.ApplyResources(this.lPassWord, "lPassWord");
            this.lPassWord.Name = "lPassWord";
            // 
            // lUserName
            // 
            resources.ApplyResources(this.lUserName, "lUserName");
            this.lUserName.Name = "lUserName";
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtUserName, "txtUserName");
            this.txtUserName.Name = "txtUserName";
            // 
            // cbIsExpiry
            // 
            resources.ApplyResources(this.cbIsExpiry, "cbIsExpiry");
            this.cbIsExpiry.Name = "cbIsExpiry";
            this.cbIsExpiry.UseVisualStyleBackColor = true;
            this.cbIsExpiry.CheckedChanged += new System.EventHandler(this.cbExpiryTime_CheckedChanged);
            // 
            // dtpExpiryTime
            // 
            resources.ApplyResources(this.dtpExpiryTime, "dtpExpiryTime");
            this.dtpExpiryTime.Name = "dtpExpiryTime";
            // 
            // Proxy_AccountForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpProxyAccount);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_AccountForm";
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
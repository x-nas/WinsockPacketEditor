namespace WPELibrary
{
    partial class Socket_PasswordFrom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_PasswordFrom));
            this.tlpSocketPasswordFrom = new System.Windows.Forms.TableLayoutPanel();
            this.rtbShowInfo = new System.Windows.Forms.RichTextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.tlpSocketPasswordFrom_Button = new System.Windows.Forms.TableLayoutPanel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.tlpSocketPasswordFrom.SuspendLayout();
            this.tlpSocketPasswordFrom_Button.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSocketPasswordFrom
            // 
            resources.ApplyResources(this.tlpSocketPasswordFrom, "tlpSocketPasswordFrom");
            this.tlpSocketPasswordFrom.Controls.Add(this.rtbShowInfo, 1, 1);
            this.tlpSocketPasswordFrom.Controls.Add(this.txtPassword, 1, 2);
            this.tlpSocketPasswordFrom.Controls.Add(this.tlpSocketPasswordFrom_Button, 1, 3);
            this.tlpSocketPasswordFrom.Name = "tlpSocketPasswordFrom";
            // 
            // rtbShowInfo
            // 
            this.rtbShowInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.rtbShowInfo, "rtbShowInfo");
            this.rtbShowInfo.Name = "rtbShowInfo";
            this.rtbShowInfo.ReadOnly = true;
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // tlpSocketPasswordFrom_Button
            // 
            resources.ApplyResources(this.tlpSocketPasswordFrom_Button, "tlpSocketPasswordFrom_Button");
            this.tlpSocketPasswordFrom_Button.Controls.Add(this.bCancel, 3, 0);
            this.tlpSocketPasswordFrom_Button.Controls.Add(this.bOK, 1, 0);
            this.tlpSocketPasswordFrom_Button.Name = "tlpSocketPasswordFrom_Button";
            // 
            // bCancel
            // 
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // Socket_PasswordFrom
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSocketPasswordFrom);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_PasswordFrom";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Socket_PasswordFrom_FormClosing);
            this.tlpSocketPasswordFrom.ResumeLayout(false);
            this.tlpSocketPasswordFrom.PerformLayout();
            this.tlpSocketPasswordFrom_Button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSocketPasswordFrom;
        private System.Windows.Forms.RichTextBox rtbShowInfo;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TableLayoutPanel tlpSocketPasswordFrom_Button;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
    }
}
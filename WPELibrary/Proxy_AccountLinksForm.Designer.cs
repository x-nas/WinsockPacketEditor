namespace WPELibrary
{
    partial class Proxy_AccountLinksForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountLinksForm));
            this.tlpAccountTime = new System.Windows.Forms.TableLayoutPanel();
            this.lAccountCNT = new System.Windows.Forms.Label();
            this.tlpAccountTime_TimeType = new System.Windows.Forms.TableLayoutPanel();
            this.cbIsLimitLinks = new System.Windows.Forms.CheckBox();
            this.nudLimitLinks = new System.Windows.Forms.NumericUpDown();
            this.tlpAccountTime_Button = new System.Windows.Forms.TableLayoutPanel();
            this.bSure = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tlpAccountTime.SuspendLayout();
            this.tlpAccountTime_TimeType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitLinks)).BeginInit();
            this.tlpAccountTime_Button.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpAccountTime
            // 
            resources.ApplyResources(this.tlpAccountTime, "tlpAccountTime");
            this.tlpAccountTime.Controls.Add(this.lAccountCNT, 0, 1);
            this.tlpAccountTime.Controls.Add(this.tlpAccountTime_TimeType, 0, 2);
            this.tlpAccountTime.Controls.Add(this.tlpAccountTime_Button, 0, 3);
            this.tlpAccountTime.Name = "tlpAccountTime";
            // 
            // lAccountCNT
            // 
            resources.ApplyResources(this.lAccountCNT, "lAccountCNT");
            this.lAccountCNT.Name = "lAccountCNT";
            // 
            // tlpAccountTime_TimeType
            // 
            resources.ApplyResources(this.tlpAccountTime_TimeType, "tlpAccountTime_TimeType");
            this.tlpAccountTime_TimeType.Controls.Add(this.cbIsLimitLinks, 1, 1);
            this.tlpAccountTime_TimeType.Controls.Add(this.nudLimitLinks, 2, 1);
            this.tlpAccountTime_TimeType.Name = "tlpAccountTime_TimeType";
            // 
            // cbIsLimitLinks
            // 
            resources.ApplyResources(this.cbIsLimitLinks, "cbIsLimitLinks");
            this.cbIsLimitLinks.Name = "cbIsLimitLinks";
            this.cbIsLimitLinks.UseVisualStyleBackColor = true;
            this.cbIsLimitLinks.CheckedChanged += new System.EventHandler(this.cbIsLimitLinks_CheckedChanged);
            // 
            // nudLimitLinks
            // 
            resources.ApplyResources(this.nudLimitLinks, "nudLimitLinks");
            this.nudLimitLinks.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLimitLinks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimitLinks.Name = "nudLimitLinks";
            this.nudLimitLinks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tlpAccountTime_Button
            // 
            resources.ApplyResources(this.tlpAccountTime_Button, "tlpAccountTime_Button");
            this.tlpAccountTime_Button.Controls.Add(this.bSure, 1, 0);
            this.tlpAccountTime_Button.Controls.Add(this.bCancel, 3, 0);
            this.tlpAccountTime_Button.Name = "tlpAccountTime_Button";
            // 
            // bSure
            // 
            resources.ApplyResources(this.bSure, "bSure");
            this.bSure.Name = "bSure";
            this.bSure.UseVisualStyleBackColor = true;
            this.bSure.Click += new System.EventHandler(this.bSure_Click);
            // 
            // bCancel
            // 
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // Proxy_AccountLinksForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpAccountTime);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_AccountLinksForm";
            this.tlpAccountTime.ResumeLayout(false);
            this.tlpAccountTime.PerformLayout();
            this.tlpAccountTime_TimeType.ResumeLayout(false);
            this.tlpAccountTime_TimeType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitLinks)).EndInit();
            this.tlpAccountTime_Button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpAccountTime;
        private System.Windows.Forms.Label lAccountCNT;
        private System.Windows.Forms.TableLayoutPanel tlpAccountTime_TimeType;
        private System.Windows.Forms.NumericUpDown nudLimitLinks;
        private System.Windows.Forms.TableLayoutPanel tlpAccountTime_Button;
        private System.Windows.Forms.Button bSure;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cbIsLimitLinks;
    }
}
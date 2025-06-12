namespace WPELibrary
{
    partial class Proxy_MapRemoteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_MapRemoteForm));
            this.bCancel = new System.Windows.Forms.Button();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSure = new System.Windows.Forms.Button();
            this.cbbProtocol_From = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPath_From = new System.Windows.Forms.TextBox();
            this.lPath_From = new System.Windows.Forms.Label();
            this.lPort_From = new System.Windows.Forms.Label();
            this.lProtocol_From = new System.Windows.Forms.Label();
            this.lHost_From = new System.Windows.Forms.Label();
            this.txtHost_From = new System.Windows.Forms.TextBox();
            this.nudPort_From = new System.Windows.Forms.NumericUpDown();
            this.gbRemote_From = new System.Windows.Forms.GroupBox();
            this.tlpMapLocal = new System.Windows.Forms.TableLayoutPanel();
            this.gbRemote_To = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbbProtocol_To = new System.Windows.Forms.ComboBox();
            this.txtPath_To = new System.Windows.Forms.TextBox();
            this.lPath_To = new System.Windows.Forms.Label();
            this.lPort_To = new System.Windows.Forms.Label();
            this.lProtocol_To = new System.Windows.Forms.Label();
            this.lHost_To = new System.Windows.Forms.Label();
            this.txtHost_To = new System.Windows.Forms.TextBox();
            this.nudPort_To = new System.Windows.Forms.NumericUpDown();
            this.tlpButton.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort_From)).BeginInit();
            this.gbRemote_From.SuspendLayout();
            this.tlpMapLocal.SuspendLayout();
            this.gbRemote_To.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort_To)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bSure, 1, 0);
            this.tlpButton.Controls.Add(this.bCancel, 3, 0);
            this.tlpButton.Name = "tlpButton";
            // 
            // bSure
            // 
            resources.ApplyResources(this.bSure, "bSure");
            this.bSure.Name = "bSure";
            this.bSure.UseVisualStyleBackColor = true;
            this.bSure.Click += new System.EventHandler(this.bSure_Click);
            // 
            // cbbProtocol_From
            // 
            resources.ApplyResources(this.cbbProtocol_From, "cbbProtocol_From");
            this.cbbProtocol_From.FormattingEnabled = true;
            this.cbbProtocol_From.Items.AddRange(new object[] {
            resources.GetString("cbbProtocol_From.Items")});
            this.cbbProtocol_From.Name = "cbbProtocol_From";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.cbbProtocol_From, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPath_From, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lPath_From, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lPort_From, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lProtocol_From, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lHost_From, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtHost_From, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.nudPort_From, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // txtPath_From
            // 
            resources.ApplyResources(this.txtPath_From, "txtPath_From");
            this.txtPath_From.Name = "txtPath_From";
            // 
            // lPath_From
            // 
            resources.ApplyResources(this.lPath_From, "lPath_From");
            this.lPath_From.Name = "lPath_From";
            // 
            // lPort_From
            // 
            resources.ApplyResources(this.lPort_From, "lPort_From");
            this.lPort_From.Name = "lPort_From";
            // 
            // lProtocol_From
            // 
            resources.ApplyResources(this.lProtocol_From, "lProtocol_From");
            this.lProtocol_From.Name = "lProtocol_From";
            // 
            // lHost_From
            // 
            resources.ApplyResources(this.lHost_From, "lHost_From");
            this.lHost_From.Name = "lHost_From";
            // 
            // txtHost_From
            // 
            resources.ApplyResources(this.txtHost_From, "txtHost_From");
            this.txtHost_From.Name = "txtHost_From";
            // 
            // nudPort_From
            // 
            resources.ApplyResources(this.nudPort_From, "nudPort_From");
            this.nudPort_From.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudPort_From.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPort_From.Name = "nudPort_From";
            this.nudPort_From.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // gbRemote_From
            // 
            resources.ApplyResources(this.gbRemote_From, "gbRemote_From");
            this.gbRemote_From.Controls.Add(this.tableLayoutPanel1);
            this.gbRemote_From.Name = "gbRemote_From";
            this.gbRemote_From.TabStop = false;
            // 
            // tlpMapLocal
            // 
            resources.ApplyResources(this.tlpMapLocal, "tlpMapLocal");
            this.tlpMapLocal.Controls.Add(this.gbRemote_To, 0, 1);
            this.tlpMapLocal.Controls.Add(this.gbRemote_From, 0, 0);
            this.tlpMapLocal.Controls.Add(this.tlpButton, 0, 2);
            this.tlpMapLocal.Name = "tlpMapLocal";
            // 
            // gbRemote_To
            // 
            resources.ApplyResources(this.gbRemote_To, "gbRemote_To");
            this.gbRemote_To.Controls.Add(this.tableLayoutPanel2);
            this.gbRemote_To.Name = "gbRemote_To";
            this.gbRemote_To.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.cbbProtocol_To, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtPath_To, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.lPath_To, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.lPort_To, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lProtocol_To, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lHost_To, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtHost_To, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.nudPort_To, 1, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // cbbProtocol_To
            // 
            resources.ApplyResources(this.cbbProtocol_To, "cbbProtocol_To");
            this.cbbProtocol_To.FormattingEnabled = true;
            this.cbbProtocol_To.Items.AddRange(new object[] {
            resources.GetString("cbbProtocol_To.Items"),
            resources.GetString("cbbProtocol_To.Items1")});
            this.cbbProtocol_To.Name = "cbbProtocol_To";
            // 
            // txtPath_To
            // 
            resources.ApplyResources(this.txtPath_To, "txtPath_To");
            this.txtPath_To.Name = "txtPath_To";
            // 
            // lPath_To
            // 
            resources.ApplyResources(this.lPath_To, "lPath_To");
            this.lPath_To.Name = "lPath_To";
            // 
            // lPort_To
            // 
            resources.ApplyResources(this.lPort_To, "lPort_To");
            this.lPort_To.Name = "lPort_To";
            // 
            // lProtocol_To
            // 
            resources.ApplyResources(this.lProtocol_To, "lProtocol_To");
            this.lProtocol_To.Name = "lProtocol_To";
            // 
            // lHost_To
            // 
            resources.ApplyResources(this.lHost_To, "lHost_To");
            this.lHost_To.Name = "lHost_To";
            // 
            // txtHost_To
            // 
            resources.ApplyResources(this.txtHost_To, "txtHost_To");
            this.txtHost_To.Name = "txtHost_To";
            // 
            // nudPort_To
            // 
            resources.ApplyResources(this.nudPort_To, "nudPort_To");
            this.nudPort_To.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudPort_To.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPort_To.Name = "nudPort_To";
            this.nudPort_To.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // Proxy_MapRemoteForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpMapLocal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_MapRemoteForm";
            this.Load += new System.EventHandler(this.Proxy_MapRemoteForm_Load);
            this.tlpButton.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort_From)).EndInit();
            this.gbRemote_From.ResumeLayout(false);
            this.tlpMapLocal.ResumeLayout(false);
            this.gbRemote_To.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort_To)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bSure;
        private System.Windows.Forms.ComboBox cbbProtocol_From;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtPath_From;
        private System.Windows.Forms.Label lPath_From;
        private System.Windows.Forms.Label lPort_From;
        private System.Windows.Forms.Label lProtocol_From;
        private System.Windows.Forms.Label lHost_From;
        private System.Windows.Forms.TextBox txtHost_From;
        private System.Windows.Forms.NumericUpDown nudPort_From;
        private System.Windows.Forms.GroupBox gbRemote_From;
        private System.Windows.Forms.TableLayoutPanel tlpMapLocal;
        private System.Windows.Forms.GroupBox gbRemote_To;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cbbProtocol_To;
        private System.Windows.Forms.TextBox txtPath_To;
        private System.Windows.Forms.Label lPath_To;
        private System.Windows.Forms.Label lPort_To;
        private System.Windows.Forms.Label lProtocol_To;
        private System.Windows.Forms.Label lHost_To;
        private System.Windows.Forms.TextBox txtHost_To;
        private System.Windows.Forms.NumericUpDown nudPort_To;
    }
}
namespace WPELibrary
{
    partial class Proxy_MapLocalForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_MapLocalForm));
            this.tlpMapLocal = new System.Windows.Forms.TableLayoutPanel();
            this.gbLocal = new System.Windows.Forms.GroupBox();
            this.tlpLocal = new System.Windows.Forms.TableLayoutPanel();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.bLocalPath = new System.Windows.Forms.Button();
            this.gbRemote = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbbProtocol = new System.Windows.Forms.ComboBox();
            this.txtRemotePath = new System.Windows.Forms.TextBox();
            this.lRemotePath = new System.Windows.Forms.Label();
            this.lPort = new System.Windows.Forms.Label();
            this.lProtocol = new System.Windows.Forms.Label();
            this.lHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSure = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tlpMapLocal.SuspendLayout();
            this.gbLocal.SuspendLayout();
            this.tlpLocal.SuspendLayout();
            this.gbRemote.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMapLocal
            // 
            resources.ApplyResources(this.tlpMapLocal, "tlpMapLocal");
            this.tlpMapLocal.Controls.Add(this.gbLocal, 0, 1);
            this.tlpMapLocal.Controls.Add(this.gbRemote, 0, 0);
            this.tlpMapLocal.Controls.Add(this.tlpButton, 0, 2);
            this.tlpMapLocal.Name = "tlpMapLocal";
            // 
            // gbLocal
            // 
            resources.ApplyResources(this.gbLocal, "gbLocal");
            this.gbLocal.Controls.Add(this.tlpLocal);
            this.gbLocal.Name = "gbLocal";
            this.gbLocal.TabStop = false;
            // 
            // tlpLocal
            // 
            resources.ApplyResources(this.tlpLocal, "tlpLocal");
            this.tlpLocal.Controls.Add(this.txtLocalPath, 0, 0);
            this.tlpLocal.Controls.Add(this.bLocalPath, 2, 0);
            this.tlpLocal.Name = "tlpLocal";
            // 
            // txtLocalPath
            // 
            resources.ApplyResources(this.txtLocalPath, "txtLocalPath");
            this.txtLocalPath.Name = "txtLocalPath";
            // 
            // bLocalPath
            // 
            resources.ApplyResources(this.bLocalPath, "bLocalPath");
            this.bLocalPath.Name = "bLocalPath";
            this.bLocalPath.UseVisualStyleBackColor = true;
            this.bLocalPath.Click += new System.EventHandler(this.bLocalPath_Click);
            // 
            // gbRemote
            // 
            resources.ApplyResources(this.gbRemote, "gbRemote");
            this.gbRemote.Controls.Add(this.tableLayoutPanel1);
            this.gbRemote.Name = "gbRemote";
            this.gbRemote.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.cbbProtocol, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtRemotePath, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lRemotePath, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lPort, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lProtocol, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lHost, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtHost, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.nudPort, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // cbbProtocol
            // 
            resources.ApplyResources(this.cbbProtocol, "cbbProtocol");
            this.cbbProtocol.FormattingEnabled = true;
            this.cbbProtocol.Items.AddRange(new object[] {
            resources.GetString("cbbProtocol.Items")});
            this.cbbProtocol.Name = "cbbProtocol";
            // 
            // txtRemotePath
            // 
            resources.ApplyResources(this.txtRemotePath, "txtRemotePath");
            this.txtRemotePath.Name = "txtRemotePath";
            // 
            // lRemotePath
            // 
            resources.ApplyResources(this.lRemotePath, "lRemotePath");
            this.lRemotePath.Name = "lRemotePath";
            // 
            // lPort
            // 
            resources.ApplyResources(this.lPort, "lPort");
            this.lPort.Name = "lPort";
            // 
            // lProtocol
            // 
            resources.ApplyResources(this.lProtocol, "lProtocol");
            this.lProtocol.Name = "lProtocol";
            // 
            // lHost
            // 
            resources.ApplyResources(this.lHost, "lHost");
            this.lHost.Name = "lHost";
            // 
            // txtHost
            // 
            resources.ApplyResources(this.txtHost, "txtHost");
            this.txtHost.Name = "txtHost";
            // 
            // nudPort
            // 
            resources.ApplyResources(this.nudPort, "nudPort");
            this.nudPort.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
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
            // bCancel
            // 
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // Proxy_MapLocalForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpMapLocal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_MapLocalForm";
            this.Load += new System.EventHandler(this.Proxy_MapLocalForm_Load);
            this.tlpMapLocal.ResumeLayout(false);
            this.gbLocal.ResumeLayout(false);
            this.tlpLocal.ResumeLayout(false);
            this.tlpLocal.PerformLayout();
            this.gbRemote.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.tlpButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMapLocal;
        private System.Windows.Forms.GroupBox gbRemote;
        private System.Windows.Forms.GroupBox gbLocal;
        private System.Windows.Forms.TableLayoutPanel tlpLocal;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.Button bLocalPath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lProtocol;
        private System.Windows.Forms.Label lHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtRemotePath;
        private System.Windows.Forms.Label lRemotePath;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bSure;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ComboBox cbbProtocol;
    }
}
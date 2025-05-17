namespace WinsockPacketEditor
{
    partial class SystemMode_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemMode_Form));
            this.tlpSystemMode = new System.Windows.Forms.TableLayoutPanel();
            this.tlpModeSelect = new System.Windows.Forms.TableLayoutPanel();
            this.bProcess_Start = new System.Windows.Forms.Button();
            this.bProxy_Start = new System.Windows.Forms.Button();
            this.msSystem = new System.Windows.Forms.MenuStrip();
            this.tsmiLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChinese = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.gbRemoteMGT = new System.Windows.Forms.GroupBox();
            this.tlpRemoteMGT = new System.Windows.Forms.TableLayoutPanel();
            this.cbIsRemote = new System.Windows.Forms.CheckBox();
            this.tlpRemoteInfo = new System.Windows.Forms.TableLayoutPanel();
            this.nudRemote_Port = new System.Windows.Forms.NumericUpDown();
            this.lRemote_Port = new System.Windows.Forms.Label();
            this.txtRemote_PassWord = new System.Windows.Forms.TextBox();
            this.lRemote_PassWord = new System.Windows.Forms.Label();
            this.txtRemote_UserName = new System.Windows.Forms.TextBox();
            this.lRemote_UserName = new System.Windows.Forms.Label();
            this.tlpRemoteURL = new System.Windows.Forms.TableLayoutPanel();
            this.lRemoteURL = new System.Windows.Forms.LinkLabel();
            this.lRemoteMGT = new System.Windows.Forms.Label();
            this.tlpSystemMode.SuspendLayout();
            this.tlpModeSelect.SuspendLayout();
            this.msSystem.SuspendLayout();
            this.gbRemoteMGT.SuspendLayout();
            this.tlpRemoteMGT.SuspendLayout();
            this.tlpRemoteInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemote_Port)).BeginInit();
            this.tlpRemoteURL.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSystemMode
            // 
            resources.ApplyResources(this.tlpSystemMode, "tlpSystemMode");
            this.tlpSystemMode.Controls.Add(this.tlpModeSelect, 0, 2);
            this.tlpSystemMode.Controls.Add(this.msSystem, 0, 0);
            this.tlpSystemMode.Controls.Add(this.gbRemoteMGT, 0, 3);
            this.tlpSystemMode.Name = "tlpSystemMode";
            // 
            // tlpModeSelect
            // 
            resources.ApplyResources(this.tlpModeSelect, "tlpModeSelect");
            this.tlpModeSelect.Controls.Add(this.bProcess_Start, 3, 0);
            this.tlpModeSelect.Controls.Add(this.bProxy_Start, 1, 0);
            this.tlpModeSelect.Name = "tlpModeSelect";
            // 
            // bProcess_Start
            // 
            resources.ApplyResources(this.bProcess_Start, "bProcess_Start");
            this.bProcess_Start.Name = "bProcess_Start";
            this.bProcess_Start.UseVisualStyleBackColor = true;
            this.bProcess_Start.Click += new System.EventHandler(this.bProcess_Start_Click);
            // 
            // bProxy_Start
            // 
            resources.ApplyResources(this.bProxy_Start, "bProxy_Start");
            this.bProxy_Start.Name = "bProxy_Start";
            this.bProxy_Start.UseVisualStyleBackColor = true;
            this.bProxy_Start.Click += new System.EventHandler(this.bProxy_Start_Click);
            // 
            // msSystem
            // 
            this.msSystem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLanguage,
            this.tsmiAbout});
            resources.ApplyResources(this.msSystem, "msSystem");
            this.msSystem.Name = "msSystem";
            // 
            // tsmiLanguage
            // 
            this.tsmiLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChinese,
            this.tsmiEnglish});
            this.tsmiLanguage.Image = global::WinsockPacketEditor.Properties.Resources.Language;
            this.tsmiLanguage.Name = "tsmiLanguage";
            resources.ApplyResources(this.tsmiLanguage, "tsmiLanguage");
            // 
            // tsmiChinese
            // 
            resources.ApplyResources(this.tsmiChinese, "tsmiChinese");
            this.tsmiChinese.Name = "tsmiChinese";
            this.tsmiChinese.Click += new System.EventHandler(this.tsmiChinese_Click);
            // 
            // tsmiEnglish
            // 
            resources.ApplyResources(this.tsmiEnglish, "tsmiEnglish");
            this.tsmiEnglish.Name = "tsmiEnglish";
            this.tsmiEnglish.Click += new System.EventHandler(this.tsmiEnglish_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Image = global::WinsockPacketEditor.Properties.Resources.help;
            this.tsmiAbout.Name = "tsmiAbout";
            resources.ApplyResources(this.tsmiAbout, "tsmiAbout");
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // gbRemoteMGT
            // 
            this.gbRemoteMGT.Controls.Add(this.tlpRemoteMGT);
            resources.ApplyResources(this.gbRemoteMGT, "gbRemoteMGT");
            this.gbRemoteMGT.Name = "gbRemoteMGT";
            this.gbRemoteMGT.TabStop = false;
            // 
            // tlpRemoteMGT
            // 
            resources.ApplyResources(this.tlpRemoteMGT, "tlpRemoteMGT");
            this.tlpRemoteMGT.Controls.Add(this.cbIsRemote, 0, 0);
            this.tlpRemoteMGT.Controls.Add(this.tlpRemoteInfo, 0, 1);
            this.tlpRemoteMGT.Controls.Add(this.tlpRemoteURL, 0, 2);
            this.tlpRemoteMGT.Name = "tlpRemoteMGT";
            // 
            // cbIsRemote
            // 
            resources.ApplyResources(this.cbIsRemote, "cbIsRemote");
            this.cbIsRemote.Name = "cbIsRemote";
            this.cbIsRemote.UseVisualStyleBackColor = true;
            this.cbIsRemote.Click += new System.EventHandler(this.cbIsRemote_Click);
            // 
            // tlpRemoteInfo
            // 
            resources.ApplyResources(this.tlpRemoteInfo, "tlpRemoteInfo");
            this.tlpRemoteInfo.Controls.Add(this.nudRemote_Port, 2, 2);
            this.tlpRemoteInfo.Controls.Add(this.lRemote_Port, 1, 2);
            this.tlpRemoteInfo.Controls.Add(this.txtRemote_PassWord, 2, 1);
            this.tlpRemoteInfo.Controls.Add(this.lRemote_PassWord, 1, 1);
            this.tlpRemoteInfo.Controls.Add(this.txtRemote_UserName, 2, 0);
            this.tlpRemoteInfo.Controls.Add(this.lRemote_UserName, 1, 0);
            this.tlpRemoteInfo.Name = "tlpRemoteInfo";
            // 
            // nudRemote_Port
            // 
            resources.ApplyResources(this.nudRemote_Port, "nudRemote_Port");
            this.nudRemote_Port.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudRemote_Port.Name = "nudRemote_Port";
            this.nudRemote_Port.Value = new decimal(new int[] {
            89,
            0,
            0,
            0});
            // 
            // lRemote_Port
            // 
            resources.ApplyResources(this.lRemote_Port, "lRemote_Port");
            this.lRemote_Port.Name = "lRemote_Port";
            // 
            // txtRemote_PassWord
            // 
            resources.ApplyResources(this.txtRemote_PassWord, "txtRemote_PassWord");
            this.txtRemote_PassWord.Name = "txtRemote_PassWord";
            this.txtRemote_PassWord.UseSystemPasswordChar = true;
            // 
            // lRemote_PassWord
            // 
            resources.ApplyResources(this.lRemote_PassWord, "lRemote_PassWord");
            this.lRemote_PassWord.Name = "lRemote_PassWord";
            // 
            // txtRemote_UserName
            // 
            resources.ApplyResources(this.txtRemote_UserName, "txtRemote_UserName");
            this.txtRemote_UserName.Name = "txtRemote_UserName";
            // 
            // lRemote_UserName
            // 
            resources.ApplyResources(this.lRemote_UserName, "lRemote_UserName");
            this.lRemote_UserName.Name = "lRemote_UserName";
            // 
            // tlpRemoteURL
            // 
            resources.ApplyResources(this.tlpRemoteURL, "tlpRemoteURL");
            this.tlpRemoteURL.Controls.Add(this.lRemoteURL, 1, 0);
            this.tlpRemoteURL.Controls.Add(this.lRemoteMGT, 0, 0);
            this.tlpRemoteURL.Name = "tlpRemoteURL";
            // 
            // lRemoteURL
            // 
            resources.ApplyResources(this.lRemoteURL, "lRemoteURL");
            this.lRemoteURL.Name = "lRemoteURL";
            this.lRemoteURL.TabStop = true;
            this.lRemoteURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lRemoteURL_LinkClicked);
            // 
            // lRemoteMGT
            // 
            resources.ApplyResources(this.lRemoteMGT, "lRemoteMGT");
            this.lRemoteMGT.Name = "lRemoteMGT";
            // 
            // SystemMode_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSystemMode);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.msSystem;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemMode_Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SystemMode_Form_FormClosing);
            this.Load += new System.EventHandler(this.SystemMode_Form_Load);
            this.tlpSystemMode.ResumeLayout(false);
            this.tlpSystemMode.PerformLayout();
            this.tlpModeSelect.ResumeLayout(false);
            this.msSystem.ResumeLayout(false);
            this.msSystem.PerformLayout();
            this.gbRemoteMGT.ResumeLayout(false);
            this.tlpRemoteMGT.ResumeLayout(false);
            this.tlpRemoteMGT.PerformLayout();
            this.tlpRemoteInfo.ResumeLayout(false);
            this.tlpRemoteInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemote_Port)).EndInit();
            this.tlpRemoteURL.ResumeLayout(false);
            this.tlpRemoteURL.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSystemMode;
        private System.Windows.Forms.MenuStrip msSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiLanguage;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.TableLayoutPanel tlpModeSelect;
        private System.Windows.Forms.Button bProcess_Start;
        private System.Windows.Forms.Button bProxy_Start;
        private System.Windows.Forms.ToolStripMenuItem tsmiChinese;
        private System.Windows.Forms.ToolStripMenuItem tsmiEnglish;
        private System.Windows.Forms.GroupBox gbRemoteMGT;
        private System.Windows.Forms.TableLayoutPanel tlpRemoteMGT;
        private System.Windows.Forms.CheckBox cbIsRemote;
        private System.Windows.Forms.TableLayoutPanel tlpRemoteInfo;
        private System.Windows.Forms.Label lRemote_UserName;
        private System.Windows.Forms.TextBox txtRemote_UserName;
        private System.Windows.Forms.Label lRemote_PassWord;
        private System.Windows.Forms.TextBox txtRemote_PassWord;
        private System.Windows.Forms.Label lRemote_Port;
        private System.Windows.Forms.NumericUpDown nudRemote_Port;
        private System.Windows.Forms.TableLayoutPanel tlpRemoteURL;
        private System.Windows.Forms.LinkLabel lRemoteURL;
        private System.Windows.Forms.Label lRemoteMGT;
    }
}
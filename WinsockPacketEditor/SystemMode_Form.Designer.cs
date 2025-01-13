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
            this.tlpSystemMode.SuspendLayout();
            this.tlpModeSelect.SuspendLayout();
            this.msSystem.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSystemMode
            // 
            resources.ApplyResources(this.tlpSystemMode, "tlpSystemMode");
            this.tlpSystemMode.Controls.Add(this.tlpModeSelect, 0, 2);
            this.tlpSystemMode.Controls.Add(this.msSystem, 0, 0);
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
            this.Load += new System.EventHandler(this.SystemMode_Form_Load);
            this.tlpSystemMode.ResumeLayout(false);
            this.tlpSystemMode.PerformLayout();
            this.tlpModeSelect.ResumeLayout(false);
            this.msSystem.ResumeLayout(false);
            this.msSystem.PerformLayout();
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
    }
}
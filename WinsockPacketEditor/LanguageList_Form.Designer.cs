namespace WinsockPacketEditor
{
    partial class LanguageList_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageList_Form));
            this.tlpLanguage = new System.Windows.Forms.TableLayoutPanel();
            this.rb_enUS = new System.Windows.Forms.RadioButton();
            this.rb_zhCN = new System.Windows.Forms.RadioButton();
            this.pb_zhCN = new System.Windows.Forms.PictureBox();
            this.pb_enUS = new System.Windows.Forms.PictureBox();
            this.tlpLanguage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zhCN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_enUS)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpLanguage
            // 
            resources.ApplyResources(this.tlpLanguage, "tlpLanguage");
            this.tlpLanguage.Controls.Add(this.rb_enUS, 1, 1);
            this.tlpLanguage.Controls.Add(this.rb_zhCN, 1, 0);
            this.tlpLanguage.Controls.Add(this.pb_zhCN, 0, 0);
            this.tlpLanguage.Controls.Add(this.pb_enUS, 0, 1);
            this.tlpLanguage.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpLanguage.Name = "tlpLanguage";
            // 
            // rb_enUS
            // 
            resources.ApplyResources(this.rb_enUS, "rb_enUS");
            this.rb_enUS.Name = "rb_enUS";
            this.rb_enUS.TabStop = true;
            this.rb_enUS.UseVisualStyleBackColor = true;
            // 
            // rb_zhCN
            // 
            resources.ApplyResources(this.rb_zhCN, "rb_zhCN");
            this.rb_zhCN.Name = "rb_zhCN";
            this.rb_zhCN.TabStop = true;
            this.rb_zhCN.UseVisualStyleBackColor = true;
            // 
            // pb_zhCN
            // 
            resources.ApplyResources(this.pb_zhCN, "pb_zhCN");
            this.pb_zhCN.Image = global::WinsockPacketEditor.Properties.Resources.zhCN;
            this.pb_zhCN.Name = "pb_zhCN";
            this.pb_zhCN.TabStop = false;
            // 
            // pb_enUS
            // 
            resources.ApplyResources(this.pb_enUS, "pb_enUS");
            this.pb_enUS.Image = global::WinsockPacketEditor.Properties.Resources.enUS;
            this.pb_enUS.Name = "pb_enUS";
            this.pb_enUS.TabStop = false;
            // 
            // LanguageList_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpLanguage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LanguageList_Form";
            this.ShowIcon = false;
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LanguageList_Form_FormClosed);
            this.tlpLanguage.ResumeLayout(false);
            this.tlpLanguage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zhCN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_enUS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLanguage;
        private System.Windows.Forms.RadioButton rb_enUS;
        private System.Windows.Forms.RadioButton rb_zhCN;
        private System.Windows.Forms.PictureBox pb_zhCN;
        private System.Windows.Forms.PictureBox pb_enUS;
    }
}
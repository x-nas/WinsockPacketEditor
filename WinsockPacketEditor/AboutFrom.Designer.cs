namespace WinsockPacketEditor
{
    partial class AboutFrom
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutFrom));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.tlpAbout = new System.Windows.Forms.TableLayoutPanel();
            this.llGitee = new System.Windows.Forms.LinkLabel();
            this.llGitHub = new System.Windows.Forms.LinkLabel();
            this.llSetup = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.tlpAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.tlpAbout, 1, 4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // logoPictureBox
            // 
            resources.ApplyResources(this.logoPictureBox, "logoPictureBox");
            this.logoPictureBox.Name = "logoPictureBox";
            this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
            this.logoPictureBox.TabStop = false;
            // 
            // labelProductName
            // 
            resources.ApplyResources(this.labelProductName, "labelProductName");
            this.labelProductName.Name = "labelProductName";
            // 
            // labelVersion
            // 
            resources.ApplyResources(this.labelVersion, "labelVersion");
            this.labelVersion.Name = "labelVersion";
            // 
            // labelCopyright
            // 
            resources.ApplyResources(this.labelCopyright, "labelCopyright");
            this.labelCopyright.Name = "labelCopyright";
            // 
            // labelCompanyName
            // 
            resources.ApplyResources(this.labelCompanyName, "labelCompanyName");
            this.labelCompanyName.Name = "labelCompanyName";
            // 
            // tlpAbout
            // 
            resources.ApplyResources(this.tlpAbout, "tlpAbout");
            this.tlpAbout.Controls.Add(this.llGitee, 1, 2);
            this.tlpAbout.Controls.Add(this.llGitHub, 1, 1);
            this.tlpAbout.Controls.Add(this.llSetup, 1, 0);
            this.tlpAbout.Name = "tlpAbout";
            // 
            // llGitee
            // 
            resources.ApplyResources(this.llGitee, "llGitee");
            this.llGitee.LinkColor = System.Drawing.Color.Green;
            this.llGitee.Name = "llGitee";
            this.llGitee.TabStop = true;
            this.llGitee.VisitedLinkColor = System.Drawing.Color.Green;
            this.llGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llGitee_LinkClicked);
            // 
            // llGitHub
            // 
            resources.ApplyResources(this.llGitHub, "llGitHub");
            this.llGitHub.LinkColor = System.Drawing.Color.Green;
            this.llGitHub.Name = "llGitHub";
            this.llGitHub.TabStop = true;
            this.llGitHub.VisitedLinkColor = System.Drawing.Color.Green;
            this.llGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llGitHub_LinkClicked);
            // 
            // llSetup
            // 
            resources.ApplyResources(this.llSetup, "llSetup");
            this.llSetup.LinkColor = System.Drawing.Color.Green;
            this.llSetup.Name = "llSetup";
            this.llSetup.TabStop = true;
            this.llSetup.VisitedLinkColor = System.Drawing.Color.Green;
            this.llSetup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSetup_LinkClicked);
            // 
            // AboutFrom
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutFrom";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.tlpAbout.ResumeLayout(false);
            this.tlpAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.TableLayoutPanel tlpAbout;
        private System.Windows.Forms.LinkLabel llSetup;
        private System.Windows.Forms.LinkLabel llGitee;
        private System.Windows.Forms.LinkLabel llGitHub;
    }
}

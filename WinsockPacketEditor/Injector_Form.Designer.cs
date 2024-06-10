
namespace WinsockPacketEditor
{
    partial class Injector_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Injector_Form));
            this.pTop = new System.Windows.Forms.Panel();
            this.tlpProcessInject = new System.Windows.Forms.TableLayoutPanel();
            this.lProcessName = new System.Windows.Forms.Label();
            this.tbProcessID = new System.Windows.Forms.TextBox();
            this.bSelectProcess = new System.Windows.Forms.Button();
            this.bInject = new System.Windows.Forms.Button();
            this.pbLanguage = new System.Windows.Forms.PictureBox();
            this.pbAbout = new System.Windows.Forms.PictureBox();
            this.pFill = new System.Windows.Forms.Panel();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pTop.SuspendLayout();
            this.tlpProcessInject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).BeginInit();
            this.pFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.tlpProcessInject);
            resources.ApplyResources(this.pTop, "pTop");
            this.pTop.Name = "pTop";
            // 
            // tlpProcessInject
            // 
            resources.ApplyResources(this.tlpProcessInject, "tlpProcessInject");
            this.tlpProcessInject.Controls.Add(this.lProcessName, 0, 1);
            this.tlpProcessInject.Controls.Add(this.tbProcessID, 1, 1);
            this.tlpProcessInject.Controls.Add(this.bSelectProcess, 2, 1);
            this.tlpProcessInject.Controls.Add(this.bInject, 3, 1);
            this.tlpProcessInject.Controls.Add(this.pbLanguage, 4, 1);
            this.tlpProcessInject.Controls.Add(this.pbAbout, 5, 1);
            this.tlpProcessInject.Name = "tlpProcessInject";
            // 
            // lProcessName
            // 
            resources.ApplyResources(this.lProcessName, "lProcessName");
            this.lProcessName.Name = "lProcessName";
            // 
            // tbProcessID
            // 
            resources.ApplyResources(this.tbProcessID, "tbProcessID");
            this.tbProcessID.Name = "tbProcessID";
            // 
            // bSelectProcess
            // 
            resources.ApplyResources(this.bSelectProcess, "bSelectProcess");
            this.bSelectProcess.Name = "bSelectProcess";
            this.bSelectProcess.UseVisualStyleBackColor = true;
            this.bSelectProcess.Click += new System.EventHandler(this.bSelectProcess_Click);
            // 
            // bInject
            // 
            resources.ApplyResources(this.bInject, "bInject");
            this.bInject.Name = "bInject";
            this.bInject.UseVisualStyleBackColor = true;
            this.bInject.Click += new System.EventHandler(this.bInject_Click);
            // 
            // pbLanguage
            // 
            resources.ApplyResources(this.pbLanguage, "pbLanguage");
            this.pbLanguage.Image = global::WinsockPacketEditor.Properties.Resources.Language;
            this.pbLanguage.Name = "pbLanguage";
            this.pbLanguage.TabStop = false;
            this.pbLanguage.Click += new System.EventHandler(this.pbLanguage_Click);
            // 
            // pbAbout
            // 
            resources.ApplyResources(this.pbAbout, "pbAbout");
            this.pbAbout.Name = "pbAbout";
            this.pbAbout.TabStop = false;
            this.pbAbout.Click += new System.EventHandler(this.pbAbout_Click);
            // 
            // pFill
            // 
            this.pFill.Controls.Add(this.rtbLog);
            resources.ApplyResources(this.pFill, "pFill");
            this.pFill.Name = "pFill";
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.rtbLog, "rtbLog");
            this.rtbLog.ForeColor = System.Drawing.Color.LawnGreen;
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            // 
            // Injector_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pFill);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Injector_Form";
            this.pTop.ResumeLayout(false);
            this.tlpProcessInject.ResumeLayout(false);
            this.tlpProcessInject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).EndInit();
            this.pFill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pFill;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.TableLayoutPanel tlpProcessInject;
        private System.Windows.Forms.Label lProcessName;
        private System.Windows.Forms.TextBox tbProcessID;
        private System.Windows.Forms.Button bSelectProcess;
        private System.Windows.Forms.Button bInject;
        private System.Windows.Forms.PictureBox pbLanguage;
        private System.Windows.Forms.PictureBox pbAbout;
    }
}



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
            this.tlpInjectorForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlpProcessInject = new System.Windows.Forms.TableLayoutPanel();
            this.lProcessName = new System.Windows.Forms.Label();
            this.tbProcessID = new System.Windows.Forms.TextBox();
            this.bSelectProcess = new System.Windows.Forms.Button();
            this.bInject = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.tlpInjectorForm.SuspendLayout();
            this.tlpProcessInject.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpInjectorForm
            // 
            resources.ApplyResources(this.tlpInjectorForm, "tlpInjectorForm");
            this.tlpInjectorForm.Controls.Add(this.tlpProcessInject, 0, 1);
            this.tlpInjectorForm.Controls.Add(this.rtbLog, 0, 2);
            this.tlpInjectorForm.Name = "tlpInjectorForm";
            // 
            // tlpProcessInject
            // 
            resources.ApplyResources(this.tlpProcessInject, "tlpProcessInject");
            this.tlpProcessInject.Controls.Add(this.lProcessName, 0, 0);
            this.tlpProcessInject.Controls.Add(this.tbProcessID, 1, 0);
            this.tlpProcessInject.Controls.Add(this.bSelectProcess, 2, 0);
            this.tlpProcessInject.Controls.Add(this.bInject, 3, 0);
            this.tlpProcessInject.Name = "tlpProcessInject";
            // 
            // lProcessName
            // 
            resources.ApplyResources(this.lProcessName, "lProcessName");
            this.lProcessName.Name = "lProcessName";
            // 
            // tbProcessID
            // 
            this.tbProcessID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.tbProcessID, "tbProcessID");
            this.tbProcessID.Name = "tbProcessID";
            this.tbProcessID.ReadOnly = true;
            // 
            // bSelectProcess
            // 
            resources.ApplyResources(this.bSelectProcess, "bSelectProcess");
            this.bSelectProcess.Image = global::WinsockPacketEditor.Properties.Resources.searchbox_button;
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
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            resources.ApplyResources(this.rtbLog, "rtbLog");
            this.rtbLog.ForeColor = System.Drawing.Color.LawnGreen;
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbLog_LinkClicked);
            // 
            // Injector_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpInjectorForm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Injector_Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Injector_Form_FormClosing);
            this.tlpInjectorForm.ResumeLayout(false);
            this.tlpProcessInject.ResumeLayout(false);
            this.tlpProcessInject.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpInjectorForm;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.TableLayoutPanel tlpProcessInject;
        private System.Windows.Forms.Label lProcessName;
        private System.Windows.Forms.TextBox tbProcessID;
        private System.Windows.Forms.Button bSelectProcess;
        private System.Windows.Forms.Button bInject;
    }
}


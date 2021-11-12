
namespace ProcessInjector
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
            this.bInject = new System.Windows.Forms.Button();
            this.bSelectProcess = new System.Windows.Forms.Button();
            this.tbProcessID = new System.Windows.Forms.TextBox();
            this.lProcessName = new System.Windows.Forms.Label();
            this.pFill = new System.Windows.Forms.Panel();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pTop.SuspendLayout();
            this.pFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.bInject);
            this.pTop.Controls.Add(this.bSelectProcess);
            this.pTop.Controls.Add(this.tbProcessID);
            this.pTop.Controls.Add(this.lProcessName);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(432, 45);
            this.pTop.TabIndex = 0;
            // 
            // bInject
            // 
            this.bInject.Location = new System.Drawing.Point(352, 9);
            this.bInject.Name = "bInject";
            this.bInject.Size = new System.Drawing.Size(66, 30);
            this.bInject.TabIndex = 3;
            this.bInject.Text = "注 入";
            this.bInject.UseVisualStyleBackColor = true;
            this.bInject.Click += new System.EventHandler(this.bInject_Click);
            // 
            // bSelectProcess
            // 
            this.bSelectProcess.Location = new System.Drawing.Point(300, 12);
            this.bSelectProcess.Name = "bSelectProcess";
            this.bSelectProcess.Size = new System.Drawing.Size(37, 25);
            this.bSelectProcess.TabIndex = 2;
            this.bSelectProcess.Text = ". . .";
            this.bSelectProcess.UseVisualStyleBackColor = true;
            this.bSelectProcess.Click += new System.EventHandler(this.bSelectProcess_Click);
            // 
            // tbProcessID
            // 
            this.tbProcessID.Location = new System.Drawing.Point(73, 13);
            this.tbProcessID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbProcessID.Name = "tbProcessID";
            this.tbProcessID.Size = new System.Drawing.Size(221, 23);
            this.tbProcessID.TabIndex = 1;
            // 
            // lProcessName
            // 
            this.lProcessName.AutoSize = true;
            this.lProcessName.Location = new System.Drawing.Point(5, 16);
            this.lProcessName.Name = "lProcessName";
            this.lProcessName.Size = new System.Drawing.Size(68, 17);
            this.lProcessName.TabIndex = 0;
            this.lProcessName.Text = "进程名称：";
            // 
            // pFill
            // 
            this.pFill.Controls.Add(this.rtbLog);
            this.pFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFill.Location = new System.Drawing.Point(0, 45);
            this.pFill.Name = "pFill";
            this.pFill.Size = new System.Drawing.Size(432, 125);
            this.pFill.TabIndex = 1;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.ForeColor = System.Drawing.Color.LawnGreen;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(432, 125);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // Injector_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(432, 170);
            this.Controls.Add(this.pFill);
            this.Controls.Add(this.pTop);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Injector_Form";
            this.Text = "进程注入器";
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pFill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Button bSelectProcess;
        private System.Windows.Forms.TextBox tbProcessID;
        private System.Windows.Forms.Label lProcessName;
        private System.Windows.Forms.Panel pFill;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button bInject;
    }
}


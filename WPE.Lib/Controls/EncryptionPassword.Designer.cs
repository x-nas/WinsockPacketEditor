namespace WPE.Lib.Controls
{
    partial class EncryptionPassword
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpEncryption = new System.Windows.Forms.TableLayoutPanel();
            this.txtEncryption = new AntdUI.Input();
            this.pEncryption = new AntdUI.Panel();
            this.lEncryption = new AntdUI.Label();
            this.tlpEncryption.SuspendLayout();
            this.pEncryption.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpEncryption
            // 
            this.tlpEncryption.ColumnCount = 1;
            this.tlpEncryption.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEncryption.Controls.Add(this.txtEncryption, 0, 1);
            this.tlpEncryption.Controls.Add(this.pEncryption, 0, 0);
            this.tlpEncryption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEncryption.Location = new System.Drawing.Point(0, 0);
            this.tlpEncryption.Margin = new System.Windows.Forms.Padding(0);
            this.tlpEncryption.Name = "tlpEncryption";
            this.tlpEncryption.RowCount = 3;
            this.tlpEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpEncryption.Size = new System.Drawing.Size(500, 200);
            this.tlpEncryption.TabIndex = 1;
            // 
            // txtEncryption
            // 
            this.txtEncryption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEncryption.Location = new System.Drawing.Point(5, 135);
            this.txtEncryption.Margin = new System.Windows.Forms.Padding(5);
            this.txtEncryption.Name = "txtEncryption";
            this.txtEncryption.PlaceholderText = "请输入字符或数字";
            this.txtEncryption.Size = new System.Drawing.Size(490, 40);
            this.txtEncryption.TabIndex = 1;
            this.txtEncryption.TextChanged += new System.EventHandler(this.txtEncryption_TextChanged);
            // 
            // pEncryption
            // 
            this.pEncryption.ArrowAlign = AntdUI.TAlign.TL;
            this.pEncryption.ArrowSize = 10;
            this.pEncryption.Controls.Add(this.lEncryption);
            this.pEncryption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEncryption.Location = new System.Drawing.Point(0, 0);
            this.pEncryption.Margin = new System.Windows.Forms.Padding(0);
            this.pEncryption.Name = "pEncryption";
            this.pEncryption.Radius = 10;
            this.pEncryption.Shadow = 24;
            this.pEncryption.ShadowOpacityAnimation = true;
            this.pEncryption.Size = new System.Drawing.Size(500, 130);
            this.pEncryption.TabIndex = 2;
            // 
            // lEncryption
            // 
            this.lEncryption.BackColor = System.Drawing.Color.Transparent;
            this.lEncryption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lEncryption.Location = new System.Drawing.Point(24, 24);
            this.lEncryption.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lEncryption.Name = "lEncryption";
            this.lEncryption.Size = new System.Drawing.Size(452, 82);
            this.lEncryption.TabIndex = 4;
            this.lEncryption.Text = "lEncryption";
            this.lEncryption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EncryptionPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpEncryption);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "EncryptionPassword";
            this.Size = new System.Drawing.Size(500, 200);
            this.tlpEncryption.ResumeLayout(false);
            this.pEncryption.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpEncryption;
        private AntdUI.Input txtEncryption;
        private AntdUI.Panel pEncryption;
        private AntdUI.Label lEncryption;
    }
}

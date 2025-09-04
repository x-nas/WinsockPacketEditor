namespace WinsockPacketEditor
{
    partial class AccountLocation
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
            this.tlpLocation = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bExit = new AntdUI.Button();
            this.tLocation = new AntdUI.Table();
            this.tlpLocation.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLocation
            // 
            this.tlpLocation.ColumnCount = 1;
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLocation.Controls.Add(this.tlpButton, 0, 1);
            this.tlpLocation.Controls.Add(this.tLocation, 0, 0);
            this.tlpLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLocation.Location = new System.Drawing.Point(0, 0);
            this.tlpLocation.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLocation.Name = "tlpLocation";
            this.tlpLocation.RowCount = 2;
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpLocation.Size = new System.Drawing.Size(1000, 700);
            this.tlpLocation.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 3;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bExit, 1, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 640);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(1000, 60);
            this.tlpButton.TabIndex = 4;
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(456, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(87, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tLocation
            // 
            this.tLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLocation.EmptyHeader = true;
            this.tLocation.Gap = 12;
            this.tLocation.Location = new System.Drawing.Point(3, 3);
            this.tLocation.Name = "tLocation";
            this.tLocation.Size = new System.Drawing.Size(994, 634);
            this.tLocation.TabIndex = 0;
            this.tLocation.Text = "table1";
            // 
            // AccountLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLocation);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AccountLocation";
            this.Size = new System.Drawing.Size(1000, 700);
            this.Load += new System.EventHandler(this.AccountLocation_Load);
            this.tlpLocation.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLocation;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bExit;
        private AntdUI.Table tLocation;
    }
}

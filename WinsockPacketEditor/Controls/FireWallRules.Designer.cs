namespace WinsockPacketEditor
{
    partial class FireWallRules
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
            this.tlpFireWallRules = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbAutoClear_Expiry = new AntdUI.Checkbox();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.cbAutoBlock_UnSupport = new AntdUI.Checkbox();
            this.tlpFireWallRules.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFireWallRules
            // 
            this.tlpFireWallRules.ColumnCount = 1;
            this.tlpFireWallRules.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallRules.Controls.Add(this.cbAutoClear_Expiry, 0, 1);
            this.tlpFireWallRules.Controls.Add(this.tlpButton, 0, 3);
            this.tlpFireWallRules.Controls.Add(this.cbAutoBlock_UnSupport, 0, 0);
            this.tlpFireWallRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFireWallRules.Location = new System.Drawing.Point(0, 0);
            this.tlpFireWallRules.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFireWallRules.Name = "tlpFireWallRules";
            this.tlpFireWallRules.RowCount = 4;
            this.tlpFireWallRules.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRules.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRules.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallRules.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallRules.Size = new System.Drawing.Size(400, 200);
            this.tlpFireWallRules.TabIndex = 0;
            // 
            // cbAutoClear_Expiry
            // 
            this.cbAutoClear_Expiry.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbAutoClear_Expiry.LocalizationText = "FireWallSetting.AutoClear.Expiry";
            this.cbAutoClear_Expiry.Location = new System.Drawing.Point(3, 41);
            this.cbAutoClear_Expiry.Name = "cbAutoClear_Expiry";
            this.cbAutoClear_Expiry.Size = new System.Drawing.Size(183, 32);
            this.cbAutoClear_Expiry.TabIndex = 7;
            this.cbAutoClear_Expiry.Text = "自动清理 - 已过期的 IP 地址";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 150);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(400, 50);
            this.tlpButton.TabIndex = 5;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(128, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 37);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(209, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // cbAutoBlock_UnSupport
            // 
            this.cbAutoBlock_UnSupport.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbAutoBlock_UnSupport.LocalizationText = "FireWallSetting.AutoBlock.UnSupport";
            this.cbAutoBlock_UnSupport.Location = new System.Drawing.Point(3, 3);
            this.cbAutoBlock_UnSupport.Name = "cbAutoBlock_UnSupport";
            this.cbAutoBlock_UnSupport.Size = new System.Drawing.Size(261, 32);
            this.cbAutoBlock_UnSupport.TabIndex = 6;
            this.cbAutoBlock_UnSupport.Text = "自动屏蔽 ( 30分钟 ) - 不支持的 Socks 协议";
            // 
            // FireWallRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFireWallRules);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FireWallRules";
            this.Size = new System.Drawing.Size(400, 200);
            this.Load += new System.EventHandler(this.FireWallRules_Load);
            this.tlpFireWallRules.ResumeLayout(false);
            this.tlpFireWallRules.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFireWallRules;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Checkbox cbAutoBlock_UnSupport;
        private AntdUI.Checkbox cbAutoClear_Expiry;
    }
}

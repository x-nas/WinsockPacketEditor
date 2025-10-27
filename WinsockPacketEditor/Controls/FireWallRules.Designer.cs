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
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpFireWallRulesInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbAutoWhiteList_AuthSuccess = new AntdUI.Checkbox();
            this.lAutoWhiteList = new AntdUI.Label();
            this.lAutoClear = new AntdUI.Label();
            this.nudAutoBlackList_UnSupport = new AntdUI.InputNumber();
            this.cbAutoBlackList_UnSupport = new AntdUI.Checkbox();
            this.cbAutoBlackList_AuthFail = new AntdUI.Checkbox();
            this.cbAutoClear_Expiry = new AntdUI.Checkbox();
            this.lAutoBlackList = new AntdUI.Label();
            this.tlpFireWallRules.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpFireWallRulesInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFireWallRules
            // 
            this.tlpFireWallRules.ColumnCount = 1;
            this.tlpFireWallRules.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallRules.Controls.Add(this.tlpButton, 0, 1);
            this.tlpFireWallRules.Controls.Add(this.tlpFireWallRulesInfo, 0, 0);
            this.tlpFireWallRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFireWallRules.Location = new System.Drawing.Point(0, 0);
            this.tlpFireWallRules.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFireWallRules.Name = "tlpFireWallRules";
            this.tlpFireWallRules.RowCount = 2;
            this.tlpFireWallRules.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallRules.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallRules.Size = new System.Drawing.Size(350, 350);
            this.tlpFireWallRules.TabIndex = 0;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 300);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(350, 50);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(103, 6);
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
            this.bExit.Location = new System.Drawing.Point(184, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpFireWallRulesInfo
            // 
            this.tlpFireWallRulesInfo.ColumnCount = 2;
            this.tlpFireWallRulesInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallRulesInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpFireWallRulesInfo.Controls.Add(this.cbAutoWhiteList_AuthSuccess, 0, 1);
            this.tlpFireWallRulesInfo.Controls.Add(this.lAutoWhiteList, 0, 0);
            this.tlpFireWallRulesInfo.Controls.Add(this.lAutoClear, 0, 5);
            this.tlpFireWallRulesInfo.Controls.Add(this.nudAutoBlackList_UnSupport, 1, 2);
            this.tlpFireWallRulesInfo.Controls.Add(this.cbAutoBlackList_UnSupport, 0, 4);
            this.tlpFireWallRulesInfo.Controls.Add(this.cbAutoBlackList_AuthFail, 0, 3);
            this.tlpFireWallRulesInfo.Controls.Add(this.cbAutoClear_Expiry, 0, 6);
            this.tlpFireWallRulesInfo.Controls.Add(this.lAutoBlackList, 0, 2);
            this.tlpFireWallRulesInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFireWallRulesInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpFireWallRulesInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFireWallRulesInfo.Name = "tlpFireWallRulesInfo";
            this.tlpFireWallRulesInfo.RowCount = 8;
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFireWallRulesInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallRulesInfo.Size = new System.Drawing.Size(350, 300);
            this.tlpFireWallRulesInfo.TabIndex = 1;
            // 
            // cbAutoWhiteList_AuthSuccess
            // 
            this.cbAutoWhiteList_AuthSuccess.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbAutoWhiteList_AuthSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAutoWhiteList_AuthSuccess.LocalizationText = "FireWallSetting.AutoWhiteList.AuthSuccess";
            this.cbAutoWhiteList_AuthSuccess.Location = new System.Drawing.Point(3, 40);
            this.cbAutoWhiteList_AuthSuccess.Name = "cbAutoWhiteList_AuthSuccess";
            this.cbAutoWhiteList_AuthSuccess.Size = new System.Drawing.Size(134, 32);
            this.cbAutoWhiteList_AuthSuccess.TabIndex = 18;
            this.cbAutoWhiteList_AuthSuccess.Text = "认证成功的 IP 地址";
            // 
            // lAutoWhiteList
            // 
            this.lAutoWhiteList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAutoWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAutoWhiteList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAutoWhiteList.LocalizationText = "FireWallSetting.AutoWhiteList";
            this.lAutoWhiteList.Location = new System.Drawing.Point(3, 3);
            this.lAutoWhiteList.Name = "lAutoWhiteList";
            this.lAutoWhiteList.Size = new System.Drawing.Size(92, 31);
            this.lAutoWhiteList.TabIndex = 16;
            this.lAutoWhiteList.Text = "自动加入白名单 :";
            // 
            // lAutoClear
            // 
            this.lAutoClear.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAutoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAutoClear.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAutoClear.LocalizationText = "FireWallSetting.AutoClear";
            this.lAutoClear.Location = new System.Drawing.Point(3, 191);
            this.lAutoClear.Name = "lAutoClear";
            this.lAutoClear.Size = new System.Drawing.Size(56, 30);
            this.lAutoClear.TabIndex = 15;
            this.lAutoClear.Text = "自动清理 :";
            // 
            // nudAutoBlackList_UnSupport
            // 
            this.nudAutoBlackList_UnSupport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudAutoBlackList_UnSupport.LocalizationSuffixText = "Minutes";
            this.nudAutoBlackList_UnSupport.Location = new System.Drawing.Point(231, 76);
            this.nudAutoBlackList_UnSupport.Margin = new System.Windows.Forms.Padding(1);
            this.nudAutoBlackList_UnSupport.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudAutoBlackList_UnSupport.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAutoBlackList_UnSupport.Name = "nudAutoBlackList_UnSupport";
            this.nudAutoBlackList_UnSupport.PrefixText = "";
            this.nudAutoBlackList_UnSupport.Size = new System.Drawing.Size(118, 35);
            this.nudAutoBlackList_UnSupport.SuffixText = "分钟";
            this.nudAutoBlackList_UnSupport.TabIndex = 13;
            this.nudAutoBlackList_UnSupport.Text = "30";
            this.nudAutoBlackList_UnSupport.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAutoBlackList_UnSupport.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // cbAutoBlackList_UnSupport
            // 
            this.cbAutoBlackList_UnSupport.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbAutoBlackList_UnSupport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAutoBlackList_UnSupport.LocalizationText = "FireWallSetting.AutoBlackList.UnSupport";
            this.cbAutoBlackList_UnSupport.Location = new System.Drawing.Point(3, 153);
            this.cbAutoBlackList_UnSupport.Name = "cbAutoBlackList_UnSupport";
            this.cbAutoBlackList_UnSupport.Size = new System.Drawing.Size(144, 32);
            this.cbAutoBlackList_UnSupport.TabIndex = 11;
            this.cbAutoBlackList_UnSupport.Text = "不支持的 Socks 协议";
            // 
            // cbAutoBlackList_AuthFail
            // 
            this.cbAutoBlackList_AuthFail.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbAutoBlackList_AuthFail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAutoBlackList_AuthFail.LocalizationText = "FireWallSetting.AutoBlackList.AuthFail";
            this.cbAutoBlackList_AuthFail.Location = new System.Drawing.Point(3, 115);
            this.cbAutoBlackList_AuthFail.Name = "cbAutoBlackList_AuthFail";
            this.cbAutoBlackList_AuthFail.Size = new System.Drawing.Size(134, 32);
            this.cbAutoBlackList_AuthFail.TabIndex = 9;
            this.cbAutoBlackList_AuthFail.Text = "认证失败的 IP 地址";
            // 
            // cbAutoClear_Expiry
            // 
            this.cbAutoClear_Expiry.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbAutoClear_Expiry.LocalizationText = "FireWallSetting.AutoClear.Expiry";
            this.cbAutoClear_Expiry.Location = new System.Drawing.Point(3, 227);
            this.cbAutoClear_Expiry.Name = "cbAutoClear_Expiry";
            this.cbAutoClear_Expiry.Size = new System.Drawing.Size(122, 32);
            this.cbAutoClear_Expiry.TabIndex = 7;
            this.cbAutoClear_Expiry.Text = "已过期的 IP 地址";
            // 
            // lAutoBlackList
            // 
            this.lAutoBlackList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lAutoBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lAutoBlackList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lAutoBlackList.LocalizationText = "FireWallSetting.AutoBlackList";
            this.lAutoBlackList.Location = new System.Drawing.Point(3, 78);
            this.lAutoBlackList.Name = "lAutoBlackList";
            this.lAutoBlackList.Size = new System.Drawing.Size(92, 31);
            this.lAutoBlackList.TabIndex = 14;
            this.lAutoBlackList.Text = "自动加入黑名单 :";
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
            this.Size = new System.Drawing.Size(350, 350);
            this.Load += new System.EventHandler(this.FireWallRules_Load);
            this.tlpFireWallRules.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpFireWallRulesInfo.ResumeLayout(false);
            this.tlpFireWallRulesInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFireWallRules;
        private TableLayoutPanelEx tlpFireWallRulesInfo;
        private AntdUI.InputNumber nudAutoBlackList_UnSupport;
        private AntdUI.Checkbox cbAutoBlackList_UnSupport;
        private AntdUI.Checkbox cbAutoBlackList_AuthFail;
        private AntdUI.Checkbox cbAutoClear_Expiry;
        private AntdUI.Label lAutoBlackList;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Label lAutoClear;
        private AntdUI.Label lAutoWhiteList;
        private AntdUI.Checkbox cbAutoWhiteList_AuthSuccess;
    }
}

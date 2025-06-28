namespace WPE.InjectMode
{
    partial class BackUpSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackUpSettingsForm));
            this.tlpBackUpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.dSystemConfig = new AntdUI.Divider();
            this.tlpBackUpContent = new System.Windows.Forms.TableLayoutPanel();
            this.cbBackUp_ProxySet = new AntdUI.Checkbox();
            this.cbBackUp_ProxyMapping = new AntdUI.Checkbox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bImport = new AntdUI.Button();
            this.bExport = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dProxyMode = new AntdUI.Divider();
            this.dInjectMode = new AntdUI.Divider();
            this.cbBackUp_SystemConfig = new AntdUI.Checkbox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbBackUp_InjectSet = new AntdUI.Checkbox();
            this.cbBackUp_FilterList = new AntdUI.Checkbox();
            this.cbBackUp_SendList = new AntdUI.Checkbox();
            this.cbBackUp_RobotList = new AntdUI.Checkbox();
            this.cbBackUp_ProxyAccount = new AntdUI.Checkbox();
            this.tlpBackUpSettings.SuspendLayout();
            this.tlpBackUpContent.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpBackUpSettings
            // 
            this.tlpBackUpSettings.ColumnCount = 1;
            this.tlpBackUpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackUpSettings.Controls.Add(this.tableLayoutPanel1, 0, 11);
            this.tlpBackUpSettings.Controls.Add(this.tlpButton, 0, 11);
            this.tlpBackUpSettings.Controls.Add(this.dSystemConfig, 0, 0);
            this.tlpBackUpSettings.Controls.Add(this.tlpBackUpContent, 0, 6);
            this.tlpBackUpSettings.Controls.Add(this.dProxyMode, 0, 4);
            this.tlpBackUpSettings.Controls.Add(this.dInjectMode, 0, 8);
            this.tlpBackUpSettings.Controls.Add(this.cbBackUp_SystemConfig, 0, 2);
            this.tlpBackUpSettings.Controls.Add(this.tableLayoutPanel2, 0, 10);
            this.tlpBackUpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBackUpSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpBackUpSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBackUpSettings.Name = "tlpBackUpSettings";
            this.tlpBackUpSettings.RowCount = 13;
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpBackUpSettings.Size = new System.Drawing.Size(484, 611);
            this.tlpBackUpSettings.TabIndex = 0;
            // 
            // dSystemConfig
            // 
            this.dSystemConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSystemConfig.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSystemConfig.Location = new System.Drawing.Point(3, 3);
            this.dSystemConfig.Name = "dSystemConfig";
            this.dSystemConfig.Orientation = AntdUI.TOrientation.Left;
            this.dSystemConfig.Size = new System.Drawing.Size(478, 23);
            this.dSystemConfig.TabIndex = 0;
            this.dSystemConfig.Text = "系统运行";
            // 
            // tlpBackUpContent
            // 
            this.tlpBackUpContent.ColumnCount = 2;
            this.tlpBackUpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBackUpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBackUpContent.Controls.Add(this.cbBackUp_ProxyAccount, 1, 0);
            this.tlpBackUpContent.Controls.Add(this.cbBackUp_ProxyMapping, 0, 1);
            this.tlpBackUpContent.Controls.Add(this.cbBackUp_ProxySet, 0, 0);
            this.tlpBackUpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBackUpContent.Location = new System.Drawing.Point(0, 166);
            this.tlpBackUpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBackUpContent.Name = "tlpBackUpContent";
            this.tlpBackUpContent.RowCount = 6;
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackUpContent.Size = new System.Drawing.Size(484, 100);
            this.tlpBackUpContent.TabIndex = 1;
            // 
            // cbBackUp_ProxySet
            // 
            this.cbBackUp_ProxySet.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_ProxySet.Checked = true;
            this.cbBackUp_ProxySet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_ProxySet.Location = new System.Drawing.Point(3, 3);
            this.cbBackUp_ProxySet.Name = "cbBackUp_ProxySet";
            this.cbBackUp_ProxySet.Size = new System.Drawing.Size(138, 42);
            this.cbBackUp_ProxySet.TabIndex = 0;
            this.cbBackUp_ProxySet.Text = "代理模式配置";
            // 
            // cbBackUp_ProxyMapping
            // 
            this.cbBackUp_ProxyMapping.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_ProxyMapping.Checked = true;
            this.cbBackUp_ProxyMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_ProxyMapping.Location = new System.Drawing.Point(3, 51);
            this.cbBackUp_ProxyMapping.Name = "cbBackUp_ProxyMapping";
            this.cbBackUp_ProxyMapping.Size = new System.Drawing.Size(106, 42);
            this.cbBackUp_ProxyMapping.TabIndex = 2;
            this.cbBackUp_ProxyMapping.Text = "代理映射";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 3;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Controls.Add(this.bExit, 1, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 551);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.bImport, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bExport, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 491);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 60);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // bImport
            // 
            this.bImport.BackExtend = "135, #6253E1, #04BEFE";
            this.bImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bImport.IconSvg = "ImportOutlined";
            this.bImport.Location = new System.Drawing.Point(85, 7);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(144, 46);
            this.bImport.TabIndex = 0;
            this.bImport.Text = "导入备份";
            this.bImport.Type = AntdUI.TTypeMini.Primary;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // bExport
            // 
            this.bExport.BackExtend = "135, #6253E1, #04BEFE";
            this.bExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExport.IconSvg = "ExportOutlined";
            this.bExport.Location = new System.Drawing.Point(255, 7);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(144, 46);
            this.bExport.TabIndex = 1;
            this.bExport.Text = "导出备份";
            this.bExport.Type = AntdUI.TTypeMini.Primary;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(185, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dProxyMode
            // 
            this.dProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProxyMode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProxyMode.Location = new System.Drawing.Point(3, 120);
            this.dProxyMode.Name = "dProxyMode";
            this.dProxyMode.Orientation = AntdUI.TOrientation.Left;
            this.dProxyMode.Size = new System.Drawing.Size(478, 23);
            this.dProxyMode.TabIndex = 7;
            this.dProxyMode.Text = "代理模式";
            // 
            // dInjectMode
            // 
            this.dInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dInjectMode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dInjectMode.Location = new System.Drawing.Point(3, 289);
            this.dInjectMode.Name = "dInjectMode";
            this.dInjectMode.Orientation = AntdUI.TOrientation.Left;
            this.dInjectMode.Size = new System.Drawing.Size(478, 23);
            this.dInjectMode.TabIndex = 8;
            this.dInjectMode.Text = "注入模式";
            // 
            // cbBackUp_SystemConfig
            // 
            this.cbBackUp_SystemConfig.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_SystemConfig.Checked = true;
            this.cbBackUp_SystemConfig.Location = new System.Drawing.Point(3, 52);
            this.cbBackUp_SystemConfig.Name = "cbBackUp_SystemConfig";
            this.cbBackUp_SystemConfig.Size = new System.Drawing.Size(138, 42);
            this.cbBackUp_SystemConfig.TabIndex = 9;
            this.cbBackUp_SystemConfig.Text = "系统运行配置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.cbBackUp_RobotList, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbBackUp_SendList, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbBackUp_FilterList, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbBackUp_InjectSet, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 335);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(484, 156);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // cbBackUp_InjectSet
            // 
            this.cbBackUp_InjectSet.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_InjectSet.Checked = true;
            this.cbBackUp_InjectSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_InjectSet.Location = new System.Drawing.Point(3, 3);
            this.cbBackUp_InjectSet.Name = "cbBackUp_InjectSet";
            this.cbBackUp_InjectSet.Size = new System.Drawing.Size(138, 42);
            this.cbBackUp_InjectSet.TabIndex = 1;
            this.cbBackUp_InjectSet.Text = "注入模式配置";
            // 
            // cbBackUp_FilterList
            // 
            this.cbBackUp_FilterList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_FilterList.Checked = true;
            this.cbBackUp_FilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_FilterList.Location = new System.Drawing.Point(245, 3);
            this.cbBackUp_FilterList.Name = "cbBackUp_FilterList";
            this.cbBackUp_FilterList.Size = new System.Drawing.Size(106, 42);
            this.cbBackUp_FilterList.TabIndex = 2;
            this.cbBackUp_FilterList.Text = "滤镜列表";
            // 
            // cbBackUp_SendList
            // 
            this.cbBackUp_SendList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_SendList.Checked = true;
            this.cbBackUp_SendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_SendList.Location = new System.Drawing.Point(3, 51);
            this.cbBackUp_SendList.Name = "cbBackUp_SendList";
            this.cbBackUp_SendList.Size = new System.Drawing.Size(106, 42);
            this.cbBackUp_SendList.TabIndex = 3;
            this.cbBackUp_SendList.Text = "发送列表";
            // 
            // cbBackUp_RobotList
            // 
            this.cbBackUp_RobotList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_RobotList.Checked = true;
            this.cbBackUp_RobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_RobotList.Location = new System.Drawing.Point(245, 51);
            this.cbBackUp_RobotList.Name = "cbBackUp_RobotList";
            this.cbBackUp_RobotList.Size = new System.Drawing.Size(122, 42);
            this.cbBackUp_RobotList.TabIndex = 4;
            this.cbBackUp_RobotList.Text = "机器人列表";
            // 
            // cbBackUp_ProxyAccount
            // 
            this.cbBackUp_ProxyAccount.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbBackUp_ProxyAccount.Checked = true;
            this.cbBackUp_ProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_ProxyAccount.Location = new System.Drawing.Point(245, 3);
            this.cbBackUp_ProxyAccount.Name = "cbBackUp_ProxyAccount";
            this.cbBackUp_ProxyAccount.Size = new System.Drawing.Size(106, 42);
            this.cbBackUp_ProxyAccount.TabIndex = 4;
            this.cbBackUp_ProxyAccount.Text = "代理账号";
            // 
            // BackUpSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 611);
            this.Controls.Add(this.tlpBackUpSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "BackUpSettingsForm";
            this.Text = "BackUpSettingsForm";
            this.Load += new System.EventHandler(this.BackUpSettingsForm_Load);
            this.tlpBackUpSettings.ResumeLayout(false);
            this.tlpBackUpSettings.PerformLayout();
            this.tlpBackUpContent.ResumeLayout(false);
            this.tlpBackUpContent.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBackUpSettings;
        private AntdUI.Divider dSystemConfig;
        private System.Windows.Forms.TableLayoutPanel tlpBackUpContent;
        private AntdUI.Checkbox cbBackUp_ProxyMapping;
        private AntdUI.Checkbox cbBackUp_ProxySet;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Button bImport;
        private AntdUI.Button bExport;
        private AntdUI.Button bExit;
        private AntdUI.Divider dProxyMode;
        private AntdUI.Divider dInjectMode;
        private AntdUI.Checkbox cbBackUp_SystemConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Checkbox cbBackUp_RobotList;
        private AntdUI.Checkbox cbBackUp_SendList;
        private AntdUI.Checkbox cbBackUp_FilterList;
        private AntdUI.Checkbox cbBackUp_InjectSet;
        private AntdUI.Checkbox cbBackUp_ProxyAccount;
    }
}
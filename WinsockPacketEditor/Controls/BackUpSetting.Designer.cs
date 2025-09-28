namespace WinsockPacketEditor
{
    partial class BackUpSetting
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
            this.tlpBackUpSettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dListInfo = new AntdUI.Divider();
            this.tlpListInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbBackUp_RobotList = new AntdUI.Checkbox();
            this.cbBackUp_SendList = new AntdUI.Checkbox();
            this.cbBackUp_FilterList = new AntdUI.Checkbox();
            this.cbBackUp_InjectSet = new AntdUI.Checkbox();
            this.tlpButton2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bImport = new AntdUI.Button();
            this.bExport = new AntdUI.Button();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bExit = new AntdUI.Button();
            this.dSystemConfig = new AntdUI.Divider();
            this.tlpBackUpContent = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbBackUp_ProxyAccount = new AntdUI.Checkbox();
            this.cbBackUp_ProxyMapping = new AntdUI.Checkbox();
            this.cbBackUp_ProxySet = new AntdUI.Checkbox();
            this.dProxyMode = new AntdUI.Divider();
            this.dInjectMode = new AntdUI.Divider();
            this.cbBackUp_SystemConfig = new AntdUI.Checkbox();
            this.tlpBackUpSettings.SuspendLayout();
            this.tlpListInfo.SuspendLayout();
            this.tlpButton2.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpBackUpContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpBackUpSettings
            // 
            this.tlpBackUpSettings.ColumnCount = 1;
            this.tlpBackUpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackUpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpBackUpSettings.Controls.Add(this.dListInfo, 0, 12);
            this.tlpBackUpSettings.Controls.Add(this.tlpListInfo, 0, 14);
            this.tlpBackUpSettings.Controls.Add(this.cbBackUp_InjectSet, 0, 10);
            this.tlpBackUpSettings.Controls.Add(this.tlpButton2, 0, 15);
            this.tlpBackUpSettings.Controls.Add(this.tlpButton, 0, 16);
            this.tlpBackUpSettings.Controls.Add(this.dSystemConfig, 0, 0);
            this.tlpBackUpSettings.Controls.Add(this.tlpBackUpContent, 0, 6);
            this.tlpBackUpSettings.Controls.Add(this.dProxyMode, 0, 4);
            this.tlpBackUpSettings.Controls.Add(this.dInjectMode, 0, 8);
            this.tlpBackUpSettings.Controls.Add(this.cbBackUp_SystemConfig, 0, 2);
            this.tlpBackUpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBackUpSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpBackUpSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBackUpSettings.Name = "tlpBackUpSettings";
            this.tlpBackUpSettings.RowCount = 17;
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpBackUpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpBackUpSettings.Size = new System.Drawing.Size(500, 700);
            this.tlpBackUpSettings.TabIndex = 1;
            // 
            // dListInfo
            // 
            this.dListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dListInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dListInfo.LocalizationText = "BackUpSettingsForm.ListData";
            this.dListInfo.Location = new System.Drawing.Point(2, 324);
            this.dListInfo.Margin = new System.Windows.Forms.Padding(2);
            this.dListInfo.Name = "dListInfo";
            this.dListInfo.Orientation = AntdUI.TOrientation.Left;
            this.dListInfo.Size = new System.Drawing.Size(496, 19);
            this.dListInfo.TabIndex = 13;
            this.dListInfo.Text = "列表数据";
            // 
            // tlpListInfo
            // 
            this.tlpListInfo.ColumnCount = 2;
            this.tlpListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpListInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpListInfo.Controls.Add(this.cbBackUp_RobotList, 0, 1);
            this.tlpListInfo.Controls.Add(this.cbBackUp_SendList, 1, 0);
            this.tlpListInfo.Controls.Add(this.cbBackUp_FilterList, 0, 0);
            this.tlpListInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListInfo.Location = new System.Drawing.Point(0, 361);
            this.tlpListInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpListInfo.Name = "tlpListInfo";
            this.tlpListInfo.RowCount = 3;
            this.tlpListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListInfo.Size = new System.Drawing.Size(500, 241);
            this.tlpListInfo.TabIndex = 12;
            // 
            // cbBackUp_RobotList
            // 
            this.cbBackUp_RobotList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_RobotList.Checked = true;
            this.cbBackUp_RobotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_RobotList.LocalizationText = "RobotList";
            this.cbBackUp_RobotList.Location = new System.Drawing.Point(2, 40);
            this.cbBackUp_RobotList.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_RobotList.Name = "cbBackUp_RobotList";
            this.cbBackUp_RobotList.Size = new System.Drawing.Size(92, 34);
            this.cbBackUp_RobotList.TabIndex = 4;
            this.cbBackUp_RobotList.Text = "机器人列表";
            // 
            // cbBackUp_SendList
            // 
            this.cbBackUp_SendList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_SendList.Checked = true;
            this.cbBackUp_SendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_SendList.LocalizationText = "SendList";
            this.cbBackUp_SendList.Location = new System.Drawing.Point(252, 2);
            this.cbBackUp_SendList.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_SendList.Name = "cbBackUp_SendList";
            this.cbBackUp_SendList.Size = new System.Drawing.Size(80, 34);
            this.cbBackUp_SendList.TabIndex = 3;
            this.cbBackUp_SendList.Text = "发送列表";
            // 
            // cbBackUp_FilterList
            // 
            this.cbBackUp_FilterList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_FilterList.Checked = true;
            this.cbBackUp_FilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_FilterList.LocalizationText = "FilterList";
            this.cbBackUp_FilterList.Location = new System.Drawing.Point(2, 2);
            this.cbBackUp_FilterList.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_FilterList.Name = "cbBackUp_FilterList";
            this.cbBackUp_FilterList.Size = new System.Drawing.Size(80, 34);
            this.cbBackUp_FilterList.TabIndex = 2;
            this.cbBackUp_FilterList.Text = "滤镜列表";
            // 
            // cbBackUp_InjectSet
            // 
            this.cbBackUp_InjectSet.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_InjectSet.Checked = true;
            this.cbBackUp_InjectSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_InjectSet.LocalizationText = "BackUpSettingsForm.InjectMode.Configuration";
            this.cbBackUp_InjectSet.Location = new System.Drawing.Point(2, 270);
            this.cbBackUp_InjectSet.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_InjectSet.Name = "cbBackUp_InjectSet";
            this.cbBackUp_InjectSet.Size = new System.Drawing.Size(104, 34);
            this.cbBackUp_InjectSet.TabIndex = 11;
            this.cbBackUp_InjectSet.Text = "注入模式配置";
            // 
            // tlpButton2
            // 
            this.tlpButton2.ColumnCount = 5;
            this.tlpButton2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton2.Controls.Add(this.bImport, 1, 1);
            this.tlpButton2.Controls.Add(this.bExport, 3, 1);
            this.tlpButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton2.Location = new System.Drawing.Point(0, 602);
            this.tlpButton2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton2.Name = "tlpButton2";
            this.tlpButton2.RowCount = 3;
            this.tlpButton2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton2.Size = new System.Drawing.Size(500, 49);
            this.tlpButton2.TabIndex = 6;
            // 
            // bImport
            // 
            this.bImport.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bImport.BackExtend = "135, #6253E1, #04BEFE";
            this.bImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bImport.IconSvg = "ImportOutlined";
            this.bImport.LocalizationText = "Import";
            this.bImport.Location = new System.Drawing.Point(154, 6);
            this.bImport.Margin = new System.Windows.Forms.Padding(2);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(87, 37);
            this.bImport.TabIndex = 0;
            this.bImport.Text = "导入备份";
            this.bImport.Type = AntdUI.TTypeMini.Primary;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // bExport
            // 
            this.bExport.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExport.BackExtend = "135, #6253E1, #04BEFE";
            this.bExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExport.IconSvg = "ExportOutlined";
            this.bExport.LocalizationText = "Export";
            this.bExport.Location = new System.Drawing.Point(259, 6);
            this.bExport.Margin = new System.Windows.Forms.Padding(2);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(87, 37);
            this.bExport.TabIndex = 1;
            this.bExport.Text = "导出备份";
            this.bExport.Type = AntdUI.TTypeMini.Primary;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 3;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bExit, 1, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 651);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 49);
            this.tlpButton.TabIndex = 5;
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(218, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dSystemConfig
            // 
            this.dSystemConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSystemConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSystemConfig.LocalizationText = "BackUpSettingsForm.SystemConfig";
            this.dSystemConfig.Location = new System.Drawing.Point(2, 2);
            this.dSystemConfig.Margin = new System.Windows.Forms.Padding(2);
            this.dSystemConfig.Name = "dSystemConfig";
            this.dSystemConfig.Orientation = AntdUI.TOrientation.Left;
            this.dSystemConfig.Size = new System.Drawing.Size(496, 19);
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
            this.tlpBackUpContent.Location = new System.Drawing.Point(0, 132);
            this.tlpBackUpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBackUpContent.Name = "tlpBackUpContent";
            this.tlpBackUpContent.RowCount = 6;
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackUpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackUpContent.Size = new System.Drawing.Size(500, 81);
            this.tlpBackUpContent.TabIndex = 1;
            // 
            // cbBackUp_ProxyAccount
            // 
            this.cbBackUp_ProxyAccount.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_ProxyAccount.Checked = true;
            this.cbBackUp_ProxyAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_ProxyAccount.LocalizationText = "BackUpSettingsForm.ProxyAccount";
            this.cbBackUp_ProxyAccount.Location = new System.Drawing.Point(252, 2);
            this.cbBackUp_ProxyAccount.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_ProxyAccount.Name = "cbBackUp_ProxyAccount";
            this.cbBackUp_ProxyAccount.Size = new System.Drawing.Size(80, 34);
            this.cbBackUp_ProxyAccount.TabIndex = 4;
            this.cbBackUp_ProxyAccount.Text = "代理账号";
            // 
            // cbBackUp_ProxyMapping
            // 
            this.cbBackUp_ProxyMapping.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_ProxyMapping.Checked = true;
            this.cbBackUp_ProxyMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_ProxyMapping.LocalizationText = "BackUpSettingsForm.ProxyMapping";
            this.cbBackUp_ProxyMapping.Location = new System.Drawing.Point(2, 40);
            this.cbBackUp_ProxyMapping.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_ProxyMapping.Name = "cbBackUp_ProxyMapping";
            this.cbBackUp_ProxyMapping.Size = new System.Drawing.Size(80, 34);
            this.cbBackUp_ProxyMapping.TabIndex = 2;
            this.cbBackUp_ProxyMapping.Text = "代理映射";
            // 
            // cbBackUp_ProxySet
            // 
            this.cbBackUp_ProxySet.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_ProxySet.Checked = true;
            this.cbBackUp_ProxySet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBackUp_ProxySet.LocalizationText = "BackUpSettingsForm.ProxyMode.Configuration";
            this.cbBackUp_ProxySet.Location = new System.Drawing.Point(2, 2);
            this.cbBackUp_ProxySet.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_ProxySet.Name = "cbBackUp_ProxySet";
            this.cbBackUp_ProxySet.Size = new System.Drawing.Size(104, 34);
            this.cbBackUp_ProxySet.TabIndex = 0;
            this.cbBackUp_ProxySet.Text = "代理模式配置";
            // 
            // dProxyMode
            // 
            this.dProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProxyMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProxyMode.LocalizationText = "BackUpSettingsForm.ProxyMode";
            this.dProxyMode.Location = new System.Drawing.Point(2, 95);
            this.dProxyMode.Margin = new System.Windows.Forms.Padding(2);
            this.dProxyMode.Name = "dProxyMode";
            this.dProxyMode.Orientation = AntdUI.TOrientation.Left;
            this.dProxyMode.Size = new System.Drawing.Size(496, 19);
            this.dProxyMode.TabIndex = 7;
            this.dProxyMode.Text = "代理模式";
            // 
            // dInjectMode
            // 
            this.dInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dInjectMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dInjectMode.LocalizationText = "BackUpSettingsForm.InjectMode";
            this.dInjectMode.Location = new System.Drawing.Point(2, 231);
            this.dInjectMode.Margin = new System.Windows.Forms.Padding(2);
            this.dInjectMode.Name = "dInjectMode";
            this.dInjectMode.Orientation = AntdUI.TOrientation.Left;
            this.dInjectMode.Size = new System.Drawing.Size(496, 19);
            this.dInjectMode.TabIndex = 8;
            this.dInjectMode.Text = "注入模式";
            // 
            // cbBackUp_SystemConfig
            // 
            this.cbBackUp_SystemConfig.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbBackUp_SystemConfig.Checked = true;
            this.cbBackUp_SystemConfig.LocalizationText = "BackUpSettingsForm.SystemOperation";
            this.cbBackUp_SystemConfig.Location = new System.Drawing.Point(2, 41);
            this.cbBackUp_SystemConfig.Margin = new System.Windows.Forms.Padding(2);
            this.cbBackUp_SystemConfig.Name = "cbBackUp_SystemConfig";
            this.cbBackUp_SystemConfig.Size = new System.Drawing.Size(104, 34);
            this.cbBackUp_SystemConfig.TabIndex = 9;
            this.cbBackUp_SystemConfig.Text = "系统运行配置";
            // 
            // BackUpSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpBackUpSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BackUpSetting";
            this.Size = new System.Drawing.Size(500, 700);
            this.Load += new System.EventHandler(this.BackUpSetting_Load);
            this.tlpBackUpSettings.ResumeLayout(false);
            this.tlpBackUpSettings.PerformLayout();
            this.tlpListInfo.ResumeLayout(false);
            this.tlpListInfo.PerformLayout();
            this.tlpButton2.ResumeLayout(false);
            this.tlpButton2.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpBackUpContent.ResumeLayout(false);
            this.tlpBackUpContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpBackUpSettings;
        private AntdUI.Divider dListInfo;
        private TableLayoutPanelEx tlpListInfo;
        private AntdUI.Checkbox cbBackUp_RobotList;
        private AntdUI.Checkbox cbBackUp_SendList;
        private AntdUI.Checkbox cbBackUp_FilterList;
        private AntdUI.Checkbox cbBackUp_InjectSet;
        private TableLayoutPanelEx tlpButton2;
        private AntdUI.Button bImport;
        private AntdUI.Button bExport;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bExit;
        private AntdUI.Divider dSystemConfig;
        private TableLayoutPanelEx tlpBackUpContent;
        private AntdUI.Checkbox cbBackUp_ProxyAccount;
        private AntdUI.Checkbox cbBackUp_ProxyMapping;
        private AntdUI.Checkbox cbBackUp_ProxySet;
        private AntdUI.Divider dProxyMode;
        private AntdUI.Divider dInjectMode;
        private AntdUI.Checkbox cbBackUp_SystemConfig;
    }
}

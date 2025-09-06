namespace WinsockPacketEditor
{
    partial class ListSetting
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            this.tlpListSettings = new System.Windows.Forms.TableLayoutPanel();
            this.dColumn = new AntdUI.Divider();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tabListSettings = new AntdUI.Tabs();
            this.tpInject = new AntdUI.TabPage();
            this.tlpInject = new System.Windows.Forms.TableLayoutPanel();
            this.cbIsShow_PacketData_Inject = new AntdUI.Checkbox();
            this.cbIsShow_PacketLen_Inject = new AntdUI.Checkbox();
            this.cbIsShow_ServerLocation_Inject = new AntdUI.Checkbox();
            this.cbIsShow_ServerAddr_Inject = new AntdUI.Checkbox();
            this.cbIsShow_ClientLocation_Inject = new AntdUI.Checkbox();
            this.cbIsShow_ClientAddr_Inject = new AntdUI.Checkbox();
            this.cbIsShow_PacketSocket_Inject = new AntdUI.Checkbox();
            this.cbIsShow_PacketType_Inject = new AntdUI.Checkbox();
            this.cbIsShow_ProxyTime_Inject = new AntdUI.Checkbox();
            this.cbIsShow_ID_Inject = new AntdUI.Checkbox();
            this.tpProxy = new AntdUI.TabPage();
            this.tlpProxy = new System.Windows.Forms.TableLayoutPanel();
            this.cbIsShow_PacketData_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_PacketLen_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_ServerLocation_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_ServerAddr_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_ClientLocation_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_ClientAddr_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_PacketSocket_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_PacketType_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_ProxyTime_Proxy = new AntdUI.Checkbox();
            this.cbIsShow_ID_Proxy = new AntdUI.Checkbox();
            this.tlpListSettings.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tabListSettings.SuspendLayout();
            this.tpInject.SuspendLayout();
            this.tlpInject.SuspendLayout();
            this.tpProxy.SuspendLayout();
            this.tlpProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpListSettings
            // 
            this.tlpListSettings.ColumnCount = 1;
            this.tlpListSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSettings.Controls.Add(this.dColumn, 0, 0);
            this.tlpListSettings.Controls.Add(this.tlpButton, 0, 3);
            this.tlpListSettings.Controls.Add(this.tabListSettings, 0, 2);
            this.tlpListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpListSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpListSettings.Name = "tlpListSettings";
            this.tlpListSettings.RowCount = 4;
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpListSettings.Size = new System.Drawing.Size(500, 750);
            this.tlpListSettings.TabIndex = 2;
            // 
            // dColumn
            // 
            this.dColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dColumn.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dColumn.LocalizationText = "ListSettingsForm.TitleSetting";
            this.dColumn.Location = new System.Drawing.Point(3, 3);
            this.dColumn.Name = "dColumn";
            this.dColumn.Orientation = AntdUI.TOrientation.Left;
            this.dColumn.Size = new System.Drawing.Size(494, 23);
            this.dColumn.TabIndex = 7;
            this.dColumn.Text = "标题设置";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 690);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 60);
            this.tlpButton.TabIndex = 4;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(155, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(82, 46);
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
            this.bExit.Location = new System.Drawing.Point(263, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(82, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tabListSettings
            // 
            this.tabListSettings.Controls.Add(this.tpInject);
            this.tabListSettings.Controls.Add(this.tpProxy);
            this.tabListSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabListSettings.Location = new System.Drawing.Point(3, 52);
            this.tabListSettings.Name = "tabListSettings";
            this.tabListSettings.Pages.Add(this.tpInject);
            this.tabListSettings.Pages.Add(this.tpProxy);
            this.tabListSettings.Size = new System.Drawing.Size(494, 635);
            this.tabListSettings.Style = styleLine1;
            this.tabListSettings.TabIndex = 8;
            this.tabListSettings.Text = "tabs1";
            // 
            // tpInject
            // 
            this.tpInject.Controls.Add(this.tlpInject);
            this.tpInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpInject.Location = new System.Drawing.Point(0, 36);
            this.tpInject.Name = "tpInject";
            this.tpInject.Size = new System.Drawing.Size(494, 599);
            this.tpInject.TabIndex = 0;
            this.tpInject.Text = "tpInject";
            // 
            // tlpInject
            // 
            this.tlpInject.ColumnCount = 2;
            this.tlpInject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpInject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpInject.Controls.Add(this.cbIsShow_PacketData_Inject, 1, 4);
            this.tlpInject.Controls.Add(this.cbIsShow_PacketLen_Inject, 0, 4);
            this.tlpInject.Controls.Add(this.cbIsShow_ServerLocation_Inject, 1, 3);
            this.tlpInject.Controls.Add(this.cbIsShow_ServerAddr_Inject, 0, 3);
            this.tlpInject.Controls.Add(this.cbIsShow_ClientLocation_Inject, 1, 2);
            this.tlpInject.Controls.Add(this.cbIsShow_ClientAddr_Inject, 0, 2);
            this.tlpInject.Controls.Add(this.cbIsShow_PacketSocket_Inject, 1, 1);
            this.tlpInject.Controls.Add(this.cbIsShow_PacketType_Inject, 0, 1);
            this.tlpInject.Controls.Add(this.cbIsShow_ProxyTime_Inject, 1, 0);
            this.tlpInject.Controls.Add(this.cbIsShow_ID_Inject, 0, 0);
            this.tlpInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpInject.Location = new System.Drawing.Point(0, 0);
            this.tlpInject.Margin = new System.Windows.Forms.Padding(0);
            this.tlpInject.Name = "tlpInject";
            this.tlpInject.RowCount = 6;
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInject.Size = new System.Drawing.Size(494, 599);
            this.tlpInject.TabIndex = 2;
            // 
            // cbIsShow_PacketData_Inject
            // 
            this.cbIsShow_PacketData_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketData_Inject.Checked = true;
            this.cbIsShow_PacketData_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketData_Inject.LocalizationText = "Table.PacketList.Column.PacketData";
            this.cbIsShow_PacketData_Inject.Location = new System.Drawing.Point(250, 195);
            this.cbIsShow_PacketData_Inject.Name = "cbIsShow_PacketData_Inject";
            this.cbIsShow_PacketData_Inject.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_PacketData_Inject.TabIndex = 10;
            this.cbIsShow_PacketData_Inject.Text = "数据";
            // 
            // cbIsShow_PacketLen_Inject
            // 
            this.cbIsShow_PacketLen_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketLen_Inject.Checked = true;
            this.cbIsShow_PacketLen_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketLen_Inject.LocalizationText = "Table.PacketList.Column.PacketLen";
            this.cbIsShow_PacketLen_Inject.Location = new System.Drawing.Point(3, 195);
            this.cbIsShow_PacketLen_Inject.Name = "cbIsShow_PacketLen_Inject";
            this.cbIsShow_PacketLen_Inject.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_PacketLen_Inject.TabIndex = 9;
            this.cbIsShow_PacketLen_Inject.Text = "长度";
            // 
            // cbIsShow_ServerLocation_Inject
            // 
            this.cbIsShow_ServerLocation_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ServerLocation_Inject.Checked = true;
            this.cbIsShow_ServerLocation_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ServerLocation_Inject.LocalizationText = "Table.PacketList.Column.ToLocation";
            this.cbIsShow_ServerLocation_Inject.Location = new System.Drawing.Point(250, 147);
            this.cbIsShow_ServerLocation_Inject.Name = "cbIsShow_ServerLocation_Inject";
            this.cbIsShow_ServerLocation_Inject.Size = new System.Drawing.Size(124, 42);
            this.cbIsShow_ServerLocation_Inject.TabIndex = 8;
            this.cbIsShow_ServerLocation_Inject.Text = "远端所属地";
            // 
            // cbIsShow_ServerAddr_Inject
            // 
            this.cbIsShow_ServerAddr_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ServerAddr_Inject.Checked = true;
            this.cbIsShow_ServerAddr_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ServerAddr_Inject.LocalizationText = "Table.PacketList.Column.PacketTo";
            this.cbIsShow_ServerAddr_Inject.Location = new System.Drawing.Point(3, 147);
            this.cbIsShow_ServerAddr_Inject.Name = "cbIsShow_ServerAddr_Inject";
            this.cbIsShow_ServerAddr_Inject.Size = new System.Drawing.Size(108, 42);
            this.cbIsShow_ServerAddr_Inject.TabIndex = 7;
            this.cbIsShow_ServerAddr_Inject.Text = "远端地址";
            // 
            // cbIsShow_ClientLocation_Inject
            // 
            this.cbIsShow_ClientLocation_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ClientLocation_Inject.Checked = true;
            this.cbIsShow_ClientLocation_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ClientLocation_Inject.LocalizationText = "Table.PacketList.Column.FromLocation";
            this.cbIsShow_ClientLocation_Inject.Location = new System.Drawing.Point(250, 99);
            this.cbIsShow_ClientLocation_Inject.Name = "cbIsShow_ClientLocation_Inject";
            this.cbIsShow_ClientLocation_Inject.Size = new System.Drawing.Size(124, 42);
            this.cbIsShow_ClientLocation_Inject.TabIndex = 6;
            this.cbIsShow_ClientLocation_Inject.Text = "本机所属地";
            // 
            // cbIsShow_ClientAddr_Inject
            // 
            this.cbIsShow_ClientAddr_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ClientAddr_Inject.Checked = true;
            this.cbIsShow_ClientAddr_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ClientAddr_Inject.LocalizationText = "Table.PacketList.Column.PacketFrom";
            this.cbIsShow_ClientAddr_Inject.Location = new System.Drawing.Point(3, 99);
            this.cbIsShow_ClientAddr_Inject.Name = "cbIsShow_ClientAddr_Inject";
            this.cbIsShow_ClientAddr_Inject.Size = new System.Drawing.Size(108, 42);
            this.cbIsShow_ClientAddr_Inject.TabIndex = 5;
            this.cbIsShow_ClientAddr_Inject.Text = "本机地址";
            // 
            // cbIsShow_PacketSocket_Inject
            // 
            this.cbIsShow_PacketSocket_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketSocket_Inject.Checked = true;
            this.cbIsShow_PacketSocket_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketSocket_Inject.LocalizationText = "Table.PacketList.Column.PacketSocket";
            this.cbIsShow_PacketSocket_Inject.Location = new System.Drawing.Point(250, 51);
            this.cbIsShow_PacketSocket_Inject.Name = "cbIsShow_PacketSocket_Inject";
            this.cbIsShow_PacketSocket_Inject.Size = new System.Drawing.Size(92, 42);
            this.cbIsShow_PacketSocket_Inject.TabIndex = 4;
            this.cbIsShow_PacketSocket_Inject.Text = "套接字";
            // 
            // cbIsShow_PacketType_Inject
            // 
            this.cbIsShow_PacketType_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketType_Inject.Checked = true;
            this.cbIsShow_PacketType_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketType_Inject.LocalizationText = "Table.PacketList.Column.PacketType";
            this.cbIsShow_PacketType_Inject.Location = new System.Drawing.Point(3, 51);
            this.cbIsShow_PacketType_Inject.Name = "cbIsShow_PacketType_Inject";
            this.cbIsShow_PacketType_Inject.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_PacketType_Inject.TabIndex = 3;
            this.cbIsShow_PacketType_Inject.Text = "类别";
            // 
            // cbIsShow_ProxyTime_Inject
            // 
            this.cbIsShow_ProxyTime_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ProxyTime_Inject.Checked = true;
            this.cbIsShow_ProxyTime_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ProxyTime_Inject.LocalizationText = "Table.PacketList.Column.PacketTime";
            this.cbIsShow_ProxyTime_Inject.Location = new System.Drawing.Point(250, 3);
            this.cbIsShow_ProxyTime_Inject.Name = "cbIsShow_ProxyTime_Inject";
            this.cbIsShow_ProxyTime_Inject.Size = new System.Drawing.Size(92, 42);
            this.cbIsShow_ProxyTime_Inject.TabIndex = 2;
            this.cbIsShow_ProxyTime_Inject.Text = "时间戳";
            // 
            // cbIsShow_ID_Inject
            // 
            this.cbIsShow_ID_Inject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ID_Inject.Checked = true;
            this.cbIsShow_ID_Inject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ID_Inject.LocalizationText = "Table.PacketList.Column.ID";
            this.cbIsShow_ID_Inject.Location = new System.Drawing.Point(3, 3);
            this.cbIsShow_ID_Inject.Name = "cbIsShow_ID_Inject";
            this.cbIsShow_ID_Inject.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_ID_Inject.TabIndex = 1;
            this.cbIsShow_ID_Inject.Text = "序号";
            // 
            // tpProxy
            // 
            this.tpProxy.Controls.Add(this.tlpProxy);
            this.tpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxy.Location = new System.Drawing.Point(0, 36);
            this.tpProxy.Name = "tpProxy";
            this.tpProxy.Size = new System.Drawing.Size(494, 599);
            this.tpProxy.TabIndex = 1;
            this.tpProxy.Text = "tpProxy";
            // 
            // tlpProxy
            // 
            this.tlpProxy.ColumnCount = 2;
            this.tlpProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProxy.Controls.Add(this.cbIsShow_PacketData_Proxy, 1, 4);
            this.tlpProxy.Controls.Add(this.cbIsShow_PacketLen_Proxy, 0, 4);
            this.tlpProxy.Controls.Add(this.cbIsShow_ServerLocation_Proxy, 1, 3);
            this.tlpProxy.Controls.Add(this.cbIsShow_ServerAddr_Proxy, 0, 3);
            this.tlpProxy.Controls.Add(this.cbIsShow_ClientLocation_Proxy, 1, 2);
            this.tlpProxy.Controls.Add(this.cbIsShow_ClientAddr_Proxy, 0, 2);
            this.tlpProxy.Controls.Add(this.cbIsShow_PacketSocket_Proxy, 1, 1);
            this.tlpProxy.Controls.Add(this.cbIsShow_PacketType_Proxy, 0, 1);
            this.tlpProxy.Controls.Add(this.cbIsShow_ProxyTime_Proxy, 1, 0);
            this.tlpProxy.Controls.Add(this.cbIsShow_ID_Proxy, 0, 0);
            this.tlpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxy.Location = new System.Drawing.Point(0, 0);
            this.tlpProxy.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxy.Name = "tlpProxy";
            this.tlpProxy.RowCount = 6;
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxy.Size = new System.Drawing.Size(494, 599);
            this.tlpProxy.TabIndex = 1;
            // 
            // cbIsShow_PacketData_Proxy
            // 
            this.cbIsShow_PacketData_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketData_Proxy.Checked = true;
            this.cbIsShow_PacketData_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketData_Proxy.LocalizationText = "Table.ProxyList.Column.PacketData";
            this.cbIsShow_PacketData_Proxy.Location = new System.Drawing.Point(250, 195);
            this.cbIsShow_PacketData_Proxy.Name = "cbIsShow_PacketData_Proxy";
            this.cbIsShow_PacketData_Proxy.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_PacketData_Proxy.TabIndex = 10;
            this.cbIsShow_PacketData_Proxy.Text = "数据";
            // 
            // cbIsShow_PacketLen_Proxy
            // 
            this.cbIsShow_PacketLen_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketLen_Proxy.Checked = true;
            this.cbIsShow_PacketLen_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketLen_Proxy.LocalizationText = "Table.ProxyList.Column.PacketLen";
            this.cbIsShow_PacketLen_Proxy.Location = new System.Drawing.Point(3, 195);
            this.cbIsShow_PacketLen_Proxy.Name = "cbIsShow_PacketLen_Proxy";
            this.cbIsShow_PacketLen_Proxy.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_PacketLen_Proxy.TabIndex = 9;
            this.cbIsShow_PacketLen_Proxy.Text = "长度";
            // 
            // cbIsShow_ServerLocation_Proxy
            // 
            this.cbIsShow_ServerLocation_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ServerLocation_Proxy.Checked = true;
            this.cbIsShow_ServerLocation_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ServerLocation_Proxy.LocalizationText = "Table.ProxyList.Column.ServerLocation";
            this.cbIsShow_ServerLocation_Proxy.Location = new System.Drawing.Point(250, 147);
            this.cbIsShow_ServerLocation_Proxy.Name = "cbIsShow_ServerLocation_Proxy";
            this.cbIsShow_ServerLocation_Proxy.Size = new System.Drawing.Size(140, 42);
            this.cbIsShow_ServerLocation_Proxy.TabIndex = 8;
            this.cbIsShow_ServerLocation_Proxy.Text = "服务端所属地";
            // 
            // cbIsShow_ServerAddr_Proxy
            // 
            this.cbIsShow_ServerAddr_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ServerAddr_Proxy.Checked = true;
            this.cbIsShow_ServerAddr_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ServerAddr_Proxy.LocalizationText = "Table.ProxyList.Column.ServerDomain";
            this.cbIsShow_ServerAddr_Proxy.Location = new System.Drawing.Point(3, 147);
            this.cbIsShow_ServerAddr_Proxy.Name = "cbIsShow_ServerAddr_Proxy";
            this.cbIsShow_ServerAddr_Proxy.Size = new System.Drawing.Size(124, 42);
            this.cbIsShow_ServerAddr_Proxy.TabIndex = 7;
            this.cbIsShow_ServerAddr_Proxy.Text = "服务端地址";
            // 
            // cbIsShow_ClientLocation_Proxy
            // 
            this.cbIsShow_ClientLocation_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ClientLocation_Proxy.Checked = true;
            this.cbIsShow_ClientLocation_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ClientLocation_Proxy.LocalizationText = "Table.ProxyList.Column.ClientLocation";
            this.cbIsShow_ClientLocation_Proxy.Location = new System.Drawing.Point(250, 99);
            this.cbIsShow_ClientLocation_Proxy.Name = "cbIsShow_ClientLocation_Proxy";
            this.cbIsShow_ClientLocation_Proxy.Size = new System.Drawing.Size(140, 42);
            this.cbIsShow_ClientLocation_Proxy.TabIndex = 6;
            this.cbIsShow_ClientLocation_Proxy.Text = "客户端所属地";
            // 
            // cbIsShow_ClientAddr_Proxy
            // 
            this.cbIsShow_ClientAddr_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ClientAddr_Proxy.Checked = true;
            this.cbIsShow_ClientAddr_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ClientAddr_Proxy.LocalizationText = "Table.ProxyList.Column.ClientAddr";
            this.cbIsShow_ClientAddr_Proxy.Location = new System.Drawing.Point(3, 99);
            this.cbIsShow_ClientAddr_Proxy.Name = "cbIsShow_ClientAddr_Proxy";
            this.cbIsShow_ClientAddr_Proxy.Size = new System.Drawing.Size(124, 42);
            this.cbIsShow_ClientAddr_Proxy.TabIndex = 5;
            this.cbIsShow_ClientAddr_Proxy.Text = "客户端地址";
            // 
            // cbIsShow_PacketSocket_Proxy
            // 
            this.cbIsShow_PacketSocket_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketSocket_Proxy.Checked = true;
            this.cbIsShow_PacketSocket_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketSocket_Proxy.LocalizationText = "Table.ProxyList.Column.PacketSocket";
            this.cbIsShow_PacketSocket_Proxy.Location = new System.Drawing.Point(250, 51);
            this.cbIsShow_PacketSocket_Proxy.Name = "cbIsShow_PacketSocket_Proxy";
            this.cbIsShow_PacketSocket_Proxy.Size = new System.Drawing.Size(92, 42);
            this.cbIsShow_PacketSocket_Proxy.TabIndex = 4;
            this.cbIsShow_PacketSocket_Proxy.Text = "套接字";
            // 
            // cbIsShow_PacketType_Proxy
            // 
            this.cbIsShow_PacketType_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_PacketType_Proxy.Checked = true;
            this.cbIsShow_PacketType_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_PacketType_Proxy.LocalizationText = "Table.ProxyList.Column.PacketType";
            this.cbIsShow_PacketType_Proxy.Location = new System.Drawing.Point(3, 51);
            this.cbIsShow_PacketType_Proxy.Name = "cbIsShow_PacketType_Proxy";
            this.cbIsShow_PacketType_Proxy.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_PacketType_Proxy.TabIndex = 3;
            this.cbIsShow_PacketType_Proxy.Text = "类别";
            // 
            // cbIsShow_ProxyTime_Proxy
            // 
            this.cbIsShow_ProxyTime_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ProxyTime_Proxy.Checked = true;
            this.cbIsShow_ProxyTime_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ProxyTime_Proxy.LocalizationText = "Table.ProxyList.Column.ProxyTime";
            this.cbIsShow_ProxyTime_Proxy.Location = new System.Drawing.Point(250, 3);
            this.cbIsShow_ProxyTime_Proxy.Name = "cbIsShow_ProxyTime_Proxy";
            this.cbIsShow_ProxyTime_Proxy.Size = new System.Drawing.Size(92, 42);
            this.cbIsShow_ProxyTime_Proxy.TabIndex = 2;
            this.cbIsShow_ProxyTime_Proxy.Text = "时间戳";
            // 
            // cbIsShow_ID_Proxy
            // 
            this.cbIsShow_ID_Proxy.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsShow_ID_Proxy.Checked = true;
            this.cbIsShow_ID_Proxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsShow_ID_Proxy.LocalizationText = "Table.ProxyList.Column.ID";
            this.cbIsShow_ID_Proxy.Location = new System.Drawing.Point(3, 3);
            this.cbIsShow_ID_Proxy.Name = "cbIsShow_ID_Proxy";
            this.cbIsShow_ID_Proxy.Size = new System.Drawing.Size(76, 42);
            this.cbIsShow_ID_Proxy.TabIndex = 1;
            this.cbIsShow_ID_Proxy.Text = "序号";
            // 
            // ListSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpListSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ListSetting";
            this.Size = new System.Drawing.Size(500, 750);
            this.Load += new System.EventHandler(this.ListSetting_Load);
            this.tlpListSettings.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tabListSettings.ResumeLayout(false);
            this.tpInject.ResumeLayout(false);
            this.tlpInject.ResumeLayout(false);
            this.tlpInject.PerformLayout();
            this.tpProxy.ResumeLayout(false);
            this.tlpProxy.ResumeLayout(false);
            this.tlpProxy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpListSettings;
        private AntdUI.Divider dColumn;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Tabs tabListSettings;
        private AntdUI.TabPage tpInject;
        private System.Windows.Forms.TableLayoutPanel tlpInject;
        private AntdUI.Checkbox cbIsShow_PacketData_Inject;
        private AntdUI.Checkbox cbIsShow_PacketLen_Inject;
        private AntdUI.Checkbox cbIsShow_ServerLocation_Inject;
        private AntdUI.Checkbox cbIsShow_ServerAddr_Inject;
        private AntdUI.Checkbox cbIsShow_ClientLocation_Inject;
        private AntdUI.Checkbox cbIsShow_ClientAddr_Inject;
        private AntdUI.Checkbox cbIsShow_PacketSocket_Inject;
        private AntdUI.Checkbox cbIsShow_PacketType_Inject;
        private AntdUI.Checkbox cbIsShow_ProxyTime_Inject;
        private AntdUI.Checkbox cbIsShow_ID_Inject;
        private AntdUI.TabPage tpProxy;
        private System.Windows.Forms.TableLayoutPanel tlpProxy;
        private AntdUI.Checkbox cbIsShow_PacketData_Proxy;
        private AntdUI.Checkbox cbIsShow_PacketLen_Proxy;
        private AntdUI.Checkbox cbIsShow_ServerLocation_Proxy;
        private AntdUI.Checkbox cbIsShow_ServerAddr_Proxy;
        private AntdUI.Checkbox cbIsShow_ClientLocation_Proxy;
        private AntdUI.Checkbox cbIsShow_ClientAddr_Proxy;
        private AntdUI.Checkbox cbIsShow_PacketSocket_Proxy;
        private AntdUI.Checkbox cbIsShow_PacketType_Proxy;
        private AntdUI.Checkbox cbIsShow_ProxyTime_Proxy;
        private AntdUI.Checkbox cbIsShow_ID_Proxy;
    }
}

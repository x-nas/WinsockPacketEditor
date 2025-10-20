namespace WinsockPacketEditor
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.btn_mode = new AntdUI.Button();
            this.btn_global = new AntdUI.Dropdown();
            this.tlpSelectMode = new WinsockPacketEditor.TableLayoutPanelEx();
            this.pInjectMode = new AntdUI.Panel();
            this.lInject2 = new AntdUI.Label();
            this.lInject1 = new AntdUI.Label();
            this.InjectMode = new AntdUI.Avatar();
            this.pProxyMode = new AntdUI.Panel();
            this.lProxy2 = new AntdUI.Label();
            this.lProxy1 = new AntdUI.Label();
            this.aProxyMode = new AntdUI.Avatar();
            this.pMultipleOpen = new AntdUI.Panel();
            this.lMultipleOpenText = new AntdUI.Label();
            this.lMultipleOpen = new AntdUI.Label();
            this.aMultipleOpen = new AntdUI.Avatar();
            this.aStartForm = new AntdUI.Avatar();
            this.tlpBackground = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lBG2 = new AntdUI.Label();
            this.lBG1 = new AntdUI.Label();
            this.tlpStartForm = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lMore = new AntdUI.Label();
            this.tlpMore = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bQA = new AntdUI.Button();
            this.bGitee = new AntdUI.Button();
            this.bGitHub = new AntdUI.Button();
            this.bTutorials = new AntdUI.Button();
            this.bWPEWebSite = new AntdUI.Button();
            this.pageHeader.SuspendLayout();
            this.tlpSelectMode.SuspendLayout();
            this.pInjectMode.SuspendLayout();
            this.pProxyMode.SuspendLayout();
            this.pMultipleOpen.SuspendLayout();
            this.aStartForm.SuspendLayout();
            this.tlpBackground.SuspendLayout();
            this.tlpStartForm.SuspendLayout();
            this.tlpMore.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader
            // 
            this.pageHeader.BackColor = System.Drawing.Color.Transparent;
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.Controls.Add(this.btn_mode);
            this.pageHeader.Controls.Add(this.btn_global);
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pageHeader.MaximizeBox = false;
            this.pageHeader.MinimizeBox = false;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1000, 30);
            this.pageHeader.SubText = "";
            this.pageHeader.TabIndex = 5;
            this.pageHeader.Text = "WPE x64";
            // 
            // btn_mode
            // 
            this.btn_mode.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_mode.Ghost = true;
            this.btn_mode.IconSvg = "SunOutlined";
            this.btn_mode.Location = new System.Drawing.Point(890, 0);
            this.btn_mode.Margin = new System.Windows.Forms.Padding(2);
            this.btn_mode.Name = "btn_mode";
            this.btn_mode.Radius = 0;
            this.btn_mode.Size = new System.Drawing.Size(35, 30);
            this.btn_mode.TabIndex = 13;
            this.btn_mode.ToggleIconSvg = "MoonOutlined";
            this.btn_mode.WaveSize = 0;
            this.btn_mode.Click += new System.EventHandler(this.btn_mode_Click);
            // 
            // btn_global
            // 
            this.btn_global.BackActive = System.Drawing.Color.Transparent;
            this.btn_global.BackHover = System.Drawing.Color.Transparent;
            this.btn_global.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_global.DropDownRadius = 6;
            this.btn_global.Ghost = true;
            this.btn_global.IconSvg = "GlobalOutlined";
            this.btn_global.Location = new System.Drawing.Point(925, 0);
            this.btn_global.Margin = new System.Windows.Forms.Padding(4);
            this.btn_global.Name = "btn_global";
            this.btn_global.Placement = AntdUI.TAlignFrom.BR;
            this.btn_global.Radius = 0;
            this.btn_global.Size = new System.Drawing.Size(35, 30);
            this.btn_global.TabIndex = 11;
            this.btn_global.WaveSize = 0;
            this.btn_global.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.btn_global_SelectedValueChanged);
            // 
            // tlpSelectMode
            // 
            this.tlpSelectMode.ColumnCount = 5;
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tlpSelectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpSelectMode.Controls.Add(this.pInjectMode, 2, 0);
            this.tlpSelectMode.Controls.Add(this.pProxyMode, 4, 0);
            this.tlpSelectMode.Controls.Add(this.pMultipleOpen, 0, 0);
            this.tlpSelectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelectMode.Location = new System.Drawing.Point(21, 180);
            this.tlpSelectMode.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSelectMode.Name = "tlpSelectMode";
            this.tlpSelectMode.RowCount = 1;
            this.tlpSelectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectMode.Size = new System.Drawing.Size(958, 80);
            this.tlpSelectMode.TabIndex = 1;
            // 
            // pInjectMode
            // 
            this.pInjectMode.BorderWidth = 1F;
            this.pInjectMode.Controls.Add(this.lInject2);
            this.pInjectMode.Controls.Add(this.lInject1);
            this.pInjectMode.Controls.Add(this.InjectMode);
            this.pInjectMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.pInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pInjectMode.Location = new System.Drawing.Point(320, 2);
            this.pInjectMode.Margin = new System.Windows.Forms.Padding(2);
            this.pInjectMode.Name = "pInjectMode";
            this.pInjectMode.Size = new System.Drawing.Size(316, 76);
            this.pInjectMode.TabIndex = 2;
            this.pInjectMode.Text = "panel1";
            this.pInjectMode.Click += new System.EventHandler(this.pInjectMode_Click);
            this.pInjectMode.MouseEnter += new System.EventHandler(this.pInjectMode_MouseEnter);
            this.pInjectMode.MouseLeave += new System.EventHandler(this.pInjectMode_MouseLeave);
            // 
            // lInject2
            // 
            this.lInject2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lInject2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lInject2.ForeColor = System.Drawing.Color.Gray;
            this.lInject2.LocalizationText = "StartForm.InjectMode.Text";
            this.lInject2.Location = new System.Drawing.Point(84, 38);
            this.lInject2.Margin = new System.Windows.Forms.Padding(2);
            this.lInject2.Name = "lInject2";
            this.lInject2.Size = new System.Drawing.Size(156, 16);
            this.lInject2.TabIndex = 2;
            this.lInject2.Text = "以注入进程的方式来拦截封包";
            this.lInject2.Click += new System.EventHandler(this.lInject2_Click);
            // 
            // lInject1
            // 
            this.lInject1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lInject1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lInject1.LocalizationText = "StartForm.InjectMode";
            this.lInject1.Location = new System.Drawing.Point(84, 19);
            this.lInject1.Margin = new System.Windows.Forms.Padding(2);
            this.lInject1.Name = "lInject1";
            this.lInject1.Size = new System.Drawing.Size(56, 19);
            this.lInject1.TabIndex = 1;
            this.lInject1.Text = "注入模式";
            this.lInject1.Click += new System.EventHandler(this.lInject1_Click);
            // 
            // InjectMode
            // 
            this.InjectMode.Image = ((System.Drawing.Image)(resources.GetObject("InjectMode.Image")));
            this.InjectMode.ImageFit = AntdUI.TFit.Fill;
            this.InjectMode.Location = new System.Drawing.Point(20, 8);
            this.InjectMode.Margin = new System.Windows.Forms.Padding(2);
            this.InjectMode.Name = "InjectMode";
            this.InjectMode.Size = new System.Drawing.Size(50, 58);
            this.InjectMode.TabIndex = 0;
            this.InjectMode.Text = "";
            this.InjectMode.Click += new System.EventHandler(this.InjectMode_Click);
            // 
            // pProxyMode
            // 
            this.pProxyMode.BorderWidth = 1F;
            this.pProxyMode.Controls.Add(this.lProxy2);
            this.pProxyMode.Controls.Add(this.lProxy1);
            this.pProxyMode.Controls.Add(this.aProxyMode);
            this.pProxyMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.pProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pProxyMode.Location = new System.Drawing.Point(647, 2);
            this.pProxyMode.Margin = new System.Windows.Forms.Padding(2);
            this.pProxyMode.Name = "pProxyMode";
            this.pProxyMode.Size = new System.Drawing.Size(309, 76);
            this.pProxyMode.TabIndex = 1;
            this.pProxyMode.Text = "panel1";
            this.pProxyMode.Click += new System.EventHandler(this.pProxyMode_Click);
            this.pProxyMode.MouseEnter += new System.EventHandler(this.pProxyMode_MouseEnter);
            this.pProxyMode.MouseLeave += new System.EventHandler(this.pProxyMode_MouseLeave);
            // 
            // lProxy2
            // 
            this.lProxy2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lProxy2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxy2.ForeColor = System.Drawing.Color.Gray;
            this.lProxy2.LocalizationText = "StartForm.ProxyMode.Text";
            this.lProxy2.Location = new System.Drawing.Point(85, 38);
            this.lProxy2.Margin = new System.Windows.Forms.Padding(2);
            this.lProxy2.Name = "lProxy2";
            this.lProxy2.Size = new System.Drawing.Size(168, 16);
            this.lProxy2.TabIndex = 2;
            this.lProxy2.Text = "以代理服务端的方式来拦截封包";
            this.lProxy2.Click += new System.EventHandler(this.lProxy2_Click);
            // 
            // lProxy1
            // 
            this.lProxy1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lProxy1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lProxy1.LocalizationText = "StartForm.ProxyMode";
            this.lProxy1.Location = new System.Drawing.Point(85, 19);
            this.lProxy1.Margin = new System.Windows.Forms.Padding(2);
            this.lProxy1.Name = "lProxy1";
            this.lProxy1.Size = new System.Drawing.Size(56, 19);
            this.lProxy1.TabIndex = 1;
            this.lProxy1.Text = "代理模式";
            this.lProxy1.Click += new System.EventHandler(this.lProxy1_Click);
            // 
            // aProxyMode
            // 
            this.aProxyMode.Image = ((System.Drawing.Image)(resources.GetObject("aProxyMode.Image")));
            this.aProxyMode.ImageFit = AntdUI.TFit.Fill;
            this.aProxyMode.Location = new System.Drawing.Point(20, 8);
            this.aProxyMode.Margin = new System.Windows.Forms.Padding(2);
            this.aProxyMode.Name = "aProxyMode";
            this.aProxyMode.Size = new System.Drawing.Size(50, 58);
            this.aProxyMode.TabIndex = 0;
            this.aProxyMode.Text = "";
            this.aProxyMode.Click += new System.EventHandler(this.aProxyMode_Click);
            // 
            // pMultipleOpen
            // 
            this.pMultipleOpen.BorderWidth = 1F;
            this.pMultipleOpen.Controls.Add(this.lMultipleOpenText);
            this.pMultipleOpen.Controls.Add(this.lMultipleOpen);
            this.pMultipleOpen.Controls.Add(this.aMultipleOpen);
            this.pMultipleOpen.Cursor = System.Windows.Forms.Cursors.Default;
            this.pMultipleOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMultipleOpen.Location = new System.Drawing.Point(2, 2);
            this.pMultipleOpen.Margin = new System.Windows.Forms.Padding(2);
            this.pMultipleOpen.Name = "pMultipleOpen";
            this.pMultipleOpen.Size = new System.Drawing.Size(307, 76);
            this.pMultipleOpen.TabIndex = 0;
            this.pMultipleOpen.Click += new System.EventHandler(this.pMultipleOpen_Click);
            this.pMultipleOpen.MouseEnter += new System.EventHandler(this.pMultipleOpen_MouseEnter);
            this.pMultipleOpen.MouseLeave += new System.EventHandler(this.pMultipleOpen_MouseLeave);
            // 
            // lMultipleOpenText
            // 
            this.lMultipleOpenText.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lMultipleOpenText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lMultipleOpenText.ForeColor = System.Drawing.Color.Gray;
            this.lMultipleOpenText.LocalizationText = "StartForm.MultipleOpen.Text";
            this.lMultipleOpenText.Location = new System.Drawing.Point(85, 38);
            this.lMultipleOpenText.Margin = new System.Windows.Forms.Padding(2);
            this.lMultipleOpenText.Name = "lMultipleOpenText";
            this.lMultipleOpenText.Size = new System.Drawing.Size(180, 16);
            this.lMultipleOpenText.TabIndex = 2;
            this.lMultipleOpenText.Text = "配置数据库的路径以实现软件多开";
            this.lMultipleOpenText.Click += new System.EventHandler(this.lMultipleOpenText_Click);
            // 
            // lMultipleOpen
            // 
            this.lMultipleOpen.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lMultipleOpen.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lMultipleOpen.LocalizationText = "StartForm.MultipleOpen";
            this.lMultipleOpen.Location = new System.Drawing.Point(85, 19);
            this.lMultipleOpen.Margin = new System.Windows.Forms.Padding(2);
            this.lMultipleOpen.Name = "lMultipleOpen";
            this.lMultipleOpen.Size = new System.Drawing.Size(56, 19);
            this.lMultipleOpen.TabIndex = 1;
            this.lMultipleOpen.Text = "多开设置";
            this.lMultipleOpen.Click += new System.EventHandler(this.lMultipleOpen_Click);
            // 
            // aMultipleOpen
            // 
            this.aMultipleOpen.Image = ((System.Drawing.Image)(resources.GetObject("aMultipleOpen.Image")));
            this.aMultipleOpen.ImageFit = AntdUI.TFit.Fill;
            this.aMultipleOpen.Location = new System.Drawing.Point(20, 8);
            this.aMultipleOpen.Margin = new System.Windows.Forms.Padding(2);
            this.aMultipleOpen.Name = "aMultipleOpen";
            this.aMultipleOpen.Size = new System.Drawing.Size(50, 58);
            this.aMultipleOpen.TabIndex = 0;
            this.aMultipleOpen.Text = "";
            this.aMultipleOpen.Click += new System.EventHandler(this.aMultipleOpen_Click);
            // 
            // aStartForm
            // 
            this.aStartForm.Controls.Add(this.tlpBackground);
            this.aStartForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aStartForm.Image = ((System.Drawing.Image)(resources.GetObject("aStartForm.Image")));
            this.aStartForm.Location = new System.Drawing.Point(23, 17);
            this.aStartForm.Margin = new System.Windows.Forms.Padding(2);
            this.aStartForm.Name = "aStartForm";
            this.aStartForm.Radius = 8;
            this.aStartForm.Size = new System.Drawing.Size(954, 146);
            this.aStartForm.TabIndex = 0;
            this.aStartForm.Text = "";
            // 
            // tlpBackground
            // 
            this.tlpBackground.ColumnCount = 1;
            this.tlpBackground.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackground.Controls.Add(this.lBG2, 0, 1);
            this.tlpBackground.Controls.Add(this.lBG1, 0, 0);
            this.tlpBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBackground.Location = new System.Drawing.Point(0, 0);
            this.tlpBackground.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBackground.Name = "tlpBackground";
            this.tlpBackground.RowCount = 2;
            this.tlpBackground.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlpBackground.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpBackground.Size = new System.Drawing.Size(954, 146);
            this.tlpBackground.TabIndex = 0;
            // 
            // lBG2
            // 
            this.lBG2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBG2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lBG2.ForeColor = System.Drawing.Color.White;
            this.lBG2.Location = new System.Drawing.Point(2, 94);
            this.lBG2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.lBG2.Name = "lBG2";
            this.lBG2.Shadow = 20;
            this.lBG2.Size = new System.Drawing.Size(950, 50);
            this.lBG2.TabIndex = 1;
            this.lBG2.Text = "Winsock Packet Editor";
            this.lBG2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lBG2.TextMultiLine = false;
            // 
            // lBG1
            // 
            this.lBG1.ColorExtend = "";
            this.lBG1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBG1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lBG1.ForeColor = System.Drawing.Color.White;
            this.lBG1.Location = new System.Drawing.Point(2, 2);
            this.lBG1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.lBG1.Name = "lBG1";
            this.lBG1.Shadow = 5;
            this.lBG1.Size = new System.Drawing.Size(950, 92);
            this.lBG1.TabIndex = 0;
            this.lBG1.Text = "WPE x64";
            this.lBG1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lBG1.TextMultiLine = false;
            // 
            // tlpStartForm
            // 
            this.tlpStartForm.BackColor = System.Drawing.Color.Transparent;
            this.tlpStartForm.ColumnCount = 3;
            this.tlpStartForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tlpStartForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStartForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tlpStartForm.Controls.Add(this.aStartForm, 1, 1);
            this.tlpStartForm.Controls.Add(this.tlpSelectMode, 1, 3);
            this.tlpStartForm.Controls.Add(this.lMore, 1, 5);
            this.tlpStartForm.Controls.Add(this.tlpMore, 1, 6);
            this.tlpStartForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStartForm.Location = new System.Drawing.Point(0, 30);
            this.tlpStartForm.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStartForm.Name = "tlpStartForm";
            this.tlpStartForm.RowCount = 7;
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpStartForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStartForm.Size = new System.Drawing.Size(1000, 550);
            this.tlpStartForm.TabIndex = 6;
            // 
            // lMore
            // 
            this.lMore.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.lMore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lMore.LocalizationText = "StartForm.LearnMore";
            this.lMore.Location = new System.Drawing.Point(23, 277);
            this.lMore.Margin = new System.Windows.Forms.Padding(2);
            this.lMore.Name = "lMore";
            this.lMore.Size = new System.Drawing.Size(48, 16);
            this.lMore.TabIndex = 2;
            this.lMore.Text = "了解更多";
            // 
            // tlpMore
            // 
            this.tlpMore.ColumnCount = 2;
            this.tlpMore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMore.Controls.Add(this.bQA, 0, 5);
            this.tlpMore.Controls.Add(this.bGitee, 0, 4);
            this.tlpMore.Controls.Add(this.bGitHub, 0, 3);
            this.tlpMore.Controls.Add(this.bTutorials, 0, 2);
            this.tlpMore.Controls.Add(this.bWPEWebSite, 0, 1);
            this.tlpMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMore.Location = new System.Drawing.Point(21, 295);
            this.tlpMore.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMore.Name = "tlpMore";
            this.tlpMore.RowCount = 7;
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMore.Size = new System.Drawing.Size(958, 255);
            this.tlpMore.TabIndex = 3;
            // 
            // bQA
            // 
            this.bQA.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bQA.ColorScheme = AntdUI.TAMode.Dark;
            this.bQA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bQA.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bQA.ForeColor = System.Drawing.Color.DodgerBlue;
            this.bQA.Ghost = true;
            this.bQA.LocalizationText = "StartForm.QA";
            this.bQA.Location = new System.Drawing.Point(3, 133);
            this.bQA.Name = "bQA";
            this.bQA.Size = new System.Drawing.Size(82, 24);
            this.bQA.TabIndex = 8;
            this.bQA.Text = "问题 & 反馈";
            this.bQA.WaveSize = 0;
            this.bQA.Click += new System.EventHandler(this.bQA_Click);
            // 
            // bGitee
            // 
            this.bGitee.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bGitee.ColorScheme = AntdUI.TAMode.Dark;
            this.bGitee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bGitee.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bGitee.ForeColor = System.Drawing.Color.DodgerBlue;
            this.bGitee.Ghost = true;
            this.bGitee.Location = new System.Drawing.Point(3, 103);
            this.bGitee.Name = "bGitee";
            this.bGitee.Size = new System.Drawing.Size(47, 24);
            this.bGitee.TabIndex = 6;
            this.bGitee.Text = "Gitee";
            this.bGitee.WaveSize = 0;
            this.bGitee.Click += new System.EventHandler(this.bGitee_Click);
            // 
            // bGitHub
            // 
            this.bGitHub.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bGitHub.ColorScheme = AntdUI.TAMode.Dark;
            this.bGitHub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bGitHub.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bGitHub.ForeColor = System.Drawing.Color.DodgerBlue;
            this.bGitHub.Ghost = true;
            this.bGitHub.Location = new System.Drawing.Point(3, 73);
            this.bGitHub.Name = "bGitHub";
            this.bGitHub.Size = new System.Drawing.Size(57, 24);
            this.bGitHub.TabIndex = 4;
            this.bGitHub.Text = "GitHub";
            this.bGitHub.WaveSize = 0;
            this.bGitHub.Click += new System.EventHandler(this.bGitHub_Click);
            // 
            // bTutorials
            // 
            this.bTutorials.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bTutorials.ColorScheme = AntdUI.TAMode.Dark;
            this.bTutorials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTutorials.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bTutorials.ForeColor = System.Drawing.Color.DodgerBlue;
            this.bTutorials.Ghost = true;
            this.bTutorials.LocalizationText = "StartForm.Tutorials";
            this.bTutorials.Location = new System.Drawing.Point(3, 43);
            this.bTutorials.Name = "bTutorials";
            this.bTutorials.Size = new System.Drawing.Size(88, 24);
            this.bTutorials.TabIndex = 2;
            this.bTutorials.Text = "软件使用教程";
            this.bTutorials.WaveSize = 0;
            this.bTutorials.Click += new System.EventHandler(this.bTutorials_Click);
            // 
            // bWPEWebSite
            // 
            this.bWPEWebSite.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bWPEWebSite.ColorScheme = AntdUI.TAMode.Dark;
            this.bWPEWebSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bWPEWebSite.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bWPEWebSite.ForeColor = System.Drawing.Color.DodgerBlue;
            this.bWPEWebSite.Ghost = true;
            this.bWPEWebSite.LocalizationText = "StartForm.OfficialWebsite";
            this.bWPEWebSite.Location = new System.Drawing.Point(3, 13);
            this.bWPEWebSite.Name = "bWPEWebSite";
            this.bWPEWebSite.Size = new System.Drawing.Size(64, 24);
            this.bWPEWebSite.TabIndex = 0;
            this.bWPEWebSite.Text = "官方网站";
            this.bWPEWebSite.WaveSize = 0;
            this.bWPEWebSite.Click += new System.EventHandler(this.bWPEWebSite_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 580);
            this.Controls.Add(this.tlpStartForm);
            this.Controls.Add(this.pageHeader);
            this.Dark = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Mode = AntdUI.TAMode.Dark;
            this.Name = "StartForm";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WPE x64";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartForm_FormClosing);
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.pageHeader.ResumeLayout(false);
            this.tlpSelectMode.ResumeLayout(false);
            this.pInjectMode.ResumeLayout(false);
            this.pInjectMode.PerformLayout();
            this.pProxyMode.ResumeLayout(false);
            this.pProxyMode.PerformLayout();
            this.pMultipleOpen.ResumeLayout(false);
            this.pMultipleOpen.PerformLayout();
            this.aStartForm.ResumeLayout(false);
            this.tlpBackground.ResumeLayout(false);
            this.tlpStartForm.ResumeLayout(false);
            this.tlpStartForm.PerformLayout();
            this.tlpMore.ResumeLayout(false);
            this.tlpMore.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private AntdUI.Dropdown btn_global;
        private TableLayoutPanelEx tlpSelectMode;
        private AntdUI.Panel pMultipleOpen;
        private AntdUI.Label lMultipleOpenText;
        private AntdUI.Label lMultipleOpen;
        private AntdUI.Avatar aMultipleOpen;
        private AntdUI.Avatar aStartForm;
        private TableLayoutPanelEx tlpBackground;
        private AntdUI.Label lBG2;
        private AntdUI.Label lBG1;
        private TableLayoutPanelEx tlpStartForm;
        private AntdUI.Panel pInjectMode;
        private AntdUI.Label lInject2;
        private AntdUI.Label lInject1;
        private AntdUI.Avatar InjectMode;
        private AntdUI.Panel pProxyMode;
        private AntdUI.Label lProxy2;
        private AntdUI.Label lProxy1;
        private AntdUI.Avatar aProxyMode;
        private AntdUI.Label lMore;
        private TableLayoutPanelEx tlpMore;
        private AntdUI.Button bGitHub;
        private AntdUI.Button bTutorials;
        private AntdUI.Button bWPEWebSite;
        private AntdUI.Button bQA;
        private AntdUI.Button bGitee;
        private AntdUI.Button btn_mode;
    }
}
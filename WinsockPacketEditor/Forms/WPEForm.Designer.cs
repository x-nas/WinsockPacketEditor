namespace WinsockPacketEditor.Forms
{
    partial class WPEForm
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WPEForm));
            this.pWPEForm = new AntdUI.Panel();
            this.tabWPEForm = new AntdUI.Tabs();
            this.tpLogin = new AntdUI.TabPage();
            this.tlpWPEForm = new System.Windows.Forms.TableLayoutPanel();
            this.bRemote = new AntdUI.Button();
            this.lWPEForm = new AntdUI.Label();
            this.aWPEForm = new AntdUI.Avatar();
            this.ddlStartMode = new AntdUI.Select();
            this.bLogin = new AntdUI.Button();
            this.tpRemote = new AntdUI.TabPage();
            this.tlpRemote = new System.Windows.Forms.TableLayoutPanel();
            this.aRemote = new AntdUI.Avatar();
            this.txtRemote_PassWord = new AntdUI.Input();
            this.txtRemote_UserName = new AntdUI.Input();
            this.nudRemote_Port = new AntdUI.InputNumber();
            this.bSaveRemote = new AntdUI.Button();
            this.lRemote = new AntdUI.Label();
            this.cbIsRemote = new AntdUI.Checkbox();
            this.pageHeader = new AntdUI.PageHeader();
            this.btn_global = new AntdUI.Dropdown();
            this.pWPEForm.SuspendLayout();
            this.tabWPEForm.SuspendLayout();
            this.tpLogin.SuspendLayout();
            this.tlpWPEForm.SuspendLayout();
            this.tpRemote.SuspendLayout();
            this.tlpRemote.SuspendLayout();
            this.pageHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pWPEForm
            // 
            this.pWPEForm.BackExtend = "135, #E3FDF5, #FFE6FA";
            this.pWPEForm.Controls.Add(this.tabWPEForm);
            this.pWPEForm.Controls.Add(this.pageHeader);
            this.pWPEForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pWPEForm.Location = new System.Drawing.Point(0, 0);
            this.pWPEForm.Name = "pWPEForm";
            this.pWPEForm.Size = new System.Drawing.Size(320, 450);
            this.pWPEForm.TabIndex = 2;
            this.pWPEForm.Text = "panel1";
            // 
            // tabWPEForm
            // 
            this.tabWPEForm.BackColor = System.Drawing.Color.Transparent;
            this.tabWPEForm.Controls.Add(this.tpLogin);
            this.tabWPEForm.Controls.Add(this.tpRemote);
            this.tabWPEForm.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabWPEForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabWPEForm.Location = new System.Drawing.Point(0, 30);
            this.tabWPEForm.Name = "tabWPEForm";
            this.tabWPEForm.Pages.Add(this.tpLogin);
            this.tabWPEForm.Pages.Add(this.tpRemote);
            this.tabWPEForm.SelectedIndex = 1;
            this.tabWPEForm.Size = new System.Drawing.Size(320, 420);
            this.tabWPEForm.Style = styleLine1;
            this.tabWPEForm.TabIndex = 5;
            this.tabWPEForm.Text = "tabs1";
            // 
            // tpLogin
            // 
            this.tpLogin.Controls.Add(this.tlpWPEForm);
            this.tpLogin.Location = new System.Drawing.Point(-314, -384);
            this.tpLogin.Name = "tpLogin";
            this.tpLogin.Size = new System.Drawing.Size(314, 384);
            this.tpLogin.TabIndex = 0;
            this.tpLogin.Text = "tpLogin";
            // 
            // tlpWPEForm
            // 
            this.tlpWPEForm.BackColor = System.Drawing.Color.Transparent;
            this.tlpWPEForm.ColumnCount = 3;
            this.tlpWPEForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWPEForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpWPEForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWPEForm.Controls.Add(this.bRemote, 1, 8);
            this.tlpWPEForm.Controls.Add(this.lWPEForm, 1, 1);
            this.tlpWPEForm.Controls.Add(this.aWPEForm, 1, 3);
            this.tlpWPEForm.Controls.Add(this.ddlStartMode, 1, 4);
            this.tlpWPEForm.Controls.Add(this.bLogin, 1, 6);
            this.tlpWPEForm.Cursor = System.Windows.Forms.Cursors.Default;
            this.tlpWPEForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWPEForm.Location = new System.Drawing.Point(0, 0);
            this.tlpWPEForm.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWPEForm.Name = "tlpWPEForm";
            this.tlpWPEForm.RowCount = 9;
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWPEForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpWPEForm.Size = new System.Drawing.Size(314, 384);
            this.tlpWPEForm.TabIndex = 6;
            // 
            // bRemote
            // 
            this.bRemote.BackActive = System.Drawing.Color.Transparent;
            this.bRemote.BackHover = System.Drawing.Color.Transparent;
            this.bRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRemote.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bRemote.ForeColor = System.Drawing.Color.DodgerBlue;
            this.bRemote.Ghost = true;
            this.bRemote.LocalizationText = "WPEForm.SetRemot";
            this.bRemote.Location = new System.Drawing.Point(60, 327);
            this.bRemote.Name = "bRemote";
            this.bRemote.Size = new System.Drawing.Size(194, 54);
            this.bRemote.TabIndex = 7;
            this.bRemote.Text = "设置远程管理后台";
            this.bRemote.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bRemote.WaveSize = 0;
            this.bRemote.Click += new System.EventHandler(this.bRemote_Click);
            // 
            // lWPEForm
            // 
            this.lWPEForm.AutoSizeMode = AntdUI.TAutoSize.Height;
            this.lWPEForm.ColorExtend = "135, #00dbde, #fc00ff";
            this.lWPEForm.Cursor = System.Windows.Forms.Cursors.Default;
            this.lWPEForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWPEForm.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lWPEForm.HandCursor = System.Windows.Forms.Cursors.Default;
            this.lWPEForm.Location = new System.Drawing.Point(60, 23);
            this.lWPEForm.Name = "lWPEForm";
            this.lWPEForm.Size = new System.Drawing.Size(194, 32);
            this.lWPEForm.TabIndex = 0;
            this.lWPEForm.Text = "WPE64";
            this.lWPEForm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aWPEForm
            // 
            this.aWPEForm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.aWPEForm.Cursor = System.Windows.Forms.Cursors.Default;
            this.aWPEForm.HandCursor = System.Windows.Forms.Cursors.Default;
            this.aWPEForm.Image = ((System.Drawing.Image)(resources.GetObject("aWPEForm.Image")));
            this.aWPEForm.ImageFit = AntdUI.TFit.Fill;
            this.aWPEForm.Location = new System.Drawing.Point(102, 81);
            this.aWPEForm.Name = "aWPEForm";
            this.aWPEForm.Size = new System.Drawing.Size(110, 110);
            this.aWPEForm.TabIndex = 1;
            this.aWPEForm.Text = "a";
            // 
            // ddlStartMode
            // 
            this.ddlStartMode.BackColor = System.Drawing.Color.Transparent;
            this.ddlStartMode.BorderWidth = 0F;
            this.ddlStartMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlStartMode.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlStartMode.List = true;
            this.ddlStartMode.Location = new System.Drawing.Point(60, 197);
            this.ddlStartMode.Name = "ddlStartMode";
            this.ddlStartMode.SelectionColor = System.Drawing.Color.Transparent;
            this.ddlStartMode.Size = new System.Drawing.Size(194, 44);
            this.ddlStartMode.TabIndex = 2;
            this.ddlStartMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ddlStartMode.Variant = AntdUI.TVariant.Underlined;
            this.ddlStartMode.WaveSize = 0;
            // 
            // bLogin
            // 
            this.bLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bLogin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bLogin.LocalizationText = "WPEForm.Login";
            this.bLogin.Location = new System.Drawing.Point(60, 262);
            this.bLogin.Name = "bLogin";
            this.bLogin.Radius = 8;
            this.bLogin.Size = new System.Drawing.Size(194, 44);
            this.bLogin.TabIndex = 3;
            this.bLogin.Text = "登录";
            this.bLogin.Type = AntdUI.TTypeMini.Primary;
            this.bLogin.Click += new System.EventHandler(this.bLogin_Click);
            // 
            // tpRemote
            // 
            this.tpRemote.Controls.Add(this.tlpRemote);
            this.tpRemote.Location = new System.Drawing.Point(3, 33);
            this.tpRemote.Name = "tpRemote";
            this.tpRemote.Size = new System.Drawing.Size(314, 384);
            this.tpRemote.TabIndex = 1;
            this.tpRemote.Text = "tpRemote";
            // 
            // tlpRemote
            // 
            this.tlpRemote.ColumnCount = 3;
            this.tlpRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tlpRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRemote.Controls.Add(this.aRemote, 1, 0);
            this.tlpRemote.Controls.Add(this.txtRemote_PassWord, 1, 2);
            this.tlpRemote.Controls.Add(this.txtRemote_UserName, 1, 1);
            this.tlpRemote.Controls.Add(this.nudRemote_Port, 1, 3);
            this.tlpRemote.Controls.Add(this.bSaveRemote, 1, 5);
            this.tlpRemote.Controls.Add(this.lRemote, 1, 6);
            this.tlpRemote.Controls.Add(this.cbIsRemote, 1, 4);
            this.tlpRemote.Cursor = System.Windows.Forms.Cursors.Default;
            this.tlpRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemote.Location = new System.Drawing.Point(0, 0);
            this.tlpRemote.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRemote.Name = "tlpRemote";
            this.tlpRemote.RowCount = 7;
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemote.Size = new System.Drawing.Size(314, 384);
            this.tlpRemote.TabIndex = 0;
            // 
            // aRemote
            // 
            this.aRemote.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.aRemote.Image = ((System.Drawing.Image)(resources.GetObject("aRemote.Image")));
            this.aRemote.ImageFit = AntdUI.TFit.Fill;
            this.aRemote.Location = new System.Drawing.Point(117, 3);
            this.aRemote.Name = "aRemote";
            this.aRemote.Size = new System.Drawing.Size(80, 80);
            this.aRemote.TabIndex = 4;
            this.aRemote.Text = "a";
            // 
            // txtRemote_PassWord
            // 
            this.txtRemote_PassWord.AllowClear = true;
            this.txtRemote_PassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote_PassWord.Location = new System.Drawing.Point(35, 140);
            this.txtRemote_PassWord.Name = "txtRemote_PassWord";
            this.txtRemote_PassWord.PlaceholderText = "请输入密码";
            this.txtRemote_PassWord.Size = new System.Drawing.Size(244, 45);
            this.txtRemote_PassWord.TabIndex = 1;
            this.txtRemote_PassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRemote_PassWord.UseSystemPasswordChar = true;
            this.txtRemote_PassWord.TextChanged += new System.EventHandler(this.txtRemote_PassWord_TextChanged);
            // 
            // txtRemote_UserName
            // 
            this.txtRemote_UserName.AllowClear = true;
            this.txtRemote_UserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemote_UserName.Location = new System.Drawing.Point(35, 89);
            this.txtRemote_UserName.Name = "txtRemote_UserName";
            this.txtRemote_UserName.PlaceholderText = "请输入管理员账号";
            this.txtRemote_UserName.Size = new System.Drawing.Size(244, 45);
            this.txtRemote_UserName.TabIndex = 0;
            this.txtRemote_UserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRemote_UserName.TextChanged += new System.EventHandler(this.txtRemote_UserName_TextChanged);
            // 
            // nudRemote_Port
            // 
            this.nudRemote_Port.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudRemote_Port.Location = new System.Drawing.Point(35, 191);
            this.nudRemote_Port.Name = "nudRemote_Port";
            this.nudRemote_Port.PlaceholderText = "";
            this.nudRemote_Port.PrefixText = "";
            this.nudRemote_Port.SelectionStart = 2;
            this.nudRemote_Port.Size = new System.Drawing.Size(244, 45);
            this.nudRemote_Port.TabIndex = 2;
            this.nudRemote_Port.Text = "88";
            this.nudRemote_Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRemote_Port.Value = new decimal(new int[] {
            88,
            0,
            0,
            0});
            this.nudRemote_Port.ValueChanged += new AntdUI.DecimalEventHandler(this.nudRemote_Port_ValueChanged);
            // 
            // bSaveRemote
            // 
            this.bSaveRemote.AutoSizeMode = AntdUI.TAutoSize.Height;
            this.bSaveRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSaveRemote.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bSaveRemote.Location = new System.Drawing.Point(35, 287);
            this.bSaveRemote.Name = "bSaveRemote";
            this.bSaveRemote.Radius = 8;
            this.bSaveRemote.Size = new System.Drawing.Size(244, 43);
            this.bSaveRemote.TabIndex = 3;
            this.bSaveRemote.Text = "确定";
            this.bSaveRemote.Type = AntdUI.TTypeMini.Primary;
            this.bSaveRemote.Click += new System.EventHandler(this.bSaveRemote_Click);
            // 
            // lRemote
            // 
            this.lRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRemote.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lRemote.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lRemote.Location = new System.Drawing.Point(35, 336);
            this.lRemote.Name = "lRemote";
            this.lRemote.Size = new System.Drawing.Size(244, 45);
            this.lRemote.TabIndex = 5;
            this.lRemote.Text = "lRemote";
            this.lRemote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbIsRemote
            // 
            this.cbIsRemote.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbIsRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsRemote.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIsRemote.Location = new System.Drawing.Point(35, 242);
            this.cbIsRemote.Name = "cbIsRemote";
            this.cbIsRemote.Size = new System.Drawing.Size(151, 39);
            this.cbIsRemote.TabIndex = 6;
            this.cbIsRemote.Text = "启用远程管理后台";
            this.cbIsRemote.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsRemote_CheckedChanged);
            // 
            // pageHeader
            // 
            this.pageHeader.BackColor = System.Drawing.Color.Transparent;
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.Controls.Add(this.btn_global);
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.MaximizeBox = false;
            this.pageHeader.MinimizeBox = false;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.Size = new System.Drawing.Size(320, 30);
            this.pageHeader.SubText = "";
            this.pageHeader.TabIndex = 4;
            this.pageHeader.Text = "WPE x64";
            // 
            // btn_global
            // 
            this.btn_global.BackActive = System.Drawing.Color.Transparent;
            this.btn_global.BackHover = System.Drawing.Color.Transparent;
            this.btn_global.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_global.DropDownRadius = 6;
            this.btn_global.Ghost = true;
            this.btn_global.IconSvg = "GlobalOutlined";
            this.btn_global.Location = new System.Drawing.Point(250, 0);
            this.btn_global.Name = "btn_global";
            this.btn_global.Placement = AntdUI.TAlignFrom.BR;
            this.btn_global.Radius = 0;
            this.btn_global.Size = new System.Drawing.Size(30, 30);
            this.btn_global.TabIndex = 11;
            this.btn_global.WaveSize = 0;
            this.btn_global.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.btn_global_SelectedValueChanged);
            // 
            // WPEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderWidth = 0;
            this.ClientSize = new System.Drawing.Size(320, 450);
            this.Controls.Add(this.pWPEForm);
            this.EnableHitTest = false;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "WPEForm";
            this.Resizable = false;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WPEForm";
            this.UseDwm = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WPEForm_FormClosing);
            this.Load += new System.EventHandler(this.WPEForm_Load);
            this.pWPEForm.ResumeLayout(false);
            this.tabWPEForm.ResumeLayout(false);
            this.tpLogin.ResumeLayout(false);
            this.tlpWPEForm.ResumeLayout(false);
            this.tlpWPEForm.PerformLayout();
            this.tpRemote.ResumeLayout(false);
            this.tlpRemote.ResumeLayout(false);
            this.tlpRemote.PerformLayout();
            this.pageHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel pWPEForm;
        private AntdUI.PageHeader pageHeader;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Tabs tabWPEForm;
        private AntdUI.TabPage tpLogin;
        private System.Windows.Forms.TableLayoutPanel tlpWPEForm;
        private AntdUI.Label lWPEForm;
        private AntdUI.Avatar aWPEForm;
        private AntdUI.Select ddlStartMode;
        private AntdUI.Button bLogin;
        private AntdUI.TabPage tpRemote;
        private System.Windows.Forms.TableLayoutPanel tlpRemote;
        private AntdUI.Input txtRemote_PassWord;
        private AntdUI.Input txtRemote_UserName;
        private AntdUI.Avatar aRemote;
        private AntdUI.InputNumber nudRemote_Port;
        private AntdUI.Button bSaveRemote;
        private AntdUI.Label lRemote;
        private AntdUI.Button bRemote;
        private AntdUI.Checkbox cbIsRemote;
    }
}
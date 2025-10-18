namespace WinsockPacketEditor
{
    partial class FireWallSetting
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
            this.tlpFireWallSetting = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tableLayoutPanelEx1 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu_BlackList = new AntdUI.Dropdown();
            this.lBlackList = new AntdUI.Label();
            this.tlpWhiteList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu_WhiteList = new AntdUI.Dropdown();
            this.lWhiteList = new AntdUI.Label();
            this.tBlackList = new AntdUI.Table();
            this.tWhiteList = new AntdUI.Table();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpEnableFireWall = new WinsockPacketEditor.TableLayoutPanelEx();
            this.rbBlackListMode = new AntdUI.Radio();
            this.rbWhiteListMode = new AntdUI.Radio();
            this.cbEnableFireWall = new AntdUI.Checkbox();
            this.bRules = new AntdUI.Button();
            this.tlpFireWallSetting.SuspendLayout();
            this.tableLayoutPanelEx1.SuspendLayout();
            this.tlpWhiteList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpEnableFireWall.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFireWallSetting
            // 
            this.tlpFireWallSetting.ColumnCount = 1;
            this.tlpFireWallSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallSetting.Controls.Add(this.tableLayoutPanelEx1, 0, 3);
            this.tlpFireWallSetting.Controls.Add(this.tlpWhiteList, 0, 1);
            this.tlpFireWallSetting.Controls.Add(this.tBlackList, 0, 4);
            this.tlpFireWallSetting.Controls.Add(this.tWhiteList, 0, 2);
            this.tlpFireWallSetting.Controls.Add(this.tlpButton, 0, 5);
            this.tlpFireWallSetting.Controls.Add(this.tlpEnableFireWall, 0, 0);
            this.tlpFireWallSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFireWallSetting.Location = new System.Drawing.Point(0, 0);
            this.tlpFireWallSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFireWallSetting.Name = "tlpFireWallSetting";
            this.tlpFireWallSetting.RowCount = 6;
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.Size = new System.Drawing.Size(700, 700);
            this.tlpFireWallSetting.TabIndex = 0;
            // 
            // tableLayoutPanelEx1
            // 
            this.tableLayoutPanelEx1.ColumnCount = 3;
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEx1.Controls.Add(this.ddMenu_BlackList, 2, 1);
            this.tableLayoutPanelEx1.Controls.Add(this.lBlackList, 0, 1);
            this.tableLayoutPanelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEx1.Location = new System.Drawing.Point(0, 350);
            this.tableLayoutPanelEx1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelEx1.Name = "tableLayoutPanelEx1";
            this.tableLayoutPanelEx1.RowCount = 3;
            this.tableLayoutPanelEx1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEx1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEx1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEx1.Size = new System.Drawing.Size(700, 50);
            this.tableLayoutPanelEx1.TabIndex = 15;
            // 
            // ddMenu_BlackList
            // 
            this.ddMenu_BlackList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu_BlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu_BlackList.Ghost = true;
            this.ddMenu_BlackList.IconRatio = 1F;
            this.ddMenu_BlackList.IconSvg = "PlusOutlined";
            this.ddMenu_BlackList.Location = new System.Drawing.Point(666, 6);
            this.ddMenu_BlackList.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu_BlackList.MaxCount = 10;
            this.ddMenu_BlackList.Name = "ddMenu_BlackList";
            this.ddMenu_BlackList.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu_BlackList.Size = new System.Drawing.Size(32, 37);
            this.ddMenu_BlackList.TabIndex = 16;
            this.ddMenu_BlackList.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu_BlackList.WaveSize = 0;
            this.ddMenu_BlackList.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_BlackList_SelectedValueChanged);
            // 
            // lBlackList
            // 
            this.lBlackList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBlackList.LocalizationText = "FireWallSetting.BlackList";
            this.lBlackList.Location = new System.Drawing.Point(3, 7);
            this.lBlackList.Name = "lBlackList";
            this.lBlackList.Size = new System.Drawing.Size(60, 35);
            this.lBlackList.TabIndex = 15;
            this.lBlackList.Text = "黑名单列表";
            // 
            // tlpWhiteList
            // 
            this.tlpWhiteList.ColumnCount = 3;
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWhiteList.Controls.Add(this.ddMenu_WhiteList, 2, 1);
            this.tlpWhiteList.Controls.Add(this.lWhiteList, 0, 1);
            this.tlpWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWhiteList.Location = new System.Drawing.Point(0, 50);
            this.tlpWhiteList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWhiteList.Name = "tlpWhiteList";
            this.tlpWhiteList.RowCount = 3;
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteList.Size = new System.Drawing.Size(700, 50);
            this.tlpWhiteList.TabIndex = 13;
            // 
            // ddMenu_WhiteList
            // 
            this.ddMenu_WhiteList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu_WhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu_WhiteList.Ghost = true;
            this.ddMenu_WhiteList.IconRatio = 1F;
            this.ddMenu_WhiteList.IconSvg = "PlusOutlined";
            this.ddMenu_WhiteList.Location = new System.Drawing.Point(666, 6);
            this.ddMenu_WhiteList.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu_WhiteList.MaxCount = 10;
            this.ddMenu_WhiteList.Name = "ddMenu_WhiteList";
            this.ddMenu_WhiteList.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu_WhiteList.Size = new System.Drawing.Size(32, 37);
            this.ddMenu_WhiteList.TabIndex = 15;
            this.ddMenu_WhiteList.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu_WhiteList.WaveSize = 0;
            this.ddMenu_WhiteList.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_WhiteList_SelectedValueChanged);
            // 
            // lWhiteList
            // 
            this.lWhiteList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWhiteList.LocalizationText = "FireWallSetting.WhiteList";
            this.lWhiteList.Location = new System.Drawing.Point(3, 7);
            this.lWhiteList.Name = "lWhiteList";
            this.lWhiteList.Size = new System.Drawing.Size(60, 35);
            this.lWhiteList.TabIndex = 14;
            this.lWhiteList.Text = "白名单列表";
            // 
            // tBlackList
            // 
            this.tBlackList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tBlackList.Bordered = true;
            this.tBlackList.CellImpactHeight = false;
            this.tBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tBlackList.EmptyHeader = true;
            this.tBlackList.Gap = 8;
            this.tBlackList.GapCell = 5;
            this.tBlackList.Gaps = new System.Drawing.Size(8, 8);
            this.tBlackList.Location = new System.Drawing.Point(2, 402);
            this.tBlackList.Margin = new System.Windows.Forms.Padding(2);
            this.tBlackList.Name = "tBlackList";
            this.tBlackList.Size = new System.Drawing.Size(696, 246);
            this.tBlackList.TabIndex = 10;
            this.tBlackList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tBlackList_CellButtonClick);
            this.tBlackList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tBlackList_CellDoubleClick);
            // 
            // tWhiteList
            // 
            this.tWhiteList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tWhiteList.Bordered = true;
            this.tWhiteList.CellImpactHeight = false;
            this.tWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tWhiteList.EmptyHeader = true;
            this.tWhiteList.Gap = 8;
            this.tWhiteList.GapCell = 5;
            this.tWhiteList.Gaps = new System.Drawing.Size(8, 8);
            this.tWhiteList.Location = new System.Drawing.Point(2, 102);
            this.tWhiteList.Margin = new System.Windows.Forms.Padding(2);
            this.tWhiteList.Name = "tWhiteList";
            this.tWhiteList.Size = new System.Drawing.Size(696, 246);
            this.tWhiteList.TabIndex = 9;
            this.tWhiteList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tWhiteList_CellButtonClick);
            this.tWhiteList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tWhiteList_CellDoubleClick);
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
            this.tlpButton.Location = new System.Drawing.Point(0, 650);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(700, 50);
            this.tlpButton.TabIndex = 4;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(278, 6);
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
            this.bExit.Location = new System.Drawing.Point(359, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpEnableFireWall
            // 
            this.tlpEnableFireWall.ColumnCount = 5;
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEnableFireWall.Controls.Add(this.rbBlackListMode, 2, 1);
            this.tlpEnableFireWall.Controls.Add(this.rbWhiteListMode, 1, 1);
            this.tlpEnableFireWall.Controls.Add(this.cbEnableFireWall, 0, 1);
            this.tlpEnableFireWall.Controls.Add(this.bRules, 3, 1);
            this.tlpEnableFireWall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEnableFireWall.Location = new System.Drawing.Point(0, 0);
            this.tlpEnableFireWall.Margin = new System.Windows.Forms.Padding(0);
            this.tlpEnableFireWall.Name = "tlpEnableFireWall";
            this.tlpEnableFireWall.RowCount = 3;
            this.tlpEnableFireWall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpEnableFireWall.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpEnableFireWall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpEnableFireWall.Size = new System.Drawing.Size(700, 50);
            this.tlpEnableFireWall.TabIndex = 16;
            // 
            // rbBlackListMode
            // 
            this.rbBlackListMode.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbBlackListMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbBlackListMode.LocalizationText = "FireWallSetting.BlackListMode";
            this.rbBlackListMode.Location = new System.Drawing.Point(199, 7);
            this.rbBlackListMode.Name = "rbBlackListMode";
            this.rbBlackListMode.Size = new System.Drawing.Size(92, 36);
            this.rbBlackListMode.TabIndex = 17;
            this.rbBlackListMode.Text = "黑名单模式";
            // 
            // rbWhiteListMode
            // 
            this.rbWhiteListMode.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbWhiteListMode.Checked = true;
            this.rbWhiteListMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbWhiteListMode.LocalizationText = "FireWallSetting.WhiteListMode";
            this.rbWhiteListMode.Location = new System.Drawing.Point(101, 7);
            this.rbWhiteListMode.Name = "rbWhiteListMode";
            this.rbWhiteListMode.Size = new System.Drawing.Size(92, 36);
            this.rbWhiteListMode.TabIndex = 16;
            this.rbWhiteListMode.Text = "白名单模式";
            // 
            // cbEnableFireWall
            // 
            this.cbEnableFireWall.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnableFireWall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnableFireWall.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbEnableFireWall.LocalizationText = "FireWallSetting.EnableFireWall";
            this.cbEnableFireWall.Location = new System.Drawing.Point(3, 7);
            this.cbEnableFireWall.Name = "cbEnableFireWall";
            this.cbEnableFireWall.Size = new System.Drawing.Size(92, 36);
            this.cbEnableFireWall.TabIndex = 15;
            this.cbEnableFireWall.Text = "启用防火墙";
            this.cbEnableFireWall.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnableFireWall_CheckedChanged);
            // 
            // bRules
            // 
            this.bRules.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRules.LocalizationText = "FireWallSetting.Rules";
            this.bRules.Location = new System.Drawing.Point(297, 7);
            this.bRules.Name = "bRules";
            this.bRules.Size = new System.Drawing.Size(84, 36);
            this.bRules.TabIndex = 18;
            this.bRules.Text = "防火墙规则";
            this.bRules.Type = AntdUI.TTypeMini.Warn;
            this.bRules.Click += new System.EventHandler(this.bRules_Click);
            // 
            // FireWallSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFireWallSetting);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FireWallSetting";
            this.Size = new System.Drawing.Size(700, 700);
            this.Load += new System.EventHandler(this.FireWallSetting_Load);
            this.tlpFireWallSetting.ResumeLayout(false);
            this.tableLayoutPanelEx1.ResumeLayout(false);
            this.tableLayoutPanelEx1.PerformLayout();
            this.tlpWhiteList.ResumeLayout(false);
            this.tlpWhiteList.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpEnableFireWall.ResumeLayout(false);
            this.tlpEnableFireWall.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFireWallSetting;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Table tBlackList;
        private AntdUI.Table tWhiteList;
        private TableLayoutPanelEx tlpWhiteList;
        private TableLayoutPanelEx tableLayoutPanelEx1;
        private TableLayoutPanelEx tlpEnableFireWall;
        private AntdUI.Radio rbBlackListMode;
        private AntdUI.Radio rbWhiteListMode;
        private AntdUI.Checkbox cbEnableFireWall;
        private AntdUI.Label lWhiteList;
        private AntdUI.Label lBlackList;
        private AntdUI.Dropdown ddMenu_WhiteList;
        private AntdUI.Dropdown ddMenu_BlackList;
        private AntdUI.Button bRules;
    }
}
